using POV.CentroEducativo.BO;
using POV.CentroEducativo.Services;
using POV.Core.PadreTutor.Implements;
using POV.Core.PadreTutor.Interfaces;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.Web.PortalTutor.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalTutor
{
    public partial class Site : System.Web.UI.MasterPage
    {
        #region Propiedades
        private IUserSession userSession;
        private IRedirector redirector;

        private Tutor tutorInicio;
        public bool correoConfirmado;
        public bool datosCompletos;
        public string tiempoEsperaNotificacion;

        public Tutor Session_Tutor
        {
            get { return (Tutor)this.Session["TUTOR_KEY"]; }
            set { this.Session["TUTOR_KEY"] = value; }
        }

        public string Session_DatosCompletos
        {
            get { return (string)this.Session["DatosCompletos"]; }
            set { this.Session["DatosCompletos"] = value; }
        }

        #region QueryString
        public string QS_AUT
        {
            get { return this.Request.QueryString["aut"]; }
        }

        private string QS_Token
        {
            get { return this.Request.QueryString["token"]; }
        }

        private string QS_FechaHora
        {
            get { return this.Request.QueryString["fechahora"]; }
        }
        #endregion
        #endregion

        public Site()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            tutorInicio = new Tutor();
            tiempoEsperaNotificacion = System.Configuration.ConfigurationManager.AppSettings["POVtiempoEsperaNotificacion"];
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Header.DataBind();
            
            if (!IsPostBack)
            {
                if (userSession.IsLogin())
                {
                    txtDatosCompletos.Text = (Session_DatosCompletos != null) ? Session_DatosCompletos.ToString() : string.Empty;

                    LblNombreTutorTop.Text = userSession.CurrentTutor.Nombre + " " + userSession.CurrentTutor.PrimerApellido;
                    tutorInicio = getDataTutorToObject();
                    if (tutorInicio.DatosCompletos != null)
                    {
                        Session_DatosCompletos = tutorInicio.DatosCompletos.ToString();
                        txtDatosCompletos.Text = tutorInicio.DatosCompletos.ToString();
                    }
                    else
                    {
                        txtDatosCompletos.Text = "False";
                    }
                    if (tutorInicio.CorreoConfirmado != null)
                    {
                        txtCorreoConfirmado1.Text = tutorInicio.CorreoConfirmado.ToString();
                    }
                    else
                    {
                        txtCorreoConfirmado1.Text = "False";
                    }

                    //lblFechaAnio.Text = DateTime.Now.ToString("yyyy");
                    InitMenuTutor();
                    ShowMenuTutor();

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

        private Tutor getDataTutorToObject()
        {
            tutorInicio.TutorID = userSession.CurrentTutor.TutorID;
            tutorInicio = new TutorCtrl(null).Retrieve(tutorInicio, false)[0];

            return tutorInicio;
        }

        private void InitMenuTutor()
        {
            this.HplLogoutTutor.NavigateUrl = UrlHlp.GetLogoutURL();
            this.HlpEditarPerfilTutor.NavigateUrl = UrlHlp.GetEditarPerfilURL();
            this.HlpCambiarPasswordTutor.NavigateUrl = UrlHlp.GetEditarPassURL();
            this.HlpExpediente.NavigateUrl = UrlHlp.GetExpedienteURL();
            this.HlpResultadoHabitos.NavigateUrl = UrlHlp.GetResultadoHabitosURL();
            this.HlpResultadoDominos.NavigateUrl = UrlHlp.GetResultadoDominosURL();
            this.HlpResultadoTerman.NavigateUrl = UrlHlp.GetResultadoTermanURL();
            this.HlpResultadoSacks.NavigateUrl = UrlHlp.GetResultadoSACKSURL();
            this.HlpResultadoCleaver.NavigateUrl = UrlHlp.GetResultadoCleaverURL();
            this.HlpResultadoChaside.NavigateUrl = UrlHlp.GetResultadoChasideURL();
            this.HlpResultadoAllport.NavigateUrl = UrlHlp.GetResultadoAllportURL();
            this.HlpResultadoKuder.NavigateUrl = UrlHlp.GetResultadoKuderURL();
            this.HlpResultadoRotter.NavigateUrl = UrlHlp.GetResultadoRotterURL();
            this.HlpResultadoRaven.NavigateUrl = UrlHlp.GetResultadoRavenURL();
            this.HlpResultadoFrases.NavigateUrl = UrlHlp.GetResultadoFrasesVocacionalesURL();
            this.HlpResultadoZavic.NavigateUrl = UrlHlp.GetResultadoZavicURL();
            this.HlpInvitacion.NavigateUrl = UrlHlp.GetInvitacionURL();
        }

        private void ShowMenuTutor()
        {
            MultiviewTopMenu.SetActiveView(ViewTopMenuTutor);
            MultiViewToolBarMenu.SetActiveView(ViewToolBarMenuTutor);
        }

        private bool EsValidoQueryString()
        {
            bool valido = true;
            string errores = "";
                if (this.QS_AUT == null || this.QS_AUT.Trim().Length == 0)
                    errores += ",QS_AUT";
                if (this.QS_Token == null || this.QS_Token.Trim().Length == 0)
                    errores += ",QS_Token";

            if (errores.Length > 0)
                valido = false;

            return valido;
        }

        public bool EsValidoToken()
        {
            bool valido = false;
            
                Tutor tutor = new Tutor();
                tutor.Codigo = this.QS_AUT.Split('-')[1].ToString().Trim();
                tutor.Estatus = true;
                TutorCtrl tutorCtrl = new TutorCtrl(null);
                var tutores = tutorCtrl.Retrieve(tutor,false).FirstOrDefault();

                if (tutores != null)
                {
                    tutor = tutorCtrl.Retrieve(tutores,false).FirstOrDefault();

                    string cadenaToken = tutor.FechaNacimiento.Value.ToString("yyyyMMddHHmmss.fff") + tutor.Nombre.Trim() + tutor.PrimerApellido.Trim() + this.QS_FechaHora;
                    byte[] bytes = EncryptHash.SHA1encrypt(cadenaToken);
                    string token = EncryptHash.byteArrayToStringBase64(bytes);

                    string qsToken = this.QS_Token;
                    if (qsToken.Contains(" "))
                        qsToken = qsToken.Replace(" ", "+");

                    DateTime fechaHoraSolicitud = DateTime.ParseExact(this.QS_FechaHora, "yyyyMMddHHmmss.fff", CultureInfo.InvariantCulture);

                    bool vigente = DateTime.Now.Subtract(fechaHoraSolicitud).TotalSeconds <= 5000;

                    valido = qsToken.CompareTo(token) == 0 && vigente;
                }
                return valido;
            }           
    }
}