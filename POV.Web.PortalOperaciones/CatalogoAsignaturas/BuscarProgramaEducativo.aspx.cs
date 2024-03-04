using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.Web.PortalOperaciones.Helper;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.CatalogoAsignaturas
{
    public partial class BuscarProgramaEducativo : CatalogPage
    {
        private PlanEducativoCtrl planEducativoCtrl;
        private NivelEducativoCtrl nivelEducativoCtrl;

        #region *** propiedades de clase ***
        private PlanEducativo LastObject
        {
            set { Session["lastPlanEducativo"] = value; }
            get { return Session["lastPlanEducativo"] != null ? Session["lastPlanEducativo"] as PlanEducativo : null; }
        }
        private DataSet DsPlanEducativo
        {
            set { Session["PlanesEducativos"] = value; }
            get { return Session["PlanesEducativos"] != null ? Session["PlanesEducativos"] as DataSet : null; }
        }
        private List<Materia> ListMaterias
        {
            get
            {
                return Session["listMateriasPlan"] != null ? Session["listMateriasPlan"] as
                    List<Materia> : null;
            }
            set { Session["listMateriasPlan"] = value; }
        }
        private Materia LastMateria
        {
            set { Session["lastMateria"] = value; }
            get { return Session["LastMateria"] as Materia; }
        }
        private string ClaveOriginal
        {
            set { Session["claveOriginal"] = value; }
            get { return Session["claveOriginal"] as string; }
        }
        private DataTable DtMateriasEstatus
        {
            get
            {
                return Session["DtMateriasPlan"] != null ? Session["DtMateriasPlan"] as
                    DataTable : null;
            }
            set { Session["DtMateriasPlan"] = value; }
        }

        private DataTable DtMaterias
        {
            get
            {
                return Session["DtMaterias"] != null ? Session["DtMaterias"] as
                    DataTable : null;
            }
            set { Session["DtMaterias"] = value; }
        }
        #endregion

        public BuscarProgramaEducativo()
        {
            planEducativoCtrl = new PlanEducativoCtrl();
            nivelEducativoCtrl = new NivelEducativoCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LastObject = null;
                DsPlanEducativo = null;
                ListMaterias = null;
                LastMateria = null;
                ClaveOriginal = null;
                DtMateriasEstatus = null;
                DtMaterias = null;
                CargarDesdeTabla();
                LoadPlanesEducativos();
            }
        }

        protected void grdProgramaEducativo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        try
                        {
                            PlanEducativo planEducativo = new PlanEducativo();
                            int planEducativoID = Convert.ToInt32(e.CommandArgument);
                            planEducativo.PlanEducativoID = planEducativoID;
                            DoDelete(planEducativo);
                            grdProgramaEducativo.DataSource = DsPlanEducativo;
                            grdProgramaEducativo.DataBind();
                            ShowMessage("El plan educativo se desactivó con éxito", MessageType.Information);
                        }
                        catch (Exception ex)
                        {
                            ShowMessage("Ocurrió un problema al intentar desactivar el plan.", MessageType.Information);
                        }
                        break;
                    }
                case "editar":
                    {
                        PlanEducativo planEducativo = new PlanEducativo();
                        planEducativo.PlanEducativoID = int.Parse(e.CommandArgument.ToString());
                        LastObject = planEducativo;
                        Response.Redirect("EditarPlanEducativo.aspx");
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }
            }
        }

        protected void grdProgramaEducativo_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                PlanEducativo planEducativo = UserInterfaceToData();
                DsPlanEducativo = planEducativoCtrl.Retrieve(ConnectionHlp.Default.Connection, planEducativo);
                if (DsPlanEducativo.Tables.Count > 1)
                    planEducativo = planEducativoCtrl.LastDataRowToPlanEducativo(DsPlanEducativo);
                grdProgramaEducativo.DataSource = DsPlanEducativo;
                grdProgramaEducativo.DataBind();
            }
            else
            {
                ShowMessage(validateMessage, MessageType.Error);
            }

        }
        #endregion

        #region *** validaciones ***
        private string ValidateData()
        {
            string sError = string.Empty;
            if (!string.IsNullOrEmpty(txtIDPlan.Text.Trim()))
            {
                int planEducativoID = 0;
                bool result = int.TryParse(txtIDPlan.Text.Trim(), out planEducativoID);
                if (!result)
                    sError += ", El identificador debe ser un número entero";
            }
            if (!string.IsNullOrEmpty(txttituloPlan.Text.Trim()))
            {
                if (txttituloPlan.Text.Trim().Length > 50)
                    sError += ",El título no debe ser mayor de 50 caracteres";
            }
            if (!string.IsNullOrEmpty(txtDescripcion.Text.Trim()))
            {
                if (txtDescripcion.Text.Trim().Length > 200)
                    sError += ",La descripción no debe ser mayor de 200 caracteres";
            }
            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void LoadPlanesEducativos()
        {
            DsPlanEducativo = planEducativoCtrl.Retrieve(ConnectionHlp.Default.Connection, new PlanEducativo());
            grdProgramaEducativo.DataSource = DsPlanEducativo;
            grdProgramaEducativo.DataBind();
        }

        private void CargarDesdeTabla()
        {
            cbNivelEducativo.DataSource = configurarCbNivelEducativo();
            cbNivelEducativo.DataTextField = "Titulo";
            cbNivelEducativo.DataValueField = "NivelEducativoID";
            cbNivelEducativo.DataBind();
        }

        private DataSet configurarCbNivelEducativo()
        {
            DataSet ds = new DataSet();
            ds = nivelEducativoCtrl.Retrieve(ConnectionHlp.Default.Connection, new NivelEducativo());
            DataRow row = ds.Tables["NivelEducativo"].NewRow();
            row["NivelEducativoID"] = DBNull.Value;
            row["Titulo"] = "Seleccionar";
            ds.Tables["NivelEducativo"].Rows.InsertAt(row, 0);
            return ds;
        }
        #endregion

        #region *** UserInterface to Data ***
        private PlanEducativo UserInterfaceToData()
        {
            PlanEducativo planEducativo = new PlanEducativo();
            int idPlan = 0;
            if (int.TryParse(txtIDPlan.Text.Trim(), out idPlan))
            {
                planEducativo.PlanEducativoID = idPlan;
            }
            if (txttituloPlan.Text.Trim().Length > 0)
                planEducativo.Titulo = txttituloPlan.Text.Trim();
            if (txtDescripcion.Text.Trim().Length > 0)
                planEducativo.Descripcion = txtDescripcion.Text.Trim();
            if (cbNivelEducativo.SelectedIndex > 0)
            {
                NivelEducativo nivelEducativo = new NivelEducativo();
                nivelEducativo.NivelEducativoID = Convert.ToInt32(cbNivelEducativo.SelectedValue);
                planEducativo.NivelEducativo = nivelEducativo;
            }
            if (txtVigenciaInicio.Text.Trim().Length > 0)
            {
                DateTime fechaInicio = Convert.ToDateTime(txtVigenciaInicio.Text.Trim());
                planEducativo.ValidoDesde = fechaInicio;
            }

            if (txtVigenciaFin.Text.Trim().Length > 0)
            {
                DateTime fechaFin = Convert.ToDateTime(txtVigenciaFin.Text.Trim());
                planEducativo.ValidoHasta = fechaFin;
            }
            if (cbEstatus.SelectedIndex > 0)
            {
                if (cbEstatus.SelectedItem.Value == "true")
                    planEducativo.Estatus = true;
                if (cbEstatus.SelectedItem.Value == "false")
                    planEducativo.Estatus = false;
            }

            return planEducativo;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoDelete(PlanEducativo planEducativo)
        {
            planEducativoCtrl.Delete(ConnectionHlp.Default.Connection, planEducativo);
            foreach (DataRow row in DsPlanEducativo.Tables[0].Rows)
            {
                if (row["PlanEducativoID"].ToString() == planEducativo.PlanEducativoID.ToString())
                    row["Estatus"] = false;
            }
        }

        private void removePlanEducativoFromDataSet(DataSet DsPlanEducativo, PlanEducativo planEducativo)
        {
            string query = "PlanEducativoID =" + planEducativo.PlanEducativoID;
            DataRow[] dr = DsPlanEducativo.Tables["PlanEducativoID"].Select(query);
            if (dr.Count() == 1)
                DsPlanEducativo.Tables["PlanEducativoID"].Rows.Remove(dr[0]);
        }
        #endregion

        #region AUTORIZACION DE LA PAGINA


        protected void grdProgramaEducativo_DataBound(object sender, GridViewRowEventArgs e)
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

            grdProgramaEducativo.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPLANEDUCATIVO) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARPLANEDUCATIVO) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARPLANEDUCATIVO) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARPLANEDUCATIVO) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARPLANEDUCATIVO) != null;

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

        #region Mostrar Mensajes
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="typeNotification">1: Error, 2: Advertencia, 3: Información</param>
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