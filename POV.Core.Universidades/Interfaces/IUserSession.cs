using System;
using POV.Seguridad.BO;
using POV.CentroEducativo.BO;
using POV.Licencias.BO;
using System.Collections.Generic;

namespace POV.Core.Universidades.Interfaces
{

    public interface IUserSession
    {

        Usuario CurrentUser { get; set; }
        List<LicenciaEscuela> LicenciasUniversidad { get; set; }
        Perfil CurrentPerfil { get; set; }
        Contrato Contrato { get; set; }
        Universidad CurrentUniversidad { get; set; }
        Escuela CurrentEscuela { get; set; }
        CicloEscolar CurrentCicloEscolar { get; set; }
        bool EsAutoridadEstatal { get; set; }
        UsuarioPrivilegios PrivilegiosAutoridadEstatal { get; set; }
        UsuarioPrivilegios CurrentPrivilegiosUniversidad { get; set; }
        List<ModuloFuncional> ModulosFuncionales { get; set; }
        bool LoggedIn { get; set; }

        void SetInSession(string key, object value);
        object GetFromSession(string key);

        void Logout();
        bool IsLogin();

    }
}
