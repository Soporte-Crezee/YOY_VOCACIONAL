using POV.Administracion.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.Service;
using POV.Logger.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Utils;
using POV.Web.Administracion.AppCode.Page;
using POV.Web.Administracion.Helper;
using Framework.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.Administracion.Docentes
{
    public partial class NuevoDocente : PageBase
    {

        private EscuelaCtrl escuelaCtrl;
        private CatalogoDocentesCtrl catalogoDocentesCtrl;
        private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;
        private DocenteCtrl docenteCtrl;

        #region *** propiedades de clase ***
        public Docente LastObject
        {
            get { return Session["LastDocente"] != null ? (Docente)Session["LastDocente"] : null; }
            set { Session["LastDocente"] = value; }
        }
        #endregion

        public NuevoDocente()
        {
            grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
            escuelaCtrl = new EscuelaCtrl();
            catalogoDocentesCtrl = new CatalogoDocentesCtrl();
            docenteCtrl = new DocenteCtrl();
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalSocial"]))
                catalogoDocentesCtrl.UrlPortalSocial =
                    System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalSocial"];

            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["POVUrlImgNatware"]))
                catalogoDocentesCtrl.UrlImgNatware =
                    System.Configuration.ConfigurationManager.AppSettings["POVUrlImgNatware"];

        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession == null || !userSession.IsLogin())
                    redirector.GoToLoginPage(true);

                if (userSession.CurrentEscuela == null || userSession.CurrentCicloEscolar == null)
                    redirector.GoToError(true);
            }
            catch (Exception ex)
            {

                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DoInsert();
            }
            catch (Exception ex)
            {

                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region *** validaciones ***
        private void ValidateData()
        {
            string sError = string.Empty;

            //Valores Requeridos.
            if (txtCurp.Text.Trim().Length <= 0)
                sError += " ,CURP";
            if (txtNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length <= 0)
                sError += " ,Primer Apellido";
            if (txtCorreo.Text.Trim().Length <= 0)
                sError += " ,Correo";
            if (CbxSexo.SelectedIndex == -1 || CbxSexo.SelectedValue.Length <= 0)
                sError += " ,Sexo";
            if (this.txtCedula.Text.Trim().Length <= 0)
                sError += " ,Cedula";
            if (ddlNivelEstudio.SelectedValue.Length <= 0)
                sError += " ,Nivel estudio";
            if (this.txtTitulo.Text.Trim().Length <= 0)
                sError += " ,titulo";
            if (this.txtEspecialidades.Text.Trim().Length <= 0)
                sError += " ,Especialidades";
            if (this.txtExperiencia.Text.Trim().Length <= 0)
                sError += " ,Experiencia";
            if (this.txtCursos.Text.Trim().Length <= 0)
                sError += " ,Cursos";
            if (this.txtSkype.Text.Trim().Length <= 0)
                sError += " ,Usuario skype";
            if (this.txtNombreUsuario.Text.Trim().Length <= 0)
                sError += " ,Nombre usuario";


            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son requeridos:{0}", sError));
            }


            //Valores con Incorrectos.
            if (txtCurp.Text.Trim().Length > 18)
                sError += " ,CURP";
            if (txtNombre.Text.Trim().Length > 80)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length > 50)
                sError += " ,Primer Apellido";
            if (txtCorreo.Text.Trim().Length > 100)
                sError += " ,Correo Electrónico";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos :{0}", sError));
            }

            if (!ValidateCurpRegex(txtCurp.Text.Trim()))
                sError += " ,Curp";

            if (!ValidateEmailRegex(txtCorreo.Text.Trim()))
                sError += " ,Correo Electrónico";

            DateTime fn;
            if (!DateTime.TryParseExact(txtFechaNacimiento.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fn))
                sError += " ,Fecha Nacimiento";

				if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros tienen un formato no válido :{0}", sError));
            }


        }
        private bool ValidateEmailRegex(string email)
        {
            string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reLenient = new Regex(patternLenient);
            bool match = reLenient.IsMatch(email);
            return match;


        }

        private bool ValidateCurpRegex(string curp)
        {
            curp = curp.ToUpper();
            string patternLenient = @"[A-Z]{1}[AEIOUX]{1}[A-Z]{2}[0-9]{2}"
                + "(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])" +
                "[HM]{1}" +
                "(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)" +
                "[B-DF-HJ-NP-TV-Z]{3}" +
                "[0-9A-Z]{1}[0-9]{1}$";
            Regex reLenient = new Regex(patternLenient);

            bool match = reLenient.IsMatch(curp);
            return match;
        }
        #endregion

        #region *** UserInterface to Data ***
        private Docente UserInterfaceToData()
        {

            Docente docente = new Docente();
            docente.Nombre = txtNombre.Text.Trim();
            docente.PrimerApellido = txtPrimerApellido.Text.Trim();
            docente.SegundoApellido = txtSegundoApellido.Text.Trim();
            docente.FechaNacimiento = DateTime.ParseExact(txtFechaNacimiento.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            docente.Cedula = txtCedula.Text.Trim();
            docente.NivelEstudio = ddlNivelEstudio.SelectedItem.Text;
            docente.Titulo = txtTitulo.Text.Trim();
            docente.Especialidades = txtEspecialidades.Text.Trim();
            docente.Experiencia = txtExperiencia.Text.Trim();
            docente.Cursos = txtCursos.Text.Trim();
            docente.UsuarioSkype = txtSkype.Text.Trim();
            docente.EsPremium = (bool)cbEsPremium.Checked;

            docente.NombreUsuario = txtNombreUsuario.Text.Trim();

            docente.Correo = txtCorreo.Text.Trim();
            docente.Sexo = bool.Parse(CbxSexo.SelectedValue);
            docente.Curp = txtCurp.Text.Trim();
            return docente;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoInsert()
        {
            try
            {
                try
                {
                    ValidateData();
                }
                catch (Exception ex)
                {
                    this.ShowMessage(ex.Message, MessageType.Error);
                    return;
                }
                LastObject = UserInterfaceToData();
                new Seguridad.Utils.PasswordProvider().GetNewPassword();
                string passwordTemp = new PasswordProvider(8).GetNewPassword();
                Usuario usrCorrecto = catalogoDocentesCtrl.InsertDocenteEscuela(ConnectionHlp.Default.Connection, userSession.CurrentEscuela, userSession.CurrentCicloEscolar, LastObject, passwordTemp);
                LastObject = docenteCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, LastObject);
                AsignacionMateriaGrupo asignacionMateriaGrupo = new AsignacionMateriaGrupo() { Docente = LastObject, Materia = new Materia() { MateriaID = 18 } };
                DoSaveUserSocialHub(asignacionMateriaGrupo);
                EnviarCorreo(usrCorrecto, LastObject, passwordTemp);
                LastObject = null;
                redirector.GoToConsultarDocente(false);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        private void DoSaveUserSocialHub(AsignacionMateriaGrupo asignacion)
        {
            GrupoCicloEscolar grupoCicloEscolar = new GrupoCicloEscolar();
            
            grupoCicloEscolar.CicloEscolar = new CicloEscolar() { CicloEscolarID = 3 };
            grupoCicloEscolar.GrupoSocialID = 1;
            grupoCicloEscolar.GrupoCicloEscolarID = new Guid("39f5a1b8-20f0-437c-aa13-78f5974d881a");
            grupoCicloEscolar.Grupo = new Grupo() { GrupoID = new Guid("{4537c8f9-089e-42d4-bfd5-a54ee0d99d75}"), Grado = 1 };
            grupoCicloEscolar.Escuela = new Escuela { EscuelaID = userSession.CurrentEscuela.EscuelaID };

            AsignacionMateriaGrupo asigmatGrupo = asignacion;
            if (asigmatGrupo == null || asigmatGrupo.Docente == null || asigmatGrupo.Materia == null)
                return;
            if (asigmatGrupo.Materia.MateriaID == null || asigmatGrupo.Docente.DocenteID == null || asigmatGrupo.Materia.MateriaID <= 0 || asigmatGrupo.Docente.DocenteID <= 0)
                return;
            try
            {
                (new AsignarDocenteGrupoCicloEscolarCtrl()).InsertAsignacionDocenteGrupoCicloEscolar(ConnectionHlp.Default.Connection, asigmatGrupo.Docente, asigmatGrupo.Materia, grupoCicloEscolar, null, (long)userSession.CurrentUser.UniversidadId);
           }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }



        }
        private void EnviarCorreo(Usuario usuario, Docente docente, string pws)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string imgalt = "YOY - Orientacion vocacional para estudiantes";
            const string titulo = "Registro exitoso";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalSocial"];
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateNewOrientador.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{user}", usuario.NombreUsuario);
            cuerpo = cuerpo.Replace("{password}", pws);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);

            List<string> tos = new List<string>();
            tos.Add(usuario.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Registro exitoso", cuerpo, texto, archivos, copias);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('¡Registro exitoso! Revisa tu bandeja de entrada y/o bandeja de correo no deseado, serás redirigido al portal.'); window.location='" + linkportal + "';", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentalo mas tarde.');window.location='" + ConfigurationManager.AppSettings["POVUrlPortalSocial"] + "';", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }

        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegiosDirector.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESODOCENTES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAINSERTARDOCENTES) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!creacion)
                redirector.GoToHomePage(true);
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
    }
}