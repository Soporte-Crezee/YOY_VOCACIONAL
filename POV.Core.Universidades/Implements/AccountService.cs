using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Base.DataAccess;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Services;
using POV.CentroEducativo.Service;
using System.Security.Cryptography;
using System.Text;
using POV.Licencias.Service;
using POV.Licencias.BO;
using POV.Core.Universidades.Interfaces;

namespace POV.Core.Universidades.Implements
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
        private UniversidadCtrl universidadCtrl;
        private UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl;
        private ModuloFuncionalCtrl modulosFuncionalCtrl;
        private CicloEscolarCtrl cicloEscolarCtrl;
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
            cicloEscolarCtrl = new CicloEscolarCtrl();
        }

        public string Login(IDataContext dctx, string username, string password, string returnUrl)
        {
            Usuario usuario = new Usuario { NombreUsuario = username.Trim() };

            DataSet dsUsuario = usuarioCtrl.Retrieve(dctx, usuario);

            if (dsUsuario.Tables[0].Rows.Count > 0) // si existe el usuario con el username
            {
                usuario = usuarioCtrl.LastDataRowToUsuario(dsUsuario);

                if (!(bool)usuario.EsActivo)
                    return "Tu cuenta está desactivada.";
                //validacion del password
                Byte[] bPassword = EncryptHash.SHA1encrypt(password);

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

                            Perfil perfilAutoridad = usuarioPrivilegios.Perfiles.FirstOrDefault(item => item.PerfilID == (int)EPerfil.UNIVERSIDAD);

                            if (perfilAutoridad != null)
                            {
                                userSession.PrivilegiosAutoridadEstatal = usuarioPrivilegios;
                                esAutoridad = true;
                                break;
                            }
                        }
                    }

                    userSession.EsAutoridadEstatal = esAutoridad;

                    #endregion

                    List<LicenciaEscuela> licenciasEscuela = licenciaEscuelaCtrl.RetrieveLicencia(dctx, usuario);
                    List<LicenciaEscuela> licenciasUniversidad = new List<LicenciaEscuela>();

                    if (licenciasEscuela.Count <= 0 && !userSession.EsAutoridadEstatal)
                        return "No tienes licencias activas o han expirado.";

                    if (licenciasEscuela.Count(item => (bool)item.Activo) <= 0 && !userSession.EsAutoridadEstatal)
                        return "No tienes licencias activas o han expirado.";

                    #region seleccionar licencias
                    foreach (LicenciaEscuela licenciaEscuela in licenciasEscuela) // se recorre la lista para encontrar las de la universidad.
                    {

                        //buscamos la licencia de universidad
                        ALicencia licencia = licenciaEscuela.ListaLicencia.Find(item => (bool)item.Activo && item.Tipo == ETipoLicencia.UNIVERSIDAD);

                        if (licencia == null)
                            continue;
                        UsuarioPrivilegios usuarioPrivilegios = new UsuarioEscolarPrivilegios();
                        usuarioPrivilegios.Usuario = usuario;
                        (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela = licenciaEscuela.Escuela;
                        (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar = licenciaEscuela.CicloEscolar;

                        DataSet dsPrivilegioUniversidad = usuarioPrivilegiosCtrl.Retrieve(dctx, usuarioPrivilegios);
                        if (dsPrivilegioUniversidad.Tables[0].Rows.Count < 1)
                            continue;

                        usuarioPrivilegios = usuarioPrivilegiosCtrl.RetrieveComplete(dctx, usuarioPrivilegios);

                        if (usuarioPrivilegios.UsuarioAccesos.Count < 1)
                            continue;
                        Boolean permitido = false;
                        List<Permiso> permisos = usuarioPrivilegios.GetPermisos();

                        foreach (Permiso per in permisos)
                        {
                            if (per.PermisoID == (int)EPermiso.PUACCESO)
                            {
                                permitido = true;
                                break;
                            }
                        }
                        if (permitido == true)
                        {
                            //solo insertamos si el usuario tiene permiso de acceso en red administrativa
                            licenciasUniversidad.Add(licenciaEscuela);
                        }
                        else
                        {
                            return "No se tienen los privilegios correspondientes para acceder al portal";
                        }
                    }
                    #endregion
                    //si no tiene licencias y no es autoridad
                    if (licenciasUniversidad.Count < 1 && !userSession.EsAutoridadEstatal)
                        return "No tienes licencias para acceder al portal.";

                    userSession.CurrentUser = usuario;
                    userSession.LoggedIn = true;

                    //si tiene licencias universidad, se carga en sesion sus licencias
                    if (licenciasUniversidad.Count > 0)
                    {
                        //se agregan los datos para la universidad en session
                        userSession.LicenciasUniversidad = licenciasUniversidad;
                        universidadCtrl = new UniversidadCtrl(null);
                        LicenciaUniversidad licenciaUniversidad = (LicenciaUniversidad)licenciasEscuela.First().ListaLicencia.First();
                        Universidad universidad = new Universidad { UniversidadID = licenciaUniversidad.Universidad.UniversidadID };
                        universidad = universidadCtrl.RetrieveWithRelationship(universidad, false).FirstOrDefault();
                        userSession.CurrentUniversidad = universidad;
                        
                        //Licencias de escuela Única
                        userSession.CurrentEscuela = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciasUniversidad.First().Escuela));
                        userSession.CurrentCicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, licenciasUniversidad.First().CicloEscolar));
                        var licenciaCurrenEscuela = userSession.LicenciasUniversidad.Find(x => x.Escuela.EscuelaID == userSession.CurrentEscuela.EscuelaID);
                        userSession.Contrato = licenciaCurrenEscuela.Contrato;

                        //Privilegios de escuela
                        UsuarioPrivilegios usuarioPrivilegios = new UsuarioEscolarPrivilegios();
                        usuarioPrivilegios.Usuario = usuario;
                        (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela = licenciasUniversidad.First().Escuela;
                        (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar = licenciasUniversidad.First().CicloEscolar;
                        usuarioPrivilegios = usuarioPrivilegiosCtrl.RetrieveComplete(dctx, usuarioPrivilegios);
                        userSession.CurrentPrivilegiosUniversidad = usuarioPrivilegios;
                        userSession.ModulosFuncionales = licenciaEscuelaCtrl.RetrieveModulosFuncionalesLicenciaEscuela(dctx, licenciaCurrenEscuela);
                    }

                    //Perfil de Tipo escuela
                    userSession.CurrentPerfil = new Perfil { PerfilID = (int)EPerfil.UNIVERSIDAD };

                    if (string.IsNullOrEmpty(returnUrl))
                        redirector.GoToHomePage(false);
                    else
                        HttpContext.Current.Response.Redirect(returnUrl, false);

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
