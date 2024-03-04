using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Prueba.Diagnostico.Service;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;

namespace POV.Web.PortalOperaciones.Pruebas
{
    public partial class BuscarPruebas : CatalogPage
    {
        private CatalogoPruebaCtrl catalogoPruebaCtrl;
        private PruebaDinamicaCtrl pruebaDinamicaCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        #region *** propiedades de clase ***
        public DataSet DsPruebas
        {
            set { Session["pruebas"] = value; }
            get { return Session["pruebas"] != null ? Session["pruebas"] as DataSet : null; }
        }

        public APrueba LastObject
        {
            set { Session["lastPruebas"] = value; }
            get { return Session["lastPruebas"] != null ? Session["lastPruebas"] as APrueba : null; }
        }
        public PruebaFilter lastObjectFilter
        {
            set { Session["lastObjectFilter"] = value; }
            get { return Session["lastObjectFilter"] != null ? Session["lastObjectFilter"] as PruebaFilter : null; }
        }
        public AModelo LastModeloSelect
        {
            set { Session["LastModeloSelect"] = value; }
            get { return Session["LastModeloSelect"] != null ? Session["LastModeloSelect"] as AModelo : null; }
        }
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }

        public string EstadoLiberacion { get { return this.ddlEstadoLiberacion.SelectedValue; } }
        #endregion

        public BuscarPruebas()
        {
            catalogoPruebaCtrl = new CatalogoPruebaCtrl();
            pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LastModeloSelect = null;
                this.LastObject = null;

                this.DataToUserInterface();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.LoadPruebas();
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int pruebaConfigID = int.Parse(e.CommandArgument.ToString());
            DataRow row = DsPruebas.Tables[0].Rows.Find(pruebaConfigID);

            // Obtener el modelo seleccionado en el grid
            ModeloCtrl modeloCtrl = new ModeloCtrl();
            int modeloID = int.Parse(row["modeloID"].ToString());
            this.LastModeloSelect = modeloCtrl.LastDataRowToModelo(modeloCtrl.Retrieve(dctx, modeloID, new Dictionary<string, string>()));

            ETipoPruebaPresentacion tipoPruebaPresentacion = (ETipoPruebaPresentacion)Convert.ToByte(row["TipoPruebaPresentacion"].ToString());
            this.LastObject = this.CreateObjectPrueba(this.LastModeloSelect, pruebaConfigID);
            this.LastObject.TipoPruebaPresentacion = tipoPruebaPresentacion;
            switch (e.CommandName.Trim())
            {
                case "editar":
                    Response.Redirect("EditarPrueba.aspx", true);
                    break;
                case "eliminar":
                    this.EliminarPrueba(this.LastObject);
                    break;
                case "liberar":
                    Response.Redirect("LiberarPrueba.aspx", true);
                    break;
                case "config":
                    this.ConfigurarPrueba(pruebaConfigID);
                    break;
                case "reactivos":
                    Response.Redirect("ConfigurarBancoReactivos.aspx");
                    break;
            }
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

        protected void grd_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                ETipoPrueba tipoPrueba = (ETipoPrueba)Convert.ToByte(rowView["Tipo"].ToString());
                EEstadoLiberacionPrueba estadoLiberacion = (EEstadoLiberacionPrueba)Convert.ToByte(rowView["EstadoLiberacion"].ToString());

                if (RenderEdit)
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                    btnEdit.Visible = (estadoLiberacion != EEstadoLiberacionPrueba.INACTIVA);
                    ImageButton btnLiberar = (ImageButton)e.Row.FindControl("btnLiberar");
                    btnLiberar.Visible = (estadoLiberacion == EEstadoLiberacionPrueba.ACTIVA);
                    LinkButton lnkConfig = (LinkButton)e.Row.FindControl("lnkConfig");
                    lnkConfig.Visible = true;
                    LinkButton lnkReactivos = (LinkButton)e.Row.FindControl("lnkReactivos");
                    lnkReactivos.Visible = true;
                }

                if (RenderDelete)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");
                    btnDelete.Visible = (estadoLiberacion != EEstadoLiberacionPrueba.INACTIVA);
                }
            }
        }
        #endregion

        #region *** validaciones ***
        private string ValidateData()
        {
            string sError = string.Empty;

            string sID = txtID.Text.Trim();
            long id = 0;
            if (!string.IsNullOrEmpty(sID) && !long.TryParse(sID, out id))
                sError += " El identificador debe ser numérico. ";

            string clave = txtClave.Text.Trim();
            if (clave.Length > 30)
                sError += " La clave no debe ser mayor a 30 caracteres";

            string nombre = txtNombre.Text.Trim();
            if (nombre.Length > 100)
                sError += " El nombre no debe ser mayor a 100 caracteres";

            return sError;
        }
        #endregion

        #region *** Data to UserInteface ***
        private void DataToUserInterface()
        {
            LoadModelos();
            LoadEstadoLiberacion();

            if (this.lastObjectFilter != null)
            {
                this.txtID.Text = this.lastObjectFilter.PruebaID == null ? string.Empty : this.lastObjectFilter.PruebaID.ToString();
                this.txtClave.Text = this.lastObjectFilter.Clave == null ? string.Empty : this.lastObjectFilter.Clave;
                this.txtNombre.Text = this.lastObjectFilter.Nombre == null ? string.Empty : this.lastObjectFilter.Nombre;
                this.ddlModelo.SelectedIndex = this.lastObjectFilter.ModeloID == null || this.lastObjectFilter.ModeloID == null ?
                    this.ddlModelo.SelectedIndex = -1 : this.ddlModelo.SelectedIndex = this.lastObjectFilter.ModeloID.Value;
                int estadoliberacion;
                if (this.lastObjectFilter.EstadoLiberacionPrueba != null)
                {
                    estadoliberacion = (int)this.lastObjectFilter.EstadoLiberacionPrueba;
                    this.ddlEstadoLiberacion.SelectedValue = estadoliberacion.ToString();
                }
                else
                    this.ddlEstadoLiberacion.SelectedIndex = -1;
            }

            this.LoadPruebas();
        }

        private void LoadModelos()
        {
            ModeloCtrl modeloCtrl = new ModeloCtrl();
            Dictionary<string, string> parametros = new Dictionary<string, string>();

            parametros.Add("Activo", "true");

            DataSet dsModelos = modeloCtrl.Retrieve(dctx, null, parametros); //new DataSet();

            ddlModelo.DataSource = dsModelos;
            ddlModelo.DataTextField = "Nombre";
            ddlModelo.DataValueField = "ModeloID";
            ddlModelo.DataBind();
            ddlModelo.Items.Insert(0, new ListItem("SELECCIONE...", "-1"));
        }

        private void LoadEstadoLiberacion()
        {
            var dictionary = new Dictionary<int, string>();
            foreach (byte value in Enum.GetValues(typeof(EEstadoLiberacionPrueba)))
            {
                if (value != (byte)EEstadoLiberacionPrueba.INACTIVA)
                    dictionary.Add(value, Enum.GetName(typeof(EEstadoLiberacionPrueba), value));
            }
            this.ddlEstadoLiberacion.DataSource = dictionary;
            this.ddlEstadoLiberacion.DataTextField = "Value";
            this.ddlEstadoLiberacion.DataValueField = "Key";
            this.ddlEstadoLiberacion.DataBind();
            this.ddlEstadoLiberacion.Items.Insert(0, new ListItem("SELECCIONE...", "-1"));
        }

        private void LoadPruebas()
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                APrueba prueba = UserIntefaceToData();

                DsPruebas = catalogoPruebaCtrl.Retrieve(dctx, prueba, false);
                DsPruebas = ConfigureGridResults(DsPruebas);
                grdPruebas.DataSource = DsPruebas;
                grdPruebas.DataBind();
            }
            else
                ShowMessage(validateMessage.Substring(2), MessageType.Warning);
        }

        private DataSet ConfigureGridResults(DataSet ds)
        {
            DataTable tablePruebas = ds.Tables[0];

            tablePruebas.Columns.Add("NombreModelo");
            tablePruebas.Columns.Add("TipoPrueba");
            tablePruebas.Columns.Add("NombreEstadoLiberacion");           

            foreach (DataRow row in tablePruebas.Rows)
            {
                int modeloID = (int)Convert.ChangeType(row["ModeloID"], typeof(int));

                ListItem item = ddlModelo.Items.FindByValue(modeloID.ToString());
                if (item == null)
                    row["NombreModelo"] = "(no encontrado)";
                else
                    row["NombreModelo"] = item.Text;

                bool esDiagnostica = (bool)Convert.ChangeType(row["EsDiagnostica"], typeof(bool));
                if (esDiagnostica)
                    row["TipoPrueba"] = "DIAGNOSTICA";
                else
                    row["TipoPrueba"] = "FINAL";                

                EEstadoLiberacionPrueba estado = (EEstadoLiberacionPrueba)Convert.ChangeType(row["EstadoLiberacion"], typeof(byte));
                row["NombreEstadoLiberacion"] = estado.ToString();
            }

            DataColumn[] pk = new DataColumn[1];
            pk[0] = tablePruebas.Columns["PruebaID"];
            tablePruebas.PrimaryKey = pk;

            return ds;
        }

        #endregion

        #region *** UserInterface to Data ***
        private AModelo GetModeloFromUI()
        {
            AModelo modelo = null;

            int modeloId = 0;
            string valorModelo = ddlModelo.SelectedValue;

            if (int.TryParse(valorModelo, out modeloId))
            {
                ModeloCtrl modeloCtrl = new ModeloCtrl();
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("Activo", "true");
                DataSet dsModelo = modeloCtrl.Retrieve(dctx, modeloId, parametros);
                if (dsModelo.Tables[0].Rows.Count > 0)
                    modelo = modeloCtrl.LastDataRowToModelo(dsModelo);
            }
            return modelo;
        }

        private APrueba UserIntefaceToData()
        {
            AModelo modelo = GetModeloFromUI();

            APrueba prueba = null;

            if (modelo == null)
                prueba = new PruebaFilter();    //Objeto para filtros
            else
                prueba = this.CreateObjectPrueba(modelo, null);

            int id = 0;
            if (int.TryParse(txtID.Text.Trim(), out id))
                prueba.PruebaID = id;
            if (txtNombre.Text.Trim().Length > 0)
                prueba.Nombre = string.Format("%{0}%", txtNombre.Text.Trim());
            if (txtClave.Text.Trim().Length > 0)
                prueba.Clave = string.Format("%{0}%", txtClave.Text.Trim());
            if (ddlEstadoLiberacion.SelectedIndex != -1)
            {
                id = 0;
                if (int.TryParse(ddlEstadoLiberacion.SelectedItem.Value, out id))
                    if (id > -1)
                        prueba.EstadoLiberacionPrueba = (EEstadoLiberacionPrueba)id;
            }

            this.lastObjectFilter = new PruebaFilter();
            this.lastObjectFilter.PruebaID = prueba.PruebaID;
            this.lastObjectFilter.Clave = prueba.Clave == null ? null : prueba.Clave.Replace("%", "");
            this.lastObjectFilter.Nombre = prueba.Nombre == null ? null : prueba.Nombre.Replace("%", "");
            this.lastObjectFilter.EstadoLiberacionPrueba = prueba.EstadoLiberacionPrueba;
            this.lastObjectFilter.ModeloID = modelo == null ? null : modelo.ModeloID;

            return prueba;
        }


        #endregion

        #region Métodos auxiliares
        private APrueba CreateObjectPrueba(AModelo modeloPrueba, int? pruebaID)
        {
            APrueba pruebaReturn = null;
           if (modeloPrueba is ModeloDinamico)
            {
                PruebaDinamica pruebaDinamica = new PruebaDinamica { PruebaID = pruebaID };
                pruebaReturn = pruebaDinamica;
            }

            if (pruebaReturn != null)
                pruebaReturn.Modelo = modeloPrueba;
            return pruebaReturn;
        }

        private void ConfigurarPrueba(int pruebaConfigID)
        {
            if (LastModeloSelect is ModeloDinamico)
            {
                if ((LastModeloSelect as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.PUNTOS)
                    Response.Redirect("ConfigurarMetodoPuntos.aspx");
                else if ((LastModeloSelect as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.PORCENTAJE)
                    Response.Redirect("ConfigurarMetodoPorcentaje.aspx");
                else if ((LastModeloSelect as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.CLASIFICACION)
                    Response.Redirect("ConfigurarMetodoClasificacion.aspx");
                else if ((LastModeloSelect as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.SELECCION)
                    Response.Redirect("ConfigurarMetodoSeleccion.aspx");
            }
        }

        private void EliminarPrueba(APrueba prueba)
        {
            try
            {
                this.catalogoPruebaCtrl.Delete(dctx, prueba);
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                ShowMessage(ex.Message, MessageType.Error);
            }
            finally
            {
                this.LoadPruebas();
            }
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
            PnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {

            grdPruebas.Visible = true;
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