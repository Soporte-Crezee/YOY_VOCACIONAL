using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.PadreTutor.Implements;
using POV.Core.PadreTutor.Interfaces;
using POV.Expediente.BO;
using POV.Expediente.Reports.Reports;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Logger.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Prueba.Diagnostico.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalTutor.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalTutor.Pages.Reportes
{
    public partial class ReporteAlumnoResultadoFrasesVocacionales : System.Web.UI.Page
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

        public string QS_ALUMNO
        {
            get { return this.Request.QueryString["num"]; }
        }

        private string SS_FechaFin
        {
            get { return (string)this.Session["FechaFin"]; }
            set { this.Session["FechaFin"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;
        private SumarioGeneralFrasesVocacionales sumarioGeneralFrasesVocacionales;

        private AlumnoCtrl alumnoCtrl;
        private UsuarioCtrl usuarioCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;

        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;

        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;

        public ReporteAlumnoResultadoFrasesVocacionales()
        {
            userSession = new UserSession();
            redirector = new Redirector();

            alumnoCtrl = new AlumnoCtrl();
            usuarioCtrl = new UsuarioCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();

            respuestaAlumnoCtrl = new RespuestaAlumnoCtrl();
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
                    SS_FechaFin = string.Empty;

                    sumarioGeneralFrasesVocacionales = CargarDatos();
                    if (sumarioGeneralFrasesVocacionales == null || SS_AlumnoCarga == null || SS_UsuarioCarga == null || SS_FechaFin == string.Empty)
                        redirector.GoToReporteSACKS(false);
                    else
                    {
                        ResultadoPruebaFrasesVocacionalesRpt report = new ResultadoPruebaFrasesVocacionalesRpt(sumarioGeneralFrasesVocacionales, this.SS_AlumnoCarga, this.SS_UsuarioCarga, this.SS_FechaFin);
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

        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/ResultadoPruebaFrasesVocacionalesTutorado.aspx";
        }

        private SumarioGeneralFrasesVocacionales CargarDatos()
        {
            Alumno alumnoEstudiante = new Alumno();//InterfaceToFiltroAlumno();
            if (QS_ALUMNO != null)
            {
                alumnoEstudiante.AlumnoID = long.Parse(QS_ALUMNO);
                string al_Curp = alumnoEstudiante.Curp;
                Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, alumnoEstudiante));
                SS_AlumnoCarga = alumno;

                // Para obtener intereses
                Contrato contrato = userSession.Contrato;
                ContratoCtrl contratoCtrl = new ContratoCtrl();
                List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = userSession.CurrentCicloEscolar });
                
                var pruebaDinamica = new PruebaDinamica();

                #region Pruebas
                // Prueba realizada
                List<ETipoPruebaPresentacion> tipoPresentacionPruebas = new List<ETipoPruebaPresentacion>();
                tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.FrasesIncompletasVocacionales);
                List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumno, tipoPresentacionPruebas, EEstadoPrueba.CERRADA).Distinct().ToList();


                foreach (var prueba in respuestaAlumno)
                {
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.FrasesIncompletasVocacionales)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);

                        var registro = resultado as ResultadoPruebaDinamica;
                        DateTime tmpFecha = new DateTime();
                        tmpFecha = Convert.ToDateTime(registro.RegistroPrueba.FechaFin);
                        SS_FechaFin = tmpFecha.ToShortDateString();
                    }
                }
                #endregion

                Usuario usuarioAlumno = licenciaEscuelaCtrl.RetrieveUsuario(ConnectionHlp.Default.Connection, new Alumno { AlumnoID = alumno.AlumnoID, Curp = alumno.Curp });
                Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuarioAlumno));
                SS_UsuarioCarga = usuario;
            }
            RegistroPruebaDinamicaCtrl ctrl = new RegistroPruebaDinamicaCtrl();
            SumarioGeneralFrasesVocacionales obj = new SumarioGeneralFrasesVocacionales();
            obj.Prueba = new PruebaDinamica() { PruebaID = 20 };
            obj.Alumno = new Alumno() { AlumnoID = long.Parse(QS_ALUMNO) };
            try
            {
                return ctrl.LastDataRowToSumarioGeneralFrasesVocacionales(ctrl.Retrieve(ConnectionHlp.Default.Connection, obj));
            }
            catch
            {
                return null;
            }
        }
    }
}