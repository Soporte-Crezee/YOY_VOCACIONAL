namespace POV.Core.PadreTutor.Interfaces
{ 
    public interface IRedirector
    {
        void GoToLoginPage( System.Boolean accion );
        void GoToHomePage ( System.Boolean accion );
        void GoToAccesDenied ( System.Boolean accion );
        void GoToPaquetes(bool accion);
        void GoToCreditos(bool accion);
        void GoToReporteSACKS(bool accion);
        void GoToConfirmarTutor(bool accion);
        void GoToAceptarTerminos(bool accion);
    }
}
