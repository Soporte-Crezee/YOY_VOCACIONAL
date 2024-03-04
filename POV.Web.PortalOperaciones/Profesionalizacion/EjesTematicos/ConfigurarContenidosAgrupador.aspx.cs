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
using POV.Seguridad.BO;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.Service;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.Operaciones.Service;

namespace POV.Web.PortalOperaciones.Profesionalizacion.EjesTematicos
{
    public partial class ConfigurarContenidosAgrupador : PageBase
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private TipoDocumentoCtrl tipoDocumentoCtrl;
        private ContenidoDigitalCtrl contenidoDigitalCtrl;

        public DataSet DsContenidosCatalogo
        {
            set { Session["dsContenidosBusqueda"] = value; }
            get { return Session["dsContenidosBusqueda"] != null ? Session["dsContenidosBusqueda"] as DataSet : null; }
        }

        public DataTable DtContenidosAgrupador
        {
            set { Session["dsContenidosAgrupador"] = value; }
            get { return Session["dsContenidosAgrupador"] != null ? Session["dsContenidosAgrupador"] as DataTable : null; }
        }

        private EjeTematico LastEjeTematico
        {
            set { Session["lastEjeTematico"] = value; }
            get { return Session["lastEjeTematico"] != null ? Session["lastEjeTematico"] as EjeTematico : null; }
        }

        private SituacionAprendizaje LastSituacionAprendizaje
        {
            get { return Session["lastSituacionAprendizaje"] != null ? Session["lastSituacionAprendizaje"] as SituacionAprendizaje : null; }
            set { Session["lastSituacionAprendizaje"] = value; }
        }

        private AAgrupadorContenidoDigital LastAgrupador
        {
            get { return Session["lastAgrupador"] != null ? Session["lastAgrupador"] as AAgrupadorContenidoDigital : null; }
            set { Session["lastAgrupador"] = value; }
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }

        public ConfigurarContenidosAgrupador() 
		{
            tipoDocumentoCtrl = new TipoDocumentoCtrl();
            contenidoDigitalCtrl = new ContenidoDigitalCtrl();
		}

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (LastEjeTematico != null && LastEjeTematico.EjeTematicoID != null && LastSituacionAprendizaje != null &&
                    LastSituacionAprendizaje.SituacionAprendizajeID != null && LastAgrupador != null && LastAgrupador.AgrupadorContenidoDigitalID != null)
                {
                    LoadInformacionEjeSituacionAgrupador();
                    LoadTiposDocumento();
                }
                else
                {
                    Response.Redirect("ConsultarAgrupadoresContenido.aspx", true);
                }
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

        protected void BtnCancelar_OnClick(object sender, EventArgs e)
        {
            LimpiarObjetosSesion();
            Response.Redirect("ConsultarAgrupadoresContenido.aspx", true);
        }

        protected void grdContenidosBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "agregar":
                    {
                        try
                        {
                            ContenidoDigital contenidoDigital = new ContenidoDigital();
                            contenidoDigital.ContenidoDigitalID = long.Parse(e.CommandArgument.ToString());

                            ValidateAddContenido(contenidoDigital);

                            ContenidoDigitalAgrupador contenidoDigitalAgrupador = new ContenidoDigitalAgrupador();
                            contenidoDigitalAgrupador.Activo = true;
                            contenidoDigitalAgrupador.FechaRegistro = DateTime.Now;
                            contenidoDigitalAgrupador.AgrupadorContenidoDigital = LastAgrupador;
                            contenidoDigitalAgrupador.ContenidoDigital = contenidoDigital;
                            contenidoDigitalAgrupador.SituacionAprendizaje = LastSituacionAprendizaje;
                            contenidoDigitalAgrupador.EjeTematico = LastEjeTematico;

                            //PROCESO DE INSERCION
                            ConfigurarContenidosDigitalesCtrl configurarContenidosCtrl = new ConfigurarContenidosDigitalesCtrl();
                            configurarContenidosCtrl.InsertComplete(dctx, contenidoDigitalAgrupador);

                            LoadInformacionEjeSituacionAgrupador();
                            ShowMessage("El recurso didáctico fue agregado con éxito.", MessageType.Information);
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message, MessageType.Error);
                        }
                        break;
                    }
                case "Sort":
                    {
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }

            }
        }

        protected void grdContenidosBusqueda_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataSet dataSet = DsContenidosCatalogo;
            if (dataSet != null)
            {
                DataView dataView = new DataView(dataSet.Tables[0]);
                dataView.Sort = e.SortExpression + " " + GetSortDirection();
                DsContenidosCatalogo.Tables.Clear();
                DsContenidosCatalogo.Tables.Add(dataView.ToTable());
                grdContenidosBusqueda.DataSource = DsContenidosCatalogo;
                grdContenidosBusqueda.DataBind();
            }
        }

        protected void grdContenidosAgrupador_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "quitar":
                    {
                        try
                        {
                            ContenidoDigital contenidoDigital = new ContenidoDigital();
                            contenidoDigital.ContenidoDigitalID = long.Parse(e.CommandArgument.ToString());

                            ContenidoDigitalAgrupador contenidoDigitalAgrupador = new ContenidoDigitalAgrupador();
                            contenidoDigitalAgrupador.AgrupadorContenidoDigital = LastAgrupador;
                            contenidoDigitalAgrupador.ContenidoDigital = contenidoDigital;
                            contenidoDigitalAgrupador.SituacionAprendizaje = LastSituacionAprendizaje;
                            contenidoDigitalAgrupador.EjeTematico = LastEjeTematico;

                            //CONTROLADOR DE MARIO
                            ConfigurarContenidosDigitalesCtrl configurarContenidosCtrl = new ConfigurarContenidosDigitalesCtrl();
                            configurarContenidosCtrl.DeleteComplete(dctx, contenidoDigitalAgrupador);

                            LoadInformacionEjeSituacionAgrupador();
                            ShowMessage("El recurso didáctico fué eliminado con éxito.", MessageType.Information);
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message, MessageType.Error);
                        }
                        break;
                    }
                case "Sort":
                    {
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }

            }
        }

        protected void grdContenidosAgrupador_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable datatable = DtContenidosAgrupador;
            if (datatable != null)
            {
                DataView dataView = new DataView(datatable);
                dataView.Sort = e.SortExpression + " " + GetSortDirection();
                DtContenidosAgrupador= dataView.ToTable();
                grdContenidosBusqueda.DataSource = DsContenidosCatalogo;
                grdContenidosBusqueda.DataBind();
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
        #endregion
		
		#region *** validaciones ***
        private void ValidateAddContenido(ContenidoDigital contenidoDigital)
        {

            contenidoDigital = contenidoDigitalCtrl.LastDataRowToContenidoDigital(contenidoDigitalCtrl.Retrieve(dctx, contenidoDigital));

            if (contenidoDigital.EstatusContenido != EEstatusContenido.ACTIVO)
                throw new Exception("Solo se pueden agregar recursos en estado ACTIVO, por favor verifique.");

            SituacionAprendizajeCtrl situacionAprendizajeCtrl = new SituacionAprendizajeCtrl();
            SituacionAprendizaje situacionAprendizaje = situacionAprendizajeCtrl.RetrieveComplete(dctx, LastEjeTematico, LastSituacionAprendizaje);

            AAgrupadorContenidoDigital agrupador = (situacionAprendizaje.AgrupadorContenidoDigital as AgrupadorCompuesto).AgrupadoresContenido.FirstOrDefault(item => item.AgrupadorContenidoDigitalID == LastAgrupador.AgrupadorContenidoDigitalID);

            ContenidoDigital contenido = agrupador.ContenidosDigitales.FirstOrDefault(item => item.ContenidoDigitalID == contenidoDigital.ContenidoDigitalID);
            if (contenido != null)
                throw new Exception("El recurso didáctico ya ha sido agregado, por favor verifique.");
            

            
        }

        private string ValidateData()
        {
            string sError = string.Empty;

            

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
		
		#region *** Data to UserInterface ***
        private void LoadInformacionEjeSituacionAgrupador()
        {
            EjeTematicoCtrl ejeTematicoCtrl = new EjeTematicoCtrl();
            EjeTematico ejeTematico = ejeTematicoCtrl.RetrieveComplete(dctx, LastEjeTematico);

            SituacionAprendizajeCtrl situacionAprendizajeCtrl = new SituacionAprendizajeCtrl();
            SituacionAprendizaje situacionAprendizaje = situacionAprendizajeCtrl.RetrieveComplete(dctx, LastEjeTematico, LastSituacionAprendizaje);

            AAgrupadorContenidoDigital agrupador = (situacionAprendizaje.AgrupadorContenidoDigital as AgrupadorCompuesto).AgrupadoresContenido.FirstOrDefault(item => item.AgrupadorContenidoDigitalID == LastAgrupador.AgrupadorContenidoDigitalID);

            txtNivelEducativo.Text = ejeTematico.AreaProfesionalizacion.NivelEducativo.Descripcion;
            txtGrado.Text = ejeTematico.AreaProfesionalizacion.Grado.ToString() + "°";
            txtAsignatura.Text = ejeTematico.AreaProfesionalizacion.Nombre;
            txtBloque.Text = ejeTematico.MateriasProfesionalizacion.FirstOrDefault().Nombre;
            txtNombreEjeTematico.Text = ejeTematico.Nombre;
            txtNombreSituacion.Text = situacionAprendizaje.Nombre;
            txtNombreClasificador.Text = agrupador.Nombre;

            LoadContenidosAgrupador(agrupador.ContenidosDigitales);

        }

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

            grdContenidosBusqueda.DataSource = ConfigureGridResults(ds);
            grdContenidosBusqueda.DataBind();
        }

        private DataSet ConfigureGridResults(DataSet ds)
        {
            if (!ds.Tables[0].Columns.Contains("NombreTipoDocumento"))
                ds.Tables[0].Columns.Add("NombreTipoDocumento");
            if (!ds.Tables[0].Columns.Contains("Estado"))
                ds.Tables[0].Columns.Add("Estado");
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
                    TipoDocumento tipoDocumento = new TipoDocumento { TipoDocumentoID = int.Parse(row["TipoDocumentoID"].ToString()) };
                    tipoDocumento = tipoDocumentoCtrl.LastDataRowToTipoDocumento(tipoDocumentoCtrl.Retrieve(dctx, tipoDocumento));
                    row["NombreTipoDocumento"] = tipoDocumento.Nombre;
                    tiposDocumento.Add(tipoDocumento);
                }
                row["Estado"] = (EEstatusContenido)byte.Parse(row["EstatusContenido"].ToString());
                
            }
            return ds;
        }

        private void LoadContenidosAgrupador(List<ContenidoDigital> contenidosDigitales)
        {
            DataTable dtContenidos = new DataTable();
            dtContenidos.Columns.Add("ContenidoDigitalID");
            dtContenidos.Columns.Add("Clave");
            dtContenidos.Columns.Add("Nombre");
            dtContenidos.Columns.Add("InstitucionOrigen");
            dtContenidos.Columns.Add("NombreTipoDocumento");
            dtContenidos.Columns.Add("Estado");
            List<TipoDocumento> tiposDocumento = new List<TipoDocumento>();

            foreach (ContenidoDigital contenidoDigital in contenidosDigitales)
            {
                DataRow dr = dtContenidos.NewRow();
                dr[0] = contenidoDigital.ContenidoDigitalID.ToString();
                dr[1] = contenidoDigital.Clave;
                dr[2] = contenidoDigital.Nombre;
                dr[3] = contenidoDigital.InstitucionOrigen.Nombre;
                
                dr[5] = contenidoDigital.EstatusContenido;

                TipoDocumento tipoDoc = tiposDocumento.SingleOrDefault(a => a.TipoDocumentoID == contenidoDigital.TipoDocumento.TipoDocumentoID);
                if (tipoDoc != null)
                {
                    dr[4] = tipoDoc.Nombre;
                }
                else
                {
                    TipoDocumento tipoDocumento = new TipoDocumento { TipoDocumentoID = contenidoDigital.TipoDocumento.TipoDocumentoID };
                    tipoDocumento = tipoDocumentoCtrl.LastDataRowToTipoDocumento(tipoDocumentoCtrl.Retrieve(dctx, tipoDocumento));
                    dr[4] = tipoDocumento.Nombre;
                    tiposDocumento.Add(tipoDocumento);
                }

                dtContenidos.Rows.Add(dr);
            }

            DtContenidosAgrupador = dtContenidos;
            grdContenidosAgrupador.DataSource = dtContenidos;
            grdContenidosAgrupador.DataBind();
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
        #endregion
		
		#region *** metodos auxiliares ***
        private void LimpiarObjetosSesion()
        {
            DtContenidosAgrupador = null;
            DsContenidosCatalogo = null;
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

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOSITUACIONES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARSITUACIONES) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!creacion)
                redirector.GoToHomePage(true);
        }
        #endregion
    }
}