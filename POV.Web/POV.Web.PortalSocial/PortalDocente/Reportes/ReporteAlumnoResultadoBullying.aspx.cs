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
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalDocente.Reportes
{
    public partial class ReporteAlumnoResultadoBullying : System.Web.UI.Page
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

        private Dictionary<string, string> SS_ImagesReporte
        {
            get { return (Dictionary<string, string>)this.Session["ImagesReporte"]; }
            set { this.Session["ImagesReporte"] = value; }
        }

        private List<ResultadoBullying> SS_ResultadoBullying
        {
            get { return (List<ResultadoBullying>)this.Session["ResultadoBullying"]; }
            set { this.Session["ResultadoBullying"] = value; }
        }

        private string QS_Alumno
        {
            get { return this.Request.QueryString["num"]; }
        }


        private IUserSession userSession;
        private IRedirector redirector;

        private AlumnoCtrl alumnoCtrl;
        private UsuarioCtrl usuarioCtrl;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        private ExpedienteEscolarCtrl expedienteEscolarCtrl;

        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        private ResultadoPruebasOrientacionVocacionalCtrl resultadoPruebasOrientacionVocacionalCtrl;

        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        #endregion

        public ReporteAlumnoResultadoBullying()
        {
            userSession = new UserSession();
            redirector = new Redirector();

            alumnoCtrl = new AlumnoCtrl();
            usuarioCtrl = new UsuarioCtrl();

            expedienteEscolarCtrl = new ExpedienteEscolarCtrl();

            respuestaAlumnoCtrl = new RespuestaAlumnoCtrl();
            resultadoPruebasOrientacionVocacionalCtrl = new ResultadoPruebasOrientacionVocacionalCtrl();

            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
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
                    SS_AlumnoCarga = null;
                    SS_UsuarioCarga = null;
                    CargarDatos();
                    if (SS_AlumnoCarga == null || SS_UsuarioCarga == null || SS_ImagesReporte.Count < 0 || SS_ResultadoBullying.Count < 0)
                    {
                        redirector.GoToHomePage(true);
                    }
                    else
                    {
                        ResultadoPruebaBullyingRpt report = new ResultadoPruebaBullyingRpt(this.SS_AlumnoCarga, this.SS_UsuarioCarga, this.SS_ResultadoBullying, this.SS_ImagesReporte);
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
            Alumno alumnoEstudiante = new Alumno();//InterfaceToFiltroAlumno();
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

                // Optimizacion para consulta de pruebas
                // Se manda la lista de la presentacion de las pruebas a consultar
                List<ETipoPruebaPresentacion> presentacionPrueba = new List<ETipoPruebaPresentacion>();
                presentacionPrueba.Add(ETipoPruebaPresentacion.Autoconcepto);
                presentacionPrueba.Add(ETipoPruebaPresentacion.Actitudes);
                presentacionPrueba.Add(ETipoPruebaPresentacion.Empatia);
                presentacionPrueba.Add(ETipoPruebaPresentacion.Humor);
                presentacionPrueba.Add(ETipoPruebaPresentacion.Victimizacion);
                presentacionPrueba.Add(ETipoPruebaPresentacion.Ciberbullying);
                presentacionPrueba.Add(ETipoPruebaPresentacion.Bullying);
                presentacionPrueba.Add(ETipoPruebaPresentacion.Violencia);
                presentacionPrueba.Add(ETipoPruebaPresentacion.Comunicacion);
                presentacionPrueba.Add(ETipoPruebaPresentacion.ImagenCorporal);
                presentacionPrueba.Add(ETipoPruebaPresentacion.Ansiedad);
                presentacionPrueba.Add(ETipoPruebaPresentacion.Depresion);
                var pruebaDinamica = new PruebaDinamica();

                // Datos Alumno
                SS_AlumnoCarga = alumno;

                // Datos Usuario
                Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuarioAlumno));
                SS_UsuarioCarga = usuario;

                #region Pruebas
                // Pruebas realizadas
                //List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumnoSession, pruebaDinamica, EEstadoPrueba.CERRADA).Distinct().ToList();
                List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumno, presentacionPrueba, EEstadoPrueba.CERRADA).Distinct().ToList();

                //Obtener prueba
                ResultadoBullying resultadoAutoconcepto = new ResultadoBullying();
                ResultadoBullying resultadoActitudes = new ResultadoBullying();
                ResultadoBullying resultadoEmpatia = new ResultadoBullying();
                ResultadoBullying resultadoHumor = new ResultadoBullying();
                ResultadoBullying resultadoVictimizacion = new ResultadoBullying();
                ResultadoBullying resultadoCiberbullying = new ResultadoBullying();
                ResultadoBullying resultadoBullying = new ResultadoBullying();
                ResultadoBullying resultadoViolencia = new ResultadoBullying();
                ResultadoBullying resultadoComunicacion = new ResultadoBullying();
                ResultadoBullying resultadoImagenCorporal = new ResultadoBullying();
                ResultadoBullying resultadoAnsiedad = new ResultadoBullying();
                ResultadoBullying resultadoDepresion = new ResultadoBullying();
                Dictionary<string, string> images = new Dictionary<string, string>();
                foreach (var prueba in respuestaAlumno)
                {
                    #region BateriaBullying
                    #region Bloque 1 : Victimario
                    #region Autoconcepto
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Autoconcepto)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
                        var registro = resultado as ResultadoPruebaDinamica;

                        Dictionary<string, int> factoresAutoconcepto = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaBullyingAutoconcepto(dctx, registro, userSession.CurrentAlumno);
                        if (factoresAutoconcepto.Count > 0)
                        {
                            resultadoAutoconcepto.ResultadoFactores = factoresAutoconcepto;
                            resultadoAutoconcepto.Calificacion = prueba.Calificacion.Value;
                        }
                    }
                    #endregion
                    #region Actitudes
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Actitudes)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
                        var registro = resultado as ResultadoPruebaDinamica;

                        Dictionary<string, int> factoresActitudes = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaBullyingActitudes(dctx, registro, userSession.CurrentAlumno);
                        if (factoresActitudes.Count > 0)
                        {
                            resultadoActitudes.ResultadoFactores = factoresActitudes;
                            resultadoActitudes.Calificacion = prueba.Calificacion.Value;
                        }
                    }
                    #endregion
                    #region Empatia
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Empatia)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
                        var registro = resultado as ResultadoPruebaDinamica;

                        Dictionary<string, int> factoresEmpatia = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaBullyingEmpatia(dctx, registro, userSession.CurrentAlumno);
                        if (factoresEmpatia.Count > 0)
                        {
                            resultadoEmpatia.ResultadoFactores = factoresEmpatia;
                            resultadoEmpatia.Calificacion = prueba.Calificacion.Value;
                        }
                    }
                    #endregion
                    #region Humor
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Humor)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
                        var registro = resultado as ResultadoPruebaDinamica;

                        Dictionary<string, int> factoresHumor = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaBullyingHumor(dctx, registro, userSession.CurrentAlumno);
                        if (factoresHumor.Count > 0)
                        {
                            resultadoHumor.ResultadoFactores = factoresHumor;
                            resultadoHumor.Calificacion = prueba.Calificacion.Value;
                        }
                    }
                    #endregion
                    #endregion
                    #region Bloque 2 : Victima
                    #region Victimizacion
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Victimizacion)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
                        var registro = resultado as ResultadoPruebaDinamica;

                        Dictionary<string, int> factoresVictimizacion = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaBullyingVictimizacion(dctx, registro, userSession.CurrentAlumno);
                        if (factoresVictimizacion.Count > 0)
                        {
                            resultadoVictimizacion.ResultadoFactores = factoresVictimizacion;
                            resultadoVictimizacion.Calificacion = prueba.Calificacion.Value;
                        }
                    }
                    #endregion
                    #region Ciberbullying
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Ciberbullying)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
                        var registro = resultado as ResultadoPruebaDinamica;

                        Dictionary<string, int> factoresCiberbullying = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaBullyingCiberbullying(dctx, registro, userSession.CurrentAlumno);
                        if (factoresCiberbullying.Count > 0)
                        {
                            resultadoCiberbullying.ResultadoFactores = factoresCiberbullying;
                            resultadoCiberbullying.Calificacion = prueba.Calificacion.Value;
                        }
                    }
                    #endregion
                    #region Bullying
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Bullying)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
                        var registro = resultado as ResultadoPruebaDinamica;

                        Dictionary<string, int> factoresBullying = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaBullyingBullying(dctx, registro, userSession.CurrentAlumno);
                        if (factoresBullying.Count > 0)
                        {
                            resultadoBullying.ResultadoFactores = factoresBullying;
                            resultadoBullying.Calificacion = prueba.Calificacion.Value;
                        }
                    }
                    #endregion
                    #endregion
                    #region Bloque 3 : Funcionamiento Familiar
                    #region Violencia
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Violencia)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
                        var registro = resultado as ResultadoPruebaDinamica;

                        Dictionary<string, int> factoresViolencia = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaBullyingViolencia(dctx, registro, userSession.CurrentAlumno);
                        if (factoresViolencia.Count > 0)
                        {
                            resultadoViolencia.ResultadoFactores = factoresViolencia;
                            resultadoViolencia.Calificacion = prueba.Calificacion.Value;
                        }
                    }
                    #endregion
                    #region Comunicacion
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Comunicacion)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
                        var registro = resultado as ResultadoPruebaDinamica;

                        Dictionary<string, int> factoresComunicacion = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaBullyingComunicacion(dctx, registro, userSession.CurrentAlumno);
                        if (factoresComunicacion.Count > 0)
                        {
                            resultadoComunicacion.ResultadoFactores = factoresComunicacion;
                            resultadoComunicacion.Calificacion = prueba.Calificacion.Value;
                        }
                    }
                    #endregion
                    #region Imagen Corporal
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.ImagenCorporal)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
                        var registro = resultado as ResultadoPruebaDinamica;

                        Dictionary<string, int> factoresImagenCorporal = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaBullyingImagenCorporal(dctx, registro, userSession.CurrentAlumno);
                        if (factoresImagenCorporal.Count > 0)
                        {
                            resultadoImagenCorporal.ResultadoFactores = factoresImagenCorporal;
                            resultadoImagenCorporal.Calificacion = prueba.Calificacion.Value;
                        }
                    }
                    #endregion
                    #region Ansiedad
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Ansiedad)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
                        var registro = resultado as ResultadoPruebaDinamica;

                        Dictionary<string, int> factoresAnsiedad = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaBullyingAnsiedad(dctx, registro, userSession.CurrentAlumno);
                        if (factoresAnsiedad.Count > 0)
                        {
                            resultadoAnsiedad.ResultadoFactores = factoresAnsiedad;
                            resultadoAnsiedad.Calificacion = prueba.Calificacion.Value;
                        }
                    }
                    #endregion
                    #region Depresion
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Depresion)
                    {
                        AResultadoPrueba resultado = new ResultadoPruebaDinamica();
                        resultado.ResultadoPruebaID = prueba.ResultadoPruebaID;
                        resultado.Prueba = new PruebaDinamica();
                        resultado.Prueba.PruebaID = prueba.PruebaID;
                        resultado = new PruebaDiagnosticoCtrl().RetrieveCompleteResultadoPrueba(dctx, resultado);
                        var registro = resultado as ResultadoPruebaDinamica;

                        Dictionary<string, int> factoresDepresion = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaBullyingDepresion(dctx, registro, userSession.CurrentAlumno);
                        if (factoresDepresion.Count > 0)
                        {
                            resultadoDepresion.ResultadoFactores = factoresDepresion;
                            resultadoDepresion.Calificacion = prueba.Calificacion.Value;
                        }
                    }
                    #endregion
                    #endregion
                    #endregion
                }

                #region Calificacion
                #region images
                #region Ulr
                string imggeneral = ConfigurationManager.AppSettings["UrlImgInformacionGeneral"];
                string circulo_rmedio = ConfigurationManager.AppSettings["UrlImgCirculoRiesgoMedio"];
                string circulo_ralto = ConfigurationManager.AppSettings["UrlImgCirculoRiesgoAlto"];
                string circulo_sriesgo = ConfigurationManager.AppSettings["UrlImgCirculoSinRiesgo"];

                string triangulo_riesgomedio = ConfigurationManager.AppSettings["UrlImgTrianguloRiesgoMedio"];
                string triangulo_riesgoalto = ConfigurationManager.AppSettings["UrlImgTrianguloRiesgoAlto"];
                string triangulo_riesgobajo = ConfigurationManager.AppSettings["UrlImgTrianguloRiesgoBajo"];
                string triangulo_riesgonulo = ConfigurationManager.AppSettings["UrlImgTrianguloRiesgoNulo"];

                string bloquei = ConfigurationManager.AppSettings["UrlImgBloqueI"];
                string bloqueii = ConfigurationManager.AppSettings["UrlImgBloqueII"];
                string bloqueiii = ConfigurationManager.AppSettings["UrlImgBloqueIII"];

                string escala5 = ConfigurationManager.AppSettings["UrlImgEscala5"];
                string escala9 = ConfigurationManager.AppSettings["UrlImgEscala9"];

                string resultadoEscala2FactorAutoridad = string.Empty;
                string resultadoEscala2FactorTrangresion = string.Empty;

                string resultadoEscala3 = string.Empty;

                string resultadoEscala5FactorRelacional = string.Empty;
                string resultadoEscala5FactorFisica = string.Empty;
                string resultadoEscala5FactorVerbal = string.Empty;
                string resultadoEscala5FactorIndirecta = string.Empty;

                string resultadoEscala5FactorRelacionalText = string.Empty;
                string resultadoEscala5FactorFisicaText = string.Empty;
                string resultadoEscala5FactorVerbalText = string.Empty;
                string resultadoEscala5FactorIndirectaText = string.Empty;

                string resultadoEscala6Ciberbullying = string.Empty;

                string resultadoEscala8 = string.Empty;

                string resultadoEscala10Imagen = string.Empty;

                string resultadoEscala12Depresion = string.Empty;
                #endregion
                #region Bloque I
                images.Add("Bloque I Auxiliar", bloquei);
                #region Escala 2
                if (resultadoActitudes != null || resultadoActitudes.ResultadoFactores.Count > 0)
                {
                    foreach (var item in resultadoActitudes.ResultadoFactores)
                    {
                        switch (item.Key)
                        {
                            case "Actitud positiva hacia la autoridad institucional":
                                if (item.Value <= 10)
                                    resultadoEscala2FactorAutoridad = ConfigurationManager.AppSettings["UrlImgEscala2ConflictoFuerte"];
                                else if (item.Value >= 11 && item.Value <= 19)
                                    resultadoEscala2FactorAutoridad = ConfigurationManager.AppSettings["UrlImgEscala2ConflictoMedio"];
                                else if (item.Value >= 20)
                                    resultadoEscala2FactorAutoridad = ConfigurationManager.AppSettings["UrlImgEscala2SinConflicto"];
                                images.Add("Escala 2 Autoridad", resultadoEscala2FactorAutoridad);
                                break;
                            case "Actitud positiva hacia la transgresión de normas sociales":
                                if (item.Value <= 10)
                                    resultadoEscala2FactorTrangresion = ConfigurationManager.AppSettings["UrlImgEscala2ConflictoFuerte"];
                                else if (item.Value >= 11 && item.Value <= 19)
                                    resultadoEscala2FactorTrangresion = ConfigurationManager.AppSettings["UrlImgEscala2ConflictoMedio"];
                                else if (item.Value >= 20)
                                    resultadoEscala2FactorTrangresion = ConfigurationManager.AppSettings["UrlImgEscala2SinConflicto"];
                                images.Add("Escala 2 Trangresion", resultadoEscala2FactorTrangresion);
                                break;
                        }
                    }
                }
                #endregion

                #region Escala 3
                int tmpemocional = 0;
                int tmpcognitiva = 0;
                if (resultadoEmpatia != null || resultadoEmpatia.ResultadoFactores.Count > 0)
                {
                    foreach (var item in resultadoEmpatia.ResultadoFactores)
                    {
                        switch (item.Key)
                        {
                            case "Empatía emocional":
                                tmpemocional = item.Value;
                                break;
                            case "Empatía cognitiva":
                                tmpcognitiva = item.Value;
                                break;
                        }
                    }
                    #region Nivel critico
                    if (tmpemocional <= 15 && tmpcognitiva <= 12)
                        resultadoEscala3 = ConfigurationManager.AppSettings["UrlImgEscala3ECcritico"];
                    else if (tmpemocional <= 15 && (tmpcognitiva >= 13 && tmpcognitiva <= 35))
                        resultadoEscala3 = ConfigurationManager.AppSettings["UrlImgEscala3Ecritico_Cpromedio"];
                    else if (tmpemocional <= 15 && tmpcognitiva >= 36)
                        resultadoEscala3 = ConfigurationManager.AppSettings["UrlImgEscala3Ecritico_Celevado"];
                    #endregion
                    #region Nivel promedio
                    if ((tmpemocional >= 16 && tmpemocional <= 44) && tmpcognitiva <= 12)
                        resultadoEscala3 = ConfigurationManager.AppSettings["UrlImgEscala3Epromedio_Ccritico"];
                    else if ((tmpemocional >= 16 && tmpemocional <= 44) && (tmpcognitiva >= 13 && tmpcognitiva <= 35))
                        resultadoEscala3 = ConfigurationManager.AppSettings["UrlImgEscala3ECpromedio"];
                    else if ((tmpemocional >= 16 && tmpemocional <= 44) && tmpcognitiva >= 36)
                        resultadoEscala3 = ConfigurationManager.AppSettings["UrlImgEscala3Epromedio_Celevado"];
                    #endregion
                    #region Nivel elevado
                    if (tmpemocional >= 45 && tmpcognitiva <= 12)
                        resultadoEscala3 = ConfigurationManager.AppSettings["UrlImgEscala3Eelevado_Ccritico"];
                    else if (tmpemocional >= 45 && (tmpcognitiva >= 13 && tmpcognitiva <= 35))
                        resultadoEscala3 = ConfigurationManager.AppSettings["UrlImgEscala3Eelevado_Cpromedio"];
                    else if (tmpemocional >= 45 && tmpcognitiva >= 36)
                        resultadoEscala3 = ConfigurationManager.AppSettings["UrlImgEscala3ECelevado"];
                    #endregion
                    images.Add("Escala 3 Empatia", resultadoEscala3);
                }
                #endregion
                #endregion
                #region Bloque II
                images.Add("Bloque II Auxiliar", bloqueii);
                #region Escala 5
                images.Add("Escala 5 Fondo", escala5);
                if (resultadoVictimizacion != null || resultadoVictimizacion.ResultadoFactores.Count > 0)
                {
                    foreach (var item in resultadoVictimizacion.ResultadoFactores)
                    {
                        switch (item.Key)
                        {
                            case "Victimización relacional":
                                if (item.Value <= 12)
                                {
                                    resultadoEscala5FactorRelacional = triangulo_riesgoalto; resultadoEscala5FactorRelacionalText = "Riesgo alto";
                                }
                                else if (item.Value >= 13 && item.Value <= 37)
                                {
                                    resultadoEscala5FactorRelacional = triangulo_riesgomedio; resultadoEscala5FactorRelacionalText = "Riesgo Medio";
                                }
                                else if (item.Value >= 38)
                                {
                                    resultadoEscala5FactorRelacional = triangulo_riesgonulo; resultadoEscala5FactorRelacionalText = "Sin Riesgo";
                                }
                                images.Add("Escala 5 Relacional", resultadoEscala5FactorRelacional);
                                images.Add("Escala 5 Relacional Text", resultadoEscala5FactorRelacionalText);
                                break;
                            case "Victimización manifiesta física":
                                if (item.Value <= 7)
                                {
                                    resultadoEscala5FactorFisica = triangulo_riesgoalto; resultadoEscala5FactorFisicaText = "Riesgo alto";
                                }
                                else if (item.Value >= 8 && item.Value <= 19)
                                {
                                    resultadoEscala5FactorFisica = triangulo_riesgomedio; resultadoEscala5FactorFisicaText = "Riesgo Medio";
                                }
                                else if (item.Value >= 20)
                                {
                                    resultadoEscala5FactorFisica = triangulo_riesgonulo; resultadoEscala5FactorFisicaText = "Sin Riesgo";
                                }
                                images.Add("Escala 5 Fisica", resultadoEscala5FactorFisica);
                                images.Add("Escala 5 Fisica Text", resultadoEscala5FactorFisicaText);
                                break;
                            case "Victimización manifiesta verbal":
                                if (item.Value <= 9)
                                {
                                    resultadoEscala5FactorVerbal = triangulo_riesgoalto; resultadoEscala5FactorVerbalText = "Riesgo alto";
                                }
                                else if (item.Value >= 10 && item.Value <= 29)
                                {
                                    resultadoEscala5FactorVerbal = triangulo_riesgomedio; resultadoEscala5FactorVerbalText = "Riesgo Medio";
                                }
                                else if (item.Value >= 30)
                                {
                                    resultadoEscala5FactorVerbal = triangulo_riesgonulo; resultadoEscala5FactorVerbalText = "Sin Riesgo";
                                }
                                images.Add("Escala 5 Verbal", resultadoEscala5FactorVerbal);
                                images.Add("Escala 5 Verbal Text", resultadoEscala5FactorVerbalText);
                                break;
                            case "Victimización indirecta":
                                if (item.Value <= 9)
                                {
                                    resultadoEscala5FactorIndirecta = triangulo_riesgoalto; resultadoEscala5FactorIndirectaText = "Riesgo alto";
                                }
                                else if (item.Value >= 10 && item.Value <= 29)
                                {
                                    resultadoEscala5FactorIndirecta = triangulo_riesgomedio; resultadoEscala5FactorIndirectaText = "Riesgo Medio";
                                }
                                else if (item.Value >= 30)
                                {
                                    resultadoEscala5FactorIndirecta = triangulo_riesgonulo; resultadoEscala5FactorIndirectaText = "Sin Riesgo";
                                }
                                images.Add("Escala 5 Indirecta", resultadoEscala5FactorIndirecta);
                                images.Add("Escala 5 Indirecta Text", resultadoEscala5FactorIndirectaText);
                                break;
                        }
                    }
                }
                #endregion

                #region Escala 6
                if (resultadoCiberbullying != null || resultadoCiberbullying.ResultadoFactores.Count > 0)
                {
                    foreach (var item in resultadoCiberbullying.ResultadoFactores)
                    {
                        switch (item.Key)
                        {
                            case "Ciberbullying":
                                if (item.Value <= 30)
                                    resultadoEscala6Ciberbullying = ConfigurationManager.AppSettings["UrlImgEscala6RiesgoNulo"];
                                else if (item.Value >= 31 && item.Value <= 59)
                                    resultadoEscala6Ciberbullying = ConfigurationManager.AppSettings["UrlImgEscala6RiesgoModerado"];
                                else if (item.Value >= 60)
                                    resultadoEscala6Ciberbullying = ConfigurationManager.AppSettings["UrlImgEscala6RiesgoAlto"];
                                images.Add("Escala 6 Ciberbullying", resultadoEscala6Ciberbullying);
                                break;
                        }
                    }
                }
                #endregion
                #endregion
                #region Bloque III
                images.Add("Bloque III Auxiliar", bloqueiii);
                #region Escala 8
                int tmpFisica = 0;
                int tmpVerbal = 0;
                int tmpEconomica = 0;
                if (resultadoViolencia != null || resultadoViolencia.ResultadoFactores.Count > 0)
                {
                    foreach (var item in resultadoViolencia.ResultadoFactores)
                    {
                        switch (item.Key)
                        {
                            case "Violencia física":
                                tmpFisica = item.Value;
                                break;
                            case "Violencia verbal":
                                tmpVerbal = item.Value;
                                break;
                            case "Violencia económica":
                                tmpEconomica = item.Value;
                                break;
                        }
                    }
                    #region Combinaciones Sin riesgo
                    if (tmpFisica < 10 && tmpVerbal < 8 && tmpEconomica <= 4)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8FVErnulo"];
                    else if (tmpFisica < 10 && tmpVerbal < 8 && (tmpEconomica > 4 && tmpEconomica <= 8))
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8FVrnulo_Ermedio"];
                    else if (tmpFisica < 10 && tmpVerbal < 8 && tmpEconomica > 8)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8FVrnulo_Eralto"];

                    if (tmpFisica < 10 && (tmpVerbal > 8 && tmpVerbal <= 26) && tmpEconomica <= 4)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Frnulo_Vrmedio_Ernulo"];
                    else if (tmpFisica < 10 && (tmpVerbal > 8 && tmpVerbal <= 26) && (tmpEconomica > 4 && tmpEconomica <= 8))
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Frnulo_VErmedio"];
                    else if (tmpFisica < 10 && (tmpVerbal > 8 && tmpVerbal <= 26) && tmpEconomica > 8)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Frnulo_Vrmedio_Eralto"];

                    if (tmpFisica < 10 && tmpVerbal > 26 && tmpEconomica <= 4)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Frnulo_Vralto_Ernulo"];
                    else if (tmpFisica < 10 && tmpVerbal > 26 && (tmpEconomica > 4 && tmpEconomica <= 8))
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Frnulo_Vralto_Ermedio"];
                    else if (tmpFisica < 10 && tmpVerbal > 26 && tmpEconomica > 8)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Frnulo_VEralto"];
                    #endregion

                    #region Combinaciones Riesgo medio
                    if ((tmpFisica > 10 && tmpFisica <= 30) && tmpVerbal < 8 && tmpEconomica <= 4)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Frmedio_VErnulo"];
                    else if ((tmpFisica > 10 && tmpFisica <= 30) && tmpVerbal < 8 && (tmpEconomica > 4 && tmpEconomica <= 8))
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Frmedio_Vrnulo_Ermedio"];
                    else if ((tmpFisica > 10 && tmpFisica <= 30) && tmpVerbal < 8 && tmpEconomica > 8)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Frmedio_Vrnulo_Eralto"];

                    if ((tmpFisica > 10 && tmpFisica <= 30) && (tmpVerbal > 8 && tmpVerbal <= 26) && tmpEconomica <= 4)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8FVrmedio_Ernulo"];
                    else if ((tmpFisica > 10 && tmpFisica <= 30) && (tmpVerbal > 8 && tmpVerbal <= 26) && (tmpEconomica > 4 && tmpEconomica <= 8))
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8FVErmedio"];
                    else if ((tmpFisica > 10 && tmpFisica <= 30) && (tmpVerbal > 8 && tmpVerbal <= 26) && tmpEconomica > 8)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8FVrmedio_Eralto"];

                    if ((tmpFisica > 10 && tmpFisica <= 30) && tmpVerbal > 26 && tmpEconomica <= 4)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Frmedio_Vralto_Ernulo"];
                    else if ((tmpFisica > 10 && tmpFisica <= 30) && tmpVerbal > 26 && (tmpEconomica > 4 && tmpEconomica <= 8))
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Frmedio_Vralto_Ermedio"];
                    else if ((tmpFisica > 10 && tmpFisica <= 30) && tmpVerbal > 26 && tmpEconomica > 8)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Frmedio_VEralto"];
                    #endregion

                    #region Combinaciones Riesgo alto
                    if (tmpFisica > 30 && tmpVerbal < 8 && tmpEconomica <= 4)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Fralto_VErnulo"];
                    else if (tmpFisica > 30 && tmpVerbal < 8 && (tmpEconomica > 4 && tmpEconomica <= 8))
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Fralto_Vrnulo_Ermedio"];
                    else if (tmpFisica > 30 && tmpVerbal < 8 && (tmpEconomica > 8 && tmpEconomica <= 10))
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Fralto_Vrnulo_Eralto"];

                    if (tmpFisica > 30 && (tmpVerbal > 8 && tmpVerbal <= 26) && tmpEconomica <= 4)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Fralto_Vrmedio_Ernulo"];
                    else if (tmpFisica > 30 && (tmpVerbal > 8 && tmpVerbal <= 26) && (tmpEconomica > 4 && tmpEconomica <= 8))
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Fralto_VErmedio"];
                    else if (tmpFisica > 30 && (tmpVerbal > 8 && tmpVerbal <= 26) && tmpEconomica > 8)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8Fralto_Vrmedio_Eralto"];

                    if (tmpFisica > 30 && tmpVerbal > 26 && tmpEconomica <= 4)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8FVralto_Ernulo"];
                    else if (tmpFisica > 30 && tmpVerbal > 26 && (tmpEconomica > 4 && tmpEconomica <= 8))
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8FVralto_Ermedio"];
                    else if (tmpFisica > 30 && tmpVerbal > 26 && tmpEconomica > 8)
                        resultadoEscala8 = ConfigurationManager.AppSettings["UrlImgEscala8FVEralto"];

                    images.Add("Escala 8 Violencia", resultadoEscala8);
                    #endregion
                }
                #endregion
                images.Add("Escala 9", escala9);

                #region Escala 10
                if (resultadoImagenCorporal != null || resultadoImagenCorporal.ResultadoFactores.Count > 0)
                {
                    foreach (var item in resultadoImagenCorporal.ResultadoFactores)
                    {
                        switch (item.Key)
                        {
                            case "Imagen":
                                if (item.Value <= 42)
                                    resultadoEscala10Imagen = ConfigurationManager.AppSettings["UrlImgEscala10RNulo"];
                                else if (item.Value >= 43 && item.Value <= 127)
                                    resultadoEscala10Imagen = ConfigurationManager.AppSettings["UrlImgEscala10RMedio"];
                                else if (item.Value >= 128)
                                    resultadoEscala10Imagen = ConfigurationManager.AppSettings["UrlImgEscala10RAlto"];

                                images.Add("Escala 10 Imagen", resultadoEscala10Imagen);
                                break;
                        }
                    }
                }
                #endregion

                #region Escala 12
                if (resultadoDepresion != null || resultadoDepresion.ResultadoFactores.Count > 0)
                {
                    foreach (var item in resultadoDepresion.ResultadoFactores)
                    {
                        switch (item.Key)
                        {
                            case "Depresión":
                                if (item.Value <= 18)
                                    resultadoEscala12Depresion = ConfigurationManager.AppSettings["UrlImgEscala12RNulo"];

                                else if (item.Value > 18 && item.Value <= 34)
                                    resultadoEscala12Depresion = ConfigurationManager.AppSettings["UrlImgEscala12RLeve"];

                                else if (item.Value > 34 && item.Value <= 49)
                                    resultadoEscala12Depresion = ConfigurationManager.AppSettings["UrlImgEscala12RMedio"];

                                else if (item.Value > 49)
                                    resultadoEscala12Depresion = ConfigurationManager.AppSettings["UrlImgEscala12RAlto"];

                                images.Add("Escala 12 Depresion", resultadoEscala12Depresion);
                                break;
                        }
                    }
                }
                #endregion
                #endregion
                List<ResultadoBullying> resultadosBateriaBullying = new List<ResultadoBullying>();
                if (resultadoAutoconcepto != null || resultadoAutoconcepto.ResultadoFactores.Count > 0)
                    resultadosBateriaBullying.Add(resultadoAutoconcepto);

                if (resultadoHumor != null || resultadoHumor.ResultadoFactores.Count > 0)
                    resultadosBateriaBullying.Add(resultadoHumor);

                if (resultadoBullying != null || resultadoBullying.ResultadoFactores.Count > 0)
                    resultadosBateriaBullying.Add(resultadoBullying);

                if (resultadoComunicacion != null || resultadoComunicacion.ResultadoFactores.Count > 0)
                    resultadosBateriaBullying.Add(resultadoComunicacion);

                if (resultadoAnsiedad != null || resultadoAnsiedad.ResultadoFactores.Count > 0)
                    resultadosBateriaBullying.Add(resultadoAnsiedad);

                #region General
                // Variables
                List<string> calificacionBloqueI = new List<string>();
                List<string> calificacionBloqueII = new List<string>();
                List<string> calificacionBloqueIII = new List<string>();
                string calificacionGralEscala1 = string.Empty;
                string calificacionGralEscala2 = string.Empty;
                string calificacionGralEscala3 = string.Empty;
                string calificacionGralEscala4 = string.Empty;
                string calificacionGralEscala5 = string.Empty;
                string calificacionGralEscala6 = string.Empty;
                string calificacionGralEscala7 = string.Empty;
                string calificacionGralEscala8 = string.Empty;
                string calificacionGralEscala9 = string.Empty;
                string calificacionGralEscala10 = string.Empty;
                string calificacionGralEscala11 = string.Empty;
                string calificacionGralEscala12 = string.Empty;

                #region Escala 1
                if (resultadoAutoconcepto.Calificacion <= 60)
                    calificacionGralEscala1 = "Riesgo Alto";
                if (resultadoAutoconcepto.Calificacion >= 61 && resultadoAutoconcepto.Calificacion <= 119)
                    calificacionGralEscala1 = "Riesgo Medio";
                if (resultadoAutoconcepto.Calificacion >= 120)
                    calificacionGralEscala1 = "Sin Riesgo";

                calificacionBloqueI.Add(calificacionGralEscala1);
                #endregion
                #region Escala 2
                if (resultadoActitudes.Calificacion <= 20)
                    calificacionGralEscala2 = "Riesgo Alto";
                if (resultadoActitudes.Calificacion >= 21 && resultadoActitudes.Calificacion <= 39)
                    calificacionGralEscala2 = "Riesgo Medio";
                if (resultadoActitudes.Calificacion >= 40)
                    calificacionGralEscala2 = "Sin Riesgo";

                calificacionBloqueI.Add(calificacionGralEscala2);
                #endregion
                #region Escala 3
                if (resultadoEmpatia.Calificacion <= 40)
                    calificacionGralEscala3 = "Riesgo Alto";
                if (resultadoEmpatia.Calificacion >= 41 && resultadoEmpatia.Calificacion <= 79)
                    calificacionGralEscala3 = "Riesgo Medio";
                if (resultadoEmpatia.Calificacion >= 80)
                    calificacionGralEscala3 = "Sin Riesgo";

                calificacionBloqueI.Add(calificacionGralEscala3);
                #endregion
                #region Escala 4
                if (resultadoHumor.Calificacion < 64)
                    calificacionGralEscala4 = "Riesgo Alto";
                if (resultadoHumor.Calificacion >= 65 && resultadoHumor.Calificacion <= 127)
                    calificacionGralEscala4 = "Riesgo Medio";
                if (resultadoHumor.Calificacion >= 128)
                    calificacionGralEscala4 = "Sin Riesgo";

                calificacionBloqueI.Add(calificacionGralEscala4);
                #endregion
                #region Escala 5
                if (resultadoVictimizacion.Calificacion <= 58)
                    calificacionGralEscala5 = "Riesgo Alto";
                if (resultadoVictimizacion.Calificacion >= 59 && resultadoVictimizacion.Calificacion <= 115)
                    calificacionGralEscala5 = "Riesgo Medio";
                if (resultadoVictimizacion.Calificacion >= 116)
                    calificacionGralEscala5 = "Sin Riesgo";

                calificacionBloqueII.Add(calificacionGralEscala5);
                #endregion
                #region Escala 6
                if (resultadoCiberbullying.Calificacion >= 60)
                    calificacionGralEscala6 = "Riesgo Alto";
                if (resultadoCiberbullying.Calificacion >= 31 && resultadoCiberbullying.Calificacion <= 59)
                    calificacionGralEscala6 = "Riesgo Medio";
                if (resultadoCiberbullying.Calificacion <= 30)
                    calificacionGralEscala6 = "Sin Riesgo";

                calificacionBloqueII.Add(calificacionGralEscala6);
                #endregion
                #region Escala 7
                if (resultadoBullying.Calificacion >= 348)
                    calificacionGralEscala7 = "Riesgo Alto";
                if (resultadoBullying.Calificacion >= 175 && resultadoBullying.Calificacion <= 347)
                    calificacionGralEscala7 = "Riesgo Medio";
                if (resultadoBullying.Calificacion <= 174)
                    calificacionGralEscala7 = "Sin Riesgo";

                calificacionBloqueII.Add(calificacionGralEscala7);
                #endregion
                #region Escala 8
                if (resultadoViolencia.Calificacion >= 64)
                    calificacionGralEscala8 = "Riesgo Alto";
                if (resultadoViolencia.Calificacion >= 33 && resultadoViolencia.Calificacion <= 63)
                    calificacionGralEscala8 = "Riesgo Medio";
                if (resultadoViolencia.Calificacion <= 32)
                    calificacionGralEscala8 = "Sin Riesgo";

                calificacionBloqueIII.Add(calificacionGralEscala8);
                #endregion
                #region Escala 9
                if (resultadoComunicacion.Calificacion <= 58)
                    calificacionGralEscala9 = "Riesgo Alto";
                if (resultadoComunicacion.Calificacion >= 59 && resultadoComunicacion.Calificacion <= 115)
                    calificacionGralEscala9 = "Riesgo Medio";
                if (resultadoComunicacion.Calificacion >= 116)
                    calificacionGralEscala9 = "Sin Riesgo";

                calificacionBloqueIII.Add(calificacionGralEscala9);
                #endregion
                #region Escala 10
                if (resultadoImagenCorporal.Calificacion >= 136)
                    calificacionGralEscala10 = "Riesgo Alto";
                if (resultadoImagenCorporal.Calificacion >= 69 && resultadoImagenCorporal.Calificacion <= 135)
                    calificacionGralEscala10 = "Riesgo Medio";
                if (resultadoImagenCorporal.Calificacion <= 68)
                    calificacionGralEscala10 = "Sin Riesgo";

                calificacionBloqueIII.Add(calificacionGralEscala10);
                #endregion
                #region Escala 11
                if (resultadoAnsiedad.Calificacion >= 21)
                    calificacionGralEscala11 = "Riesgo Alto";
                if (resultadoAnsiedad.Calificacion >= 8 && resultadoAnsiedad.Calificacion <= 20)
                    calificacionGralEscala11 = "Riesgo Medio";
                if (resultadoAnsiedad.Calificacion <= 7)
                    calificacionGralEscala11 = "Sin Riesgo";

                calificacionBloqueIII.Add(calificacionGralEscala11);
                #endregion
                #region Escala 12
                if (resultadoDepresion.Calificacion >= 50)
                    calificacionGralEscala12 = "Riesgo Alto";
                if (resultadoDepresion.Calificacion >= 19 && resultadoDepresion.Calificacion <= 49)
                    calificacionGralEscala12 = "Riesgo Medio";
                if (resultadoDepresion.Calificacion <= 18)
                    calificacionGralEscala12 = "Sin Riesgo";

                calificacionBloqueIII.Add(calificacionGralEscala12);
                #endregion
                int sriesgo = 0;
                int rmedio = 0;
                int ralto = 0;
                #region Bloque I
                if (calificacionBloqueI.Count > 0)
                {
                    foreach (var item in calificacionBloqueI)
                    {
                        switch (item)
                        {
                            case "Sin Riesgo":
                                sriesgo++;
                                break;
                            case "Riesgo Medio":
                                rmedio++;
                                break;
                            case "Riesgo Alto":
                                ralto++;
                                break;
                        }
                    }
                    if (sriesgo == 4 || (sriesgo == 3 && rmedio == 1))
                    {
                        images.Add("General Bloque I", circulo_sriesgo);
                        images.Add("Texto Bloque I", "Sin Riesgo");
                    }
                    else if (ralto == 4 || (ralto == 3 && rmedio == 1))
                    {
                        images.Add("General Bloque I", circulo_ralto);
                        images.Add("Texto Bloque I", "Riesgo Alto");
                    }
                    else
                    {
                        images.Add("General Bloque I", circulo_rmedio);
                        images.Add("Texto Bloque I", "Riesgo Medio");
                    }
                }
                #endregion

                #region Bloque II
                sriesgo = 0;
                rmedio = 0;
                ralto = 0;
                if (calificacionBloqueII.Count > 0)
                {
                    foreach (var item in calificacionBloqueII)
                    {
                        switch (item)
                        {
                            case "Sin Riesgo":
                                sriesgo++;
                                break;
                            case "Riesgo Medio":
                                rmedio++;
                                break;
                            case "Riesgo Alto":
                                ralto++;
                                break;
                        }
                    }
                    if (sriesgo == 3 || (sriesgo == 2 && rmedio == 1))
                    {
                        images.Add("General Bloque II", circulo_sriesgo);
                        images.Add("Texto Bloque II", "Sin Riesgo");
                    }
                    else if (ralto == 3 || (ralto == 2 && rmedio == 1))
                    {
                        images.Add("General Bloque II", circulo_ralto);
                        images.Add("Texto Bloque II", "Riesgo Alto");
                    }
                    else
                    {
                        images.Add("General Bloque II", circulo_rmedio);
                        images.Add("Texto Bloque II", "Riesgo Medio");
                    }
                }
                #endregion

                #region Bloque III
                sriesgo = 0;
                rmedio = 0;
                ralto = 0;
                if (calificacionBloqueIII.Count > 0)
                {
                    foreach (var item in calificacionBloqueIII)
                    {
                        switch (item)
                        {
                            case "Sin Riesgo":
                                sriesgo++;
                                break;
                            case "Riesgo Medio":
                                rmedio++;
                                break;
                            case "Riesgo Alto":
                                ralto++;
                                break;
                        }
                    }
                    if (sriesgo == 5 || (sriesgo == 4 && rmedio == 1))
                    {
                        images.Add("General Bloque III", circulo_sriesgo);
                        images.Add("Texto Bloque III", "Sin Riesgo");
                    }
                    else if (ralto == 5 || (ralto == 4 && rmedio == 1))
                    {
                        images.Add("General Bloque III", circulo_ralto);
                        images.Add("Texto Bloque III", "Riesgo Alto");
                    }
                    else
                    {
                        images.Add("General Bloque III", circulo_rmedio);
                        images.Add("Texto Bloque III", "Riesgo Medio");
                    }
                }
                #endregion

                images.Add("Imagen Auxiliar General", imggeneral);
                #endregion

                SS_ResultadoBullying = resultadosBateriaBullying;
                SS_ImagesReporte = images;
                #endregion


                #endregion
                #endregion
            }
        }
        #endregion

    }
}