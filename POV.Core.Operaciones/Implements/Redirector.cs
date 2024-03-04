using System;
using POV.Core.Operaciones.Interfaces;

namespace POV.Core.Operaciones.Implements
{
    public class Redirector : IRedirector
    {

        public void GoToLoginPage(bool endResponse)
        {
            Redirect(UrlHelper.GetLoginURL(), endResponse);
        }

        public void GoToHomePage(bool endResponse)
        {
            Redirect(UrlHelper.GetDefaultURL(), endResponse);
        }

        public void GoToAccesDenied(bool endResponse)
        {

        }
        public void GoToConsultarDirectores(bool endResponse)
        {
            Redirect(UrlHelper.GetConsultarDirectoresURL(),endResponse);
        }
        public void GoToEditarDirector(bool endResponse)
        {
            Redirect(UrlHelper.GetEditarDirectorURL(),endResponse);
        }
        public void GoToRegistrarNuevoDirector(bool endResponse)
        {
            Redirect(UrlHelper.GetRegistrarNuevoDirectorURL(),endResponse);
        }
        public void GoToConsultarEscuelas(bool endResponse)
        {
            Redirect(UrlHelper.GetConsultarEscuelasURL(),endResponse);
        }
        public void GoToEditarEscuela(bool endResponse)
        {
            Redirect(UrlHelper.GetEditarEscuelaURL(),endResponse);
        }
        public void GoToRegistrarNuevaEscuela(bool endResponse)
        {
            Redirect(UrlHelper.GetRegistrarNuevaEscuelaURL(),endResponse);
        }
        /*Ciclo Escolar*/
        public void GoToEditarCicloEscolar(bool endResponse)
        {
            Redirect(UrlHelper.EditarCicloEscolarURL(), endResponse);
        }
        public void GoToConsultarCiclosEscolares(bool endResponse)
        {
            Redirect(UrlHelper.GetConsultarCiclosEscolaresURL(),endResponse);
        }

        private void Redirect(string path, bool endResponse)
        {
            System.Web.HttpContext.Current.Response.Redirect(path, endResponse);
        }

   
    }
}
