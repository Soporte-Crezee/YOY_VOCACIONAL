using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Web.Administracion.Helper;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Core.Administracion.Implements;
using POV.Core.Administracion.Interfaces;
using POV.Seguridad.BO;
using POV.Web.Administracion.AppCode.Page;

namespace POV.Web.Administracion
{
    public partial class Site : MasterPageBase
    {

        public Site()
            : base()
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {


            this.Page.Header.DataBind();
            if (!IsPostBack)
            {
                if (userSession.CurrentPerfil != null)
                {
                    lblAnio.Text = DateTime.Now.ToString("yyyy");
                    string usuario = userSession.CurrentUser.NombreUsuario;
                    this.LblNombreUsuario.Text = usuario;

                    if (userSession.CurrentPerfil.PerfilID == (int)EPerfil.AUTORIDAD) //es autoridad
                    {
                        ShowMenuAutoridad();
                    }
                    else if (userSession.CurrentPerfil.PerfilID == (int)EPerfil.DIRECTOR)
                    {
                        if (userSession.CurrentEscuela != null && userSession.CurrentCicloEscolar != null)
                        {
                            ShowMenuDirector();
                        }
                        else
                            redirector.GoToSeleccionarEscuela(true);
                    }
                    else
                        redirector.GoToLoginPage(true);
                }
                else
                    redirector.GoToSeleccionarPerfil(true);


            }//end postback

        }

        private void ShowMenuDirector()
        {
            MultiViewTopMenu.SetActiveView(ViewTopMenuDirector);
        }

        private void ShowMenuAutoridad()
        {
        }

        protected override void AuthorizeUser()
        {
            //validamos los elementos del menu que se desplegaran
            if (userSession.CurrentPerfil.PerfilID == (int)EPerfil.DIRECTOR)
            {
                if (userSession.CurrentEscuela != null && userSession.CurrentCicloEscolar != null)
                {
                    List<Permiso> permisos = userSession.CurrentPrivilegiosDirector.GetPermisos();

                    bool accesoAlumnos = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOALUMNOS) != null;
                    bool accesoDocentes = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESODOCENTES) != null;
                    bool accesoReporteDiagnostico = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOREPORTEDIAGNOSTICO) != null;
                    bool accesoExpediente = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOEXPEDIENTE) != null;
                    bool accesoGrupos = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOGRUPOS) != null;
                    bool accesoEspecialista = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOESPECIALISTA) != null;

                    if (accesoDocentes)
                    {
                        opcion_Docentes.Visible = true;
                        opcion_Universidad.Visible = true;
                        menu_DocentesCatalogo.Visible = true;
                        menu_UniversidadCatalogo.Visible = true;
                    }

                    string moduloTalentosKey = @System.Configuration.ConfigurationManager.AppSettings["MODULO_TALENTOS_KEY"];
                    bool tieneAccesoTalentos = userSession.ModulosFuncionales != null && userSession.ModulosFuncionales.FirstOrDefault(m => m.ModuloFuncionalId == int.Parse(moduloTalentosKey)) != null;
                }
            }
        }
    }
}