using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.Services;
using POV.Core.PadreTutor.Implements;
using POV.Core.PadreTutor.Interfaces;
using POV.Logger.Service;
using POV.Seguridad.Utils;
using POV.Web.Administracion.Helper;
using Framework.Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.Administracion.Auth
{
    public partial class AccesoPaquetesPremium : System.Web.UI.Page
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private IRedirector redirector = new Redirector();

        private Tutor tutor;
        private Alumno alumno;

        private Alumno Session_Alumno
        {
            get { return (Alumno)this.Session["ALUMNO_KEY"]; }
            set { this.Session["ALUMNO_KEY"] = value; }
        }

        private Tutor Session_Tutor
        {
            get { return (Tutor)this.Session["TUTOR_KEY"]; }
            set { this.Session["TUTOR_KEY"] = value; }
        }

        #region QueryString
        private string QS_AUT
        {
            get { return this.Request.QueryString["aut"]; }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            tutor = new Tutor();
            alumno = new Alumno();
            string errores = "";

            if (!this.EsValidoQueryString(out errores))
            {
                this.RedirigirLandingPage();

            }
            bool accesoValido = false;
            try
            {
                accesoValido = this.EsValidoToken();
                if (!accesoValido)
                {
                    if (this.QS_AUT != null && this.QS_AUT.Split('-')[0].ToString().Trim() == "t")
                        this.RedirigirLoginTutor();
                    else
                        if (this.QS_AUT != null && this.QS_AUT.Split('-')[0].ToString().Trim() == "a")
                            this.RedirigirLoginAspirante();
                        else
                            this.RedirigirLandingPage();

                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                return;
            }

            if (accesoValido)
            {
                if (tutor.TutorID != null)
                    Session_Tutor = tutor;
                if(alumno.AlumnoID!=null)
                    Session_Alumno=alumno;
                redirector.GoToPaquetes(true);
            }
        }

        /// <summary>
        /// Verifica que la url recibida en la página es correcta
        /// </summary>
        /// <param name="errores">La cadena que sirve de parámetro de entrada, para validar</param>
        /// <returns>Si es válido o no la url</returns>
        public bool EsValidoQueryString(out string errores)
        {
            bool valido = true; errores = "";

            if (this.QS_AUT == null || this.QS_AUT.Trim().Length == 0)
                errores += ",QS_AUT";
            if (this.QS_FechaHora == null || this.QS_FechaHora.Trim().Length == 0)
                errores += ",QS_FechaHora";
            if (this.QS_Token == null || this.QS_Token.Trim().Length == 0)
                errores += ",QS_Token";

            if (errores.Length > 0)
                valido = false;

            return valido;
        }

        public bool EsValidoToken()
        {
            bool valido = false;
            if (QS_AUT.Split('-')[0].ToString() == "t")
            {
                tutor.Codigo = this.QS_AUT.Split('-')[1].ToString().Trim();
                tutor.Estatus = true;
                TutorCtrl tutorCtrl = new TutorCtrl(null);
                tutor = tutorCtrl.Retrieve(tutor, false)[0];

                if (tutor != null)
                {
                    string cadenaToken = tutor.FechaNacimiento.Value.ToString("yyyyMMddHHmmss.fff") + tutor.Nombre.Trim() + tutor.PrimerApellido.Trim() + tutor.Codigo.Trim() + this.QS_FechaHora;
                    byte[] bytes = EncryptHash.SHA1encrypt(cadenaToken);
                    string token = EncryptHash.byteArrayToStringBase64(bytes);

                    string qsToken = this.QS_Token;
                    if (qsToken.Contains(" "))
                        qsToken = qsToken.Replace(" ", "+");

                    DateTime fechaHoraSolicitud = DateTime.ParseExact(this.QS_FechaHora, "yyyyMMddHHmmss.fff", CultureInfo.InvariantCulture);

                    bool vigente = DateTime.Now.Subtract(fechaHoraSolicitud).TotalSeconds <= 5000;

                    valido = qsToken.CompareTo(token) == 0 && vigente;
                }
            }
            else if (QS_AUT.Split('-')[0].ToString() == "a")
            {
                alumno.Curp = this.QS_AUT.Split('-')[1].ToString().Trim();
                alumno.Estatus = true;
                AlumnoCtrl alumnoCtrl = new AlumnoCtrl();
                var alumnos = alumnoCtrl.Retrieve(ConnectionHlp.Default.Connection, alumno);
                bool existeAlumno = alumnos.Tables[0].Rows.Count > 0;

                if (existeAlumno)
                {
                    alumno = alumnoCtrl.LastDataRowToAlumno(alumnos);

                    string cadenaToken = alumno.FechaNacimiento.Value.ToString("yyyyMMddHHmmss.fff") + alumno.Nombre.Trim() + alumno.PrimerApellido.Trim() + alumno.Curp.Trim() + this.QS_FechaHora;
                    byte[] bytes = EncryptHash.SHA1encrypt(cadenaToken);
                    string token = EncryptHash.byteArrayToStringBase64(bytes);

                    string qsToken = this.QS_Token;
                    if (qsToken.Contains(" "))
                        qsToken = qsToken.Replace(" ", "+");

                    DateTime fechaHoraSolicitud = DateTime.ParseExact(this.QS_FechaHora, "yyyyMMddHHmmss.fff", CultureInfo.InvariantCulture);

                    bool vigente = DateTime.Now.Subtract(fechaHoraSolicitud).TotalSeconds <= 5000;

                    valido = qsToken.CompareTo(token) == 0 && vigente;
                }
            }
            return valido;
        }

        /// <summary>
        /// Redirige al portal de administracion/PaquetesPremium
        /// </summary>
        private void RedirigirPortalTutor()
        {
            tutor.Codigo = this.QS_AUT.Split('-')[1].ToString().Trim();
            TutorCtrl tutorCtrl = new TutorCtrl(null);
            tutor = tutorCtrl.Retrieve(tutor, false).FirstOrDefault();
            string parametros = GenerarTokenYUrl(tutor);
            string urlPortal = @ConfigurationManager.AppSettings["POVUrlPortalTutor"];
            string url = urlPortal + parametros;
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Redirige al portal de administracion/PaquetesPremium
        /// </summary>
        private void RedirigirPortalAspirante()
        {
            alumno.Curp = this.QS_AUT.Split('-')[1].ToString().Trim();
            AlumnoCtrl alumnoCtrl = new AlumnoCtrl();
            var alumnos = alumnoCtrl.Retrieve(ConnectionHlp.Default.Connection, alumno);
            alumno = alumnoCtrl.LastDataRowToAlumno(alumnos);
            string parametros = GenerarTokenYUrlAspirante(alumno);
            string urlPortal = @ConfigurationManager.AppSettings["POVUrlPortalAspirante"];
            string url = urlPortal + parametros;
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Redirige a la landing Page
        /// </summary>
        private void RedirigirLandingPage()
        {
            string urlPortal = @ConfigurationManager.AppSettings["POVUrlLandingPage"];
            string url = urlPortal;
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Redirige a la login Tutor
        /// </summary>
        private void RedirigirLoginTutor()
        {
            string urlPortal = @ConfigurationManager.AppSettings["POVUrlPortalTutor"];
            string url = urlPortal;
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Redirige a la login Aspirante
        /// </summary>
        private void RedirigirLoginAspirante()
        {
            string urlPortal = @ConfigurationManager.AppSettings["POVUrlPortalAspirante"];
            string url = urlPortal;
            Response.Redirect(url, true);
        }

        /// <summary>
        /// genera un query string con los siguientes elementos: aut, fecha de solicitud, token de autorizacion
        /// </summary>
        /// <param name="docente"></param>
        /// <param name="escuela"></param>
        /// <param name="cicloEscolar"></param>
        /// <returns>Query string</returns>
        private string GenerarTokenYUrl(Tutor tutor)
        {
            string UrlPaquetesPremium;
            string formatoFecha = "yyyyMMddHHmmss.fff";
            DateTime fecha = System.DateTime.Now;
            string nombre = tutor.Nombre;
            nombre = nombre.Trim();
            string apellido = tutor.PrimerApellido;
            apellido = apellido.Trim();
            DateTime fechaNacimiento = (DateTime)tutor.FechaNacimiento;
            string token = fechaNacimiento.ToString(formatoFecha) + nombre + apellido + fecha.ToString(formatoFecha);
            byte[] clave = POV.Seguridad.Utils.EncryptHash.SHA1encrypt(token);
            token = POV.Seguridad.Utils.EncryptHash.byteArrayToStringBase64(clave);
            token = System.Web.HttpUtility.UrlEncode(token);
            UrlPaquetesPremium = "?tutor" + tutor.Codigo + "&fechahora=" + fecha.ToString(formatoFecha) + "&token=" + token;
            return UrlPaquetesPremium;
        }

        /// <summary>
        /// genera un query string con los siguientes elementos: aut, fecha de solicitud, token de autorizacion
        /// </summary>
        /// <param name="docente"></param>
        /// <param name="escuela"></param>
        /// <param name="cicloEscolar"></param>
        /// <returns>Query string</returns>
        private string GenerarTokenYUrlAspirante(Alumno alumno)
        {
            string UrlPaquetesPremium;
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
            UrlPaquetesPremium = "?alumno=" + alumno.Curp + "&fechahora=" + fecha.ToString(formatoFecha) + "&token=" + token;
            return UrlPaquetesPremium;
        }
    }
}