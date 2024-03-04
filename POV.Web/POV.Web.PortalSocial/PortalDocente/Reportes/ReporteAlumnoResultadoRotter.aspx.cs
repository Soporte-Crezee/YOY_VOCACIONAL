using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Expediente.BO;
using POV.Expediente.Reports.Reports;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Logger.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalDocente.Reportes
{
    public partial class ReporteAlumnoResultadoRotter : System.Web.UI.Page
    {
        #region Propiedades de la clase
        private string QS_Alumno
        {
            get { return this.Request.QueryString["num"]; }
        }

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

        private int SS_ResultadoRotter
        {
            get { return (int)this.Session["ResultadoRotter"]; }
            set { this.Session["ResultadoRotter"] = value; }
        }

        private string SS_FechaFin
        {
            get { return (string)this.Session["FechaFin"]; }
            set { this.Session["FechaFin"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;

        private AlumnoCtrl alumnoCtrl;
        private UsuarioCtrl usuarioCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        #endregion

        public ReporteAlumnoResultadoRotter()
        {
            userSession = new UserSession();
            redirector = new Redirector();

            alumnoCtrl = new AlumnoCtrl();
            usuarioCtrl = new UsuarioCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            respuestaAlumnoCtrl = new RespuestaAlumnoCtrl();
        }

        #region Eventos de la página
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
            SS_ResultadoRotter = 0;
            SS_FechaFin = string.Empty;

            CargarDatos();

            if (SS_AlumnoCarga == null || SS_UsuarioCarga == null || SS_FechaFin == string.Empty)
            {
                redirector.GoToHomePage(false);
            }
            else
            {
                ResultadoPruebaRotterRpt report = new ResultadoPruebaRotterRpt(this.SS_AlumnoCarga, this.SS_UsuarioCarga, this.SS_ResultadoRotter, this.SS_FechaFin);
                rptVAlumnos.PageByPage = false;
                rptVAlumnos.Report = report;

            }
        }
        #endregion

        #region Métodos auxiliares
        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Default.aspx";
        }

        private void CargarDatos()
        {
            Alumno alumnoEstudiante = new Alumno();
            if (QS_Alumno != null)
                alumnoEstudiante.AlumnoID = long.Parse(QS_Alumno);

            Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, alumnoEstudiante));
            // Datos alumno
            SS_AlumnoCarga = alumno;

            // Datos Usuario
            Usuario usuarioAlumno = licenciaEscuelaCtrl.RetrieveUsuario(dctx, new Alumno { AlumnoID = alumno.AlumnoID, Curp = alumno.Curp });
            Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuarioAlumno));
            SS_UsuarioCarga = usuario;

            // Para obtener intereses
            Contrato contrato = userSession.Contrato;
            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = userSession.CurrentCicloEscolar });
            var pruebaDinamica = new PruebaDinamica();

            #region Pruebas
            // Prueba realizada
            List<ETipoPruebaPresentacion> tipoPresentacionPruebas = new List<ETipoPruebaPresentacion>();
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.Rotter);
            List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumno, tipoPresentacionPruebas, EEstadoPrueba.CERRADA).Distinct().ToList();
            int resultadoRotter = 0;
            foreach (var prueba in respuestaAlumno)
            {
                if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Rotter)
                {
                    resultadoRotter = Convert.ToInt32(prueba.Calificacion.Value);

                    AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                    resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                    resultado.Prueba = new PruebaDinamica();
                    resultado.Prueba.PruebaID = prueba.PruebaID;
                    resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);

                    var registro = resultado as ResultadoPruebaDinamica;
                    DateTime tmpFecha = new DateTime();
                    tmpFecha = Convert.ToDateTime(registro.RegistroPrueba.FechaFin);
                    SS_FechaFin = tmpFecha.ToShortDateString();

                    SS_ResultadoRotter = resultadoRotter;
                }
            }
            #endregion
        }
        #endregion
    }
}