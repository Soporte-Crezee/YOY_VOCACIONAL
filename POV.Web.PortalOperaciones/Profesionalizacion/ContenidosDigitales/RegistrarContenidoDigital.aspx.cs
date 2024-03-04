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
using POV.ContenidosDigital.Service;
using POV.ContenidosDigital.BO;
using POV.Operaciones.Service;
using POV.Comun.Service;
using POV.Comun.BO;
using System.Configuration;

namespace POV.Web.PortalOperaciones.Profesionalizacion.ContenidosDigitales
{
    public partial class RegistrarContenidoDigital : PageBase
    {
        private TipoDocumentoCtrl tipoDocumentoCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private CatalogoContenidosDigitalesCtrl catalogoContenidosDigitalesCtrl;


        public RegistrarContenidoDigital()
        {
            tipoDocumentoCtrl = new TipoDocumentoCtrl();
            catalogoContenidosDigitalesCtrl = new CatalogoContenidosDigitalesCtrl();
        }
        
        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTiposDocumento();

            }
        }

        protected void BtnGuardar_OnClick(object sender, EventArgs e)
        {
            string sError = ValidateContenidoDigital();
            
            if (string.IsNullOrEmpty(sError))
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
                    ContenidoDigital contenidoDigital = GetContenidoDigitalFromUI();
                    FileWrapper file = null;
                    if (contenidoDigital.EsInterno.Value)
                        file = GetArchivoFromUI();

                    catalogoContenidosDigitalesCtrl.InsertComplete(dctx, contenidoDigital, servidorContenidos, userServidor, passervidor, metodoServidor, carpetaContenido, file);

                    

                    txtRedirect.Value = "BuscarContenidoDigital.aspx";
                    ShowMessage("Registro exitoso.", MessageType.Information);
                }
                catch (Exception ex)
                {
                    txtRedirect.Value = "";
                    ShowMessage("Error: " + ex.Message, MessageType.Error);
                }

            }
            else
            {
                txtRedirect.Value = "";
                ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
            }
        }

        protected void BtnCancelar_OnClick(object sender, EventArgs e)
        {
            
            Response.Redirect("BuscarContenidoDigital.aspx", true);
        }
        #endregion

        #region *** validaciones ***
        private string ValidateContenidoDigital()
        {
            string sError = string.Empty;

            string sClave = TxtClave.Text.Trim();
            string sNombre = TxtNombre.Text.Trim();
            string sOrigen = TxtIntitucion.Text.Trim();
            string sTags = TxtEtiquetas.Text.Trim();
            string sReferencia = RdBtnTipoReferencia.SelectedValue;
            string sDescargable = RdBtnEsDescargable.SelectedValue;
            TipoDocumento tipoDocumento = GetTipoDocumentoFromUI();

            if (string.IsNullOrEmpty(sClave))
                sError += ", Clave Requerida ";
            if (string.IsNullOrEmpty(sNombre))
                sError += ", Nombre es Requerido ";
            
            if (string.IsNullOrEmpty(sReferencia))
                sError += ", Referencia es Requerido ";
            if (string.IsNullOrEmpty(sDescargable))
                sError += ", Tipo de descarga es Requerido ";
            if (tipoDocumento.TipoDocumentoID == null)
                sError += ", Tipo de documento es Requerido ";

            if (string.IsNullOrEmpty(sTags))
                sError += ", Etiquetas es Requerida ";
            else
            {
                string [] lsTags = sTags.Trim().Split(',');
                
                foreach (string sTag in lsTags)
                {
                    if (lsTags.Count(item => item == sTag) > 1)
                    {
                        sError += ", No se pueden repetir las etiquetas";
                        break;
                    }
                }
            }

            bool esDescargable = true;
            bool esInterno = true;

            if (!bool.TryParse(sReferencia, out esInterno))
                sError += ", Referencia es Requerido ";
            if (!bool.TryParse(sDescargable, out esDescargable))
                sError += ", Tipo de descarga es Requerido ";

            if (string.IsNullOrEmpty(sError))
            {
                //validamos el tipo de documento si es interno
                if (esInterno)
                {
                    HttpPostedFile file11 = fupArchivoInterno.PostedFile;

                    if (file11 != null && !string.IsNullOrEmpty(file11.FileName))
                    {
                        //si el mime y la extension corresponden
                        bool validFile = file11.ContentType.CompareTo(tipoDocumento.MIME) == 0 && file11.FileName.ToLower().EndsWith(tipoDocumento.Extension.ToLower());

                        if (!validFile)
                            sError += ", El archivo seleccionado no tiene el formato o extensión correcta ";

                    }
                    else
                        sError += ", El archivo seleccionado no tiene el formato o extensión correcta ";
                }
                else
                {
                    if (string.IsNullOrEmpty(TxtReferenciaExterna.Text.Trim()))
                        sError += ", Referencia Externa Requerida ";
                }
            }
            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void LoadTiposDocumento()
        {
            DataSet dsTiposDocumento = tipoDocumentoCtrl.Retrieve(dctx, new TipoDocumento { Activo = true });
            DDLTipoDocumento.DataSource = dsTiposDocumento;
            DDLTipoDocumento.DataTextField = "Nombre";
            DDLTipoDocumento.DataValueField = "TipoDocumentoID";
            DDLTipoDocumento.DataBind();
            DDLTipoDocumento.Items.Insert(0, new ListItem("SELECCIONE...", ""));
        }


        #endregion

        #region *** UserInterface to Data ***

        private ContenidoDigital GetContenidoDigitalFromUI()
        {
            ContenidoDigital contenidoDigital = new ContenidoDigital();
            contenidoDigital.InstitucionOrigen = new InstitucionOrigen();

            contenidoDigital.Clave = TxtClave.Text.Trim();
            contenidoDigital.Nombre = TxtNombre.Text.Trim();
            contenidoDigital.InstitucionOrigen.Nombre = TxtIntitucion.Text.Trim();
            contenidoDigital.TipoDocumento = GetTipoDocumentoFromUI();
            contenidoDigital.Tags = TxtEtiquetas.Text.Trim();
            contenidoDigital.EsInterno = bool.Parse(RdBtnTipoReferencia.SelectedValue);
            contenidoDigital.EsDescargable = bool.Parse(RdBtnEsDescargable.SelectedValue);
            contenidoDigital.EstatusContenido = EEstatusContenido.ACTIVO;
            contenidoDigital.FechaRegistro = DateTime.Now;

            if (!contenidoDigital.EsInterno.Value)
            {
                URLContenido contenidoExterno = new URLContenido();
                contenidoExterno.EsPredeterminada = true;
                contenidoExterno.FechaRegistro = DateTime.Now;
                contenidoExterno.Nombre = "DEFAULT";
                contenidoExterno.Activo = true;
                contenidoExterno.URL = TxtReferenciaExterna.Text.Trim();

                contenidoDigital.URLContenidoAgregar(contenidoExterno);
            }


            return contenidoDigital;

        }

        private TipoDocumento GetTipoDocumentoFromUI()
        {
            TipoDocumento tipo = new TipoDocumento();
            int tipoID = 0;
            string valorID = DDLTipoDocumento.SelectedValue;
            if (int.TryParse(valorID, out tipoID))
            {
                if (tipoID > 0)
                {
                    tipo.TipoDocumentoID = tipoID;
                    tipo = tipoDocumentoCtrl.LastDataRowToTipoDocumento(tipoDocumentoCtrl.Retrieve(dctx, tipo));
                    
                }
            }

            return tipo;
        }

        private FileWrapper GetArchivoFromUI()
        {
            HttpPostedFile file = fupArchivoInterno.PostedFile;

            return new FileWrapper(file.InputStream, file.ContentLength, file.ContentType.ToString(), file.FileName);
        }
        #endregion

        #region *** metodos auxiliares ***
        
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCONTENIDOSDIGITALES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARCONTENIDOSDIGITALES) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!creacion)
                redirector.GoToHomePage(true);
        }
        #endregion
    }
}