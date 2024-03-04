using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Web.PortalSocial.AppCode;
using POV.Core.RedSocial.Implement;
namespace POV.Web.PortalSocial
{
    public partial class DefaultSite : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Header.DataBind();
            this.HplLogout.NavigateUrl = UrlHelper.GetLogoutURL();
        }
    }
}