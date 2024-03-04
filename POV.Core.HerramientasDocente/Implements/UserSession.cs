using System;
using System.Web;
using POV.Core.HerramientasDocente.Interfaces;
using POV.Seguridad.BO;
using POV.Licencias.BO;
using GP.SocialEngine.BO;
using POV.CentroEducativo.BO;
using System.Collections.Generic;

namespace POV.Core.HerramientasDocente.Implement
{
    public class UserSession: IUserSession
    {        
        private const string USUARIO_KEY = "USUARIO_KEY";
        private const string DOCENTE_KEY = "DOCENTE_KEY";
        private const string USUARIO_LOGGED_KEY = "USUARIO_LOGGED_KEY";
       
        private const string ESCUELA_KEY = "ESCUELA_KEY";
        private const string CICLO_KEY = "CICLO_KEY";
        private const string LICENCIAS_DOCENTE_KEY = "LICENCIAS_DOCENTE_KEY";
        
        private const string CONTRATO_KEY = "CONTRATO_KEY";      

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
  
        public string NombreUsuario
        {
            get { return CurrentUser.NombreUsuario; }
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

        public Docente CurrentDocente
        {
            get
            {

                if (webContext.ContainsInSession(DOCENTE_KEY))
                {
                    return webContext.GetFromSession(DOCENTE_KEY) as Docente;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(DOCENTE_KEY, value);
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

        public Escuela CurrentEscuela
        {
            get
            {

                if (webContext.ContainsInSession(ESCUELA_KEY))
                {
                    return webContext.GetFromSession(ESCUELA_KEY) as Escuela;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(ESCUELA_KEY, value);
            }
        }

        public CicloEscolar CurrentCicloEscolar
        {
            get
            {

                if (webContext.ContainsInSession(CICLO_KEY))
                {
                    return webContext.GetFromSession(CICLO_KEY) as CicloEscolar;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(CICLO_KEY, value);
            }
        }

        public Contrato Contrato
        {
            get
            {

                if (webContext.ContainsInSession(CONTRATO_KEY))
                {
                    return webContext.GetFromSession(CONTRATO_KEY) as Contrato;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(CONTRATO_KEY, value);
            }
        }


        
    }
}
