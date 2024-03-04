using POV.CentroEducativo.BO;
using POV.Logger.Service;
using POV.Operaciones.Reports.Reports;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalOperaciones.Reportes
{
    public partial class CargaAlumnosReport : CatalogPage
    {
        #region Propiedades de la clase
        private Escuela EscuelaActual
        {
            get { return (Escuela)this.Session["ESCUELA_KEY"]; }
            set { this.Session["ESCUELA_KEY"] = value; }
        }
        private CicloEscolar CicloEscolarActual
        {
            get { return (CicloEscolar)this.Session["CICLO_KEY"]; }
            set { this.Session["CICLO_KEY"] = value; }
        }

        private DataTable SS_ResultadoCarga
        {
            get { return (DataTable)this.Session["ResultadoCargaAlumnos"]; }
            set { this.Session["ResultadoCargaAlumnos"] = value; }
        }

        private DataTable SS_ListadoCarga
        {
            get { return (DataTable)this.Session["ListadoCargaAlumnos"]; }
            set { this.Session["ListadoCargaAlumnos"] = value; }
        }
        #endregion

        public CargaAlumnosReport() 
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                        FillBack();                  
                }
                if (EscuelaActual == null || CicloEscolarActual == null || SS_ListadoCarga == null || SS_ResultadoCarga == null)
                {
                    redirector.GoToHomePage(false);
                    return;
                }
    
                CargaAlumnosRpt report = new CargaAlumnosRpt(this.EscuelaActual, this.CicloEscolarActual, this.SS_ListadoCarga, this.SS_ResultadoCarga);
                rptVAlumnos.Report = report;
            }
            catch (Exception ex) 
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        private void FillBack() 
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Default.aspx";
        }

        #region AUTORIZACION DE LA PAGINA

        private bool RenderEdit = false;
        private bool RenderDelete = false;

        protected override void DisplayCreateAction()
        {
            //pnlCargarAlumnos.Visible = true;
        }

        protected override void DisplayReadAction()
        {

            //Contenido.Visible = true;
        }

        protected override void DisplayUpdateAction()
        {
            RenderEdit = true;
        }

        protected override void DisplayDeleteAction()
        {
            RenderDelete = true;
        }

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCATPRUEBAS) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARCATPRUEBA) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARCATPRUEBA) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARCATPRUEBA) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCATPRUEBA) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (lectura)
                DisplayReadAction();
            if (creacion)
                DisplayCreateAction();
            if (delete)
                DisplayDeleteAction();
            if (edit)
                DisplayUpdateAction();
        }
        #endregion
    }
}