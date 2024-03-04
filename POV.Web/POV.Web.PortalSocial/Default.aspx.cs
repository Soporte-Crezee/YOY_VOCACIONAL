using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;

namespace POV.Web.PortalSocial
{
    public partial class Default : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;

        public Default()
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
                    if (userSession.IsAlumno())
                    {
						redirector.GoToHomeAlumno(true);
                    }
                    else if (userSession.CurrentEscuela != null)
                        redirector.GoToHomeDocente(true);
                    else
                        redirector.GoToCambiarEscuela(true);
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }
    }
}