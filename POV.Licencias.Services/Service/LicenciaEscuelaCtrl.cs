using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using POV.CentroEducativo.BO;
using POV.Licencias.BO;
using POV.Licencias.DA;
using POV.Licencias.DAO;
using POV.Seguridad.BO;
using POV.Prueba.BO;
using POV.Modelo.BO;
using Framework.Base.Exceptions;
using POV.Modelo.Service;
using POV.CentroEducativo.Service;
using POV.Seguridad.Service;
using POV.Expediente.BO;

namespace POV.Licencias.Service
{
    /// <summary>
    /// Servicios para las Licencias de Escuela
    /// </summary>
    public class LicenciaEscuelaCtrl
    {
        private string nameSpace = "POV.Licencias.Service";
        private string className = "LicenciaEscuelaCtrl";

        /// <summary>
        /// Crear registros de LicenciaEscuela en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaEscuela">LicenciaEscuela que provee el criterio de selección para realizar la consulta</param>
        public void Insert(IDataContext dctx, LicenciaEscuela licenciaEscuela)
        {
            #region Iniciar transaccion en el sistema
            object firma = new object();
            try
            {
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);
            }
            catch (Exception)
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);

                throw new Exception("Inconsistencias al conectarse a la base de datos.");
            }

            try
            {
            #endregion

                LicenciaEscuelaInsHlp da = new LicenciaEscuelaInsHlp();
                da.Action(dctx, licenciaEscuela);
                LicenciaEscuela escuela = this.LastDataRowToLicenciaEscuela(new LicenciaEscuelaRetHlp().Action(dctx, licenciaEscuela));

                foreach (ALicencia licencia in licenciaEscuela.ListaLicencia)
                {
                    this.InsertLicenciaDirector(dctx, licencia, escuela);
                }

                if (licenciaEscuela.ModulosFuncionales != null)
                    licenciaEscuela.ModulosFuncionales.ForEach(mod => da.Action(dctx, escuela, mod));

                #region Finalizar transaccion en el sistema
                dctx.CommitTransaction(firma);
            }
            catch (Exception)
            {
                dctx.RollbackTransaction(firma);
                dctx.CloseConnection(firma);

                throw;
            }
            finally
            {
                dctx.CloseConnection(firma);
            }
                #endregion
        }

        public void InsertLicenciaDirector(IDataContext dctx, LicenciaEscuela licenciaEscuela, Director docente, Usuario usuario)
        {
            LicenciaDirector licencia = new LicenciaDirector();
            licencia.Director = docente;
            licencia.Usuario = usuario;

            LicenciaDirectorRetHlp licDir = new LicenciaDirectorRetHlp();
            DataSet ds = licDir.Action(dctx, licencia, licenciaEscuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
                licencia = (LicenciaDirector)this.CreateLicenciaAlumno(ds.Tables[0].Rows[total - 1]);

            if (licencia.LicenciaID != null)
            {
                if (licencia.Activo.Value)
                    throw new InvalidOperationException("Licencia de director ya se encuentra registrada.");

                LicenciaDirector anterior = (LicenciaDirector)licencia.Clone();
                licencia.Activo = true;
                new LicenciaDirectorUpdHlp().Action(dctx, licencia, anterior, licenciaEscuela);

                return;
            }
            else
            {
                licencia.LicenciaID = Guid.NewGuid();
                licencia.Activo = true;
                this.InsertLicenciaDirector(dctx, licencia, licenciaEscuela);
            }
        }

        public void InsertLicenciaDocente(IDataContext dctx, LicenciaEscuela licenciaEscuela, Docente docente, Usuario usuario, UsuarioSocial usuarioSocial)
        {
            LicenciaDocente licencia = new LicenciaDocente();
            licencia.Docente = docente;
            licencia.Usuario = usuario;

            LicenciaDocenteRetHlp licDoc = new LicenciaDocenteRetHlp();
            DataSet ds = licDoc.Action(dctx, licencia, licenciaEscuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
                licencia = (LicenciaDocente)this.CreateLicenciaAlumno(ds.Tables[0].Rows[total - 1]);

            if (licencia.LicenciaID != null)
            {
                //if (licencia.Activo.Value)
                //    throw new InvalidOperationException("Licencia de docente ya se encuentra registrada.");

                LicenciaDocente anterior = (LicenciaDocente)licencia.Clone();
                licencia.Activo = true;
                new LicenciaDocenteUpdHlp().Action(dctx, licencia, anterior, licenciaEscuela);
            }
            else
            {
                licencia.LicenciaID = Guid.NewGuid();
                licencia.Activo = true;
                licencia.UsuarioSocial = usuarioSocial;
                this.InsertLicenciaDirector(dctx, licencia, licenciaEscuela);
            }
        }

        public void InsertLicenciaTutor(IDataContext dctx, LicenciaEscuela licenciaEscuela, Tutor tutor, Usuario usuario)
        {
            LicenciaTutor licencia = new LicenciaTutor();
            licencia.Tutor = tutor;
            licencia.Usuario = usuario;

            LicenciaTutorRetHlp licTut = new LicenciaTutorRetHlp();
            DataSet ds = licTut.Action(dctx, licencia, licenciaEscuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
                licencia = (LicenciaTutor)this.CreateLicenciaAlumno(ds.Tables[0].Rows[total - 1]);

            if (licencia.LicenciaID != null)
            {
                if (licencia.Activo.Value)
                    throw new InvalidOperationException("Licencia de tutor ya se encuentra registrada.");

                LicenciaTutor anterior = (LicenciaTutor)licencia.Clone();
                licencia.Activo = true;
                new LicenciaTutorUpdHlp().Action(dctx, licencia, anterior, licenciaEscuela);
            }
            else
            {
                licencia.LicenciaID = Guid.NewGuid();
                licencia.Activo = true;
                this.InsertLicenciaTutor(dctx, licencia, licenciaEscuela);
            }
        }

        public void InsertLicenciaUniversidad(IDataContext dctx, LicenciaEscuela licenciaEscuela, Universidad universidad, Usuario usuario)
        {
            LicenciaUniversidad licencia = new LicenciaUniversidad();
            licencia.Universidad = universidad;
            licencia.Usuario = usuario;

            LicenciaUniversidadRetHlp licUni = new LicenciaUniversidadRetHlp();
            DataSet ds = licUni.Action(dctx, licencia, licenciaEscuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
                licencia = (LicenciaUniversidad)this.CreateLicenciaUniversidad(ds.Tables[0].Rows[total - 1]);

            if (licencia.LicenciaID != null)
            {
                if (licencia.Activo.Value)
                    throw new InvalidOperationException("Licencia de universidad ya se encuentra registrada.");

                LicenciaUniversidad anterior = (LicenciaUniversidad)licencia.Clone();
                licencia.Activo = true;
                new LicenciaUniversidadUpdHlp().Action(dctx, licencia, anterior, licenciaEscuela);
            }
            else
            {
                licencia.LicenciaID = Guid.NewGuid();
                licencia.Activo = true;
                this.InsertLicenciaUniversidad(dctx, licencia, licenciaEscuela);
            }
        }

        public void InsertLicenciaEspecialista(IDataContext dctx, LicenciaEscuela licenciaEscuela, EspecialistaPruebas especialista, Usuario usuario)
        {
            LicenciaEspecialistaPruebas licencia = new LicenciaEspecialistaPruebas();
            licencia.EspecialistaPruebas = especialista;
            licencia.Usuario = usuario;

            LicenciaEspecialistaRetHlp licDoc = new LicenciaEspecialistaRetHlp();
            DataSet ds = licDoc.Action(dctx, licencia, licenciaEscuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
                licencia = (LicenciaEspecialistaPruebas)this.CreateLicenciaAlumno(ds.Tables[0].Rows[total - 1]);

            if (licencia.LicenciaID != null)
            {
                if (licencia.Activo.Value)
                    throw new InvalidOperationException("Licencia de especialista ya se encuentra registrada.");

                LicenciaEspecialistaPruebas anterior = (LicenciaEspecialistaPruebas)licencia.Clone();
                licencia.Activo = true;
                new LicenciaEspecialistaUpdHlp().Action(dctx, licencia, anterior, licenciaEscuela);
            }
            else
            {
                licencia.LicenciaID = Guid.NewGuid();
                licencia.Activo = true;
                this.InsertLicenciaDirector(dctx, licencia, licenciaEscuela);
            }
        }
        public void DeleteLicenciasDocente(IDataContext dctx, LicenciaEscuela licenciaEscuela, Docente docente, Usuario usuario)
        {
            LicenciaDocente licencia = new LicenciaDocente();
            licencia.Docente = docente;
            licencia.Usuario = usuario;

            LicenciaDocenteRetHlp licDoc = new LicenciaDocenteRetHlp();
            DataSet ds = licDoc.Action(dctx, licencia, licenciaEscuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
            {
                object firma = new object();
                try
                {
                    dctx.OpenConnection(firma);
                    dctx.BeginTransaction(firma);
                    foreach (DataRow rwlicencia in ds.Tables[0].Rows)
                    {
                        licencia = (LicenciaDocente)this.CreateLicenciaAlumno(ds.Tables[0].Rows[total - 1]);

                        if (licencia.LicenciaID != null)
                        {
                            if (licencia.Activo.Value)
                            {
                                LicenciaDocente anterior = (LicenciaDocente)licencia.Clone();
                                licencia.Activo = false;
                                new LicenciaDocenteUpdHlp().Action(dctx, licencia, anterior, licenciaEscuela);
                            }
                        }
                    }
                    dctx.CommitTransaction(firma);
                }
                catch (Exception ex)
                {
                    dctx.RollbackTransaction(firma);
                    if (dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(firma);
                    throw;
                }
                finally
                {
                    if (dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(firma);
                }
            }


        }

        public void DeleteLicenciasUniversidad(IDataContext dctx, LicenciaEscuela licenciaEscuela, Universidad universidad, Usuario usuario)
        {
            LicenciaUniversidad licencia = new LicenciaUniversidad();
            licencia.Universidad = universidad;
            licencia.Usuario = usuario;

            LicenciaUniversidadRetHlp licUni = new LicenciaUniversidadRetHlp();
            DataSet ds = licUni.Action(dctx, licencia, licenciaEscuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
            {
                object firma = new object();
                try
                {
                    dctx.OpenConnection(firma);
                    dctx.BeginTransaction(firma);
                    foreach (DataRow rwlicencia in ds.Tables[0].Rows)
                    {
                        licencia = (LicenciaUniversidad)this.CreateLicenciaUniversidad(ds.Tables[0].Rows[total - 1]);

                        if (licencia.LicenciaID != null)
                        {
                            if (licencia.Activo.Value)
                            {
                                LicenciaUniversidad anterior = (LicenciaUniversidad)licencia.Clone();
                                licencia.Activo = false;
                                new LicenciaUniversidadUpdHlp().Action(dctx, licencia, anterior, licenciaEscuela);
                            }
                        }
                    }
                    dctx.CommitTransaction(firma);
                }
                catch (Exception ex)
                {
                    dctx.RollbackTransaction(firma);
                    if (dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(firma);
                    throw;
                }
                finally
                {
                    if (dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(firma);
                }
            }


        }

        public void UpdateLicenciasUniversidad(IDataContext dctx, LicenciaEscuela licenciaEscuela, Universidad universidad, Usuario usuario)
        {
            LicenciaUniversidad licencia = new LicenciaUniversidad();
            licencia.Universidad = universidad;
            licencia.Usuario = usuario;

            LicenciaUniversidadRetHlp licUni = new LicenciaUniversidadRetHlp();
            DataSet ds = licUni.Action(dctx, licencia, licenciaEscuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
            {
                object firma = new object();
                try
                {
                    dctx.OpenConnection(firma);
                    dctx.BeginTransaction(firma);
                    foreach (DataRow rwlicencia in ds.Tables[0].Rows)
                    {
                        licencia = (LicenciaUniversidad)this.CreateLicenciaUniversidad(ds.Tables[0].Rows[total - 1]);

                        if (licencia.LicenciaID != null)
                        {                           
                                LicenciaUniversidad anterior = (LicenciaUniversidad)licencia.Clone();
                                licencia.Activo = true;
                                new LicenciaUniversidadUpdHlp().Action(dctx, licencia, anterior, licenciaEscuela);
                            
                        }
                    }
                    dctx.CommitTransaction(firma);
                }
                catch (Exception ex)
                {
                    dctx.RollbackTransaction(firma);
                    if (dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(firma);
                    throw;
                }
                finally
                {
                    if (dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(firma);
                }
            }


        }

        public void DeleteLicenciasEspecialista(IDataContext dctx, LicenciaEscuela licenciaEscuela, EspecialistaPruebas especialista, Usuario usuario)
        {
            LicenciaEspecialistaPruebas licencia = new LicenciaEspecialistaPruebas();
            licencia.EspecialistaPruebas = especialista;
            licencia.Usuario = usuario;

            LicenciaEspecialistaRetHlp licDoc = new LicenciaEspecialistaRetHlp();
            DataSet ds = licDoc.Action(dctx, licencia, licenciaEscuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
            {
                object firma = new object();
                try
                {
                    dctx.OpenConnection(firma);
                    dctx.BeginTransaction(firma);
                    foreach (DataRow rwlicencia in ds.Tables[0].Rows)
                    {
                        licencia = (LicenciaEspecialistaPruebas)this.CreateLicenciaAlumno(ds.Tables[0].Rows[total - 1]);

                        if (licencia.LicenciaID != null)
                        {
                            if (licencia.Activo.Value)
                            {
                                LicenciaEspecialistaPruebas anterior = (LicenciaEspecialistaPruebas)licencia.Clone();
                                licencia.Activo = false;
                                new LicenciaEspecialistaUpdHlp().Action(dctx, licencia, anterior, licenciaEscuela);
                            }
                        }
                    }
                    dctx.CommitTransaction(firma);
                }
                catch (Exception ex)
                {
                    dctx.RollbackTransaction(firma);
                    if (dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(firma);
                    throw;
                }
                finally
                {
                    if (dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(firma);
                }
            }


        }


        /// <summary>
        /// Verifica si existe una licencia docente para la licencia escuela proporcionada
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="licenciaEscuela"></param>
        /// <param name="docente"></param>
        /// <param name="usuario"></param>
        /// <returns>True en caso de existir una licencia docente activa, false en caso contrario</returns>
        public LicenciaDocente RetrieveLicenciaDocente(IDataContext dctx, LicenciaEscuela licenciaEscuela, Docente docente, Usuario usuario)
        {
            LicenciaDocente licencia = new LicenciaDocente();

            LicenciaDocenteRetHlp licDoc = new LicenciaDocenteRetHlp();
            DataSet ds = licDoc.Action(dctx, new LicenciaDocente { Docente = docente, Usuario = usuario }, licenciaEscuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
                licencia = (LicenciaDocente)this.CreateLicenciaAlumno(ds.Tables[0].Rows[total - 1]);

            return licencia;
        }

        public LicenciaTutor RetrieveLicenciaTutor(IDataContext dctx, LicenciaEscuela licenciaEscuela, Tutor tutor, Usuario usuario)
        {
            LicenciaTutor licencia = new LicenciaTutor();
            LicenciaTutorRetHlp licAlum = new LicenciaTutorRetHlp();

            LicenciaTutor licenciaTutor = new LicenciaTutor();
            licenciaTutor.Tutor = tutor;
            licenciaTutor.Usuario = usuario;

            DataSet ds = licAlum.Action(dctx, licenciaTutor, licenciaEscuela);
            int index = ds.Tables[0].Rows.Count;
            if (index > 0)
                licencia = (LicenciaTutor)this.CreateLicenciaTutor(ds.Tables[0].Rows[index - 1]);
            return licencia;
        }

        public LicenciaUniversidad RetrieveLicenciaUniversidad(IDataContext dctx, LicenciaEscuela licenciaEscuela, Universidad universidad, Usuario usuario)
        {
            LicenciaUniversidad licencia = new LicenciaUniversidad();
            LicenciaUniversidadRetHlp licAlum = new LicenciaUniversidadRetHlp();

            LicenciaUniversidad licenciaUniversidad = new LicenciaUniversidad();
            licenciaUniversidad.Universidad = universidad;
            licenciaUniversidad.Usuario = usuario;

            DataSet ds = licAlum.Action(dctx, licenciaUniversidad, licenciaEscuela);
            int index = ds.Tables[0].Rows.Count;
            if (index > 0)
                licencia = (LicenciaUniversidad)this.CreateLicenciaUniversidad(ds.Tables[0].Rows[index - 1]);
            return licencia;
        }

        public LicenciaAlumno RetrieveLicenciaAlumno(IDataContext dctx, LicenciaEscuela licenciaEscuela, Alumno alumno, Usuario usuario)
        {
            LicenciaAlumno licencia = new LicenciaAlumno();
            LicenciaAlumnoRetHlp licAlum = new LicenciaAlumnoRetHlp();

            LicenciaAlumno licenciaAlumno = new LicenciaAlumno();
            licenciaAlumno.Alumno = alumno;
            licenciaAlumno.Usuario = usuario;
            licenciaAlumno.UsuarioSocial = new UsuarioSocial();

            DataSet ds = licAlum.Action(dctx, licenciaAlumno, licenciaEscuela);
            int index = ds.Tables[0].Rows.Count;
            if (index > 0)
                licencia = (LicenciaAlumno)this.CreateLicenciaAlumno(ds.Tables[0].Rows[index - 1]);
            return licencia;
        }

        public LicenciaEspecialistaPruebas RetrieveLicenciaEspecialista(IDataContext dctx,LicenciaEscuela licenciaEscuela, EspecialistaPruebas especialista, Usuario usuario)
        {
            LicenciaEspecialistaPruebas licencia = new LicenciaEspecialistaPruebas();

            LicenciaEspecialistaRetHlp licDoc = new LicenciaEspecialistaRetHlp();
            DataSet ds = licDoc.Action(dctx, new LicenciaEspecialistaPruebas { EspecialistaPruebas = especialista, Usuario = usuario }, licenciaEscuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
                licencia = (LicenciaEspecialistaPruebas)this.CreateLicenciaAlumno(ds.Tables[0].Rows[total - 1]);

            return licencia;
        }

        public void InsertLicenciaAlumno(IDataContext dctx, LicenciaEscuela licenciaEscuela, Alumno alumno, Usuario usuario, UsuarioSocial usuarioSocial)
        {
            LicenciaEscuela actual = this.RetriveLicenciaEscuela(dctx, licenciaEscuela)[0];
            ContratoCtrl contratoCtrl = new ContratoCtrl();

            int disponibles;
            if (!contratoCtrl.TieneLicenciasDisponibles(dctx, actual.Contrato, out disponibles))
                throw new InvalidOperationException("Todas las licencias han sido asignadas.");
            //if (actual.LicenciasPorAsignar() == 0)

            //throw new InvalidOperationException("Todas las licencias han sido asignadas.");

            LicenciaAlumno licencia = new LicenciaAlumno();
            licencia.Alumno = alumno;
            licencia.UsuarioSocial = usuarioSocial;
            licencia.Usuario = usuario;

            LicenciaAlumnoRetHlp licAlu = new LicenciaAlumnoRetHlp();
            DataSet ds = licAlu.Action(dctx, licencia, licenciaEscuela);
            int total = ds.Tables[0].Rows.Count;

            if (total > 0)
                licencia = (LicenciaAlumno)this.CreateLicenciaAlumno(ds.Tables[0].Rows[total - 1]);

            if (licencia.LicenciaID != null)
            {
                //if (licencia.Activo.Value)
                //    throw new InvalidOperationException("Licencia de alumno ya se encuentra registrada.");

                LicenciaAlumno anterior = (LicenciaAlumno)licencia.Clone();
                licencia.Activo = true;
                new LicenciaAlumnoUpdHlp().Action(dctx, licencia, anterior, licenciaEscuela);
            }
            else
            {
                licencia.LicenciaID = Guid.NewGuid();
                licencia.Activo = true;
                this.InsertLicenciaDirector(dctx, licencia, licenciaEscuela);
                ds = licAlu.Action(dctx, licencia, licenciaEscuela);
                total = ds.Tables[0].Rows.Count;
                licencia = (LicenciaAlumno)this.CreateLicenciaAlumno(ds.Tables[0].Rows[total - 1]);
            }

            new LicenciaAlumnoDesactivarHlp().Action(dctx, licencia);
        }

        public void DeleteLicenciaAlumno(IDataContext dctx, LicenciaEscuela licenciaEscuela, LicenciaAlumno licenciaAlumno)
        {
            if (licenciaEscuela == null || licenciaEscuela.LicenciaEscuelaID == null)
                throw new ArgumentException("DeleteLicenciaAlumno:LicenciaEscuela es requerido", "licenciaEscuela");
            if (licenciaAlumno == null || licenciaAlumno.LicenciaID == null)
                throw new ArgumentException("DeleteLicenciaAlumno:LicenciaAlumno es requerido", "licenciaAlumno");

            LicenciaEscuela actual = this.RetriveLicenciaEscuela(dctx, licenciaEscuela)[0];

            LicenciaAlumnoRetHlp retHlp = new LicenciaAlumnoRetHlp();
            DataSet ds = retHlp.Action(dctx, licenciaAlumno, licenciaEscuela);
            int index = ds.Tables[0].Rows.Count;
            if (index == 1)
            {
                DataRow dr = ds.Tables["LicenciaAlumno"].Rows[index - 1];
                if (dr.Field<byte>("Tipo") == (byte)ETipoLicencia.ALUMNO)
                {
                    LicenciaAlumno licalumno = (LicenciaAlumno)CreateLicenciaAlumno(dr);
                    if (licalumno.Activo != null && (bool)licalumno.Activo)
                    {
                        LicenciaAlumno lianterior = (LicenciaAlumno)licalumno.Clone();
                        licalumno.Activo = false;
                        LicenciaAlumnoUpdHlp updHlp = new LicenciaAlumnoUpdHlp();
                        updHlp.Action(dctx, licalumno, lianterior, actual);
                    }

                }
            }


        }

        public void Update(IDataContext dctx, LicenciaEscuela licenciaEscuela, LicenciaEscuela anterior)
        {
            #region Iniciar transaccion en el sistema
            object firma = new object();
            try
            {
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);
            }
            catch (Exception)
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);

                throw new Exception("Inconsistencias al conectarse a la base de datos.");
            }

            try
            {
            #endregion

                LicenciaEscuelaUpdHlp da = new LicenciaEscuelaUpdHlp();
                da.Action(dctx, licenciaEscuela, anterior);
                LicenciaEscuela escuela = this.LastDataRowToLicenciaEscuela(new LicenciaEscuelaRetHlp().Action(dctx, licenciaEscuela));

                foreach (ALicencia licencia in licenciaEscuela.ListaLicencia)
                    this.InsertLicenciaDirector(dctx, licencia, escuela);
                //actualizamos los modulos funcionales
                da.Action(dctx, licenciaEscuela);

                LicenciaEscuelaInsHlp daInsHlp = new LicenciaEscuelaInsHlp();
                if (licenciaEscuela.ModulosFuncionales != null)
                    licenciaEscuela.ModulosFuncionales.ForEach(mod => daInsHlp.Action(dctx, escuela, mod));

                #region Finalizar transaccion en el sistema
                dctx.CommitTransaction(firma);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firma);
                dctx.CloseConnection(firma);

                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
                #endregion
        }

        /// <summary>
        /// Actualiza el director de una licencia escuela, creando la su nueva licencia director y desactivando la
        /// licencia del antiguo director asociado.
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="licenciaEscuela">Licencia escuela donde cambio el director</param>
        /// <param name="nuevoDirector"> Nuevo director de la escuela</param>
        public void UpdateDirectorLicenciaEscuela(IDataContext dctx, LicenciaEscuela licenciaEscuela, Director nuevoDirector, Usuario usuarioDirector)
        {
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                List<LicenciaEscuela> licencias = RetriveLicenciaEscuela(dctx, licenciaEscuela);

                if (licencias.Count > 0)
                {
                    LicenciaEscuela licenciaEsc = licencias[0];

                    ALicencia licencia = licenciaEsc.ListaLicencia.FirstOrDefault(item => item.Tipo == ETipoLicencia.DIRECTOR);
                    if (licencia != null)
                    {
                        #region baja de la anterior licencia director
                        LicenciaDirector licenciaDirector = (LicenciaDirector)licencia;
                        LicenciaDirector anterior = (LicenciaDirector)licenciaDirector.Clone();
                        anterior.Usuario = licenciaDirector.Usuario;
                        anterior.Director = licenciaDirector.Director;
                        licenciaDirector.Activo = false;

                        new LicenciaDirectorUpdHlp().Action(dctx, licenciaDirector, anterior, licenciaEsc);
                        #endregion

                        InsertLicenciaDirector(dctx, licenciaEsc, nuevoDirector, usuarioDirector);
                    }
                }
                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }
        internal bool InsertLicenciaDirector(IDataContext dctx, ALicencia licencia, LicenciaEscuela licenciaEscuela)
        {
            if (licencia.Tipo != ETipoLicencia.DIRECTOR)
                return InsertLicenciaDocente(dctx, licencia, licenciaEscuela);

            new LicenciaDirectorInsHlp().Action(dctx, (LicenciaDirector)licencia, licenciaEscuela);
            return true;
        }

        private bool InsertLicenciaDocente(IDataContext dctx, ALicencia licencia, LicenciaEscuela licenciaEscuela)
        {
            if (licencia.Tipo != ETipoLicencia.DOCENTE)
                return InsertLicenciaAlumno(dctx, licencia, licenciaEscuela);

            new LicenciaDocenteInsHlp().Action(dctx, (LicenciaDocente)licencia, licenciaEscuela);
            return true;
        }

        private bool InsertLicenciaEspecialista(IDataContext dctx, ALicencia licencia, LicenciaEscuela licenciaEscuela)
        {
            if (licencia.Tipo != ETipoLicencia.ESPECIALISTA)
                throw new ArgumentException("Tipo de licencia no existe.");
                

            new LicenciaEspecialistaInsHlp().Action(dctx, (LicenciaEspecialistaPruebas)licencia, licenciaEscuela);
            return true;
        }

        private bool InsertLicenciaTutor(IDataContext dctx, ALicencia licencia, LicenciaEscuela licenciaEscuela)
        {
            if (licencia.Tipo != ETipoLicencia.TUTOR)
                throw new ArgumentException("Tipo de licencia no existe.");

            new LicenciaTutorInsHlp().Action(dctx, (LicenciaTutor)licencia, licenciaEscuela);
            return true;
        }

        private bool InsertLicenciaUniversidad(IDataContext dctx, ALicencia licencia, LicenciaEscuela licenciaEscuela)
        {
            if (licencia.Tipo != ETipoLicencia.UNIVERSIDAD)
                throw new ArgumentException("Tipo de licencia no existe.");

            new LicenciaUniversidadInsHlp().Action(dctx, (LicenciaUniversidad)licencia, licenciaEscuela);
            return true;
        }

        private bool InsertLicenciaAlumno(IDataContext dctx, ALicencia licencia, LicenciaEscuela licenciaEscuela)
        {
            if (licencia.Tipo != ETipoLicencia.ALUMNO)
                return InsertLicenciaEspecialista(dctx, licencia, licenciaEscuela);

            new LicenciaAlumnoInsHlp().Action(dctx, (LicenciaAlumno)licencia, licenciaEscuela);

            return true;
        }

        /// <summary>
        /// Consulta registros de LicenciaEscuela en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaEscuela">LicenciaEscuela que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de LicenciaEscuela generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, LicenciaEscuela licenciaEscuela)
        {
            LicenciaEscuelaRetHlp da = new LicenciaEscuelaRetHlp();
            DataSet ds = da.Action(dctx, licenciaEscuela);
            return ds;
        }

        public DataSet RetrieveFilterByEscuela(IDataContext dctx, LicenciaEscuela licenciaEscuela)
        {
            return new LicenciaEscuelaRetHlp().ActionFilterByEscuela(dctx, licenciaEscuela);
        }
        /// <summary>
        /// Consulta registros de LicenciaEscuela en la base de datos, retornando cada licencia asociada a Alumo, Docente y Director.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaEscuela">LicenciaEscuela que provee el criterio de selección para realizar la consulta</param>
        /// <returns> Lista de LicenciaEscuela que contiene la información generada por la consulta</returns>
        public List<LicenciaEscuela> RetriveLicenciaEscuela(IDataContext dctx, LicenciaEscuela licenciaEscuela)
        {
            List<LicenciaEscuela> resultado = new List<LicenciaEscuela>();
            LicenciaRetHlp licenciaRet = new LicenciaRetHlp();

            DataSet ds = new LicenciaEscuelaRetHlp().Action(dctx, licenciaEscuela);
            foreach (DataRow rowLicEscuela in ds.Tables[0].Rows)
            {
                LicenciaEscuela licEscuela = this.DataRowToLicenciaEscuela(rowLicEscuela);
                DataSet dsLicencias = licenciaRet.Action(dctx, licEscuela);
                licEscuela.ModulosFuncionales = this.RetrieveModulosFuncionalesLicenciaEscuela(dctx, licEscuela);
                foreach (DataRow rowLicencia in dsLicencias.Tables[0].Rows)
                    licEscuela.AddLicencia(this.CreateLicenciaAlumno(rowLicencia));

                resultado.Add(licEscuela);
            }

            return resultado;
        }
        /// <summary>
        /// Consulta registros de LicenciaEscuela en la base de datos, para el usuario especificado.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuario">Usuario que provee el criterio de selección para realizar la consulta</param>
        /// <returns> Lista de LicenciaEscuela que contiene la información generada por la consulta</returns>
        public List<LicenciaEscuela> RetrieveLicencia(IDataContext dctx, Usuario usuario)
        {
            List<LicenciaEscuela> resultado = new List<LicenciaEscuela>();

            LicenciaRetHlp licenciaRet = new LicenciaRetHlp();
            LicenciaEscuelaRetHlp licenciaEscuelaRet = new LicenciaEscuelaRetHlp();

            DataSet ds = licenciaRet.Action(dctx, usuario);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (!resultado.Exists(e => e.LicenciaEscuelaID.Value == row.Field<long>("LicenciaEscuelaID")))
                {
                    LicenciaEscuela escuela = new LicenciaEscuela();
                    escuela.LicenciaEscuelaID = row.Field<long>("LicenciaEscuelaID");
                    escuela = this.LastDataRowToLicenciaEscuela(licenciaEscuelaRet.Action(dctx, escuela));
                    escuela.ModulosFuncionales = this.RetrieveModulosFuncionalesLicenciaEscuela(dctx, escuela);
                    bool agregar = true;
                    if (escuela.Tipo == ELicenciaEscuela.TEMPORAL)
                    {
                        LicenciaEscuelaTemporal tmp = (LicenciaEscuelaTemporal)escuela;
                        DateTime actual = DateTime.Now;

                        if (!(tmp.FechaInicio <= actual && actual <= tmp.FechaFin))
                            agregar = false;
                    }

                    if (agregar)
                        resultado.Add(escuela);
                }

                ALicencia licencia = this.CreateLicenciaAlumno(row);
                if (resultado.Exists(e => e.LicenciaEscuelaID.Value == row.Field<long>("LicenciaEscuelaID")))
                    resultado.First(e => e.LicenciaEscuelaID.Value == row.Field<long>("LicenciaEscuelaID")).AddLicencia(licencia);
            }

            return resultado;
        }

        public Usuario RetrieveUsuario(IDataContext dctx, Director director)
        {
            Usuario usuario = new Usuario();

            UsuarioDirector da = new UsuarioDirector();
            DataSet ds = da.Action(dctx, director);

            if (ds.Tables[0].Rows.Count > 0)
                usuario.UsuarioID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<int>("UsuarioID");

            return usuario;
        }

        public Usuario RetrieveUsuario(IDataContext dctx, Docente docente)
        {
            Usuario usuario = new Usuario();

            UsuarioDocente da = new UsuarioDocente();
            DataSet ds = da.Action(dctx, docente);

            if (ds.Tables[0].Rows.Count > 0)
                usuario.UsuarioID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<int>("UsuarioID");

            return usuario;
        }

        public List<Usuario> RetrieveUsuarios(IDataContext dctx, Docente docente, bool valid = true)
        {
            Usuario usuario = new Usuario();
            List<Usuario> usuarios = new List<Usuario>();
            UsuarioCtrl usrCtrl = new UsuarioCtrl();

            UsuarioDocente da = new UsuarioDocente();
            
            DataSet ds = da.Action(dctx, docente, valid);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Usuario usuarioAdd = new Usuario();

                if (row.IsNull("UsuarioID"))
                    usuarioAdd.UsuarioID = null;
                else
                    usuarioAdd.UsuarioID = (int)Convert.ChangeType(row["UsuarioID"], typeof(int));

                usuarios.Add(usrCtrl.LastDataRowToUsuario(usrCtrl.Retrieve(dctx, usuarioAdd)));
            }

            return usuarios;
        }


        /// <summary>
        /// Obtiene el Usuario del Especialista en Pruebas
        /// </summary>
        /// <param name="dctx">Contexto</param>
        /// <param name="especialista">Especialista filtro</param>
        /// <returns></returns>
        public Usuario RetrieveUsuario(IDataContext dctx, EspecialistaPruebas especialista)
        {
            Usuario usuario = new Usuario();
            UsuarioEspecialista da = new UsuarioEspecialista();
            DataSet ds = da.Action(dctx, especialista);
            if (ds.Tables[0].Rows.Count > 0)
                usuario.UsuarioID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<int>("UsuarioID");
            return usuario;
        }

        /// <summary>
        /// Obtiene el usuario del tutor
        /// </summary>
        /// <param name="dctx">Contexto</param>
        /// <param name="usuario">Usuario filtro</param>
        /// <param name="tutor">Tutor filtro</param>
        /// <returns>Usuario</returns>
        public Usuario RetrieveUsuarioTutor(IDataContext dctx, Tutor tutor)
        {            
            UsuarioTutor da = new UsuarioTutor();
            Usuario usuario = new Usuario();
            DataSet ds = da.Action(dctx, new Usuario(), tutor);
            if (ds.Tables[0].Rows.Count > 0)
            {
                usuario.UsuarioID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<int>("UsuarioID");
                usuario.NombreUsuario = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("NombreUsuario");
                usuario.AceptoTerminos = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<bool>("AceptoTerminos");
            }
            return usuario;
        }

        /// <summary>
        /// Obtiene el usuario de la universidad
        /// </summary>
        /// <param name="dctx">Contexto</param>
        /// <param name="usuario">Usuario filtro</param>
        /// <param name="tutor">Universidad filtro</param>
        /// <returns>Usuario</returns>
        public Usuario RetrieveUsuarioUniversidad(IDataContext dctx, Universidad universidad)
        {
            UsuarioUniversidad da = new UsuarioUniversidad();
            Usuario usuario = new Usuario();
            DataSet ds = da.Action(dctx, new Usuario(), universidad);
            if (ds.Tables[0].Rows.Count > 0)
            {
                usuario.UsuarioID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<int>("UsuarioID");
                usuario.NombreUsuario = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("NombreUsuario");
            }
            return usuario;
        }

        /// <summary>
        /// Obtiene el tutor del usuario
        /// </summary>
        /// <param name="dctx">Contexto</param>
        /// <param name="usuario">Usuario filtro</param>
        /// <param name="tutor">Tutor filtro</param>
        /// <returns>Tutor</returns>
        public Tutor RetrieveUsuarioTutor(IDataContext dctx, Usuario usuario)
        {
            UsuarioTutor da = new UsuarioTutor();
            Tutor tutor = new Tutor();
            DataSet ds = da.Action(dctx, usuario, null);
            if (ds.Tables[0].Rows.Count > 0)
            {
                tutor.TutorID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<long>("TutorID");
                tutor.Nombre = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("Nombre");
            }
            return tutor;
        }

        public Alumno RetrieveUsuarioAlumno(IDataContext dctx, Usuario usuario)
        {
            UsuarioAlumno da = new UsuarioAlumno();
            Alumno alumno = new Alumno();
            DataSet ds = da.Action(dctx, usuario, null);
            if (ds.Tables[0].Rows.Count > 0)
            {
                alumno.AlumnoID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<long>("AlumnoID");
                alumno.Nombre = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("Nombre");
            }
            return alumno;
        }

        /// <summary>
        /// Obtiene el docente del usuario
        /// </summary>
        /// <param name="dctx">Contexto</param>
        /// <param name="usuario">Usuario filtro</param>
        /// <param name="tutor">Docente filtro</param>
        /// <returns>Tutor</returns>
        public Docente RetrieveUsuarioOrientador(IDataContext dctx, Usuario usuario)
        {
            UsuarioOrientador da = new UsuarioOrientador();
            Docente docente = new Docente();
            DataSet ds = da.Action(dctx, usuario);
            if (ds.Tables[0].Rows.Count > 0)
            {
                docente.DocenteID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<int>("DocenteID");
                docente.Nombre = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("Nombre");
                docente.PrimerApellido = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("PrimerApellido");
                docente.SegundoApellido = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("SegundoApellido");
            }
            return docente;
        }

        public OrientadorUniversidad RetrieveUsuarioOrientadorUniversidad(IDataContext dctx, Usuario usuario)
        {
            UsuarioOrientador da = new UsuarioOrientador();
            OrientadorUniversidad orientador = new OrientadorUniversidad();
            DataSet ds = da.Action(dctx, usuario);
            if (ds.Tables[0].Rows.Count > 0)
            {
                orientador.UsuarioID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<int>("UsuarioID");
                orientador.NombreUsuario = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("NombreUsuario");
                orientador.DocenteID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<int>("DocenteID");
                orientador.Nombre = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("Nombre");
                orientador.PrimerApellido = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("PrimerApellido");
                orientador.SegundoApellido = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("SegundoApellido");


                if (ds.Tables[0].Rows[0]["UniversidadID"] != DBNull.Value) 
                {
                    orientador.UniversidadID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<long>("universidadid");
                }
                else 
                {
                    orientador.UniversidadID = 0;
                }

                if (ds.Tables[0].Rows[0]["NombreUniversidad"] != DBNull.Value)
                {
                    orientador.NombreUniversidad = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("NombreUniversidad");
                }
                else
                {
                    orientador.NombreUniversidad = "Plataforma";
                }
                
                
            }
            return orientador;
        }

        public Usuario RetrieveUsuario(IDataContext dctx, Alumno alumno)
        {
            Usuario usuario = new Usuario();

            UsuarioAlumno da = new UsuarioAlumno();
            DataSet ds = da.Action(dctx, alumno);

            if (ds.Tables[0].Rows.Count > 0)
                usuario.UsuarioID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<int>("UsuarioID");

            return usuario;
        }

        public UsuarioSocial RetrieveUsuarioSocial(IDataContext dctx, Docente docente)
        {
            UsuarioSocial usuarioSocial = new UsuarioSocial();

            UsuarioSocialDocente da = new UsuarioSocialDocente();
            DataSet ds = da.Action(dctx, docente);

            if (ds.Tables[0].Rows.Count > 0)
                usuarioSocial.UsuarioSocialID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<long>("UsuarioSocialID");

            return usuarioSocial;
        }

        public UsuarioSocial RetrieveUsuarioSocial(IDataContext dctx, Usuario usuario)
        {
            UsuarioSocial usuarioSocial = new UsuarioSocial();

            UsuarioSocialUsuario da = new UsuarioSocialUsuario();
            DataSet ds = da.Action(dctx, null, usuario);

            if (ds.Tables[0].Rows.Count > 0)
            {
                usuarioSocial.UsuarioSocialID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<long>("UsuarioSocialID");
                usuarioSocial.ScreenName = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("ScreenName");
            }
            return usuarioSocial;
        }

        //Obtiene el usuario social, cuando el parametro docente es false
        public UsuarioSocial RetrieveUserSocial(IDataContext dctx, Usuario usuario, bool esDocente = false)
        {
            UsuarioSocial usuarioSocial = new UsuarioSocial();

            UsuarioSocialUsuario da = new UsuarioSocialUsuario();
            DataSet ds = da.Action(dctx, null, usuario, esDocente);

            if (ds.Tables[0].Rows.Count > 0)
            {
                usuarioSocial.UsuarioSocialID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<long>("UsuarioSocialID");
                usuarioSocial.ScreenName = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<string>("ScreenName");
            }
            return usuarioSocial;
        }

        public Usuario RetrieveUsuario(IDataContext dctx, UsuarioSocial usuarioSocial, bool esDocente = true)
        {
            Usuario usuario = new Usuario();

            UsuarioSocialUsuario da = new UsuarioSocialUsuario();
            DataSet ds = da.Action(dctx, usuarioSocial, null, esDocente);

            if (ds.Tables[0].Rows.Count > 0)
                usuario.UsuarioID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<int>("UsuarioID");

            return usuario;
        }

        public UsuarioSocial RetrieveUsuarioSocial(IDataContext dctx, LicenciaEscuela licenciaEscuela, Docente docente)
        {
            UsuarioSocial usuarioSocial = new UsuarioSocial();
            UsuarioSocialDocente da = new UsuarioSocialDocente();
            DataSet ds = da.Action(dctx, licenciaEscuela, docente);

            if (ds.Tables[0].Rows.Count > 0)
                usuarioSocial.UsuarioSocialID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<long>("UsuarioSocialID");

            return usuarioSocial;
        }

        public UsuarioSocial RetrieveUsuarioSocial(IDataContext dctx, Alumno alumno)
        {
            UsuarioSocial usuarioSocial = new UsuarioSocial();

            UsuarioSocialAlumno da = new UsuarioSocialAlumno();
            DataSet ds = da.Action(dctx, alumno);

            if (ds.Tables[0].Rows.Count > 0)
                usuarioSocial.UsuarioSocialID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<long>("UsuarioSocialID");

            return usuarioSocial;
        }

        public Docente RetrieveDocente(IDataContext dctx, UsuarioSocial usuarioSocial)
        {
            Docente docente = new Docente();

            DocenteUsuarioSocialDARetHlp da = new DocenteUsuarioSocialDARetHlp();
            DataSet ds = da.Action(dctx, usuarioSocial);

            if (ds.Tables[0].Rows.Count > 0)
                docente.DocenteID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<int>("DocenteID");

            return docente;
        }

        public Alumno RetrieveAlumno(IDataContext dctx, UsuarioSocial usuarioSocial)
        {
            Alumno alumno = new Alumno();
            AlumnoUsuarioSocialDARetHlp da = new AlumnoUsuarioSocialDARetHlp();
            DataSet ds = da.Action(dctx, usuarioSocial);

            if (ds.Tables[0].Rows.Count > 0)
                alumno.AlumnoID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<long>("AlumnoID");

            return alumno;
        }

        public List<Alumno> RetriveAlumnosEscuela(IDataContext dctx, CicloEscolar cicloEscolar, Escuela escuela)
        {
            List<Alumno> ls = new List<Alumno>();
            DataSet ds = Retrieve(dctx,
                                  new LicenciaEscuela { Activo = true, Escuela = escuela, CicloEscolar = cicloEscolar });

            List<LicenciaEscuela> lslicenciaEscuela = RetriveLicenciaEscuela(dctx, new LicenciaEscuela { Activo = true, Escuela = escuela, CicloEscolar = cicloEscolar });

            foreach (LicenciaEscuela licenciaEscuela in lslicenciaEscuela)
            {
                //Consultar la licencia
                if (licenciaEscuela == null)
                    continue;


                ds = (new LicenciaAlumnoRetHlp()).Action(dctx, new LicenciaAlumno { Activo = true, Alumno = new Alumno(), Usuario = new Usuario(), UsuarioSocial = new UsuarioSocial() }, licenciaEscuela);
                int cont = ds.Tables[0].Rows.Count;
                if (cont > 0)
                {
                    foreach (DataRow dralumno in ds.Tables[0].Rows)
                    {
                        LicenciaAlumno licalumno = (LicenciaAlumno)this.CreateLicenciaAlumno(dralumno);
                        if (licalumno.LicenciaID != null)
                            if (licalumno.Activo.Value)
                                ls.Add(licalumno.Alumno);
                    }
                }

            }
            return ls;
        }

        [System.Obsolete("Usar TieneLicenciasDisponibles() de la clase ContratoCtrl")]
        public short RetrieveLicenciasAlumnoDisponibles(IDataContext dctx, LicenciaEscuela licenciaEscuela)
        {
            if (licenciaEscuela == null || licenciaEscuela.LicenciaEscuelaID <= 0)
                throw new Exception("RetrieveLicenciasAlumnoDisponibles:LicenciaEscuela no puede ser nulo");

            DataSet dslicEscuela = Retrieve(dctx, licenciaEscuela);
            LicenciaEscuela licencia = LastDataRowToLicenciaEscuela(dslicEscuela);

            LicenciaAlumno licenciaAlumno = new LicenciaAlumno();
            licenciaAlumno.Usuario = new Usuario();
            licenciaAlumno.Alumno = new Alumno();
            licenciaAlumno.UsuarioSocial = new UsuarioSocial();


            LicenciaAlumnoRetHlp da = new LicenciaAlumnoRetHlp();
            DataSet ds = da.Action(dctx, licenciaAlumno, licencia);

            int index = ds.Tables[0].Rows.Count;
            short total = (short)(licenciaEscuela.NumeroLicencias != null ? (short)licenciaEscuela.NumeroLicencias : 0);
            if (total <= 0)
                return total;

            short utilizadas = 0;

            foreach (DataRow rw in ds.Tables[0].Rows)
                if (rw.Field<byte>("Tipo") == (byte)ETipoLicencia.ALUMNO)
                {
                    LicenciaAlumno licAlumno = (LicenciaAlumno)CreateLicenciaAlumno(rw);
                    if (licAlumno.Activo != null && (bool)licAlumno.Activo)
                        utilizadas++;
                }

            if (utilizadas >= total)
                return 0;

            return (short)(total - utilizadas);
        }

        /// <summary>
        /// Consulta los alumnos dentro de las licencias de una licencia escuela activas.
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="alumno"></param>
        /// <param name="licenciaEscuela"></param>
        /// <returns>DataSet con registros de alumnos</returns>
        public DataSet RetrieveAlumnosLicenciaEscuela(IDataContext dctx, Alumno alumno, LicenciaEscuela licenciaEscuela)
        {
            AlumnosLicenciaEscuelaDARetHlp da = new AlumnosLicenciaEscuelaDARetHlp();
            return da.Action(dctx, licenciaEscuela, alumno);
        }

        internal ALicencia CreateLicenciaAlumno(DataRow row)
        {
            if (row.Field<byte>("Tipo") != (byte)ETipoLicencia.ALUMNO)
                return CreateLicenciaDocente(row);

            LicenciaAlumno licencia = new LicenciaAlumno();
            licencia.Usuario = new Usuario();
            licencia.UsuarioSocial = new UsuarioSocial();
            licencia.Alumno = new Alumno();

            if (row.IsNull("LicenciaID"))
                licencia.LicenciaID = null;
            else
                licencia.LicenciaID = row.Field<Guid>("LicenciaID");
            if (row.IsNull("UsuarioID"))
                licencia.Usuario.UsuarioID = null;
            else
                licencia.Usuario.UsuarioID = row.Field<int>("UsuarioID");
            if (row.IsNull("UsuarioSocialID"))
                licencia.UsuarioSocial.UsuarioSocialID = null;
            else
                licencia.UsuarioSocial.UsuarioSocialID = row.Field<long>("UsuarioSocialID");
            if (row.IsNull("AlumnoID"))
                licencia.Alumno.AlumnoID = null;
            else
                licencia.Alumno.AlumnoID = row.Field<long>("AlumnoID");
            if (row.IsNull("Activo"))
                licencia.Activo = null;
            else
                licencia.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            return licencia;
        }

        internal ALicencia CreateLicenciaTutor(DataRow row)
        {
            if (row.Field<byte>("Tipo") != (byte)ETipoLicencia.TUTOR)
                return CreateLicenciaUniversidad(row);

            LicenciaTutor licencia = new LicenciaTutor();
            licencia.Usuario = new Usuario();
            licencia.Tutor = new Tutor();

            if (row.IsNull("LicenciaID"))
                licencia.LicenciaID = null;
            else
                licencia.LicenciaID = row.Field<Guid>("LicenciaID");
            if (row.IsNull("UsuarioID"))
                licencia.Usuario.UsuarioID = null;
            else
                licencia.Usuario.UsuarioID = row.Field<int>("UsuarioID");            
            if (row.IsNull("TutorID"))
                licencia.Tutor.TutorID = null;
            else
                licencia.Tutor.TutorID = row.Field<long>("TutorID");
            if (row.IsNull("Activo"))
                licencia.Activo = null;
            else
                licencia.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            return licencia;
        }

        internal ALicencia CreateLicenciaUniversidad(DataRow row)
        {
            if (row.Field<byte>("Tipo") != (byte)ETipoLicencia.UNIVERSIDAD)
                return CreateLicenciaExperto(row);

            LicenciaUniversidad licencia = new LicenciaUniversidad();
            licencia.Usuario = new Usuario();
            licencia.Universidad = new Universidad();

            if (row.IsNull("LicenciaID"))
                licencia.LicenciaID = null;
            else
                licencia.LicenciaID = row.Field<Guid>("LicenciaID");
            if (row.IsNull("UsuarioID"))
                licencia.Usuario.UsuarioID = null;
            else
                licencia.Usuario.UsuarioID = row.Field<int>("UsuarioID");
            if (row.IsNull("UniversidadID"))
                licencia.Universidad.UniversidadID = null;
            else
                licencia.Universidad.UniversidadID = row.Field<long>("UniversidadID");
            if (row.IsNull("Activo"))
                licencia.Activo = null;
            else
                licencia.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            return licencia;
        }

        private ALicencia CreateLicenciaDocente(DataRow row)
        {
            if (row.Field<byte>("Tipo") != (byte)ETipoLicencia.DOCENTE)
                return CreateLicenciaDirector(row);

            LicenciaDocente licencia = new LicenciaDocente();
            licencia.Usuario = new Usuario();
            licencia.UsuarioSocial = new UsuarioSocial();
            licencia.Docente = new Docente();

            if (row.IsNull("LicenciaID"))
                licencia.LicenciaID = null;
            else
                licencia.LicenciaID = row.Field<Guid>("LicenciaID");
            if (row.IsNull("UsuarioID"))
                licencia.Usuario.UsuarioID = null;
            else
                licencia.Usuario.UsuarioID = row.Field<int>("UsuarioID");
            if (row.IsNull("UsuarioSocialID"))
                licencia.UsuarioSocial.UsuarioSocialID = null;
            else
                licencia.UsuarioSocial.UsuarioSocialID = row.Field<long>("UsuarioSocialID");
            if (row.IsNull("DocenteID"))
                licencia.Docente.DocenteID = null;
            else
                licencia.Docente.DocenteID = row.Field<int>("DocenteID");
            if (row.IsNull("Activo"))
                licencia.Activo = null;
            else
                licencia.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));

            return licencia;
        }

        private ALicencia CreateLicenciaDirector(DataRow row)
        {
            if (row.Field<byte>("Tipo") != (byte)ETipoLicencia.DIRECTOR)
                return CreateLicenciaTutor(row);
            //throw new ArgumentOutOfRangeException("Licencia", "Tipo de Licencia no definido.");

            LicenciaDirector licencia = new LicenciaDirector();
            licencia.Usuario = new Usuario();
            licencia.Director = new Director();

            if (!row.IsNull("LicenciaID"))
                licencia.LicenciaID = row.Field<Guid>("LicenciaID");
            if (!row.IsNull("UsuarioID"))
                licencia.Usuario.UsuarioID = row.Field<int>("UsuarioID");
            if (!row.IsNull("DirectorID"))
                licencia.Director.DirectorID = row.Field<int>("DirectorID");
            if (row.IsNull("Activo"))
                licencia.Activo = null;
            else
                licencia.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));

            return licencia;
        }

        private ALicencia CreateLicenciaExperto(DataRow row)
        {
            if (row.Field<byte>("Tipo") != (byte)ETipoLicencia.ESPECIALISTA)
                throw new ArgumentOutOfRangeException("Licencia", "Tipo de Licencia no definido.");

            LicenciaEspecialistaPruebas licencia = new LicenciaEspecialistaPruebas();
            licencia.Usuario = new Usuario();
            licencia.EspecialistaPruebas = new EspecialistaPruebas();
            if (!row.IsNull("LicenciaID"))
                licencia.LicenciaID = row.Field<Guid>("LicenciaID");
            if (!row.IsNull("UsuarioID"))
                licencia.Usuario.UsuarioID = row.Field<int>("UsuarioID");
            if (!row.IsNull("EspecialistaID"))
                licencia.EspecialistaPruebas.EspecialistaPruebaID = row.Field<int>("EspecialistaID");
            if (row.IsNull("Activo"))
                licencia.Activo = null;
            else
                licencia.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            return licencia;
        }

        /// <summary>
        /// Recupera el objeto LicenciaEscuela y sus miembros Escuela, CicloEscolar, Contrato y la lista de Contrato.CicloContrato
        /// Método creado para - Consultar paquetes de juegos configurados a la escuela
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaEscuela">LicenciaEscuela que proveerá los criterios para la consulta</param>
        /// <returns>Un objeto LicenciaEscuela con sus relaciones</returns>
        public LicenciaEscuela RetrieveComplete(IDataContext dctx, LicenciaEscuela licenciaEscuela)
        {
            // TODO: JAB - Faltaría, de ser necesario, recuperar la lista de licencias según su tipo
            LicenciaEscuela licenciaEscuelaRet = null;

            DataSet ds = this.Retrieve(dctx, licenciaEscuela);
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                return null;

            if (ds.Tables[0].Rows.Count > 1)
                throw new StandardException(MessageType.Error, "Consulta ambigua!", "La consulta devolvió más de un resultado, no se puede determinar la licencia solicitada!", nameSpace, className, "RetrieveComplete", null);
            else
                licenciaEscuelaRet = this.LastDataRowToLicenciaEscuela(ds);

            EscuelaCtrl escuelaCtrl = new EscuelaCtrl();
            licenciaEscuelaRet.Escuela = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, new Escuela() { EscuelaID = licenciaEscuelaRet.Escuela.EscuelaID }));

            CicloEscolarCtrl cicloEscolarCtrl = new CicloEscolarCtrl();
            licenciaEscuelaRet.CicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, new CicloEscolar() { CicloEscolarID = licenciaEscuelaRet.CicloEscolar.CicloEscolarID }));

            ContratoCtrl contratoCtrl = new ContratoCtrl();
            licenciaEscuelaRet.Contrato = contratoCtrl.RetrieveComplete(dctx, new Contrato() { ContratoID = licenciaEscuelaRet.Contrato.ContratoID });

            licenciaEscuelaRet.ModulosFuncionales = this.RetrieveModulosFuncionalesLicenciaEscuela(dctx, licenciaEscuelaRet);

            return licenciaEscuelaRet;
        }

        /// <summary>
        /// Crea un objeto de LicenciaEscuela a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de LicenciaEscuela</param>
        /// <returns>Un objeto de LicenciaEscuela creado a partir de los datos</returns>
        public LicenciaEscuela LastDataRowToLicenciaEscuela(DataSet ds)
        {
            if (!ds.Tables.Contains("LicenciaEscuela"))
                throw new Exception("LastDataRowToLicenciaEscuela: DataSet no tiene la tabla Ubicacion");
            int index = ds.Tables["LicenciaEscuela"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToLicenciaEscuela: El DataSet no tiene filas");
            return this.DataRowToLicenciaEscuela(ds.Tables["LicenciaEscuela"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de LicenciaEscuela a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de LicenciaEscuela</param>
        /// <returns>Un objeto de LicenciaEscuela creado a partir de los datos</returns>
        public LicenciaEscuela DataRowToLicenciaEscuela(DataRow row)
        {
            if (row.Field<byte>("Tipo") != (byte)ELicenciaEscuela.NOTEMPORAL)
                return DataRowToLicenciaEscuelaTemporal(row);

            LicenciaEscuela licenciaEscuela = new LicenciaEscuela();
            licenciaEscuela.Contrato = new Contrato();
            licenciaEscuela.Escuela = new Escuela();
            licenciaEscuela.CicloEscolar = new CicloEscolar();
            licenciaEscuela.ModulosFuncionales = new List<ModuloFuncional>();

            if (row.IsNull("LicenciaEscuelaID"))
                licenciaEscuela.LicenciaEscuelaID = null;
            else
                licenciaEscuela.LicenciaEscuelaID = (long)Convert.ChangeType(row["LicenciaEscuelaID"], typeof(long));
            if (row.IsNull("NumeroLicencias"))
                licenciaEscuela.NumeroLicencias = null;
            else
                licenciaEscuela.NumeroLicencias = (short)Convert.ChangeType(row["NumeroLicencias"], typeof(short));
            if (row.IsNull("Activo"))
                licenciaEscuela.Activo = null;
            else
                licenciaEscuela.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            if (row.IsNull("ContratoID"))
                licenciaEscuela.Contrato.ContratoID = null;
            else
                licenciaEscuela.Contrato.ContratoID = (long)Convert.ChangeType(row["ContratoID"], typeof(long));
            if (row.IsNull("EscuelaID"))
                licenciaEscuela.Escuela.EscuelaID = null;
            else
                licenciaEscuela.Escuela.EscuelaID = (int)Convert.ChangeType(row["EscuelaID"], typeof(int));
            if (row.IsNull("CicloEscolarID"))
                licenciaEscuela.CicloEscolar.CicloEscolarID = null;
            else
                licenciaEscuela.CicloEscolar.CicloEscolarID = (int)Convert.ChangeType(row["CicloEscolarID"], typeof(int));
            return licenciaEscuela;
        }
        /// <summary>
        /// Crea un objeto de LicenciaEscuelaTemporal a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de LicenciaEscuelaTemporal</param>
        /// <returns>Un objeto de LicenciaEscuelaTemporal creado a partir de los datos</returns>
        internal LicenciaEscuela DataRowToLicenciaEscuelaTemporal(DataRow row)
        {
            if (row.Field<byte>("Tipo") != (byte)ELicenciaEscuela.TEMPORAL)
                throw new ArgumentOutOfRangeException("LicenciaEscuela", "Tipo de Licencia Escuela no definido.");

            LicenciaEscuelaTemporal licenciaEscuela = new LicenciaEscuelaTemporal();
            licenciaEscuela.Contrato = new Contrato();
            licenciaEscuela.Escuela = new Escuela();
            licenciaEscuela.CicloEscolar = new CicloEscolar();
            licenciaEscuela.ModulosFuncionales = new List<ModuloFuncional>();

            if (row.IsNull("LicenciaEscuelaID"))
                licenciaEscuela.LicenciaEscuelaID = null;
            else
                licenciaEscuela.LicenciaEscuelaID = (long)Convert.ChangeType(row["LicenciaEscuelaID"], typeof(long));
            if (row.IsNull("NumeroLicencias"))
                licenciaEscuela.NumeroLicencias = null;
            else
                licenciaEscuela.NumeroLicencias = (short)Convert.ChangeType(row["NumeroLicencias"], typeof(short));
            if (row.IsNull("Activo"))
                licenciaEscuela.Activo = null;
            else
                licenciaEscuela.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
            if (row.IsNull("ContratoID"))
                licenciaEscuela.Contrato.ContratoID = null;
            else
                licenciaEscuela.Contrato.ContratoID = (long)Convert.ChangeType(row["ContratoID"], typeof(long));
            if (row.IsNull("EscuelaID"))
                licenciaEscuela.Escuela.EscuelaID = null;
            else
                licenciaEscuela.Escuela.EscuelaID = (int)Convert.ChangeType(row["EscuelaID"], typeof(int));
            if (row.IsNull("CicloEscolarID"))
                licenciaEscuela.CicloEscolar.CicloEscolarID = null;
            else
                licenciaEscuela.CicloEscolar.CicloEscolarID = (int)Convert.ChangeType(row["CicloEscolarID"], typeof(int));
            if (row.IsNull("FechaInicio"))
                licenciaEscuela.FechaInicio = null;
            else
                licenciaEscuela.FechaInicio = (DateTime)Convert.ChangeType(row["FechaInicio"], typeof(DateTime));
            if (row.IsNull("FechaFin"))
                licenciaEscuela.FechaFin = null;
            else
                licenciaEscuela.FechaFin = (DateTime)Convert.ChangeType(row["FechaFin"], typeof(DateTime));
            return licenciaEscuela;
        }

        /// <summary>
        /// Consulta una licencia activa para ciclo escolar y contrato activos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaId">LicenciaId que se desea consultar</param>
        /// <param name="licenciaEscuela"> </param>
        /// <returns>ALicencia encontrada o nulo de no encontrar licencia valida</returns>
        public ALicencia RetrieveLicencia(IDataContext dctx, Guid licenciaId, out LicenciaEscuela licenciaEscuela)
        {
            ALicencia licencia = null;
            LicenciaDARetHlp da = new LicenciaDARetHlp();
            DataSet ds = da.Action(dctx, licenciaId);
            licenciaEscuela = new LicenciaEscuela();
            int index = ds.Tables[0].Rows.Count;
            if (index >= 1)
            {
                licencia = CreateLicenciaAlumno(ds.Tables["Licencia"].Rows[index - 1]);
                //Consultar los datos de la LicenciaEscuela al que pertenece la licencia
                licenciaEscuela.LicenciaEscuelaID = (long)Convert.ChangeType(ds.Tables["Licencia"].Rows[index - 1]["LicenciaEscuelaID"], typeof(long));
                licenciaEscuela.Activo = true;
                licenciaEscuela = LastDataRowToLicenciaEscuela(Retrieve(dctx, licenciaEscuela));
            }


            return licencia;
        }

        /// <summary>
        /// Actualiza la licencia ciclo escolar de una escuela
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaEscuela">licenciaEscuela que contiene los nuevos datos</param>
        /// <param name="previous">previous que tiene los datos anteriores</param>
        public void ActualizarCicloEscolarLicenciaEscuela(IDataContext dctx, LicenciaEscuela licenciaEscuela, LicenciaEscuela previous)
        {
            if (licenciaEscuela.LicenciaEscuelaID == null) throw new Exception("LicenciaID no puede ser nulo");

            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();
                UsuarioEscolarPrivilegios usuarioPrivilegios = new UsuarioEscolarPrivilegios { Escuela = previous.Escuela, CicloEscolar = previous.CicloEscolar, Estado = true };
                DataSet dsUsuarios = usuarioPrivilegiosCtrl.Retrieve(dctx, usuarioPrivilegios);
                if (dsUsuarios.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drUsuario in dsUsuarios.Tables[0].Rows)
                    {
                        UsuarioPrivilegios usuario = usuarioPrivilegiosCtrl.DataRowToUsuarioPrivilegios(drUsuario) as UsuarioEscolarPrivilegios;
                        UsuarioEscolarPrivilegios usuarioAux = usuario.Clone() as UsuarioEscolarPrivilegios;
                        usuarioAux.CicloEscolar.CicloEscolarID = licenciaEscuela.CicloEscolar.CicloEscolarID;

                        usuarioPrivilegiosCtrl.Update(dctx, usuarioAux, usuario);
                    }
                }

                Update(dctx, licenciaEscuela, previous);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }


        public List<ModuloFuncional> RetrieveModulosFuncionalesLicenciaEscuela(IDataContext dctx, LicenciaEscuela licenciaEscuela)
        {
            LicenciaEscuelaRetHlp da = new LicenciaEscuelaRetHlp();
            DataSet ds = da.Action(dctx, licenciaEscuela, new ModuloFuncional());
            List<ModuloFuncional> modulos = new List<ModuloFuncional>();
            ModuloFuncionalCtrl ctrl = new ModuloFuncionalCtrl();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                modulos.Add(ctrl.DataRowToModuloFuncional(dr));
            }

            return modulos;
        }
    }
}
