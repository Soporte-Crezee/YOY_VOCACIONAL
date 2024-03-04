using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.LandingPage
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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