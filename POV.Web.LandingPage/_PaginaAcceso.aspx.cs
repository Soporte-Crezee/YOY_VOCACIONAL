using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.LandingPage
{
    public partial class PaginaAcceso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            portalSocial.HRef = System.Configuration.ConfigurationManager.AppSettings["POVURLPortalSocial"];
            portalOrientadores.HRef = System.Configuration.ConfigurationManager.AppSettings["POVURLPortalOrientadores"];
            portalOperaciones.HRef = System.Configuration.ConfigurationManager.AppSettings["POVURLPortalOperaciones"];
            //blog.HRef = System.Configuration.ConfigurationManager.AppSettings["POVURLBlog"];
            //portalAdministracion.HRef = System.Configuration.ConfigurationManager.AppSettings["POVPortalAdministracion"];
            portalTutor.HRef = System.Configuration.ConfigurationManager.AppSettings["POVPortalTutor"];
            portalUniversidad.HRef = System.Configuration.ConfigurationManager.AppSettings["POVPortalUniversidad"];
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            if (txtMensaje.Text != string.Empty)
            {
                string telefono = "529992190747";
                string mensaje = txtMensaje.Text.Trim();
                string url = "https://api.whatsapp.com/send?phone=" + telefono + "&text=" + mensaje;
                Response.Redirect(url, true);
            }
        }
    }
}