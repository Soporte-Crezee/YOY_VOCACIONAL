using System;
using System.Collections.Generic;
using System.Linq;
using POV.Seguridad.BO;
using POV.Web.PortalUniversidad.AppCode.Page;
using POV.Core.Universidades;

namespace POV.Web.PortalUniversidad
{
    public partial class Site : MasterPageBase
    {       

        #region Session_variables
        private string session_ReturnUrl
        {
            get
            {
                return Session["session_ReturnUrl"] as string;
            }
            set
            {
                Session["session_ReturnUrl"] = value;
            }
        }
        #endregion

        public Site()
            : base()
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Header.DataBind();
            if (!IsPostBack)
            {
                if (userSession.CurrentPerfil.PerfilID == (int)EPerfil.UNIVERSIDAD)
                {
                    if (userSession.CurrentEscuela != null && userSession.CurrentCicloEscolar != null)
                    {
                        ShowMenuUniversidad();
                        if (!string.IsNullOrEmpty(session_ReturnUrl))
                        {
                            string redirect = session_ReturnUrl;
                            session_ReturnUrl = string.Empty;
                            Response.Redirect("~" + redirect, true);
                        }
                    }
                    else
                        redirector.GoToLoginPage(true);
                }
                else
                    redirector.GoToLoginPage(true);

            }//end postback
        }

        private void ShowMenuUniversidad()
        {
            MultiViewTopMenu.SetActiveView(ViewTopMenuUniversidad);
            MultiViewToolBarMenu.SetActiveView(ViewToolBarMenuUniversidad);

            this.LblNombreUsuario.Text =  userSession.CurrentUser.NombreUsuario;
            this.HplNombreUniversidad.Text = userSession.CurrentUniversidad.NombreUniversidad;
            this.HplNombreUniversidad.NavigateUrl = UrlHelper.GetDefaultURL();
            this.HlpEditarPerfil.NavigateUrl = UrlHelper.GetEditarUniversidadURL();
            this.HlpCambiarPassword.NavigateUrl = UrlHelper.GetEditarPassURL();

            this.HplLogout.NavigateUrl = UrlHelper.GetLogoutURL();
        }

        protected override void AuthorizeUser()
        {
            //validamos los elementos del menu que se desplegaran
            if (userSession.CurrentPerfil.PerfilID == (int)EPerfil.UNIVERSIDAD)
            {
                if (userSession.CurrentEscuela != null && userSession.CurrentCicloEscolar != null)
                {
                    List<Permiso> permisos = userSession.CurrentPrivilegiosUniversidad.GetPermisos();

                    bool accesoOrientadores = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOORIENTADOR) != null;
                    bool accesoExpediente = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOXPEDIENTE) != null;

                    if (accesoOrientadores)
                    {
                        menu_OrientadoresCatalogo.Visible = true;
                        menu_CarrerasCatalogo.Visible = false;
                        menu_EventosCatalogo.Visible = true;
                        menu_VerExpedienteCatalogo.Visible = true;
                    }
                }
            }
        }
    }
}