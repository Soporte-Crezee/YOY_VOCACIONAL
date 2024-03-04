using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;

namespace POV.Web.PortalSocial.ContenidosDigitales
{
    public partial class BuscarContenidos : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;

        public BuscarContenidos() 
        {
            userSession = new UserSession();
            redirector = new Redirector();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //usuario valido si: esta logueado, no es alumno y tiene seleccionado escuela
                bool validUser = userSession.IsLogin() && !userSession.IsAlumno() && userSession.CurrentEscuela != null;
                if (!validUser)
                {
                    redirector.GoToHomePage(true);
                }
            }
        }
    }
}