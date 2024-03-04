using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Web.PortalOperaciones.Helper;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalOperaciones.AppCode.Page;

namespace POV.Web.PortalOperaciones.Perfiles
{
    public partial class RegistrarPerfil : PageBase
    {
        private PerfilCtrl perfilCtrl;

        #region *** propiedades de clase ***

        #endregion

        public RegistrarPerfil()
        {
            perfilCtrl = new PerfilCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBack();
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DoInsert();
        }
        #endregion

        #region *** validaciones ***
        private string ValidateData()
        {
            string sError = string.Empty;

            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                if (txtNombre.Text.Trim().Length > 40)
                    sError += ",El nombre no debe ser mayor de 40 caracteres";
            }
            if (!string.IsNullOrEmpty(txtDescripcion.Text.Trim()))
            {
                if (txtDescripcion.Text.Trim().Length > 100)
                    sError += ",La descripción no debe ser mayor de 100 caracteres";
            }
            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Perfiles/BuscarPerfiles.aspx";
        }

        #endregion

        #region *** UserInterface to Data ***

        private Perfil UserInterfaceToData()
        {
            Perfil perfil = new Perfil();
            if (txtNombre.Text.Trim().Length > 0)
                perfil.Nombre = txtNombre.Text.Trim();
            if (txtDescripcion.Text.Trim().Length > 0)
                perfil.Descripcion = txtDescripcion.Text.Trim();
            perfil.Estatus = true;
            perfil.Operaciones = chkOperacion.Checked;
            return perfil;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoInsert()
        {
            string validateMessage = ValidateData();

            if (string.IsNullOrEmpty(validateMessage))
            {
                IDataContext dctx = ConnectionHlp.Default.Connection;

                Perfil perfil = UserInterfaceToData();

                DataSet dsUsuario = perfilCtrl.Retrieve(dctx, new Perfil { Nombre = perfil.Nombre });
                if (dsUsuario.Tables[0].Rows.Count > 0)
                    ShowMessage("El nombre del perfil ya está ocupado", MessageType.Error);
                else
                {
                    perfilCtrl.Insert(dctx, perfil);
                    txtRedirect.Value = "BuscarPerfiles.aspx";
                    ShowMessage("El perfil se ha registrado con éxito", MessageType.Information);
                }
            }
            else
            {
                txtRedirect.Value = string.Empty;
                ShowMessage(validateMessage, MessageType.Error);
            }

        }
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPERFILES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARPERFILES) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!creacion)
                redirector.GoToHomePage(true);
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
    }
}