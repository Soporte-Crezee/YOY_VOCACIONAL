using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.Universidades.Implements;
using POV.Core.Universidades.Interfaces;

namespace POV.Web.PortalUniversidad.Auth
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IAccountService accountCtrl = new AccountService();
            accountCtrl.Logout();
        }
    }
}