using System;
using POV.Seguridad.BO;
using GP.SocialEngine.BO;
using POV.CentroEducativo.BO;
using POV.Licencias.BO;
using System.Collections.Generic;
using POV.Prueba.BO;
using POV.Localizacion.BO;

namespace POV.Core.RedSocial.Interfaces
{
    
    public interface IUserSession
    {   
        /// <summary>
        /// Usuario Seguridad Actual
        /// </summary>
        Usuario CurrentUser { get; set; }
        /// <summary>
        /// Alumno actual
        /// </summary>
        Alumno CurrentAlumno { get; set; }
        /// <summary>
        /// Perfil Actual
        /// </summary>
        string Perfil { get; set; }
        /// <summary>
        /// Social Hub actual
        /// </summary>
        SocialHub SocialHub { get; set; }
        GrupoSocial CurrentGrupoSocial { get; set; }
        Escuela CurrentEscuela { get; set; }
        CicloEscolar CurrentCicloEscolar { get; set; }

        Docente CurrentDocente { get; set; }
        List<LicenciaEscuela> LicenciasDocente { get; set; }
        string NombreUsuario{ get;}
        bool LoggedIn { get; set; }
        UsuarioSocial CurrentUsuarioSocial { get; set; }

        UsuarioPrivilegios UsuarioPrivilegios { get; set; }
        GrupoCicloEscolar CurrentGrupoCicloEscolar { get; set; }

        List<ModuloFuncional> ModulosFuncionales { get; set; }
        void SetInSession(string key, object value);
        object GetFromSession(string key);
        Contrato Contrato { get; set; }
        APrueba PruebaAsignada { get; set; }

        void Logout();
        bool IsLogin();
        bool IsAlumno();
        bool IsDocente();


    }
}
