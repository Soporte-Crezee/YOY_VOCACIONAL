namespace POV.Core.RedSocial.Interfaces
{ 
    public interface IRedirector
    {
        void GoToLoginPage(System.Boolean accion);
        void GoToLogoutPage(System.Boolean accion);
        void GoToLoginPageAspirante(System.Boolean accion);
        void GoToLoginPageOrientador(System.Boolean accion);
        void GoToHomePage ( System.Boolean accion );
        void GoToAccesDenied ( System.Boolean accion );
        void GoToNotFound(System.Boolean bandera);
        void GoToHomeAlumno(System.Boolean accion);
        void GoToHomeDocente(System.Boolean accion);
        void GoToCambiarEscuela(bool accion);
        void GoToDiagnostica(bool accion);
        void GoToAceptarTerminos(bool accion);
        void GoToValidarDiagnostico(bool accion);
        void GoToConfirmarAlumno(bool accion);
        void GoToConfirmarMaestro(bool accion);
        void GoToActividades(bool accion);
        void GoToRegistrarNuevoAspirante(bool accion);
        void GoToSeleccionarCarrera(bool accion);
        void GoToActivarUsuario(bool accion);
        void GoToPruebas(bool accion);
        // Para realizar otra prueba de la Bateria de Bullying
        void GoToValidarBateriaBullying(bool accion);
    }
}
