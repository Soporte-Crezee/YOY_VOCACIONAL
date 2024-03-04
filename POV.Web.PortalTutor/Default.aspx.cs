using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Services;
using POV.Comun.Service;
using POV.Core.PadreTutor.Implements;
using POV.Core.PadreTutor.Interfaces;
using POV.Licencias.BO;
using POV.Logger.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalTutor.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalTutor
{
    public partial class Default : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;
        private Tutor tutor;
        private TutorCtrl tutorCtrl;
        public bool correoConfirmado;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;
        private UsuarioCtrl usuarioCtrl;
        public Default()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            tutor = new Tutor();
            tutorCtrl = new TutorCtrl(null);
            usuarioCtrl = new UsuarioCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["__EVENTTARGET"] == "btnConfirmarCorreo") btnConfirmarCorreo();
            if (!IsPostBack)
            {
                if (!userSession.IsLogin())
                {
                    redirector.GoToLoginPage(true);
                }
                else
                {
                    Tutor tutor = tutorCtrl.Retrieve(new Tutor { TutorID = userSession.CurrentTutor.TutorID }, true).FirstOrDefault();
                    if (tutor.EstatusIdentificacion.Value)
                    {
                        Usuario usuario = usuarioCtrl.RetrieveComplete(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID });
                        if ((bool)usuario.AceptoTerminos)
                        {
                            tutor = getDataTutorToObject();
                            correoConfirmado = (bool)tutor.CorreoConfirmado;
                            txtCorreoConfirmado.Text = correoConfirmado.ToString().Trim();
                        }
                        else 
                        {
                            redirector.GoToAceptarTerminos(true);
                        }
                    }
                    else 
                    {
                        redirector.GoToConfirmarTutor(true);
                    }
                }
            }           
        }

        private void btnConfirmarCorreo()
        {
            tutor = tutorCtrl.Retrieve(new Tutor{TutorID=userSession.CurrentTutor.TutorID},true)[0];
            tutor.CorreoConfirmado = true;
            bool update = tutorCtrl.Update(tutor);
            if (update)
            {
                enviarCorreo(userSession.CurrentUser, "");
                txtCorreoConfirmado.Text = userSession.CurrentTutor.CorreoConfirmado.ToString().Trim();
                userSession.CurrentTutor = tutor;
            }
        }

        private Tutor getDataTutorToObject()
        {
            tutor.TutorID = userSession.CurrentTutor.TutorID;
            tutor = tutorCtrl.Retrieve(tutor, false)[0];

            return tutor;
        }

        private void enviarCorreo(Usuario usuario, string Password)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"]; ;
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string imgalt = "YOY - PADRES";
            const string titulo = "Correo confirmado";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = dominio + @"PortalTutor/Auth/Login.aspx";
            string nombre = userSession.CurrentTutor.Nombre + " " + userSession.CurrentTutor.PrimerApellido + " " + userSession.CurrentTutor.SegundoApellido;
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateConfirmarCorreo.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{nombre}", nombre);
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
                correoCtrl.sendMessage(tos, "YOY - Correo confirmado", cuerpo, texto, archivos, copias);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('¡Gracias por confirmar tu correo! Los datos para acceder al portal fueron enviados a tu correo. Revisa tu bandeja de entrada y/o bandeja de correo no deseado.'); window.location='" + ConfigurationManager.AppSettings["POVUrlPortalTutor"] + "';", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentalo mas tarde.');window.location='" + ConfigurationManager.AppSettings["POVUrlPortalTutor"] + "';", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }
    }
}