using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.Services;
using POV.Core.PadreTutor.Implements;
using POV.Core.PadreTutor.Interfaces;
using POV.Logger.Service;
using POV.Seguridad.Utils;
using POV.Web.Administracion.Helper;
using Framework.Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Web.Administracion.Logic;
using POV.Administracion.BO;
using POV.Administracion.Services;

namespace POV.Web.Administracion.PaquetesPremium
{
    public partial class AdquirirPaquete : System.Web.UI.Page
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private PaquetePremium paquetePremium;
        private PaquetePremiumCtrl paquetePremiumCtrl;

        #region Variables de session
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

        private PaquetePremium Session_PaquetePremium
        {
            get { return (PaquetePremium)this.Session["Session_PaquetePremium"]; }
            set { this.Session["Session_PaquetePremium"] = value; }
        }

        private string Session_errorPaypal
        {
            get { return (string)this.Session["errorPaypal"]; }
            set { this.Session["errorPaypal"] = value; }
        }
        #endregion

        public AdquirirPaquete() {
            paquetePremium = new PaquetePremium();
            paquetePremiumCtrl = new PaquetePremiumCtrl(null);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Session_errorPaypal = string.Empty;
                if (Session_Alumno == null && Session_Tutor == null)
                {
                    ((Paquete)this.Master).RedirigirLandingPage();
                }               
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                return;
            }
        }	

        protected void ComprarPaquete_Click(object sender, EventArgs e)
        {
            var paquetesPremium = paquetePremiumCtrl.Retrieve(paquetePremium, false).ToList();

            if (paquetesPremium.Count > 0)
            {
                paquetePremium = paquetesPremium.First();
                Session_PaquetePremium = paquetePremium;

                Session["PayPalPaymentRequestName"] = paquetePremium.Nombre.Trim();
                Session["payment_amt"] = paquetePremium.CostoPaquete.ToString().Trim();
                Session["currency_code"] = PaypalSettings.PayPalCURRENCYCODE;

                Response.Redirect("~/Checkout/CheckoutStart.aspx", true);
            }            
        }
    }
}