using POV.Administracion.BO;
using POV.Administracion.Services;
using POV.CentroEducativo.BO;
using POV.CentroEducativo;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.Services;
using POV.Core.Universidades.Implements;
using POV.Core.Universidades.Interfaces;

namespace POV.Web.PortalUniversidad.Checkout
{
    public partial class CheckoutComplete : System.Web.UI.Page
    {
        private CompraProducto compraProducto;
        private CompraProductoCtrl compraProductoCtrl;
        private EFAlumnoCtrl eFAlumnoCtrl;
        private Alumno alumno;
        private IUserSession userSession;
        private IRedirector redirector;
        private UniversidadCtrl universidadCtrl;

        #region Variables de session
        private List<InfoAlumnoUsuario> Session_listaSeleccionados
        {
            get { return Session["Session_listaSeleccionados"] as List<InfoAlumnoUsuario>; }
            set { Session["Session_listaSeleccionados"] = value; }
        }
        private string Session_cantidadExpedientes
        {
            get { return Session["Session_cantidadExpedientes"] as string; }
            set { Session["Session_cantidadExpedientes"] = value; }
        }
        private string Session_errorPaypal
        {
            get { return (string)this.Session["errorPaypal"]; }
            set { this.Session["errorPaypal"] = value; }
        }
        #endregion

        public CheckoutComplete()
        {
            compraProducto = new CompraProducto();
            compraProductoCtrl = new CompraProductoCtrl(null);
            alumno = new Alumno();
            userSession = new UserSession();
            redirector = new Redirector();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin())
                {
                    // Verificar que el usuario ha completado el proceso de pago.
                    if ((string)Session["userCheckoutCompleted"] != "true")
                    {
                        if (Session["token"] == null) 
                            redirector.GoToHomePage(true);                        
                        else
                        {
                            Session["userCheckoutCompleted"] = string.Empty;
                            Session_errorPaypal = "Error";
                            Response.Redirect("CheckoutError.aspx?Desc=Compra%20invalida.");
                        }
                    }
                    else
                        VerifyPayment();
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        #region Verificar Compra
        public void VerifyPayment()
        {
            NVPAPICaller payPalCaller = new NVPAPICaller();
            string retMsg = "";
            string token = "";
            string PayerID = "";
            NVPCodec decoder = new NVPCodec();

            token = Session["token"].ToString();
            string currency_code = Session["currency_code"].ToString();

            try
            {
                bool ret = payPalCaller.GetCheckoutDetails(token, ref PayerID, ref decoder, ref retMsg);
                if (ret)
                {
                    // Verificar la cantidad total del pago según lo establecido en CheckoutStart.aspx.
                    try
                    {
                        string payerID = PayerID;
                        decimal paymentAmountOnCheckout = Convert.ToDecimal(Session["payment_amt"].ToString());
                        decimal paymentAmoutFromPayPal = Convert.ToDecimal(decoder["AMT"].ToString());

                        //Almecenar costo de compra y fecha
                        compraProducto.CostoCompra = Convert.ToDouble(decoder["AMT"].ToString());
                        compraProducto.FechaCompra = Convert.ToDateTime(decoder["TIMESTAMP"].ToString());

                        if (paymentAmountOnCheckout != paymentAmoutFromPayPal)
                        {
                            Response.Redirect("CheckoutError.aspx?" + "Desc=Monto%20total%20de%20desajuste.");
                        }
                    }
                    catch (Exception)
                    {
                        Response.Redirect("CheckoutError.aspx?" + "Desc=Monto%20total%20de%20desajuste.");
                    }

                }

                string finalPaymentAmount = Session["payment_amt"].ToString();

                bool res = payPalCaller.DoCheckoutPayment(finalPaymentAmount, token, PayerID, ref decoder, ref retMsg);
                if (res)
                {
                    // Retrieve PayPal confirmation value.
                    string PaymentConfirmation = decoder["PAYMENTINFO_0_TRANSACTIONID"].ToString();
                    TransactionId.Text = PaymentConfirmation;

                    //completar datos de compra
                    compraProducto.UniversidadID = userSession.CurrentUniversidad.UniversidadID;
                    compraProducto.CodigoCompra = PaymentConfirmation;
                                       
                    #region *********************** fin compra **********************

                    object objeto = new object();
                    Contexto context = new Contexto(objeto);

                    eFAlumnoCtrl = new EFAlumnoCtrl(context);
                    universidadCtrl = new UniversidadCtrl(context);

                    //Guardar la compra en la DB
                    compraProductoCtrl.Insert(compraProducto);

                    int expComprados = int.Parse(Session_cantidadExpedientes);
                                        
                    Universidad universidad = new Universidad();
                    universidad = universidadCtrl.Retrieve(new Universidad { UniversidadID = userSession.CurrentUniversidad.UniversidadID }, true).FirstOrDefault();

                    List<Alumno> alumnos = new List<Alumno>();
                    alumnos = eFAlumnoCtrl.Retrieve(new Alumno(), false);
                    alumnos = (from a in alumnos
                               where
                               ((from s_ls in Session_listaSeleccionados select s_ls.AlumnoID)
                                           .Contains(a.AlumnoID)
                               ) 
                               select a).Distinct().OrderByDescending(x => x.AlumnoID).ToList();
                    alumnos = eFAlumnoCtrl.GetAlumnosMenosCarga(alumnos, expComprados);
                                      
                    universidadCtrl.Update(universidad, alumnos);
                    context.Commit(objeto);
                    context.Dispose();

                    //Recargamos la universidad en la sesión
                    universidadCtrl = new UniversidadCtrl(null);
                    userSession.CurrentUniversidad = universidadCtrl.RetrieveWithRelationship(new Universidad { UniversidadID = userSession.CurrentUniversidad.UniversidadID }, true).FirstOrDefault(); ;

                    Session["userCheckoutCompleted"] = string.Empty;
                    Session["token"] = null;

                    #endregion                   
                }
                else
                {
                    Response.Redirect("CheckoutError.aspx?" + retMsg);
                }
            }
            catch (Exception ex)
            {
                Session_errorPaypal = "error";                
                Response.Redirect("CheckoutError.aspx"+"?ErrorCode=" + ex.Message, true);
            }
        }
        #endregion

        protected void Continue_Click(object sender, EventArgs e)
        {
            redirector.GoToHomePage(true);
        }
    }
}