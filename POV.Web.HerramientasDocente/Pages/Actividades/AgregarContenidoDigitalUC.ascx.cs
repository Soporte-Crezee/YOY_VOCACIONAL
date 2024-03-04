using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using System.IO;
using Framework.Base.Exceptions;
using POV.Content.MasterPages;
using POV.ConfiguracionActividades.BO;
using Framework.Base.DataAccess;
using POV.Web.Helper;

namespace POV.Web.HerramientasDocente.Pages.Actividades
{
    public partial class AgregarContenidoDigitalUC : System.Web.UI.UserControl
    {
        private TipoDocumentoCtrl tipoDocumentoCtrl;

        IDataContext dctx = ConnectionHlp.Default.Connection;

        public string PaginaContenedora { get; set; }

        private const string CMD_ADD_CONTENIDO = "CMD_ADD_CONTENIDO";
        //TODO POner la config en el web config
        //temporales a AMAZON S3 guardar y generar la transaccion con entity


        public AgregarContenidoDigitalUC()
        {
            tipoDocumentoCtrl = new TipoDocumentoCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAgregar_OnClick(object sender, EventArgs e)
        {
            try
            {
                string sError = ValidateContenidoDigital();

                if (string.IsNullOrEmpty(sError))
                {
                    TareaContenidoDigital tarea = GetTareaContenidoFromUI();
                    if (PaginaContenedora == "CrearActividadUI")
                        (this.Page as CrearActividadUI).AgregarTareasFromRegistro(CMD_ADD_CONTENIDO, tarea);
                    else if (PaginaContenedora == "EditarActividadUI")
                        (this.Page as EditarActividadUI).AgregarTareasFromRegistro(CMD_ADD_CONTENIDO, tarea);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "cerrarDialogo", "CerrarDialogo();", true);
                }
                else
                {
                    lblErrorAgregarContenido.Text = "Error: " + sError.Substring(2);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //ShowMessage("Error al enviar a realizar la asignación", Contenido.MasterPages.Site.EMessageType.Error);
            }
        }

        public void IniciarRegistro()
        {
            txtInstruccionesContenido.Text = "";
            lblErrorAgregarContenido.Text = "";
            TxtClave.Text = "";
            TxtNombre.Text = "";
            TxtReferenciaExterna.Text = "";
            RdBtnEsDescargable.SelectedIndex = 0;
            RdBtnTipoReferencia.SelectedIndex = 0;
        }

        private string ValidateContenidoDigital()
        {
            string sError = string.Empty;

            string sClave = TxtClave.Text.Trim();
            string sNombre = TxtNombre.Text.Trim();
            string sReferencia = RdBtnTipoReferencia.SelectedValue;
            string sDescargable = RdBtnEsDescargable.SelectedValue;
            string instruccion = txtInstruccionesContenido.Text.Trim();

            if (string.IsNullOrEmpty(instruccion))
                sError += " , El campo Instrucción es requerido ";
            else if (instruccion.Length > 200)
                sError += " , El campo Instrucción solo admite 200 caracteres ";

            if (string.IsNullOrEmpty(sClave))
                sError += ", Clave Requerida ";
            if (string.IsNullOrEmpty(sNombre))
                sError += ", Nombre es Requerido ";

            if (string.IsNullOrEmpty(sReferencia))
                sError += ", Referencia es Requerido ";
            if (string.IsNullOrEmpty(sDescargable))
                sError += ", Tipo de descarga es Requerido ";


            bool esDescargable = true;
            bool esInterno = true;

            if (!bool.TryParse(sReferencia, out esInterno))
                sError += ", Referencia es Requerido ";
            if (!bool.TryParse(sDescargable, out esDescargable))
                sError += ", Tipo de descarga es Requerido ";

            //validamos el tipo de documento si es interno
            if (esInterno)
            {
                HttpPostedFile file11 = fupArchivoInterno.PostedFile;
                if (string.IsNullOrEmpty(file11.FileName))
                {
                    sError += ", Referencia Interna Requerida ";
                }
                else
                {
                    if (file11 != null && !string.IsNullOrEmpty(file11.FileName))
                    {
                        //si el mime y la extension corresponden 

                    }
                    else
                        sError += ", El archivo seleccionado no tiene el formato o extensión correcta ";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(TxtReferenciaExterna.Text.Trim()))
                    sError += ", Referencia Externa Requerida ";
            }
            return sError;
        }

        private TareaContenidoDigital GetTareaContenidoFromUI()
        {
            TareaContenidoDigital tarea = new TareaContenidoDigital();
            ContenidoDigital contenido = GetContenidoDigitalFromUI();



            tarea.Instruccion = txtInstruccionesContenido.Text.Trim();
            tarea.Nombre = contenido.Nombre;
            tarea.ContenidoDigital = contenido;
            return tarea;
        }

        private ContenidoDigital GetContenidoDigitalFromUI()
        {
            ContenidoDigital contenidoDigital = new ContenidoDigital();
            contenidoDigital.InstitucionOrigen = new InstitucionOrigen();

            contenidoDigital.Clave = TxtClave.Text.Trim();
            contenidoDigital.Nombre = TxtNombre.Text.Trim();
            contenidoDigital.InstitucionOrigen.Nombre = "Docente";
            contenidoDigital.Tags = "Docente";
            contenidoDigital.InstitucionOrigen.Nombre = "Docente";
            contenidoDigital.EsInterno = bool.Parse(RdBtnTipoReferencia.SelectedValue);
            contenidoDigital.EsDescargable = bool.Parse(RdBtnEsDescargable.SelectedValue);
            contenidoDigital.EstatusContenido = EEstatusContenido.ACTIVO;
            contenidoDigital.FechaRegistro = DateTime.Now;

            List<TipoDocumento> tiposDocumento = GetTiposDocumento();

            if (!contenidoDigital.EsInterno.Value)
            {
                URLContenido contenidoExterno = new URLContenido();
                contenidoExterno.EsPredeterminada = true;
                contenidoExterno.FechaRegistro = DateTime.Now;
                contenidoExterno.Nombre = "DEFAULT";
                contenidoExterno.Activo = true;
                contenidoExterno.URL = TxtReferenciaExterna.Text.Trim();

                contenidoDigital.URLContenidoAgregar(contenidoExterno);

                if (contenidoExterno.URL.ToLower().StartsWith("http://www.youtube.com/watch") ||
                    contenidoExterno.URL.ToLower().StartsWith("https://www.youtube.com/watch") ||
                    contenidoExterno.URL.ToLower().StartsWith("www.youtube.com/watch"))
                {
                    contenidoDigital.TipoDocumentoId = tiposDocumento.FirstOrDefault(item => item.Fuente != null && item.Fuente.CompareTo("www.youtube.com") == 0).TipoDocumentoID;
                    contenidoDigital.EsDescargable = false;
                }
                else
                {
                    TipoDocumento tipo = tiposDocumento.FirstOrDefault(item => item.Extension != null && contenidoExterno.URL.ToLower().EndsWith(item.Extension));
                    if (tipo == null)
                    {
                        tipo = tiposDocumento.FirstOrDefault(item => item.Fuente != null && item.Fuente.CompareTo("www") == 0);
                        contenidoDigital.EsDescargable = false;
                    }

                    contenidoDigital.TipoDocumentoId = tipo.TipoDocumentoID;

                }
            }
            else
            {
                FileUpload fiUp = fupArchivoInterno;
                string fileName = fiUp.PostedFile.FileName;

                string guidArchivo = Guid.NewGuid().ToString("N");
                string extension = fiUp.PostedFile.FileName.Substring(fiUp.FileName.LastIndexOf('.') + 1).ToLower();
                guidArchivo += "." + extension;

                fiUp.SaveAs(Server.MapPath(@System.Configuration.ConfigurationManager.AppSettings["RUTA_CONTENIDOS_TEMP"]) + guidArchivo);

                URLContenido contenidoExterno = new URLContenido();
                contenidoExterno.EsPredeterminada = true;
                contenidoExterno.FechaRegistro = DateTime.Now;
                contenidoExterno.Nombre = fiUp.PostedFile.ContentType;
                contenidoExterno.Activo = true;
                contenidoExterno.URL = guidArchivo;

                contenidoDigital.URLContenidoAgregar(contenidoExterno);

                TipoDocumento tipo = tiposDocumento.FirstOrDefault(item => item.Extension != null && item.Extension.CompareTo("." + extension) == 0);
                if (tipo == null)
                {
                    tipo = tiposDocumento.FirstOrDefault(item => item.Extension != null && item.Extension.CompareTo("*.*") == 0);
                }

                contenidoDigital.TipoDocumentoId = tipo.TipoDocumentoID;

            }


            return contenidoDigital;

        }

        private List<TipoDocumento> GetTiposDocumento()
        {
            DataSet ds = tipoDocumentoCtrl.Retrieve(dctx, new TipoDocumento { Activo = true });

            List<TipoDocumento> tipos = new List<TipoDocumento>();
            //castear el tipo documento.
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tipos.Add(tipoDocumentoCtrl.DataRowToTipoDocumento(dr));
            }

            return tipos;
        }

        private FileWrapper GetArchivoFromUI()
        {
            HttpPostedFile file = fupArchivoInterno.PostedFile;

            return new FileWrapper(file.InputStream, file.ContentLength, file.ContentType.ToString(), file.FileName);
        }

        #region *****Message  Showing*****
        private void ShowMessage(string message, Site.EMessageType messageType)
        {
            Site site = (Site)Page.Master;
            site.ShowMessage(message, messageType);
        }
        #endregion

    }
}