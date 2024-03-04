﻿using System;
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

namespace POV.Web.PortalOperaciones.Paises
{
    public partial class EditarPais : PageBase
    {
        private PaisCtrl paisCtrl;

        #region *** propiedades de clase ***
        private Pais LastObject
        {
            set { Session["lastPais"] = value; }
            get { return Session["lastPais"] as Pais; }
        }
        #endregion

        public EditarPais()
        {
            paisCtrl = new PaisCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (LastObject != null)
                {
                    LastObject = paisCtrl.LastDataRowToPais(paisCtrl.Retrieve(ConnectionHlp.Default.Connection, LastObject));
                    DataToUserInterface(LastObject);
                    FillBack();
                }
                else
                {
                    txtRedirect.Value = "BuscarPaises.aspx";
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
            if (!string.IsNullOrEmpty(txtID.Text.Trim()))
            {
                int paisId = 0;
                bool result = int.TryParse(txtID.Text.Trim(), out paisId);
                if (!result)
                    sError += ", El identificador debe ser un número entero";
            }
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                if (txtNombre.Text.Trim().Length > 30)
                    sError = ", El tamaño del texto no debe ser mayor a 30 caracteres)";
            }
            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void DataToUserInterface(Pais pais)
        {
            txtID.Text = pais.PaisID.ToString();
            txtNombre.Text = pais.Nombre;
            txtCodigo.Text = pais.Codigo;
        }
        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Paises/BuscarPaises.aspx";
        }
        #endregion

        #region *** UserInterface to Data ***
        private Pais UserInterfaceToData()
        {
            Pais pais = new Pais();
            int id = 0;
            if (int.TryParse(txtID.Text.Trim(), out id))
            {
                pais.PaisID = id;
            }
            if (txtNombre.Text.Trim().Length > 0)
                pais.Nombre = txtNombre.Text.Trim();
            pais.Codigo = txtCodigo.Text.Trim();
            return pais;

        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoUpdate()
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                try
                {
                    Pais pais = UserInterfaceToData();
                    pais.FechaRegistro = LastObject.FechaRegistro;
                    paisCtrl.Update(ConnectionHlp.Default.Connection, pais, LastObject);
                    LastObject = pais;
                    txtRedirect.Value = "BuscarPaises.aspx";
                    ShowMessage("El país se ha actualizado con éxito", MessageType.Information);
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message, MessageType.Information);
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPAIS) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARPAIS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARPAIS) != null;

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