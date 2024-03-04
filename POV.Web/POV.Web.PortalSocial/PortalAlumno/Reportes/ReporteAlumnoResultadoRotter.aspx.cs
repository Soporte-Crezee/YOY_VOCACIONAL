using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.Services;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Expediente.BO;
using POV.Expediente.Reports.Reports;
using POV.Expediente.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Logger.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalSocial.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalAlumno.Reportes
{
    public partial class ReporteAlumnoResultadoRotter : System.Web.UI.Page
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

        private AlumnoCtrl alumnoCtrl;
        private UsuarioCtrl usuarioCtrl;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        private ExpedienteEscolarCtrl expedienteEscolarCtrl;

        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        #endregion

        public ReporteAlumnoResultadoRotter() 
        {
            userSession = new UserSession();
            redirector = new Redirector();

            alumnoCtrl = new AlumnoCtrl();
            usuarioCtrl = new UsuarioCtrl();

            expedienteEscolarCtrl = new ExpedienteEscolarCtrl();

            respuestaAlumnoCtrl = new RespuestaAlumnoCtrl();
        }

        #region Eventos de la página
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
                    SS_ResultadoRotter = 0;
                    SS_AlumnoCarga = null;
                    SS_UsuarioCarga = null;
                    SS_FechaFin = string.Empty;
                    CargarDatos();
                    if (SS_AlumnoCarga == null || SS_UsuarioCarga == null || SS_FechaFin == string.Empty)
                    {
                        redirector.GoToHomePage(true);
                    }
                    else 
                    {
                        ResultadoPruebaRotterRpt report = new ResultadoPruebaRotterRpt(this.SS_AlumnoCarga, this.SS_UsuarioCarga, this.SS_ResultadoRotter, this.SS_FechaFin);
                        rptVAlumnos.PageByPage = false;
                        rptVAlumnos.Report = report;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }
        #endregion

        #region Metodos Auxiliares
        private void FillBack() 
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Default.aspx";
        }

        private void CargarDatos() 
        {
            UsuarioSocial usuarioSocial = userSession.CurrentUsuarioSocial;
            SocialHub socialHub = userSession.SocialHub;
            Usuario usuarioSession = userSession.CurrentUser;
            Alumno alumnoSession = userSession.CurrentAlumno;

            // Para obtener intereses
            GrupoCicloEscolar grupocicloEscolar = userSession.CurrentGrupoCicloEscolar;
            Contrato contrato = userSession.Contrato;
            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = grupocicloEscolar.CicloEscolar });

            var pruebaDinamica = new PruebaDinamica();

            // Datos Alumno
            Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, new Alumno { AlumnoID = alumnoSession.AlumnoID }));
            SS_AlumnoCarga = alumno;

            // Datos Usuario
            Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = usuarioSession.UsuarioID }));
            SS_UsuarioCarga = usuario;

            #region Pruebas
            // Pruebas realizadas
            List<ETipoPruebaPresentacion> tipoPresentacionPruebas = new List<ETipoPruebaPresentacion>();
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.Rotter);
            List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumnoSession, pruebaDinamica, EEstadoPrueba.CERRADA).Distinct().ToList();

            // Obtener prueba
            int resultadoRotter = 0;
            foreach (var prueba in respuestaAlumno)
            {
                // Resultado Rotter
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