using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Logger.Service;
using POV.Web.Administracion.Helper;
using POV.Web.Administracion.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.Administracion.ComprasPortal
{
    public partial class AdquirirCredito : System.Web.UI.Page
    {
        #region Prpiedades de la clase
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        #region Variables de sesion
        private Alumno Session_Alumno
        {
            get { return (Alumno)this.Session["ALUMNO_KEY"]; }
            set { this.Session["ALUMNO_KEY"] = value; }
        }

        private Tutor Session_Tutor
        {
            get { return (Tutor)this.Session["TUTOR_KEY"]; }
            set { this.Session["TUTOR_KEY"] = value; }
        }

        private string Session_errorPaypal
        {
            get { return (string)this.Session["errorPaypal"]; }
            set { this.Session["errorPaypal"] = value; }
        }

        private string Session_cantidadCreditos
        {
            get { return (string)this.Session["Session_cantidadCreditos"]; }
            set { this.Session["Session_cantidadCreditos"] = value; }
        }
        #endregion
        #endregion

        public AdquirirCredito()
        {

        }

        #region Eventos de la página
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session_errorPaypal = string.Empty;
                if (Session_Alumno == null && Session_Tutor == null) 
                {
                    ((Compra)this.Master).RedirigirLandingPage();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void CompraCredito_Click(object sender, ImageClickEventArgs e)
        {
            if (txtCantidadCreditos.Text != "")
            {
                int cantidad = 0;
                if (int.TryParse(txtCantidadCreditos.Text, out cantidad))
                {
                    int cantidadCredito = int.Parse(txtCantidadCreditos.Text);
                    if (cantidadCredito < 1)
                    {
                        string error = "La cantidad no puede ser menor que 1.";
                        this.ShowMessage(error, MessageType.Error);
                    }
                    else
                    {
                        string total = double.Parse(txtCantidadCreditos.Text).ToString().Trim();
                        Session["PayPalPaymentRequestName"] = "Compra de crédito";
                        Session["payment_amt"] = total;
                        Session["currency_code"] = PaypalSettings.PayPalCURRENCYCODE;

                        Session_cantidadCreditos = txtCantidadCreditos.Text.Trim();
                        Response.Redirect("~/Checkout/CheckoutStart.aspx", true);
                    }
                }
                else
                {
                    this.ShowMessage("La cantidad tiene un formato incorrecto.", MessageType.Error);
                }
            }
            else 
            {
                this.ShowMessage("Introducir la cantidad de credito a comprar.", MessageType.Error);
            }
        }
        #endregion

        #region Message Showing
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
            Control m = Page.Master.FindControl("hdnLasteMessage");
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