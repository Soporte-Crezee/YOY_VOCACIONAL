using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;
using POV.Web.PortalTutor.Helper;
using POV.Comun.Service;
using POV.Logger.Service;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using POV.CentroEducativo.Services;
using POV.Seguridad.Utils;
using GP.SocialEngine.BO;
using POV.Licencias.Service;
using POV.Seguridad.Service;
using POV.Licencias.BO;
using POV.Administracion.Service;

namespace POV.Web.PortalTutor.Tutores
{
    public partial class NuevoTutor : System.Web.UI.Page 
    {        
        #region *** propiedades de clase ***
        private TutorCtrl tutorCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;
        private UsuarioCtrl usuarioCtrl;
        private CatalogoTutoresCtrl catalogoTutoresCtrl;
        #endregion

        public NuevoTutor()
        {
            tutorCtrl = new TutorCtrl(null);
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            usuarioCtrl = new UsuarioCtrl();
            catalogoTutoresCtrl = new CatalogoTutoresCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DoInsert();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('" + ex.Message + "');", true);
            }
        }
        #endregion

        #region *** validaciones ***
       
        private void TutorValidateData()
        {
            //Campos Requeridos
            string sError = string.Empty;

            if (txtNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length <= 0)
                sError += " ,Primer Apellido";
            if (CbSexo.SelectedIndex == -1 || CbSexo.SelectedValue.Length <= 0)
                sError += " ,Sexo";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son requeridos: {0}", sError));
            }
            if (txtNombre.Text.Trim().Length > 30)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length > 20)
                sError += " ,Primer Apellido";
            if (txtSegundoApellido.Text.Trim().Length > 20)
                sError += " ,Segundo Apellido";                  

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos: {0}", sError));
            }
        }
        private void UsuarioValidateData()
        {
            string sError = string.Empty;
            //Campos Validos 
            if (txtNombreUsuario.Text.Trim().Length <= 0)
                sError += " ,Usuario";
            if (txtPassword.Text.Trim().Length <= 0)
                sError += " ,Contraseña";
            if (txtCorreoElectronico.Text.Trim().Length > 50)
                sError += " ,Correo";
            if (txtNombreUsuario.Text.Trim().Length > 50)
                sError += " ,Nombre de Usuario";
            if (txtPassword.Text.Trim().Length > 50)
                sError += " ,Contraseña";
            if (txtConfirmarPassword.Text.Trim() != txtPassword.Text.Trim())
                sError += " ,Confirmar Contraseña";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos: {0}", sError));
            }
            //Formato Incorrecto
            if (txtCorreoElectronico.Text.Trim().Length > 0 && !ValidateEmailRegex(txtCorreoElectronico.Text.Trim()))
                sError += " ,Correo";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros tienen un formato no valido: {0}", sError));
            }
        }
        private bool ValidateEmailRegex(string email)
        {
            string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reLenient = new Regex(patternLenient);

            bool match = reLenient.IsMatch(email);
            return match;
        }
        #endregion

        #region *** UserInterface to Data ***
        private Tutor TutorUserInterfaceToData()
        {
            Tutor tutor = new Tutor();

            //DateTime dateval;
            bool boolval;
            tutor.Nombre = !string.IsNullOrEmpty(txtNombre.Text.Trim()) ? txtNombre.Text.Trim() : string.Empty;
            tutor.PrimerApellido = !string.IsNullOrEmpty(txtPrimerApellido.Text.Trim()) ? txtPrimerApellido.Text.Trim() : string.Empty;
            tutor.SegundoApellido = !string.IsNullOrEmpty(txtSegundoApellido.Text.Trim()) ? txtSegundoApellido.Text.Trim() : string.Empty;
          
            if (CbSexo.SelectedIndex != -1 && !string.IsNullOrEmpty(CbSexo.SelectedValue))
                if (bool.TryParse(CbSexo.SelectedValue, out boolval)) tutor.Sexo = boolval;

            tutor.CorreoElectronico = txtCorreoElectronico.Text.Trim();

            return tutor;

        }
        private Usuario UsuarioUserInterfaceToData()
        {
            Usuario usuario = new Usuario();
            string password = !string.IsNullOrEmpty(txtPassword.Text) ? txtPassword.Text.Trim() : string.Empty;                
            usuario.Email = !string.IsNullOrEmpty(txtCorreoElectronico.Text) ? txtCorreoElectronico.Text.Trim() : string.Empty;
            usuario.NombreUsuario = !string.IsNullOrEmpty(txtNombreUsuario.Text) ? txtNombreUsuario.Text.Trim() : string.Empty;
            usuario.Password = EncryptHash.SHA1encrypt(password);           

            return usuario;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoInsert()
        {
            try
            {
                try
                {
                    TutorValidateData();
                    UsuarioValidateData();
                }
                catch (Exception ex)
                {                    
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('" + ex.Message + "');", true);
                    return;
                }

                Tutor tutor = TutorUserInterfaceToData();
                Usuario usuario = UsuarioUserInterfaceToData();

                var tutorRegistroCorrecto = catalogoTutoresCtrl.InsertTutor(dctx, tutor, usuario);

                if (tutorRegistroCorrecto)
                {
                    enviarCorreo(usuario, tutor);                   
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Ocurrió un error inesperado al registrar al usuario, inténtelo más tarde');", true);    
                }  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region *****Message  Showing*****
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="messageType">Tipo de mensaje</param>
        private void ShowMessage(string message, MessageType messageType)
        {
            string type = string.Empty;

            switch (messageType)
            {
                case MessageType.Error:
                    type = "1";
                    break;
                case MessageType.Information:
                    type = "3";
                    break;
                case MessageType.Warning:
                    type = "2";
                    break;
            }

            ShowMessage(message, type);
        }
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="typeNotification">1: Error, 2: Advertencia, 3: Información</param>
        private void ShowMessage(string message, string typeNotification)
        {
            //Se ubican los controles que manejan el desplegado de error/advertencia/información
            if (Page.Master == null) return;
            Control m = Page.Master.FindControl("hdnLastMessage");
            Control t = Page.Master.FindControl("hdnShowMessage");

            if (m == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnLastMessage' en la MasterPage.\nEl error original es:\n" + message);
            if (t == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnShowMessage' en la MasterPage.\nEl error original es:\n" + message);

            if (m.GetType() != typeof(HiddenField) || t.GetType() != typeof(HiddenField))
                throw new Exception("No se pudo desplegar correctamente el error.\nAlguno de los controles de la MasterPage para el manejo de errores no es HiddenField.\nEl error original es:\n" + message);

            //Si el HiddenField del mensaje de error ya tiene un mensaje guardado, se da un 'enter' y se concatena el nuevo mensaje (errores acumulados)
            //En caso contrario, se pone el encabezado y se concatena el nuevo mensaje
            if (((HiddenField)m).Value != null && ((HiddenField)m).Value.Trim().CompareTo("") != 0)
                ((HiddenField)m).Value += "<br />";


            ((HiddenField)m).Value += message.Replace("\n", "<br />");
            ((HiddenField)t).Value = typeNotification;
        }
        #endregion   
     
        //Enviar Correo de registro exitoso
        private void enviarCorreo(Usuario usuario, Tutor tutor)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
                string urlimg = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgEmail"];
                string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
                const string altimg = "YOY - Email";
                const string titulo = "Registro exitoso";            
                string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
                string linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalTutor"];
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateNewUserTutor.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{altimage}", altimg);
            cuerpo = cuerpo.Replace("{urlimage}",urlimg);
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);

            List<string> tos = new List<string>();
            tos.Add(usuario.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Registro exitoso", cuerpo, texto, archivos, copias);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('¡Registro exitoso! Revise su bandeja de entrada y/o bandeja de correo no deseado, será redirigido al portal.'); window.location='"+linkportal+"';", true);              
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentelo mas tarde.');window.location='" + ConfigurationManager.AppSettings["POVUrlPortalTutor"] + "';", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }       

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(ConfigurationManager.AppSettings["POVUrlPortalSocial"]);
        }

        protected void btnValidarUsuario_Click(object sender, EventArgs e)
        {
        }
    }
}