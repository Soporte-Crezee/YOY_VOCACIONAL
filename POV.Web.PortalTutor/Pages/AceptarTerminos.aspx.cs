using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.Logger.Service;
using POV.Core.PadreTutor.Interfaces;
using POV.Core.PadreTutor.Implements;
using POV.Web.PortalTutor.Helper;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Services;

namespace POV.Web.PortalSocial.CuentaUsuario
{
    public partial class AceptarTerminos : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;
        private UsuarioCtrl usuarioCtrl;
        private AccountService accountService;
        private TutorCtrl tutorCtrl;
        public AceptarTerminos()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            accountService = new AccountService();
            usuarioCtrl = new UsuarioCtrl();
            tutorCtrl = new TutorCtrl(null);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.CurrentUser != null && userSession.LoggedIn)
                {
                    Tutor tutor = tutorCtrl.Retrieve(new Tutor { TutorID = userSession.CurrentTutor.TutorID }, true).FirstOrDefault();
                    if (tutor.EstatusIdentificacion.Value)
                    {
                        Usuario usuario = usuarioCtrl.RetrieveComplete(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID });
                        if (!(bool)usuario.AceptoTerminos)
                        {
                            hdnRefTerminos.Value = usuario.Termino.Cuerpo;
                        }
                        else 
                        {
                            redirector.GoToHomePage(true);
                        }
                    }
                    else 
                    {
                        redirector.GoToConfirmarTutor(true);
                    }
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
            else 
            {
                if (!userSession.IsLogin())
                {
                    redirector.GoToLoginPage(true);
                }
            }
        }

        protected void BtnAceptarTerminos_OnClick(object sender, EventArgs e)
        {
            bool isOk = false;
            try
            {
                Usuario usuario = usuarioCtrl.RetrieveComplete(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID });
                Usuario copyUsuario = (Usuario)usuario.Clone();
                copyUsuario.AceptoTerminos = true;

                usuarioCtrl.Update(dctx, copyUsuario, usuario);

                userSession.CurrentUser = usuarioCtrl.RetrieveComplete(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID });
                isOk = true;
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            if (isOk)
                redirector.GoToLoginPage(true);
        }

        protected void BtnRechazarTerminos_OnClick(object sender, EventArgs e)
        {
            accountService.Logout();
        }
    }
}