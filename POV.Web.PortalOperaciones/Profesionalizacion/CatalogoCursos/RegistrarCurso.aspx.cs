using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using POV.Web.PortalOperaciones.Helper;
using POV.Profesionalizacion.Service;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Profesionalizacion.BO;
using POV.Core.Operaciones;
using Framework.Base.Exceptions;
using System.Data;
using POV.Seguridad.BO;
using System.Configuration;
using POV.Comun.BO;
using POV.Operaciones.Service;

namespace POV.Web.PortalOperaciones.Profesionalizacion.CatalogoCursos
{
    public partial class RegistrarCurso : PageBase
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private TemaCursoCtrl temaCursoCtrl;
        private CursoCtrl cursoCtrl;
        private CatalogoCursoCtrl catalogoCursoCtrl;

        private List<string> FILES_EXTENSION = new List<string> { ".PDF" };
        private const int DEFMAXLENGHT = 4194304;

        public RegistrarCurso()
        {
            temaCursoCtrl = new TemaCursoCtrl();
            cursoCtrl = new CursoCtrl();
            catalogoCursoCtrl = new CatalogoCursoCtrl();
        }
        #region *** propiedades de clase ***
        public string NombreCurso
        {
            get { return this.txtNombre.Text; }
            set { this.txtNombre.Text = value; }
        }
        public string Modalidad
        {
            get { return this.DDLPresencial.SelectedValue; }
            set { this.DDLPresencial.SelectedValue = value; }
        }
        public string EstadoCurso
        {
            get { return this.DDLEstatusProfesionalizacion.SelectedValue; }
            set { this.DDLEstatusProfesionalizacion.SelectedValue = value; }
        }
        public string Tema
        {
            get { return this.DDLTema.SelectedValue; }
            set { this.DDLTema.SelectedValue = value; }
        }
        #endregion

        #region Eventos de Página
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string msgErr = string.Empty;
                try
                {
                    this.DataToUserInterface();

                    string archivos = string.Empty;

                    FILES_EXTENSION.ForEach(item => archivos += ", " + item + " ");

                    LblExtensiones.Text = String.Format("Máximo {0}. Archivos soportados: {1}",this.GetMaxLengthFileUploadInMB(),archivos.Substring(2));
                }
                catch (Exception ex)
                {
                    msgErr = ex.Message;
                    this.txtRedirect.Value = UrlHelper.GetDefaultURL();
                    this.ShowMessage(msgErr, MessageType.Error);
                }
            }
        }

        protected void BtnGuardar_OnClick(object sender, EventArgs e)
        {
            string sError = this.ValidateData();

            if (string.IsNullOrEmpty(sError))
            {
                try
                {
                    AAgrupadorContenidoDigital curso = this.UserIntefaceToData();

                    //Agregar el primer elemento (AgrupadorSimple) al curso que se insertará
                    curso.Agregar(new AgrupadorSimple() { Nombre = curso.Nombre, EsPredeterminado = true });

                    //Obtener el archivo que se desea adjuntar al curso
                    FileWrapper documentoInformacion = GetDocumentoInformacionFromUI();

                    if (!string.IsNullOrEmpty(documentoInformacion.Name))
                    {
                        string rutaDoctos = @ConfigurationManager.AppSettings["POVURLInformacionCurso"];
                        string guidDocto = Guid.NewGuid().ToString("N");
                        string extension = documentoInformacion.Name.Substring(documentoInformacion.Name.LastIndexOf('.') + 1).ToLower();
                        documentoInformacion.Name = guidDocto + "." + extension;

                        ((Curso)curso).Informacion = rutaDoctos.Replace("~", "") + documentoInformacion.Name;

                        this.catalogoCursoCtrl.InsertComplete(dctx, curso, null, Server.MapPath(rutaDoctos), documentoInformacion);
                    }
                    else
                        this.catalogoCursoCtrl.InsertComplete(dctx, curso, null);
                }
                catch (Exception ex)
                {
                    sError += ex.Message;
                }
            }

            if (string.IsNullOrEmpty(sError))
            {
                txtRedirect.Value = "BuscarCurso.aspx";
                ShowMessage("Registro exitoso.", MessageType.Information);
            }
            else
            {
                this.txtRedirect.Value = string.Empty;
                ShowMessage("Se encontraron los siguientes errores:\n" + sError, MessageType.Error);
            }
        }

        protected void BtnCancelar_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("BuscarCurso.aspx", true);
        }
        #endregion

        #region *** validaciones ***
        //METODOS PARA VALIDACIONES
        private string ValidateData()
        {
            string sError = string.Empty;

            string nombre = txtNombre.Text.Trim();
            if (nombre.Length > 200 || nombre.Length < 1)
                sError += "El campo NOMBRE no debe estar vacío y no debe ser mayor a 200 caracteres\n";

            int nInd = 0;
            if (int.TryParse(this.Tema.Trim(), out nInd))
                if (nInd < 0)
                    sError += "Debe seleccionar un Tema para el curso.\n";

            if (int.TryParse(this.Modalidad.Trim(), out nInd))
                if (nInd < 0)
                    sError += "Debe seleccionar una Modalidad para el curso.\n";

            HttpPostedFile file11 = this.FileInformacion.PostedFile;

            if (file11 != null && !string.IsNullOrEmpty(file11.FileName))
            {
                string extension = FILES_EXTENSION.FirstOrDefault(item => file11.FileName.ToUpper().EndsWith(item));

                if (string.IsNullOrEmpty(extension))
                    sError += "Extensión de Archivo de Información Inválido\n";

                int maxLenght = this.GetMaxLengthFileUpload();
                if (file11.ContentLength > maxLenght)
                    sError += string.Format("El archivo es más grande de lo permitido: {0}\n",this.GetMaxLengthFileUploadInMB());
            }

            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        //METODOS DE CONVERSION OBJETO A INFORMACION PARA LOS CONTROLES DE UI
        private void DataToUserInterface()
        {
            this.LoadTema();
            this.LoadPresencial();
            this.LoadEstadoProfesionalizacion();

            this.NombreCurso = string.Empty;
            this.Tema = "-1";
            this.Modalidad = "-1";
            this.EstadoCurso = ((byte)EEstatusProfesionalizacion.MANTENIMIENTO).ToString();

            this.DDLEstatusProfesionalizacion.Enabled = false;
        }

        private void LoadTema()
        {
            DataSet ds = this.temaCursoCtrl.Retrieve(dctx, new TemaCurso() { Activo = true });
            DDLTema.DataSource = ds;
            DDLTema.DataTextField = "Nombre";
            DDLTema.DataValueField = "TemaCursoID";
            DDLTema.DataBind();
            DDLTema.Items.Insert(0, new ListItem("SELECCIONE...", "-1"));
        }

        private void LoadPresencial()
        {
            var dictionary = new Dictionary<int, string>();
            foreach (byte value in Enum.GetValues(typeof(EPresencial)))
            {
                dictionary.Add(value, Enum.GetName(typeof(EPresencial), value));
            }
            DDLPresencial.DataSource = dictionary;
            DDLPresencial.DataTextField = "Value";
            DDLPresencial.DataValueField = "Key";
            DDLPresencial.DataBind();
            DDLPresencial.Items.Insert(0, new ListItem("SELECCIONE...", "-1"));
        }

        private void LoadEstadoProfesionalizacion()
        {
            var dictionary = new Dictionary<int, string>();
            foreach (byte value in Enum.GetValues(typeof(EEstatusProfesionalizacion)))
            {
                if (value != (Byte)EEstatusProfesionalizacion.INACTIVO)
                    dictionary.Add(value, Enum.GetName(typeof(EEstatusProfesionalizacion), value));
            }
            DDLEstatusProfesionalizacion.DataSource = dictionary;
            DDLEstatusProfesionalizacion.DataTextField = "Value";
            DDLEstatusProfesionalizacion.DataValueField = "Key";
            DDLEstatusProfesionalizacion.DataBind();
        }
        #endregion

        #region *** UserInterface to Data ***
        //METODOS DE CONVERSION DE UI A OBJETO CONCRETO
        private Curso UserIntefaceToData()
        {
            Curso cursoReturn = new Curso() { TemaCurso = new TemaCurso() };

            cursoReturn.Nombre = this.NombreCurso.Trim();

            int nInd;
            if (int.TryParse(this.Tema.Trim(), out nInd))
                if (nInd > -1)
                    cursoReturn.TemaCurso.TemaCursoID = nInd;
                else
                    cursoReturn.TemaCurso.TemaCursoID = null;

            if (int.TryParse(this.Modalidad.Trim(), out nInd))
                if (nInd > -1)
                    cursoReturn.Presencial = (EPresencial)nInd;
                else
                    cursoReturn.Presencial = null;

            cursoReturn.EsPredeterminado = true;

            return cursoReturn;
        }

        private FileWrapper GetDocumentoInformacionFromUI()
        {
            HttpPostedFile file = this.FileInformacion.PostedFile;

            return new FileWrapper(file.InputStream, file.ContentLength, file.ContentType.ToString(), file.FileName);
        }

        private int GetMaxLengthFileUpload()
        {
            int maxLenght = 0;
            try
            {
                if (!(int.TryParse(@ConfigurationManager.AppSettings["POVMaxLenghFileUpload"], out maxLenght)))
                    maxLenght = DEFMAXLENGHT;
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(ex.Source, ex);
            }
            return maxLenght;
        }

        private string GetMaxLengthFileUploadInMB()
        {
            String strLength = String.Format("{0}MB", Math.Truncate((double)(this.GetMaxLengthFileUpload() / (1024 * 1024))));
            return strLength;
        }
        #endregion

        #region *** metodos auxiliares ***
        //CUALQUIER METODO AUXILIAR
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
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCURSOS) != null;
            bool create = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARCURSOS) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!create)
                redirector.GoToHomePage(true);
        }

        #endregion
    }
}