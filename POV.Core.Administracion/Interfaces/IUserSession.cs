using System;
using POV.Seguridad.BO;
using POV.CentroEducativo.BO;
using POV.Licencias.BO;
using System.Collections.Generic;

namespace POV.Core.Administracion.Interfaces
{

    public interface IUserSession
    {

        Usuario CurrentUser { get; set; }
        List<LicenciaEscuela> LicenciasDirector { get; set; }
        Perfil CurrentPerfil { get; set; }
        Contrato Contrato{get;set;}
        Director CurrentDirector { get; set; }
        Escuela CurrentEscuela { get; set; }
        CicloEscolar CurrentCicloEscolar { get; set; }
        bool EsAutoridadEstatal { get; set; }
        UsuarioPrivilegios PrivilegiosAutoridadEstatal { get; set; }
        UsuarioPrivilegios CurrentPrivilegiosDirector { get; set; }
        List<ModuloFuncional> ModulosFuncionales { get; set; }
        bool LoggedIn { get; set; }
 
        void SetInSession(string key, object value);
        object GetFromSession(string key);

        void Logout();
        bool IsLogin();

    }
}
