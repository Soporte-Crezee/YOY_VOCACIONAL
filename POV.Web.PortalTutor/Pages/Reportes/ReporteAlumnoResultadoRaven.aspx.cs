using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.PadreTutor.Implements;
using POV.Core.PadreTutor.Interfaces;
using POV.Expediente.BO;
using POV.Expediente.Reports.Reports;
using POV.Expediente.Service;
using POV.Expediente.Services;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Logger.Service;
using POV.Modelo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Reactivos.BO;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalTutor.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalTutor.Pages.Reportes
{
    public partial class ReporteAlumnoResultadoRaven : System.Web.UI.Page
    {
        private Alumno SS_AlumnoCarga
        {
            get { return (Alumno)this.Session["AlumnoCarga"]; }
            set { this.Session["AlumnoCarga"] = value; }
        }

        private Usuario SS_UsuarioCarga
        {
            get { return (Usuario)this.Session["UsuarioCarga"]; }
            set { this.Session["UsuarioCarga"] = value; }
        }

        private string SS_FechaInicio
        {
            get { return (string)this.Session["FechaInicio"]; }
            set { this.Session["FechaInicio"] = value; }
        }

        private string SS_FechaFin
        {
            get { return (string)this.Session["FechaFin"]; }
            set { this.Session["FechaFin"] = value; }
        }

        private string QS_Alumno
        {
            get { return this.Request.QueryString["num"]; }
        }

        private DiagnosticoRaven SS_DiagnosticoRaven
        {
            get { return (DiagnosticoRaven)this.Session["DiagnosticoRaven"]; }
            set { this.Session["DiagnosticoRaven"] = value; }
        }

        #region Propiedades
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;

        private AlumnoCtrl alumnoCtrl;

        private UsuarioCtrl usuarioCtrl;

        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;

        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        private ResultadoPruebasOrientacionVocacionalCtrl resultadoPruebasOrientacionVocacionalCtrl;
        #endregion

        public ReporteAlumnoResultadoRaven()
        {
            userSession = new UserSession();
            redirector = new Redirector();

            alumnoCtrl = new AlumnoCtrl();
            usuarioCtrl = new UsuarioCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();

            respuestaAlumnoCtrl = new RespuestaAlumnoCtrl();
            resultadoPruebasOrientacionVocacionalCtrl = new ResultadoPruebasOrientacionVocacionalCtrl();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!userSession.IsLogin())
                    {
                        redirector.GoToLoginPage(true);
                    }
                    else
                    {
                        FillBack();
                    }
                }
                else
                {
                    if (!userSession.IsLogin())
                    {
                        redirector.GoToLoginPage(true);
                    }
                }

                if ((bool)userSession.CurrentTutor.CorreoConfirmado && (bool)userSession.CurrentTutor.DatosCompletos)
                {
                    SS_AlumnoCarga = null;
                    SS_UsuarioCarga = null;
                    SS_DiagnosticoRaven = null;
                    SS_FechaInicio = string.Empty;
                    SS_FechaFin = string.Empty;

                    CargarDatos();

                    if (SS_DiagnosticoRaven == null || SS_AlumnoCarga == null || SS_UsuarioCarga == null || SS_FechaInicio == string.Empty || SS_FechaFin == string.Empty)
                    {
                        redirector.GoToHomePage(false);
                    }
                    else
                    {

                        ResultadoPruebaRavenRpt report = new ResultadoPruebaRavenRpt(this.SS_DiagnosticoRaven, this.SS_AlumnoCarga, this.SS_UsuarioCarga, this.SS_FechaInicio, this.SS_FechaFin);
                        rptVAlumnos.PageByPage = false;
                        rptVAlumnos.Report = report;
                    }
                }
                else
                    redirector.GoToHomePage(true);
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        private void CargarDatos()
        {
            Alumno alumnoEstudiante = new Alumno();//InterfaceToFiltroAlumno();
            if (QS_Alumno != null)
            {
                alumnoEstudiante.AlumnoID = long.Parse(QS_Alumno);
                string al_Curp = alumnoEstudiante.Curp;
                Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, alumnoEstudiante));

                Usuario usuarioAlumno = licenciaEscuelaCtrl.RetrieveUsuario(dctx, new Alumno { AlumnoID = alumno.AlumnoID, Curp = alumno.Curp });
                // Para obtener intereses
                Contrato contrato = userSession.Contrato;
                ContratoCtrl contratoCtrl = new ContratoCtrl();
                List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = userSession.CurrentCicloEscolar });

                var pruebaDinamica = new PruebaDinamica();

                // Datos del alumno
                SS_AlumnoCarga = alumno;

                // Datos de Usuario
                Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuarioAlumno));
                SS_UsuarioCarga = usuario;

                #region Pruebas
                // Prueba realizada
                List<ETipoPruebaPresentacion> tipoPresentacionPruebas = new List<ETipoPruebaPresentacion>();
                tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.Raven);
                List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumno, tipoPresentacionPruebas, EEstadoPrueba.CERRADA).Distinct().ToList();

                DiagnosticoRaven diagnostico = new DiagnosticoRaven();

                foreach (var prueba in respuestaAlumno)
                {
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Raven)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
                        var registro = resultado as ResultadoPruebaDinamica;

                        diagnostico = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaRaven(dctx, registro, alumno);
                                                
                        DateTime tmpFecha = new DateTime();
                        tmpFecha = Convert.ToDateTime(registro.RegistroPrueba.FechaInicio);
                        SS_FechaInicio = tmpFecha.ToShortDateString();
                        tmpFecha = Convert.ToDateTime(registro.RegistroPrueba.FechaFin);
                        SS_FechaFin = tmpFecha.ToShortDateString();

                        SS_DiagnosticoRaven= diagnostico;
                    }
                }
                #endregion
            }
        }

        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            }
            else
            {
                lnkBack.NavigateUrl = "ResultadoPruebaRavenTutorado.aspx";
            }
        }
    }
}