using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.Services;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
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
using POV.Web.Helper;
using POV.Web.PortalSocial.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalAlumno.Reportes
{
    public partial class ReporteAlumnoResultadoFrasesVocacionales : System.Web.UI.Page
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

        private string SS_FechaFin
        {
            get { return (string)this.Session["FechaFin"]; }
            set { this.Session["FechaFin"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;
        private SumarioGeneralFrasesVocacionales sumarioGeneralFrasesVocacionales;

        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        private AlumnoCtrl alumnoCtrl;
        private UsuarioCtrl usuarioCtrl;

        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        #endregion

        public ReporteAlumnoResultadoFrasesVocacionales()
        {
            userSession = new UserSession();
            redirector = new Redirector();

            alumnoCtrl = new AlumnoCtrl();
            usuarioCtrl = new UsuarioCtrl();

            respuestaAlumnoCtrl = new RespuestaAlumnoCtrl();
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
                    SS_AlumnoCarga = null;
                    SS_UsuarioCarga = null;
                    SS_FechaFin = string.Empty;
                    sumarioGeneralFrasesVocacionales = CargarDatos();

                    if (sumarioGeneralFrasesVocacionales == null || SS_AlumnoCarga == null || SS_UsuarioCarga == null || SS_FechaFin == string.Empty)
                        redirector.GoToHomePage(false);
                    else
                    {
                        ResultadoPruebaFrasesVocacionalesRpt report = new ResultadoPruebaFrasesVocacionalesRpt(sumarioGeneralFrasesVocacionales, this.SS_AlumnoCarga, this.SS_UsuarioCarga, this.SS_FechaFin);
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
        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Default.aspx";
        }

        private SumarioGeneralFrasesVocacionales CargarDatos()
        {
            RegistroPruebaDinamicaCtrl ctrl = new RegistroPruebaDinamicaCtrl();
            SumarioGeneralFrasesVocacionales obj = new SumarioGeneralFrasesVocacionales();
            obj.Prueba = new PruebaDinamica(){PruebaID = 20};
            obj.Alumno = new Alumno(){AlumnoID = userSession.CurrentAlumno.AlumnoID};
            // Datos del alumno
            Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }));
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

            // Datos de Usuario
            Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }));
            SS_UsuarioCarga = usuario;
            try
            {
                return ctrl.LastDataRowToSumarioGeneralFrasesVocacionales(ctrl.Retrieve(ConnectionHlp.Default.Connection, obj));
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}