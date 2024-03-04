using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using POV.Seguridad.BO;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using Framework.Base.DataAccess;
using POV.Web.PortalSocial.AppCode;
using POV.Logger.Service;
using System.Configuration;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Services;
using POV.Modelo.BO;
using POV.CentroEducativo.Service;
using POV.Expediente.Service;
using POV.Licencias.BO;
using POV.Prueba.BO;
using POV.Expediente.BO;
using POV.Licencias.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using System.Collections;
using POV.DetalleExpediente.Services;
using POV.Expediente.Services;
using POV.Modelo.Context;
using POV.Blog.Services;
using POV.Blog.BO;
using System.Text.RegularExpressions;

namespace POV.Web.PortalSocial.Auth
{
    public partial class Login : System.Web.UI.Page
    {
        private IUserSession userSession;

        private AccountService accountService;

        private IRedirector redirector;

        #region QueryString
        private string QS_TipoUsuario
        {
            get { return this.Request.QueryString["u"]; }
        }             
        #endregion

        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        public Login()
        {
            userSession = new UserSession();
            accountService = new AccountService();
            redirector = new Redirector();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblFechaAnio.Text = DateTime.Now.ToString("yyyy");

                if (userSession.IsLogin())
                {
                    redirector.GoToHomePage(true);
                }

                switch (QS_TipoUsuario)
                {
                    case "asp":
                        btnOlvidasteContrasena.PostBackUrl += "?u=" + QS_TipoUsuario;
                        tituloPortal.InnerText = "PORTAL ESTUDIANTES";
                        imgIconoPortal.ImageUrl += "SOV_ELEMENTOS_portalestudiante.png";
                        divContainer.Style["background"] = "url(../images/backgroundloginestudiante.jpg)";
                        break;
                    case "ori":
                        btnOlvidasteContrasena.PostBackUrl += "?u=" + QS_TipoUsuario;
                        tituloPortal.InnerText = "PORTAL ORIENTADOR";
                        imgIconoPortal.ImageUrl += "SOV_ELEMENTOS_portalorientador.png";
                        divContainer.Style["background"] = "url(../images/backgroundloginorientador.jpg)";
                        LblNewUser.Visible = false;
                        btnRegistrar.Visible = false;
                        break;
                    default:
                        Response.Redirect(ConfigurationManager.AppSettings["POVUrlLandingPage"], true);
                        break;
                }               
            }             
        }

        protected void BtnEntrar_Click(object sender, EventArgs e)
        {
            string sError = string.Empty;
            try
            {
                userSession = new UserSession();

                bool bValid = true;
                
                clearErrorInputs();
                accountService = new AccountService();

                //Requeridos
                if (String.IsNullOrEmpty(TxtNombre.Text.Trim()))
                {
                    LblLoginFail.Text = "El usuario es requerido.";
                    TxtNombre.CssClass += " error";
                    bValid = false;
                }
                if (bValid && string.IsNullOrEmpty(TxtPassword.Text.Trim()))
                {
                    LblLoginFail.Text = "La contraseña es requerida.";
                    TxtPassword.CssClass += " error";
                    bValid = false;
                }
                //longitud
                if (bValid && TxtNombre.Text.Trim().Length > 50)
                {
                    LblLoginFail.Text = "El usuario no debe ser mayor a 50 caracteres.";
                    TxtNombre.CssClass += " error";
                    bValid = false;
                }
                if (bValid && TxtPassword.Text.Trim().Length > 50)
                {
                    LblLoginFail.Text = "La contraseña no debe ser mayor a 50 caracteres.";
                    TxtPassword.CssClass += " error";
                    bValid = false;
                }
                if (bValid)
                {
                    sError = accountService.Login(dctx, TxtNombre.Text.Trim(), TxtPassword.Text, tipoUsuario:QS_TipoUsuario);

                    UniversidadCtrl universidadCtrl = new UniversidadCtrl(null);
                    switch (QS_TipoUsuario)
                    {
                        case "asp":
                            userSession.CurrentAlumno.Universidades = new List<Universidad>();
                            userSession.CurrentAlumno.Universidades = universidadCtrl.RetrieveUniversidadByAlumno(userSession.CurrentAlumno, false).ToList();
                            if (sError == string.Empty)
                            {
                                if (userSession.CurrentAlumno.NivelEscolar != ENivelEscolar.Superior)
                                {
                                    TutorAlumnoCtrl tutorAlumnoCtrl = new TutorAlumnoCtrl(null);
                                    var tutores = tutorAlumnoCtrl.Retrieve(new TutorAlumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).ToList();
                                    LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                                    bool acpTerminos = false;
                                    foreach (TutorAlumno tut in tutores)
                                    {                                        
                                        sError = string.Empty;
                                        Usuario usr = licenciaEscuelaCtrl.RetrieveUsuarioTutor(dctx, new Tutor { TutorID = tut.TutorID });
                                        if (usr.AceptoTerminos == true)
                                        {
                                            acpTerminos = true;
                                            
                                        }
                                    }
                                    if (!acpTerminos) 
                                    {
                                        userSession.Logout();
                                        sError = "Tu tutor no ha aceptado los términos y condiciones de YOY";
                                        throw new Exception();
                                    }

                                    if (userSession.CurrentAlumno.EstatusPago != EEstadoPago.PAGADO ) 
                                    {
                                        userSession.Logout();
                                        sError = "No se ha actualizado tu información de pago";
                                        throw new Exception();
                                    }

                                }



                                if ((userSession.CurrentAlumno.EstatusIdentificacion == false || userSession.CurrentAlumno.EstatusIdentificacion == null) && userSession.IsAlumno())
                                {
                                    redirector.GoToConfirmarAlumno(false);
                                }
                                else
                                {
                                    if (userSession.CurrentAlumno.NivelEscolar != ENivelEscolar.Superior)
                                        redirector.GoToValidarDiagnostico(false);
                                    else
                                        redirector.GoToAceptarTerminos(false);
                                }

                            }
                            break;
                        case "ori":
                            userSession.CurrentDocente.Universidades = new List<Universidad>();
                            userSession.CurrentDocente.Universidades = universidadCtrl.RetrieveUniversidadByDocente(userSession.CurrentDocente, false).ToList();
                            break;
                    }

                    if (!string.IsNullOrEmpty(sError))
                    {
                        LblLoginFail.Text = sError;
                        LoggerHlp.Default.Info(this, sError);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(sError);
                LoggerHlp.Default.Error(this, ex);
            }
            
        }
        private void ShowMessage(string p)
        {
            LblLoginFail.Text = p;
        }

        private void clearErrorInputs()
        {
            LblLoginFail.Text = "";
            TxtNombre.CssClass = "form-control";
            TxtPassword.CssClass = "form-control";
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {            
            switch (QS_TipoUsuario)
	        {
                case "asp":
                    Response.Redirect(ConfigurationManager.AppSettings["CAMPUSNuevoAspirante"],true);
                    break;
                case "ori":
                    Response.Redirect(ConfigurationManager.AppSettings["POVUrlLandingPageRequisitos"], true);
                    break;
		        default :
                    Response.Redirect(ConfigurationManager.AppSettings["POVUrlLandingPage"], true);
                    break;
	        }
        }
    }
}