using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using GP.SocialEngine.BO;
using POV.Logger.Service;
using POV.CentroEducativo.Service;
using POV.Web.Administracion.Helper;
using POV.CentroEducativo.BO;
using System.Data;
using System.Net.Mail;
using POV.Comun.Service;
using System.Configuration;
using POV.Seguridad.BO;
using System.IO;
using POV.CentroEducativo.Services;

namespace POV.Web.PortalSocial.PortalAlumno
{
    public partial class Noticias : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IUserSession alumnoPrevious;
        private IRedirector redirector;
        public bool estatusIdentificacion;
        public bool correoConfirmado;
        public bool datosCompletos;
        AlumnoCtrl alumnoCtrl;
        private Alumno alumno;

        public Noticias()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            alumnoPrevious = new UserSession();
            alumnoCtrl = new AlumnoCtrl();
            alumno = new Alumno();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form["__EVENTTARGET"] == "btnActualizarEstatusIdentificacion") btnActualizarEstatusIdentificacion();
                if (Request.Form["__EVENTTARGET"] == "btnConfirmarCorreo") btnConfirmarCorreo();

                if (!IsPostBack)
                {
                    if (userSession.IsLogin() && userSession.IsAlumno())
                    {
                        alumno = getDataAlumnoToObject();
                        if ((bool)alumno.EstatusIdentificacion)
                        {
                            if (alumno.EstatusPago == EEstadoPago.PAGADO)
                            {
                                ((PortalAlumno)this.Master).LoadControlsAlumnoMaster((long)userSession.CurrentUsuarioSocial.UsuarioSocialID, false);

                                this.hdnSessionSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                                this.hdnSessionUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();
                                hdnSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                                hdnUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();
                                hdnTipoPublicacionSuscripcionReactivo.Value = ((short)ETipoPublicacion.REACTIVO).ToString();
                                hdnTipoPublicacionTexto.Value = ((short)ETipoPublicacion.TEXTO).ToString();

                                
                                estatusIdentificacion = (bool)alumno.EstatusIdentificacion;
                                txtEstatusIdentificacion.Text = estatusIdentificacion.ToString().Trim();
                                if (alumno.DatosCompletos != null)
                                {
                                    txtDatosCompletos.Text = alumno.DatosCompletos.ToString();
                                }
                                else
                                {
                                    txtDatosCompletos.Text = "False";
                                }
                                correoConfirmado = (bool)alumno.CorreoConfirmado;
                                txtCorreoConfirmado.Text = correoConfirmado.ToString().Trim();
                            }
                            else
                            {
                                Response.Redirect("~/CuentaUsuario/ActivarUsuario.aspx", false);
                            }
                        }
                        else
                        {
                            redirector.GoToConfirmarAlumno(true);
                        }
                    }
                    else
                        redirector.GoToLoginPage(false);
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        private void btnActualizarEstatusIdentificacion()
        {
            alumno = getDataAlumnoToObject();
            alumnoCtrl.Update(ConnectionHlp.Default.Connection, userSession.CurrentAlumno, alumno);
            txtEstatusIdentificacion.Text = userSession.CurrentAlumno.EstatusIdentificacion.ToString().Trim();
            txtDatosCompletos.Text = userSession.CurrentAlumno.DatosCompletos.ToString().Trim();
        }

        private void btnConfirmarCorreo()
        {
            alumno = getDataAlumnoToObject();
            Alumno alumnoNuevo = (Alumno)alumno.Clone();
            alumnoNuevo.CorreoConfirmado = true;
            alumnoNuevo.EstatusIdentificacion = alumno.EstatusIdentificacion;

            alumnoCtrl.Update(ConnectionHlp.Default.Connection, alumnoNuevo, alumno);
            //enviarCorreo(userSession.CurrentUser, "");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "window.location='" + ConfigurationManager.AppSettings["POVUrlPortalSocial"] + "';", true);
            txtCorreoConfirmado.Text = userSession.CurrentAlumno.CorreoConfirmado.ToString().Trim();
            EFAlumnoCtrl efAlumnoCtrl = new EFAlumnoCtrl(null);
            alumno = efAlumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();
            userSession.CurrentAlumno = alumno;
        }

        private Alumno getDataAlumnoToObject()
        {

            alumno.AlumnoID = userSession.CurrentAlumno.AlumnoID;
            DataSet ds = alumnoCtrl.Retrieve(ConnectionHlp.Default.Connection, alumno);

            if (ds.Tables[0].Rows.Count == 1)
                alumno = alumnoCtrl.LastDataRowToAlumno(ds);

            return alumno;
        }

        private void enviarCorreo(Usuario usuario, string Password)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string imgalt = "YOY - ESTUDIANTES";
            const string titulo = "Correo confirmado";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = dominio + @"PortalSocial/Auth/ValidarAutoLogin.aspx" + GenerarTokenYUrl(userSession.CurrentAlumno);
            string nombre = userSession.CurrentUsuarioSocial.ScreenName;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('¡Gracias por confirmar tu correo! Los datos para acceder al portal fueron enviados a tu correo. Revisa tu bandeja de entrada y/o bandeja de correo no deseado.'); window.location='" + ConfigurationManager.AppSettings["POVUrlPortalSocial"] + "';", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentalo mas tarde.');window.location='" + ConfigurationManager.AppSettings["POVUrlPortalSocial"] + "';", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }

        private string GenerarTokenYUrl(Alumno alumno)
        {
            string strUrlAutoLogin;
            string formatoFecha = "yyyyMMddHHmmss.fff";
            DateTime fecha = System.DateTime.Now;
            string nombre = alumno.Nombre;
            nombre = nombre.Trim();
            string apellido = alumno.PrimerApellido;
            apellido = apellido.Trim();
            string curp = alumno.Curp;
            DateTime fechaNacimiento = (DateTime)alumno.FechaNacimiento;
            curp = curp.Trim();
            string token = fechaNacimiento.ToString(formatoFecha) + nombre + apellido + curp + fecha.ToString(formatoFecha);
            byte[] clave = POV.Seguridad.Utils.EncryptHash.SHA1encrypt(token);
            token = POV.Seguridad.Utils.EncryptHash.byteArrayToStringBase64(clave);
            token = System.Web.HttpUtility.UrlEncode(token);
            strUrlAutoLogin = "?alumno=" + alumno.Curp + "&fechahora=" + fecha.ToString(formatoFecha) + "&token=" + token;
            return strUrlAutoLogin;
        }
    }
}