// Clase de GrupoCicloEscolar
using System;
using System.Collections.Generic;
using System.Data;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.CentroEducativo.DA;
using POV.Seguridad.BO;
namespace POV.CentroEducativo.Service
{
    /// <summary>
    /// Controlador del objeto Escuela
    /// </summary>
    public class GrupoCicloEscolarCtrl
    {
        /// <summary>
        /// Consulta registros de GrupoCicloEscolar en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="grupoCicloEscolar">GrupoCicloEscolar que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de GrupoCicloEscolar generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar)
        {
            GrupoCicloEscolarRetHlp da = new GrupoCicloEscolarRetHlp();
            DataSet ds = da.Action(dctx, grupoCicloEscolar);
            return ds;
        }
        /// <summary>
        /// Crea un registro de GrupoCicloEscolar en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="grupoCicloEscolar">GrupoCicloEscolar que desea crear</param>
        public void Insert(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar)
        {
            GrupoCicloEscolarInsHlp da = new GrupoCicloEscolarInsHlp();
            da.Action(dctx, grupoCicloEscolar);
        }

        public void InsertAsignacionAlumno(IDataContext dctx, Alumno alumno, GrupoCicloEscolar grupoCicloEscolar)
        {
            object firma = new object();
            try
            {
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);

                AsignacionAlumnoGrupo asignacion = new AsignacionAlumnoGrupo();
                asignacion.Alumno = alumno;

                AsignacionAlumnoGrupoRetHlp asigRet = new AsignacionAlumnoGrupoRetHlp();
                DataSet ds = asigRet.Action(dctx, asignacion, grupoCicloEscolar);
                int total = ds.Tables[0].Rows.Count;

                if (total > 0)
                    asignacion = this.DataRowToAsignacionAlumnoGrupo(ds.Tables[0].Rows[total - 1]);

                if (asignacion.AsignacionAlumnoGrupoID != null)
                {
                    //if (asignacion.Activo.Value)
                    //    throw new InvalidOperationException("Alumno ya se encuentra asignado a la escuela.");

                    AsignacionAlumnoGrupo anterior = (AsignacionAlumnoGrupo)asignacion.CloneAll();
                    asignacion.Activo = true;
                    asignacion.FechaBaja = null;
                    new AsignacionAlumnoGrupoUpdHlp().Action(dctx, asignacion, anterior, grupoCicloEscolar);
                }
                else
                {
                    asignacion.AsignacionAlumnoGrupoID = Guid.NewGuid();
                    asignacion.Activo = true;
                    asignacion.FechaRegistro = DateTime.Now;
                    new AsignacionAlumnoGrupoInsHlp().Action(dctx, asignacion, grupoCicloEscolar);
                    ds = asigRet.Action(dctx, asignacion, grupoCicloEscolar);
                    total = ds.Tables[0].Rows.Count;
                    asignacion = this.DataRowToAsignacionAlumnoGrupo(ds.Tables[0].Rows[total - 1]);
                }

                new AsignacionAlumnoGrupoUpdHlp().ActionDesactivarAlumno(dctx, asignacion, grupoCicloEscolar);

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
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }

        public void DeleteAsignacionAlumno(IDataContext dctx, Alumno Alumno, GrupoCicloEscolar grupoCicloEscolar)
        {
            object firma = new object();
            try
            {
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);

                AsignacionAlumnoGrupo asignacion = new AsignacionAlumnoGrupo();
                asignacion.Alumno = Alumno;

                AsignacionAlumnoGrupoRetHlp asigRet = new AsignacionAlumnoGrupoRetHlp();
                DataSet ds = asigRet.Action(dctx, asignacion, grupoCicloEscolar);
                int total = ds.Tables[0].Rows.Count;

                if (total > 0)
                    asignacion = this.DataRowToAsignacionAlumnoGrupo(ds.Tables[0].Rows[total - 1]);

                if (asignacion.AsignacionAlumnoGrupoID != null)
                {
                    if (asignacion.Activo.Value)
                    {
                        AsignacionAlumnoGrupo anterior = (AsignacionAlumnoGrupo)asignacion.CloneAll();
                        asignacion.Activo = false;
                        asignacion.FechaBaja = DateTime.Now;
                        new AsignacionAlumnoGrupoUpdHlp().Action(dctx, asignacion, anterior, grupoCicloEscolar);
                    }                                          
                }
                

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
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }
        
        public void InsertAsignacionDocente(IDataContext dctx, Docente docente, Materia materia, GrupoCicloEscolar grupoCicloEscolar)
        {
            object firma = new object();
            try
            {
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);

                AsignacionMateriaGrupoRetHlp asignacionMateriaGrupoRet = new AsignacionMateriaGrupoRetHlp();
                AsignacionMateriaGrupo asignacionMateriaGrupo = new AsignacionMateriaGrupo { Docente = docente, Materia = materia };

                DataSet dsAsignacionMateriaGrupo = asignacionMateriaGrupoRet.Action(dctx, asignacionMateriaGrupo, grupoCicloEscolar);
                int index = dsAsignacionMateriaGrupo.Tables[0].Rows.Count;
                AsignacionMateriaGrupoUpdHlp asignacionMateriaGrupoUpd = new AsignacionMateriaGrupoUpdHlp();
                if (index > 0)
                {
                    //El docente ya se encuentra asignado al grupo para la materia proporcionada
                    asignacionMateriaGrupo = DataRowToAsignacionMateriaGrupo(dsAsignacionMateriaGrupo.Tables[0].Rows[index - 1]);
                    //if (asignacionMateriaGrupo.AsignacionMateriaGrupoID != null)
                    //    if (asignacionMateriaGrupo.Activo != null && (bool)asignacionMateriaGrupo.Activo)
                    //        throw new InvalidOperationException("InsertAsignacionDocente:El docente ya se encuentra asignado al grupo y materia proporcionado");

                    //Actualizar la asignación
                    AsignacionMateriaGrupo previusAsignacion = (AsignacionMateriaGrupo)asignacionMateriaGrupo.CloneAll();
                    asignacionMateriaGrupo.Activo = true;
                    asignacionMateriaGrupo.FechaBaja = null;
                    asignacionMateriaGrupoUpd.Action(dctx, asignacionMateriaGrupo, previusAsignacion, grupoCicloEscolar);
                }
                else
                {
                    //Asignar el docente a  grupo y materia.
                    AsignacionMateriaGrupoInsHlp asignacionMateriaGrupoIns = new AsignacionMateriaGrupoInsHlp();
                    asignacionMateriaGrupo.Activo = true;
                    asignacionMateriaGrupo.FechaRegistro = DateTime.Now;
                    asignacionMateriaGrupoIns.Action(dctx, asignacionMateriaGrupo, grupoCicloEscolar);
                }

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
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }

        public void RemoveAsignacionDocente(IDataContext dctx, Docente docente, Materia materia, GrupoCicloEscolar grupoCicloEscolar)
        {
            //la materia se encuentra asignada para el grupo y cicloEscolar.
            DataSet dsAsignacionMateria = (new AsignacionMateriaGrupoRetHlp()).Action(dctx, new AsignacionMateriaGrupo { Activo = true, Materia = new Materia { MateriaID = materia.MateriaID }, Docente = new Docente { DocenteID = docente.DocenteID } }, grupoCicloEscolar);
            int index = dsAsignacionMateria.Tables[0].Rows.Count;
            if (index == 1) //significa que la materia se encuentra asignada y activa para el docente en el ciclo escolar proporcionado
            {
                AsignacionMateriaGrupo asignacionMateriaGrupo = DataRowToAsignacionMateriaGrupo(dsAsignacionMateria.Tables[0].Rows[index - 1]);
                AsignacionMateriaGrupo previo = (AsignacionMateriaGrupo)asignacionMateriaGrupo.CloneAll();
                asignacionMateriaGrupo.Activo = false;
                asignacionMateriaGrupo.FechaBaja = DateTime.Now;
                (new AsignacionMateriaGrupoUpdHlp()).Action(dctx, asignacionMateriaGrupo, previo, grupoCicloEscolar);

            }
        }

        public List<Alumno> RetrieveAlumnos(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar)
        {
            List<Alumno> alumnos = new List<Alumno>();

            AsignacionAlumnoGrupo asignacion = new AsignacionAlumnoGrupo();
            asignacion.Activo = true;

            AsignacionAlumnoGrupoRetHlp da = new AsignacionAlumnoGrupoRetHlp();
            DataSet ds = da.Action(dctx, asignacion, grupoCicloEscolar);
            AlumnoCtrl AluCtrl = new AlumnoCtrl();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                asignacion = this.DataRowToAsignacionAlumnoGrupo(row);
                Alumno alumno = new Alumno();
                alumno.AlumnoID = asignacion.Alumno.AlumnoID;

                alumnos.Add(AluCtrl.LastDataRowToAlumno(AluCtrl.Retrieve(dctx, alumno)));
            }

            return alumnos;
        }

        public List<Materia> RetrieveMateriasDocente(IDataContext dctx, Docente docente, GrupoCicloEscolar grupoCicloEscolar)
        {
            List<Materia> materias = new List<Materia>();

            MateriaDocenteGrupoCicloEscolarDARetHlp da = new MateriaDocenteGrupoCicloEscolarDARetHlp();
            DataSet ds = da.Action(dctx, docente, grupoCicloEscolar);

            if (ds.Tables[0].Rows.Count > 0)
            {
                MateriaCtrl materiaCtrl = new MateriaCtrl();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    materias.Add(materiaCtrl.LastDataRowToMateria(materiaCtrl.Retrieve(dctx, new Materia { MateriaID = (int)Convert.ChangeType(dr["MateriaID"], typeof(int)) })));
                }
            }

            return materias;
        }

        public List<Materia> RetrieveAsignacionMateriasDocente(IDataContext dctx, Docente docente, GrupoCicloEscolar grupoCicloEscolar)
        {
            List<Materia> materias = new List<Materia>();

            MateriaDocenteGrupoCicloEscolarDARetHlp da = new MateriaDocenteGrupoCicloEscolarDARetHlp();
            DataSet ds = da.Action(dctx, docente, grupoCicloEscolar);

            if (ds.Tables[0].Rows.Count > 0)
            {
                MateriaCtrl materiaCtrl = new MateriaCtrl();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    materias.Add(materiaCtrl.LastDataRowToMateria(materiaCtrl.Retrieve(dctx, new Materia { MateriaID = (int)Convert.ChangeType(dr["MateriaID"], typeof(int)) })));
                }
            }

            return materias;
        }

        public List<Docente> RetrieveDocentes(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar)
        {
            List<Docente> docentes = new List<Docente>();

            AsignacionMateriaGrupo asignacion = new AsignacionMateriaGrupo();
            asignacion.Activo = true;

            AsignacionMateriaGrupoRetHlp da = new AsignacionMateriaGrupoRetHlp();
            DataSet ds = da.Action(dctx, asignacion, grupoCicloEscolar);
            DocenteCtrl AluCtrl = new DocenteCtrl();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                asignacion = this.DataRowToAsignacionMateriaGrupo(row);
                Docente docente = new Docente();
                docente.DocenteID = asignacion.Docente.DocenteID;
                docentes.Add(AluCtrl.LastDataRowToDocente(AluCtrl.Retrieve(dctx, docente)));
            }

            return docentes;
        }

        public List<Materia> RetriveMateriasGrupoCicloEscolar(IDataContext dctx, AsignacionMateriaGrupo asignacionMateria, GrupoCicloEscolar grupoCicloEscolar)
        {
            AsignacionMateriaGrupoRetHlp da = new AsignacionMateriaGrupoRetHlp();
            List<Materia> lsmateriagrupo = new List<Materia>();
            asignacionMateria = asignacionMateria != null ? asignacionMateria : new AsignacionMateriaGrupo();
            asignacionMateria.Activo = true;

            DataSet ds = da.Action(dctx, asignacionMateria, grupoCicloEscolar);
            foreach (DataRow row in ds.Tables[0].Rows)
                lsmateriagrupo.Add(this.DataRowToAsignacionMateriaGrupo(row).Materia);
            return lsmateriagrupo;
        }

        public GrupoCicloEscolar RetrieveGrupoCicloEscolar(IDataContext dctx, Alumno alumno, CicloEscolar cicloEscolar)
        {
            GrupoCicloEscolar resultado = new GrupoCicloEscolar();

            AsignacionAlumnoGrupo asignacion = new AsignacionAlumnoGrupo();
            asignacion.Alumno = alumno;
            asignacion.Activo = true;

            AsignacionAlumnoGrupoRetHlp dao = new AsignacionAlumnoGrupoRetHlp();
            DataSet ds = dao.ActionAsignacionAlumno(dctx, asignacion, cicloEscolar);
            if (ds.Tables[0].Rows.Count > 0)
            {
                resultado.GrupoCicloEscolarID = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Field<Guid>("GRUPOCICLOESCOLARID");
                GrupoCicloEscolarRetHlp gpoDAO = new GrupoCicloEscolarRetHlp();
                resultado = this.LastDataRowToGrupoCicloEscolar(gpoDAO.Action(dctx, resultado));
                DataSet dsgrupo = (new GrupoRetHlp()).Action(dctx, resultado.Grupo, resultado.Escuela);

                if (dsgrupo.Tables["Grupo"].Rows.Count == 1)
                    resultado.Grupo = LastDataRowToGrupo(dsgrupo);
            }

            return resultado;
        }

        /// <summary>
        /// Consulta el grupocicloescolar con el 
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="grupoCicloEscolar"></param>
        /// <returns>GrupoCicloEscolar</returns>
        public GrupoCicloEscolar RetriveComplete(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar)
        {
            if (grupoCicloEscolar == null)
                throw new SystemException("GrupoCicloEscolarCtrl:RetriveComple GrupoCicloEscolar no puede ser nulo");
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                throw new SystemException("GrupoCicloEscolarCtrl:RetriveComplete GrupoCicloEscolarID no puede ser nulo");
            if (grupoCicloEscolar.Escuela == null)
                throw new SystemException("GrupoCicloEscolarCtrl:RetriveComplete Escuela no puede ser nula");
            if (grupoCicloEscolar.Escuela.EscuelaID == null)
                throw new SystemException("GrupoCicloEscolarCtrl:RetriveComplete EscuelaID no puede ser nula");

            DataSet ds = Retrieve(dctx, grupoCicloEscolar);

            if (ds.Tables["GrupoCicloEscolar"].Rows.Count != 1)
                throw new SystemException("GrupoCicloEscolarCtrl:RetriveComplete no se encontraron resultados");

            GrupoCicloEscolar gr = LastDataRowToGrupoCicloEscolar(ds);
            
            AsignacionAlumnoGrupoRetHlp asignacionAlumnoGrupoRet = new AsignacionAlumnoGrupoRetHlp();
            DataSet dsAsignacionAlumnosGrupo = asignacionAlumnoGrupoRet.Action(dctx, new AsignacionAlumnoGrupo { Activo = true }, gr);

            List<AsignacionAlumnoGrupo> lsasignacionAlumnos = new List<AsignacionAlumnoGrupo>();
            if (dsAsignacionAlumnosGrupo.Tables["AsignacionAlumnoGrupo"].Rows.Count >= 1)
                foreach (DataRow dralumno in dsAsignacionAlumnosGrupo.Tables["AsignacionAlumnoGrupo"].Rows)
                    lsasignacionAlumnos.Add(DataRowToAsignacionAlumnoGrupo(dralumno));

            AsignacionMateriaGrupoRetHlp asignacionMateriaGrupoRet = new AsignacionMateriaGrupoRetHlp();
            DataSet dsAsignacionMateriasGrupos = asignacionMateriaGrupoRet.Action(dctx, new AsignacionMateriaGrupo { Activo = true }, gr);
            List<AsignacionMateriaGrupo> lsasignacionMateria=new List<AsignacionMateriaGrupo>();

            if (dsAsignacionMateriasGrupos.Tables["AsignacionMateriaGrupo"].Rows.Count >= 1)
                foreach (DataRow drasignacionMateriaGrupo in dsAsignacionMateriasGrupos.Tables["AsignacionMateriaGrupo"].Rows)
                    lsasignacionMateria.Add(DataRowToAsignacionMateriaGrupo(drasignacionMateriaGrupo));

            int index = ds.Tables["GrupoCicloEscolar"].Rows.Count;

            gr = DataRowToGrupoCicloEscolar(ds.Tables["GrupoCicloEscolar"].Rows[index - 1], lsasignacionAlumnos, lsasignacionMateria);

            gr.Escuela = (new EscuelaCtrl().RetrieveComplete(dctx, gr.Escuela));

            CicloEscolarCtrl cicloEscolarCtrl = new CicloEscolarCtrl();
            gr.CicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, gr.CicloEscolar));

            PlanEducativoCtrl planEducativoCtrl = new PlanEducativoCtrl();
            gr.PlanEducativo = planEducativoCtrl.RetriveComplete(dctx, gr.PlanEducativo);

            DataSet dsgrupo = (new GrupoRetHlp()).Action(dctx, gr.Grupo, gr.Escuela);

            if (dsgrupo.Tables["Grupo"].Rows.Count == 1)
                gr.Grupo = LastDataRowToGrupo(dsgrupo);

            return gr;
        }

        /// <summary>
        /// Actualiza de manera optimista un registro de GrupoCicloEscolar en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="grupoCicloEscolar">GrupoCicloEscolar que tiene los datos nuevos</param>
        /// <param name="previous">GrupoCicloEscolar que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar, GrupoCicloEscolar previous)
        {
            GrupoCicloEscolarUpdHlp da = new GrupoCicloEscolarUpdHlp();
            da.Action(dctx, grupoCicloEscolar, previous);
        }
        /// <summary>
        /// Crea un objeto de GrupoCicloEscolar a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de GrupoCicloEscolar</param>
        /// <returns>Un objeto de GrupoCicloEscolar creado a partir de los datos</returns>
        public GrupoCicloEscolar LastDataRowToGrupoCicloEscolar(DataSet ds)
        {
            if (!ds.Tables.Contains("GrupoCicloEscolar"))
                throw new Exception("LastDataRowToGrupoCicloEscolar: DataSet no tiene la tabla GrupoCicloEscolar");
            int index = ds.Tables["GrupoCicloEscolar"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToGrupoCicloEscolar: El DataSet no tiene filas");
            return this.DataRowToGrupoCicloEscolar(ds.Tables["GrupoCicloEscolar"].Rows[index - 1]);
        }

        /// <summary>
        /// Crea un objeto GrupoCicloEscolar con sus asignacionesAlumnoGrupo y AsignacionesMateriaGrupo
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de GrupoCicloEscolar</param>
        /// <param name="asignacionAlumno">IEnumerable que contiene la lista de Asignaciones de Alumnos del grupo</param>
        /// <param name="asignacionMateria">IEnumerable que contiene la lista de Asignaciones Materias del grupo</param>
        /// <returns></returns>
        private GrupoCicloEscolar DataRowToGrupoCicloEscolar(DataRow row,IEnumerable<AsignacionAlumnoGrupo> asignacionAlumno,IEnumerable<AsignacionMateriaGrupo> asignacionMateria)
        {
            GrupoCicloEscolar grupoCicloEscolar = new GrupoCicloEscolar(asignacionAlumno,asignacionMateria);
            grupoCicloEscolar.CicloEscolar = new CicloEscolar();
            grupoCicloEscolar.Escuela = new Escuela();
            grupoCicloEscolar.Grupo = new Grupo();
            grupoCicloEscolar.PlanEducativo = new PlanEducativo();
            if (row.IsNull("GrupoCicloEscolarID"))
                grupoCicloEscolar.GrupoCicloEscolarID = null;
            else
                grupoCicloEscolar.GrupoCicloEscolarID = (Guid)Convert.ChangeType(row["GrupoCicloEscolarID"], typeof(Guid));
            if (row.IsNull("GrupoSocialID"))
                grupoCicloEscolar.GrupoSocialID = null;
            else
                grupoCicloEscolar.GrupoSocialID = (long)Convert.ChangeType(row["GrupoSocialID"], typeof(long));
            if (row.IsNull("Clave"))
                grupoCicloEscolar.Clave = null;
            else
                grupoCicloEscolar.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
            if (row.IsNull("CicloEscolarID"))
                grupoCicloEscolar.CicloEscolar.CicloEscolarID = null;
            else
                grupoCicloEscolar.CicloEscolar.CicloEscolarID = (int)Convert.ChangeType(row["CicloEscolarID"], typeof(int));
            if (row.IsNull("EscuelaID"))
                grupoCicloEscolar.Escuela.EscuelaID = null;
            else
                grupoCicloEscolar.Escuela.EscuelaID = (int)Convert.ChangeType(row["EscuelaID"], typeof(int));
            if (row.IsNull("GrupoID"))
                grupoCicloEscolar.Grupo.GrupoID = null;
            else
                grupoCicloEscolar.Grupo.GrupoID = (Guid)Convert.ChangeType(row["GrupoID"], typeof(Guid));
            if (row.IsNull("PlanEducativoID"))
                grupoCicloEscolar.PlanEducativo.PlanEducativoID = null;
            else
                grupoCicloEscolar.PlanEducativo.PlanEducativoID = (int)Convert.ChangeType(row["PlanEducativoID"], typeof(int));

            if (row.IsNull("Activo"))
                grupoCicloEscolar.Activo = null;
            else
                grupoCicloEscolar.Activo = (bool)Convert.ChangeType(row["Activo"], typeof(bool));
           
                
            return grupoCicloEscolar;

        }
        /// <summary>
        /// Crea un objeto de GrupoCicloEscolar a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de GrupoCicloEscolar</param>
        /// <returns>Un objeto de GrupoCicloEscolar creado a partir de los datos</returns>
        public GrupoCicloEscolar DataRowToGrupoCicloEscolar(DataRow row)
        {
            GrupoCicloEscolar grupoCicloEscolar = new GrupoCicloEscolar();
            grupoCicloEscolar.CicloEscolar = new CicloEscolar();
            grupoCicloEscolar.Escuela = new Escuela();
            grupoCicloEscolar.Grupo = new Grupo();
            grupoCicloEscolar.PlanEducativo = new PlanEducativo();
            if (row.IsNull("GrupoCicloEscolarID"))
                grupoCicloEscolar.GrupoCicloEscolarID = null;
            else
                grupoCicloEscolar.GrupoCicloEscolarID = (Guid)Convert.ChangeType(row["GrupoCicloEscolarID"], typeof(Guid));
            if (row.IsNull("GrupoSocialID"))
                grupoCicloEscolar.GrupoSocialID = null;
            else
                grupoCicloEscolar.GrupoSocialID = (long)Convert.ChangeType(row["GrupoSocialID"], typeof(long));
            if (row.IsNull("Clave"))
                grupoCicloEscolar.Clave = null;
            else
                grupoCicloEscolar.Clave = (string)Convert.ChangeType(row["Clave"], typeof(string));
            if (row.IsNull("CicloEscolarID"))
                grupoCicloEscolar.CicloEscolar.CicloEscolarID = null;
            else
                grupoCicloEscolar.CicloEscolar.CicloEscolarID = (int)Convert.ChangeType(row["CicloEscolarID"], typeof(int));
            if (row.IsNull("EscuelaID"))
                grupoCicloEscolar.Escuela.EscuelaID = null;
            else
                grupoCicloEscolar.Escuela.EscuelaID = (int)Convert.ChangeType(row["EscuelaID"], typeof(int));
            if (row.IsNull("GrupoID"))
                grupoCicloEscolar.Grupo.GrupoID = null;
            else
                grupoCicloEscolar.Grupo.GrupoID = (Guid)Convert.ChangeType(row["GrupoID"], typeof(Guid));
            if (row.IsNull("PlanEducativoID"))
                grupoCicloEscolar.PlanEducativo.PlanEducativoID = null;
            else
                grupoCicloEscolar.PlanEducativo.PlanEducativoID = (int)Convert.ChangeType(row["PlanEducativoID"], typeof(int));
            return grupoCicloEscolar;
        }

        private AsignacionAlumnoGrupo DataRowToAsignacionAlumnoGrupo(DataRow row)
        {
            AsignacionAlumnoGrupo asignacion = new AsignacionAlumnoGrupo();
            asignacion.Alumno = new Alumno();

            if (row.IsNull("ASIGNACIONALUMNOGRUPOID"))
                asignacion.AsignacionAlumnoGrupoID = null;
            else
                asignacion.AsignacionAlumnoGrupoID = (Guid)Convert.ChangeType(row["ASIGNACIONALUMNOGRUPOID"], typeof(Guid));
            if (row.IsNull("ALUMNOID"))
                asignacion.Alumno.AlumnoID = null;
            else
                asignacion.Alumno.AlumnoID = (long)Convert.ChangeType(row["ALUMNOID"], typeof(long));
            if (row.IsNull("ACTIVO"))
                asignacion.Activo = null;
            else
                asignacion.Activo = (bool)Convert.ChangeType(row["ACTIVO"], typeof(bool));
            if (row.IsNull("FECHAREGISTRO"))
                asignacion.FechaRegistro = null;
            else
                asignacion.FechaRegistro = (DateTime)Convert.ChangeType(row["FECHAREGISTRO"], typeof(DateTime));
            if (row.IsNull("FECHABAJA"))
                asignacion.FechaBaja = null;
            else
                asignacion.FechaBaja = (DateTime)Convert.ChangeType(row["FECHABAJA"], typeof(DateTime));

            return asignacion;
        }

        private AsignacionMateriaGrupo DataRowToAsignacionMateriaGrupo(DataRow row)
        {
            AsignacionMateriaGrupo asignacion = new AsignacionMateriaGrupo();
            asignacion.Docente = new Docente();
            asignacion.Materia = new Materia();

            if (row.IsNull("ASIGNACIONMATERIAGRUPOID"))
                asignacion.AsignacionMateriaGrupoID = null;
            else
                asignacion.AsignacionMateriaGrupoID = (long)Convert.ChangeType(row["ASIGNACIONMATERIAGRUPOID"], typeof(long));
            if (row.IsNull("DOCENTEID"))
                asignacion.Docente.DocenteID = null;
            else
                asignacion.Docente.DocenteID = (int)Convert.ChangeType(row["DOCENTEID"], typeof(int));
            if (row.IsNull("MATERIAID"))
                asignacion.Materia.MateriaID = null;
            else
                asignacion.Materia.MateriaID = (int)Convert.ChangeType(row["MATERIAID"], typeof(int));
            if (row.IsNull("ACTIVO"))
                asignacion.Activo = null;
            else
                asignacion.Activo = (bool)Convert.ChangeType(row["ACTIVO"], typeof(bool));
            if (row.IsNull("FECHAREGISTRO"))
                asignacion.FechaRegistro = null;
            else
                asignacion.FechaRegistro = (DateTime)Convert.ChangeType(row["FECHAREGISTRO"], typeof(DateTime));
            if (row.IsNull("FECHABAJA"))
                asignacion.FechaBaja = null;
            else
                asignacion.FechaBaja = (DateTime)Convert.ChangeType(row["FECHABAJA"], typeof(DateTime));

            return asignacion;
        }
        /// <summary>
        /// Crea un objeto de Grupo a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de Grupor</param>
        /// <returns>Un objeto de Grupo creado a partir de los datos</returns>
        private Grupo LastDataRowToGrupo(DataSet ds)
        {
            if (!ds.Tables.Contains("Grupo"))
                throw new Exception("LastDataRowToGrupo: DataSet no tiene la tabla Grupo");
            int index = ds.Tables["Grupo"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToGrupo:El DataSet ni tiene filas");
            return this.DataRowToGrupo(ds.Tables["Grupo"].Rows[index - 1]);
        }

        private Grupo DataRowToGrupo(DataRow row)
        {
            Grupo grupo = new Grupo();
            if (row.IsNull("GRUPOID"))
                grupo.GrupoID = null;
            else
                grupo.GrupoID = (Guid)Convert.ChangeType(row["GRUPOID"], typeof(Guid));
            if (row.IsNull("NOMBRE"))
                grupo.Nombre = null;
            else
                grupo.Nombre = (string)Convert.ChangeType(row["NOMBRE"], typeof(string));
            if (row.IsNull("GRADO"))
                grupo.Grado = null;
            else
                grupo.Grado = (byte)Convert.ChangeType(row["GRADO"], typeof(byte));

            return grupo;
        }

        /// <summary>
        /// Consulta los alumnos asignados al GrupoCicloEscolar que se proporciona como filtro que tambien puede filtrarse por los datos del alumno.
        /// </summary>
        /// <param name="dctx">DataContext que proporciona el acceso a la base de datos</param>
        /// <param name="grupoCicloEscolar">GrupoCicloEscolar que se usará como filtro para la consulta</param>
        /// <param name="alumno">Alumno que se usará como filtro para la consulta</param>
        /// <returns></returns>
        public DataSet RetrieveAsignacionAlumnosGrupo(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar, Alumno alumno, AreaConocimiento areaConocimiento, Docente docente = null, Usuario usuario = null)
        {
            AsignacionAlumnoGrupoRetHlp da = new AsignacionAlumnoGrupoRetHlp();
            DataSet ds = new DataSet();
            if (docente != null & usuario != null)
                ds = da.ActionAsignacionAlumnoByGrupoAlumno(dctx, grupoCicloEscolar, alumno, areaConocimiento, docente, usuario);
            else
                ds = da.ActionAsignacionAlumnoByGrupoAlumno(dctx, grupoCicloEscolar, alumno, areaConocimiento);

            return ds;
        }
    }
}
