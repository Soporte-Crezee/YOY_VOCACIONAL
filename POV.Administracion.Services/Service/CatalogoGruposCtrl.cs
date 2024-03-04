using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.BO;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Logger.Service;
using POV.Seguridad.Utils;

namespace POV.Administracion.Service
{
    public class CatalogoGruposCtrl
    {
        private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private GrupoSocialCtrl grupoSocialCtrl;
        private UsuarioSocialCtrl usuarioSocialCtrl;
        private SocialHubCtrl socialHubCtrl;
       
        
        public CatalogoGruposCtrl()
        {
            grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            grupoSocialCtrl = new GrupoSocialCtrl();
            usuarioSocialCtrl = new UsuarioSocialCtrl();
            socialHubCtrl = new SocialHubCtrl();
        }

        public void UpdateGrupoCicloEscolar(IDataContext dctx,GrupoCicloEscolar grupoCicloEscolar,List<AsignacionAlumnoGrupo> lsAlumns,bool updinfgrupo)
        {
        
            if (grupoCicloEscolar == null)
                throw new Exception("Grupo CicloEscolar es requerido");
            if(!lsAlumns.Any())
                throw new Exception("Asignación Alumnos es requerido");

            if (grupoCicloEscolar.Escuela == null)
                throw new Exception("Escuela es requerida");

            if (grupoCicloEscolar.CicloEscolar == null)
                throw new Exception("Ciclo es requerido");


            object firm = new object();
            try
            {
                dctx.OpenConnection(firm);
                dctx.BeginTransaction(firm);

                #region Licencia Escuela

                DataSet ds = licenciaEscuelaCtrl.Retrieve(dctx,new LicenciaEscuela{CicloEscolar = grupoCicloEscolar.CicloEscolar,Escuela = grupoCicloEscolar.Escuela,Activo = true});

                if (ds.Tables[0].Rows.Count == 0)
                    throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

                LicenciaEscuela licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);

                #endregion

                #region Actualizar Datos Grupo

                GrupoCicloEscolar antgrupoCicloEscolar = grupoCicloEscolarCtrl.RetriveComplete(dctx, new GrupoCicloEscolar{Escuela = grupoCicloEscolar.Escuela,GrupoCicloEscolarID = grupoCicloEscolar.GrupoCicloEscolarID});
                grupoCicloEscolar.GrupoSocialID = antgrupoCicloEscolar.GrupoSocialID;

                if (updinfgrupo)
                {
                    //Actualizar Datos del Grupo
                    GrupoCicloEscolar grupoActual = new GrupoCicloEscolar();
                    grupoActual.Escuela = antgrupoCicloEscolar.Escuela;
                    grupoActual.CicloEscolar = antgrupoCicloEscolar.CicloEscolar;
                    grupoActual.GrupoSocialID = antgrupoCicloEscolar.GrupoSocialID;
                    grupoActual.Activo = antgrupoCicloEscolar.Activo;
                    grupoActual.Grupo = (Grupo)antgrupoCicloEscolar.Grupo.Clone();
                    grupoActual.GrupoCicloEscolarID = antgrupoCicloEscolar.GrupoCicloEscolarID;
                    grupoActual.Clave = antgrupoCicloEscolar.Clave;

                    //Campos a Editar
                    grupoActual.PlanEducativo = grupoCicloEscolar.PlanEducativo;
                    if (antgrupoCicloEscolar.Grupo.Grado != grupoCicloEscolar.Grupo.Grado || antgrupoCicloEscolar.Grupo.Nombre != grupoCicloEscolar.Grupo.Nombre)
                    {

                        grupoActual.Grupo.Nombre = grupoCicloEscolar.Grupo.Nombre;
                        grupoActual.Grupo.Grado = grupoCicloEscolar.Grupo.Grado;
                        GrupoCtrl grupoCtrl = new GrupoCtrl();
                        if (!ValidarGrupoEscolar(dctx, licenciaEscuela, grupoActual.Grupo))
                        {
                            Exception ex = new SystemException("El grupo proporcionado no esta disponible");
                            ex.Source = "Grupo";
                            throw ex;
                        }
                        grupoCtrl.Update(dctx, grupoActual.Grupo, licenciaEscuela.Escuela, antgrupoCicloEscolar.Grupo);
                        GrupoSocial antgruposocial = grupoSocialCtrl.LastDataRowToGrupoSocial(grupoSocialCtrl.Retrieve(dctx, new GrupoSocial { GrupoSocialID = antgrupoCicloEscolar.GrupoSocialID }));

                        GrupoSocial grupoSocial = new GrupoSocial();
                        grupoSocial.GrupoSocialID = antgruposocial.GrupoSocialID;
                        grupoSocial.FechaCreacion = antgruposocial.FechaCreacion;
                        grupoSocial.NumeroMiembros = antgruposocial.NumeroMiembros;
                        grupoSocial.TipoGrupoSocial = antgruposocial.TipoGrupoSocial;
                        grupoSocial.GrupoSocialID = antgrupoCicloEscolar.GrupoSocialID;
                        grupoSocial.Nombre = grupoActual.Grupo.Grado + " " + grupoActual.Grupo.Nombre;
                        grupoSocial.Descripcion = "Grupo " + grupoSocial.Nombre;

                        grupoSocialCtrl.Update(dctx, grupoSocial, antgruposocial);
                    }
                    grupoCicloEscolarCtrl.Update(dctx, grupoActual, antgrupoCicloEscolar);

                    dctx.CommitTransaction(firm);
                    return;
                }
                #endregion

                #region Actualizar Alumnos Asociados Al Grupo

                foreach (AsignacionAlumnoGrupo asignacion in antgrupoCicloEscolar.AsignacionAlumnos)
                    if (!lsAlumns.Exists(asig => asig.Alumno.AlumnoID == asignacion.Alumno.AlumnoID))
                        antgrupoCicloEscolar.AsignacionAlumnoEliminar(asignacion);

                foreach (AsignacionAlumnoGrupo asignacion in lsAlumns)
                    antgrupoCicloEscolar.AsignacionAlumnoAgregar(asignacion);


                AlumnoCtrl alumnoCtrl = new AlumnoCtrl();
                foreach (AsignacionAlumnoGrupo asig in antgrupoCicloEscolar.AsignacionAlumnos)
                {
                    asig.Alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, asig.Alumno));
                    EObjetoEstado estado = antgrupoCicloEscolar.AsignacionAlumnoEstado(asig);
                    switch (estado)
                    {

                        case EObjetoEstado.NUEVO:
                            grupoCicloEscolarCtrl.InsertAsignacionAlumno(dctx, asig.Alumno, grupoCicloEscolar);
                            InsertAlumnoGrupoSocial(dctx, licenciaEscuela, grupoCicloEscolar, asig.Alumno, (long)asig.Alumno.Universidades[0].UniversidadID);
                            break;
                        case EObjetoEstado.ELIMINADO:
                            grupoCicloEscolarCtrl.DeleteAsignacionAlumno(dctx, asig.Alumno, grupoCicloEscolar);
                            DeleteAlumnoGrupoSocial(dctx, licenciaEscuela, grupoCicloEscolar, asig.Alumno);
                            break;
                    }
                }

                #endregion
                dctx.CommitTransaction(firm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firm);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);

                LoggerHlp.Default.Error(this,ex);
                if (ex.Source == "Usuario" || ex.Source == "Telefono" || ex.Source == "Email" || ex.Source=="Grupo")
                    throw;

                throw new Exception("Ocurrió un error al actualizar el grupo");
            }
            finally
            {
                if(dctx.ConnectionState== ConnectionState.Open)
                    dctx.CloseConnection(firm);
            }
        }

        public void InsertGrupoCicloEscolar(IDataContext dctx,GrupoCicloEscolar grupoCicloEscolar,List<AsignacionAlumnoGrupo> lsAlumnos)
        {
            if (grupoCicloEscolar == null)
                throw new Exception("Grupo CicloEscolar es requerido");
      

            if (grupoCicloEscolar.Escuela == null)
                throw new Exception("Escuela es requerida");

            if (grupoCicloEscolar.CicloEscolar == null)
                throw new Exception("Ciclo es requerido");


            object firm = new object();
            try
            {
                dctx.OpenConnection(firm);
                dctx.BeginTransaction(firm);

                #region Licencia Escuela


                DataSet ds = licenciaEscuelaCtrl.Retrieve(dctx, new LicenciaEscuela { CicloEscolar = grupoCicloEscolar.CicloEscolar, Escuela = grupoCicloEscolar.Escuela, Activo = true });

                if (ds.Tables[0].Rows.Count == 0)
                    throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

                LicenciaEscuela licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);

                #endregion

                #region Registrar Grupo
                Grupo grupo = new Grupo();


                grupo.Grado = grupoCicloEscolar.Grupo.Grado;
                grupo.Nombre = grupoCicloEscolar.Grupo.Nombre;
                EscuelaCtrl escuelaCtrl = new EscuelaCtrl();

                if (!ValidarGrupoEscolar(dctx, licenciaEscuela, grupo))
                {
                    Exception ex = new SystemException("El grupo proporcionado no esta disponible");
                    ex.Source = "Grupo";
                    throw ex;
                }

                List<Grupo> grupos = escuelaCtrl.RetrieveGrupos(dctx, licenciaEscuela.Escuela, grupo);
                if (!grupos.Any())
                {
                    grupo.GrupoID = Guid.NewGuid();
                    escuelaCtrl.InsertGrupo(dctx, grupo, licenciaEscuela.Escuela);
                    grupos = escuelaCtrl.RetrieveGrupos(dctx, licenciaEscuela.Escuela, grupo);
                }
                grupo = grupos.First();
                #endregion

                GrupoCicloEscolarCtrl gpoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
                grupoCicloEscolar = new GrupoCicloEscolar();
                GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
                GrupoSocial grupoSocial = new GrupoSocial();

                #region Registrar GrupoCicloEscolar
                grupoCicloEscolar.CicloEscolar = licenciaEscuela.CicloEscolar;
                grupoCicloEscolar.Escuela = licenciaEscuela.Escuela;
                grupoCicloEscolar.Grupo = grupo;

                ds = gpoCicloEscolarCtrl.Retrieve(dctx, grupoCicloEscolar);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    #region Registramos GrupoSocial
                    grupoSocial.Nombre = grupo.Grado + " " + grupo.Nombre;
                    grupoSocial.Descripcion = "Grupo " + grupoSocial.Nombre;
                    grupoSocial.FechaCreacion = DateTime.Now;
                    grupoSocial.NumeroMiembros = 0;
                    grupoSocial.TipoGrupoSocial = ETipoGrupoSocial.GRUPO_ALUMNOS;
                    grupoSocial.GrupoSocialGuid = Guid.NewGuid();

                    grupoSocialCtrl.Insert(dctx, grupoSocial);
                    grupoSocial = grupoSocialCtrl.LastDataRowToGrupoSocial(grupoSocialCtrl.Retrieve(dctx, grupoSocial));
                    #endregion

                    grupoCicloEscolar.PlanEducativo = new PlanEducativoCtrl().RetrievePlanEducativo(dctx, DateTime.Now);
                    grupoCicloEscolar.GrupoCicloEscolarID = Guid.NewGuid();
                    grupoCicloEscolar.GrupoSocialID = grupoSocial.GrupoSocialID;
                    grupoCicloEscolar.Clave = new PasswordProvider(5).GetNewPassword(); // generamos Clave

                    #region Asignar Plan Educativo

                    PlanEducativoCtrl planEducativoCtrl = new PlanEducativoCtrl();
                    grupoCicloEscolar.PlanEducativo = grupoCicloEscolar.PlanEducativo;

                    #endregion

                    gpoCicloEscolarCtrl.Insert(dctx, grupoCicloEscolar);
                    grupoCicloEscolar = gpoCicloEscolarCtrl.LastDataRowToGrupoCicloEscolar(gpoCicloEscolarCtrl.Retrieve(dctx, grupoCicloEscolar));


                }
                else
                {
                    grupoCicloEscolar = gpoCicloEscolarCtrl.LastDataRowToGrupoCicloEscolar(ds);

                    grupoSocial.GrupoSocialID = grupoCicloEscolar.GrupoSocialID;
                    grupoSocial = grupoSocialCtrl.LastDataRowToGrupoSocial(grupoSocialCtrl.Retrieve(dctx, grupoSocial));
                }
                #endregion

                dctx.CommitTransaction(firm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firm);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);

                LoggerHlp.Default.Error(this, ex);
                if (ex.Source == "Usuario" || ex.Source == "Telefono" || ex.Source == "Email"|| ex.Source=="Grupo")
                    throw;

                throw new Exception("Ocurrió un error al actualizar el grupo");
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firm);
            }
        }
        
        public void InsertAlumnoGrupoSocial(IDataContext dctx,LicenciaEscuela licenciaEscuela,GrupoCicloEscolar grupoCicloEscolar,Alumno alumno, long universidadID)
        {
           

            try
            {
                //Consultar Datos del Alumno
                if (licenciaEscuela == null || alumno == null || grupoCicloEscolar == null || grupoCicloEscolar.GrupoSocialID==null)
                    return;

                Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, alumno);
                LicenciaAlumno licenciaAlumno = licenciaEscuelaCtrl.RetrieveLicenciaAlumno(dctx, licenciaEscuela, alumno, usuario);
                UsuarioSocial usuarioSocial = licenciaAlumno.UsuarioSocial;
                
                #region Registrar UsuarioSocial

                if (usuarioSocial.UsuarioSocialID != null)
                {
                    usuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, usuarioSocial));

                    if (usuarioSocial.Estatus == false)
                    {
                        UsuarioSocial originalUsrSocial = (UsuarioSocial)usuarioSocial.Clone();
                        originalUsrSocial.Estatus = true;

                        usuarioSocialCtrl.Update(dctx, usuarioSocial, originalUsrSocial);
                        long usuarioSocialID = usuarioSocial.UsuarioSocialID.Value;
                        usuarioSocial = new UsuarioSocial();
                        usuarioSocial.UsuarioSocialID = usuarioSocialID;
                        usuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, usuarioSocial));
                    }
                }
                else
                {
                    usuarioSocial.LoginName = usuario.NombreUsuario;
                    usuarioSocial.ScreenName = alumno.Nombre + " " + alumno.PrimerApellido + ((alumno.SegundoApellido == null || alumno.SegundoApellido.Trim().Length == 0) ? "" : " " + alumno.SegundoApellido);
                    usuarioSocial.FechaNacimiento = alumno.FechaNacimiento;
                    usuario.FechaCreacion = DateTime.Now;
                    usuarioSocial.Estatus = true;

                    socialHubCtrl.InsertSocialHubUsuarioSocial(dctx,usuarioSocial);
                    usuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, usuarioSocial));
                }
                #endregion

                SocialHub socialHub = new SocialHub(); 
                socialHub.SocialProfile = usuarioSocial;
                socialHub.SocialProfileType = ESocialProfileType.USUARIOSOCIAL;
                DataSet ds = socialHubCtrl.RetrieveSocialHubUsuario(dctx, socialHub);
                socialHub = socialHubCtrl.LastDataRowToSocialHub(ds);

                GrupoSocial grupoSocial = grupoSocialCtrl.RetrieveComplete(dctx, new GrupoSocial { GrupoSocialID = grupoCicloEscolar.GrupoSocialID }, alumno.AreasConocimiento, null, universidadID);
                socialHubCtrl.AsociarSocialHubGrupoSocialUsuarioGrupo(dctx,socialHub,grupoSocial,false, alumno.AreasConocimiento, universidadID);

            }
            catch (Exception ex)
            {
                
                throw new Exception("Ocurrió un error mientras al agregar alumno al grupo social");
            }
        }

        public void DeleteAlumnoGrupoSocial(IDataContext dctx, LicenciaEscuela licenciaEscuela, GrupoCicloEscolar grupoCicloEscolar, Alumno alumno)
        {
            if (licenciaEscuela == null || alumno == null || grupoCicloEscolar == null)
                return;

            Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, alumno);
            LicenciaAlumno licenciaAlumno = licenciaEscuelaCtrl.RetrieveLicenciaAlumno(dctx, licenciaEscuela, alumno, usuario);
            UsuarioSocial usuarioSocial = licenciaAlumno.UsuarioSocial;

            if (usuarioSocial.UsuarioSocialID != null)
            {
                //Actualizar el estado del UsuarioSocial del Alumno
                DataSet ds = usuarioSocialCtrl.Retrieve(dctx, usuarioSocial);
                if (ds.Tables[0].Rows.Count != 1)
                    throw new Exception("UsuarioSocial no encontrado");

                usuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(ds);

                SocialHub socialHub = new SocialHub
                                          {
                                              SocialProfile = usuarioSocial,
                                              SocialProfileType = ESocialProfileType.USUARIOSOCIAL
                                          };

                ds = socialHubCtrl.RetrieveSocialHubUsuario(dctx, socialHub);

                if (ds.Tables["SocialHub"].Rows.Count <= 0) //El alumno no tiene socialHub
                    return;
                socialHub = socialHubCtrl.LastDataRowToSocialHub(ds);
                List<GrupoSocial> lsGrupoSocial = (new GrupoSocialCtrl()).RetrieveGruposSocialSocialHub(dctx, socialHub);


                DataSet dtGrupoCiclo = grupoCicloEscolarCtrl.Retrieve(dctx, grupoCicloEscolar);
                int index = dtGrupoCiclo.Tables["GrupoCicloEscolar"].Rows.Count;
                if (index > 0)
                    foreach (DataRow drgrupoCiclo in dtGrupoCiclo.Tables["GrupoCicloEscolar"].Rows)
                    {
                        GrupoCicloEscolar grupoCiclo = grupoCicloEscolarCtrl.DataRowToGrupoCicloEscolar(drgrupoCiclo);
                        if (lsGrupoSocial.Exists(gr => gr.GrupoSocialID == grupoCiclo.GrupoSocialID))
                            socialHubCtrl.DesasociarSocialHubGrupoSocialUsuarioGrupo(dctx, socialHub,
                                                                                     new GrupoSocial { GrupoSocialID = grupoCiclo.GrupoSocialID }, alumno.AreasConocimiento, (long)alumno.Universidades[0].UniversidadID);
                    }

            }
        }

        private bool ValidarGrupoEscolar(IDataContext dctx, LicenciaEscuela licenciaEscuela, Grupo grupo)
        {
            if (licenciaEscuela == null)
                throw new ArgumentException("Licencia escuela es requerido", "licenciaEscuela");

            if (grupo == null)
                throw new ArgumentException("Grupo es requerido", "grupo");

            Grupo antgrupo = new Grupo();

            antgrupo.Grado = grupo.Grado;
            antgrupo.Nombre = grupo.Nombre;
            EscuelaCtrl escuelaCtrl = new EscuelaCtrl();

            List<Grupo> grupos = escuelaCtrl.RetrieveGrupos(dctx, licenciaEscuela.Escuela, antgrupo);

            if (grupos.Any())
            {
                if (grupo.GrupoID != null)
                    return grupo.GrupoID == grupos.First().GrupoID;
            }
            else
                return true;

            return false;
        }
        
    }
}
