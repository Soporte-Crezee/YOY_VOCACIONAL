using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using Framework.Base.DataAccess;
using POV.Web.PortalSocial.AppCode;
using System.Data;
using System.Net.Mail;
using System.Net.Mime;
using POV.Comun.Service;
using System.Security.Cryptography;
using System.Text;
using POV.Logger.Service;
using System.IO;
using System.Configuration;
using POV.Licencias.Service;
using POV.CentroEducativo.BO;
using POV.Licencias.BO;
namespace POV.Web.PortalSocial.Auth
{
    public partial class RecuperarPassword : System.Web.UI.Page
    {
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private string linkportal;
        #region QueryString
        private string QS_TipoUsuario
        {
            get { return this.Request.QueryString["u"]; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblFechaAnio.Text = DateTime.Now.ToString("yyyy");
                switch (QS_TipoUsuario)
                {
                    case "asp":
                        linkRegresar.HRef += "?u=" + QS_TipoUsuario;
                        linkportal = "?u=" + QS_TipoUsuario;
                        tituloPortal.InnerText = "PORTAL ESTUDIANTES";
                        imgIconoPortal.ImageUrl += "SOV_ELEMENTOS_portalestudiante.png";
                        container_login_.Style["background"] = "url(../images/backgroundloginestudiante.jpg)";
                        btnRecuperar.PostBackUrl = "RecuperarPassword.aspx?enviar=true&u=" + QS_TipoUsuario;
                        break;
                    case "ori":
                        linkRegresar.HRef += "?u=" + QS_TipoUsuario;
                        linkportal = "?u=" + QS_TipoUsuario;
                        tituloPortal.InnerText = "PORTAL ORIENTADOR";
                        imgIconoPortal.ImageUrl += "SOV_ELEMENTOS_portalorientador.png";
                        container_login_.Style["background"] = "url(../images/backgroundloginorientador.jpg)";
                        btnRecuperar.PostBackUrl = "RecuperarPassword.aspx?enviar=true&u=" + QS_TipoUsuario;
                        break;
                    default:
                        Response.Redirect(ConfigurationManager.AppSettings["POVUrlLandingPage"], true);
                        break;
                }
            }
        }


        protected void btnRecuperar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = new Usuario();
                UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                usuario.Email = txtCorreo.Text;
                if (correoValido(usuario, usuarioCtrl))
                {
                    POV.Seguridad.Utils.PasswordProvider passwordProvider = new POV.Seguridad.Utils.PasswordProvider(8);
                    string newPassword = passwordProvider.GetNewPassword();
                    usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));
                    Byte[] bPassword = nuevoPassword(newPassword);
                    ActualizarPassword(usuario, usuarioCtrl, bPassword, newPassword);

                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        private void enviarCorreo(Usuario usuario, string newPassword)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string imgalt = "YOY - SOCIAL";
            const string titulo = "Recuperar contraseña";
            linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalSocial"] + linkportal;
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateRecuperarPassword.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{user}", usuario.NombreUsuario);
            cuerpo = cuerpo.Replace("{password}", newPassword);
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
                correoCtrl.sendMessage(tos, "YOY - Recuperación de contraseña", cuerpo, texto, archivos, copias);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Los datos para acceder al portal fueron enviados a su correo. Revise su bandeja de entrada y/o bandeja de correo no deseado.'); window.location='Login.aspx?u=" + QS_TipoUsuario + "';", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentelo más tarde.');window.location='Login.aspx?u=" + QS_TipoUsuario + "';", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }

        private void ActualizarPassword(Usuario usuario, UsuarioCtrl usuarioCtrl, byte[] bPassword, string newPassword)
        {
            object myFirm = new Object();
            Usuario newUsuario = usuario;
            newUsuario.Password = bPassword;
            try
            {
                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);
                usuarioCtrl.Update(dctx, newUsuario, usuario);
                enviarCorreo(usuario, newPassword);
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                dctx.RollbackTransaction(myFirm);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert(Lo sentimos ocurrio un error, intentelo más tarde);window.location='Login.aspx?u=" + QS_TipoUsuario + "';", true);
            }
            finally
            {
                dctx.CommitTransaction(myFirm);
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }

        }




        private Byte[] nuevoPassword(string newPassword)
        {
            Byte[] pass;

            pass = POV.Seguridad.Utils.EncryptHash.SHA1encrypt(newPassword);
            return pass;
        }

        private bool correoValido(Usuario usuario, UsuarioCtrl usuarioCtrl)
        {
            bool valido = false;
            bool error = false;
            if (usuario.Email.Trim().Length > 100)
                error = true;

            if (error)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Proporciona un correo válido.');", true);
            }
            else
            {
                DataSet dsUsuarioSocial = usuarioCtrl.Retrieve(dctx, usuario);
                if (dsUsuarioSocial.Tables["Usuario"].Rows.Count <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos no se encontró el correo electrónico proporcionado.');", true);
                }
                else
                {

                    LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                    Usuario usuarioencontrado = new Usuario();
                    usuarioencontrado = usuarioCtrl.LastDataRowToUsuario(dsUsuarioSocial);
                    List<LicenciaEscuela> licenciasEscuela = licenciaEscuelaCtrl.RetrieveLicencia(dctx, usuarioencontrado);
                    ALicencia licencia = null;
                    foreach (LicenciaEscuela licenciaEscuela in licenciasEscuela) // se recorre la lista para encontrar las del docente, solo se recorre una vez para alumnos.
                    {
                        //buscamos la licencia
                        licencia = licenciaEscuela.ListaLicencia.Find(item => (bool)item.Activo && item.Tipo != ETipoLicencia.DIRECTOR);
                    }

                    if ((QS_TipoUsuario == "asp" && licencia.Tipo == ETipoLicencia.DOCENTE) || (QS_TipoUsuario == "ori" && licencia.Tipo == ETipoLicencia.ALUMNO)
                        || (licencia.Tipo == ETipoLicencia.TUTOR || licencia.Tipo == ETipoLicencia.UNIVERSIDAD))
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos no se encontró el correo electrónico proporcionado.');", true);
                    else
                        valido = true;
                }
            }
            return valido;
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {

        }
    }
}