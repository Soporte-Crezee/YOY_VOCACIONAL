using System;
using POV.Core.Universidades.Interfaces;

namespace POV.Core.Universidades.Implements
{
    public class Redirector : IRedirector
    {

        public void GoToLoginPage(bool endResponse)
        {
            Redirect(UrlHelper.GetLoginURL(), endResponse);
        }

        public void GoToLogoutPage(bool endResponse)
        {
            Redirect(UrlHelper.GetLogoutURL(), endResponse);
        }

        public void GoToHomePage(bool endResponse)
        {
            Redirect(UrlHelper.GetDefaultURL(), endResponse);
        }

        public void GoToAccesDenied(bool endResponse)
        {

        }

        private void Redirect(string path, bool endResponse)
        {
            System.Web.HttpContext.Current.Response.Redirect(path, endResponse);
        }

        public void GoToReportView(string parameter, bool endResponse)
        {
            if (!string.IsNullOrEmpty(parameter))
                Redirect(string.Format("{0}?{1}", UrlHelper.GetReportURL(), parameter), endResponse);
            else
                Redirect(UrlHelper.GetReportURL(), endResponse);
        }

        public void GoToAceptarTerminos(bool endResponse)
        {
            Redirect(UrlHelper.GetAceptarTerminosURL(), endResponse);
        }

        public void GoToConfirmarUniversidad(bool endResponse)
        {
            Redirect(UrlHelper.GetConfirmarUniversidadURL(), endResponse);
        }

        public void GoToSeleccionarEscuela(bool endResponse)
        {
            Redirect(UrlHelper.GetSeleccionarEscuelaURL(), endResponse);
        }

        public void GoToErrorPage(bool endResponse)
        {
            Redirect(UrlHelper.GetErrorURL(), endResponse);
        }

        public void GoToEditarOrientador(bool endResponse) { Redirect(UrlHelper.GetEditarOrientadorURL(), endResponse); }

        public void GoToVincularExpediente(bool endResponse) { Redirect(UrlHelper.GetVincularExpedienteURL(), endResponse); }

        public void GoToEditarCarrera(bool endResponse) { Redirect(UrlHelper.GetEditarCarreraURL(), endResponse); }

        public void GoToEditarEspecialista(bool endResponse)
        {
            Redirect(UrlHelper.GetEditarEspecialistaURL(), endResponse); 
        }

        public void GoToRegistrarOrientador(bool endResponse) { Redirect(UrlHelper.GetRegistrarOrientadorURL(), endResponse); }

        public void GoToError(bool endResponse) { Redirect(UrlHelper.GetErrorURL(), endResponse); }
        public void GoToConsultarOrientador(bool endResponse) { Redirect(UrlHelper.GetConsultarOrientadorURL(), endResponse); }
        public void GoToConsultarEspecialista(bool endResponse)
        {
            Redirect(UrlHelper.GetConsultarEspecialistaURL(), endResponse);
        }
        public void GoToConsultarCarreras(bool endResponse)
        {
            Redirect(UrlHelper.GetConsultarCarrerasURL(), endResponse);
        }
        public void GoToVincularCarreras(bool endResponse)
        {
            Redirect(UrlHelper.GetVincularCarrerasURL(), endResponse);
        }

        public void GoToAsignarOrientadorGrupo(bool endResponse){ Redirect(UrlHelper.GetAsignarOrientadorGrupoURL(),endResponse);}
        public void GoToConsultarAlumnosGrupo(bool endResponse){ Redirect(UrlHelper.GetConsultarAlumnosGrupoURL(),endResponse);}
        public void GoToEditarGrupo(bool endResponse){Redirect(UrlHelper.GetEditarGrupoURL(),endResponse);}
        public void GoToConsultarGrupos(bool endResponse){Redirect(UrlHelper.GetConsultarGruposURL(),endResponse);}
		public void GoToConsultarPruebasAsiganadasGrupo(bool endResponse) { Redirect(UrlHelper.GetConsultarPruebasAsignadasGrupoURL(), endResponse); }
		public void GoToAsignarPruebaGrupo(bool endResponse) {Redirect (UrlHelper.GetAsignarPruebaGrupoURL(), endResponse);}
		
        public void GoToConsultarAlumnos(bool endResponse){ Redirect(UrlHelper.GetConsultarAlumnosURL(),endResponse);}
        public void GoToEditarUniversidad(bool endResponse){ Redirect(UrlHelper.GetEditarUniversidadURL(),endResponse);}
        public void GoToGestionarAlumnosGrupo(bool endResponse){Redirect(UrlHelper.GetGestionarAlumnosGrupoURL(),endResponse);}
        public void GoToSeleccionarPerfil(bool endResponse) { Redirect(UrlHelper.GetSeleccionarPerfilURL(), endResponse); }

        public void GoToConsultarEventos(bool endResponse) { Redirect(UrlHelper.GetConsultarEventosURL(), endResponse); }
        public void GoToEditarEvento(bool endResponse) { Redirect(UrlHelper.GetEditarEventoURL(), endResponse); }

		
    }
}
