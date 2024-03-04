using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.Operaciones;
using POV.Core.Operaciones.Implements;
using POV.Core.Operaciones.Interfaces;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;

namespace POV.Web.PortalOperaciones.Profesionalizacion.CatalogoAreas
{
    public partial class BuscarAreas : CatalogPage
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private AreaProfesionalizacionCtrl areaProfesionalizacionCtrl;
        private NivelEducativoCtrl nivelEducativoCtrl;
        private IRedirector redirector;

        #region Propiedades de clase.
        private AreaProfesionalizacion LastObject
        {
            set { Session["lastAreaProfesionalizacion"] = value; }
            get { return Session["lastAreaProfesionalizacion"] != null ? Session["lastAreaProfesionalizacion"] as AreaProfesionalizacion : null; }
        }
        private DataSet DsAreaProfesionalizacion
        {
            get { return Session["DsAreaProfesionalizacion"] != null ? Session["DsAreaProfesionalizacion"] as DataSet : null; }
            set { Session["DsAreaProfesionalizacion"] = value; }
        }
        #endregion

        public BuscarAreas()
        {
            redirector = new Redirector();
            areaProfesionalizacionCtrl = new AreaProfesionalizacionCtrl();
            nivelEducativoCtrl = new NivelEducativoCtrl();
        }

        #region Eventos de la Página.
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    configurarCbNivelEducativo();
                    cbGradoAsignatura.Enabled = false;
                    DsAreaProfesionalizacion = areaProfesionalizacionCtrl.Retrieve(dctx, new AreaProfesionalizacion(){Activo=true});
                    LoadAreaProfesionalizacion();
                }
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                redirector.GoToHomePage(true);
            }
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                DoSearch();
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void grdAreas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "editar":
                        LastObject = new AreaProfesionalizacion { AreaProfesionalizacionID = int.Parse(e.CommandArgument.ToString()) };
                        Response.Redirect("EditarArea.aspx");
                        break;
                    case "eliminar":
                        AreaProfesionalizacion area = new AreaProfesionalizacion { AreaProfesionalizacionID = int.Parse(e.CommandArgument.ToString()) };
                        DoDelete(area);
                        break;
                    case "ver":
                        break;
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void cbNivelEducativo_SelectedIndexChanged(object sender, EventArgs e)
        {

            NivelEducativo nivelEducativo = GetNivelEducativoFromUI();
            if (nivelEducativo.NivelEducativoID != null)
            {
                DataSet ds = nivelEducativoCtrl.Retrieve(ConnectionHlp.Default.Connection, nivelEducativo);
                nivelEducativo = nivelEducativoCtrl.LastDataRowToNivelEducativo(ds);
                loadGrados(nivelEducativo);
                if (cbGradoAsignatura.Items.Count > 0)
                    cbGradoAsignatura.Enabled = true;
            }
            else
            {
                cbGradoAsignatura.Items.Clear();
                cbGradoAsignatura.Enabled = false;
            }
        }
        #endregion

        #region DataToUserInterface

        private void configurarCbNivelEducativo()
        {
            DataSet ds = new DataSet();
            ds = nivelEducativoCtrl.Retrieve(ConnectionHlp.Default.Connection, new NivelEducativo());
            DataRow row = ds.Tables["NivelEducativo"].NewRow();
            row["NivelEducativoID"] = DBNull.Value;
            row["Titulo"] = "TODOS";
            ds.Tables["NivelEducativo"].Rows.InsertAt(row, 0);

            cbNivelEducativo.DataSource = ds;
            cbNivelEducativo.DataTextField = "Titulo";
            cbNivelEducativo.DataValueField = "NivelEducativoID";
            cbNivelEducativo.DataBind();
        }
        
        private void loadGrados(NivelEducativo nivelEducativo)
        {
            if (nivelEducativo.NumeroGrados != null)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                ds.Tables[0].Columns.Add("Grado", typeof(string));
                ds.Tables[0].Columns.Add("Nombre", typeof(string));

                DataRow row = ds.Tables[0].NewRow();
                row["Grado"] = DBNull.Value;
                row["Nombre"] = "TODOS";
                ds.Tables[0].Rows.InsertAt(row, 0);

                for (int i = 1; i <= nivelEducativo.NumeroGrados.Value; i++)
                {
                    DataRow newRow = ds.Tables[0].NewRow();
                    newRow["Grado"] = i.ToString();
                    newRow["Nombre"] = i + "°";
                    ds.Tables[0].Rows.Add(newRow);
                }
                cbGradoAsignatura.DataSource = ds;
                cbGradoAsignatura.DataTextField = "Nombre";
                cbGradoAsignatura.DataValueField = "Grado";
                cbGradoAsignatura.DataBind();
            }
        }
        private NivelEducativo GetNivelEducativoFromUI()
        {
            NivelEducativo nivelEducativo = new NivelEducativo();
            int nivelEducativoID = 0;

            int.TryParse(cbNivelEducativo.SelectedValue, out nivelEducativoID);

            if (nivelEducativoID > 0)
                nivelEducativo.NivelEducativoID = nivelEducativoID;

            return nivelEducativo;
        }
        private byte? GetGradoFromUI()
        {
            byte grado = 0;

            byte.TryParse(cbGradoAsignatura.SelectedValue, out grado);

            if (grado <= 0)
                return null;

            return grado;
        }

        private void LoadAreaProfesionalizacion()
        {
            DataTable dtable = DsAreaProfesionalizacion.Tables[0];
            dtable.Columns.Add("NivelEducativo", typeof(string));

            foreach (DataRow row in DsAreaProfesionalizacion.Tables[0].Rows)
            {
                AreaProfesionalizacion area =  areaProfesionalizacionCtrl.RetrieveComplete(dctx, areaProfesionalizacionCtrl.DataRowToAreaProfesionalizacion(row));
                row["Activo"] = area.Activo == true ?  "True": "False";
                row["NivelEducativo"] = area.NivelEducativo.Titulo;
            }
            grdAreas.DataSource = DsAreaProfesionalizacion;
            grdAreas.DataBind();
        }
        #endregion

        public void DoSearch()
        {
            AreaProfesionalizacion areaProfesionalizacion = UserInterfaceToData();
            dctx = Helper.ConnectionHlp.Default.Connection;
            DsAreaProfesionalizacion = areaProfesionalizacionCtrl.Retrieve(dctx, areaProfesionalizacion);
            LoadAreaProfesionalizacion();
        }
        public void DoDelete(AreaProfesionalizacion area)
        {
            try
            {
                EjeTematicoCtrl ejeTematicoCtrl = new EjeTematicoCtrl();
                dctx = Helper.ConnectionHlp.Default.Connection;
                DataSet eje = ejeTematicoCtrl.Retrieve(dctx, new EjeTematico() { AreaProfesionalizacion = area });
                area = (AreaProfesionalizacion)areaProfesionalizacionCtrl.RetrieveComplete(dctx, area);
                area.Activo = false;
                if (eje.Tables[0].Rows.Count < 1)
                {
                    areaProfesionalizacionCtrl.DeleteComplete(dctx, area);
                    this.ShowMessage("Asignatura eliminada exitosamente", MessageType.Information);
                }
                else
                {
                    this.ShowMessage("Esta asignatura está asignada a un eje o ámbito, por lo cual no puede ser eliminada", MessageType.Error);
                }
                this.DoSearch();
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw new Exception("Ocurrió un error al eliminar la asignatura");
            }
        }
        private AreaProfesionalizacion UserInterfaceToData()
        {
            AreaProfesionalizacion area = new AreaProfesionalizacion();
            area.AreaProfesionalizacionID = !string.IsNullOrEmpty(txtIdentificador.Text.Trim()) ? int.Parse(txtIdentificador.Text.Trim()) : (int?)null;
            area.Nombre = !string.IsNullOrEmpty(txtNombre.Text.Trim()) ? string.Format("%{0}%",txtNombre.Text.Trim()) : null;
            area.NivelEducativo = GetNivelEducativoFromUI();
            area.Grado = GetGradoFromUI();
            area.Activo = true;

            return area;
        }

        #region *** AUTORIZACION DE LA PAGINA ***
        protected void grdAreas_DataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (RenderEdit)
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                    btnEdit.Visible = true;
                }
                if (RenderDelete)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");
                    btnDelete.Visible = true;
                }
            }
        }
        private bool RenderEdit = false;
        private bool RenderDelete = false;
        protected override void DisplayCreateAction()
        {
            PnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {
            grdAreas.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOAREASPROFESIONALIZACION) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARAREASPROFESIONALIZACION) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARAREASPROFESIONALIZACION) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARAREASPROFESIONALIZACION) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARAREASPROFESIONALIZACION) != null;

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
    }
}