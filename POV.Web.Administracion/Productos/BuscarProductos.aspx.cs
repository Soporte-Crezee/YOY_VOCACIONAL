using Framework.Base.Exceptions;
using POV.Administracion.BO;
using POV.Administracion.Services;
using POV.Modelo.Context;
using POV.Seguridad.BO;
using POV.Web.Administracion.AppCode.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.Administracion.Productos
{
    public partial class BuscarProductos : CatalogPage
    {
        #region Prpiedades de la clase
        public List<CostoProducto> DsProductos
        {
            get { return Session["dsProductos"] != null ? Session["dsProductos"] as List<CostoProducto> : null; }
            set { Session["dsProdcutos"] = value; }
        }

        public CostoProducto CostoProductoUpd
        {
            get { return Session["CostoProductoUpd"] != null ? (CostoProducto)Session["CostoProductoUpd"] : null; }
            set { Session["CostoProductoUpd"] = value; }
        }
        private CostoProductoCtrl costoProductoCtrl;
        #endregion

        public BuscarProductos()
        {
            costoProductoCtrl = new CostoProductoCtrl(null);
        }

        #region Eventos de la pagina
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CostoProductoUpd = null;
                if (userSession != null && userSession.CurrentDirector != null && userSession.CurrentCicloEscolar != null)
                {
                    if (!IsPostBack)
                        LoadCostoProducto(new CostoProducto { FechaFin = null });
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                CostoProducto costoProducto = UserInterfaceToData();
                LoadCostoProducto(costoProducto);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void grdProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "editar":
                        CostoProductoUpd = new CostoProducto { CostoProductoId = int.Parse(e.CommandArgument.ToString()) };
                        redirector.GoToEditarCostoProducto(false);
                        break;
                    case "eliminar":
                        CostoProductoUpd = new CostoProducto { CostoProductoId = int.Parse(e.CommandArgument.ToString()) };
                        DoDelete(CostoProductoUpd);
                        CostoProductoUpd = null;
                        LoadCostoProducto(new CostoProducto { FechaFin = null });
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void grdProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell statusCell = e.Row.Cells[2];
                if (statusCell.Text == "HorasOrientador")
                {
                    statusCell.Text = "Horas orientador";
                }

                if (RenderEdit)
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                    btnEdit.Visible = true;
                }

                if (RenderDelete)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDel");
                    btnDelete.Visible = true;
                }
            }
        }
        #endregion

        #region Metodos Auxiliares
        private void LoadCostoProducto(CostoProducto costoProductoFilter)
        {
            List<CostoProducto> listCostoProducto = costoProductoCtrl.Retrieve(costoProductoFilter, false);

            grdProductos.DataSource = listCostoProducto;
            grdProductos.DataBind();
            DsProductos = listCostoProducto;
        }

        private CostoProducto UserInterfaceToData()
        {
            CostoProducto costoProducto = new CostoProducto();
            costoProducto.Producto = new ProductoCosteo();
            if (DDLTipoProducto.SelectedValue != "" && DDLTipoProducto.SelectedValue.Length == 1)
                costoProducto.Producto.TipoProducto = (ETipoProducto)(byte)Convert.ChangeType(DDLTipoProducto.SelectedValue, typeof(byte));

            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
                costoProducto.Producto.Nombre = txtNombre.Text.Trim();
            return costoProducto;
        }

        private string EscapeLike(string valueWithoutWildcards)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueWithoutWildcards.Length; i++)
            {
                char c = valueWithoutWildcards[i];
                if (c == '*' || c == '%' || c == '[' || c == ']')
                    sb.Append("[").Append(c).Append("]");
                else if (c == '\'')
                    sb.Append("''");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private void DoDelete(CostoProducto costoProductoFilter)
        {
            try
            {
                var costoDelete = costoProductoCtrl.Retrieve(costoProductoFilter, true).First();
                if (costoDelete != null)
                {
                    costoDelete.FechaFin = DateTime.Now;
                    costoProductoCtrl.Update(costoDelete);
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region Auorizacion de la pagina
        private bool RenderEdit = false;
        private bool RenderDelete = false;

        protected override void DisplayCreateAction()
        {
            PnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {
            grdProductos.Visible = true;
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
            List<Permiso> permisos = userSession.CurrentPrivilegiosDirector.GetPermisos();
            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOPRODUCTOS) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOPRODUCTOS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOPRODUCTOS) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOPRODUCTOS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOPRODUCTOS) != null;

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

        #region Message Showing
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