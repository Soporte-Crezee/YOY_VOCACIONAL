using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Web.PortalTutor.Helper;
using POV.CentroEducativo.BO;

namespace POV.Web.PortalTutor.Tutores
{
    public partial class TutoresPage : System.Web.UI.MasterPage
    {  
        protected void Page_Load(object sender, EventArgs e)
        {
            lblAnio.Text = DateTime.Now.ToString("yyyy");
        }          
    }
}