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
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.Service;
using System.Configuration;

namespace POV.Web.PortalOperaciones.Profesionalizacion.ContenidosDigitales
{
    public partial class BuscarContenidoDigital : CatalogPage
    {

        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private TipoDocumentoCtrl tipoDocumentoCtrl;
        private ContenidoDigitalCtrl contenidoDigitalCtrl;
        private CatalogoContenidosDigitalesCtrl catalogoContenidosDigitalesCtrl;


        public BuscarContenidoDigital()
        {
            tipoDocumentoCtrl = new TipoDocumentoCtrl();
            contenidoDigitalCtrl = new ContenidoDigitalCtrl();
            catalogoContenidosDigitalesCtrl = new CatalogoContenidosDigitalesCtrl();
        }

        #region *** propiedades de clase ***
        public DataSet DsContenidosCatalogo
        {
            set { Session["dsContenidosCatalogo"] = value; }
            get { return Session["dsContenidosCatalogo"] != null ? Session["dsContenidosCatalogo"] as DataSet : null; }
        }

        public ContenidoDigital LastObject
        {
            set { Session["lastContenidoDigital"] = value; }
            get { return Session["lastContenidoDigital"] != null ? Session["lastContenidoDigital"] as ContenidoDigital : null; }
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }
        #endregion

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DsContenidosCatalogo = null;
                LoadEstadosContenido();
                LoadTiposDocumento();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                ContenidoDigital contenidoDigital = UserIntefaceToData();
                DsContenidosCatalogo = contenidoDigitalCtrl.Retrieve(dctx, contenidoDigital);
                LoadContenidos(DsContenidosCatalogo);
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
                            ContenidoDigital contenido = new ContenidoDigital(){ContenidoDigitalID = int.Parse(e.CommandArgument.ToString())};
                            DoDelete(contenido);
                            ShowMessage("El recurso didáctico se eliminó con éxito", MessageType.Information);
                            DsContenidosCatalogo = contenidoDigitalCtrl.Retrieve(dctx, new ContenidoDigital {EstatusContenido = EEstatusContenido.ACTIVO});
                            LoadContenidos(DsContenidosCatalogo);
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message, MessageType.Error);
                        }
                        break;
                    }
                case "editar":
                    {
                        ContenidoDigital contenido = new ContenidoDigital { ContenidoDigitalID = long.Parse(e.CommandArgument.ToString()) };
                        LastObject = contenido;
                        Response.Redirect("EditarContenidoDigital.aspx", true);
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
            DataSet dataSet = DsContenidosCatalogo;
            if (dataSet != null)
            {
                DataView dataView = new DataView(dataSet.Tables[0]);
                dataView.Sort = e.SortExpression + " " + GetSortDirection();
                DsContenidosCatalogo.Tables.Clear();
                DsContenidosCatalogo.Tables.Add(dataView.ToTable());
                grdContenidos.DataSource = DsContenidosCatalogo;
                grdContenidos.DataBind();
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

        private void DoDelete(ContenidoDigital contenido)
        {
            try
            {
                string servidorContenidos = @ConfigurationManager.AppSettings["POVURLServidorContenidos"];
                string metodoServidor = @ConfigurationManager.AppSettings["POVMetodoServidorContenidos"];
                string userServidor = @ConfigurationManager.AppSettings["POVUserServidorContenidos"];
                string passervidor = @ConfigurationManager.AppSettings["POVPassServidorContenidos"];
                string carpetaContenido = @ConfigurationManager.AppSettings["POVRutaContenidos"];
                //si el metodo es local, mapeamos la ubicacion en el servidor
                if (metodoServidor.ToUpper().CompareTo("LOCAL") == 0)
                    servidorContenidos = Server.MapPath(servidorContenidos);
                else if (metodoServidor.ToUpper().CompareTo("AMAZONS3") == 0)
                {
                    userServidor = @ConfigurationManager.AppSettings["AWSAccessKey"];
                    passervidor = @ConfigurationManager.AppSettings["AWSSecretKey"];
                    servidorContenidos = @ConfigurationManager.AppSettings["BucketName"];
                }


                dctx = Helper.ConnectionHlp.Default.Connection;
                ContenidoDigital contenidoDigital =  contenidoDigitalCtrl.RetrieveComplete(dctx, contenido);
                catalogoContenidosDigitalesCtrl.DeleteComplete(dctx, contenidoDigital, servidorContenidos, userServidor, passervidor, metodoServidor, carpetaContenido);
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw new Exception("Ocurrió un error al eliminar el recurso didáctico: " + ex.Message);
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
                sError += " La clave debe ser menor a 30 caracteres";


            string nombre = txtNombre.Text.Trim();
            if (nombre.Length > 200)
                sError += " El nombre debe ser menor a 200 caracteres";

            string origen = txtInsitucion.Text.Trim();
            if (origen.Length > 200)
                sError += " El nombre debe ser menor a 200 caracteres";

            return sError;
        }
        #endregion

        #region *** Data to UserInteface ***
        private void LoadTiposDocumento()
        {
            DataSet ds = tipoDocumentoCtrl.Retrieve(dctx, new TipoDocumento { Activo = true });
            DDLTipoDocumento.DataSource = ds;
            DDLTipoDocumento.DataValueField = "TipoDocumentoID";
            DDLTipoDocumento.DataTextField = "Nombre";
            DDLTipoDocumento.DataBind();
            DDLTipoDocumento.Items.Insert(0, new ListItem("SELECCIONE...", "-1"));
        }
        /// <summary>
        /// Carga los datos del resultado de busqueda en el gridview de la interfaz grafica
        /// </summary>
        /// <param name="ds">DataSet con la informacion del los resultados de la busqueda</param>
        private void LoadContenidos(DataSet ds)
        {
            string query = "EstatusContenido =" + (byte)EEstatusContenido.INACTIVO;
            DataRow[] dr = ds.Tables[0].Select(query);

            if (dr != null && dr.Count() > 0)
            {
                foreach (DataRow drJuego in dr)
                    ds.Tables[0].Rows.Remove(drJuego);
            }

            grdContenidos.DataSource = ConfigureGridResults(ds);
            grdContenidos.DataBind();
        }

        private DataSet ConfigureGridResults(DataSet ds)
        {
            if (!ds.Tables[0].Columns.Contains("NombreTipoDocumento"))
                ds.Tables[0].Columns.Add("NombreTipoDocumento");
            if (!ds.Tables[0].Columns.Contains("Estado"))
                ds.Tables[0].Columns.Add("Estado");
            if (!ds.Tables[0].Columns.Contains("Referencia"))
                ds.Tables[0].Columns.Add("Referencia");
            List<TipoDocumento> tiposDocumento = new List<TipoDocumento>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                TipoDocumento ar = tiposDocumento.SingleOrDefault(a => a.TipoDocumentoID == int.Parse(row["TipoDocumentoID"].ToString()));
                if (ar != null)
                {
                    row["NombreTipoDocumento"] = ar.Nombre;
                }
                else
                {
                    TipoDocumento nivel = new TipoDocumento { TipoDocumentoID = int.Parse(row["TipoDocumentoID"].ToString()) };
                    nivel = tipoDocumentoCtrl.LastDataRowToTipoDocumento(tipoDocumentoCtrl.Retrieve(dctx, nivel));
                    row["NombreTipoDocumento"] = nivel.Nombre;
                    tiposDocumento.Add(nivel);
                }
                row["Estado"] = (EEstatusContenido)byte.Parse(row["EstatusContenido"].ToString());
                row["Referencia"] = bool.Parse(row["EsInterno"].ToString())? "INTERNO" : "EXTERNO";
            }
            return ds;
        }

        private void LoadEstadosContenido()
        {
            var valenum = Enum.GetValues(typeof(EEstatusContenido));
            foreach (byte en in valenum)
            {
                EEstatusContenido est = (EEstatusContenido)en;
                if (est != EEstatusContenido.INACTIVO)
                    DDLEstatusContenido.Items.Add(new ListItem(est.ToString(), en.ToString()));
            }
            DDLEstatusContenido.Items.Insert(0, new ListItem("SELECCIONE...", ""));

        }
        #endregion

        #region *** UserInterface to Data ***
        /// <summary>
        /// Toma la informacion de los controles de interfaz grafica para formar un objeto de tipo TipoDocumento
        /// </summary>
        /// <returns>TipoDocumento</returns>
        private TipoDocumento GetTipoDocumentoFromUI()
        {
            TipoDocumento tipo = null;

            int tipoDocumentoID = 0;
            string valorTipoDocumento = DDLTipoDocumento.SelectedValue;

            if (int.TryParse(valorTipoDocumento, out tipoDocumentoID))
            {
                if (tipoDocumentoID < 0)
                    tipo = new TipoDocumento();
                else
                    tipo = new TipoDocumento { TipoDocumentoID = tipoDocumentoID };
            }
            return tipo;
        }
        /// <summary>
        /// Toma la información de los controles de interfaz grafica para formar un objeto de tipo ContenidoDigital
        /// </summary>
        /// <returns>Contenido digital a partir de los controles de interfaz grafica</returns>
        private ContenidoDigital UserIntefaceToData()
        {
            ContenidoDigital contenido = new ContenidoDigital();
            contenido.InstitucionOrigen = new InstitucionOrigen();
            contenido.EstatusContenido = GetEstadoContenidoFromUI();
            

            string sID = txtID.Text.Trim();
            long id = 0;
            if (!string.IsNullOrEmpty(sID) && long.TryParse(sID, out id))
                contenido.ContenidoDigitalID = id;
            if (!string.IsNullOrEmpty(txtClave.Text.Trim()))
                contenido.Clave = txtClave.Text.Trim();
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
                contenido.Nombre = "%" + txtNombre.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(txtInsitucion.Text.Trim()))
                contenido.InstitucionOrigen.Nombre = "%" + txtInsitucion.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(txtEtiquetas.Text.Trim()))
                contenido.Tags = "%" + txtEtiquetas.Text.Trim() + "%";
            if (GetTipoDocumentoFromUI().TipoDocumentoID != null)
                contenido.TipoDocumento = GetTipoDocumentoFromUI();

            return contenido;
        }
        /// <summary>
        /// Toma la informacion de los controles de interfaz grafica para formar un objeto de tipo EEstatusContenido
        /// </summary>
        /// <returns></returns>
        private EEstatusContenido? GetEstadoContenidoFromUI()
        {
            byte estado = 0;

            if (byte.TryParse(DDLEstatusContenido.SelectedItem.Value, out estado))
                return (EEstatusContenido)byte.Parse(DDLEstatusContenido.SelectedItem.Value);

            return null;
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

            grdContenidos.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCONTENIDOSDIGITALES) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARCONTENIDOSDIGITALES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARCONTENIDOSDIGITALES) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARCONTENIDOSDIGITALES) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCONTENIDOSDIGITALES) != null;

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