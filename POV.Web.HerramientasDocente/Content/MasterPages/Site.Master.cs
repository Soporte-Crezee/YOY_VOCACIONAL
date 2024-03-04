//Satisface Adecuacion Login 
using POV.AppCode.Page;
using POV.Seguridad.BO;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace POV.Content.MasterPages
{

    public partial class Site : MasterPageBase
    {

        public Site()
            : base()
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {


            this.Page.Header.DataBind();
            if (!IsPostBack)
            {
                ShowMenuExpertoPruebas();
                string usuario = userSession.CurrentDocente.Nombre + " " +
                    userSession.CurrentDocente.PrimerApellido +
                    (string.IsNullOrEmpty(userSession.CurrentDocente.SegundoApellido) ? "" : " " + userSession.CurrentDocente.SegundoApellido);
                this.LblNombreUsuario.Text = usuario.Trim();
                LblMiEscuela.Visible = false;
                this.LblNombreEscuela.Text = userSession.CurrentEscuela.NombreEscuela + ", Turno: " + userSession.CurrentEscuela.Turno.ToString();


            }//end postback

        }



        private void ShowMenuExpertoPruebas()
        {
            MultiViewTopMenu.SetActiveView(ViewToMenuExpertoPruebas);
        }

        protected override void AuthorizeUser()
        {


        }

        #region *****Message  Showing*****
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="messageType">Tipo de mensaje</param>
        public void ShowMessage(string message, EMessageType messageType)
        {
            string type = string.Empty;

            switch (messageType)
            {
                case EMessageType.Error:
                    type = "1";
                    break;
                case EMessageType.Information:
                    type = "3";
                    break;
                case EMessageType.Warning:
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
            Control m = this.hdnLastMessage;
            Control t = this.hdnShowMessage;

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

        public enum EMessageType
        {
            Error = 1,
            Warning = 2,
            Information = 3
        }
    }
}