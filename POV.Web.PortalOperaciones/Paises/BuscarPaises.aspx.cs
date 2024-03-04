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

namespace POV.Web.PortalOperaciones.Paises
{
    public partial class BuscarPaises : CatalogPage
    {
        private PaisCtrl paisCtrl;

        #region *** propiedades de clase ***
        private Pais LastObject
        {
            set { Session["lastPais"] = value; }
            get { return Session["lastPais"] != null ? Session["lastPais"] as Pais : null; }
        }
        private DataSet DsPaises
        {
            get { return Session["paises"] != null ? Session["paises"] as DataSet : null; }
            set { Session["paises"] = value; }
        }
        #endregion

        public BuscarPaises()
        {
            paisCtrl = new PaisCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadPaises();
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                Pais pais = UserInterfaceToData();
                DsPaises = paisCtrl.Retrieve(ConnectionHlp.Default.Connection, pais);
                grdPaises.DataSource = DsPaises;
                grdPaises.DataBind();

            }
            else
            {

                ShowMessage(validateMessage.Substring(2), MessageType.Warning);
            }
        }
        protected void grdPaises_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        try
                        {
                            Pais pais = new Pais();
                            int paisId = Convert.ToInt32(e.CommandArgument);
                            pais.PaisID = paisId;
                            Estado estado = new Estado();
                            estado.Pais = pais;
                            EstadoCtrl estadoCtrl = new EstadoCtrl();
                            DataSet dsEstados = estadoCtrl.Retrieve(ConnectionHlp.Default.Connection, estado);
                            if (dsEstados.Tables["Estado"].Rows.Count > 0)
                                ShowMessage("El país tiene estados relacionados, elimínelos antes de eliminar el país", MessageType.Information);
                            else
                            {
                                UbicacionCtrl ubicacionCtrl = new UbicacionCtrl();
                                DataSet dsUbicaciones = ubicacionCtrl.Retrieve(ConnectionHlp.Default.Connection, new Ubicacion { Pais = pais });
                                if (dsUbicaciones.Tables["Ubicacion"].Rows.Count > 0)
                                    ShowMessage("El país tiene ubicaciones relacionadas, elimínelas antes de eliminar el país", MessageType.Information);
                                else
                                {
                                    DoDelete(pais);
                                    grdPaises.DataSource = DsPaises;
                                    grdPaises.DataBind();
                                    ShowMessage("El pais se eliminó con éxito", MessageType.Information);
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
                        Pais pais = new Pais();
                        pais.PaisID = int.Parse(e.CommandArgument.ToString());
                        LastObject = pais;
                        Response.Redirect("EditarPais.aspx");
                        break;
                    }
                case "Sort":
                    {
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }

            }
        }

        protected void grdPaises_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataSet dataSet = DsPaises;

            if (dataSet != null)
            {
                DataView dataView = new DataView(dataSet.Tables["Pais"]);
                dataView.Sort = e.SortExpression + " " + GetSortDirection();
                DsPaises.Tables.Clear();
                DsPaises.Tables.Add(dataView.ToTable());
                grdPaises.DataSource = DsPaises;
                grdPaises.DataBind();
            }
        }
        #endregion

        #region *** validaciones ***
        private string ValidateData()
        {
            string sError = string.Empty;
            if (!string.IsNullOrEmpty(txtID.Text.Trim()))
            {
                int paisId = 0;
                bool result = int.TryParse(txtID.Text.Trim(), out paisId);
                if (!result)
                    sError += ", El identificador debe ser un número entero";
            }
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                if (txtNombre.Text.Trim().Length > 30)
                    sError = ", El tamaño del texto no debe ser mayor a 30 caracteres)";
            }
            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void LoadPaises()
        {
            DsPaises = paisCtrl.Retrieve(ConnectionHlp.Default.Connection, new Pais());
            grdPaises.DataSource = DsPaises;
            grdPaises.DataBind();
        }
        #endregion

        #region *** UserInterface to Data ***
        private Pais UserInterfaceToData()
        {
            Pais pais = new Pais();
            int id = 0;
            if (int.TryParse(txtID.Text.Trim(), out id))
            {
                pais.PaisID = id;
            }
            if (txtNombre.Text.Trim().Length > 0)
                pais.Nombre = txtNombre.Text.Trim() + "%";
            return pais;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoDelete(Pais pais)
        {
            paisCtrl.Delete(ConnectionHlp.Default.Connection, pais);
            RemovePaisFromDataSet(DsPaises, pais);
        }
        private void RemovePaisFromDataSet(DataSet ds, Pais pais)
        {
            string query = "PaisID =" + pais.PaisID;
            DataRow[] dr = ds.Tables["Pais"].Select(query);

            if (dr.Count() == 1)
                ds.Tables["Pais"].Rows.Remove(dr[0]);
        }
        private string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    GridViewSortDirection = "DESC";
                    break;

                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    GridViewSortDirection = "ASC";
                    break;
            }

            return newSortDirection;
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

        protected void grdPaises_DataBound(object sender, GridViewRowEventArgs e)
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

            grdPaises.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPAIS) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARPAIS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARPAIS) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARPAIS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARPAIS) != null;

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