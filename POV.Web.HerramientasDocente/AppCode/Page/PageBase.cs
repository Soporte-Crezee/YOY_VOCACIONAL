using System;
using POV.Core.HerramientasDocente.Interfaces;
using POV.Core.HerramientasDocente.Implement;

namespace POV.AppCode.Page
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
        }

        protected abstract void AuthorizeUser();
    }
}