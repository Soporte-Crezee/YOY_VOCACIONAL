using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Web.PortalOperaciones.Helper;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.Estados
{
    public partial class BuscarEstado : CatalogPage
    {
        private EstadoCtrl estadoCtrl;
        private PaisCtrl paisCtrl;

        #region *** propiedades de clase ***
        private DataSet DSEstado
        {
            get
            {
                if (Session["Estados"] != null) return Session["Estados"] as DataSet;
                return null;
            }
            set { Session["Estados"] = value; }

        }
        private Estado LastObject
        {
            set { Session["lastEstado"] = value; }
            get { return Session["lastEstado"] != null ? Session["lastEstado"] as Estado : null; }
        }
        #endregion

        public BuscarEstado()
        {
            estadoCtrl = new EstadoCtrl();
            paisCtrl = new PaisCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPaises();
                LoadEstados();
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                Estado estado = UserInterfaceToData();
                DSEstado = estadoCtrl.Retrieve(ConnectionHlp.Default.Connection, estado);
                grdEstados.DataSource = ConfigureGridResults(DSEstado);
                grdEstados.DataBind();
            }
            else
            {

                ShowMessage(validateMessage.Substring(2), MessageType.Warning);
            }
        }
        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnBuscar_Click(sender, null);
        }
        protected void grdEstados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        try
                        {
                            Estado estado = new Estado();
                            int estadoId = Convert.ToInt32(e.CommandArgument);
                            estado.EstadoID = estadoId;
                            Ciudad ciudad = new Ciudad();
                            ciudad.Estado = estado;
                            CiudadCtrl ciudadCtrl = new CiudadCtrl();
                            DataSet dsCiudades = ciudadCtrl.Retrieve(ConnectionHlp.Default.Connection, ciudad);
                            if (dsCiudades.Tables["Ciudad"].Rows.Count > 0)
                                ShowMessage("El estado tiene ciudades relacionadas, elimínelas antes de eliminar el estado", MessageType.Information);
                            else
                            {
                                UbicacionCtrl ubicacionCtrl = new UbicacionCtrl();
                                DataSet dsUbicaciones = ubicacionCtrl.Retrieve(ConnectionHlp.Default.Connection, new Ubicacion { Estado = estado });
                                if (dsUbicaciones.Tables["Ubicacion"].Rows.Count > 0)
                                    ShowMessage("El estado tiene ubicaciones relacionadas, elimínelas antes de eliminar el estado", MessageType.Information);
                                else
                                {
                                    estadoCtrl.Delete(ConnectionHlp.Default.Connection, estado);
                                    RemoveEstadoFromDataSet(DSEstado, estado);
                                    grdEstados.DataSource = DSEstado;
                                    grdEstados.DataBind();
                                    ShowMessage("El estado se eliminó con éxito", MessageType.Information);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message, MessageType.Error);
                        }
                        break;
                    }
                case "editar":
                    {
                        Estado estado = new Estado();
                        estado.EstadoID = int.Parse(e.CommandArgument.ToString());
                        LastObject = estado;
                        Response.Redirect("EditarEstado.aspx");
                        break;
                    }
                case "Sort":
                    {
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }

            }
        }
        protected void grdEstados_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataSet dataSet = DSEstado;
            if (dataSet != null)
            {
                DataView dataView = new DataView(dataSet.Tables["Estado"]);
                dataView.Sort = e.SortExpression + " " + GetSortDirection();
                DSEstado.Tables.Clear();
                DSEstado.Tables.Add(dataView.ToTable());
                grdEstados.DataSource = DSEstado;
                grdEstados.DataBind();
            }
        }
        #endregion

        #region *** validaciones ***
        private string ValidateData()
        {
            string sError = string.Empty;
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                if (txtNombre.Text.Trim().Length > 30)
                    sError = ", El tamaño del texto no debe ser mayor a 30 caracteres)";
            }
            return sError;

        }
        #endregion

        #region *** Data to UserInterface ***
        private void LoadEstados()
        {
            DSEstado = estadoCtrl.Retrieve(ConnectionHlp.Default.Connection, new Estado());
            DSEstado = ConfigureGridResults(DSEstado);
            grdEstados.DataSource = DSEstado;
            grdEstados.DataBind();
        }
        private void LoadPaises()
        {
            ddlPais.DataSource = ConfigureDropDownResults(paisCtrl.Retrieve(ConnectionHlp.Default.Connection, new Pais()));
            ddlPais.DataTextField = "Nombre";
            ddlPais.DataValueField = "PaisID";
            ddlPais.DataBind();
        }
        private DataSet ConfigureGridResults(DataSet ds)
        {
            ds.Tables["Estado"].Columns.Add("Pais");
            foreach (DataRow row in ds.Tables["Estado"].Rows)
            {
                Estado estado = estadoCtrl.DataRowToEstado(row);
                ListItem item = ddlPais.Items.FindByValue(row["PaisID"].ToString());
                row["Pais"] = item.Text;
            }
            return ds;
        }
        private static DataSet ConfigureDropDownResults(DataSet ds)
        {
            DataRow row = ds.Tables["Pais"].NewRow();
            row["PaisID"] = 0;
            row["Nombre"] = "Todos";
            ds.Tables["Pais"].Rows.InsertAt(row, 0);
            return ds;
        }
        #endregion

        #region *** UserInterface to Data ***
        private Estado UserInterfaceToData()
        {
            Estado estado = new Estado();
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
                estado.Nombre = txtNombre.Text.Trim() + "%";

            int paisId = 0;
            bool result = int.TryParse(ddlPais.SelectedValue, out paisId);
            if (paisId > 0)
            {
                estado.Pais = new Pais();
                estado.Pais.PaisID = paisId;
            }
            return estado;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void RemoveEstadoFromDataSet(DataSet ds, Estado estado)
        {
            string query = "EstadoID =" + estado.EstadoID;
            DataRow[] dr = ds.Tables["Estado"].Select(query);

            if (dr.Count() == 1)
                ds.Tables["Estado"].Rows.Remove(dr[0]);
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }
        private string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";
                    break;

                case "DESC":
                    GridViewSortDirection = "ASC";
                    break;
            }

            return GridViewSortDirection;
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

        protected void grdEstados_DataBound(object sender, GridViewRowEventArgs e)
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

            grdEstados.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOESTADOS) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARESTADOS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARESTADOS) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARESTADOS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARESTADOS) != null;

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