using System;
using POV.Seguridad.BO;
using GP.SocialEngine.BO;
using POV.CentroEducativo.BO;
using POV.Licencias.BO;
using System.Collections.Generic;

namespace POV.Core.HerramientasDocente.Interfaces
{    
    public interface IUserSession
    {   
        /// <summary>
        /// Usuario Seguridad Actual
        /// </summary>
        Usuario CurrentUser { get; set; }
                 
        /// <summary>
        /// Social Hub actual
        /// </summary>
         
        Escuela CurrentEscuela { get; set; }
        CicloEscolar CurrentCicloEscolar { get; set; }

        Docente CurrentDocente { get; set; }

        string NombreUsuario{ get;}
        bool LoggedIn { get; set; }
        
        void SetInSession(string key, object value);
        object GetFromSession(string key);
       
        Contrato Contrato { get; set; }

        void Logout();
        bool IsLogin();


    }
}
