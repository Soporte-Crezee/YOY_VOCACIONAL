
namespace POV.Core.Operaciones.Interfaces
{
    public interface IRedirector
    {
        void GoToLoginPage(bool endResponse);
        void GoToHomePage(bool endResponse);
        void GoToAccesDenied(bool endResponse);
        void GoToConsultarEscuelas(bool endResponse);
        void GoToEditarEscuela(bool endResponse);
        void GoToRegistrarNuevaEscuela(bool endResponse);
        void GoToConsultarDirectores(bool endResponse);
        void GoToEditarDirector(bool endResponse);
        void GoToRegistrarNuevoDirector(bool endResponse);
        void GoToEditarCicloEscolar(bool endResponse);
        void GoToConsultarCiclosEscolares(bool endResponse);
    }
}
