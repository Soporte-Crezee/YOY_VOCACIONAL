using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Web.Administracion.Helper;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Core.Administracion.Implements;
using POV.Core.Administracion.Interfaces;
using POV.Seguridad.BO;
using Framework.Base.Exceptions;
using POV.Web.Administracion.AppCode.Page;

namespace POV.Web.Administracion
{
    public partial class Default : PageBase
    {
        public Default()
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (userSession.IsLogin())
                {
                    if (userSession.CurrentPerfil.PerfilID == (int)EPerfil.DIRECTOR)
                    {
                        if (userSession.CurrentEscuela != null && userSession.CurrentCicloEscolar != null)
                        {
                            List<Permiso> permisos = userSession.CurrentPrivilegiosDirector.GetPermisos();
                            string moduloTalentosKey = @System.Configuration.ConfigurationManager.AppSettings["MODULO_TALENTOS_KEY"];
                            bool tieneAccesoTalentos = userSession.ModulosFuncionales != null &&
                                                       userSession.ModulosFuncionales.FirstOrDefault(
                                                           m => m.ModuloFuncionalId == int.Parse(moduloTalentosKey)) != null;
                            bool accesoEspecialista =
                                permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOESPECIALISTA) != null;

                            DivMenuDirector.Visible = true;
                        }
                        else
                            redirector.GoToSeleccionarEscuela(true);
                        
                    }
                    else
                    {
                        DivMenuDirector.Visible = false;
                    }
                }
                else
                    new Redirector().GoToLoginPage(true);
            }
        }

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            if (userSession.CurrentPerfil.PerfilID == (int)EPerfil.DIRECTOR)
            {
                if (userSession.CurrentEscuela != null && userSession.CurrentCicloEscolar != null)
                {
                    List<Permiso> permisos = userSession.CurrentPrivilegiosDirector.GetPermisos();

                    bool accesoDocentes = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESODOCENTES) != null;
                    if (accesoDocentes)
                    {
                        opcion_Docentes.Visible = true;
                        opcion_Universidades.Visible = true;
                        menu_DocentesCatalogo.Visible = true;
                        menu_UniversidadCatalogo.Visible = true;
                        RightColumn.Visible = true;
                        LeftColumn.Visible = true;
                        
                    }

                    string moduloTalentosKey = @System.Configuration.ConfigurationManager.AppSettings["MODULO_TALENTOS_KEY"];
                    bool tieneAccesoTalentos = userSession.ModulosFuncionales != null && userSession.ModulosFuncionales.FirstOrDefault(m => m.ModuloFuncionalId == int.Parse(moduloTalentosKey)) != null;
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