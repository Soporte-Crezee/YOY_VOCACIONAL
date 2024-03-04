using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.LandingPage
{
    public partial class RequisitosUniversidad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            regresar.HRef = System.Configuration.ConfigurationManager.AppSettings["POVPortalUniversidad"];
            enviarCorreo.HRef = System.Configuration.ConfigurationManager.AppSettings["POVURLMailToUniversidad"];
        }
 
    }
}