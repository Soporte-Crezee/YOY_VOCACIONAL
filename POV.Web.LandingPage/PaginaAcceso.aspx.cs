using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.LandingPage
{
    public partial class PaginaAcceso1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            A4.HRef = System.Configuration.ConfigurationManager.AppSettings["POVURLPortalSocial"];
            A1.HRef = System.Configuration.ConfigurationManager.AppSettings["POVURLPortalOrientadores"];
            //portalOperaciones.HRef = System.Configuration.ConfigurationManager.AppSettings["POVURLPortalOperaciones"];
            //blog.HRef = System.Configuration.ConfigurationManager.AppSettings["POVURLBlog"];
            //portalAdministracion.HRef = System.Configuration.ConfigurationManager.AppSettings["POVPortalAdministracion"];
            A2.HRef = System.Configuration.ConfigurationManager.AppSettings["POVPortalTutor"];
            A3.HRef = System.Configuration.ConfigurationManager.AppSettings["POVPortalUniversidad"];
        }
    }
}