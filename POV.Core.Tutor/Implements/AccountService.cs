using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Base.DataAccess;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.CentroEducativo.BO;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.CentroEducativo.Service;
using System.Security.Cryptography;
using System.Text;
using POV.Licencias.Service;
using POV.Licencias.BO;
using POV.Localizacion.Service;
using POV.Expediente.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Expediente.BO;
using POV.Modelo.BO;
using POV.Core.PadreTutor.Interfaces;
using POV.CentroEducativo.Services;

namespace POV.Core.PadreTutor.Implements
{
    public class AccountService : IAccountService
    {
        #region Members
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private EscuelaCtrl escuelaCtrl;
        private UsuarioCtrl usuarioCtrl;
        private IUserSession userSession;
        private IWebContext webContext;
        private IRedirector redirector;
        private RSACryptoServiceProvider sec;
        private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;
        private TutorCtrl tutorCtrl;
        private UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl;
        private ModuloFuncionalCtrl modulosFuncionalCtrl;
        #endregion

        public AccountService()
        {
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            userSession = new UserSession();
            webContext = new WebContext();
            redirector = new Redirector();
            usuarioCtrl = new UsuarioCtrl();
            sec = new RSACryptoServiceProvider();
            escuelaCtrl = new EscuelaCtrl();
            grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
            usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();
            modulosFuncionalCtrl = new ModuloFuncionalCtrl();
        }

        public string Login(IDataContext dctx, string username, string password, byte[] binaryPassword = null)
        {
            Usuario usuario = new Usuario { NombreUsuario = username.Trim() };

            DataSet dsUsuario = usuarioCtrl.Retrieve(dctx, usuario);

            if (dsUsuario.Tables[0].Rows.Count > 0) // si existe el usuario con el username
            {
                usuario = usuarioCtrl.LastDataRowToUsuario(dsUsuario);

                if (!(bool)usuario.EsActivo)
                    return "Tu cuenta está desactivada.";
                //validacion del password
                Byte[] bPassword = (!string.IsNullOrEmpty(password) && binaryPassword==null)?EncryptHash.SHA1encrypt(password):binaryPassword;

                if (EncryptHash.compareByteArray(usuario.Password, bPassword)) // password correcto
                {
                    Usuario usuarioAux = usuario.Clone() as Usuario;
                    usuarioAux.FechaUltimoAcceso = DateTime.Now;
                    usuarioCtrl.Update(dctx, usuarioAux, usuario);

                    #region privilegios de autoridad estatal
                    DataSet dsPrivilegios = usuarioPrivilegiosCtrl.Retrieve(dctx, new UsuarioPrivilegios { Usuario = usuario });

                    bool esAutoridad = false;

                    if (dsPrivilegios.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drPrivilegios in dsPrivilegios.Tables[0].Rows)
                        {
                            UsuarioPrivilegios usuarioPrivilegios = usuarioPrivilegiosCtrl.DataRowToUsuarioPrivilegios(drPrivilegios);

                            usuarioPrivilegios = usuarioPrivilegiosCtrl.RetrieveComplete(dctx, usuarioPrivilegios);

                            Perfil perfilAutoridad = usuarioPrivilegios.Perfiles.FirstOrDefault(item => item.PerfilID == (int)EPerfil.TUTOR);

                            if (perfilAutoridad != null)
                            {
                                userSession.PrivilegiosAutoridadEstatal = usuarioPrivilegios;
                                userSession.CurrentPerfil=perfilAutoridad;
                                esAutoridad = true;
                                break;
                            }
                        }
                    }

                    userSession.EsAutoridadEstatal = esAutoridad;

                    #endregion

                    List<LicenciaEscuela> licenciasEscuela = licenciaEscuelaCtrl.RetrieveLicencia(dctx, usuario);
                    List<LicenciaEscuela> licenciasTutor = new List<LicenciaEscuela>();
                    
                    if (licenciasEscuela.Count <= 0 && !userSession.EsAutoridadEstatal)
                        return "No tienes licencias activas o han expirado.";

                    if (licenciasEscuela.Count(item => (bool)item.Activo) <= 0 && !userSession.EsAutoridadEstatal)
                        return "No tienes licencias activas o han expirado.";

                    #region seleccionar licencias
                    foreach (LicenciaEscuela licenciaEscuela in licenciasEscuela) // se recorre la lista para encontrar las del tutor.
                    {

                        //buscamos la licencia de tutor
                        ALicencia licencia = licenciaEscuela.ListaLicencia.Find(item => (bool)item.Activo && item.Tipo == ETipoLicencia.TUTOR);

                        if (licencia == null)
                            continue;
                        UsuarioPrivilegios usuarioPrivilegios = new UsuarioEscolarPrivilegios();
                        usuarioPrivilegios.Usuario = usuario;
                        (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela = licenciaEscuela.Escuela;
                        (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar = licenciaEscuela.CicloEscolar;
                        
                        DataSet dsPrivilegioTutor = usuarioPrivilegiosCtrl.Retrieve(dctx, usuarioPrivilegios);
                        if (dsPrivilegioTutor.Tables[0].Rows.Count < 1)
                            continue;

                        usuarioPrivilegios = usuarioPrivilegiosCtrl.RetrieveComplete(dctx, usuarioPrivilegios);

                        if (usuarioPrivilegios.UsuarioAccesos.Count < 1)
                            continue;
                        Boolean permitido = false;
                        List<Permiso> permisos = usuarioPrivilegios.GetPermisos();
                        userSession.CurrentPrivilegiosTutor = usuarioPrivilegios;
                        foreach (Permiso per in permisos)
                        {
                            if (per.PermisoID == (int)EPermiso.PTAACCESO)
                            {                                
                                permitido = true;
                                break;
                            }
                        }
                        if (permitido == true)
                        {
                            //solo insertamos si el usuario tiene permiso de acceso en red administrativa
                            userSession.Contrato = licenciaEscuela.Contrato;
                            userSession.CurrentEscuela = licenciaEscuela.Escuela;
                            userSession.CurrentCicloEscolar = licenciaEscuela.CicloEscolar;                            
                            licenciasTutor.Add(licenciaEscuela);                            
                            //break;
                        }
                        else
                        {
                            return "No se tienen los privilegios correspondientes para acceder al portal";
                        }

                    }
                    #endregion
                    //si no tiene licencias y no es autoridad
                    if (licenciasTutor.Count < 1 && !userSession.EsAutoridadEstatal)
                        return "No tienes licencias para acceder al portal.";



                    userSession.CurrentUser = usuario;
                    userSession.LoggedIn = true;

                    //si tiene licencias tutor, se carga en sesion sus licencias
                    if (licenciasTutor.Count > 0)
                    {
                        //se agregan los datos para el tutor en session
                        userSession.LicenciasTutor = licenciasTutor;
                        tutorCtrl = new TutorCtrl(null);
                        LicenciaTutor licenciaTutor = (LicenciaTutor)licenciasEscuela[0].ListaLicencia[0];
                        Tutor tutor = new Tutor { TutorID = licenciaTutor.Tutor.TutorID };
                        tutor = tutorCtrl.Retrieve(tutor, false).ToList()[0];
                        userSession.CurrentTutor = tutor;
                        //Modulos Funcionales
                    }

                    
                    if (!(bool)userSession.CurrentTutor.EstatusIdentificacion)
                    {
                        redirector.GoToConfirmarTutor(false);
                        return "";
                    }

                    //si esta confirmado preguntar si acepto terminos

                    if (!(bool)usuario.AceptoTerminos)
                    {
                        redirector.GoToAceptarTerminos(false);
                    }
                    else
                    {
                        //redireccionar a página de inicio del tutor
                        redirector.GoToHomePage(true);
                    }


                    return "";

                }
                else
                    return "Usuario y contraseña incorrectos";
            }
            else
                return "Usuario y contraseña incorrectos.";

        }

        public void Logout()
        {
            userSession.Logout();
            redirector.GoToLoginPage(false);
        }
    }
}
