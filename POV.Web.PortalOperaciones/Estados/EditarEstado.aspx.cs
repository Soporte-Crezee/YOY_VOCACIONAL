using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.Estados
{
    public partial class EditarEstado : PageBase
    {
        private EstadoCtrl estadoCtrl;
        private PaisCtrl paisCtrl;

        #region *** propiedades de clase ***
        private Estado LastObject
        {
            set { Session["lastEstado"] = value; }
            get { return Session["lastEstado"] as Estado; }
        }
        #endregion

        public EditarEstado()
        {
            estadoCtrl = new EstadoCtrl();
            paisCtrl = new PaisCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillPaises();
                FillBack();
                if (LastObject != null)
                {
                    LastObject =
                        estadoCtrl.LastDataRowToEstado(estadoCtrl.Retrieve(ConnectionHlp.Default.Connection, LastObject));
                    DataToUSerInterface(LastObject);

                }
                else
                {
                    txtRedirect.Value = "BuscarEstado.aspx";
                    ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DoUpdate();
        }
        #endregion

        #region *** validaciones ***
        private string ValidateData()
        {
            string sError = string.Empty;

            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                if (txtNombre.Text.Trim().Length > 30)
                    sError = ", El tamaño del texto no debe ser mayor a 30 caracteres)";
            }
            else
            {
                sError = "Debe especificar un nombre para el estado";
            }
            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void DataToUSerInterface(Estado estado)
        {
            ddlPais.SelectedValue = estado.Pais.PaisID.ToString();
            txtNombre.Text = estado.Nombre;
            txtCodigo.Text = estado.Codigo;
        }
        private void FillPaises()
        {
            ddlPais.DataSource = paisCtrl.Retrieve(ConnectionHlp.Default.Connection, new Pais());
            ddlPais.DataValueField = "PaisID";
            ddlPais.DataTextField = "Nombre";
            ddlPais.DataBind();
            if (ddlPais.Items.Count <= 0)
            {
                txtNombre.Enabled = false;
                btnGuardar.Enabled = false;
                ShowMessage("No es posible editar el estado, no se cuenta con paises registrados", MessageType.Error);
            }

        }
        #endregion

        #region *** UserInterface to Data ***
        private Estado UserInterfaceToData()
        {
            Estado estado = new Estado();
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
                estado.Nombre = txtNombre.Text.Trim();
            estado.Codigo = txtCodigo.Text.Trim();
            int paisId = 0;
            bool result = int.TryParse(ddlPais.SelectedValue, out paisId);
            if (paisId > 0)
            {
                estado.Pais = new Pais();
                estado.Pais.PaisID = paisId;
            }
            estado.EstadoID = LastObject.EstadoID;
            estado.FechaRegistro = DateTime.Now;
            return estado;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Paises/BuscarPaises.aspx";
        }
        private void DoUpdate()
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage.Trim()))
            {
                try
                {
                    Estado estado = UserInterfaceToData();
                    estado.FechaRegistro = LastObject.FechaRegistro;
                    estadoCtrl.Update(ConnectionHlp.Default.Connection, estado, LastObject);
                    LastObject = estado;
                    txtRedirect.Value = "BuscarEstado.aspx";
                    ShowMessage("El estado se ha actualizado con éxito", MessageType.Information);
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message, MessageType.Error);
                }

            }
            else
            {
                ShowMessage(validateMessage.Substring(2), MessageType.Warning);
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

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOESTADOS) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARESTADOS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARESTADOS) != null;

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