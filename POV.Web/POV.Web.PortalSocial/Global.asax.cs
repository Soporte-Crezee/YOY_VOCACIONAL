using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using log4net;
using log4net.Config;


namespace POV.Web.PortalSocial
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Global));

        protected void Application_Start(object sender, EventArgs e)
        {
            //RegisterRoutes(RouteTable.Routes);
            XmlConfigurator.Configure();
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            
            routes.Ignore("{resource}.axd/{*pathInfo}");

            routes.MapPageRoute("Error", "error", "~/Error.aspx");

            routes.MapPageRoute("Login", "login", "~/Auth/Login.aspx");
            routes.MapPageRoute("Logout", "logout", "~/Auth/Logout.aspx");
            routes.MapPageRoute("Default", "", "~/Default.aspx");
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
          
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}