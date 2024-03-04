using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.Universidades.Implements;
using POV.Core.Universidades.Interfaces;
using Framework.Base.DataAccess;
using POV.Web.PortalUniversidad.Helper;
using System.Configuration;
using System.Web.Security;

namespace POV.Web.PortalUniversidad.Auth
{
    public partial class Login : System.Web.UI.Page
    {

        private IAccountService accountCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private IUserSession userSession;
        private IRedirector redirector;

        #region QueryString
        private string QS_ReturnUrl
        {
            get { return this.Request.QueryString["ReturnUrl"]; }
        }       
        #endregion

        #region Session_variables
        private string session_ReturnUrl
        {
            get
            {
                return Session["session_ReturnUrl"] as string;
            }
            set
            {
                Session["session_ReturnUrl"] = value;
            }
        }       
        #endregion

        public Login()
        {
            accountCtrl = new AccountService();
            userSession = new UserSession();
            redirector = new Redirector();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblFechaAnio.Text = DateTime.Now.ToString("yyyy");
                if (userSession.IsLogin())
                {
                    redirector.GoToHomePage(true);
                }
            }
        }

        protected void BtnEntrar_Click(object sender, EventArgs e)
        {
            try
            {

                bool bValid = true;
                clearErrorInputs();

                //Requeridos
                if (String.IsNullOrEmpty(TxtNombre.Text.Trim()))
                {
                    LblLoginFail.Text = "El usuario es requerido.";
                    TxtNombre.CssClass += " error";
                    bValid = false;
                }
                if (bValid && string.IsNullOrEmpty(TxtPassword.Text.Trim()))
                {
                    LblLoginFail.Text = "La contraseña es requerida.";
                    TxtPassword.CssClass += " error";
                    bValid = false;
                }
                //longitud
                if (bValid && TxtNombre.Text.Trim().Length > 50)
                {
                    LblLoginFail.Text = "El usuario no debe ser mayor a 50 caracteres.";
                    TxtNombre.CssClass += " error";
                    bValid = false;
                }
                if (bValid && TxtPassword.Text.Trim().Length > 50)
                {
                    LblLoginFail.Text = "La contraseña no debe ser mayor a 50 caracteres.";
                    TxtPassword.CssClass += " error";
                    bValid = false;
                }

                if (bValid)
                {
                    string sError = accountCtrl.Login(dctx, TxtNombre.Text.Trim(), TxtPassword.Text, QS_ReturnUrl);

                    if (!string.IsNullOrEmpty(sError))
                    {
                        LblLoginFail.Text = sError;
                        session_ReturnUrl = string.Empty;
                    }
                    else
                    {
                        if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                        {
                            FormsAuthentication.RedirectFromLoginPage(TxtNombre.Text.Trim(), false);
                        }
                      
                        if (!string.IsNullOrEmpty(QS_ReturnUrl))
                        {
                            session_ReturnUrl = QS_ReturnUrl;
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                ShowMessage(ex.Message);

            }

        }
        private void ShowMessage(string p)
        {
            LblLoginFail.Text = p;
        }

        private void clearErrorInputs()
        {
            LblLoginFail.Text = "";
            TxtNombre.CssClass = "form-control";
            TxtPassword.CssClass = "form-control";
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            Response.Redirect(ConfigurationManager.AppSettings["POVUrlLandingPageRequisitos"], true);
        }
    }
}