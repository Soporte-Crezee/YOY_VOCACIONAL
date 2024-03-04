using POV.Core.PadreTutor.Implements;
using System;
namespace POV.Web.PortalTutor
{
    public partial class DefaultSite : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.HplLogout.NavigateUrl = UrlHlp.GetLogoutURL();
        }
    }
}