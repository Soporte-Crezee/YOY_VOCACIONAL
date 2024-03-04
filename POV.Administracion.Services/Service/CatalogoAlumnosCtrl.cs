using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;

namespace POV.Administracion.Service
{
 public class CatalogoAlumnosCtrl
    {

        private AlumnoCtrl alumnoCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;
        private UsuarioCtrl usuarioCtrl;
        private UsuarioSocialCtrl usuarioSocialCtrl;
        private SocialHubCtrl socialHubCtrl;
        private UsuarioGrupoCtrl usuarioGrupoCtrl; 
       public CatalogoAlumnosCtrl()
       {
           alumnoCtrl = new AlumnoCtrl();
           licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
           grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
           usuarioCtrl = new UsuarioCtrl();
           usuarioSocialCtrl = new UsuarioSocialCtrl();
           socialHubCtrl = new SocialHubCtrl();
           usuarioGrupoCtrl = new UsuarioGrupoCtrl();          
       }

       public Alumno InsertAlumno(IDataContext dctx, Alumno alumno, Usuario usuario,string password)
       {
           object firm = new object();

           try
           {
               dctx.OpenConnection(firm);
               dctx.BeginTransaction(firm);
               //Licencias Disponibles
               int disponibles;

               ////Consultar alumno
               Alumno antalumno = new Alumno();
               antalumno.Curp = alumno.Curp;
               DataSet dsAlumno = alumnoCtrl.Retrieve(dctx, antalumno);
               //bool exite = ds.Tables[0].Rows.Count != 0;

               #region Registrar Alumno
               if (dsAlumno.Tables[0].Rows.Count != 0)
               {
                   Exception ex = new Exception("El nombre alumno ya se encuentra registrado") { Source = "Alumno" };
                   throw ex;
               }
               else
               {
                   //Registrar alumno
                   alumno.Estatus = true;
                   alumno.EstatusIdentificacion = false;
                   alumno.CorreoConfirmado = false;
                   alumno.CarreraSeleccionada = false;
                   alumno.FechaRegistro = DateTime.Now;
                   alumno.EstatusPago = 0;
                   alumnoCtrl.Insert(dctx, alumno);
                   antalumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, alumno));
                   alumno = antalumno;
               }
               #endregion

               #region Registrar Usuario

               //Consultar Usuario
               Usuario antusuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, antalumno);
               if (antusuario != null && antusuario.UsuarioID != null)
               {
                   //Actualizar Usuario
                   antusuario = usuarioCtrl.RetrieveComplete(dctx, antusuario);

                   //Datos a actualizar
                   Usuario usr = (Usuario)antusuario.Clone();
                   usr.Email = usuario.Email;
                   usr.TelefonoReferencia = usuario.TelefonoReferencia;
                   usr.EsActivo = true;

                  if (!string.IsNullOrEmpty(usr.Email))
                       if (!EmailDisponible(dctx, usr))
                       {
                           Exception ex = new Exception("El correo electrónico proporcionado no se encuentra disponible") { Source = "Email" };
                           throw ex;
                       }

                   if (!string.IsNullOrEmpty(usr.TelefonoReferencia))
                       if (!TelefonoDisponible(dctx, usr))
                       {
                           Exception ex = new Exception("El Teléfono proporcionado no se encuentra disponible ") { Source = "Telefono" };
                           throw ex;
                       }

                   usuarioCtrl.Update(dctx, usr, antusuario);
                   antusuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usr));
                   usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usr));
               }
               else
               {
                   //Registrar Usuario
                   usuario.NombreUsuario = usuario.NombreUsuario;
                   usuario.Password = EncryptHash.SHA1encrypt(password);
                   usuario.EsActivo = true;
                   usuario.FechaCreacion = DateTime.Now;

                   //Consultar Termino Activo
                   usuario.Termino = new GP.SocialEngine.BO.Termino() { TerminoID=1,Cuerpo = "Terminos legales", Estatus=true };
                   if (alumno.NivelEscolar != ENivelEscolar.Superior)
                       usuario.AceptoTerminos = true;
                   else
                       usuario.AceptoTerminos = false;
                   usuario.PasswordTemp = true;

                   if (!string.IsNullOrEmpty(usuario.NombreUsuario))
                       if (UsuarioExiste(dctx, usuario))
                       {
                           Exception ex = new Exception("El nombre de usuario ya se encuentra registrado") { Source = "Usuario" };
                           throw ex;
                       }

                   if (!string.IsNullOrEmpty(usuario.Email))
                       if (!EmailDisponible(dctx, usuario))
                       {
                           Exception ex = new Exception("El correo electrónico proporcionado no se encuentra disponible") { Source = "Email" };
                           throw ex;
                       }

                   if (!string.IsNullOrEmpty(usuario.TelefonoReferencia))
                       if (!TelefonoDisponible(dctx, usuario))
                       {
                           Exception ex = new Exception("El Teléfono proporcionado no se encuentra disponible ") { Source = "Telefono" };
                           throw ex;
                       }

                   usuarioCtrl.Insert(dctx, usuario);
                   usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));
                   antusuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));
               }

               #endregion

               UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();

               #region registrar usuario privilegios

               //asignamos el perfil alumno a la lista de privilegios
               Perfil perfil = new Perfil { PerfilID = (int)EPerfil.ALUMNO };

               List<IPrivilegio> privilegios = new List<IPrivilegio>();
               privilegios.Add(perfil);

               usuarioPrivilegiosCtrl.InsertUsuarioEscolarPrivilegios(dctx, usuario, new Escuela() { EscuelaID = 1 }, new CicloEscolar() { CicloEscolarID=3}, privilegios);
               #endregion

               #region Licencia Alumno Escuela

               //Consultar licencia del alumno
               LicenciaEscuela licenciaEscuela=new LicenciaEscuela { LicenciaEscuelaID=1};
               LicenciaAlumno licenciaAlumno = licenciaEscuelaCtrl.RetrieveLicenciaAlumno(dctx, licenciaEscuela, alumno, usuario);
               UsuarioSocial antusuariosocial = null;
               if (licenciaAlumno != null && licenciaAlumno.LicenciaID != null)
               {
                   //Actualizar usuario social
                   UsuarioSocial usuarioSocial = licenciaAlumno.UsuarioSocial;

                   usuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, usuarioSocial));

                   UsuarioSocial nuevoUsrSocial = (UsuarioSocial)usuarioSocial.Clone();
                   nuevoUsrSocial.Estatus = true;
                   nuevoUsrSocial.ScreenName = antalumno.Nombre + " " + antalumno.PrimerApellido + ((antalumno.SegundoApellido == null || antalumno.SegundoApellido.Trim().Length == 0) ? "" : " " + antalumno.SegundoApellido);
                   nuevoUsrSocial.FechaNacimiento = antalumno.FechaNacimiento;
                   nuevoUsrSocial.Email = null;
                   nuevoUsrSocial.LoginName = usuarioSocial.LoginName;
                   usuarioSocialCtrl.Update(dctx,nuevoUsrSocial,usuarioSocial);

                   antusuariosocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx,new UsuarioSocial{UsuarioSocialID = nuevoUsrSocial.UsuarioSocialID}));
               }
               else
               {
                   //Registrar Usuario Social
                     UsuarioSocial usuarioSocial = new UsuarioSocial();
                     usuarioSocial.LoginName = antusuario.NombreUsuario;
                     usuarioSocial.ScreenName = antalumno.Nombre + " " + antalumno.PrimerApellido + ((antalumno.SegundoApellido == null || antalumno.SegundoApellido.Trim().Length == 0) ? "" : " " + antalumno.SegundoApellido);
                     usuarioSocial.FechaNacimiento = antalumno.FechaNacimiento;
                     usuarioSocial.Estatus = true;

                     usuarioSocialCtrl.Insert(dctx, usuarioSocial);
                     antusuariosocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, usuarioSocial));
               }

               //Crear el socialHub de no existir
               socialHubCtrl.InsertSocialHubUsuarioSocial(dctx, antusuariosocial);

               //Registro social hub Grupo social
               SocialHub socialHub = new SocialHub(); 
               socialHub.SocialProfile = antusuariosocial;
               socialHub.SocialProfileType = ESocialProfileType.USUARIOSOCIAL;
               DataSet ds = socialHubCtrl.RetrieveSocialHubUsuario(dctx, socialHub);
               socialHub = socialHubCtrl.LastDataRowToSocialHub(ds);  
               GrupoSocial grupoSocial = new GrupoSocial {GrupoSocialID=1};
        
               socialHubCtrl.InsertGrupoSocialSocialHub(dctx, grupoSocial, socialHub);
               
               //Asignar Grupo ciclo escolar 
               GrupoCicloEscolar grupoCicloEscolar = new GrupoCicloEscolar();
               grupoCicloEscolar.GrupoCicloEscolarID = new Guid("39f5a1b8-20f0-437c-aa13-78f5974d881a");
               grupoCicloEscolar.CicloEscolar = new CicloEscolar() { CicloEscolarID = 3 };
               grupoCicloEscolarCtrl.InsertAsignacionAlumno(dctx, alumno, grupoCicloEscolar);

               //Registrar licencia.
               licenciaEscuelaCtrl.InsertLicenciaAlumno(dctx, licenciaEscuela, antalumno, antusuario, antusuariosocial);
               
               //Registrar UsuarioGrupo -  para publicar
               UsuarioGrupo usuarioGrupo = new UsuarioGrupo();
               usuarioGrupo.FechaAsignacion = DateTime.Now;
               usuarioGrupo.Estatus = true;
               usuarioGrupo.EsModerador = false;
               usuarioGrupo.UsuarioSocial = antusuariosocial;

               usuarioGrupoCtrl.Insert(dctx, usuarioGrupo, grupoSocial);

               #endregion               
               dctx.CommitTransaction(firm);
           }
           catch (Exception ex)
           {
               dctx.RollbackTransaction(firm);
               if (dctx.ConnectionState == ConnectionState.Open)
                   dctx.CloseConnection(firm);

               POV.Logger.Service.LoggerHlp.Default.Error(this, ex);

               if (ex.Source == "Telefono" || ex.Source == "Email" || ex.Source=="Usuario")
                   throw new Exception("El " + ex.Source + " ya está en uso");

               if (ex.Source == "Alumno")
                   throw new Exception("El " + ex.Source + " ya está registrado");
               
              throw new SystemException("Ocurrió un error al agregar al estudiante");               
           }
           finally
           {
               if(dctx.ConnectionState==ConnectionState.Open)
                   dctx.CloseConnection(firm);
           }
           return alumno;
       }

       /// <summary>
       /// Consulta los alumnos asignados a la escuela para el ciclo escolar
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="escuela">Escuela a consultar</param>
       /// <param name="cicloEscolar">Ciclo escolar a consultar</param>
       /// <returns>Listado de alumnos asignados a la escuela para el ciclo escolar</returns>
       public List<Alumno> RetriveAlumnos(IDataContext dctx,Escuela escuela, CicloEscolar cicloEscolar)
       {
           if (escuela == null)
               throw new Exception("Escuela es requerido");
           if (cicloEscolar == null)
               throw new Exception("CicloEscolar es requerido");
           try
           {
               List<Alumno> lsalum = new List<Alumno>();
               List<Alumno> lsalumEscuelaCiclo = licenciaEscuelaCtrl.RetriveAlumnosEscuela(dctx, cicloEscolar, escuela);
               if (lsalumEscuelaCiclo.Any())
               {
                   foreach (Alumno alumno in lsalumEscuelaCiclo)
                   {
                       DataSet ds = alumnoCtrl.Retrieve(dctx, alumno);
                       if (ds.Tables[0].Rows.Count == 1)
                           lsalum.Add(alumnoCtrl.LastDataRowToAlumno(ds));
                   }
               }
               return lsalum;
           }
           catch (Exception ex)
           {
               Logger.Service.LoggerHlp.Default.Error(this, ex);
               throw;
           }
       } 
        
       /// <summary>
       /// Consulta los alumnos que pertenecen a la escuela y ciclo escolar proporcionado siempre que no pertenecen al GrupoCicloEscolar proporcionado
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="escuela">Escuela a consultar</param>
       /// <param name="cicloEscolar">Ciclo escolar a consultar</param>
       /// <param name="grupoCiclo"> GrupoCicloEscolar </param>
       /// <returns>Listado de alumnos asignados a la escuela para el ciclo escolar</returns>
       public List<Alumno> RetriveAlumnosNoAsignadosGrupo(IDataContext dctx, Escuela escuela, CicloEscolar cicloEscolar, GrupoCicloEscolar grupoCiclo)
       {
           if (escuela == null)
               throw new Exception("Escuela es requerido");
           if (cicloEscolar == null)
               throw new Exception("CicloEscolar es requerido");
           if (grupoCiclo == null)
               throw new Exception("GrupoCicloEscolar es requerido");

           try
           {
               List<Alumno> lsalumnos = RetriveAlumnos(dctx, escuela, cicloEscolar);
               List<Alumno> lsgr = grupoCicloEscolarCtrl.RetrieveAlumnos(dctx, grupoCiclo);

               return lsalumnos.Where(alumno => !lsgr.Exists(ls => ls.AlumnoID == alumno.AlumnoID)).ToList();
           }
           catch (Exception ex)
           {
               Logger.Service.LoggerHlp.Default.Error(this, ex);
               throw;
           }

       } 

       /// <summary>
       /// Identifica si el alumno se encuentra asignado,con licencia activa en una escuela diferente a la proporcionada, para el ciclo escolar solicitado
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="alumno"> Alumno a consultar</param>
       /// <param name="escuela">Escuela que solicita transferencia del alumno</param>
       /// <param name="cicloEscolar">Ciclo escolar actual</param>
       /// <returns>verdadero si el alumno se encuentra asignado</returns>
       public bool EsTransferenciaDeEscuela(IDataContext dctx, Alumno alumno, Escuela escuela, CicloEscolar cicloEscolar)
       {
           return false;
       }

       public bool UsuarioExiste(IDataContext dctx, Usuario usuario)
       {
           DataSet ds = usuarioCtrl.Retrieve(dctx, new Usuario { NombreUsuario = usuario.NombreUsuario, EsActivo = true });
           int index = ds.Tables[0].Rows.Count;
           if (index == 1 && usuario.NombreUsuario != null)
           {
               Usuario usr = usuarioCtrl.LastDataRowToUsuario(ds);
               if (usr.NombreUsuario == usuario.NombreUsuario && usr.UsuarioID!=null)
                   return true;
           }

           else if (index <= 0)
               return false;
           return false;
       }

       public bool EmailDisponible(IDataContext dctx,Usuario usuario)
        {
            if(usuario==null || string.IsNullOrEmpty(usuario.Email))
                throw new ArgumentException("Email requerido","usuario");

            DataSet ds = usuarioCtrl.Retrieve(dctx, new Usuario {Email = usuario.Email, EsActivo = true});
            int index = ds.Tables[0].Rows.Count;
            if (index == 1 && usuario.UsuarioID!=null)
            {
                Usuario usr = usuarioCtrl.LastDataRowToUsuario(ds);
                return usr.UsuarioID == usuario.UsuarioID;
            }

            if (index <= 0)
                return true;

            return false;
        }

       public bool TelefonoDisponible(IDataContext dctx,Usuario usuario)
         {
             if (usuario == null || string.IsNullOrEmpty(usuario.TelefonoReferencia))
                 throw new ArgumentException("Teléfono requerido", "usuario");

             DataSet ds = usuarioCtrl.Retrieve(dctx, new Usuario { TelefonoReferencia = usuario.TelefonoReferencia });
             int index = ds.Tables[0].Rows.Count;
             if (index == 1 && usuario.UsuarioID!=null)
             {
                 Usuario usr = usuarioCtrl.LastDataRowToUsuario(ds);
                 return usr.UsuarioID == usuario.UsuarioID;
             }

             if (index <= 0)
                 return true;

             return false;
         }
    }
   
}
