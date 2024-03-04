using POV.Administracion.BO;
using POV.CentroEducativo.BO;
using POV.Web.Administracion.ComprasPortal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Checkout
{
    public partial class CheckoutError : System.Web.UI.Page
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
        #region QueryString
        private string QS_ErrorCode
        {
            get { return this.Request.QueryString["ErrorCode"]; }
        }

        private string QS_Desc
        {
            get { return this.Request.QueryString["Desc"]; }
        }

        private string QS_Desc2
        {
            get { return this.Request.QueryString["Desc2"]; }
        }

        private string QS_ErrorEx
        {
            get { return this.Request.QueryString["ErrorEx"]; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {            
            if ((string.IsNullOrEmpty(QS_ErrorCode) && string.IsNullOrEmpty(QS_Desc) && string.IsNullOrEmpty(QS_Desc2) && string.IsNullOrEmpty(QS_ErrorEx)))
            {
                Response.Redirect("~/ComprasPortal/AdquirirCredito.aspx", true);
            }
            if (string.IsNullOrEmpty(Session_errorPaypal))
            {
                Response.Redirect("~/ComprasPortal/AdquirirCredito.aspx", true);
            }            
        }
        protected void Continue_Click(object sender, EventArgs e)
        {
            Session_errorPaypal = string.Empty;
            ((Compra)Master).GoToHomePage();  
        }
    }
}