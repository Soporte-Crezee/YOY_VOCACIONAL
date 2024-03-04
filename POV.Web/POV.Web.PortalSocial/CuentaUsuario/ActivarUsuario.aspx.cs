using conekta;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using Newtonsoft.Json.Linq;
using POV.Administracion.BO;
using POV.Administracion.Services;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.Services;
using POV.Comun.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Logger.Service;
using POV.Seguridad.Service;
using POV.Web.PortalSocial.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.CuentaUsuario
{
    public partial class ActivarUsuario : System.Web.UI.Page
    {
        #region Propiedades de la clase
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private UsuarioCtrl usuarioCtrl;
        private AccountService accountService;
        private AlumnoCtrl alumnoCtrl;

        private NotaCompra Session_NotaCompra
        {
            get { return (NotaCompra)this.Session["NOTACOMPRA"]; }
            set { this.Session["NOTACOMPRA"] = value; }
        }


        #endregion

        public ActivarUsuario()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            accountService = new AccountService();
            usuarioCtrl = new UsuarioCtrl();
            alumnoCtrl = new AlumnoCtrl();
        }

        #region Eventos de la página
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.CurrentUser != null && userSession.LoggedIn)
                {
                    Alumno alumnoIdentificacion = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }));
                    if (alumnoIdentificacion.EstatusIdentificacion.Value && alumnoIdentificacion.NivelEscolar == ENivelEscolar.Superior)
                    {
                        // Llaves de produccion habilitadas
                        // Llaves de produccion publica y privada, respectivamente
                        // key_VXUQtCKLAXyZ3S8y6eWmjyA (Publica) key_QSuFnZEmUUJAiSkVzbr1XQ (privada)
                        // reemplazarlas por la de desarrollo
                        // Llaves de desarrollo publica y privada, respectivamente
                        // key_PV5xao1PfvTm3ydFbw186CA (Publica) key_cixWekt58XwyhoFzwS4csw (Privada)
                        // aspx Conekta.setPublishableKey y codebehind conekta.Api.apiKey
                        conekta.Api.apiKey = "key_QSuFnZEmUUJAiSkVzbr1XQ";
                        conekta.Api.version = "2.0.0";

                        EFAlumnoCtrl efAlumnoCtrl = new EFAlumnoCtrl(null);
                        DoFillExpireCard();
                        Alumno alumno = efAlumnoCtrl.Retrieve(userSession.CurrentAlumno, false).FirstOrDefault();
                        if (alumno.EstatusPago == EEstadoPago.PAGADO)
                        {
                            redirector.GoToValidarDiagnostico(false);
                        }
                        else if (alumno.EstatusPago == EEstadoPago.PENDIENTE)
                        {

                            btnSendFicha.Text = "Regresar";
                            Button4.Visible = false;
                            txtReferenciaOXXO.Text = userSession.CurrentAlumno.ReferenciaOXXO;
                            Session_NotaCompra = ProcesarPagoSeleccionado("OXXO");

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewModal(2);", true);


                        }
                    }
                    else 
                    {
                        redirector.GoToConfirmarAlumno(true);
                    }
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
        }

        #region Pagos en una sola exhibicion con tarjeta de Credito/Debito
        protected void btnPagoTarjetas_Click(object sender, EventArgs e)
        {
            try
            {

                Session_NotaCompra = ProcesarPagoSeleccionado("CreditoDebito");

                ddlVenceAnioCC.SelectedIndex = 0;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewModal(1);", true);

            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnPagarCC_Click(object sender, EventArgs e)
        {
            Order ord = new Order();
            try
            {
                ord = PagoCreditCard();
                if (ord != null)
                {
                    DoInsertNotaCompra(Session_NotaCompra);
                    DoUpdateAlumnos(userSession.CurrentAlumno, EEstadoPago.PAGADO, true);
                    EnviarCorreoPagoTarjeta(Session_NotaCompra);
                    Response.Write("<script>alert('Tu ticket de pago fue enviado a tu correo.');window.location='../Auth/ValidarDiagnostica.aspx';</script>");
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected Order PagoCreditCard()
        {
            string nombreCompleto = userSession.CurrentAlumno.NombreCompletoAlumno;
            string telefono = userSession.CurrentUser.TelefonoReferencia.Trim();
            string correo = userSession.CurrentUser.Email.Trim();

            Order order = new Order();
            try
            {
                string strJSONOrder = @"{
	                ""currency"":""MXN"",
				    ""customer_info"": {
					    ""name"": ""[Nombre]"",
					    ""phone"": ""[Telefono]"",
					    ""email"": ""[EMail]""
				    },
	                ""line_items"": [{
	                  ""name"": ""Pago de derecho para acceso a YOY"",
	                  ""unit_price"": [Costo],
	                  ""quantity"": [Cantidad]
	                }]
			    }";

                strJSONOrder = strJSONOrder.Replace("[Nombre]", nombreCompleto);
                if (string.IsNullOrEmpty(telefono)) telefono = "9993549081";
                strJSONOrder = strJSONOrder.Replace("[Telefono]", telefono);
                strJSONOrder = strJSONOrder.Replace("[EMail]", correo);
                strJSONOrder = strJSONOrder.Replace("[Costo]", (double.Parse(txtCostoUnitario.Text.Replace("$", "").Replace(",", "")) * 100).ToString());
                strJSONOrder = strJSONOrder.Replace("[Cantidad]", txtAlumnosSeleccionados.Text);
                order = new conekta.Order().create(strJSONOrder);
                
                string strJSONCharge = @"{
				    ""payment_method"": {
					    ""type"": ""card"",
					    ""token_id"": ""[Token]""
				    },
				    ""amount"": [Total]
			    }";
                strJSONCharge = strJSONCharge.Replace("[Total]", (double.Parse(txtTotalPagar.Text.Replace("$", "").Replace(",", "")) * 100).ToString());
                strJSONCharge = strJSONCharge.Replace("[Token]", hdnToken.Value.ToString());

                order.createCharge(strJSONCharge);


            }
            catch (ConektaException e)
            {
                order = null;
                foreach (JObject obj in e.details)
                {
                    System.Console.WriteLine("\n [ERROR]:\n");
                    System.Console.WriteLine("message:\t" + obj.GetValue("message"));
                    System.Console.WriteLine("debug:\t" + obj.GetValue("debug_message"));
                    System.Console.WriteLine("code:\t" + obj.GetValue("code"));
                    throw new Exception(obj.GetValue("message").ToString());
                }
            }
            return order;
        }
        #endregion

        #region Pagos en efectivo con OXXO
        protected void btnPagoOxxo_Click(object sender, EventArgs e)
        {
            Order ord = new Order();
            try
            {

                Session_NotaCompra = ProcesarPagoSeleccionado("OXXO");
                
                ord = PagoOXXO();
                if (ord != null)
                {
                    DoInsertNotaCompra(Session_NotaCompra);
                    userSession.CurrentAlumno.ReferenciaOXXO = ((dynamic)((Newtonsoft.Json.Linq.JObject)(ord.charges.data[0]))).payment_method.reference;
                    userSession.CurrentAlumno.IDReferenciaOXXO = ((dynamic)((Newtonsoft.Json.Linq.JObject)(ord.charges.data[0]))).id;
                    DoUpdateAlumnos(userSession.CurrentAlumno, EEstadoPago.PENDIENTE, false);
                    EnviarCorreoPagoOXXO(Session_NotaCompra);
                    Response.Write("<script>alert('Tu ticket de pago fue enviado a tu correo.');window.location='../Auth/Logout.aspx';</script>");
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected Order PagoOXXO()
        {
            string nombreCompleto = userSession.CurrentAlumno.NombreCompletoAlumno;
            string telefono = userSession.CurrentUser.TelefonoReferencia.Trim();
            string correo = userSession.CurrentUser.Email.Trim();

            Order order = new Order();
            try
            {
                string strJSON = @"{
	                ""currency"":""MXN"",
				    ""customer_info"": {
					    ""name"": ""[Nombre]"",
					    ""phone"": ""[Telefono]"",
					    ""email"": ""[EMail]""
				    },
	                ""line_items"": [{
	                    ""name"": ""[Pago de derecho de acceso a YOY]"",
	                    ""unit_price"": [Costo],
	                    ""quantity"": [Cantidad]
	                }],
				    ""charges"": [{
					    ""payment_method"": {
						    ""type"": ""oxxo_cash""
					    },
					    ""amount"": [Total]
				    }]
	            }";
                strJSON = strJSON.Replace("[Nombre]", nombreCompleto);
                if (string.IsNullOrEmpty(telefono)) telefono = "9993549081";
                strJSON = strJSON.Replace("[Telefono]", telefono);
                strJSON = strJSON.Replace("[EMail]", correo);
                strJSON = strJSON.Replace("[Costo]", (double.Parse(txtCostoUnitarioOXXO.Text.Replace("$", "").Replace(",", "")) * 100).ToString());
                strJSON = strJSON.Replace("[Cantidad]", txtCantidadOXXO.Text);
                strJSON = strJSON.Replace("[Total]", (double.Parse(txtTotalPagaOXXO.Text.Replace("$", "").Replace(",", "")) * 100).ToString());

                order = new conekta.Order().create(strJSON);
            }
            catch (ConektaException e)
            {
                order = null;
                foreach (JObject obj in e.details)
                {
                    System.Console.WriteLine("\n [ERROR]:\n");
                    System.Console.WriteLine("message:\t" + obj.GetValue("message"));
                    System.Console.WriteLine("debug:\t" + obj.GetValue("debug_message"));
                    System.Console.WriteLine("code:\t" + obj.GetValue("code"));
                    throw new Exception(obj.GetValue("message").ToString());
                }
            }
            return order;
        }
        #endregion

        #region Pago a Meses sin intereses con tarjeta de credito
        protected void btnPagoMeses_Click(object sender, EventArgs e)
        {
            try
            {
                Session_NotaCompra = ProcesarPagoSeleccionado("MesesSinIntereses");
                
                ddlVenceAnioMSS.SelectedIndex = 0;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewModal(3);", true);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }

        }

        protected void btnPagarMSS_Click(object sender, EventArgs e)
        {
            Order ord = new Order();
            try
            {
                ord = PagoMesesSinIntereses();
                if (ord != null)
                {
                    DoInsertNotaCompra(Session_NotaCompra);
                    DoUpdateAlumnos(userSession.CurrentAlumno, EEstadoPago.PAGADO, true);
                    EnviarCorreoPagoTarjeta(Session_NotaCompra);
                    Response.Write("<script>alert('Tu ticket de pago fue enviado a tu correo.');window.location='../Auth/ValidarDiagnostica.aspx';</script>");
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected Order PagoMesesSinIntereses()
        {
            string nombreCompleto = userSession.CurrentAlumno.NombreCompletoAlumno;
            string telefono = userSession.CurrentUser.TelefonoReferencia.Trim();
            string correo = userSession.CurrentUser.Email.Trim();

            Order order = new Order();
            try
            {
                string strJSONOrder = @"{
	                ""currency"":""MXN"",
				    ""customer_info"": {
					    ""name"": ""[Nombre]"",
					    ""phone"": ""[Telefono]"",
					    ""email"": ""[EMail]""
				    },
	                ""line_items"": [{
	                  ""name"": ""Pago de derecho para acceso a YOY"",
	                  ""unit_price"": [Costo],
	                  ""quantity"": [Cantidad]
	                }]
			    }";

                strJSONOrder = strJSONOrder.Replace("[Nombre]", nombreCompleto);
                if (string.IsNullOrEmpty(telefono)) telefono = "9993549081";
                strJSONOrder = strJSONOrder.Replace("[Telefono]", telefono);
                strJSONOrder = strJSONOrder.Replace("[EMail]", correo);
                strJSONOrder = strJSONOrder.Replace("[Costo]", (double.Parse(txtCostoUnitarioMSS.Text.Replace("$", "").Replace(",", "")) * 100).ToString());
                strJSONOrder = strJSONOrder.Replace("[Cantidad]", txtCantidaMSS.Text);
                order = new conekta.Order().create(strJSONOrder);

                string strJSONCharge = @"{
				    ""payment_method"": {
					    ""type"": ""card"",
					    ""token_id"": ""[Token]"",
					    ""monthly_installments"": ""[Plazo]""
				    },
				    ""amount"": [Total]
			    }";
                strJSONCharge = strJSONCharge.Replace("[Total]", (double.Parse(txtTotalPagoMSS.Text.Replace("$", "").Replace(",", "")) * 100).ToString());
                strJSONCharge = strJSONCharge.Replace("[Token]", hdnToken.Value.ToString());
                strJSONCharge = strJSONCharge.Replace("[Plazo]", ddlPlazo.SelectedValue.ToString().Trim());
                order.createCharge(strJSONCharge);


            }
            catch (ConektaException e)
            {
                order = null;
                foreach (JObject obj in e.details)
                {
                    System.Console.WriteLine("\n [ERROR]:\n");
                    System.Console.WriteLine("message:\t" + obj.GetValue("message"));
                    System.Console.WriteLine("debug:\t" + obj.GetValue("debug_message"));
                    System.Console.WriteLine("code:\t" + obj.GetValue("code"));
                    throw new Exception(obj.GetValue("message").ToString());
                }
            }
            return order;
        }
        #endregion

        #region Metodos auxiliares

        void DoFillExpireCard()
        {

            #region Drop Mes
            for (int i = 1; i < 15; i++)
            {
                ddlVenceAnioCC.Items.Add(new ListItem(((DateTime.Now.Year - 1) + i).ToString().Trim(), (DateTime.Now.Year + i).ToString().Trim()));
                ddlVenceAnioMSS.Items.Add(new ListItem(((DateTime.Now.Year - 1) + i).ToString().Trim(), (DateTime.Now.Year + i).ToString().Trim()));
            }
            #endregion
        }

        protected NotaCompra ProcesarPagoSeleccionado(string strWhoCall)
        {
            Alumno alumno = new Alumno();

            ProductoCosteoCtrl prod = new ProductoCosteoCtrl(null);
            List<ProductoCosteo> lstProductoCosteo = (prod.RetrieveWithRelationship(new ProductoCosteo(), false)).ToList();

            CostoProducto miProducto = new CostoProducto();

            miProducto = lstProductoCosteo.Where(x => x.Nombre == "Paquete 1").FirstOrDefault().CostoProducto.Where(y => y.FechaFin == null).FirstOrDefault();

            switch (strWhoCall)
            {
                case "CreditoDebito":
                    txtAlumnosSeleccionados.Text = "1";
                    txtCostoUnitario.Text = String.Format("{0:C}", miProducto.Precio);
                    txtTotalPagar.Text = String.Format("{0:C}", miProducto.Precio);
                    break;
                case "OXXO":
                    txtCantidadOXXO.Text = "1";
                    txtCostoUnitarioOXXO.Text = String.Format("{0:C}", miProducto.Precio);
                    txtTotalPagaOXXO.Text = String.Format("{0:C}", miProducto.Precio);
                    break;
                case "MesesSinIntereses":
                    txtCantidaMSS.Text = "1";
                    txtCostoUnitarioMSS.Text = String.Format("{0:C}", miProducto.Precio);
                    txtTotalPagoMSS.Text = String.Format("{0:C}", miProducto.Precio);
                    break;
            }

            NotaCompra notaCompra = new NotaCompra()
            {
                AlumnoID = userSession.CurrentAlumno.AlumnoID,
                FechaCompra = DateTime.Now,
                CostoProductoID = miProducto.CostoProductoId,
                Cantidad = 1,
                ConceptoCompra = "Pago de derecho para acceso a YOY"
            };

            return notaCompra;
        }

        protected void DoInsertNotaCompra(NotaCompra notaCompra)
        {
            NotaCompraCtrl notaCompraCtrl = new NotaCompraCtrl(null);
            notaCompraCtrl.Insert(notaCompra);
        }

        protected void DoUpdateAlumnos(Alumno alumno, EEstadoPago EstatusPago, bool notificacionpago)
        {
            EFAlumnoCtrl efAlumnoCtrl = new EFAlumnoCtrl(null);

            Alumno alumnoUpd = efAlumnoCtrl.Retrieve(new Alumno() { AlumnoID = alumno.AlumnoID}, true).FirstOrDefault();
            alumnoUpd.EstatusPago = EstatusPago;
            alumnoUpd.ReferenciaOXXO = alumno.ReferenciaOXXO;
            alumnoUpd.IDReferenciaOXXO = alumno.IDReferenciaOXXO;
            alumnoUpd.NotificacionPago = notificacionpago;
            efAlumnoCtrl.Update(alumnoUpd);
        }
        #endregion

        #region *****Message  Showing*****
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
            Control m = Page.Master.FindControl("hdnLastMessage");
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

        protected void btnSendFicha_Click(object sender, EventArgs e)
        {
            redirector.GoToLogoutPage(false);
        }

        private void EnviarCorreoPagoTarjeta(NotaCompra nota, string strWhoCall = "CreditCard")
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string titulo = "Pago de derecho";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = ConfigurationManager.AppSettings["POVUrlPortalAspirante"];
            string location = ConfigurationManager.AppSettings["POVUrlTutorados"];
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplatePagoTarjeta.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{nombre}", userSession.CurrentAlumno.NombreCompletoAlumno);
            if (strWhoCall == "MesesSinIntereses")
                cuerpo = cuerpo.Replace("{plazo}", "a " + ddlPlazo.SelectedValue.ToString().Trim() + " meses sin intereses ");
            else
                cuerpo = cuerpo.Replace("{plazo}", string.Empty);

            cuerpo = cuerpo.Replace("{cantidad}", "1");
            CostoProductoCtrl ctrl = new CostoProductoCtrl(null);
            CostoProducto costo = ctrl.Retrieve(new CostoProducto() { CostoProductoId = nota.CostoProductoID }, false).FirstOrDefault();
            cuerpo = cuerpo.Replace("{costo}", String.Format("{0:C}", costo.Precio));
            cuerpo = cuerpo.Replace("{total}", String.Format("{0:C}", (costo.Precio * nota.Cantidad)));
            cuerpo = cuerpo.Replace("{usuario}", userSession.CurrentUser.NombreUsuario);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(userSession.CurrentUser.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Nota de compra", cuerpo, texto, archivos, copias);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('¡La notificación ha sido enviada correctamente!'); window.location='" + location + "';", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentelo mas tarde.');", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }

        private void EnviarCorreoPagoOXXO(NotaCompra nota) 
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string titulo = "Pago de derecho";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = ConfigurationManager.AppSettings["POVUrlPortalAspirante"];
            string location = ConfigurationManager.AppSettings["POVUrlTutorados"];
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplatePagoOXXO.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{nombre}", userSession.CurrentAlumno.NombreCompletoAlumno);
            CostoProductoCtrl ctrl = new CostoProductoCtrl(null);
            CostoProducto costo = ctrl.Retrieve(new CostoProducto() { CostoProductoId = nota.CostoProductoID }, false).FirstOrDefault();
            cuerpo = cuerpo.Replace("{cantidad}", String.Format("{0:C}", (costo.Precio * nota.Cantidad)));
            cuerpo = cuerpo.Replace("{referencia}", txtReferenciaOXXO.Text);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(userSession.CurrentUser.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Nota de compra", cuerpo, texto, archivos, copias);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentelo mas tarde.');", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }

        #endregion
    }
}