using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;


namespace POV.Comun.Service
{
    public class CorreoCtrl
    {
        public void sendMessage(List<string> tos,string subject, string body,AlternateView av,List<string> files, List<string> CCs)
        {
            var sbuilder = new StringBuilder();
            sbuilder.Append(AppDomain.CurrentDomain.BaseDirectory);
            sbuilder.Append( @"Files\protocolo\config.txt" );
            string[] lines = System.IO.File.ReadAllLines(Path.GetFullPath(sbuilder.ToString()).ToString());//archivo con las configuraciones del cliente, este archivo debe estar en    POV.Comun.Services/Service/config.txt
            //donde la primera linea es el remitente
            //la segunda linea es la contraseña
            //la tercera el puerto
            //y la cuarta el servidor
            //la quinta indica si es con seguridad  ssl
            string from = lines[0];
            string password = lines[1];
            MailMessage msg = new MailMessage();
            foreach (string to in tos)
            {
                msg.To.Add(new MailAddress(to));//se agrega el destinatario
            }
            msg.From = new MailAddress(from);//se agrega el remitente
            msg.Subject = subject;//se agrega el asunto
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            if (av != null)
            {
                msg.AlternateViews.Add(av);//se agrega el mensaje html con imagen
                msg.IsBodyHtml = true;
            }
            else if (body != null)
            {
                msg.Body = body;//se agrega el mensaje
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = true;
            }

            if (CCs != null)
            {
                foreach (string cc in CCs)
                {
                    msg.CC.Add(new MailAddress(cc));//se agregan los destinatarios con copias
                }
            }

            if (files != null)
            {
                foreach (string file in files)
                {
                    Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(file);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                    msg.Attachments.Add(data);//se adjutan los archivos
                }
            }

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential(from, password);
            client.Port = Convert.ToInt32(lines[2]);
            client.Host = lines[3];
            client.EnableSsl = bool.Parse(lines[4]);
            try
            {
                client.Send(msg);//se envia el mensaje
            }
            catch (SmtpException e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
