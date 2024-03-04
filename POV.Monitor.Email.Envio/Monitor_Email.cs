using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using GP.SocialEngine.BO;
using POV.Comun.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Logger.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.Administracion.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Net;
using System.Net.Mime;
namespace POV.Monitor.Email.Envio
{
    public partial class Monitor_Email : ServiceBase
    {
        Timer tmServicio = null;
        AlumnoCtrl alumnoCtrl;
        LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private Alumno alumno;
        public bool correoConfirmado;
        UsuarioCtrl usuarioCtrl;
        EnviarCorreo correoCtrl;

        public Monitor_Email()
        {
            InitializeComponent();
            alumnoCtrl = new AlumnoCtrl();
            alumno = new Alumno();

            //Licencia
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            usuarioCtrl = new UsuarioCtrl();
            correoCtrl = new EnviarCorreo();

            double intervalo = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["Servicio"]);
            
            // 1d = 86400000
            // 1h = 3600000
            // 15m = 900000
            double conversionmddia = intervalo * 86400000; // 1 dia tiempo real
            tmServicio = new Timer(conversionmddia);
            tmServicio.Elapsed += new ElapsedEventHandler(tmServicio_Elapsed);            
        }

        void tmServicio_Elapsed(object sender, ElapsedEventArgs e) 
        {
            try
            {
                List<Usuario> lista = getDataUsuarioToList();
                foreach (var item in lista)
                {                    
                    enviarCorreo(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Aplication", "Ocurrió el siguiente error: " + ex.Message);                
            }
        }

        private void enviarCorreo(Usuario usuario)
        {            
            #region Variables
            string urlimg = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgEmail"]; ;
            const string imgalt = "YOY - ESTUDIANTES";
            const string titulo = "Confirmación de correo";
            string linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalSocial"];
            #endregion

            string cuerpo = string.Empty;
            String filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(filePath, System.Configuration.ConfigurationManager.AppSettings["EmailPath"]);
            string ubicacion = Convert.ToString(filePath);
            using (StreamReader reader = new StreamReader(ubicacion))
            {
                
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urlimage}", urlimg);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{user}", usuario.NombreUsuario);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);

            List<string> tos = new List<string>();
            tos.Add(usuario.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Confirmacion de correo", cuerpo, texto, archivos, copias);
            }
            catch (Exception ex)
            {
               //Message ex log
            }
        }

        private List<Usuario> getDataUsuarioToList() 
        {
            List<Usuario> listaAlumnos = new List<Usuario>();
            alumno.CorreoConfirmado = false;
            DataSet ds = alumnoCtrl.Retrieve(ConnectionHlp.Default.Connection, alumno);
            if (ds.Tables[0].Rows.Count > 0)
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Alumno student = alumnoCtrl.DataRowToAlumno(ds.Tables[0].Rows[i]);
                    //LicenciaAlumno
                    Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuario(ConnectionHlp.Default.Connection, student);
                    usuario = usuarioCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, usuario);
                    if (usuario.Email != null) 
                    {
                        listaAlumnos.Add(usuario);       
                    }
                }
            return listaAlumnos;
        }

        protected override void OnStart(string[] args)
        {
            tmServicio.Start();
            
        }

        protected override void OnStop()
        {
            tmServicio.Stop();
        }
    }


    public class EnviarCorreo
    {
        public void sendMessage(List<string> tos, string subject, string body, AlternateView av, List<string> files, List<string> CCs)
        {
            String filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(filePath, System.Configuration.ConfigurationManager.AppSettings["ConfigPath"]);
            var sbuilder = new StringBuilder();
            sbuilder.Append(filePath);
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
            //msg.From = new MailAddress(from,"alias",System.Text.Encoding.UTF8);
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
