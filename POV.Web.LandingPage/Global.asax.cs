using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using POV.Web.LandingPage;

namespace POV.Web.LandingPage
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Código que se ejecuta al cerrarse la aplicación

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando se produce un error sin procesar

        }
    }
}
