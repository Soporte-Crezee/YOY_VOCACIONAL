using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Administracion.BO;
using POV.Administracion.Services;
using POV.CentroEducativo.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Prueba.BO;
using POV.Seguridad.BO;
using POV.Web.Administracion.AppCode.Page;
using POV.Web.Administracion.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.Administracion.Productos
{
    public partial class EditarProducto : PageBase
    {
        #region Prpoiedades de la clase
        public CostoProducto CostoProductoUpd
        {
            get { return Session["CostoProductoUpd"] != null ? (CostoProducto)Session["CostoProductoUpd"] : null; }
            set { Session["CostoProductoUpd"] = value; }
        }


        private EscuelaCtrl escuelaCtrl;
        private GrupoCicloEscolarCtrl grupoCicloEscolar;
        private CostoProductoCtrl costoProductoCtrl;
        private ProductoCosteoCtrl productoCosteoCtrl;
        #endregion

        public EditarProducto() 
        {
            escuelaCtrl = new EscuelaCtrl();
            grupoCicloEscolar = new GrupoCicloEscolarCtrl();
            costoProductoCtrl = new CostoProductoCtrl(null);
            productoCosteoCtrl = new ProductoCosteoCtrl(null);
        }

        #region Eventos de la pagina
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession == null || !userSession.IsLogin())
                    redirector.GoToLoginPage(true);

                if (userSession.CurrentEscuela == null || userSession.CurrentCicloEscolar == null)
                    redirector.GoToError(true);

                if (!IsPostBack)
                {
                    if (CostoProductoUpd != null)
                    {
                        lblAccion.Text = "Editar";
                        txtNombre.Enabled = false;
                        ddlPruebas.Enabled = false;
                        DDLTipoProducto.Enabled = false;
                        LoadPruebas();
                        LoadUpdate();
                    }
                    else 
                    {
                        redirector.GoToError(true);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                    DoUpdate();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region Metodos Auxiliares
        #region validaciones
        private void ValidateData()
        {
            string sError = string.Empty;

            // valores requeridos
            if (txtNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre";
            if (txtDescripcion.Text.Trim().Length <= 0)
                sError += " ,Descripcion";
            if (txtPrecio.Text.Trim().Length <= 0)
                sError += " ,Precio";
            if (DDLTipoProducto.SelectedValue == "")
                sError += " ,tipo producto";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son requeridos : {0}", sError));
            }

            // valores incorrectos
            if (txtNombre.Text.Trim().Length > 250)
                sError += " ,Nombre";
            if (txtDescripcion.Text.Trim().Length > 500)
                sError += " ,Descripcion";
            if (txtPrecio.Text.Trim().Length > 15)
                sError += " ,Precio";
            if (DDLTipoProducto.SelectedValue.Length > 1)
                sError += " ,tipo producto";      

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos : {0}", sError));
            }
        }
        #endregion
        #region Edicion
        private void LoadUpdate()
        {
            CostoProductoCtrl costoProductoCtrl = new CostoProductoCtrl(null);
            CostoProducto costoProductoUpd = costoProductoCtrl.Retrieve(new CostoProducto { CostoProductoId = CostoProductoUpd.CostoProductoId }, false).First();
            DataToUserInterface(costoProductoUpd);
        }

        private void DataToUserInterface(CostoProducto costoProducto)
        {
            txtNombre.Text = costoProducto.Producto.Nombre;
            txtDescripcion.Text = costoProducto.Producto.Descripcion;
            txtPrecio.Text = costoProducto.Precio.ToString();
            DDLTipoProducto.SelectedValue = ((byte)(costoProducto.Producto.TipoProducto)).ToString();
            ddlPruebas.SelectedValue = costoProducto.Producto.PruebaID != null ? ((int)(costoProducto.Producto.PruebaID)).ToString() : string.Empty;

            hdnFechaInicio.Value = costoProducto.FechaInicio != null ? string.Format("{0:dd/MM/yyyy}", costoProducto.FechaInicio.Value) : string.Empty;
        }

        private void DoUpdate()
        {
            try
            {
                try
                {
                    ValidateData();
                }
                catch (Exception ex)
                {
                    this.ShowMessage(ex.Message, MessageType.Error);
                    return;
                }

                CostoProducto costoProductoOld = costoProductoCtrl.Retrieve(new CostoProducto { CostoProductoId = CostoProductoUpd.CostoProductoId }, true).First();
                ProductoCosteo productoCosteoOld = productoCosteoCtrl.Retrieve(new ProductoCosteo { ProductoID = costoProductoOld.Producto.ProductoID }, true).First();

                var producto = UserInterfaceToDataUpdProducto();
                var costo = UserInterfaceToDataUpdCosto();

                if (productoCosteoOld.Descripcion != producto.Descripcion || productoCosteoOld.TipoProducto != producto.TipoProducto || productoCosteoOld.PruebaID != producto.PruebaID)
                {
                    productoCosteoOld.Descripcion = txtDescripcion.Text.Trim();
                    productoCosteoOld.TipoProducto = producto.TipoProducto;
                    if (!string.IsNullOrEmpty(ddlPruebas.SelectedValue))
                        productoCosteoOld.PruebaID = int.Parse(ddlPruebas.SelectedValue);
                    productoCosteoCtrl.Update(productoCosteoOld);
                }

                if (costoProductoOld.Precio != costo.Precio) 
                {
                    costoProductoOld.FechaFin = DateTime.Now;
                    costoProductoCtrl.Update(costoProductoOld);
                    CostoProducto nuevoCosto = UserInterfaceToDataUpdCosto();
                    nuevoCosto.ProductoID = productoCosteoOld.ProductoID;
                    costoProductoCtrl.Insert(nuevoCosto);

                }

                CostoProductoUpd = null;
                redirector.GoToConsultarProducto(false);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        private ProductoCosteo UserInterfaceToDataUpdProducto()
        {
            ProductoCosteo obj = new ProductoCosteo();
            obj.Nombre = txtNombre.Text.Trim();
            obj.Descripcion = txtDescripcion.Text.Trim();
            obj.TipoProducto = (ETipoProducto)(byte)Convert.ChangeType(DDLTipoProducto.SelectedValue, typeof(byte));
            if (!string.IsNullOrEmpty(ddlPruebas.SelectedValue))
            obj.PruebaID=int.Parse(ddlPruebas.SelectedValue);

            return obj;
        }

        private CostoProducto UserInterfaceToDataUpdCosto()
        {
            CostoProducto obj = new CostoProducto();
            obj.Precio = Convert.ToDouble(txtPrecio.Text.Trim());
            obj.FechaInicio = DateTime.ParseExact(hdnFechaInicio.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return obj;
        }
        #endregion

        private void LoadPruebas() 
        {
            ContratoCtrl contratoCtrl = new ContratoCtrl();
            IDataContext dctx = ConnectionHlp.Default.Connection;            
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, userSession.Contrato, new CicloContrato { CicloEscolar = userSession.CurrentCicloEscolar });

            List<APrueba> pl = new List<APrueba>();
            foreach (PruebaContrato item in pruebas)
            {
                if (item.Prueba != null)
                {
                    pl.Add(item.Prueba);
                }
            }
            ddlPruebas.DataSource = pl;
            ddlPruebas.DataTextField = "TipoPruebaPresentacion";
            ddlPruebas.DataValueField = "PruebaID";
            ddlPruebas.DataBind();
            ddlPruebas.Items.Insert(0, new ListItem("SELECCIONE", ""));
        }
        #endregion

        #region Autorizacion de la pagina
        protected override void AuthorizeUser()
        {
            // ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegiosDirector.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOPRODUCTOS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOPRODUCTOS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOPRODUCTOS) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!creacion)
                redirector.GoToHomePage(true);
            if (!edit)
                redirector.GoToHomePage(true);
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