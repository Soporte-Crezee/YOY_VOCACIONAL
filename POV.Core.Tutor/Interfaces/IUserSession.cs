using POV.CentroEducativo.BO;
using POV.Licencias.BO;
using POV.Seguridad.BO;
using GP.SocialEngine.BO;
using System;
using System.Collections.Generic;

namespace POV.Core.PadreTutor.Interfaces
{

    public interface IUserSession
    {

        Usuario CurrentUser { get; set; }
        List<LicenciaEscuela> LicenciasTutor { get; set; }
        Perfil CurrentPerfil { get; set; }
        Contrato Contrato { get; set; }
        Tutor CurrentTutor { get; set; }
        Escuela CurrentEscuela { get; set; }
        CicloEscolar CurrentCicloEscolar { get; set; }
        bool EsAutoridadEstatal { get; set; }
        UsuarioPrivilegios PrivilegiosAutoridadEstatal { get; set; }
        UsuarioPrivilegios CurrentPrivilegiosTutor { get; set; }
        List<ModuloFuncional> ModulosFuncionales { get; set; }
        bool LoggedIn { get; set; }

        void SetInSession(string key, object value);
        object GetFromSession(string key);

        void Logout();
        bool IsLogin();

    }

}
