using POV.Administracion.BO;
using POV.CentroEducativo.BO;
using POV.Core.Universidades.Implements;
using POV.Core.Universidades.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalUniversidad.Checkout
{
    public partial class CheckoutError : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;

        #region Variables de session
        private string Session_errorPaypal
        {
            get { return (string)this.Session["errorPaypal"]; }
            set { this.Session["errorPaypal"] = value; }
        }
        private string session_compraActiva
        {
            get { return Session["session_compraActiva"] as string; }
            set { Session["session_compraActiva"] = value; }
        }
        private string session_token
        {
            get { return Session["token"] as string; }
            set { Session["token"] = value; }
        }
        #endregion
        #region QueryString
        private string QS_ErrorCode
        {
            get { return this.Request.QueryString["ErrorCode"]; }
        }

        private string QS_Desc
        {
            get { return this.Request.QueryString["Desc"]; }
        }

        private string QS_Desc2
        {
            get { return this.Request.QueryString["Desc2"]; }
        }

        private string QS_ErrorEx
        {
            get { return this.Request.QueryString["ErrorEx"]; }
        }
        #endregion

        public CheckoutError()
        {
            userSession = new UserSession();
            redirector = new Redirector();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin())
                {
                    if (string.IsNullOrEmpty(Session_errorPaypal))
                        redirector.GoToHomePage(true);

                    if ((string.IsNullOrEmpty(QS_ErrorCode) && string.IsNullOrEmpty(QS_Desc) && string.IsNullOrEmpty(QS_Desc2) && string.IsNullOrEmpty(QS_ErrorEx)))
                        redirector.GoToHomePage(true);

                    if (string.IsNullOrEmpty(session_compraActiva))
                    {
                        session_token = string.Empty;
                        redirector.GoToHomePage(true);
                    }

                    session_token = string.Empty;
                    session_compraActiva = string.Empty;
                    Session_errorPaypal = string.Empty;
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }
        protected void Continue_Click(object sender, EventArgs e)
        {
            redirector.GoToHomePage(true); 
        }
    }
}