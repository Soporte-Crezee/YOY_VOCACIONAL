using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Core.Operaciones;
using POV.Core.Operaciones.Implements;
using POV.Core.Operaciones.Interfaces;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;

namespace POV.Web.PortalOperaciones.Profesionalizacion.EjesTematicos
{
    public partial class ConfigurarSituacionesAprendizaje :CatalogPage 
    {

        #region Propiedades

        private EjeTematico LastObjectEjeTematico
        {
            set { Session["lastEjeTematico"] = value; }
            get { return Session["lastEjeTematico"] != null ? Session["lastEjeTematico"] as EjeTematico : null; }
        }
        private SituacionAprendizaje LastObject
        {
            get { return Session["lastSituacionAprendizaje"] != null ? Session["lastSituacionAprendizaje"] as SituacionAprendizaje : null; }
            set { Session["lastSituacionAprendizaje"] = value; }
        }
        private DataSet DsSituacionesAprendizaje
        {
            get { return Session["DsSituacionesAprendizaje"] != null ? Session["DsSituacionesAprendizaje"] as DataSet : null; }
            set { Session["DsSituacionesAprendizaje"] = value; }
        }

        private EjeTematicoCtrl ejeTematicoCtrl;
        private SituacionAprendizajeCtrl situacionAprendizajeCtrl;
        private IDataContext dctx;
        private IRedirector redirector;

        #endregion

        public ConfigurarSituacionesAprendizaje()
        {
            ejeTematicoCtrl=new EjeTematicoCtrl();
            situacionAprendizajeCtrl=new SituacionAprendizajeCtrl();
            redirector=new Redirector();
        }

        #region *****Eventos de pagina*****

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (LastObjectEjeTematico == null || LastObjectEjeTematico.EjeTematicoID == null)
                    Response.Redirect(UrlHelper.GetConsultarEjesTematicosURL(), true);

                if (!Page.IsPostBack){
                    LoadEjeTematico();
                    LoadSituacionesAprendizaje();
                }
            }
            catch (Exception ex)
            {

               Logger.Service.LoggerHlp.Default.Error(this,ex);
               Response.Redirect(UrlHelper.GetConsultarEjesTematicosURL(), true);
            }
        }
        protected void lnkBack1_Click(object sender, EventArgs e)
        {
            try
            {
                ClearSessions();
                Response.Redirect(UrlHelper.GetConsultarEjesTematicosURL(), true);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        protected void grdSituacionAprendizaje_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "consultarclasificadores":
                        SituacionAprendizaje situaciones = new SituacionAprendizaje { SituacionAprendizajeID = long.Parse(e.CommandArgument.ToString()) };
                        LastObject = situaciones;
                        Response.Redirect("ConsultarAgrupadoresContenido.aspx", true);
                        break;
                    case "editar":
                        LastObject = new SituacionAprendizaje() { SituacionAprendizajeID = long.Parse(e.CommandArgument.ToString()) };
                        Response.Redirect("EditarSituacionAprendizaje.aspx",true);
                        break;
                    case "eliminar":
                        SituacionAprendizaje situacion = new SituacionAprendizaje { SituacionAprendizajeID = long.Parse(e.CommandArgument.ToString()) };
                        DoDelete(situacion);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message,MessageType.Error);
            }
        }
        protected void grdSituacionAprendizaje_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (RenderEdit)
                    {
                        ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                        btnEdit.Visible = true;

                        Image imgConsultarClasificadores = (Image) e.Row.FindControl("imgConsultarClasificadores");
                        imgConsultarClasificadores.Visible = true;
                        LinkButton lnkbtnConsultarClasificadores = (LinkButton)e.Row.FindControl("lnkbtnConsultarClasificadores");
                        lnkbtnConsultarClasificadores.Visible = true;
                    }
                    if (RenderDelete)
                    {
                        ImageButton btnDelete = (ImageButton) e.Row.FindControl("btnDelete");
                        btnDelete.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        #endregion

        #region *****Metodos auxiliares*****

        private void LoadEjeTematico()
        {
            dctx = Helper.ConnectionHlp.Default.Connection;
            LastObjectEjeTematico = ejeTematicoCtrl.RetrieveComplete(dctx,
                    new EjeTematico {EjeTematicoID = LastObjectEjeTematico.EjeTematicoID});
            txtNivelEducativo.Text = LastObjectEjeTematico.AreaProfesionalizacion.NivelEducativo.Descripcion;
            txtGrado.Text = LastObjectEjeTematico.AreaProfesionalizacion.Grado.ToString() + "°";
            txtAsignatura.Text = LastObjectEjeTematico.AreaProfesionalizacion.Nombre;
            txtBloque.Text = LastObjectEjeTematico.MateriasProfesionalizacion.FirstOrDefault().Nombre;
            txtEjeTematicoID.Text = LastObjectEjeTematico.EjeTematicoID.ToString();
            txtEjeTematico.Text = LastObjectEjeTematico.Nombre;

        }
        private void LoadSituacionesAprendizaje()
        {
            dctx = Helper.ConnectionHlp.Default.Connection;
            DataSet ds = situacionAprendizajeCtrl.Retrieve(dctx, LastObjectEjeTematico, new SituacionAprendizaje());
            DsSituacionesAprendizaje = ConfigureGridResults(ds);
            grdSituacionAprendizaje.DataSource = DsSituacionesAprendizaje;
            grdSituacionAprendizaje.DataBind();
        }
        private DataSet ConfigureGridResults(DataSet ds)
        {
            if (!ds.Tables[0].Columns.Contains("NombreEstatus"))
                ds.Tables[0].Columns.Add(new DataColumn("NombreEstatus", typeof (string)));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                byte status = (byte)Convert.ChangeType(row["EstatusProfesionalizacion"], typeof(byte));
                row["NombreEstatus"] = ((EEstatusProfesionalizacion?)status).ToString();
            }

            return ds;
        }
        private void DoDelete(SituacionAprendizaje situacion)
        {
            try
            {
                dctx = Helper.ConnectionHlp.Default.Connection;
                situacionAprendizajeCtrl.Delete(dctx,LastObjectEjeTematico,situacion);
                this.ShowMessage("Tema eliminado exitosamente",MessageType.Information);
                this.LoadSituacionesAprendizaje();
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this,ex);
                throw new Exception("Ocurrió un error al eliminar el tema");
            }
        }
        private void ClearSessions()
        {
            LastObjectEjeTematico = null;
            LastObject = null;
            DsSituacionesAprendizaje = null;
        }
        #endregion

        #region *****Message  Showing*****
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="messageType">Tipo de mensaje</param>
        private void ShowMessage(string message, MessageType messageType)
        {
            string type = string.Empty;

            switch (messageType)
            {
                case MessageType.Error:
                    type = "1";
                    break;
                case MessageType.Information:
                    type = "3";
                    break;
                case MessageType.Warning:
                    type = "2";
                    break;
            }

            ShowMessage(message, type);
        }
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="typeNotification">1: Error, 2: Advertencia, 3: Información</param>
        private void ShowMessage(string message, string typeNotification)
        {
            //Se ubican los controles que manejan el desplegado de error/advertencia/información
            if (Page.Master == null) return;
            Control m = Page.Master.FindControl("hdnLastMessage");
            Control t = Page.Master.FindControl("hdnShowMessage");

            if (m == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnLastMessage' en la MasterPage.\nEl error original es:\n" + message);
            if (t == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnShowMessage' en la MasterPage.\nEl error original es:\n" + message);

            if (m.GetType() != typeof(HiddenField) || t.GetType() != typeof(HiddenField))
                throw new Exception("No se pudo desplegar correctamente el error.\nAlguno de los controles de la MasterPage para el manejo de errores no es HiddenField.\nEl error original es:\n" + message);

            //Si el HiddenField del mensaje de error ya tiene un mensaje guardado, se da un 'enter' y se concatena el nuevo mensaje (errores acumulados)
            //En caso contrario, se pone el encabezado y se concatena el nuevo mensaje
            if (((HiddenField)m).Value != null && ((HiddenField)m).Value.Trim().CompareTo("") != 0)
                ((HiddenField)m).Value += "<br />";


            ((HiddenField)m).Value += message.Replace("\n", "<br />");
            ((HiddenField)t).Value = typeNotification;
        }
        #endregion

        #region AUTORIZACION DE LA PAGINA

        private bool RenderEdit = false;
        private bool RenderDelete = false;

        protected override void DisplayCreateAction()
        {
            this.PnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {
            this.grdSituacionAprendizaje.Visible = true;
        }

        protected override void DisplayUpdateAction()
        {
            this.RenderEdit = true;
        }

        protected override void DisplayDeleteAction()
        {
            this.RenderDelete = true;
        }

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOSITUACIONES) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARSITUACIONES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARSITUACIONES) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARSITUACIONES) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARSITUACIONES) != null;

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