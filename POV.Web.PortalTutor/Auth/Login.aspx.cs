using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Seguridad.BO;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using Framework.Base.DataAccess;
using POV.Logger.Service;
using System.Configuration;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Services;
using POV.Modelo.BO;
using POV.CentroEducativo.Service;
using POV.Expediente.Service;
using POV.Licencias.BO;
using POV.Prueba.BO;
using POV.Expediente.BO;
using POV.Licencias.Service;
using System.Collections;
using POV.Modelo.Context;
using System.Text.RegularExpressions;
using POV.Core.PadreTutor.Interfaces;
using POV.Web.PortalTutor.Helper;
using POV.Core.PadreTutor.Implements;

namespace POV.Web.PortalTutor.Auth
{
    public partial class Login : System.Web.UI.Page
    {
        private IUserSession userSession;
        private AccountService accountService;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;

        public Login()
        {
            userSession = new UserSession();
            accountService = new AccountService();
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
                userSession = new UserSession();

                bool bValid = true;
                clearErrorInputs();
                accountService = new AccountService();

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
                    string sError = accountService.Login(dctx, TxtNombre.Text.Trim(), TxtPassword.Text);
                    if (!string.IsNullOrEmpty(sError))
                    {
                        LblLoginFail.Text = sError;
                        //txtLoginFail.Text = "error";
                        LoggerHlp.Default.Info(this, sError);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                LoggerHlp.Default.Error(this, ex);
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
            //Response.Redirect(ConfigurationManager.AppSettings["POVNuevoTutor"]);
            Response.Redirect(ConfigurationManager.AppSettings["CAMPUSNuevoPadre"]);
        }
    }
}