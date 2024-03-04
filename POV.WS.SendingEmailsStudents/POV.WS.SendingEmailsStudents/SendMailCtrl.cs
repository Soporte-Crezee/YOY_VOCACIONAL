using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace POV.WS.SendingEmailsStudents
{
    public class SendMailCtrl
    {
        public void SendMail(List<string> tos, string subject, string body, AlternateView av = null, List<string> files = null, List<string> CCs = null, List<string> Bcc = null)
        {
            String filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(filePath, ConfigurationManager.AppSettings["ConfigPath"]);
            var sBuilder = new StringBuilder();
            sBuilder.Append(filePath);
            // Archivo con las configuraciones del Cliente (gmail, outlook, otros), Se ubica en Files/protocolo/config.txt
            string[] lines = File.ReadAllLines(Path.GetFullPath(sBuilder.ToString()).ToString());
            // Donde cada una de las lineas representa ::
            // Remitente
            // Contraseña
            // Puerto
            // Servidor
            // Seguridad SSL
            string from = lines[0];
            string password = lines[1];

            MailMessage msg = new MailMessage();
            // Destinatarios
            foreach (var to in tos)
            {
                msg.To.Add(new MailAddress(to));
            }

            // Remitente
            msg.From = new MailAddress(from);

            // Asunto
            msg.Subject = subject;

            // Formato
            msg.SubjectEncoding = System.Text.Encoding.UTF8;

            // Mensaje HTML con Imagen
            if (av != null)
            {
                msg.AlternateViews.Add(av);
                msg.IsBodyHtml = true;
            }
            else
            {
                msg.Body = body;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = true;
            }

            // Envio con Copia
            if(CCs != null){
                foreach (var cc in CCs)
                {
                    msg.CC.Add(new MailAddress(cc));
                }
            }

            // Envio con Copia Oculta
            if (Bcc != null)
            {
                foreach (var bcc in Bcc)
                {
                    msg.Bcc.Add(new MailAddress(bcc));
                }
            }

            if (files != null)
            {
                foreach (var file in files)
                {
                    Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(file);
                    disposition.ModificationDate = File.GetLastWriteTime(file);
                    disposition.ReadDate = File.GetLastAccessTime(file);
                }
            }

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential(from, password);
            client.Port = Convert.ToInt32(lines[2]);
            client.Host = lines[3];
            client.EnableSsl = bool.Parse(lines[4]);

            try
            {
                // Envio del mensaje
                client.Send(msg);
            }
            catch (SmtpException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
