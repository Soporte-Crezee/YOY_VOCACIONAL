using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Expediente.BO;
using POV.Expediente.Reports.Reports;
using POV.Expediente.Service;
using POV.Expediente.Services;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Modelo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Reactivos.BO;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalSocial.AppCode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalDocente.Reportes
{
    public partial class ReporteAlumnoResultadoZavic : System.Web.UI.Page
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

        private string QS_Alumno
        {
            get { return this.Request.QueryString["num"]; }
        }

        private string SS_FechaFin
        {
            get { return (string)this.Session["FechaFin"]; }
            set { this.Session["FechaFin"] = value; }
        }

        private Dictionary<string, string> SS_RespuestaZavic
        {
            get { return (Dictionary<string, string>)this.Session["RespuestaZavic"]; }
            set { this.Session["RespuestaZavic"] = value; }
        }

        #region Propiedades
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        private AlumnoCtrl alumnoCtrl;

        private UsuarioCtrl usuarioCtrl;

        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;

        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        private ResultadoPruebasOrientacionVocacionalCtrl resultadoPruebasOrientacionVocacionalCtrl;
        #endregion

        public ReporteAlumnoResultadoZavic()
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

            SS_AlumnoCarga = null;
            SS_UsuarioCarga = null;
            SS_RespuestaZavic = null;
            SS_FechaFin = string.Empty;

            CargarDatos();

            if (SS_RespuestaZavic == null || SS_AlumnoCarga == null || SS_UsuarioCarga == null || SS_FechaFin == string.Empty)
            {
                redirector.GoToHomePage(false);
            }
            else
            {

                ResultadoPruebaZavicRpt report = new ResultadoPruebaZavicRpt(this.SS_AlumnoCarga, this.SS_UsuarioCarga, this.SS_RespuestaZavic, this.SS_FechaFin);
                rptVAlumnos.PageByPage = false;
                rptVAlumnos.Report = report;
            }
        }

        private void CargarDatos()
        {
            Alumno alumnoEstudiante = new Alumno();
            if (QS_Alumno != null)
            {
                alumnoEstudiante.AlumnoID = long.Parse(QS_Alumno);
                string al_Curp = alumnoEstudiante.Curp;
                Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, alumnoEstudiante));

                Usuario usuarioAlumno = licenciaEscuelaCtrl.RetrieveUsuario(ConnectionHelper.Default.Connection, new Alumno { AlumnoID = alumno.AlumnoID, Curp = alumno.Curp });
                // Para obtener intereses
                Contrato contrato = userSession.Contrato;
                ContratoCtrl contratoCtrl = new ContratoCtrl();
                List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = userSession.CurrentCicloEscolar });
                
                var pruebaDinamica = new PruebaDinamica();

                // Areas de conocimiento
                ArrayList arrAreaConocimiento = new ArrayList();
                List<Clasificador> areasConocimiento = new List<Clasificador>();

                // Datos del alumno
                SS_AlumnoCarga = alumno;

                // Datos de Usuario
                Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuarioAlumno));
                SS_UsuarioCarga = usuario;

                #region Pruebas
                // Prueba realizada
                List<ETipoPruebaPresentacion> tipoPresentacionPruebas = new List<ETipoPruebaPresentacion>();
                tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.Zavic);
                List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumno, tipoPresentacionPruebas, EEstadoPrueba.CERRADA).Distinct().ToList();

                Dictionary<string, string> sumaCalificacionesZavic = new Dictionary<string, string>();

                foreach (var prueba in respuestaAlumno)
                {
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Zavic)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);

                        var registro = resultado as ResultadoPruebaDinamica;

                        sumaCalificacionesZavic = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaZavic(dctx, registro, alumno);

                        
                        DateTime tmpFecha = new DateTime();
                        tmpFecha = Convert.ToDateTime(registro.RegistroPrueba.FechaInicio);
                        tmpFecha = Convert.ToDateTime(registro.RegistroPrueba.FechaFin);
                        SS_FechaFin = tmpFecha.ToShortDateString();

                        SS_RespuestaZavic = sumaCalificacionesZavic;
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
                lnkBack.NavigateUrl = "~/Default.aspx";
            }
        }
    }
}