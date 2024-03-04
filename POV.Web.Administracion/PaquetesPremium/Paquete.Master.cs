using POV.Administracion.BO;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Logger.Service;
using POV.Web.Administracion.Helper;
using Framework.Base.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.Administracion.PaquetesPremium
{
    public partial class Paquete : System.Web.UI.MasterPage
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private Tutor tutor;
        private Alumno alumno;

        #region Variables de session
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

        private PaquetePremium Session_PaquetePremium
        {
            get { return (PaquetePremium)this.Session["Session_PaquetePremium"]; }
            set { this.Session["Session_PaquetePremium"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            tutor = new Tutor();
            alumno = new Alumno();            
            try
            {
                lblAnio.Text = DateTime.Now.ToString("yyyy");
                if (Session_Alumno == null && Session_Tutor == null)
                {
                    this.RedirigirLandingPage();
                }
                else {
                    ShowMenu();
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                return;
            }
        }

        protected void HlpRedireccion_Click(object sender, EventArgs e)
        {
            GoToHomePage();
        }

        public void GoToHomePage()
        {
            if (Session_Alumno != null)
            {
                RedirigirPortalAspirante();
            }
            if (Session_Tutor != null)
            {
                RedirigirPortalTutor();
            }
        }         

        private void ShowMenu()
        {
            MultiViewToolBarMenu.SetActiveView(ViewToolBarMenuTutor);
        }

        /// <summary>
        /// Redirige al portal de administracion/PaquetesPremium
        /// </summary>
        public void RedirigirPortalTutor()
        {
            tutor = Session_Tutor;
            string parametros = GenerarTokenYUrl(tutor);
            string urlPortal = @ConfigurationManager.AppSettings["POVUrlPortalTutor"];
            string url = urlPortal + parametros;
            ClearSession();
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Redirige al portal de administracion/PaquetesPremium
        /// </summary>
        public void RedirigirPortalAspirante()
        {
            alumno = Session_Alumno;
            AlumnoCtrl alumnoCtrl = new AlumnoCtrl();
            var alumnos = alumnoCtrl.Retrieve(ConnectionHlp.Default.Connection, alumno);
            alumno = alumnoCtrl.LastDataRowToAlumno(alumnos);
            string parametros = GenerarTokenYUrlAspirante(alumno);
            string urlPortal = @ConfigurationManager.AppSettings["POVUrlPortalAspirante"];
            string url = urlPortal + parametros;
            ClearSession();
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Redirige a la landing Page
        /// </summary>
        public void RedirigirLandingPage()
        {
            string urlPortal = @ConfigurationManager.AppSettings["POVUrlLandingPage"];
            string url = urlPortal;
            ClearSession();
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Redirige a la login Tutor
        /// </summary>
        private void RedirigirLoginTutor()
        {
            string urlPortal = @ConfigurationManager.AppSettings["POVUrlPortalTutor"];
            string url = urlPortal;
            ClearSession();
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Redirige a la login Aspirante
        /// </summary>
        private void RedirigirLoginAspirante()
        {
            string urlPortal = @ConfigurationManager.AppSettings["POVUrlPortalAspirante"];
            string url = urlPortal;
            ClearSession();
            Response.Redirect(url, true);
        }

        /// <summary>
        /// genera un query string con token de autorizacion
        /// </summary>
        /// <param name="tutor">tutor</param>
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
            string token = fechaNacimiento.ToString(formatoFecha) + nombre + apellido + tutor.Codigo.Trim() + fecha.ToString(formatoFecha);
            byte[] clave = POV.Seguridad.Utils.EncryptHash.SHA1encrypt(token);
            token = POV.Seguridad.Utils.EncryptHash.byteArrayToStringBase64(clave);
            token = System.Web.HttpUtility.UrlEncode(token);
            UrlPaquetesPremium = "?tutor=" + tutor.Codigo + "&fechahora=" + fecha.ToString(formatoFecha) + "&token=" + token;
            return UrlPaquetesPremium;
        }

        /// <summary>
        /// genera un query string con token de autorizacion
        /// </summary>
        /// <param name="alumno">alumno</param>
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

        private void ClearSession() {
            Session_Alumno = null;
            Session_Tutor = null;
            Session_PaquetePremium = null;
        }
    }
}