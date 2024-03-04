using System;
using POV.Seguridad.BO;
using System.Collections.Generic;

namespace POV.Core.Operaciones.Interfaces
{

    public interface IUserSession
    {

        Usuario CurrentUser { get; set; }

        UsuarioPrivilegios CurrentPrivilegios { get; set; }

        bool LoggedIn { get; set; }
 
        void SetInSession(string key, object value);
        object GetFromSession(string key);

        void Logout();
        bool IsLogin();

    }
}
