using System;
using POV.Core.Administracion.Interfaces;

namespace POV.Core.Administracion.Implements
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

        public void GoToConfirmarDirector(bool endResponse)
        {
            Redirect(UrlHelper.GetConfirmarDirectorURL(), endResponse);
        }

        public void GoToSeleccionarEscuela(bool endResponse)
        {
            Redirect(UrlHelper.GetSeleccionarEscuelaURL(), endResponse);
        }

        public void GoToErrorPage(bool endResponse)
        {
            Redirect(UrlHelper.GetErrorURL(), endResponse);
        }

        public void GoToEditarDocente(bool endResponse) { Redirect(UrlHelper.GetEditarDocenteURL(), endResponse); }

        public void GoToEditarCostoProducto(bool endResponse) { Redirect(UrlHelper.GetEditarCostoProductoURL(), endResponse); }

        public void GoToEditarUniversidad(bool endResponse) { Redirect(UrlHelper.GetEditarUniversidadURL(), endResponse); }

        public void GoToEditarEspecialista(bool endResponse)
        {
            Redirect(UrlHelper.GetEditarEspecialistaURL(), endResponse); 
        }

        public void GoToRegistrarDocente(bool endResponse) { Redirect(UrlHelper.GetRegistrarDocenteURL(), endResponse); }

        public void GoToError(bool endResponse) { Redirect(UrlHelper.GetErrorURL(), endResponse); }
        public void GoToConsultarDocente(bool endResponse) { Redirect(UrlHelper.GetConsultarDocenteURL(), endResponse); }
        public void GoToConsultarProducto(bool endResponse) { Redirect(UrlHelper.GetConsultarProductoURL(), endResponse); }
        public void GoToConsultarUniversidad(bool endResponse) { Redirect(UrlHelper.GetConsultarUniversidadURL(), endResponse); }
        public void GoToConsultarEspecialista(bool endResponse)
        {
            Redirect(UrlHelper.GetConsultarEspecialistaURL(), endResponse);
        }

        public void GoToAsignarDocenteGrupo(bool endResponse){ Redirect(UrlHelper.GetAsignarDocenteGrupoURL(),endResponse);}
        public void GoToConsultarAlumnosGrupo(bool endResponse){ Redirect(UrlHelper.GetConsultarAlumnosGrupoURL(),endResponse);}
        public void GoToEditarGrupo(bool endResponse){Redirect(UrlHelper.GetEditarGrupoURL(),endResponse);}
        public void GoToConsultarGrupos(bool endResponse){Redirect(UrlHelper.GetConsultarGruposURL(),endResponse);}
		public void GoToConsultarPruebasAsiganadasGrupo(bool endResponse) { Redirect(UrlHelper.GetConsultarPruebasAsignadasGrupoURL(), endResponse); }
		public void GoToAsignarPruebaGrupo(bool endResponse) {Redirect (UrlHelper.GetAsignarPruebaGrupoURL(), endResponse);}
		
        public void GoToConsultarAlumnos(bool endResponse){ Redirect(UrlHelper.GetConsultarAlumnosURL(),endResponse);}
        public void GoToEditarAlumno(bool endResponse){ Redirect(UrlHelper.GetEditarAlumnoURL(),endResponse);}
        public void GoToGestionarAlumnosGrupo(bool endResponse){Redirect(UrlHelper.GetGestionarAlumnosGrupoURL(),endResponse);}
        public void GoToSeleccionarPerfil(bool endResponse) { Redirect(UrlHelper.GetSeleccionarPerfilURL(), endResponse); }

		
    }
}
