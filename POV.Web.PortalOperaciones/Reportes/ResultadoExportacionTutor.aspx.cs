using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalOperaciones.Reportes
{
    public partial class ResultadoExportacionTutor : CatalogPage
    {
        private string SS_ListadoLog
        {
            get { return (string)this.Session["ListadoLog"]; }
            set { this.Session["ListadoLog"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBack();
                LeerLog();                
            }
            if (string.IsNullOrEmpty(SS_ListadoLog.Trim()))
            {
                redirector.GoToHomePage(false);
                return;
            }
            else 
            {
                txtFichero.Text = SS_ListadoLog;
            }
        }

        protected void btnBorrarLog_Click(object sender, EventArgs e)
        {
            string path = System.Configuration.ConfigurationManager.AppSettings["POVUrlResultadoExportacionTutor"];
            string fichero = Server.MapPath(path);
            string lines = string.Empty;
            StreamWriter sw = new StreamWriter(fichero);
            sw.WriteLine(lines);
            sw.Close();
            SS_ListadoLog = string.Empty;
            Response.Redirect("~/Default.aspx");
        }

        private void LeerLog() 
        {
            string path = System.Configuration.ConfigurationManager.AppSettings["POVUrlResultadoExportacionTutor"];
            string fichero = Server.MapPath(path);
            StreamReader sr = new StreamReader(fichero);
            SS_ListadoLog = sr.ReadToEnd();
            sr.Close();

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