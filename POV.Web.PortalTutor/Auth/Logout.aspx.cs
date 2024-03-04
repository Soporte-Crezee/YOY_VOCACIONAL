using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.PadreTutor.Implements;
using POV.Core.PadreTutor.Interfaces;

namespace POV.Web.PortalTutor.Auth
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