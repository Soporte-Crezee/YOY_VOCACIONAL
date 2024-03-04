using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalUniversidad.Checkout
{
  public partial class CheckoutStart : System.Web.UI.Page
  {      
      #region Session_variables
      private string Session_errorPaypal
      {
          get { return (string)this.Session["errorPaypal"]; }
          set { this.Session["errorPaypal"] = value; }
      }

      private string session_compraActiva
      {
          get { return Session["session_compraActiva"] as string; }
          set { Session["session_compraActiva"] = value; }
      }
      #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
      session_compraActiva = "activo";
      
      NVPAPICaller payPalCaller = new NVPAPICaller();
      string retMsg = "";
      string token = "";

      if (Session["payment_amt"] != null)
      {
          string amt = Session["payment_amt"].ToString();

        bool ret = payPalCaller.ShortcutExpressCheckout(amt, ref token, ref retMsg);
        if (ret)
        {
          Session["token"] = token;
          Response.Redirect(retMsg);
        }
        else
        {
          Session_errorPaypal = "error";
          Response.Redirect("CheckoutError.aspx?" + retMsg);
        }
      }
      else
      {
          Session_errorPaypal = "error";
          Response.Redirect("CheckoutError.aspx?ErrorCode=Error%20en%20la%20compra");
      }
    }
  }
}