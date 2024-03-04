using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using System.Configuration;
using POV.Licencias.BO;
using POV.Seguridad.Utils;
using POV.Web.PortalSocial.AppCode;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using System.Globalization;

namespace POV.Web.PortalSocial.Auth
{
    public partial class AccesoHerramientasDocente : System.Web.UI.Page
    {
        private IRedirector redirector = new Redirector();
        private IUserSession userSession = new UserSession();

        private const string COMANDO_ACCESO_GO_HERRAmiENTAS = "go";

        private const string COMANDO_ACCESO_RETURN_HERRAmiENTAS = "return";

        private Docente Session_Docente
        {
            get { return (Docente)this.Session["DOCENTE_KEY"]; }
            set { this.Session["DOCENTE_KEY"] = value; }
        }

        private string QS_COMANDO_ACCESO
        {
            get { return this.Request.QueryString["cmd"]; }
        }

        #region QueryString
        private string QS_CURP
        {
            get { return this.Request.QueryString["docente"]; }
        }
        private string QS_FechaHora
        {
            get { return this.Request.QueryString["fechahora"]; }
        }
        private string QS_Token
        {
            get { return this.Request.QueryString["token"]; }
        }
        private string QS_Identidad
        {
            get { return this.Request.QueryString["identidad"]; }
        }
        #endregion

        public AccesoHerramientasDocente()
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin()) //si esta logueado
                    if (!userSession.IsAlumno() && EsValidoQueryString()) //si es un docente y el query string es valido
                        if (QS_COMANDO_ACCESO.CompareTo(COMANDO_ACCESO_GO_HERRAmiENTAS) == 0) // si es una redireccion a las herramientas
                            RedirigirHerramientasDocente();
                        else
                            redirector.GoToHomePage(true);
                    else
                        redirector.GoToHomePage(true);
                else if (EsValidoQueryString() && QS_COMANDO_ACCESO.CompareTo(COMANDO_ACCESO_RETURN_HERRAmiENTAS) == 0 && EsValidoToken()) //si no esta logueado, se loguea al sistema
                {
                    GenerarSession(this.Session_Docente);
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        private bool EsValidoQueryString()
        {
            bool valido = true;
            string errores = "";
            if (this.QS_COMANDO_ACCESO == null || this.QS_COMANDO_ACCESO.Trim().Length == 0)
                errores += ",QS_COMANDO";

            if (errores.Length > 0)
                return false;

            if (QS_COMANDO_ACCESO.CompareTo(COMANDO_ACCESO_RETURN_HERRAmiENTAS) == 0)
            {
                if (this.QS_CURP == null || this.QS_CURP.Trim().Length == 0)
                    errores += ",QS_CURP";
                if (this.QS_FechaHora == null || this.QS_FechaHora.Trim().Length == 0)
                    errores += ",QS_FechaHora";
                if (this.QS_Token == null || this.QS_Token.Trim().Length == 0)
                    errores += ",QS_Token";
                if (this.QS_Identidad == null || this.QS_Identidad.Trim().Length == 0)
                    errores += ",QS_Identidad";

            }

            if (errores.Length > 0)
                valido = false;

            return valido;
        }

        /// <summary>
        /// Redirige al portal de herramientas del docente
        /// </summary>
        private void RedirigirHerramientasDocente()
        {
            Docente docente = userSession.CurrentDocente;
            Escuela escuela = userSession.CurrentEscuela;
            CicloEscolar cicloEscolar = userSession.CurrentCicloEscolar;
            Contrato contrato = userSession.Contrato;
            string parametros = GenerarTokenYUrl(docente, escuela, cicloEscolar, contrato, userSession.CurrentUser);
            string urlPortal = @ConfigurationManager.AppSettings["POVUrlHerramientasDocente"];
            userSession.Logout();
            url.Value = urlPortal + parametros;

        }
        /// <summary>
        /// genera un query string con los siguientes elementos: curp docente, escuela id, ciclo escolar id, fecha de solicitud, token de autorizacion
        /// </summary>
        /// <param name="docente"></param>
        /// <param name="escuela"></param>
        /// <param name="cicloEscolar"></param>
        /// <returns>Query string</returns>
        private string GenerarTokenYUrl(Docente docente, Escuela escuela, CicloEscolar cicloEscolar, Contrato contrato, Usuario usr)
        {
            string UrlPortalHerramientas;
            string formatoFecha = "yyyyMMddHHmmss.fff";
            DateTime fecha = System.DateTime.Now;
            string nombre = docente.Nombre;
            nombre = nombre.Trim();
            string apellido = docente.PrimerApellido;
            apellido = apellido.Trim();
            string curp = docente.Curp;
            DateTime fechaNacimiento = (DateTime)docente.FechaNacimiento;
            curp = curp.Trim();
            string token = fechaNacimiento.ToString(formatoFecha) + nombre + apellido + curp + fecha.ToString(formatoFecha);
            byte[] clave = POV.Seguridad.Utils.EncryptHash.SHA1encrypt(token);
            token = POV.Seguridad.Utils.EncryptHash.byteArrayToStringBase64(clave);
            token = System.Web.HttpUtility.UrlEncode(token);
            UrlPortalHerramientas = "?cmd=" + COMANDO_ACCESO_GO_HERRAmiENTAS + "&contrato=" + contrato.ContratoID + "&docente=" + docente.Curp + "&escuela=" + escuela.EscuelaID + "&Identidad=" + usr.UsuarioID + "&ciclo=" + cicloEscolar.CicloEscolarID + "&fechahora=" + fecha.ToString(formatoFecha) + "&token=" + token;
            return UrlPortalHerramientas;
        }


        private void GenerarSession(Docente docente)
        {
            int usuarioId;
            int.TryParse(this.QS_Identidad, out usuarioId);

            UsuarioCtrl usrCtrl = new UsuarioCtrl();
            Usuario usuario = usrCtrl.LastDataRowToUsuario(usrCtrl.Retrieve(ConnectionHelper.Default.Connection, new Usuario() { UsuarioID = usuarioId }));
            usuario = new UsuarioCtrl().RetrieveComplete(ConnectionHelper.Default.Connection, usuario);
            AccountService accountSrv = new AccountService();
            accountSrv.Login(ConnectionHelper.Default.Connection, usuario.NombreUsuario, String.Empty, usuario.Password);

        }
        public bool EsValidoToken()
        {
            bool valido = false;

            Docente docente = new Docente();
            docente.Curp = this.QS_CURP;
            docente.Estatus = true;
            DocenteCtrl docenteCtrl = new DocenteCtrl();
            var docentes = docenteCtrl.Retrieve(ConnectionHelper.Default.Connection, docente);
            bool existeDocente = docentes.Tables[0].Rows.Count > 0;

            if (existeDocente)
            {
                docente = docenteCtrl.LastDataRowToDocente(docentes);

                string cadenaToken = docente.FechaNacimiento.Value.ToString("yyyyMMddHHmmss.fff") + docente.Nombre.Trim() + docente.PrimerApellido.Trim() + docente.Curp.Trim() + this.QS_FechaHora;
                byte[] bytes = EncryptHash.SHA1encrypt(cadenaToken);
                string token = EncryptHash.byteArrayToStringBase64(bytes);
                //string tokenEncode = System.Web.HttpUtility.UrlEncodeUnicode(QS_Token);

                string qsToken = this.QS_Token;
                if (qsToken.Contains(" "))
                    qsToken = qsToken.Replace(" ", "+");

                DateTime fechaHoraSolicitud = DateTime.ParseExact(this.QS_FechaHora, "yyyyMMddHHmmss.fff", CultureInfo.InvariantCulture);

                bool vigente = DateTime.Now.Subtract(fechaHoraSolicitud).TotalSeconds <= 5000;

                valido = qsToken.CompareTo(token) == 0 && vigente;

                if (valido)
                    this.Session_Docente = docente;
                else
                    this.Session_Docente = null;
            }

            return valido;
        }

    }
}