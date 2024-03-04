//Satuface AN5 VerEjeTematico
using System;
using System.Web;
using POV.Core.RedSocial.Interfaces;
using POV.Seguridad.BO;
using POV.Licencias.BO;
using GP.SocialEngine.BO;
using POV.CentroEducativo.BO;
using System.Collections.Generic;
using POV.Prueba.BO;

namespace POV.Core.RedSocial.Implement
{
    public class UserSession: IUserSession
    {
        private const string SOCIAL_HUB_KEY = "SOCIAL_HUB_KEY";
        private const string USUARIO_SOCIAL_KEY = "USUARIO_SOCIAL_KEY";
        private const string GRUPO_SOCIAL_KEY = "GRUPO_SOCIAL_KEY";
        private const string USUARIO_KEY = "USUARIO_KEY";
        private const string DOCENTE_KEY = "DOCENTE_KEY";
        private const string ALUMNO_KEY = "ALUMNO_KEY";
        private const string USUARIO_LOGGED_KEY = "USUARIO_LOGGED_KEY";
        private const string PERFIL_KEY = "PERFIL_KEY";
        private const string ESCUELA_KEY = "ESCUELA_KEY";
        private const string CICLO_KEY = "CICLO_KEY";
        private const string LICENCIAS_DOCENTE_KEY = "LICENCIAS_DOCENTE_KEY";
        private const string PRIVILEGIOS_KEY = "USER_PRIVILEGIOS_KEY";
        private const string GRUPO_CICLO_KEY = "GRUPO_CLICLO_KEY";
        private const string CONTRATO_KEY = "CONTRATO_KEY";
        private const string PRUEBA_ASIGNADA_KEY = "PRUEBA_ASIGNADA_KEY";
        private const string ASIGNACION_PAQUETE = "ASIGNACION_PAQUETE_KEY";
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

        
        public SocialHub SocialHub 
        {
            get
            {
                if (webContext.ContainsInSession(SOCIAL_HUB_KEY))
                {
                    return webContext.GetFromSession(SOCIAL_HUB_KEY) as SocialHub;
                }

                return null;
            }

            set { webContext.SetInSession(SOCIAL_HUB_KEY, value); }
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

        

        public Alumno CurrentAlumno
        {
            get
            {

                if (webContext.ContainsInSession(ALUMNO_KEY))
                {
                    return webContext.GetFromSession(ALUMNO_KEY) as Alumno;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(ALUMNO_KEY, value);
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


        public UsuarioSocial CurrentUsuarioSocial
        {
            get
            {
                if (webContext.ContainsInSession(USUARIO_SOCIAL_KEY))
                {
                    return webContext.GetFromSession(USUARIO_SOCIAL_KEY) as UsuarioSocial;
                }
                return null;
            }
            set
            {
                webContext.SetInSession(USUARIO_SOCIAL_KEY, value);
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

            if (resp && !IsAlumno())
            {
                resp = (bool)CurrentUser.AceptoTerminos;
            }
            else
            {
                if (CurrentAlumno != null)
                    if (CurrentAlumno.EstatusIdentificacion == true)
                        resp = true;
                    else
                        resp = false;
            }
            
            return resp;
        }

        public bool IsAlumno()
        {
            return Perfil.CompareTo("A") == 0;
        }

        public bool IsDocente()
        {
            return Perfil.CompareTo("D") == 0;
        }


        public string Perfil
        {
            get
            {
                if (webContext.ContainsInSession(PERFIL_KEY))
                {
                    return webContext.GetFromSession(PERFIL_KEY) as string;
                }
                return null;
            }
            set
            {
                webContext.SetInSession(PERFIL_KEY, value);
            }
        }


        public GrupoSocial CurrentGrupoSocial
        {
            get
            {

                if (webContext.ContainsInSession(GRUPO_SOCIAL_KEY))
                {
                    return webContext.GetFromSession(GRUPO_SOCIAL_KEY) as GrupoSocial;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(GRUPO_SOCIAL_KEY, value);
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

        public List<LicenciaEscuela> LicenciasDocente
        {
            get
            {

                if (webContext.ContainsInSession(LICENCIAS_DOCENTE_KEY))
                {
                    return webContext.GetFromSession(LICENCIAS_DOCENTE_KEY) as List<LicenciaEscuela>;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(LICENCIAS_DOCENTE_KEY, value);
            }
        }


        public UsuarioPrivilegios UsuarioPrivilegios
        {
            get
            {

                if (webContext.ContainsInSession(PRIVILEGIOS_KEY))
                {
                    return webContext.GetFromSession(PRIVILEGIOS_KEY) as UsuarioPrivilegios;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(PRIVILEGIOS_KEY, value);
            }
        }


        public GrupoCicloEscolar CurrentGrupoCicloEscolar
        {
            get
            {

                if (webContext.ContainsInSession(GRUPO_CICLO_KEY))
                {
                    return webContext.GetFromSession(GRUPO_CICLO_KEY) as GrupoCicloEscolar;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(GRUPO_CICLO_KEY, value);
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

        public APrueba PruebaAsignada
        {
            get
            {

                if (webContext.ContainsInSession(PRUEBA_ASIGNADA_KEY))
                {
                    return webContext.GetFromSession(PRUEBA_ASIGNADA_KEY) as APrueba;
                }
                else
                {
                    return null;
                }
            }
            set
            {

                webContext.SetInSession(PRUEBA_ASIGNADA_KEY, value);
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
