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
    public partial class CheckoutCancel : System.Web.UI.Page
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session_Alumno == null && Session_Tutor == null)
            {
                ((Compra)this.Master).RedirigirLandingPage();
            }
        }
        protected void Continue_Click(object sender, EventArgs e)
        {
            ((Compra)Master).GoToHomePage();
        }
    }
}