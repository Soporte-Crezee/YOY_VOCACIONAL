using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.Web.Administracion.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace POV.Envio.Correo
{
    public partial class ServiceEnvioCorreoVocacional : ServiceBase
    {
        #region Propiedades de la clase
        private Timer tmServicio = null;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private UsuarioCtrl usuarioCtrl;
        private ServiceCorreoCtrl serviceCorreoCtrl;
        #endregion

        public ServiceEnvioCorreoVocacional()
        {
            InitializeComponent();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            usuarioCtrl = new UsuarioCtrl();
            serviceCorreoCtrl = new ServiceCorreoCtrl();
            
            // Intervalo de tiempo para ejecución del servicio
            // 1 dia = 86400000
            // 1 hora = 3600000
            // 15 min = 900000
            // obtencion del intervalo de tiempo obtenido del App.config
            double intervalo = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["Servicio"]);
            double conversionmddia = intervalo * 3600000; // 1 hora en tiempo real
            tmServicio = new Timer(conversionmddia);
            tmServicio.Elapsed += new ElapsedEventHandler(tmServicio_Elapsed);
        }

        private void tmServicio_Elapsed(object sender, ElapsedEventArgs e) 
        {
            try
            {
                System.Diagnostics.EventLog.WriteEntry("ServiceEnvioCorreoVocacional", "Servicio Iniciado");
                Action();
                System.Diagnostics.EventLog.WriteEntry("ServiceEnvioCorreoVocacional", "Servicio Finalizado");
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("ServiceEnvioCorreoVocacional", "Ocurrió el siguiente error: " + ex.Message);
            }
        }

        /// <summary>
        /// Envia los correos a los usuarios del sistema
        /// </summary>
        private void Action() 
        {
            List<KeyValuePair<Usuario, ALicencia>> listaUsuarioTipo = getDataUsuarioLicenciaToList();
            if (listaUsuarioTipo.Count > 0)
            {
                int cont = 0;
                int max = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaximoCorreos"]);
                foreach (var item in listaUsuarioTipo)
                {
                    if (cont < max)
                    {
                        new Seguridad.Utils.PasswordProvider().GetNewPassword();
                        string passwordTemp = new PasswordProvider(8).GetNewPassword();
                        byte[] pws = EncryptHash.SHA1encrypt(passwordTemp);
                        Usuario original = (Usuario)item.Key.Clone();
                        item.Key.Password = pws;
                        item.Key.EmailVerificado = true;
                        usuarioCtrl.Update(ConnectionHlp.Default.Connection, item.Key, original);
                        enviarCorreo(item.Key, item.Value, passwordTemp);
                        cont++;
                    }
                    else
                        break;
                }
            }
        }

        /// <summary>
        /// Obtiene la lista de usuarios con su licencia del sistema
        /// </summary>
        /// <returns> Lista de usuarios y licencia</returns>
        private List<KeyValuePair<Usuario, ALicencia>> getDataUsuarioLicenciaToList() 
        {
            List<KeyValuePair<Usuario, ALicencia>> listaUsuariosWithLicencia = new List<KeyValuePair<Usuario, ALicencia>>();
            Usuario usuario = new Usuario();
            usuario.EmailVerificado = false;
            DataSet dsUsuario = usuarioCtrl.Retrieve(ConnectionHlp.Default.Connection, usuario);
            if (dsUsuario.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsUsuario.Tables[0].Rows)
                {
                    Usuario usuarioencontrado = new Usuario();
                    usuarioencontrado = usuarioCtrl.DataRowToUsuario(row);
                    usuarioencontrado = usuarioCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, usuarioencontrado);
                    List<LicenciaEscuela> licenciasEscuela = licenciaEscuelaCtrl.RetrieveLicencia(ConnectionHlp.Default.Connection, usuarioencontrado);
                    ALicencia licencia = null;
                    foreach (LicenciaEscuela licenciaEscuela in licenciasEscuela)
                    {
                        // buscamos la licencia
                        licencia = licenciaEscuela.ListaLicencia.Find(item => (bool)item.Activo && item.Tipo != ETipoLicencia.DIRECTOR);
                    }
                    listaUsuariosWithLicencia.Add(new KeyValuePair<Usuario, ALicencia>(usuarioencontrado, licencia));
                }                
            }
            return listaUsuariosWithLicencia;
        }

        /// <summary>
        /// Envia correo de registro existoso a los usuarios del sistema
        /// </summary>
        /// <param name="usuario"> Usuario del sistema con emailverificado false</param>
        /// <param name="licencia"> Licencia del usuario </param>
        /// <param name="password"> Password del usuario </param>
        private void enviarCorreo(Usuario usuario, ALicencia licencia, string password)
        {
            #region Variables
            string urllogo = ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];

            string imgalt = "";
            string linkportal = "";
            string cuerpo = string.Empty;
            String filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(filePath, ConfigurationManager.AppSettings["POVUrlEmailTemplateNewUser"]);
            switch (licencia.Tipo)
            {
                case ETipoLicencia.ALUMNO:
                    imgalt = "YOY - ESTUDIANTES";                    
                    linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalAspirante"];                    
                    break;
                case ETipoLicencia.DOCENTE:
                    imgalt = "YOY - ORIENTADOR";
                    linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlOrientador"];                    
                    break;
                case ETipoLicencia.TUTOR:
                    imgalt = "YOY - PADRES";
                    linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlPadres"];                    
                    break;
                default:
                    break;
            }     

            const string titulo = "Registro exitoso";
            #endregion

            string ubicacion = Convert.ToString(filePath);
            using (StreamReader reader = new StreamReader(ubicacion))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlalt}", imgalt);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{user}", usuario.NombreUsuario);
            cuerpo = cuerpo.Replace("{password}", password);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(usuario.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                serviceCorreoCtrl.sendMessage(tos, "YOY - Registro exitoso", cuerpo, texto, archivos, copias);
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("ServiceEnvioCorreoVocacional", "Ocurrió el siguiente error: " + ex.Message);
            }
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
