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

namespace POV.Web.PortalSocial.PortalAlumno.Reportes
{
    public partial class ExpedienteAlumnoReport : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;

        private AlumnoCtrl alumnoCtrl;
        private UsuarioCtrl usuarioCtrl;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        private ExpedienteEscolarCtrl expedienteEscolarCtrl;

        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        private ResultadoPruebasOrientacionVocacionalCtrl resultadoPruebasOrientacionVocacionalCtrl;

        public ExpedienteAlumnoReport() 
        {
            userSession = new UserSession();
            redirector = new Redirector();

            alumnoCtrl = new AlumnoCtrl();
            usuarioCtrl = new UsuarioCtrl();

            expedienteEscolarCtrl = new ExpedienteEscolarCtrl();

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

                        SS_RespuestaHabitos = 0;
                        SS_RespuestaDominos = 0;
                        SS_RespuestaTerman = null;
                        SS_RespuestaKuder = null;
                        SS_RespuestaAllport = null;
                        CargarDatos();
                        FillBack();
                    }
                    
                }

                if (SS_AlumnoCarga == null || SS_UsuarioCarga == null)
                {
                    redirector.GoToHomePage(false);
                }

                ExpedienteAlumnoRpt report = new ExpedienteAlumnoRpt(this.SS_AlumnoCarga, this.SS_UsuarioCarga, this.SS_RespuestaHabitos, this.SS_RespuestaDominos, this.SS_RespuestaTerman, this.SS_RespuestaKuder, this.SS_RespuestaAllport);
                   
                rptVAlumnos.Report = report;
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }


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
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = grupoCicloEscolar.CicloEscolar });

            var pruebaDinamica = new PruebaDinamica();

            // Datos del alumno
             Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, new Alumno { AlumnoID = alumnoSession.AlumnoID }));
            SS_AlumnoCarga = alumno;

            // Datos de Usuario
             Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = usuarioSession.UsuarioID }));
            SS_UsuarioCarga = usuario;

            #region Preubas
            // Prueba realizada
            #region TipoPruebaPresentacion
            List<ETipoPruebaPresentacion> tipoPresentacionPruebas = new List<ETipoPruebaPresentacion>();
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.HabitosEstudio);
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.Dominos);
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.TermanMerrill);
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.FrasesIncompletasSacks);
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.Cleaver);
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.Chaside);
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.Allport);
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.Kuder);
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.Rotter);
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.Raven);
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.FrasesIncompletasVocacionales);
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.Zavic);
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.EstilosdeAprendizaje);
            #endregion
            List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumnoSession, tipoPresentacionPruebas, EEstadoPrueba.CERRADA).Distinct().ToList();
            
            // Obtener pruebas
            decimal sumaCalificacionHabitos = 0;
            decimal sumaCalificacionDominos = 0;
            DataSet sumaSeccionesTerman = new DataSet();
            Dictionary<string, string> sumaCalificacionesKuder = new Dictionary<string, string>();
            Dictionary<string, string> sumaCalificacionesAllport = new Dictionary<string, string>();
            foreach (var prueba in respuestaAlumno)
            {
                if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.HabitosEstudio)
                {
                    sumaCalificacionHabitos = prueba.Calificacion.Value;
                    SS_RespuestaHabitos = sumaCalificacionHabitos;
                }

                if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Dominos)
                {
                    sumaCalificacionDominos = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaDomino(dctx, prueba.ResultadoPruebaID, prueba.PruebaID, alumno);
                    SS_RespuestaDominos = sumaCalificacionDominos;
                }
                // Resultados Terman
                if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.TermanMerrill) 
                {
                    sumaSeccionesTerman = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaTerman(dctx, prueba.ResultadoPruebaID, prueba.PruebaID, alumno);
                    SS_RespuestaTerman = sumaSeccionesTerman;
                }
                // Resultado Kuder
                if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Kuder)
                {
                    sumaCalificacionesKuder = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaKuder(dctx, prueba.ResultadoPruebaID, prueba.PruebaID, alumno);
                    SS_RespuestaKuder = sumaCalificacionesKuder;
                }
                // Resultado Allport
                if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Allport)
                {
                    sumaCalificacionesAllport = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaAllport(dctx, prueba.ResultadoPruebaID, prueba.PruebaID, alumno);
                    SS_RespuestaAllport = sumaCalificacionesAllport;
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

        private DataTable SS_ResultadoCarga
        {
            get { return (DataTable)this.Session["ResultadoCargaEscuelas"]; }
            set { this.Session["ResultadoCargaEscuelas"] = value; }
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

        private decimal SS_RespuestaHabitos
        {
            get { return (decimal)this.Session["RespuestaHabitos"]; }
            set { this.Session["RespuestaHabitos"] = value; }
        }

        private decimal SS_RespuestaDominos
        {
            get { return (decimal)this.Session["RespuestaDominos"]; }
            set { this.Session["RespuestaDominos"] = value; }
        }

        private DataSet SS_RespuestaTerman
        {
            get { return (DataSet)this.Session["RespuestaTerman"]; }
            set { this.Session["RespuestaTerman"] = value; }
        }

        private Dictionary<string, string> SS_RespuestaKuder
        {
            get { return (Dictionary<string, string>)this.Session["RespuestaKuder"]; }
            set { this.Session["RespuestaKuder"] = value; }
        }

        private Dictionary<string, string> SS_RespuestaAllport
        {
            get { return (Dictionary<string, string>)this.Session["RespuestaAllport"]; }
            set { this.Session["RespuestaAllport"] = value; }
        }
    }
}