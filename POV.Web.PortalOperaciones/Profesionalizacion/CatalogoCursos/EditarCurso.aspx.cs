using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using POV.Web.PortalOperaciones.Helper;
using POV.Profesionalizacion.Service;
using POV.Profesionalizacion.BO;
using POV.Core.Operaciones;
using Framework.Base.Exceptions;
using System.Data;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Comun.BO;
using System.Configuration;
using POV.Operaciones.Service;
using System.IO;

namespace POV.Web.PortalOperaciones.Profesionalizacion.CatalogoCursos
{
    public partial class EditarCurso : PageBase
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private TemaCursoCtrl temaCursoCtrl;
        private CursoCtrl cursoCtrl;
        private CatalogoCursoCtrl catalogoCursoCtrl;

        private List<string> FILES_EXTENSION = new List<string> { ".PDF" };
        private const int DEFMAXLENGHT = 4194304;
        private const string URL_BUSCAR = "BuscarCurso.aspx";

        public EditarCurso()
        {
            temaCursoCtrl = new TemaCursoCtrl();
            cursoCtrl = new CursoCtrl();
            catalogoCursoCtrl = new CatalogoCursoCtrl();
        }
        #region *** propiedades de clase ***
        public Curso LastObject
        {
            set { Session["lastCurso"] = value; }
            get { return Session["lastCurso"] != null ? Session["lastCurso"] as Curso : null; }
        }

        public string CursoID
        {
            get { return this.txtID.Text; }
            set { this.txtID.Text = value; }
        }
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
                    if (this.LastObject == null || this.LastObject.AgrupadorContenidoDigitalID == null)
                        throw new Exception("EditarCurso: Error de datos, el identificador del curso no debe ser nulo");

                    AAgrupadorContenidoDigital curso = this.cursoCtrl.LastDataRowToAAgrupadorContenidoDigital(this.cursoCtrl.Retrieve(dctx, this.LastObject));
                    if (!(curso is Curso))
                        throw new Exception("No se puede editar el elemento. Se esperaba un objeto Curso");

                    this.LastObject = curso as Curso;
                    this.DataToUserInterface(this.LastObject);

                    string archivos = string.Empty;

                    FILES_EXTENSION.ForEach(item => archivos += ", " + item + " ");

                    LblExtensiones.Text = String.Format("Máximo {0}. Archivos soportados: {1}", this.GetMaxLengthFileUploadInMB(), archivos.Substring(2));
                }
                catch (Exception ex)
                {
                    msgErr = ex.Message;
                    this.txtRedirect.Value = URL_BUSCAR;
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

                    //Obtener el archivo que se desea adjuntar al curso
                    FileWrapper documentoInformacion = GetDocumentoInformacionFromUI();

                    string rutaDoctos = @ConfigurationManager.AppSettings["POVURLInformacionCurso"];
                    string rutaDocAnterior = string.Empty;
                    string fileAnt = string.Empty;
                    string pathAnt = string.Empty;
                    if (!string.IsNullOrEmpty(this.LastObject.Informacion))
                    {
                        rutaDocAnterior = this.LastObject.Informacion;
                        if (!rutaDocAnterior.StartsWith("~")) rutaDocAnterior = "~" + rutaDocAnterior;
                        fileAnt = Path.GetFileName(rutaDocAnterior);
                        pathAnt = Path.GetDirectoryName(rutaDocAnterior);
                        if (pathAnt != string.Empty && !pathAnt.EndsWith("\\"))
                            pathAnt += "\\";
                    }

                    // Eliminar explícito
                    bool lEliminarArchivo = (this.chkTieneDocto.Checked == false && !string.IsNullOrEmpty(rutaDocAnterior));

                    if (!string.IsNullOrEmpty(documentoInformacion.Name))
                    {
                        string guidDocto = Guid.NewGuid().ToString("N");
                        string extension = documentoInformacion.Name.Substring(documentoInformacion.Name.LastIndexOf('.') + 1).ToLower();
                        documentoInformacion.Name = guidDocto + "." + extension;

                        ((Curso)curso).Informacion = rutaDoctos.Replace("~", "") + documentoInformacion.Name;

                        // Eliminar implícito (por cambio de archivo)
                        lEliminarArchivo = (lEliminarArchivo || !string.IsNullOrEmpty(rutaDocAnterior));

                        this.catalogoCursoCtrl.Update(dctx, curso, this.LastObject, lEliminarArchivo, Server.MapPath(rutaDoctos), documentoInformacion, Server.MapPath(pathAnt));
                    }
                    else
                    {
                        if (lEliminarArchivo)
                            ((Curso)curso).Informacion = null;

                        this.catalogoCursoCtrl.Update(dctx, curso, this.LastObject, lEliminarArchivo, null, null, Server.MapPath(pathAnt));
                    }
                }
                catch (Exception ex)
                {
                    sError += ex.Message;
                }
            }

            if (string.IsNullOrEmpty(sError))
            {
                LimpiarObjetosSesion();
                txtRedirect.Value = URL_BUSCAR;
                ShowMessage("Actualización exitosa.", MessageType.Information);
            }
            else
            {
                this.txtRedirect.Value = string.Empty;
                ShowMessage("Se encontraron los siguientes errores:\n" + sError, MessageType.Error);
            }
        }

        protected void BtnCancelar_OnClick(object sender, EventArgs e)
        {
            LimpiarObjetosSesion();
            Response.Redirect(URL_BUSCAR, true);
        }


        protected void BtnEliminarDoctoInformacion_Click(object sender, EventArgs e)
        {
            this.chkTieneDocto.Checked = false;
            this.BtnEliminarDoctoInformacion.Enabled = false;
        }
        #endregion

        #region *** validaciones ***
        //METODOS PARA VALIDACIONES
        private string ValidateData()
        {
            string sError = string.Empty;

            string nombre = txtNombre.Text.Trim();
            if (nombre.Length > 200 || nombre.Length < 1)
                sError += "El campoo NOMBRE no debe estar vacío y no debe ser mayor a 200 caracteres\n";

            HttpPostedFile file11 = this.FileInformacion.PostedFile;

            if (file11 != null && !string.IsNullOrEmpty(file11.FileName))
            {
                string extension = FILES_EXTENSION.FirstOrDefault(item => file11.FileName.ToUpper().EndsWith(item));

                if (string.IsNullOrEmpty(extension))
                    sError += "Extensión de Archivo de Información Inválido\n";

                int maxLenght = this.GetMaxLengthFileUpload();
                if (file11.ContentLength > maxLenght)
                    sError += string.Format("El archivo es más grande de lo permitido: {0}\n", this.GetMaxLengthFileUploadInMB());
            }

            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        //METODOS DE CONVERSION OBJETO A INFORMACION PARA LOS CONTROLES DE UI
        private void DataToUserInterface(Curso curso)
        {
            this.LoadTema();
            this.LoadPresencial();
            this.LoadEstadoProfesionalizacion();

            this.CursoID = curso.AgrupadorContenidoDigitalID.ToString();
            this.NombreCurso = curso.Nombre;
            this.Tema = ((int)curso.TemaCurso.TemaCursoID).ToString();
            this.Modalidad = ((Byte)curso.Presencial).ToString();
            this.EstadoCurso = ((Byte)curso.Estatus).ToString();

            this.txtID.ReadOnly = true;
            this.txtID.Enabled = true;

            this.chkTieneDocto.Enabled = false;
            if (string.IsNullOrEmpty(curso.Informacion))
            {
                this.lblInformacion.Text = "Agregar documento PDF de información";
                this.chkTieneDocto.Checked = false;
            }
            else
            {
                this.lblInformacion.Text = "Cambiar documento PDF de información";
                this.chkTieneDocto.Checked = true;
            }
            this.BtnEliminarDoctoInformacion.Enabled = this.chkTieneDocto.Checked;
        }

        private void LoadTema()
        {
            DataSet ds = this.temaCursoCtrl.Retrieve(dctx, new TemaCurso() { Activo = true });
            DDLTema.DataSource = ds;
            DDLTema.DataTextField = "Nombre";
            DDLTema.DataValueField = "TemaCursoID";
            DDLTema.DataBind();
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

        #region *** UserInterface to Data ***
        //METODOS DE CONVERSION DE UI A OBJETO CONCRETO

        private Curso UserIntefaceToData()
        {
            Curso cursoReturn = new Curso() { TemaCurso = new TemaCurso() };

            cursoReturn.AgrupadorContenidoDigitalID = long.Parse(this.CursoID.Trim());
            cursoReturn.Nombre = this.NombreCurso.Trim();
            cursoReturn.TemaCurso.TemaCursoID = int.Parse(this.Tema.Trim());
            cursoReturn.Presencial = (EPresencial)int.Parse(this.Modalidad);
            cursoReturn.Estatus = (EEstatusProfesionalizacion)int.Parse(this.EstadoCurso);
            cursoReturn.EsPredeterminado = this.LastObject.EsPredeterminado;
            cursoReturn.FechaRegistro = DateTime.Now;
            cursoReturn.Informacion = this.LastObject.Informacion;

            return cursoReturn;
        }

        private FileWrapper GetDocumentoInformacionFromUI()
        {
            HttpPostedFile file = this.FileInformacion.PostedFile;

            return new FileWrapper(file.InputStream, file.ContentLength, file.ContentType.ToString(), file.FileName);
        }
        #endregion

        #region *** metodos auxiliares ***
        //CUALQUIER METODO AUXILIAR
        private void LimpiarObjetosSesion()
        {
            LastObject = null;
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
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCURSOS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCURSOS) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!edit)
                redirector.GoToHomePage(true);
        }

        #endregion

    }
}