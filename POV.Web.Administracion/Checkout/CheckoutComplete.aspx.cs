using POV.Administracion.BO;
using POV.Administracion.Services;
using POV.CentroEducativo.BO;
using POV.CentroEducativo;
using POV.Modelo.Context;
using POV.Web.Administracion.ComprasPortal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.Services;
using POV.CentroEducativo.Service;
using POV.Web.Administracion.Helper;
using POV.Seguridad.BO;
using POV.Licencias.Service;
using POV.Expediente.BO;
using POV.Expediente.Services;

namespace POV.Checkout
{
    public partial class CheckoutComplete : System.Web.UI.Page
    {
        private CompraCredito compraCredito;
        private CompraCreditoCtrl compraCreditoCtrl;
        private EFAlumnoCtrl eFAlumnoCtrl;
        private DocenteCtrl docenteCtrl;
        private TutorCtrl tutorCtrl;
        private Alumno alumno;
        private Tutor tutor;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private UsuarioExpedienteCtrl usuarioExpedienteCtrl;


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

        public CheckoutComplete()
        {
            compraCredito = new CompraCredito();
            compraCreditoCtrl = new CompraCreditoCtrl(null);
            eFAlumnoCtrl = new EFAlumnoCtrl(null);
            docenteCtrl = new DocenteCtrl();
            tutorCtrl = new TutorCtrl(null);
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            alumno = new Alumno();
            usuarioExpedienteCtrl = new UsuarioExpedienteCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar que el usuario ha completado el proceso de pago.
                if ((string)Session["userCheckoutCompleted"] != "true" || Session["token"] == null)
                {
                    Session["userCheckoutCompleted"] = string.Empty;
                    Session_errorPaypal = "Error";
                    Response.Redirect("CheckoutError.aspx?" + "Desc=Compra%20invalida.");
                }
                else
                    VerifyPayment();
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
                        compraCredito.CostoCompra = Convert.ToDouble(decoder["AMT"].ToString());
                        compraCredito.FechaCompra = Convert.ToDateTime(decoder["TIMESTAMP"].ToString());

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

                #region Confirmar existencia de acceso DB
                List<Tutor> tutorPaga = new List<Tutor>();
                List<Alumno> alumnoPaga = new List<Alumno>();

                if (Session_Tutor != null && Session_cantidadCreditos != null)
                {
                    tutorPaga = tutorCtrl.Retrieve(new Tutor { TutorID = Session_Tutor.TutorID }, false);
                }
                else if (Session_Alumno != null && Session_cantidadCreditos != null)
                {
                    alumnoPaga = eFAlumnoCtrl.Retrieve(new Alumno { AlumnoID = Session_Alumno.AlumnoID }, false);
                }
                #endregion

                if ((tutorPaga != null && tutorPaga.Count > 0) || (alumnoPaga != null && alumnoPaga.Count > 0))
                {
                    bool res = payPalCaller.DoCheckoutPayment(finalPaymentAmount, token, PayerID, ref decoder, ref retMsg);
                    if (res)
                    {
                        // Retrieve PayPal confirmation value.
                        string PaymentConfirmation = decoder["PAYMENTINFO_0_TRANSACTIONID"].ToString();
                        TransactionId.Text = PaymentConfirmation;

                        if (Session_Tutor != null && Session_cantidadCreditos != null)
                        {
                            divCongratulationAlumno.Visible = false;
                            divCongratulationTutor.Visible = true;

                            //Asignacion de credito al Tutor
                            compraCredito.TutorID = Session_Tutor.TutorID;
                        }
                        else if (Session_Alumno != null && Session_cantidadCreditos != null)
                        {
                            divCongratulationAlumno.Visible = true;
                            divCongratulationTutor.Visible = false;

                            //Asignacion de credito al alumno
                            compraCredito.AlumnoID = Session_Alumno.AlumnoID;
                        }

                        compraCredito.CodigoCompra = PaymentConfirmation;

                        //Guardar la compra de paquete en la DB
                        compraCreditoCtrl.Insert(compraCredito);

                        if (Session_Alumno != null)
                        {
                            alumno = eFAlumnoCtrl.Retrieve(new Alumno { AlumnoID = Session_Alumno.AlumnoID }, true).ToList().First();

                            // si el alumno es premiun solo actualizara el valor de su credito
                            // de lo contrario asignara un orientador ademas de ingresar el credito que ha adquirido
                            if (alumno.Premium == false || alumno.Premium == null)
                            {
                                var docente = docenteCtrl.GetDocenteMenosCarga(ConnectionHlp.Default.Connection, new Docente() { Estatus = true });

                                //Actualiza el status premium del alumno
                                alumno.Premium = true;
                                double? credito = string.IsNullOrEmpty(alumno.Credito.ToString()) ? (double?)null : Convert.ToDouble(alumno.Credito);
                                double? creditoComprado = Convert.ToDouble(Session_cantidadCreditos);
                                alumno.Credito = credito + creditoComprado;
                                eFAlumnoCtrl.Update(alumno);
                                Session_Alumno = alumno;

                                //Vicula Orientador-Alumno 
                                var usuarioDocente = licenciaEscuelaCtrl.RetrieveUsuarios(ConnectionHlp.Default.Connection, docente).Where(x => x.UniversidadId == null).FirstOrDefault();
                                UsuarioExpediente usuarioExpediente = new UsuarioExpediente();
                                usuarioExpediente.AlumnoID = alumno.AlumnoID;
                                usuarioExpediente.UsuarioID = usuarioDocente.UsuarioID;
                                usuarioExpedienteCtrl.Insert(ConnectionHlp.Default.Connection, usuarioExpediente);
                            }
                            else
                            {
                                //Actualiza el status premium del alumno
                                double? credito = string.IsNullOrEmpty(alumno.Credito.ToString()) ? (double?)null : Convert.ToDouble(alumno.Credito);
                                double? creditoComprado = Convert.ToDouble(Session_cantidadCreditos);
                                alumno.Credito = credito + creditoComprado;
                                eFAlumnoCtrl.Update(alumno);
                                Session_Alumno = alumno;
                            }
                        }

                        if (Session_Tutor != null)
                        {
                            tutor = tutorCtrl.Retrieve(new Tutor { TutorID = Session_Tutor.TutorID }, true).ToList().First();

                            //Actualiza el credito del tutor
                            double? credito = string.IsNullOrEmpty(tutor.Credito.ToString()) ? (double?)null : Convert.ToDouble(tutor.Credito);
                            double? creditoComprado = Convert.ToDouble(Session_cantidadCreditos);
                            tutor.Credito = credito + creditoComprado;
                            tutorCtrl.Update(tutor);
                            Session_Tutor = tutor;
                        }

                        Session["userCheckoutCompleted"] = string.Empty;
                        Session["token"] = null;
                    }
                    else
                    {
                        Response.Redirect("CheckoutError.aspx?" + retMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Session_errorPaypal = "error";
                Response.Redirect("CheckoutError.aspx?" + "ErrorCode=" + ex.Message);
            }
        }
        #endregion

        protected void Continue_Click(object sender, EventArgs e)
        {
            ((Compra)Master).GoToHomePage();
        }
    }
}