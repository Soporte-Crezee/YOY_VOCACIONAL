using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.LandingPage
{
    public partial class Default1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var request = Request.Form;

            if (request["sendMail"] == "OK")
            {
                this.sendMail(request);
            }
        }

        private void sendMail(System.Collections.Specialized.NameValueCollection request)
        {
            string user = System.Configuration.ConfigurationManager.AppSettings["emailFromUser"];
            string password = System.Configuration.ConfigurationManager.AppSettings["emailFromPassword"];
            string from = string.IsNullOrEmpty(request["email"]) ? System.Configuration.ConfigurationManager.AppSettings["emailFromUser"] : request["email"];
            string subject = "Contacto desde LandingPage Yoy";
            string to = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["emailTo"]) ? "fcanul@plenumsoft.com.mx" : System.Configuration.ConfigurationManager.AppSettings["emailTo"];
            string body = "<strong>Nuevo mensaje de contacto</strong> <br><br>" + request["soy"] + "<br><br>" + request["nombre"] + " desea mayor información sobre Yoy <BR><br> <strong>Teléfono:</strong> " + request["telefono"] + "<br> <strong>Institución:</strong> " + (string.IsNullOrEmpty(request["institucion"]) ? "NA" : request["institucion"]) + " <br> <strong>Cargo</strong> " + (string.IsNullOrEmpty(request["cargo"]) ? "NA" : request["cargo"]) + "<br> <strong>Email:</strong> " + request["email"] + "<br> <strong>Ubicación:</strong> " + request["pais"] + ", " + request["estado"] + ", " + request["ciudad"] + "<br> <strong>Como sé enteró:</strong> " + request["comonos"] + "<br><strong>Mensaje:<br></strong>" + request["mensaje"];
            int port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["emailPort"]);
            string host = System.Configuration.ConfigurationManager.AppSettings["emailHost"];
            bool enableSSL = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["emailSSL"]);

            MailMessage msg = new MailMessage();
            
            msg.To.Add(new MailAddress(to));//se agrega el destinatario            
            msg.From = new MailAddress(from);//se agrega el remitente

            msg.Subject = subject;//se agrega el asunto
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            
            msg.Body = body;//se agrega el mensaje
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;
            

            //if (CCs != null)
            //{
            //    foreach (string cc in CCs)
            //    {
            //        msg.CC.Add(new MailAddress(cc));//se agregan los destinatarios con copias
            //    }
            //}                       

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential(user, password);
            client.Port = port;
            client.Host = host;
            client.EnableSsl = enableSSL;
            try
            {
                client.Send(msg);//se envia el mensaje
                Context.Response.Write("Correo enviado");
            }
            catch (SmtpException e)
            {
                Context.Response.Write("Correo no enviado");                
            }
        }        
    }
}