using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Base.DataAccess;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.Core.HerramientasDocente.Interfaces;
using POV.CentroEducativo.BO;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.CentroEducativo.Service;
using System.Security.Cryptography;
using System.Text;
using POV.Licencias.Service;
using POV.Licencias.BO;

namespace POV.Core.HerramientasDocente.Implement
{
    public class AccountService:IAccountService
    {   
        #region Members
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private AlumnoCtrl alumnoCtrl;
        private DocenteCtrl docenteCtrl;
        private EscuelaCtrl escuelaCtrl;
        private UsuarioCtrl usuarioCtrl;
        private SocialHubCtrl socialHubCtrl;      
        private IUserSession userSession;
        private IWebContext webContext;
        private IRedirector redirector;
        private UsuarioSocialCtrl usuarioSocialCtrl;
        private GrupoSocialCtrl grupoSocialCtrl;
        private RSACryptoServiceProvider sec;
        private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;

        #endregion

        public  AccountService()
        {
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            userSession = new UserSession();
            webContext = new WebContext();
            redirector = new Redirector();
            socialHubCtrl = new SocialHubCtrl();
            grupoSocialCtrl = new GrupoSocialCtrl();
            usuarioSocialCtrl = new UsuarioSocialCtrl();
            usuarioCtrl = new UsuarioCtrl();
            sec = new RSACryptoServiceProvider();
            alumnoCtrl = new AlumnoCtrl();
            docenteCtrl = new DocenteCtrl();
            escuelaCtrl = new EscuelaCtrl();
            grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();


        }

        public string Login(IDataContext dctx, string username, string password, byte[] binaryPassword = null )
        {       
            
             
                return "Usuario y contraseña incorrectos.";
            
        }

        public void AutoLogin(IDataContext dctx, string username) 
        {
            Usuario usuario = new Usuario { NombreUsuario = username.Trim() };

            DataSet dsUsuario = usuarioCtrl.Retrieve(dctx, usuario);
            if (dsUsuario.Tables[0].Rows.Count > 0)
            {
                usuario = usuarioCtrl.LastDataRowToUsuario(dsUsuario);
            }
            else 
            {
                //TODO: ERROR
            }
        }

        /// <summary>
        /// Finallizar la session y redireccionar a la pagina de logueo
        /// </summary>
        public void Logout()
        {
            
            userSession.LoggedIn = false;
            userSession.CurrentUser = null;
            
            userSession.CurrentDocente = null;
            userSession.Contrato = null;
            userSession.CurrentCicloEscolar = null;
             
            redirector.GoToLoginPage( false );
        }
    }
}
