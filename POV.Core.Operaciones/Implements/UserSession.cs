using System;
using System.Web;
using POV.Seguridad.BO;
using System.Collections.Generic;
using POV.Core.Operaciones.Interfaces;

namespace POV.Core.Operaciones.Implements
{
    public class UserSession : IUserSession
    {
        private const string USUARIO_KEY = "USUARIO_KEY";
        private const string USUARIO_LOGGED_KEY = "USUARIO_LOGGED_KEY";
        private const string CURRENT_PRIVILEGIOS_KEY = "CURRENT_PRIVILEGIOS_KEY";

        private IWebContext webContext;

        public UserSession()
        {
            webContext = new WebContext();
        }

        public Usuario CurrentUser
        {
            get
            {
                if (webContext.ContainsInSession(USUARIO_KEY))
                {
                    return webContext.GetFromSession(USUARIO_KEY) as Usuario;
                }
                return null;
            }
            set
            {
                SetInSession(USUARIO_KEY, value);
            }
        }
        
        public bool LoggedIn
        {
            get
            {
                if (webContext.ContainsInSession(USUARIO_LOGGED_KEY))
                {
                    if ((bool)webContext.GetFromSession(USUARIO_LOGGED_KEY))
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                webContext.SetInSession(USUARIO_LOGGED_KEY, value);
            }
        }

        public void SetInSession(string key, object value)
        {
            webContext.SetInSession(key, value);
        }

        public object GetFromSession(string key)
        {
            return webContext.GetFromSession(key);
        }

        public void Logout()
        {
            webContext.ClearSession();
        }

        public bool IsLogin()
        {
            bool resp = CurrentUser != null && LoggedIn;

            return resp;
        }

        public UsuarioPrivilegios CurrentPrivilegios
        {
            get
            {

                if (webContext.ContainsInSession(CURRENT_PRIVILEGIOS_KEY))
                {
                    return webContext.GetFromSession(CURRENT_PRIVILEGIOS_KEY) as UsuarioPrivilegios;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(CURRENT_PRIVILEGIOS_KEY, value);
            }
        }
    }
}
