﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.Universidades.Implements;
using POV.Seguridad.BO;
using Framework.Base.Exceptions;
using POV.Web.PortalUniversidad.AppCode.Page;
using POV.Logger.Service;

namespace POV.Web.PortalUniversidad
{
    public partial class Default : PageBase
    {
        public Default()
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    if (userSession.IsLogin())
                    {
                        if (userSession.CurrentPerfil.PerfilID == (int)EPerfil.UNIVERSIDAD)
                        {
                            if (userSession.CurrentEscuela != null && userSession.CurrentCicloEscolar != null)
                            {
                                List<Permiso> permisos = userSession.CurrentPrivilegiosUniversidad.GetPermisos();
                            }
                            else
                                redirector.GoToSeleccionarEscuela(true);
                        }
                    }
                    else
                        new Redirector().GoToLoginPage(true);
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
            }
        }

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            if (userSession.CurrentPerfil.PerfilID == (int)EPerfil.UNIVERSIDAD)
            {
                if (userSession.CurrentEscuela != null && userSession.CurrentCicloEscolar != null)
                {
                    List<Permiso> permisos = userSession.CurrentPrivilegiosUniversidad.GetPermisos();

                    bool accesoDocentes = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOORIENTADOR) != null;
                    if (accesoDocentes)
                    {
                        opcion_Orientadores.Visible = true;
                        opcion_Eventos.Visible = true;
                        opcion_Expedientes.Visible = true;
                        menu_OrientadoresCatalogo.Visible = true;
                        menu_EventosCatalogo.Visible = true;
                        menu_ExpedientesCatalogo.Visible = true;
                        LeftColumn.Visible = true;
                        CenterColumn.Visible = true;
                        RightColumn.Visible = true;
                    }
                }
                else
                    redirector.GoToSeleccionarEscuela(true);
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
    }
}