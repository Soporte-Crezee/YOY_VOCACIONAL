using POV.Administracion.BO;
using POV.CentroEducativo.BO;
using POV.Modelo.Context;
using POV.Web.Administracion.ComprasPortal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Checkout
{
    public partial class CheckoutReview : System.Web.UI.Page
    {
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

        private string Session_cantidadCreditos
        {
            get { return (string)this.Session["Session_cantidadCreditos"]; }
            set { this.Session["Session_cantidadCreditos"] = value; }
        }

        private string Session_errorPaypal
        {
            get { return (string)this.Session["errorPaypal"]; }
            set { this.Session["errorPaypal"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["token"] == null)
                {
                    Response.Redirect("~/PaquetesPremium/AdquirirCredito.aspx", true);
                }
                else {
                    CheckoutReviewPaypal();
                }
            }
        }

        protected void LoadReviewProduct(string costo) 
        {
            divCompraCreditos.Visible = true;
            lblCantidad.Text = Session_cantidadCreditos;
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
                    Response.Redirect("CheckoutError.aspx?" + retMsg);
                }
            }
            catch (Exception ex)
            {
                Session_errorPaypal = "error";
                Response.Redirect("CheckoutError.aspx?" + "ErrorCode="+ex);
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