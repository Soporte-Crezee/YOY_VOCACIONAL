using System;
using System.Web;
using POV.Seguridad.BO;
using POV.Licencias.BO;
using POV.CentroEducativo.BO;
using System.Collections.Generic;
using POV.Core.Administracion.Interfaces;

namespace POV.Core.Administracion.Implements
{
    public class UserSession : IUserSession
    {
        private const string USUARIO_KEY = "USUARIO_KEY";
        private const string USUARIO_LOGGED_KEY = "USUARIO_LOGGED_KEY";
        private const string PERFIL_KEY = "PERFIL_KEY";
        private const string ESCUELA_KEY = "ESCUELA_KEY";
        private const string CICLO_KEY = "CICLO_KEY";
        private const string DIRECTOR_KEY = "DIRECTOR_KEY";
        private const string LICENCIAS_DIRECTOR_KEY = "LICENCIAS_DIRECTOR_KEY";
        private const string ES_AUTORIDAD_KEY = "ES_AUTORIDAD_KEY";
        private const string PRIVILEGIOS_AUTORIDAD_KEY = "PRIVILEGIOS_AUTORIDAD_KEY";
        private const string PRIVILEGIOS_DIRECTOR_KEY = "PRIVILEGIOS_DIRECTOR_KEY";
        private const string CONTRATO_KEY = "CONTRATO_KEY";
        private const string MODULOS_FUNCIONALES_KEY = "MODULOS_FUNCIONALES_KEY";

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

            if (resp)
            {
                if (CurrentDirector.EstatusIdentificacion == true)
                    resp = CurrentUser.AceptoTerminos.Value;
                else
                    resp = false;
            }

            return resp;
        }

        public Perfil CurrentPerfil
        {
            get
            {
                if (webContext.ContainsInSession(PERFIL_KEY))
                {
                    return webContext.GetFromSession(PERFIL_KEY) as Perfil;
                }
                return null;
            }
            set
            {
                webContext.SetInSession(PERFIL_KEY, value);
            }
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

        public List<LicenciaEscuela> LicenciasDirector
        {
            get
            {

                if (webContext.ContainsInSession(LICENCIAS_DIRECTOR_KEY))
                {
                    return webContext.GetFromSession(LICENCIAS_DIRECTOR_KEY) as List<LicenciaEscuela>;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(LICENCIAS_DIRECTOR_KEY, value);
            }
        }



        public Director CurrentDirector
        {
            get
            {

                if (webContext.ContainsInSession(DIRECTOR_KEY))
                {
                    return webContext.GetFromSession(DIRECTOR_KEY) as Director;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(DIRECTOR_KEY, value);
            }
        }


        public bool EsAutoridadEstatal
        {
            get
            {

                if (webContext.ContainsInSession(ES_AUTORIDAD_KEY))
                {
                    return (bool)webContext.GetFromSession(ES_AUTORIDAD_KEY);
                }
                else
                {
                    return false;
                }
            }
            set
            {

                webContext.SetInSession(ES_AUTORIDAD_KEY, value);
            }
        }


        public UsuarioPrivilegios PrivilegiosAutoridadEstatal
        {
            get
            {

                if (webContext.ContainsInSession(PRIVILEGIOS_AUTORIDAD_KEY))
                {
                    return webContext.GetFromSession(PRIVILEGIOS_AUTORIDAD_KEY) as UsuarioPrivilegios;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(PRIVILEGIOS_AUTORIDAD_KEY, value);
            }
        }


        public UsuarioPrivilegios CurrentPrivilegiosDirector
        {
            get
            {

                if (webContext.ContainsInSession(PRIVILEGIOS_DIRECTOR_KEY))
                {
                    return webContext.GetFromSession(PRIVILEGIOS_DIRECTOR_KEY) as UsuarioPrivilegios;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(PRIVILEGIOS_DIRECTOR_KEY, value);
            }
        }

        public List<ModuloFuncional> ModulosFuncionales
        {
            get
            {

                if (webContext.ContainsInSession(MODULOS_FUNCIONALES_KEY))
                {
                    return webContext.GetFromSession(MODULOS_FUNCIONALES_KEY) as List<ModuloFuncional>;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(MODULOS_FUNCIONALES_KEY, value);
            }
        }
    }
}
