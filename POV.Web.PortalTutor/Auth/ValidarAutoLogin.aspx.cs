using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.Service;
using System.Globalization;
using POV.CentroEducativo.BO;
using POV.Seguridad.Utils;
using POV.Web.PortalTutor.Helper;
using POV.Core.PadreTutor.Implements;
using POV.Core.PadreTutor.Interfaces;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.CentroEducativo.Services;

namespace POV.Web.PortalTutor.Auth
{
    public partial class ValidarAutoLogin : System.Web.UI.Page
    {
        #region QueryString
        private string QS_CODIGO
        {
            get { return this.Request.QueryString["tutor"]; }
        }
        private string QS_FechaHora
        {
            get { return this.Request.QueryString["fechahora"]; }
        }
        private string QS_Token
        {
            get { return this.Request.QueryString["token"]; }
        }
        #endregion
        #region Session
        private Tutor Session_Tutor
        {
            get { return (Tutor)this.Session["TUTOR_KEY"]; }
            set { this.Session["TUTOR_KEY"] = value; }
        }
        #endregion

        private IRedirector redirector;

        protected void Page_Load(object sender, EventArgs e)
        {
            redirector = new Redirector();
            if (!this.IsPostBack)
            {
                string errores = "";

                if (!this.EsValidoQueryString(out errores))
                {
                    redirector.GoToLoginPage(true);
                }
                if (!this.EsValidoToken())
                {
                    redirector.GoToLoginPage(true);
                }

                GenerarSession(this.Session_Tutor);
            }
        }

        private void GenerarSession(Tutor tutor) 
        {
            LicenciaEscuelaCtrl licenciaCtrl = new LicenciaEscuelaCtrl();
            Usuario usuario = licenciaCtrl.RetrieveUsuarioTutor(ConnectionHlp.Default.Connection,tutor);
            usuario = new UsuarioCtrl().RetrieveComplete(ConnectionHlp.Default.Connection, usuario);
            AccountService accountSrv = new AccountService();
            accountSrv.Login(ConnectionHlp.Default.Connection, usuario.NombreUsuario, string.Empty, usuario.Password);

        }

        public bool EsValidoToken()
        {
            bool valido = false;

            Tutor tutor = new Tutor();
            tutor.Codigo = this.QS_CODIGO;
            tutor.Estatus = true;
            TutorCtrl tutorCtrl = new TutorCtrl(null);
            var tutors = tutorCtrl.Retrieve(tutor, false).ToList();
            bool existeTutor = (tutors.Count > 0)?true:false;

            if (existeTutor)
            {
                tutor= tutors[0];

                string cadenaToken = tutor.FechaNacimiento.Value.ToString("yyyyMMddHHmmss.fff") + tutor.Nombre.Trim() + tutor.PrimerApellido.Trim() + tutor.Codigo.Trim() + this.QS_FechaHora;
                byte[] bytes = EncryptHash.SHA1encrypt(cadenaToken);
                string token = EncryptHash.byteArrayToStringBase64(bytes);

                string qsToken = this.QS_Token;
                if (qsToken.Contains(" "))
                    qsToken = qsToken.Replace(" ", "+");

                DateTime fechaHoraSolicitud = DateTime.ParseExact(this.QS_FechaHora, "yyyyMMddHHmmss.fff", CultureInfo.InvariantCulture);
              
                bool vigente = DateTime.Now.Subtract(fechaHoraSolicitud).TotalSeconds <= 5000;

                valido = qsToken.CompareTo(token) == 0 && vigente;

                if (valido)
                    this.Session_Tutor = tutor;
                else
                    this.Session_Tutor = null;
            }

            return valido;
        }

        public bool EsValidoQueryString(out string errores)
        {
            bool valido = true; errores = "";

            if (this.QS_CODIGO == null || this.QS_CODIGO.Trim().Length == 0)
                errores += ",QS_CODIGO";
            if (this.QS_FechaHora == null || this.QS_FechaHora.Trim().Length == 0)
                errores += ",QS_FechaHora";
            if (this.QS_Token == null || this.QS_Token.Trim().Length == 0)
                errores += ",QS_Token";

            if (errores.Length > 0)
                valido = false;

            return valido;
        }
    }
}