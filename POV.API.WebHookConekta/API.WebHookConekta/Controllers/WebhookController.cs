using conekta;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;

namespace API.WebHookConekta.Controllers
{
    public class WebhookController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        //[AllowAnonymous]
        //[HttpPost]
        //[Route("token")]
        [Route("api/webhook/pagos")]
        [HttpPost]
        public IHttpActionResult Pagos(/*[FromBody] string text*/)
        {
            string path = "";
            string result = "";
            string method_payment = "";
            string typeCard = "";
            string last4 = "";
            try
            {
                Task<string> content = ActionContext.Request.Content.ReadAsStringAsync();
                string json = content.Result;

                MailMessage mail = new MailMessage();
                string to = string.Empty;
                string toAdmin = string.Empty;
                string from = System.Configuration.ConfigurationManager.AppSettings["emailFrom"];
                string password = System.Configuration.ConfigurationManager.AppSettings["emailFromPassword"];
                string tutorName = string.Empty;

                int port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["emailPort"]);
                string host = System.Configuration.ConfigurationManager.AppSettings["emailHost"];
                bool enableSSL = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["emailSSL"]);
                toAdmin = System.Configuration.ConfigurationManager.AppSettings["emailToAdmin"];

                if (string.IsNullOrEmpty(json))
                    return InternalServerError();
                var obj = JObject.Parse(json);
                try
                {
                    path = AppDomain.CurrentDomain.BaseDirectory + "\\API\\conekta.txt";

                    using (StreamWriter s = new StreamWriter(path, true))
                    {
                        s.WriteLine(DateTime.Now.ToShortTimeString() + ">>\n" + json);
                    }
                }
                catch (Exception exxx)
                {

                }


                var data = obj.SelectToken("data");
                string orderId = "";
                string orderStatus = "";

                foreach (JToken token in obj.FindTokens("type"))
                {
                    //Console.WriteLine(token.Path + ": " + token.ToString());
                    if (token.Path == "type")
                    {
                        orderStatus = token.ToString();
                        break;
                    }
                }

                if (orderStatus == "charge.paid")
                {
                    foreach (JToken token in obj.FindTokens("id"))
                    {
                        //Console.WriteLine(token.Path + ": " + token.ToString());
                        if (token.Path == "data.object.id")
                        {
                            orderId = token.ToString();
                            break;
                        }
                    }

                    #region Update Aspirantes to active in Yoy                                                     
                    try
                    {
                        WCF.OxxoPaid.OxxoPaidServiceClient clientOxxoPaid = new WCF.OxxoPaid.OxxoPaidServiceClient();
                        result = clientOxxoPaid.UpdateAspirantes(orderId);
                    }
                    catch (Exception ePaid)
                    {
                        using (StreamWriter s = new StreamWriter(path, true))
                        {
                            s.WriteLine(DateTime.Now.ToShortTimeString() + ">>\n" + ePaid.Message);
                        }
                    }
                }
                else if (orderStatus == "order.paid")
                {
                    foreach (JToken token in obj.FindTokens("object"))
                    {
                        if (token.Path == "data.object.charges.data[0].payment_method.object")
                        {
                            method_payment = token.ToString();
                            break;
                        }

                    }

                    foreach (JToken token in obj.FindTokens("type"))
                    {
                        if (token.Path == "data.object.charges.data[0].payment_method.type")
                        {
                            typeCard = token.ToString();
                            break;
                        }

                    }

                    foreach (JToken token in obj.FindTokens("email"))
                    {
                        //Console.WriteLine(token.Path + ": " + token.ToString());
                        if (token.Path == "data.object.customer_info.email")
                        {
                            to = token.ToString();
                            break;
                        }
                    }

                    foreach (JToken token in obj.FindTokens("name"))
                    {
                        //Console.WriteLine(token.Path + ": " + token.ToString());
                        if (token.Path == "data.object.customer_info.name")
                        {
                            tutorName = token.ToString();
                            break;
                        }
                    }

                    #endregion

                    #region Consultar usuario si existe en mi base
                    try
                    {
                        WCF.OxxoPaid.OxxoPaidServiceClient clientOxxoPaid = new WCF.OxxoPaid.OxxoPaidServiceClient();
                        result = clientOxxoPaid.RetrieveAspirante(to);
                    }
                    catch (Exception ePaid)
                    {
                        using (StreamWriter s = new StreamWriter(path, true))
                        {
                            s.WriteLine(DateTime.Now.ToShortTimeString() + ">>\n" + ePaid.Message);
                        }
                    }
                    #endregion

                    if (result == "Notificar") { 
                    using (StreamWriter s = new StreamWriter(path, true))
                    {
                        s.WriteLine("ID:" + orderId);
                        s.WriteLine("Status:" + orderStatus);
                        s.WriteLine("Nombre:" + tutorName);
                        s.WriteLine("Email:" + to);
                    }
                    #region Send Mail
                    mail.IsBodyHtml = true;
                    mail.From = new MailAddress(from);//se agrega el remitente
                    mail.Subject = "Su pago de tutorado ha sido confirmado - Yoy Vocacional";
                    //to = "fcanul@plenumsoft.com.mx";
                    mail.To.Add(new MailAddress(to));
                    mail.Bcc.Add(new MailAddress(toAdmin));
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;


                    mail.Body = string.Format("<p><strong>Estimado(a) {0}: </strong></p><br>El pago que realizó mediante Conekta ({1} - {2}) para tutorados de Yoy Vocacional ha sido confirmado. A partir de este momento, sus tutorados ya pueden entrar al portal.<br><br><br>Atentamente - Yoy Vocacional", tutorName, method_payment, typeCard);

                    SmtpClient client = new SmtpClient();
                    client.Credentials = new NetworkCredential(from, password);
                    client.Port = port;
                    client.Host = host;
                    client.EnableSsl = enableSSL;

                    try
                    {
                        client.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        //Que manejo se le dará?
                        using (StreamWriter s = new StreamWriter(path, true))
                        {
                            s.WriteLine(DateTime.Now.ToShortTimeString() + ">>\n" + ex.Message);
                        }
                    }
                    #endregion
                }
            }
                if (!string.IsNullOrEmpty(result))
            {
                //Enviaremos mensaje de que no se pudo actualizar
                using (StreamWriter s = new StreamWriter(path, true))
                {
                    s.WriteLine(DateTime.Now.ToShortTimeString() + "ERROR>>\n" + result);
                }
            }
        }
            catch(Exception myEx)
            {
                using (StreamWriter s = new StreamWriter(path, true))
                {
                    s.WriteLine(DateTime.Now.ToShortTimeString() + ">>\n" + myEx.Message);
                }
            }

            return Ok();
        }
        
    }        
}