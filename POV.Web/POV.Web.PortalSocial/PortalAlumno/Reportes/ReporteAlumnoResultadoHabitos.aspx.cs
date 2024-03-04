using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Expediente.BO;
using POV.Expediente.Reports.Reports;
using POV.Expediente.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Logger.Service;
using POV.Modelo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalSocial.AppCode;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.Services;

namespace POV.Web.PortalSocial.PortalAlumno.Reportes
{
    public partial class ReporteAlumnoResultadoHabitos : System.Web.UI.Page
    {
        #region Propiedades de la clase
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

        private decimal SS_RespuestaHabitos
        {
            get { return (decimal)this.Session["RespuestaHabitos"]; }
            set { this.Session["RespuestaHabitos"] = value; }
        }

        private string SS_FechaFin
        {
            get { return (string)this.Session["FechaFin"]; }
            set { this.Session["FechaFin"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;

        private AlumnoCtrl alumnoCtrl;
        private UsuarioCtrl usuarioCtrl;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        private ExpedienteEscolarCtrl expedienteEscolarCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;

        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        private ResultadoPruebasOrientacionVocacionalCtrl resultadoPruebasOrientacionVocacionalCtrl;
        #endregion

        public ReporteAlumnoResultadoHabitos()
        {
            userSession = new UserSession();
            redirector = new Redirector();

            alumnoCtrl = new AlumnoCtrl();
            usuarioCtrl = new UsuarioCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();

            expedienteEscolarCtrl = new ExpedienteEscolarCtrl();

            respuestaAlumnoCtrl = new RespuestaAlumnoCtrl();
            resultadoPruebasOrientacionVocacionalCtrl = new ResultadoPruebasOrientacionVocacionalCtrl();
        }

        #region Eventos de la pagina
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
                EFAlumnoCtrl alumnoCtrl = new EFAlumnoCtrl(null);
                Alumno alumno = alumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();

                if ((bool)alumno.CorreoConfirmado)
                {
                    SS_RespuestaHabitos = 0;
                    SS_AlumnoCarga = null;
                    SS_UsuarioCarga = null;
                    SS_FechaFin = string.Empty;
                    CargarDatos();

                    if (SS_AlumnoCarga == null || SS_UsuarioCarga == null || SS_RespuestaHabitos == 0 || SS_FechaFin == string.Empty)
                    {
                        redirector.GoToHomePage(false);
                    }
                    else
                    {

                        ResultadoPruebaHabitosRpt report = new ResultadoPruebaHabitosRpt(this.SS_AlumnoCarga, this.SS_UsuarioCarga, this.SS_RespuestaHabitos, this.SS_FechaFin);
                        rptVAlumnos.PageByPage = false;
                        rptVAlumnos.Report = report;
                    }
                }
                else
                    redirector.GoToHomeAlumno(true);
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }
        #endregion

        #region Metodos Auxiliares
        private void CargarDatos()
        {
            UsuarioSocial usuarioSocial = userSession.CurrentUsuarioSocial;
            SocialHub socialHub = userSession.SocialHub;
            Usuario usuarioSession = userSession.CurrentUser;
            Alumno alumnoSession = userSession.CurrentAlumno;

            // Para obtener intereses
            GrupoCicloEscolar grupoCicloEscolar = userSession.CurrentGrupoCicloEscolar;
            Contrato contrato = userSession.Contrato;
            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = userSession.CurrentCicloEscolar });
            
            var pruebaDinamica = new PruebaDinamica(); 
            // Datos del alumno
            Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, new Alumno { AlumnoID = alumnoSession.AlumnoID }));
            SS_AlumnoCarga = alumno;

            // Datos de Usuario
            Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = usuarioSession.UsuarioID }));
            SS_UsuarioCarga = usuario;

            #region Pruebas
            // Prueba realizada
            List<ETipoPruebaPresentacion> tipoPresentacionPruebas = new List<ETipoPruebaPresentacion>();
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.HabitosEstudio);
            List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumno, tipoPresentacionPruebas, EEstadoPrueba.CERRADA).Distinct().ToList();

            // Obtener pruebas
            decimal sumaCalificacionHabitos = 0;
            foreach (var prueba in respuestaAlumno)
            {
                if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.HabitosEstudio)
                {
                    sumaCalificacionHabitos = prueba.Calificacion.Value;

                    AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                    resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                    resultado.Prueba = new PruebaDinamica();
                    resultado.Prueba.PruebaID = prueba.PruebaID;
                    resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);

                    var registro = resultado as ResultadoPruebaDinamica;
                    DateTime tmpFecha = new DateTime();
                    tmpFecha = Convert.ToDateTime(registro.RegistroPrueba.FechaFin);
                    SS_FechaFin = tmpFecha.ToShortDateString(); 

                    SS_RespuestaHabitos = sumaCalificacionHabitos;
                }
            }
            #endregion

        }

        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Default.aspx";
        }
        #endregion
    }
}