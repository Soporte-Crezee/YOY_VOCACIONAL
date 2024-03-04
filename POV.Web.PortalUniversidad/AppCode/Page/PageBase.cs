using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using POV.Core.Universidades.Implements;
using POV.Core.Universidades.Interfaces;
using POV.Seguridad.BO;

namespace POV.Web.PortalUniversidad.AppCode.Page
{
    public abstract class PageBase : System.Web.UI.Page
    {

        protected IUserSession userSession;
        protected IRedirector redirector;

        // Constructor base
        public PageBase()
        {
            userSession = new UserSession();
            redirector = new Redirector();
        }

        //Inicializa la pagina
        protected void Page_Init(object sender, EventArgs e)
        {
            //validamos la autenticacion del usuario
            AuthenticateUser();
            //validamos la autorizacion del usuario
            AuthorizeUser();
        }

        protected void AuthenticateUser()
        {
            if (!userSession.IsLogin())
                redirector.GoToLoginPage(true);

            else if (userSession.CurrentPerfil != null && userSession.CurrentPerfil.PerfilID == (int)EPerfil.UNIVERSIDAD)
                if (userSession.CurrentCicloEscolar == null || userSession.CurrentEscuela == null)
                    redirector.GoToSeleccionarEscuela(true);

        }

        protected abstract void AuthorizeUser();
    }
}