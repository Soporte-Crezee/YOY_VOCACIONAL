using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.Administracion.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace POV.WS.SendingEmailsStudents
{
    public partial class SendingEmailsStudents : ServiceBase
    {
        #region Objetos
        private Timer tmServicio = null;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private UsuarioCtrl usuarioCtrl;
        private SendMailCtrl sendMailCtrl;
        bool bandera = false;
        #endregion

        public SendingEmailsStudents()
        {
            InitializeComponent();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            usuarioCtrl = new UsuarioCtrl();
            sendMailCtrl = new SendMailCtrl();

            double intervaloMilisegundos = Convert.ToDouble(ConfigurationManager.AppSettings["IntervaloMilisegundos"]);
            double duracion = Convert.ToDouble(ConfigurationManager.AppSettings["Duracion"]);

            double tiempoEjecucion = duracion * intervaloMilisegundos;
            tmServicio = new Timer(tiempoEjecucion);
            tmServicio.Elapsed += new ElapsedEventHandler(tmServicio_Elapsed);
        }

        void tmServicio_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (bandera) return;

                bandera = true;
                EventLog.WriteEntry("SendEmailsStudents", "Servicio Iniciado", EventLogEntryType.Information);
                Action();
                EventLog.WriteEntry("SendEmailsStudents", "Servicio Finalizado", EventLogEntryType.Information);
                bandera = false;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("SendEmailsStudents", "Ocurrió el siguiente error: " + ex.Message, EventLogEntryType.Error);
            }
        }

        public void Action()
        {
            List<Usuario> lstUsers = getUsers();
            if (lstUsers.Count > 0)
            {
                int cont = 0;
                int max = Convert.ToInt32(ConfigurationManager.AppSettings["MaximoCorreos"]);

                foreach (var user in lstUsers)
                {
                    if (cont < max)
                    {
                        SendMail(user);
                        cont++;
                    }
                    else
                        break;
                }
            }
        }

        private void SendMail(Usuario user)
        {
            #region Variables
            const string titulo = "Aviso de Inactividad";
            string urllogo = ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            string linkPortal = System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalAspirante"];
            string imgAlt = "YOY - ESTUDIANTES";
            #endregion Variables

            #region Template
            string cuerpo = string.Empty;
            String filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(filePath, ConfigurationManager.AppSettings["POVUrlEmailTemplateNotification"]);
            string ubicacion = Convert.ToString(filePath);
            using (StreamReader reader = new StreamReader(ubicacion))
            {
                cuerpo = reader.ReadToEnd();
            }

            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlalt}", imgAlt);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{user}", user.NombreUsuario);
            cuerpo = cuerpo.Replace("{linkportal}", linkPortal);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);
            #endregion

            #region Send Mail
            List<string> tos = new List<string>();
            tos.Add(user.Email);
            try
            {
                sendMailCtrl.SendMail(tos, "YOY – Aviso de inactividad", cuerpo);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("SendEmailsStudents", "Ocurrió el siguiente error: " + ex.Message, EventLogEntryType.Error);
            }
            #endregion
        }

        private List<Usuario> getUsers()
        {
            List<Usuario> lstUsers = new List<Usuario>();
            Usuario usuario = new Usuario();
            //Filtro de busqueda
            DateTime date = DateTime.Now;
            DateTime filterDate = date.AddMonths(-3);
            usuario.FechaUltimoAcceso = filterDate;

            DataSet dsUsers = usuarioCtrl.Retrieve(ConnectionHlp.Default.Connection, usuario, true);

            if (dsUsers.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsUsers.Tables[0].Rows)
                {
                    Usuario oUser = new Usuario();
                    oUser = usuarioCtrl.DataRowToUsuario(row);
                    oUser = usuarioCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, oUser);
                    var lstLicencias = licenciaEscuelaCtrl.RetrieveLicencia(ConnectionHlp.Default.Connection, oUser);
                    ALicencia oLicencia = null;

                    //Se busca la licencia
                    foreach (var licencia in lstLicencias)
                    {
                        oLicencia = licencia.ListaLicencia.Find(x => (bool)x.Activo && x.Tipo == ETipoLicencia.ALUMNO);
                    }

                    if (oLicencia != null)
                    {
                        var userSocial = licenciaEscuelaCtrl.RetrieveUserSocial(ConnectionHlp.Default.Connection, oUser);
                        if (userSocial != null)
                        {
                            if (!String.IsNullOrEmpty(userSocial.ScreenName))
                            {
                                oUser.NombreUsuario = userSocial.ScreenName;
                            }
                        }
                        lstUsers.Add(oUser);
                    }
                }
            }
            return lstUsers;
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
}
