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
using POV.Web.PortalSocial.AppCode;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.CentroEducativo.Services;

namespace POV.Web.PortalSocial.Auth
{
    public partial class ValidarAutoLogin : System.Web.UI.Page
    {
        #region QueryString
        private string QS_CURP
        {
            get { return this.Request.QueryString["alumno"]; }
        }
        private string QS_FechaHora
        {
            get { return this.Request.QueryString["fechahora"]; }
        }
        private string QS_Token
        {
            get { return this.Request.QueryString["token"]; }
        }
        private string QS_Portal
        {
            get { return this.Request.QueryString["portal"]; }
        }

        private string QS_Bateria
        {
            get { return this.Request.QueryString["bateria"]; }
        }
        #endregion
        #region Session
        private Alumno Session_Alumno
        {
            get { return (Alumno)this.Session["ALUMNO_KEY"]; }
            set { this.Session["ALUMNO_KEY"] = value; }
        }
        #endregion

        private IRedirector redirector;
        private IUserSession userSession;

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

                GenerarSession(this.Session_Alumno);
            }
        }

        private void GenerarSession(Alumno alumno) 
        {
            userSession = new UserSession();
            LicenciaEscuelaCtrl licenciaCtrl = new LicenciaEscuelaCtrl();
            Usuario usuario = licenciaCtrl.RetrieveUsuario(ConnectionHelper.Default.Connection, alumno);
            usuario = new UsuarioCtrl().RetrieveComplete(ConnectionHelper.Default.Connection, usuario);
            AccountService accountSrv = new AccountService();
            //accountSrv.Login(ConnectionHelper.Default.Connection, usuario.NombreUsuario, String.Empty, usuario.Password);
            // validar si viene de pruebas
            if (!String.IsNullOrEmpty(Request.QueryString["portal"]))
                accountSrv.LoginPruebas(ConnectionHelper.Default.Connection, usuario.NombreUsuario, String.Empty, usuario.Password, QS_Portal);
            else
                accountSrv.Login(ConnectionHelper.Default.Connection, usuario.NombreUsuario, String.Empty, usuario.Password);

            UniversidadCtrl universidadCtrl = new UniversidadCtrl(null);
            Session_Alumno.Universidades = new List<Universidad>();
            Session_Alumno.Universidades = universidadCtrl.RetrieveUniversidadByAlumno(Session_Alumno, false).ToList();

            if (Session_Alumno.NivelEscolar != ENivelEscolar.Superior)
            {
                TutorAlumnoCtrl tutorAlumnoCtrl = new TutorAlumnoCtrl(null);
                var tutores = tutorAlumnoCtrl.Retrieve(new TutorAlumno { AlumnoID = alumno.AlumnoID }, false).ToList();
                LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                bool acpTerminos = false;
                foreach (TutorAlumno tut in tutores)
                {
                    Usuario usr = licenciaEscuelaCtrl.RetrieveUsuarioTutor(ConnectionHelper.Default.Connection, new Tutor { TutorID = tut.TutorID });
                    if (usr.AceptoTerminos == true)
                    {
                        acpTerminos = true;

                    }
                }
                if (!acpTerminos || Session_Alumno.EstatusPago != EEstadoPago.PAGADO)
                {
                    userSession.Logout();
                    redirector.GoToLoginPageAspirante(true);
                }
            }
            else 
            {

                if (Session_Alumno.EstatusPago != EEstadoPago.PAGADO)
                    redirector.GoToActivarUsuario(true);
            }
        }

        public bool EsValidoToken()
        {
            bool valido = false;

            Alumno alumno = new Alumno();
            alumno.Curp = this.QS_CURP.Trim();
            alumno.Estatus = true;
            AlumnoCtrl alumnoCtrl = new AlumnoCtrl();
            var alumnos = alumnoCtrl.Retrieve(ConnectionHelper.Default.Connection, alumno);
            bool existeAlumno = alumnos.Tables[0].Rows.Count > 0;

            if (existeAlumno)
            {
                alumno= alumnoCtrl.LastDataRowToAlumno(alumnos);

                string cadenaToken = alumno.FechaNacimiento.Value.ToString("yyyyMMddHHmmss.fff") + alumno.Nombre.Trim() + alumno.PrimerApellido.Trim() + alumno.Curp.Trim() + this.QS_FechaHora;
                byte[] bytes = EncryptHash.SHA1encrypt(cadenaToken);
                string token = EncryptHash.byteArrayToStringBase64(bytes);

                string qsToken = this.QS_Token;
                if (qsToken.Contains(" "))
                    qsToken = qsToken.Replace(" ", "+");

                DateTime fechaHoraSolicitud = DateTime.ParseExact(this.QS_FechaHora, "yyyyMMddHHmmss.fff", CultureInfo.InvariantCulture);
              
                bool vigente = DateTime.Now.Subtract(fechaHoraSolicitud).TotalSeconds <= 5000;

                valido = qsToken.CompareTo(token) == 0 && vigente;

                if (valido)
                    this.Session_Alumno = alumno;
                else                
                    this.Session_Alumno = null;
            }

            return valido;
        }

        public bool EsValidoQueryString(out string errores)
        {
            bool valido = true; errores = "";

            if (this.QS_CURP == null || this.QS_CURP.Trim().Length == 0)
                errores += ",QS_CURP";
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