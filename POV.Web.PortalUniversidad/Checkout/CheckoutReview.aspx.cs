using POV.Administracion.BO;
using POV.Administracion.Services;
using POV.CentroEducativo.BO;
using POV.Core.Universidades.Implements;
using POV.Core.Universidades.Interfaces;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalUniversidad.Checkout
{
    public partial class CheckoutReview : System.Web.UI.Page
    {
        private CostoProductoCtrl costoProductoCtrl;
        protected IUserSession userSession;
        protected IRedirector redirector;

        #region Variables de session
        private string Session_errorPaypal
        {
            get { return (string)this.Session["errorPaypal"]; }
            set { this.Session["errorPaypal"] = value; }
        }
        private string Session_cantidadExpedientes
        {
            get { return Session["Session_cantidadExpedientes"] as string; }
            set { Session["Session_cantidadExpedientes"] = value; }
        }

        private string Session_costoPorExpediente
        {
            get { return Session["Session_costoPorExpediente"] as string; }
            set { Session["Session_costoPorExpediente"] = value; }
        }
        #endregion

        public CheckoutReview()
        {
            costoProductoCtrl = new CostoProductoCtrl(null);
            userSession = new UserSession();
            redirector = new Redirector();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin())
                {
                    if (Session["token"] == null)
                    {
                        redirector.GoToHomePage(true);
                    }
                    else
                    {
                        CheckoutReviewPaypal();
                    }
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        protected void LoadReviewProduct(string costo) {
            divCompraExpedientes.Visible = true;
            lblPrecio.Text = Session_costoPorExpediente;
            lblCantidad.Text = Session_cantidadExpedientes;
            lblTotal.Text = costo;
        }

        protected void CheckoutReviewPaypal()
        {
            NVPAPICaller payPalCaller = new NVPAPICaller();

            string retMsg = "";
            string token = "";
            string PayerID = "";
            NVPCodec decoder = new NVPCodec();
            token = Session["token"].ToString();
            try
            {
                bool ret = payPalCaller.GetCheckoutDetails(token, ref PayerID, ref decoder, ref retMsg);
                if (ret)
                {
                    Session["payerId"] = PayerID;

                    // Verificar la cantidad total del pago según lo establecido en CheckoutStart.aspx.
                    try
                    {
                        decimal paymentAmountOnCheckout = Convert.ToDecimal(Session["payment_amt"].ToString());
                        decimal paymentAmoutFromPayPal = Convert.ToDecimal(decoder["AMT"].ToString());
                        if (paymentAmountOnCheckout != paymentAmoutFromPayPal)
                        {
                            Session_errorPaypal = "error";
                            Response.Redirect("CheckoutError.aspx?" + "Desc=Error%20en%20monto%20total.");
                        }
                    }
                    catch (Exception)
                    {
                        Session_errorPaypal = "error";
                        Response.Redirect("CheckoutError.aspx?" + "Desc=Error%20en%20monto%20total.");
                    }

                    // Display OrderDetails.
                    LoadReviewProduct(decoder["AMT"].ToString());
                  
                }
                else
                {
                    Session_errorPaypal = "error";
                    Response.Redirect("CheckoutError.aspx?ErrorCode=" + retMsg);
                }
            }
            catch (Exception ex)
            {
                Session_errorPaypal = "error";
                Response.Redirect("CheckoutError.aspx?ErrorCode="+ex);
            }            
        }

        protected void CheckoutConfirm_Click(object sender, EventArgs e)
        {
            Session["userCheckoutCompleted"] = "true";
            Response.Redirect("~/Checkout/CheckoutComplete.aspx");
        }

        protected void CheckoutCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Checkout/CheckoutCancel.aspx");
        }
    }
}