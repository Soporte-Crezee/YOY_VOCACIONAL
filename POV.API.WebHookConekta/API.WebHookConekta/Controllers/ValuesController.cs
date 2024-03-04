using conekta;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace API.WebHookConekta.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values        
        [HttpPost]
        public HttpResponseMessage Webhook(string json)
        {

            MailMessage mail = new MailMessage();

            string to = string.Empty;
            string from = System.Configuration.ConfigurationManager.AppSettings["emailFrom"];
            string password = System.Configuration.ConfigurationManager.AppSettings["emailFromPassword"];
            string tutorName = string.Empty;

            int port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["emailPort"]);
            string host = System.Configuration.ConfigurationManager.AppSettings["emailHost"];
            bool enableSSL = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["emailSSL"]);

            conekta.Order order = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(json);
            var obj = JObject.Parse(json);
            var data = obj.SelectToken("data");
            var or = order.charges.data;
            string orderId = "";

            foreach (JToken token in obj.FindTokens("id"))
            {
                //Console.WriteLine(token.Path + ": " + token.ToString());
                if (token.Path == "id")
                {
                    orderId = token.ToString();
                }
            }

            string orderStatus = "";

            foreach (JToken token in obj.FindTokens("type"))
            {
                //Console.WriteLine(token.Path + ": " + token.ToString());
                if (token.Path == "type")
                {
                    orderStatus = token.ToString();
                }
            }

            if (orderStatus == "charge.paid")
            {
                #region Update Aspirantes to active in Yoy                
                #endregion
                #region Send Mail
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(from);//se agrega el remitente
                mail.Subject = "Pago comprobado";
                to = "fcanul@plenumsoft.com.mx";
                mail.To.Add(new MailAddress(to));
                mail.SubjectEncoding = System.Text.Encoding.UTF8;


                mail.Body = string.Format("<p><strong>Estimado {0}: </strong></p><br>Tu pago ha sido confirmado.", tutorName);

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
                }
                #endregion

            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }

    public static class JsonExtensions
    {
        public static List<JToken> FindTokens(this JToken containerToken, string name)
        {
            List<JToken> matches = new List<JToken>();
            FindTokens(containerToken, name, matches);
            return matches;
        }

        private static void FindTokens(JToken containerToken, string name, List<JToken> matches)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    if (child.Name == name)
                    {
                        matches.Add(child.Value);
                    }
                    FindTokens(child.Value, name, matches);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    FindTokens(child, name, matches);
                }
            }
        }
    }
}
