using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;

namespace POV.Web.PortalSocial.Auth
{
    public partial class Logout : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IAccountService accountService;
        private IRedirector redirector;

        public Logout()
        {
            accountService = new AccountService( );
            userSession = new UserSession( );
            redirector = new Redirector( );                
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            accountService.Logout();
        }
    }
}