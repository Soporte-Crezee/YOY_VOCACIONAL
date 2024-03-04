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
using POV.Prueba.Diagnostico.Dinamica.Service;
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

namespace POV.Web.PortalSocial.PortalDocente.Reportes
{
    public partial class ReporteAlumnoResultadoSacks : System.Web.UI.Page
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

        private string QS_Alumno
        {
            get { return this.Request.QueryString["num"]; }
        }

        private IUserSession userSession;
        private IRedirector redirector;
        private SumarioGeneralSacks sumarioGeneralSacks;

        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        private AlumnoCtrl alumnoCtrl;
        private UsuarioCtrl usuarioCtrl;

        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        #endregion

        public ReporteAlumnoResultadoSacks()
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
                SS_AlumnoCarga = null;
                SS_UsuarioCarga = null;
                SS_FechaFin = string.Empty;
                sumarioGeneralSacks = CargarDatos();

                if (sumarioGeneralSacks == null || SS_AlumnoCarga == null || SS_UsuarioCarga == null || SS_FechaFin == string.Empty)
                    redirector.GoToHomePage(false);
                else
                {
                    ResultadoPruebaSACKSRpt report = new ResultadoPruebaSACKSRpt(sumarioGeneralSacks, this.SS_AlumnoCarga, this.SS_UsuarioCarga, this.SS_FechaFin);
                    rptVAlumnos.PageByPage = false;
                    rptVAlumnos.Report = report;
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }
        #endregion
        #region Métodos Auxiliares
        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Default.aspx";
        }

        private SumarioGeneralSacks CargarDatos()
        {
            Alumno alumnoEstudiante = new Alumno();//InterfaceToFiltroAlumno();
            if (QS_Alumno != null)
            {
                alumnoEstudiante.AlumnoID = long.Parse(QS_Alumno);
                
                // Datos del alumno
                Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, new Alumno { AlumnoID = alumnoEstudiante.AlumnoID }));
                SS_AlumnoCarga = alumno;

                // Para obtener intereses
                Contrato contrato = userSession.Contrato;
                ContratoCtrl contratoCtrl = new ContratoCtrl();
                List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = userSession.CurrentCicloEscolar });

                var pruebaDinamica = new PruebaDinamica();

                #region Pruebas
                // Prueba realizada
                List<ETipoPruebaPresentacion> tipoPresentacionPruebas = new List<ETipoPruebaPresentacion>();
                tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.FrasesIncompletasSacks);
                List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumno, tipoPresentacionPruebas, EEstadoPrueba.CERRADA).Distinct().ToList();


                foreach (var prueba in respuestaAlumno)
                {
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.FrasesIncompletasSacks)
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
            }
            RegistroPruebaDinamicaCtrl ctrl = new RegistroPruebaDinamicaCtrl();
            SumarioGeneralSacks obj = new SumarioGeneralSacks();
            obj.Prueba = new PruebaDinamica() { PruebaID = 12 };
            obj.Alumno = new Alumno() { AlumnoID = userSession.CurrentAlumno.AlumnoID };
            try
            {
                return ctrl.LastDataRowToSumarioGeneralSacks(ctrl.Retrieve(dctx, obj));
            }
            catch
            {
                return null;
            }

        }
        #endregion
    }
}