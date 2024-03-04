namespace POV.Core.Universidades.Interfaces
{
    public interface IRedirector
    {
        void GoToLoginPage(bool endResponse);
        void GoToLogoutPage(bool endResponse);
        void GoToHomePage(bool endResponse);
        void GoToAccesDenied(bool endResponse);
        void GoToAceptarTerminos(bool endResponse);
        void GoToConfirmarUniversidad(bool endResponse);
        void GoToSeleccionarEscuela(bool endResponse);
        void GoToReportView(string parametros, bool endResponse);
        void GoToEditarOrientador(bool endResponse);
        void GoToVincularExpediente(bool endResponse);
        void GoToEditarCarrera(bool endResponse);
        void GoToEditarEspecialista(bool endResponse);
        void GoToError(bool endResponse);
        void GoToConsultarOrientador(bool endResponse);
        void GoToConsultarEspecialista(bool endResponse);
        void GoToEditarGrupo(bool endResponse );
		void GoToConsultarPruebasAsiganadasGrupo(bool endResponse);
		void GoToAsignarPruebaGrupo(bool endResponse);
        void GoToConsultarAlumnosGrupo(bool endResponse);
        void GoToAsignarOrientadorGrupo(bool endResponse);
        void GoToConsultarGrupos(bool endResponse);
        void GoToConsultarAlumnos(bool endResponse);
        void GoToEditarUniversidad(bool endResponse);
        void GoToGestionarAlumnosGrupo(bool endResponse);
        void GoToSeleccionarPerfil(bool endResponse);
        void GoToConsultarCarreras(bool endResponse);
        void GoToVincularCarreras(bool endResponse);
        void GoToConsultarEventos(bool endResponse);
        void GoToEditarEvento(bool endResponse);
    }
}
