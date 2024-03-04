using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.Administracion.Implements;
using POV.Core.Administracion.Interfaces;
using Framework.Base.DataAccess;
using POV.Web.Administracion.Helper;

namespace POV.Web.Administracion.Auth
{
    public partial class Login : System.Web.UI.Page
    {

        private IAccountService accountCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private IUserSession userSession;
        private IRedirector redirector;

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
                    string sError = accountCtrl.Login(dctx, TxtNombre.Text.Trim(), TxtPassword.Text);

                    if (!string.IsNullOrEmpty(sError))
                    {
                        LblLoginFail.Text = sError;
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
    }
}