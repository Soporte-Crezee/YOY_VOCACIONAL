using POV.Seguridad.BO;
using POV.Web.Administracion.AppCode.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.Administracion
{
    public partial class DefaultSite : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblAnio.Text = DateTime.Now.ToString("yyyy");
            this.Page.Header.DataBind();

        }

    }
}