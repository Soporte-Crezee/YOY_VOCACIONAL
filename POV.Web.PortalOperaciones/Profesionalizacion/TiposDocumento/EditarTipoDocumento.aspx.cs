using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.Service;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;
using POV.Seguridad.BO;
using Framework.Base.Exceptions;
using Framework.Base.DataAccess;

namespace POV.Web.PortalOperaciones.Profesionalizacion.TiposDocumento
{
    public partial class EditarTipoDocumento : PageBase
    {
        private TipoDocumentoCtrl tipoDocumentoCtrl;
        IDataContext dctx = ConnectionHlp.Default.Connection;

        #region  *** propiedades de la clase ***
        private TipoDocumento LastObject
        {
            set { Session["lastTipoDocumento"] = value; }
            get { return Session["lastTipoDocumento"] != null ? Session["lastTipoDocumento"] as TipoDocumento : null; }
        }
        #endregion

        public EditarTipoDocumento()
        {

            tipoDocumentoCtrl = new TipoDocumentoCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            txtID.Enabled = false;
            if (!IsPostBack)
            {
                if (LastObject != null)
                {
                    LastObject = tipoDocumentoCtrl.LastDataRowToTipoDocumento(tipoDocumentoCtrl.Retrieve(dctx, LastObject));
                    DataToUserInterface(LastObject);
                    GetTipoDocumento();
                }
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DoUpdate();
        }
        protected void RadBtnListTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadBtnListTipoDocumento.SelectedValue == "0")
            {
                txtFuente.Enabled = false;

            }
            else
            {
                txtFuente.Enabled = true;
            }
            if (RadBtnListTipoDocumento.SelectedValue == "1")
            {
                txtExtension.Enabled = false;
                txtMime.Enabled = false;
            }
            else
            {
                txtExtension.Enabled = true;
                txtMime.Enabled = true;
            }
        }
        #endregion

        #region *** validaciones ***
        private string ValidateData()
        {
            string sError = string.Empty;
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                if (txtNombre.Text.Trim().Length > 50)
                    sError += ", Nombre no puede ser mayor a 50 caracteres";
            }
            if (!string.IsNullOrEmpty(txtExtension.Text.Trim()))
            {
                if (txtExtension.Text.Trim().Length > 30)
                    sError += ", Extensión no puede ser mayor a 30 caracteres";
            }
            if (!string.IsNullOrEmpty(txtMime.Text.Trim()))
            {
                if (txtMime.Text.Trim().Length > 100)
                    sError += ", MIME no puede ser mayor a 100 caracteres";
            }
            if (!string.IsNullOrEmpty(txtFuente.Text.Trim()))
            {
                if (txtFuente.Text.Trim().Length > 100)
                    sError += ", Fuente no puede ser mayor a 100 caracteres";
            }
            if (RadBtnListTipoDocumento.SelectedValue == "")
                sError += ", Tipo de documento es requerido";
            if (RadBtnListTipoDocumento.SelectedValue == "1")
            {
                if (txtFuente.Text.Trim().Length <= 0)
                    sError += ", Fuente es requerido";
            }

            if (RadBtnListTipoDocumento.SelectedValue == "0")
            {
                if (txtExtension.Text.Trim().Length <= 0)
                {
                    sError += ", Extensión es requerido";
                }
                if (txtMime.Text.Trim().Length <= 0)
                {
                    sError += ", MIME es requerido";
                }
            }

            return sError;
        }

        #endregion

        #region *** Data to UserInterface ***
        private void DataToUserInterface(TipoDocumento tipoDocumento)
        {
            txtID.Text = tipoDocumento.TipoDocumentoID.ToString();
            txtNombre.Text = tipoDocumento.Nombre;
            txtExtension.Text = tipoDocumento.Extension;
            txtMime.Text = tipoDocumento.MIME;
            if (tipoDocumento.EsEditable == true)
                ddlEsEditable.SelectedValue = "true";
            else
                ddlEsEditable.SelectedValue = "false";
            txtFuente.Text = tipoDocumento.Fuente;

        }

        #endregion

        #region *** UserInterface to Data ***

        private TipoDocumento UserInterfaceToData()
        {
            TipoDocumento tipoDocumento = new TipoDocumento();
            int tipoDocumentoID;

            if (int.TryParse(txtID.Text.Trim(), out tipoDocumentoID))
            {
                tipoDocumento.TipoDocumentoID = tipoDocumentoID;
            }
            if (txtNombre.Text.Trim().Length > 0)
                tipoDocumento.Nombre = txtNombre.Text.Trim();
            if (txtExtension.Text.Trim().Length > 0)
                tipoDocumento.Extension = txtExtension.Text.Trim();
            if (txtMime.Text.Trim().Length > 0)
                tipoDocumento.MIME = txtMime.Text.Trim();
            if (ddlEsEditable.SelectedValue == "true")
                tipoDocumento.EsEditable = true;
            else
                tipoDocumento.EsEditable = false;

            if (txtFuente.Text.Trim().Length > 0)
                tipoDocumento.Fuente = txtFuente.Text.Trim();

            return tipoDocumento;
        }
        #endregion

        #region  *** metodos auxiliares ***
        private void DoUpdate()
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                try
                {
                    TipoDocumento tipoDocumento = UserInterfaceToData();
                    tipoDocumento.FechaRegistro = LastObject.FechaRegistro;
                    if (RadBtnListTipoDocumento.SelectedValue == "0")
                    {
                        tipoDocumento.Fuente = null;
                    }
                    if (RadBtnListTipoDocumento.SelectedValue == "1")
                    {
                        tipoDocumento.Extension = null;
                        tipoDocumento.MIME = null;
                    }
                    tipoDocumentoCtrl.Update(dctx, tipoDocumento, LastObject);
                    LastObject = tipoDocumento;
                    txtRedirect.Value = "BuscarTipoDocumento.aspx";
                    ShowMessage("El tipo de documento se ha actualizado con éxito", MessageType.Information);

                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message, MessageType.Information);
                }

            }
            else
            {
                ShowMessage("Errores en los campos: " + validateMessage.Substring(2), MessageType.Error);
            }
        }

        private void GetTipoDocumento()
        {
            if (txtFuente.Text.Trim().Length > 0)
            {
                RadBtnListTipoDocumento.SelectedValue = "1";
                txtExtension.Enabled = false;
                txtMime.Enabled = false;
            }
            if (txtExtension.Text.Trim().Length > 0 && txtMime.Text.Trim().Length > 0)
            {
                RadBtnListTipoDocumento.SelectedValue = "0";
                txtFuente.Enabled = false;
            }
        }

        #endregion

        #region *****Message Showing*****
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="typeNotification">1: Error, 2: Advertencia, 3: Información</param>
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOTIPODOCUMENTO) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARTIPODOCUMENTO) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARTIPODOCUMENTO) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!lectura)
                redirector.GoToHomePage(true);
            if (!edit)
                redirector.GoToHomePage(true);
        }
        #endregion

    }
}