using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.Administracion.Implements;
using POV.Core.Administracion.Interfaces;
using POV.Licencias.BO;
using POV.CentroEducativo.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using Framework.Base.DataAccess;
using POV.Web.Administracion.Helper;

namespace POV.Web.Administracion
{
    public partial class SeleccionarPerfil : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;
        private PerfilCtrl perfilCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private UsuarioPrivilegiosCtrl usuarioPrivCtrl;

        public SeleccionarPerfil()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            perfilCtrl = new PerfilCtrl();
            usuarioPrivCtrl = new UsuarioPrivilegiosCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.CurrentUser != null && userSession.LoggedIn)
                {
                    if (userSession.EsAutoridadEstatal == true)
                    {
                        if (userSession.LicenciasDirector != null && userSession.LicenciasDirector.Count > 0)
                        {
                            userSession.CurrentPerfil = null;

                        }
                        else
                        {
                            SeleccionarAutoridad();
                        }
                    }
                    else
                    {
                        if (userSession.LicenciasDirector != null && userSession.LicenciasDirector.Count > 0)
                        {
                            AceptarTerminos();
                        }
                        else
                        {
                            redirector.GoToLoginPage(false);
                        }
                    }
                }
                else
                {
                    if (userSession.CurrentDirector != null && userSession.EsAutoridadEstatal == false)
                        AceptarTerminos();
                    else
                    {
                        redirector.GoToLoginPage(false);
                    }
                }
            }
        }

        private void SeleccionarAutoridad()
        {
            userSession.CurrentPerfil = userSession.PrivilegiosAutoridadEstatal.Perfiles.FirstOrDefault(item => item.PerfilID == (int)EPerfil.AUTORIDAD);
            redirector.GoToHomePage(false);
        }

        protected void Btn_Autoridad_OnClick(Object sender, EventArgs e)
        {
                SeleccionarAutoridad();
        }

        protected void Btn_Director_OnClick(Object sender, EventArgs e)
        {
                AceptarTerminos();
        }

        private void AceptarTerminos()
        {
            Usuario usuario = userSession.CurrentUser;
            userSession.CurrentPerfil = new Perfil { PerfilID = (int)EPerfil.DIRECTOR };
            if (userSession.CurrentDirector.EstatusIdentificacion == true)
            {
                if ((bool)usuario.AceptoTerminos)
                    redirector.GoToSeleccionarEscuela(false);
                else
                    redirector.GoToAceptarTerminos(false);
            }
            else
            {
                redirector.GoToConfirmarDirector(false);
            }
        }
    }
}