using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using Framework.Base.DataAccess;
using POV.Operaciones.Service;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.Modelo.BO;
using POV.Modelo.Service;
using System.Configuration;

namespace POV.Web.PortalOperaciones.CatalogoReactivos.Dinamico
{
    public partial class BuscarReactivos : CatalogPage
    {
        private ReactivoCtrl reactivoCtrl;

        private ModeloCtrl modeloCtrl;

        #region *** propiedades de clase ***
        public DataSet DsReactivos
        {
            set { Session["dsReactivosDinamico"] = value; }
            get { return Session["dsReactivosDinamico"] != null ? Session["dsReactivosDinamico"] as DataSet : null; }
        }

        public Reactivo LastObject
        {
            set { Session["lastReactivoDinamico"] = value; }
            get { return Session["lastReactivoDinamico"] != null ? Session["lastReactivoDinamico"] as Reactivo : null; }
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }
        #endregion

        public BuscarReactivos()
        {
            reactivoCtrl = new ReactivoCtrl();
            modeloCtrl = new ModeloCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Reactivo reactivo = new Reactivo();
                reactivo.Caracteristicas = new CaracteristicasModeloGenerico();
                reactivo.Asignado = false;
                reactivo.Activo = true;
                reactivo.TipoReactivo = ETipoReactivo.ModeloGenerico;

                DsReactivos = reactivoCtrl.Retrieve(ConnectionHlp.Default.Connection, reactivo);
                LoadModelos();

                LoadReactivos(DsReactivos);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                Reactivo reactivo = UserIntefaceToData();
                DsReactivos = reactivoCtrl.Retrieve(ConnectionHlp.Default.Connection, reactivo);
                LoadReactivos(DsReactivos);
            }
            else
                ShowMessage(validateMessage.Substring(2), MessageType.Warning);
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        try
                        {
                            Reactivo reactivo = new Reactivo();
                            reactivo.ReactivoID = Guid.Parse(e.CommandArgument.ToString());
                            reactivo.Caracteristicas = new CaracteristicasModeloGenerico();
                            reactivo.TipoReactivo = ETipoReactivo.ModeloGenerico;

                            string servidorContenidos = @ConfigurationManager.AppSettings["POVURLServidorContenidos"];
                            string metodoServidor = @ConfigurationManager.AppSettings["POVMetodoServidorContenidos"];
                            string userServidor = @ConfigurationManager.AppSettings["POVUserServidorContenidos"];
                            string passervidor = @ConfigurationManager.AppSettings["POVPassServidorContenidos"];
                            //si el metodo es local, mapeamos la ubicacion en el servidor
                            if (metodoServidor.ToUpper().CompareTo("LOCAL") == 0)
                                servidorContenidos = Server.MapPath(servidorContenidos);
                            else if (metodoServidor.ToUpper().CompareTo("AMAZONS3") == 0)
                            {
                                userServidor = @ConfigurationManager.AppSettings["AWSAccessKey"];
                                passervidor = @ConfigurationManager.AppSettings["AWSSecretKey"];
                                servidorContenidos = @ConfigurationManager.AppSettings["BucketName"];
                            }

                            CatalogoReactivosDinamicoCtrl catalogoReactivosDinamicoCtrl = new CatalogoReactivosDinamicoCtrl();
                            catalogoReactivosDinamicoCtrl.DeleteComplete(ConnectionHlp.Default.Connection, reactivo, servidorContenidos, userServidor, passervidor, metodoServidor);

                            RemoveReactivoFromDataSet(DsReactivos, reactivo);
                            grdReactivos.DataSource = DsReactivos;
                            grdReactivos.DataBind();
                            ShowMessage("El reactivo se eliminó con éxito", MessageType.Information);

                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message, MessageType.Error);
                        }
                        break;
                    }
                case "editar":
                    {
                        Reactivo reactivo = new Reactivo();
                        reactivo.ReactivoID = Guid.Parse(e.CommandArgument.ToString());
                        reactivo.Caracteristicas = new CaracteristicasModeloGenerico();
                        reactivo.Activo = true;
                        reactivo.TipoReactivo = ETipoReactivo.ModeloGenerico;
                        LastObject = reactivo;

                        Response.Redirect("EditarReactivoOpcion.aspx", true);
                        break;
                    }

                case "Sort":
                    {
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }

            }
        }

        protected void grd_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataSet dataSet = DsReactivos;
            if (dataSet != null)
            {
                DataView dataView = new DataView(dataSet.Tables[0]);
                dataView.Sort = e.SortExpression + " " + GetSortDirection();
                DsReactivos.Tables.Clear();
                DsReactivos.Tables.Add(dataView.ToTable());
                grdReactivos.DataSource = DsReactivos;
                grdReactivos.DataBind();
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

        private void RemoveReactivoFromDataSet(DataSet ds, Reactivo reactivo)
        {
            DataRow drRemove = null;
            foreach (DataRow dr in ds.Tables["Reactivo"].Rows)
            {
                if ((Guid)Convert.ChangeType(dr["ReactivoID"], typeof(Guid)) == reactivo.ReactivoID.Value)
                {
                    drRemove = dr;
                    break;
                }

            }

            if (drRemove != null)
                ds.Tables["Reactivo"].Rows.Remove(drRemove);
        }
        #endregion

        #region *** validaciones ***
        private string ValidateData()
        {
            string sError = string.Empty;

            

            string clave = txtClave.Text.Trim();
            if (clave.Length > 30)
                sError += " La clave debe ser menor a 30 caracteres";


            string nombre = txtNombre.Text.Trim();
            if (nombre.Length > 200)
                sError += " El nombre debe ser menor a 200 caracteres";



            return sError;
        }
        #endregion

        #region *** Data to UserInteface ***
        private void LoadModelos()
        {
            DataSet ds = modeloCtrl.Retrieve(ConnectionHlp.Default.Connection, new ModeloDinamico { Estatus = true });
            ddlModelo.DataSource = ds;
            ddlModelo.DataValueField = "ModeloID";
            ddlModelo.DataTextField = "Nombre";
            ddlModelo.DataBind();
            ddlModelo.Items.Insert(0, new ListItem("SELECCIONE...", "-1"));
        }

        private DataSet ConfigureGridResults(DataSet ds)
        {
            if (!ds.Tables[0].Columns.Contains("NombreModelo"))
                ds.Tables[0].Columns.Add("NombreModelo");
            if (!ds.Tables[0].Columns.Contains("MetodoCalificacion"))
                ds.Tables[0].Columns.Add("MetodoCalificacion");
            List<ModeloDinamico> modelos = new List<ModeloDinamico>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ModeloDinamico ar = modelos.SingleOrDefault(a => a.ModeloID == int.Parse(row["ModeloID"].ToString()));
                if (ar != null)
                {
                    row["NombreModelo"] = ar.Nombre;
                    row["MetodoCalificacion"] = ar.MetodoCalificacion.ToString();
                }
                else
                {
                    ModeloDinamico modeloDinamico = new ModeloDinamico { ModeloID = int.Parse(row["ModeloID"].ToString()) };
                    modeloDinamico = modeloCtrl.LastDataRowToModelo(modeloCtrl.Retrieve(ConnectionHlp.Default.Connection, modeloDinamico)) as ModeloDinamico;
                    row["NombreModelo"] = modeloDinamico.Nombre;
                    row["MetodoCalificacion"] = modeloDinamico.MetodoCalificacion.ToString();
                    modelos.Add(modeloDinamico);
                }

            }
            return ds;
        }

        private void LoadReactivos(DataSet dsReactivos)
        {
            grdReactivos.DataSource = ConfigureGridResults(dsReactivos);
            grdReactivos.DataBind();
        }

        #endregion

        #region *** UserInterface to Data ***


        private ModeloDinamico GetModeloFromUI()
        {
            ModeloDinamico modeloDinamico = null;

            int modeloID = 0;
            string valorModeloID = ddlModelo.SelectedValue;

            if (int.TryParse(valorModeloID, out modeloID))
            {
                if (modeloID < 0)
                    modeloDinamico = new ModeloDinamico();
                else
                    modeloDinamico = new ModeloDinamico { ModeloID = modeloID };
            }
            return modeloDinamico;
        }


        private Reactivo UserIntefaceToData()
        {
            Reactivo reactivo = new Reactivo();
            reactivo.Asignado = false;
            reactivo.Activo = true;
            reactivo.TipoReactivo = ETipoReactivo.ModeloGenerico;
            reactivo.Caracteristicas = new CaracteristicasModeloGenerico();


            if (!string.IsNullOrEmpty(txtClave.Text.Trim()))
                reactivo.Clave = txtClave.Text.Trim();
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
                reactivo.NombreReactivo = "%" + txtNombre.Text.Trim() + "%";

            if (GetModeloFromUI().ModeloID != null)
                (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo = GetModeloFromUI();

            return reactivo;
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

            grdReactivos.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOREACTIVOSDINAMICO) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARREACTIVOSDINAMICO) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARREACTIVOSDINAMICO) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARREACTIVOSREACTIVOSDINAMICO) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARREACTIVOSREACTIVOSDINAMICO) != null;

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