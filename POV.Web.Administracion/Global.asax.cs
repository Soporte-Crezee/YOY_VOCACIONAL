using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using log4net;
using log4net.Config;
namespace POV.Web.Administracion
{
    public class Global : HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Global));
        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
            XmlConfigurator.Configure();

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }
        void RegisterRoutes(RouteCollection routes)
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