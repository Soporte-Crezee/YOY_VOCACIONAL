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
    public partial class CheckoutCancel : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;

        #region Session_variables
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

        public CheckoutCancel()
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
                    if (string.IsNullOrEmpty(session_compraActiva))
                    {
                        session_token = string.Empty;
                        redirector.GoToHomePage(true);
                    }

                    session_token = string.Empty;
                    session_compraActiva = string.Empty;
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