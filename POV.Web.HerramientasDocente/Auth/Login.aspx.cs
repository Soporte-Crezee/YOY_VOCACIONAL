using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using Framework.Base.Exceptions;
using POV.Logger.Service;
using Framework.Base.DataAccess;
using POV.Seguridad.Utils;
using System.Globalization;
using POV.Web.Helper;
using POV.Core.HerramientasDocente.Interfaces;
using POV.Core.HerramientasDocente.Implement;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Licencias.Service;
using POV.Licencias.BO;

namespace POV.Web.HerramientasDocente.Auth
{
    public partial class Login : System.Web.UI.Page
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private IUserSession userSession = new UserSession();
        private IRedirector redirector = new Redirector();

        #region QueryString
        private string QS_CURP
        {
            get { return this.Request.QueryString["docente"]; }
        }
        private string QS_FechaHora
        {
            get { return this.Request.QueryString["fechahora"]; }
        }
        private string QS_EscuelaID
        {
            get { return this.Request.QueryString["escuela"]; }
        }
        private string QS_CicloEscolarID
        {
            get { return this.Request.QueryString["ciclo"]; }
        }
        private string QS_ContratoID
        {
            get { return this.Request.QueryString["contrato"]; }
        }
        private string QS_Token
        {
            get { return this.Request.QueryString["token"]; }
        }
        private string QS_COMANDO
        {
            get { return this.Request.QueryString["cmd"]; }
        }
        private string QS_Identidad
        {
            get { return this.Request.QueryString["Identidad"]; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            Response.Write("<script>alert('Pagina de acceso a herramientas docentes');</script>");

            string errores = "";

            if (!this.EsValidoQueryString(out errores))
            {
                this.RedirigirRedSocial("Información incorrecta.");
                return;
            }
            bool accesoValido = false;
            try
            {
                accesoValido = this.EsValidoToken();
                if (!accesoValido)
                {
                    this.RedirigirRedSocial("Información incorrecta.");
                    return;
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                this.RedirigirRedSocial("No es posible validar la información del Alumno, intentalo nuevamente.");
                return;
            }


            if (accesoValido)
            {
                userSession.LoggedIn = true;
                redirector.GoToHomePage(true);
            }
        }

        /// <summary>
        /// Valida que el token recibido en la página es correcto o no
        /// </summary>
        /// <returns>Una bandera que indica si el token es válido o no</returns>
        public bool EsValidoToken()
        {
            bool valido = false;
            #region validacion de identificadores
            int escuelaID;
            if (!int.TryParse(this.QS_EscuelaID, out escuelaID))
                return false;

            int cicloEscolarID;
            if (!int.TryParse(this.QS_CicloEscolarID, out cicloEscolarID))
                return false;

            int contratoId;
            if (!int.TryParse(this.QS_ContratoID, out contratoId))
                return false;

            int usuarioId;
            if (!int.TryParse(this.QS_Identidad, out usuarioId))
                return false;
            #endregion

            Docente docente = new Docente();
            docente.Curp = this.QS_CURP;
            docente.Estatus = true;

            DocenteCtrl docentesCtrl = new DocenteCtrl();
            DataSet docentes = docentesCtrl.Retrieve(dctx, docente);
            bool existeDocente = docentes.Tables[0].Rows.Count > 0;

            EscuelaCtrl escuelaCtrl = new EscuelaCtrl();
            DataSet escuelas = escuelaCtrl.Retrieve(dctx, new Escuela { EscuelaID = escuelaID });
            existeDocente = existeDocente && escuelas.Tables[0].Rows.Count > 0;

            CicloEscolarCtrl cicloEscolarCtrl = new CicloEscolarCtrl();
            DataSet ciclos = cicloEscolarCtrl.Retrieve(dctx, new CicloEscolar { CicloEscolarID = cicloEscolarID });
            existeDocente = existeDocente && ciclos.Tables[0].Rows.Count > 0;

            ContratoCtrl contratoCtrl = new ContratoCtrl();
            Contrato contrato = contratoCtrl.RetrieveComplete(dctx, new Contrato { ContratoID = contratoId });
            existeDocente = existeDocente && contrato != null && contrato.ContratoID != null;

            if (existeDocente)
            {
                this.userSession.CurrentDocente = docentesCtrl.LastDataRowToDocente(docentes);
                this.userSession.CurrentCicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(ciclos);
                this.userSession.CurrentEscuela = escuelaCtrl.LastDataRowToEscuela(escuelas);

                Usuario usuario = new Usuario();

                UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario() { UsuarioID = usuarioId }));
                this.userSession.CurrentUser = usuario;
                this.userSession.Contrato = contrato;

                //TODO seguir con probar la comunicación SOCIAL - HERRAMIENTAS

                string cadenaToken = this.userSession.CurrentDocente.FechaNacimiento.Value.ToString("yyyyMMddHHmmss.fff") + this.userSession.CurrentDocente.Nombre.Trim() + this.userSession.CurrentDocente.PrimerApellido.Trim() + this.userSession.CurrentDocente.Curp.Trim() + this.QS_FechaHora;
                byte[] bytes = EncryptHash.SHA1encrypt(cadenaToken);
                string token = EncryptHash.byteArrayToStringBase64(bytes);
                string tokenEncode = Server.UrlEncode(token);

                DateTime fechaHoraSolicitud = DateTime.ParseExact(this.QS_FechaHora, "yyyyMMddHHmmss.fff", CultureInfo.InvariantCulture);
                bool vigente = DateTime.Now.Subtract(fechaHoraSolicitud).TotalSeconds <= 5000;

                valido = this.QS_Token.CompareTo(token) == 0 && vigente;
            }

            return valido;
        }

        /// <summary>
        /// Verifica que la url recibida en la página es correcta
        /// </summary>
        /// <param name="errores">La cadena que sirve de parámetro de entrada, para validar</param>
        /// <returns>Si es válido o no la url</returns>
        public bool EsValidoQueryString(out string errores)
        {
            bool valido = true; errores = "";

            if (this.QS_CURP == null || this.QS_CURP.Trim().Length == 0)
                errores += ",QS_CURP";
            if (this.QS_FechaHora == null || this.QS_FechaHora.Trim().Length == 0)
                errores += ",QS_FechaHora";
            if (this.QS_Token == null || this.QS_Token.Trim().Length == 0)
                errores += ",QS_Token";
            if (this.QS_EscuelaID == null || this.QS_EscuelaID.Trim().Length == 0)
                errores += ",QS_EscuelaID";
            if (this.QS_CicloEscolarID == null || this.QS_CicloEscolarID.Trim().Length == 0)
                errores += ",QS_CicloEscolarID";
            if (this.QS_ContratoID == null || this.QS_ContratoID.Trim().Length == 0)
                errores += ",QS_ContratoID";
            if (this.QS_COMANDO == null || this.QS_COMANDO.Trim().Length == 0)
                errores += ",QS_COMANDO";
            if (this.QS_Identidad == null || this.QS_Identidad.Trim().Length == 0)
                errores += ",QS_Identidad";

            if (errores.Length > 0)
                valido = false;

            return valido;
        }

        public void RedirigirRedSocial(string message)
        {
            this.LimpiarSession();

            Response.Redirect("RegresarPortalSocial.aspx?cmd=return", true);
        }

        private void LimpiarSession()
        {
            this.userSession.CurrentDocente = null;
            this.userSession.CurrentCicloEscolar = null;
            this.userSession.LoggedIn = false;
        }
    }
}