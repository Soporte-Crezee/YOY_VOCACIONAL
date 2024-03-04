using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GP.SocialEngine.BO;
using System.Data;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.DA;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Licencias.Service;
using POV.Licencias.BO;
using System.Net.Mail;
using POV.CentroEducativo.Services;
using POV.Logger.Service;
using System.IO;
using System.Text.RegularExpressions;
using POV.Web.PortalTutor.Helper;
using POV.Core.PadreTutor.Implements;
using POV.Core.PadreTutor.Interfaces;

namespace POV.Web.PortalSocial.CuentaUsuario
{
    public partial class ConfirmarTutor : System.Web.UI.Page
    {
        private AccountService accountService;
        IUserSession userSession = new UserSession();
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;
        private IRedirector redirector = new Redirector();

        private TutorCtrl tutorCtrl;

        public ConfirmarTutor()
        {
            tutorCtrl = new TutorCtrl(null);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["__EVENTTARGET"] == "BtnEnviar_Click") BtnEnviar_Click(sender, e);
            if (!IsPostBack)
            {
                if (userSession.CurrentUser != null && userSession.LoggedIn)
                {
                    Tutor tutor = tutorCtrl.Retrieve(new Tutor { TutorID = userSession.CurrentTutor.TutorID }, true).FirstOrDefault();
                    if (!tutor.EstatusIdentificacion.Value)
                    {
                        DatosPersonales();
                    }
                    else
                        redirector.GoToAceptarTerminos(true);
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
            else
            {
                if (!userSession.IsLogin())
                {
                    redirector.GoToLoginPage(true);
                }
            }
        }

        private void DatosPersonales()
        {
            DateTime fechaNacimiento = (DateTime)userSession.CurrentTutor.FechaNacimiento;
            lblNombre.Text = userSession.CurrentTutor.Nombre;
            lblApellidos.Text = userSession.CurrentTutor.PrimerApellido + " " + userSession.CurrentTutor.SegundoApellido;
            lblFechaNacimiento.Text = fechaNacimiento.ToString("dd/MM/yyyy");
            if ((bool)userSession.CurrentTutor.Sexo)
                lblGenero.Text = "Hombre";
            else
                lblGenero.Text = "Mujer";
            lblCorreo.Text = userSession.CurrentUser.Email;
            txtNombreUsuario.Text = userSession.CurrentUser.NombreUsuario;
            txtAntNombreUsuario.Text = userSession.CurrentUser.NombreUsuario;

            #region LLenar Tutores vinculados
            grdTutores.DataSource = null;
            grdTutores.DataSource = userSession.CurrentTutor.Alumnos;
            grdTutores.DataBind();

            #endregion

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Tutor tutorNuevo = (Tutor)userSession.CurrentTutor.Clone();
            Tutor tutorPrevio = (Tutor)userSession.CurrentTutor.Clone();
            Usuario usuarioPrevio = (Usuario)userSession.CurrentUser.Clone(); ;
            Usuario usuarioNuevo = (Usuario)userSession.CurrentUser.Clone();
            try
            {
                tutorNuevo.EstatusIdentificacion = true;
                usuarioNuevo = UsuarioToObjeto(usuarioNuevo);
                UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                usuarioCtrl.Update(dctx, usuarioNuevo, usuarioPrevio);
                Tutor tutor = tutorCtrl.Retrieve(new Tutor { TutorID = userSession.CurrentTutor.TutorID }, true).FirstOrDefault();
                tutor.EstatusIdentificacion = true;
                tutorCtrl.Update(tutor);

                userSession.CurrentTutor.EstatusIdentificacion = true;

                if (usuarioNuevo.NombreUsuario != usuarioPrevio.NombreUsuario)
                {
                    if (usuarioNuevo.Email != null)
                    {
                        EnviarCorreo(usuarioNuevo);
                        Response.Write("<script>alert('Tu usuario ha sido actualizado.Te hemos enviado un correo con tu nuevo usuario.');window.location='AceptarTerminos.aspx';</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Tu usuario ha sido actualizado.');window.location='AceptarTerminos.aspx';</script>");
                    }
                }
                else
                {
                    Response.Write("<script>window.location='AceptarTerminos.aspx';</script>");
                }

            }
            catch (Exception ex)
            {
                tutorNuevo.EstatusIdentificacion = false;
                MostrarMensajeError(ex);
            }
            finally
            {
            }
        }


        private void EnviarCorreo(Usuario usuarioNuevo)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string imgalt = "YOY - PADRES";
            const string titulo = "Nuevo usuario";
            string linkportal = ConfigurationManager.AppSettings["POVUrlPortalTutor"];
            #endregion
            string cuerpo = string.Empty;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateNuevoUser.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{nombreusuario}", usuarioNuevo.NombreUsuario);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(usuarioNuevo.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();

            try
            {
                correoCtrl.sendMessage(tos, "Cambio de Nombre de Usuario", cuerpo, texto, archivos, copias);
            }
            catch (Exception ex)
            {
                ex.Source = "EnviarCorreo";
                Response.Write("<script>alert('Tu usuario ha sido actualizado. Por alguna razón no pudimos enviarte un correo con tu nuevo usuario, te recomendamos apuntarlo.');window.location='Default.aspx';</script>");
            }

        }

        private void MostrarMensajeError(Exception ex)
        {

            if (ex.Source == "Usuario")
            {
                lblMensajeError.Text = "El usuario no esta disponible";
            }
            if (ex.Source == "usuarioTamano")
            {
                lblMensajeError.Text = "El usuario es de mínimo 6 caracteres y máximo 50 caracteres.";
            }
            if (ex.Source == "whitespace")
            {
                lblMensajeError.Text = "El usuario no puede tener espacios en blanco.";
            }
            if (ex.Source == "Telefono")
            {
                lblMensajeError.Text = "El teléfono no esta disponible";
            }
            if (ex.Source == "Email")
            {
                lblMensajeError.Text = "El correo ya esta en uso. Por favor proporcione otro.";
            }
            if (ex.Source == "EnviarCorreo")
            {
                Response.Write("<script>alert('Tu nombre de usuario ha sido actualizado, pero no pudimos enviarte un correo. Recuerda apuntar tu nuevo usuario.);</script>");
            }
        }

        protected Usuario UsuarioToObjeto(Usuario usuarioNuevo)
        {
            if (txtNombreUsuario.Text.Trim().Contains(" "))
            {
                var ex = new Exception();
                ex.Source = "whitespace";
                throw (ex);
            }

            if (txtNombreUsuario.Text.Trim() == "" || (txtNombreUsuario.Text.Count() < 6 || txtNombreUsuario.Text.Count() > 50))
            {
                var ex = new Exception();
                ex.Source = "usuarioTamano";
                throw (ex);
            }
            else
                usuarioNuevo.NombreUsuario = txtNombreUsuario.Text.Trim();
            return usuarioNuevo;
        }
        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = txtDatosIncorrectos.Text.Trim();
                //Formato Incorrecto
                if (mensaje.Length > 0)
                {
                    enviarCorreo(mensaje);
                    accountService = new AccountService();
                    accountService.Logout();
                }
                else
                {
                    if (string.IsNullOrEmpty(mensaje))
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('El mensaje no debe estar vacio');", true);

                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
            }

        }

        //Enviar Correo de registro exitoso
        private void enviarCorreo(string mensaje)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string titulo = "Datos incorrectos de tutors";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = ConfigurationManager.AppSettings["POVUrlOperaciones"];
            string location = ConfigurationManager.AppSettings["POVUrlLocation"];
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateDatosIncorrectos.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{tipopersona}", "tutor");
            cuerpo = cuerpo.Replace("{nombre}", userSession.CurrentTutor.Nombre + " " + userSession.CurrentTutor.PrimerApellido + " " + userSession.CurrentTutor.SegundoApellido);
            cuerpo = cuerpo.Replace("{mensaje}", mensaje);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(ConfigurationManager.AppSettings["EmailSoporte"]);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Confirmar datos", cuerpo, texto, archivos, copias);               
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentelo mas tarde.');", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }

        private bool ValidateEmailRegex(string email)
        {
            string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reLenient = new Regex(patternLenient);

            bool match = reLenient.IsMatch(email);
            return match;
        }

        protected void grdTutores_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}