using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using Framework.Base.DataAccess;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Core.Operaciones;
using POV.Core.Operaciones.Interfaces;
using POV.Core.Operaciones.Implements;
using POV.Prueba.Diagnostico.Service;
using POV.Reactivos.BO;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Modelo.Estandarizado.BO;

namespace POV.Web.PortalOperaciones.CatalogoModeloPrueba
{
    public partial class BuscarModeloPrueba : System.Web.UI.Page
    {
        #region Propiedades
        AModelo LastObject
        {
            get { return Session["lastModelo"] != null ? Session["lastModelo"] as ModeloDinamico : null; }
            set { Session["lastModelo"] = value; }
        }
        public DataSet DsModelos
        {
            set { Session["modelos"] = value; }
            get { return Session["modelos"] != null ? Session["modelos"] as DataSet : null; }
        }
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }
        private IRedirector redirector;
        private IUserSession userSession;
        private ModeloCtrl modeloPruebaCtrl;
        private IDataContext dctx;
        
        public BuscarModeloPrueba()
        {
            redirector = new Redirector();
            modeloPruebaCtrl = new ModeloCtrl();
            userSession = new UserSession();
        }
        #endregion

        #region *****> Eventos de Pagina <*****
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LastObject = null;
                    DsModelos = null;
                    LoadModelos();
                }
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                redirector.GoToHomePage(true);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                LoadModelos();
            }
            else
                ShowMessage(validateMessage.Substring(2), MessageType.Warning);
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "clasificador":
                        LastObject = new ModeloDinamico { ModeloID = int.Parse(e.CommandArgument.ToString()) };
                        Response.Redirect("~/CatalogoClasificador/BuscarClasificadorModelo.aspx", true);
                        break;
                    case "editar":
                        LastObject = new ModeloDinamico { ModeloID = int.Parse(e.CommandArgument.ToString()) };
                        Response.Redirect("~/CatalogoModeloPrueba/EditarModeloPrueba.aspx", true);
                        break;
                    case "eliminar":
                        dctx = Helper.ConnectionHlp.Default.Connection;
                        ModeloDinamico modeloPrueba = new ModeloDinamico { ModeloID = int.Parse(e.CommandArgument.ToString()) };
                        try
                        {
                            Dictionary<string, string> parametros = new Dictionary<string, string>();
                            parametros.Add("Activo", "true");
                            AModelo modelo = new ModeloDinamico() { ModeloID = int.Parse(e.CommandArgument.ToString()) };
                            modelo = modeloPruebaCtrl.LastDataRowToModelo(modeloPruebaCtrl.Retrieve(dctx, modelo.ModeloID, parametros));

                            if (modelo.EsEditable == true)
                            {
                                CatalogoPruebaCtrl catalogoPruebaCtrl = new CatalogoPruebaCtrl();
                                DataSet ds = catalogoPruebaCtrl.RetrievePruebasModelo(dctx, modelo.ModeloID, parametros, EEstadoLiberacionPrueba.ACTIVA);

                                if (ds.Tables["PruebasModelo"].Rows.Count > 0)
                                {
                                    ShowMessage("El modelo tiene Pruebas asignadas", MessageType.Error);
                                }
                                else
                                {
                                    Reactivo filtro = new Reactivo();
                                    filtro.Activo = true;
                                    filtro.TipoReactivo = ETipoReactivo.ModeloGenerico;                                    
                                    filtro.Caracteristicas = new CaracteristicasModeloGenerico() { Modelo = modelo as ModeloDinamico };
                                    List<Reactivo> ls = catalogoPruebaCtrl.RetrieveListaReactivos(dctx, modelo, filtro, false);

                                    if (ls.Count() > 0)
                                    {
                                        ShowMessage("El modelo tiene Reactivos asignados", MessageType.Error);
                                    }
                                    else
                                    {
                                        modeloPruebaCtrl.DeleteModeloDinamico(dctx, modeloPrueba);
                                        LoadModelos();
                                    }
                                }
                            }
                            else
                            {
                                ShowMessage("El modelo NO es editable", MessageType.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message, MessageType.Error);
                        }
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

        protected void grd_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                if (RenderEdit)
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");                    
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");                    
                    LinkButton lnkClasificador = (LinkButton)e.Row.FindControl("lnkClasificador");
                    lnkClasificador.Visible = true;

                    if (rowView["NombreEsEditable"].ToString() == "SI")
                    {
                        if (rowView["TipoModelo"].ToString() == "Dinamico")
                        {
                            lnkClasificador.Visible = true;
                            btnEdit.Visible = true;
                            btnDelete.Visible = true;
                        }
                        else 
                        {
                            lnkClasificador.Visible = false;
                            btnEdit.Visible = false;
                            btnDelete.Visible = false;
                        }                        
                    }
                    else
                    {
                        lnkClasificador.Visible = false;
                        btnEdit.Visible = false;
                        btnDelete.Visible = false;
                    }                        
                }

                if (RenderDelete)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");
                    btnDelete.Visible = true;
                }
            }
        }
        #endregion

        #region *****> UserInterface to Data <*****
        private DataSet UserInterfaceToData()
        {
            int? modeloID = null;
            dctx = Helper.ConnectionHlp.Default.Connection;

            ModeloCtrl modeloCtrl = new ModeloCtrl();
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("Activo", "true");

            if (!string.IsNullOrEmpty(txtID.Text.Trim()))
                modeloID = int.Parse(txtID.Text.Trim());

            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
                parametros.Add("Nombre", string.Format("%{0}%", txtNombre.Text.Trim()));

            DataSet dsModelos = modeloCtrl.Retrieve(dctx, modeloID, parametros); //new DataSet();

            DataSet dsReturn = new DataSet();
            dsReturn.Tables.Add(new DataTable());
            dsReturn.Tables[0].Columns.Add("ModeloID");
            dsReturn.Tables[0].Columns.Add("NombreModelo");
            dsReturn.Tables[0].Columns.Add("NombreEsEditable");
            dsReturn.Tables[0].Columns.Add("TipoModelo");

            if (dsModelos != null && dsModelos.Tables.Count > 0)
            {
                foreach (DataRow modelo in dsModelos.Tables[0].Rows)
                {
                    AModelo modeloD = modeloPruebaCtrl.DataRowToAModelo(modelo);
                    DataRow row = dsReturn.Tables[0].NewRow();
                    row["ModeloID"] = int.Parse(modeloD.ModeloID.ToString());
                    row["NombreModelo"] = modeloD.Nombre;

                    bool esEditable = (bool)Convert.ChangeType(modeloD.EsEditable, typeof(bool));
                    if (esEditable)
                        row["NombreEsEditable"] = "SI";
                    else
                        row["NombreEsEditable"] = "NO";

                    row["TipoModelo"] = modeloD.TipoModelo.ToString();
                    dsReturn.Tables[0].Rows.InsertAt(row, 0);
                }
            }
            
            return dsReturn;
        }
        #endregion

        #region *****> Métodos Auxiliares <*****        
        private void LoadModelos()
        {
            DsModelos = UserInterfaceToData();

            DsModelos.Tables[0].DefaultView.Sort = "ModeloID ASC";
            DataTable dt = DsModelos.Tables[0].DefaultView.ToTable();
            DsModelos.Tables[0].Rows.Clear();
            foreach (DataRow row in dt.Rows)
                DsModelos.Tables[0].Rows.Add(row.ItemArray);
                                                           
            grdModelosPrueba.DataSource = DsModelos;
            grdModelosPrueba.DataBind();
            grdModelosPrueba.Visible = true;                                    
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
        
        #region *** validaciones ***
        private string ValidateData()
        {
            string sError = string.Empty;

            return sError;
        }

        private string ValidatePruebasModelo()
        {
            string sError = string.Empty;

            CatalogoPruebaCtrl catalogoPruebaCtrl = new CatalogoPruebaCtrl();
          
            return sError;
        }
        #endregion
                                
        private bool RenderEdit = true;
        private bool RenderDelete = false;

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