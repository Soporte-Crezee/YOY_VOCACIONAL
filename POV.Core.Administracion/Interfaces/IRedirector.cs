namespace POV.Core.Administracion.Interfaces
{
    public interface IRedirector
    {
        void GoToLoginPage(bool endResponse);
        void GoToHomePage(bool endResponse);
        void GoToAccesDenied(bool endResponse);
        void GoToAceptarTerminos(bool endResponse);
        void GoToConfirmarDirector(bool endResponse);
        void GoToSeleccionarEscuela(bool endResponse);
        void GoToReportView(string parametros, bool endResponse);
        void GoToEditarDocente(bool endResponse);
        void GoToEditarCostoProducto(bool endResponse);
        void GoToEditarUniversidad(bool endResponse);
        void GoToEditarEspecialista(bool endResponse);
        void GoToError(bool endResponse);
        void GoToConsultarDocente(bool endResponse);
        void GoToConsultarProducto(bool endResponse);
        void GoToConsultarUniversidad(bool endResponse);
        void GoToConsultarEspecialista(bool endResponse);
        void GoToEditarGrupo(bool endResponse );
		void GoToConsultarPruebasAsiganadasGrupo(bool endResponse);
		void GoToAsignarPruebaGrupo(bool endResponse);
        void GoToConsultarAlumnosGrupo(bool endResponse);
        void GoToAsignarDocenteGrupo(bool endResponse);
        void GoToConsultarGrupos(bool endResponse);
        void GoToConsultarAlumnos(bool endResponse);
        void GoToEditarAlumno(bool endResponse);
        void GoToGestionarAlumnosGrupo(bool endResponse);
        void GoToSeleccionarPerfil(bool endResponse);		
    }
}
