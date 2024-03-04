using Framework.Base.DataAccess;
using POV.Administracion.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Expediente.BO;
using POV.Expediente.Service;
using POV.Expediente.Services;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Localizacion.Service;
using POV.Logger.Service;
using POV.Modelo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Prueba.Diagnostico.Service;
using POV.Reactivos.BO;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalSocial.AppCode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Web.PortalSocial.PortalAlumno.Reportes;
using POV.Expediente.Reports.Reports;
using System.Data.SqlClient;
using System.Configuration;

namespace POV.Web.PortalSocial.PortalDocente
{
    public partial class DetalleExpedienteAlumnoOrientador : System.Web.UI.Page
    {
        #region Propiedades de la clase
        private string QS_Alumno
        {
            get { return this.Request.QueryString["num"]; }
        }

        public ExpedienteEscolar LastObject
        {
            get { return Session["LastExpedienteEscolar"] != null ? (ExpedienteEscolar)Session["LastExpedienteEscolar"] : null; }
            set { Session["LastExpedienteEscolar"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        private AlumnoCtrl alumnoCtrl;

        private ExpedienteEscolarCtrl expedienteEscolarCtrl;

        private UsuarioCtrl usuarioCtrl;

        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;

        private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;

        private InfoAlumnoUsuario alumnoUsuario;
        private InfoAlumnoUsuarioCtrl infoAlumnoUsuarioCtrl;

        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        private ResultadoPruebasOrientacionVocacionalCtrl resultadoPruebasOrientacionVocacionalCtrl;

        private ResultadoPruebaCleaver porcentajeMas;
        private ResultadoPruebaCleaver porcentajeMenos;
        private ResultadoPruebaCleaver porcentajeTotal;
        private ResultadoPruebaDinamicaCtrl resultadoPruebaDinamicaCtrl;
        #endregion

        public DetalleExpedienteAlumnoOrientador()
        {
            userSession = new UserSession();
            redirector = new Redirector();

            alumnoCtrl = new AlumnoCtrl();

            expedienteEscolarCtrl = new ExpedienteEscolarCtrl();

            usuarioCtrl = new UsuarioCtrl();

            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();

            grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();

            alumnoUsuario = new InfoAlumnoUsuario();
            infoAlumnoUsuarioCtrl = new InfoAlumnoUsuarioCtrl();

            respuestaAlumnoCtrl = new RespuestaAlumnoCtrl();
            resultadoPruebasOrientacionVocacionalCtrl = new ResultadoPruebasOrientacionVocacionalCtrl();

            porcentajeMas = new ResultadoPruebaCleaver();
            porcentajeMenos = new ResultadoPruebaCleaver();
            porcentajeTotal = new ResultadoPruebaCleaver();

            resultadoPruebaDinamicaCtrl = new ResultadoPruebaDinamicaCtrl();
        }

        #region Eventos de la página
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (userSession.IsLogin())
                        if (!userSession.IsAlumno() && userSession.CurrentEscuela != null)
                        {
                            if (QS_Alumno != string.Empty)
                            {
                                ReiniciarEstilos();
                                CargarDatos();
                            }
                            else
                            {
                                redirector.GoToHomePage(true);
                            }
                        }
                        else
                            redirector.GoToHomePage(true);
                    else
                        redirector.GoToLoginPage(true);
                }
                else
                {
                    if (!userSession.IsLogin()) //es alumno
                    {
                        redirector.GoToLoginPage(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            ExpedienteEscolar expOld = (ExpedienteEscolar)LastObject.Clone();
            ExpedienteEscolar expNew = (ExpedienteEscolar)LastObject.Clone();
            expNew.Apuntes = txtApuntesExpediente.Text;
            expedienteEscolarCtrl.Update(dctx, expNew, expOld);
        }
        #endregion

        #region Métodos Auxiliares
        private void CargarDatos()
        {
            Alumno alumnoEstudiante = new Alumno();
            alumnoEstudiante.AlumnoID = long.Parse(QS_Alumno);

            // Consultar alumno y asignarle los datos a la interfaz
            Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, alumnoEstudiante));

            Usuario usuarioAlumno = licenciaEscuelaCtrl.RetrieveUsuario(dctx, new Alumno { AlumnoID = alumno.AlumnoID, Curp = alumno.Curp });

            Contrato contrato = userSession.Contrato;

            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = userSession.CurrentCicloEscolar });
            PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);

            var pruebaDinamica = new PruebaDinamica();

            // Areas de conocimiento
            ArrayList arrAreaConocimiento = new ArrayList();
            List<Clasificador> areasConocimiento = new List<Clasificador>();

            // Datos del alumno
            txtNombre.Text = alumno.Nombre + " " + alumno.PrimerApellido + " " + alumno.SegundoApellido;
            txtEscuela.Text = alumno.Escuela;
            if (alumno.Grado != null)
            {
                if (alumno.Grado == EGrado.SEMESTRE_1)
                    txtNivEstudio.Text = "Semestre 1";
                else if (alumno.Grado == EGrado.SEMESTRE_2)
                    txtNivEstudio.Text = "Semestre 2";
                else if (alumno.Grado == EGrado.SEMESTRE_3)
                    txtNivEstudio.Text = "Semestre 3";
                else if (alumno.Grado == EGrado.SEMESTRE_4)
                    txtNivEstudio.Text = "Semestre 4";
                else if (alumno.Grado == EGrado.SEMESTRE_5)
                    txtNivEstudio.Text = "Semestre 5";
                else if (alumno.Grado == EGrado.SEMESTRE_6)
                    txtNivEstudio.Text = "Semestre 6";
            }

            // Datos de Usuario
            Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuarioAlumno));
            txtEmail.Text = usuario.Email;
            txtTelCasa.Text = usuario.TelefonoCasa;
            txtTelReferencia.Text = usuario.TelefonoReferencia;
            


            // Obtener interes
            List<InteresAspirante> interesesAspirante = expedienteEscolarCtrl.RetrieveInteresesAspirante(dctx, alumnoEstudiante, pruebaDinamica).Distinct().ToList();
            gvIntereses.DataSource = interesesAspirante.ToList();
            gvIntereses.DataBind();

            // Obtener Apuntes del orientador
            ExpedienteEscolar exp = new ExpedienteEscolar();
            exp.Alumno = alumno;
            LastObject = expedienteEscolarCtrl.LastDataRowToExpedienteEscolar(expedienteEscolarCtrl.Retrieve(dctx, exp));
            txtApuntesExpediente.Text = LastObject.Apuntes;


            // Para obtener areas de conocmiento
            foreach (InteresAspirante clas in interesesAspirante)
            {
                if (arrAreaConocimiento.IndexOf(clas.clasificador.ClasificadorID) == -1)
                {
                    arrAreaConocimiento.Add(clas.clasificador.ClasificadorID);
                    areasConocimiento.Add(clas.clasificador);
                }
            }
            gvAreasConocimiento.DataSource = areasConocimiento.ToList();
            gvAreasConocimiento.DataBind();

            // Obtener pruebas
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
            tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.InteligengiasMultiples);
            #endregion
            List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumno, tipoPresentacionPruebas, EEstadoPrueba.CERRADA).Distinct().ToList();

            decimal sumaCalificacion = 0;
            DataSet sumaSeccionesTerman = new DataSet();
            Dictionary<string, string> sumaCalificacionesKuder = new Dictionary<string, string>();
            Dictionary<string, string> sumaCalificacionesAllport = new Dictionary<string, string>();
            DiagnosticoRaven diagnostico = new DiagnosticoRaven();
            Dictionary<string, string> sumaCalificacionesZavic = new Dictionary<string, string>();
            int resultadoRotter = 0;

            if (respuestaAlumno.Count > 0)
            {
                foreach (var prueba in respuestaAlumno)
                {
                    #region Habitos
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.HabitosEstudio)
                    {
                        sumaCalificacion = prueba.Calificacion.Value;
                        string spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                        lblResultadoGratis1.InnerText = spaces + prueba.Nombre;
                        divTablaResultados.Visible = true;
                        VistaResultado(sumaCalificacion);
                    }
                    #endregion
                    #region Dominos
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Dominos)
                    {
                        sumaCalificacion = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaDomino(dctx, prueba.ResultadoPruebaID, prueba.PruebaID, alumno);
                        string spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                        lbResultadoDominos.InnerText = spaces + prueba.Nombre;
                        divTablaResultadosDominos.Visible = true;
                        VistaResultadoDominos(sumaCalificacion);
                    }
                    #endregion
                    #region Terman
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.TermanMerrill)
                    {
                        sumaSeccionesTerman = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaTerman(dctx, prueba.ResultadoPruebaID, prueba.PruebaID, alumno);
                        string spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                        lblResultadoTerman.InnerText = spaces + prueba.Nombre;
                        divTablaResultadosTerman.Visible = true;
                        VistaResultadoTerman(sumaSeccionesTerman);
                    }
                    #endregion
                    #region Sacks
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.FrasesIncompletasSacks)
                    {
                        RegistroPruebaDinamicaCtrl ctrlRegistroPruebaDinamica = new RegistroPruebaDinamicaCtrl();
                        SumarioGeneralSacks obj = new SumarioGeneralSacks();
                        obj.Prueba = new PruebaDinamica() { PruebaID = 12 };
                        obj.Alumno = new Alumno() { AlumnoID = alumno.AlumnoID };
                        SumarioGeneralSacks resultadoSacks = ctrlRegistroPruebaDinamica.LastDataRowToSumarioGeneralSacks(ctrlRegistroPruebaDinamica.Retrieve(dctx, obj));
                        if (resultadoSacks != null)
                        {
                            string spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                            lblResultadoSacks.InnerText = spaces + prueba.Nombre;
                            divTablaResultadosSacks.Visible = true;
                            VistaResultadoSacks(resultadoSacks);
                        }
                    }
                    #endregion
                    #region Cleaver
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Cleaver)
                    {
                        var dsCleaver = resultadoPruebaDinamicaCtrl.RetrieveResultadoPruebaCleaver(dctx, new CentroEducativo.BO.Alumno() { AlumnoID = alumno.AlumnoID }, new PruebaDinamica() { PruebaID = 14 });
                        if (dsCleaver.Tables[0].Rows.Count > 0)
                        {
                            ResultadoPruebaCleaver serieMas = new ResultadoPruebaCleaver();
                            ResultadoPruebaCleaver serieMenos = new ResultadoPruebaCleaver();
                            ResultadoPruebaCleaver serieTotal = new ResultadoPruebaCleaver();
                            #region Recolectando datos
                            foreach (DataRow row in dsCleaver.Tables[0].Rows)
                            {
                                switch ((string)Convert.ChangeType(row["Texto"], typeof(string)))
                                {
                                    case "Más":
                                        switch ((string)Convert.ChangeType(row["Nombre"], typeof(string)))
                                        {
                                            case "D":
                                                serieMas.Resultado_D = (int)Convert.ChangeType(row["Valor"], typeof(int));
                                                if (row["Porcentaje"] != DBNull.Value)
                                                {
                                                    porcentajeMas.Resultado_D = (int)Convert.ChangeType(row["Porcentaje"], typeof(int));
                                                }
                                                else
                                                {
                                                    porcentajeMas.Resultado_D = 0;
                                                }
                                                break;
                                            case "I":
                                                serieMas.Resultado_I = (int)Convert.ChangeType(row["Valor"], typeof(int));
                                                if (row["Porcentaje"] != DBNull.Value)
                                                {
                                                    porcentajeMas.Resultado_I = (int)Convert.ChangeType(row["Porcentaje"], typeof(int));
                                                }
                                                else
                                                {
                                                    porcentajeMas.Resultado_I = 0;
                                                }
                                                break;
                                            case "S":
                                                serieMas.Resultado_S = (int)Convert.ChangeType(row["Valor"], typeof(int));

                                                if (row["Porcentaje"] != DBNull.Value)
                                                {
                                                    porcentajeMas.Resultado_S = (int)Convert.ChangeType(row["Porcentaje"], typeof(int));
                                                }
                                                else
                                                {
                                                    porcentajeMas.Resultado_S = 0;
                                                }
                                                break;
                                            case "C":
                                                serieMas.Resultado_C = (int)Convert.ChangeType(row["Valor"], typeof(int));
                                                if (row["Porcentaje"] != DBNull.Value)
                                                {
                                                    porcentajeMas.Resultado_C = (int)Convert.ChangeType(row["Porcentaje"], typeof(int));
                                                }
                                                else
                                                {
                                                    porcentajeMas.Resultado_C = 0;
                                                }
                                                break;
                                        }
                                        break;
                                    case "Menos":
                                        switch ((string)Convert.ChangeType(row["Nombre"], typeof(string)))
                                        {
                                            case "D":
                                                serieMenos.Resultado_D = (int)Convert.ChangeType(row["Valor"], typeof(int));
                                                porcentajeMenos.Resultado_D = (int)Convert.ChangeType(row["Porcentaje"], typeof(int));
                                                break;
                                            case "I":
                                                serieMenos.Resultado_I = (int)Convert.ChangeType(row["Valor"], typeof(int));
                                                porcentajeMenos.Resultado_I = (int)Convert.ChangeType(row["Porcentaje"], typeof(int));
                                                break;
                                            case "S":
                                                serieMenos.Resultado_S = (int)Convert.ChangeType(row["Valor"], typeof(int));
                                                porcentajeMenos.Resultado_S = (int)Convert.ChangeType(row["Porcentaje"], typeof(int));
                                                break;
                                            case "C":
                                                serieMenos.Resultado_C = (int)Convert.ChangeType(row["Valor"], typeof(int));
                                                porcentajeMenos.Resultado_C = (int)Convert.ChangeType(row["Porcentaje"], typeof(int));
                                                break;
                                        }
                                        break;
                                }
                            }
                            #endregion
                            #region Valores nulos
                            #region Menos
                            if (serieMenos.Resultado_D == null)
                                serieMenos.Resultado_D = 0;
                            if (serieMenos.Resultado_I == null)
                                serieMenos.Resultado_I = 0;
                            if (serieMenos.Resultado_S == null)
                                serieMenos.Resultado_S = 0;
                            if (serieMenos.Resultado_C == null)
                                serieMenos.Resultado_C = 0;
                            #endregion
                            #region Más
                            if (serieMas.Resultado_D == null)
                                serieMas.Resultado_D = 0;
                            if (serieMas.Resultado_I == null)
                                serieMas.Resultado_I = 0;
                            if (serieMas.Resultado_S == null)
                                serieMas.Resultado_S = 0;
                            if (serieMas.Resultado_C == null)
                                serieMas.Resultado_C = 0;
                            #endregion
                            #endregion

                            #region Obtencion de totales
                            serieTotal.Resultado_D = serieMas.Resultado_D - serieMenos.Resultado_D;
                            serieTotal.Resultado_I = serieMas.Resultado_I - serieMenos.Resultado_I;
                            serieTotal.Resultado_S = serieMas.Resultado_S - serieMenos.Resultado_S;
                            serieTotal.Resultado_C = serieMas.Resultado_C - serieMenos.Resultado_C;
                            #endregion

                            if (serieTotal.Resultado_D != null && serieTotal.Resultado_I != null && serieTotal.Resultado_S != null && serieTotal.Resultado_C != null)
                            {
                                if (serieTotal.Resultado_I > 17)
                                    serieTotal.Resultado_I = 17;
                                porcentajeTotal.Resultado_D = resultadoPruebaDinamicaCtrl.LastDataRowToPlantillaResultadoCleaver(resultadoPruebaDinamicaCtrl.RetrievePlantillaResultadoCleaver(dctx, new PlantillaResultadoCleaver() { Tag = "D", Opcion = "Total", Valor = serieTotal.Resultado_D })).Porcentaje;
                                porcentajeTotal.Resultado_I = resultadoPruebaDinamicaCtrl.LastDataRowToPlantillaResultadoCleaver(resultadoPruebaDinamicaCtrl.RetrievePlantillaResultadoCleaver(dctx, new PlantillaResultadoCleaver() { Tag = "I", Opcion = "Total", Valor = serieTotal.Resultado_I })).Porcentaje;
                                porcentajeTotal.Resultado_S = resultadoPruebaDinamicaCtrl.LastDataRowToPlantillaResultadoCleaver(resultadoPruebaDinamicaCtrl.RetrievePlantillaResultadoCleaver(dctx, new PlantillaResultadoCleaver() { Tag = "S", Opcion = "Total", Valor = serieTotal.Resultado_S })).Porcentaje;
                                porcentajeTotal.Resultado_C = resultadoPruebaDinamicaCtrl.LastDataRowToPlantillaResultadoCleaver(resultadoPruebaDinamicaCtrl.RetrievePlantillaResultadoCleaver(dctx, new PlantillaResultadoCleaver() { Tag = "C", Opcion = "Total", Valor = serieTotal.Resultado_C })).Porcentaje;

                                string spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                                lblResultadoCleaver.InnerText = spaces + prueba.Nombre;
                                divTablaResultadosCleaver.Visible = true;
                                VistaResultadoCleaver(porcentajeMas, porcentajeMenos, porcentajeTotal);

                            }
                        }
                    }
                    #endregion
                    #region Chaside
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Chaside)
                    {
                        DataSet dsChasideInteres = resultadoPruebaDinamicaCtrl.RetrieveResultadoPruebaCHASIDE(dctx, alumno, new PruebaDinamica { PruebaID = prueba.PruebaID }, 1);
                        DataSet dsChasideAptitud = resultadoPruebaDinamicaCtrl.RetrieveResultadoPruebaCHASIDE(dctx, alumno, new PruebaDinamica { PruebaID = prueba.PruebaID }, 2);
                        if (dsChasideInteres.Tables[0].Rows.Count > 0 && dsChasideAptitud.Tables[0].Rows.Count > 0)
                        {
                            string spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                            lblResultadoChaside.InnerText = spaces + prueba.Nombre;
                            divTablaResultadosChaside.Visible = true;
                            VistaResultadoChaside(dsChasideInteres, dsChasideAptitud);
                        }
                    }
                    #endregion
                    #region Allport
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Allport)
                    {
                        sumaCalificacionesAllport = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaAllport(dctx, prueba.ResultadoPruebaID, prueba.PruebaID, alumno);
                        string spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                        lblResultadoAllport.InnerText = spaces + prueba.Nombre;
                        divTablaResultadosAllport.Visible = true;
                        VistaResultadoAllport(sumaCalificacionesAllport);
                    }
                    #endregion
                    #region Kuder
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Kuder)
                    {
                        sumaCalificacionesKuder = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaKuder(dctx, prueba.ResultadoPruebaID, prueba.PruebaID, alumno);
                        string spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                        lblResultadoKuder.InnerText = spaces + prueba.Nombre;
                        divTablaResultadosKuder.Visible = true;
                        VistaResultadoKuder(sumaCalificacionesKuder);
                    }
                    #endregion
                    #region Rotter
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Rotter)
                    {
                        resultadoRotter = Convert.ToInt32(prueba.Calificacion.Value);
                        string spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                        lblResultadoRotter.InnerText = spaces + prueba.Nombre;
                        divTablaResultadosRotter.Visible = true;
                        VistaResultadoRotter(resultadoRotter);
                    }
                    #endregion
                    #region Raven
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
                        string spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                        lblResultadoRaven.InnerText = spaces + prueba.Nombre;
                        divTablaResultadoRaven.Visible = true;
                        VistaResultadoRaven(diagnostico, tmpFecha.ToShortDateString());
                    }
                    #endregion
                    #region Frases Vocacionales
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.FrasesIncompletasVocacionales)
                    {
                        RegistroPruebaDinamicaCtrl ctrlRegistroPruebaDinamica = new RegistroPruebaDinamicaCtrl();
                        SumarioGeneralFrasesVocacionales obj = new SumarioGeneralFrasesVocacionales();
                        obj.Prueba = new PruebaDinamica() { PruebaID = 20 };
                        obj.Alumno = new Alumno() { AlumnoID = userSession.CurrentAlumno.AlumnoID };
                        SumarioGeneralFrasesVocacionales resultadoFrasesVocacionales = ctrlRegistroPruebaDinamica.LastDataRowToSumarioGeneralFrasesVocacionales(ctrlRegistroPruebaDinamica.Retrieve(dctx, obj));
                        if (resultadoFrasesVocacionales != null)
                        {
                            string spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                            lblResultadoFrases.InnerText = spaces + prueba.Nombre;
                            divTablaResultadosFrases.Visible = true;
                            VistaResultadoFrasesVocacionales(resultadoFrasesVocacionales);
                        }
                    }
                    #endregion
                    #region Zavic
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Zavic)
                    {
                        sumaCalificacionesZavic = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaZavic(dctx, prueba.ResultadoPruebaID, prueba.PruebaID, alumno);
                        string spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                        lblResultadoZavic.InnerText = spaces + prueba.Nombre;
                        divTablaResultadosZavic.Visible = true;
                        VistaResultadoZavic(sumaCalificacionesZavic);
                    }
                    #endregion
                    #region InteligenciasMultiples
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.InteligengiasMultiples)
                    {
                        sumaCalificacion = prueba.Calificacion.Value;
                        string spaces = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                        //lblResultadoMultiples.InnerText = spaces + prueba.Nombre;
                        lblResultadoMultiples.InnerText = spaces + "Inteligencias Múltiples";
                        divTablaResultadosMultiples.Visible = true;
                        VistaResultadoMultiples(sumaCalificacion);
                    }
                    #endregion
                }
            }
            else
            {
                divTablaResultados.Visible = false;
                divTablaResultadosDominos.Visible = false;
                divTablaResultadosTerman.Visible = false;
            }
        }

        public void ReiniciarEstilos()
        {
            string backColor = "padding: 5px 0 5px 8px;margin: 0;border: 1px solid #ccc;color: #aaa;width: 16%;height: auto;width: 200px;";

            td050.Style.Value = backColor;
            tdMal.Style.Value = backColor;
            tdMBajo.Style.Value = backColor;

            td5175.Style.Value = backColor;
            tdReg.Style.Value = backColor;
            tdBajo.Style.Value = backColor;

            td7695.Style.Value = backColor;
            tdBue.Style.Value = backColor;
            tdMedio.Style.Value = backColor;

            td96115.Style.Value = backColor;
            tdMuy.Style.Value = backColor;
            tdAlto.Style.Value = backColor;

            td116150.Style.Value = backColor;
            tdExc.Style.Value = backColor;
            tdMAlto.Style.Value = backColor;

            td1a.Style.Value = backColor;
            td1b.Style.Value = backColor;

            td2a.Style.Value = backColor;
            td2b.Style.Value = backColor;

            td3a.Style.Value = backColor;
            td3b.Style.Value = backColor;

            td4a.Style.Value = backColor;
            td4b.Style.Value = backColor;

            td5a.Style.Value = backColor;
            td5b.Style.Value = backColor;

            #region Terman
            lblPunt1Terman.Text = "";
            lblPunt2Terman.Text = "";
            lblPunt3Terman.Text = "";
            lblPunt4Terman.Text = "";
            lblPunt5Terman.Text = "";
            lblPunt6Terman.Text = "";
            lblPunt7Terman.Text = "";
            lblPunt8Terman.Text = "";
            lblPunt9Terman.Text = "";
            lblPunt10Terman.Text = "";

            lblRango1Terman.Text = "";
            lblRango2Terman.Text = "";
            lblRango3Terman.Text = "";
            lblRango4Terman.Text = "";
            lblRango5Terman.Text = "";
            lblRango6Terman.Text = "";
            lblRango7Terman.Text = "";
            lblRango8Terman.Text = "";
            lblRango9Terman.Text = "";
            lblRango10Terman.Text = "";

            RangoCI.Text = "";
            ValorCI.Text = "";
            #endregion
        }

        public void VistaResultado(Decimal sumaCalificacion)
        {
            divTablaResultados.Visible = true;
            string backColor = "background-color: #33acfd; color: #fff !important;";

            if (sumaCalificacion >= 0 && sumaCalificacion <= 50)
            {
                td050.Style.Value = backColor;
                tdMal.Style.Value = backColor;
                tdMBajo.Style.Value = backColor;
            }
            else if (sumaCalificacion >= 51 && sumaCalificacion <= 75)
            {
                td5175.Style.Value = backColor;
                tdReg.Style.Value = backColor;
                tdBajo.Style.Value = backColor;
            }
            else if (sumaCalificacion >= 76 && sumaCalificacion <= 95)
            {
                td7695.Style.Value = backColor;
                tdBue.Style.Value = backColor;
                tdMedio.Style.Value = backColor;
            }
            else if (sumaCalificacion >= 96 && sumaCalificacion <= 115)
            {
                td96115.Style.Value = backColor;
                tdMuy.Style.Value = backColor;
                tdAlto.Style.Value = backColor;
            }
            else if (sumaCalificacion >= 114)
            {
                td116150.Style.Value = backColor;
                tdExc.Style.Value = backColor;
                tdMAlto.Style.Value = backColor;
            }

        }

        public void VistaResultadoDominos(Decimal sumaCalificacion)
        {
            divTablaResultadosDominos.Visible = true;
            string backColor = "background-color: #33acfd; color: #fff !important;";

            if (sumaCalificacion > 90 && sumaCalificacion <= 100)
            {
                td1a.Style.Value = backColor;
                td1b.Style.Value = backColor;
            }
            else if (sumaCalificacion >= 75 && sumaCalificacion <= 90)
            {
                td2a.Style.Value = backColor;
                td2b.Style.Value = backColor;
            }
            else if (sumaCalificacion > 25 && sumaCalificacion < 75)
            {
                td3a.Style.Value = backColor;
                td3b.Style.Value = backColor;
            }
            else if (sumaCalificacion >= 10 && sumaCalificacion <= 25)
            {
                td4a.Style.Value = backColor;
                td4b.Style.Value = backColor;
            }
            else if (sumaCalificacion >= 0 && sumaCalificacion < 10)
            {
                td5a.Style.Value = backColor;
                td5b.Style.Value = backColor;
            }
        }

        private void VistaResultadoTerman(DataSet series)
        {
            series.Tables[0].DefaultView.Sort = "ClasificadorID ASC";
            DataTable dtb = series.Tables[0].DefaultView.ToTable();
            series.Tables[0].Rows.Clear();
            foreach (DataRow row in dtb.Rows)
                series.Tables[0].Rows.Add(row.ItemArray);

            int serieUno = Convert.ToInt32(series.Tables[0].Rows[0].ItemArray[1].ToString());
            lblPunt1Terman.Text = serieUno + "";

            int serieDos = Convert.ToInt32(series.Tables[0].Rows[1].ItemArray[1].ToString());
            lblPunt2Terman.Text = serieDos + "";

            int serieTres = Convert.ToInt32(series.Tables[0].Rows[2].ItemArray[1].ToString());
            lblPunt3Terman.Text = serieTres + "";

            double serieCuatro = Convert.ToDouble(series.Tables[0].Rows[3].ItemArray[1].ToString());
            lblPunt4Terman.Text = serieCuatro + "";

            int serieCinco = Convert.ToInt32(series.Tables[0].Rows[4].ItemArray[1].ToString());
            lblPunt5Terman.Text = serieCinco + "";

            int serieSeis = Convert.ToInt32(series.Tables[0].Rows[5].ItemArray[1].ToString());
            lblPunt6Terman.Text = serieSeis + "";

            int serieSiete = Convert.ToInt32(series.Tables[0].Rows[6].ItemArray[1].ToString());
            lblPunt7Terman.Text = serieSiete + "";

            int serieOcho = Convert.ToInt32(series.Tables[0].Rows[7].ItemArray[1].ToString());
            lblPunt8Terman.Text = serieOcho + "";

            int serieNueve = Convert.ToInt32(series.Tables[0].Rows[8].ItemArray[1].ToString());
            lblPunt9Terman.Text = serieNueve + "";

            int serieDiez = Convert.ToInt32(series.Tables[0].Rows[9].ItemArray[1].ToString());
            lblPunt10Terman.Text = serieDiez + "";

            #region Puntuaciones
            #region Serie I
            if (serieUno >= 16)
                lblRango1Terman.Text = "Sobresaliente";
            if (serieUno == 15)
                lblRango1Terman.Text = "Superior";
            if (serieUno == 14)
                lblRango1Terman.Text = "Térm. M. A";
            if (serieUno == 12 || serieUno == 13)
                lblRango1Terman.Text = "Térm. Medio";
            if (serieUno == 10 || serieUno == 11)
                lblRango1Terman.Text = "Térm. M. B.";
            if (serieUno == 8 || serieUno == 9)
                lblRango1Terman.Text = "Inferior";
            if (serieUno <= 7)
                lblRango1Terman.Text = "Deficiente";
            #endregion

            #region Serie II
            if (serieDos >= 21)
                lblRango2Terman.Text = "Sobresaliente";
            if (serieDos == 20)
                lblRango2Terman.Text = "Superior";
            if (serieDos >= 17 && serieDos <= 19)
                lblRango2Terman.Text = "Térm. M. A";
            if (serieDos >= 12 && serieDos <= 16)
                lblRango2Terman.Text = "Térm. Medio";
            if (serieDos == 10 || serieDos == 11)
                lblRango2Terman.Text = "Térm. M. B.";
            if (serieDos >= 7 && serieDos <= 9)
                lblRango2Terman.Text = "Inferior";
            if (serieDos <= 6)
                lblRango2Terman.Text = "Deficiente";
            #endregion

            #region Serie III
            if (serieTres >= 29)
                lblRango3Terman.Text = "Sobresaliente";
            if (serieTres == 27 || serieTres == 28)
                lblRango3Terman.Text = "Superior";
            if (serieTres >= 23 && serieTres <= 26)
                lblRango3Terman.Text = "Térm. M. A";
            if (serieTres >= 14 && serieTres <= 22)
                lblRango3Terman.Text = "Térm. Medio";
            if (serieTres == 12 || serieTres == 13)
                lblRango3Terman.Text = "Térm. M. B.";
            if (serieTres >= 8 && serieTres <= 11)
                lblRango3Terman.Text = "Inferior";
            if (serieTres <= 7)
                lblRango3Terman.Text = "Deficiente";
            #endregion

            #region Serie IV
            if (serieCuatro >= 18)
                lblRango4Terman.Text = "Sobresaliente";
            if (serieCuatro == 16 || serieCuatro == 17)
                lblRango4Terman.Text = "Superior";
            if (serieCuatro == 14 || serieCuatro == 15)
                lblRango4Terman.Text = "Térm. M. A";
            if (serieCuatro >= 10 && serieCuatro <= 13)
                lblRango4Terman.Text = "Térm. Medio";
            if (serieCuatro >= 7 && serieCuatro <= 9)
                lblRango4Terman.Text = "Térm. M. B.";
            if (serieCuatro == 6)
                lblRango4Terman.Text = "Inferior";
            if (serieCuatro <= 5)
                lblRango4Terman.Text = "Deficiente";
            #endregion

            #region Serie V
            if (serieCinco >= 23)
                lblRango5Terman.Text = "Sobresaliente";
            if (serieCinco >= 19 && serieCinco <= 22)
                lblRango5Terman.Text = "Superior";
            if (serieCinco >= 16 && serieCinco <= 18)
                lblRango5Terman.Text = "Térm. M. A";
            if (serieCinco >= 12 && serieCinco <= 15)
                lblRango5Terman.Text = "Térm. Medio";
            if (serieCinco >= 7 && serieCinco <= 11)
                lblRango5Terman.Text = "Térm. M. B.";
            if (serieCinco == 6)
                lblRango5Terman.Text = "Inferior";
            if (serieCinco <= 5)
                lblRango5Terman.Text = "Deficiente";
            #endregion

            #region Serie VI
            if (serieSeis >= 20)
                lblRango6Terman.Text = "Sobresaliente";
            if (serieSeis == 18 || serieSeis == 19)
                lblRango6Terman.Text = "Superior";
            if (serieSeis >= 15 && serieSeis <= 17)
                lblRango6Terman.Text = "Térm. M. A";
            if (serieSeis >= 9 && serieSeis <= 14)
                lblRango6Terman.Text = "Térm. Medio";
            if (serieSeis == 7 || serieSeis == 8)
                lblRango6Terman.Text = "Térm. M. B.";
            if (serieSeis == 5 || serieSeis == 6)
                lblRango6Terman.Text = "Inferior";
            if (serieSeis <= 4)
                lblRango6Terman.Text = "Deficiente";
            #endregion

            #region Serie VII
            if (serieSiete >= 19)
                lblRango7Terman.Text = "Sobresaliente";
            if (serieSiete == 18)
                lblRango7Terman.Text = "Superior";
            if (serieSiete == 16 || serieSiete == 17)
                lblRango7Terman.Text = "Térm. M. A";
            if (serieSiete >= 9 && serieSiete <= 15)
                lblRango7Terman.Text = "Térm. Medio";
            if (serieSiete >= 6 && serieSiete <= 8)
                lblRango7Terman.Text = "Térm. M. B.";
            if (serieSiete == 5)
                lblRango7Terman.Text = "Inferior";
            if (serieSiete <= 4)
                lblRango7Terman.Text = "Deficiente";
            #endregion

            #region Serie VIII
            if (serieOcho >= 17)
                lblRango8Terman.Text = "Sobresaliente";
            if (serieOcho == 15 || serieOcho == 16)
                lblRango8Terman.Text = "Superior";
            if (serieOcho == 13 || serieOcho == 14)
                lblRango8Terman.Text = "Térm. M. A";
            if (serieOcho >= 8 && serieOcho <= 12)
                lblRango8Terman.Text = "Térm. Medio";
            if (serieOcho == 7)
                lblRango8Terman.Text = "Térm. M. B.";
            if (serieOcho == 6)
                lblRango8Terman.Text = "Inferior";
            if (serieOcho <= 5)
                lblRango8Terman.Text = "Deficiente";
            #endregion

            #region Serie IX
            if (serieNueve >= 18)
                lblRango9Terman.Text = "Sobresaliente";
            if (serieNueve == 17)
                lblRango9Terman.Text = "Superior";
            if (serieNueve == 16)
                lblRango9Terman.Text = "Térm. M. A";
            if (serieNueve >= 10 && serieNueve <= 15)
                lblRango9Terman.Text = "Térm. Medio";
            if (serieNueve == 9)
                lblRango9Terman.Text = "Térm. M. B.";
            if (serieNueve == 7 || serieNueve == 8)
                lblRango9Terman.Text = "Inferior";
            if (serieNueve <= 6)
                lblRango9Terman.Text = "Deficiente";
            #endregion

            #endregion

            double Total = serieUno + serieDos + serieTres + serieCuatro + serieCinco + serieSeis + serieSiete + serieOcho + serieNueve + serieDiez;
            PuntosTerman.Text = Total + "";

            #region Tabla de CI y Rango
            int CI = 0;
            double tmpTotal = double.Parse(PuntosTerman.Text);
            Total = Convert.ToInt32(tmpTotal);
            #region CI
            if (Total < 67)
                ValorCI.Text = Total + "";

            if (Total >= 67 && Total <= 69)
                ValorCI.Text = "80";

            if (Total == 70 || Total == 71)
                ValorCI.Text = "81";

            if (Total >= 72 && Total <= 74)
                ValorCI.Text = "82";

            if (Total == 75 || Total == 76)
                ValorCI.Text = "83";

            if (Total >= 77 && Total <= 80)
                ValorCI.Text = "84";

            if (Total == 81 || Total == 82)
                ValorCI.Text = "85";

            if (Total >= 83 && Total <= 85)
                ValorCI.Text = "86";

            if (Total == 86)
                ValorCI.Text = "87";

            if (Total >= 87 && Total <= 90)
                ValorCI.Text = "88";

            if (Total >= 91 && Total <= 93)
                ValorCI.Text = "89";

            if (Total >= 94 && Total <= 96)
                ValorCI.Text = "90";

            if (Total >= 97 && Total <= 99)
                ValorCI.Text = "91";

            if (Total >= 100 && Total <= 102)
                ValorCI.Text = "92";

            if (Total == 103 || Total == 104)
                ValorCI.Text = "93";

            if (Total == 105 || Total == 106)
                ValorCI.Text = "94";

            if (Total >= 107 && Total <= 110)
                ValorCI.Text = "95";

            if (Total >= 111 && Total <= 113)
                ValorCI.Text = "96";

            if (Total >= 114 && Total <= 117)
                ValorCI.Text = "97";

            if (Total == 118 || Total == 119)
                ValorCI.Text = "98";

            if (Total >= 120 && Total <= 123)
                ValorCI.Text = "99";

            if (Total == 124 || Total == 125)
                ValorCI.Text = "100";

            if (Total >= 126 && Total <= 129)
                ValorCI.Text = "101";

            if (Total >= 130 && Total <= 133)
                ValorCI.Text = "102";

            if (Total >= 134 && Total <= 137)
                ValorCI.Text = "103";

            if (Total >= 138 && Total <= 141)
                ValorCI.Text = "104";

            if (Total >= 142 && Total <= 145)
                ValorCI.Text = "105";

            if (Total >= 146 && Total <= 149)
                ValorCI.Text = "106";

            if (Total >= 150 && Total <= 153)
                ValorCI.Text = "107";

            if (Total >= 154 && Total <= 157)
                ValorCI.Text = "108";

            if (Total == 158 || Total == 159)
                ValorCI.Text = "109";

            if (Total >= 160 && Total <= 162)
                ValorCI.Text = "110";

            if (Total >= 163 && Total <= 166)
                ValorCI.Text = "111";

            if (Total == 167)
                ValorCI.Text = "112";

            if (Total >= 168 && Total <= 170)
                ValorCI.Text = "113";

            if (Total >= 171 && Total <= 173)
                ValorCI.Text = "114";

            if (Total == 174 || Total == 175)
                ValorCI.Text = "115";

            if (Total == 176 || Total == 177)
                ValorCI.Text = "116";

            if (Total >= 178 && Total <= 180)
                ValorCI.Text = "117";

            if (Total >= 181 && Total <= 183)
                ValorCI.Text = "118";

            if (Total == 184 || Total == 185)
                ValorCI.Text = "119";

            if (Total == 186)
                ValorCI.Text = "120";

            if (Total == 187)
                ValorCI.Text = "121";

            if (Total == 188)
                ValorCI.Text = "122";

            if (Total == 189)
                ValorCI.Text = "123";

            if (Total == 190)
                ValorCI.Text = "124";

            if (Total == 191)
                ValorCI.Text = "125";

            if (Total == 192)
                ValorCI.Text = "126";

            if (Total == 193)
                ValorCI.Text = "127";

            if (Total == 194)
                ValorCI.Text = "128";

            if (Total == 195)
                ValorCI.Text = "129";

            if (Total == 196)
                ValorCI.Text = "130";

            if (Total == 197)
                ValorCI.Text = "131";

            if (Total == 198)
                ValorCI.Text = "132";

            if (Total == 199)
                ValorCI.Text = "133";

            if (Total == 200)
                ValorCI.Text = "134";

            if (Total == 201)
                ValorCI.Text = "135";

            if (Total == 202)
                ValorCI.Text = "136";

            if (Total == 203)
                ValorCI.Text = "137";

            if (Total == 204)
                ValorCI.Text = "138";

            if (Total == 205)
                ValorCI.Text = "139";

            if (Total == 206)
                ValorCI.Text = "140";

            if (Total == 207)
                ValorCI.Text = "141";
            #endregion

            #region Rango
            double tmp = double.Parse(ValorCI.Text);
            CI = Convert.ToInt32(tmp);
            if (CI >= 140)
                RangoCI.Text = "Sobresaliente";
            if (CI >= 120 && CI <= 139)
                RangoCI.Text = "Superior";
            if (CI >= 110 && CI <= 119)
                RangoCI.Text = "Térm. M. A";
            if (CI >= 90 && CI <= 109)
                RangoCI.Text = "Normal";
            if (CI >= 80 && CI <= 89)
                RangoCI.Text = "Térm. M. B";
            if (CI >= 70 && CI <= 79)
                RangoCI.Text = "Inferior";
            if (CI <= 69)
                RangoCI.Text = "Deficiente";
            #endregion
            #endregion
        }

        private void VistaResultadoKuder(Dictionary<string, string> plantillas)
        {
            if (plantillas.Count > 0)
            {
                foreach (var item in plantillas)
                {
                    string plantillaSeleccionado = item.Key;
                    string descripcion = string.Empty;
                    string plantilla = string.Empty;
                    string carreras = string.Empty;
                    string salto = Server.HtmlDecode("</br>");

                    switch (plantillaSeleccionado)
                    {
                        case "0 Area Libre":
                            descripcion = @"Altos puntajes en esta área significan que al examinado le gusta pasar la mayor parte del tiempo en, el campo, 
                                            en los bosques o en el mar, le agrada cultivar plantas, cuidar animales, etc. En cambio, no se sentiría muy a gusto en 
                                            una fábrica, en un laboratorio o en una oficina.";

                            salto = Server.HtmlDecode("</br>");

                            carreras = @"Licenciatura en oceanología, Licenciatura en turismo, Profesores de educación física";

                            plantilla = "“0” Interés para el trabajo al aire libre" + salto + carreras;
                            break;
                        case "1 Mecanico":
                            descripcion = @"Un alto puntaje aquí indica interés para trabajar con máquinas y herramientas, construir o arreglar objetos mecánicos, 
                                            artefactos eléctricos, muebles, etc.";

                            salto = Server.HtmlDecode("</br>");
                            carreras = @"Ingeniería en mecatrónica, Licenciatura en radio y televisión, Piloto aviadores, Química Aplicada.";

                            plantilla = "“1” Interés mecánico" + salto + carreras;
                            break;
                        case "2 Calculo":
                            descripcion = @"Lo poseen aquellas personas a quienes les gusta trabajar con números. Muchos ingenieros revelan también un marcado interés 
                                            por las actividades relacionadas con el cálculo.";

                            salto = Server.HtmlDecode("</br>");

                            carreras = @"Enseñanza de las matemáticas,  Ingeniería en computación, Licenciatura en economía.";

                            plantilla = "“2” Interés para el cálculo" + salto + carreras;
                            break;
                        case "3 Cientifico":
                            descripcion = @"Manifiestan este interés las personas que encuentran placer en investigar la razón de los hechos o de las cosas, en 
                                            descubrir sus causas y en resolver problemas de distinta índole, por mera curiosidad científica y sin pensar en los beneficios 
                                            económicos que puedan resultar de sus descubrimientos. El interés científico es de gran importancia en el ejercicio de muchas 
                                            carreras profesionales, aun de aquéllas donde el móvil de la actividad puede ser de índole distinta al progreso de la ciencia.";

                            salto = Server.HtmlDecode("</br>");

                            carreras = @"Antropólogos, Arqueólogos, Licenciatura en nutrición, Médico cirujano.";

                            plantilla = @"“3” Interés científico" + salto + carreras;
                            break;
                        case "4 Persuasivo":
                            descripcion = @"Lo poseen aquellas personas a quienes les gusta tratar con la gente, imponer sus puntos de vista, convencer a los demás 
                                            respecto a algún proyecto, venderles un artículo, etc.";

                            salto = Server.HtmlDecode("</br>");
                            carreras = @"Licenciatura en periodismo, Locución de radio y TV, Licenciatura en ciencias de la comunicación.";

                            plantilla = "“4” Interés persuasivo" + salto + carreras;
                            break;
                        case "5 Artistico":
                            descripcion = @"Lo poseen las personas a quienes les agrada hacer trabajos de creación de tipo manual, usando combinaciones de colores, 
                                            materiales, formas y diseños.";

                            salto = Server.HtmlDecode("</br>");

                            carreras = @"Animación digital, Licenciatura en diseño de productos, Licenciatura en diseño gráfico.";

                            plantilla = "“5” Interes Artístico-plástico" + salto + carreras;
                            break;
                        case "6 Literario":
                            descripcion = @"Es propio de todos aquellos a quienes les gusta la lectura o encuentran placer en expresar sus ideas en forma oral o escrita.";

                            salto = Server.HtmlDecode("</br>");

                            carreras = @"Estudio de idiomas, Licenciatura en derecho, Licenciatura en traducción.";

                            plantilla = "“6” Interés literario" + salto + carreras;
                            break;
                        case "7 Musical":
                            descripcion = @"Se sitúan aquí las personas que muestran un marcado gusto para tocar instrumentos musicales, cantar, bailar, leer sobre 
                                            música, estudiar la vida de compositores famosos, asistir a conciertos, etc.";

                            salto = Server.HtmlDecode("</br>");

                            carreras = @"Artistas de Ballet, Licenciatura en ciencias del arte, Licenciatura en teatro, Profesores de música.";

                            plantilla = "“7” Interés musical" + salto + carreras;
                            break;
                        case "8 Social":
                            descripcion = @"Un alto puntaje en esta área indica un gran interés por servir a los demás: a los necesitados, enfermos, niños y ancianos.";

                            salto = Server.HtmlDecode("</br>");

                            carreras = @"Cirujano dentista, Fisioterapia y rehabilitación, Licenciatura en desarrollo comunitario.";

                            plantilla = "“8” Interés para el servicio social" + salto + carreras;
                            break;
                        case "9 Oficina":
                            descripcion = @"Es propio de las personas a quienes les gusta un tipo de trabajo de escritorio, que requiere exactitud y precisión.";

                            salto = Server.HtmlDecode("</br>");

                            carreras = @"Estudios en telemática, Licenciatura en bibliotecología, Psicología organizacional.";

                            plantilla = "“9” Interés en el trabajo de oficina" + salto + carreras;
                            break;
                    }
                    if (string.IsNullOrEmpty(lblplantilla1.Text) && string.IsNullOrEmpty(lbldescripcion1.Text))
                    {
                        lblplantilla1.Text = plantilla;
                        lbldescripcion1.Text = descripcion;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(lblplantilla2.Text) && string.IsNullOrEmpty(lbldescripcion2.Text))
                    {
                        lblplantilla2.Text = plantilla;
                        lbldescripcion2.Text = descripcion;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(lblplantilla3.Text) && string.IsNullOrEmpty(lbldescripcion3.Text))
                    {
                        lblplantilla3.Text = plantilla;
                        lbldescripcion3.Text = descripcion;
                        continue;
                    }
                }
            }
        }

        private void VistaResultadoAllport(Dictionary<string, string> plantillas)
        {
            if (plantillas.Count > 0)
            {
                foreach (var item in plantillas)
                {
                    string plantilla = item.Key;
                    string valor = string.Empty;
                    string meta = string.Empty;

                    switch (plantilla)
                    {
                        case "1 Teorica":
                            valor = "Alto Valor Teórico - Búsqueda de la Verdad: ";
                            meta = "El interés dominante de esta persona tiende a ser la búsqueda del conocimiento. Innatamente curioso, está interesado en el aprendizaje y el "
                                 + "proceso del razonamiento. Tendiente a intelectualizar o teorizar, esta persona puede tratar de arreglar las ideas o la información en base a "
                                 + "sus análisis o en base a sistemas lógicos. Más que aceptar las cosas tal cual son, tiende a ser inquisitivo y crítico. Trata de organizar y "
                                 + "sistematizar el conocimiento con investigación y validación. Estudioso tanto como creativo, es una buena fuente de información y de ideas.";
                            break;
                        case "2 Economico":
                            valor = "Alto Valor Económico - Sentido de Negocio y Búsqueda de Bienestar";
                            meta = "Tiende a ser impulsado por un abundante interés en el dinero, las utilidades y la posición económica. Ve todas las cosas e ideas en su "
                                 + "ambiente como parte de una estructura materialista. Esta persona tiende a ser práctico y darle un valor monetario a todo. Siempre está "
                                 + "buscando utilidades o rendimientos sobre la inversión. Tiene un deseo de ganancias materiales personales y, en el sentido empresarial, "
                                 + "aprecia los resultados positivos de un Centro de Utilidades. Asimismo, respeta los logros económicos de la gente que está muy consciente de "
                                 + "las utilidades o de los costos. Es adquisitivo y con frecuencia competitivamente, quiere que los demás sepan que sus posesiones tienen un "
                                 + "valor en el mercado.";
                            break;
                        case "5 Estetico":
                            valor = "Alto Valor Artístico - Apreciación de la Belleza";
                            meta = "Esta persona tiende a buscar la realización artística en las áreas de expresión cultural. Se siente enormemente atraído por las formas, "
                                 + "la armonía, la gracia y la simetría. Sus intereses pueden variar desde la música hasta las bellas artes y la naturaleza. Las percepciones de "
                                 + "esta persona pueden variar desde individualistas hasta “vanguardistas”. Es probable que sea un perfeccionista con respecto al diseño, los "
                                 + "colores y los detalles. Asimismo, exige libertad para crear lo “suyo”. Su clara sensibilidad hacia lo bello puede, por otro lado, ir "
                                 + "acompañada de una intolerancia hacia lo feo. Puede en ocasiones argumentar la falta de belleza o falta de sensibilidad artística en algunas "
                                 + "tareas que le sean presentadas por otras personas.";
                            break;
                        case "4 Social":
                            valor = "Alto Valor Social - Preocupación por la Gente";
                            meta = "Esta persona tiende a tener buenos sentimientos hacia toda la gente por igual, sean propios o extraños. Desinteresadamente intenta mejorar "
                                 + "el bienestar de los demás. Su fuerte interés humanitario y su sentido de justicia social son lo que mueve a esta persona a actuar."
                                 + "\n" +
                                 "En asuntos donde se trate asuntos sobre el bienestar de gente, esta persona puede emitir juicios subjetivos, emocionales o idealistas. Su "
                                 + "interés humano lo puede llevar a tener diferencias con gente económicamente motivada o en ocasiones pudiera pasar por alto controles de "
                                 + "costos o presupuestos, con tal de ayudar a la gente.";
                            break;
                        case "3 Politico":
                            valor = "Alto Valor Político - Búsqueda de la Autoridad y del Status";
                            meta = "Tiende a buscar status y poder en la sociedad o en la Organización. Es una persona que tiene ambiciones en la vida y que busca oportunidades "
                                 + "que le brinden proyección en la Organización y oportunidades de avanzar en la jerarquía organizacional. Esta persona le gusta tener gente "
                                 + "bajo su responsabilidad, ya que le gusta tener influencia sobre otras personas. Le emocionan, más que apenarlo los reconocimientos personales. "
                                 + "Es una persona que está dispuesto a pagar el precio personal con tal de obtener ascensos, reconocimiento y puestos desde donde pueda "
                                 + "controlar.";
                            break;
                        case "6 Religioso":
                            valor = "Alto Valor Regulatorio - Respeto a la Autoridad";
                            meta = "Tiende a identificarse con una fuerza del bien y a gobernar su bien organizada vida según un código de conducta muy personal. Es una persona "
                                 + "que desea orden y unidad en todo lo que hace. Lo “bueno” y lo “malo” con respecto a sus estándares de conducta son muy observados por esta "
                                 + "persona, ya que siempre busca que se actúe “correctamente”. Generalmente, es disciplinado, controlado y observa los estándares establecidos. "
                                 + "Es una persona que conduce su vida, ganándose el respeto de quienes lo rodean.";
                            break;
                    }
                    if (string.IsNullOrEmpty(lblplantillaAll1.Text) && string.IsNullOrEmpty(lblObjetivoAll1.Text))
                    {
                        lblplantillaAll1.Text = valor;
                        lblObjetivoAll1.Text = meta;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(lblplantillaAll2.Text) && string.IsNullOrEmpty(lblObjetivoAll2.Text))
                    {
                        lblplantillaAll2.Text = valor;
                        lblObjetivoAll2.Text = meta;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(lblplantillaAll3.Text) && string.IsNullOrEmpty(lblObjetivoAll3.Text))
                    {
                        lblplantillaAll3.Text = valor;
                        lblObjetivoAll3.Text = meta;
                        continue;
                    }
                }
            }
        }

        private void VistaResultadoSacks(SumarioGeneralSacks sumarioGeneralSacks)
        {
            if (sumarioGeneralSacks.SumarioGeneralSACKSID != null)
            {
                lblMadurez.Text = sumarioGeneralSacks.SumarioMadurez;
                lblNivelRealidad.Text = sumarioGeneralSacks.SumarioNivelRealida;
                lblConflictos.Text = sumarioGeneralSacks.SumarioConflictoExpresados;
            }
        }

        private void VistaResultadoCleaver(ResultadoPruebaCleaver serieMore, ResultadoPruebaCleaver serieLess, ResultadoPruebaCleaver serieTotal)
        {
            if (serieMore != null && serieLess != null && serieTotal != null)
            {
                #region Características
                #region More
                if (serieMore.Resultado_D > 55 || serieLess.Resultado_D > 55 || serieTotal.Resultado_D > 55)
                    trDMore.Visible = true;
                if (serieMore.Resultado_I > 55 || serieLess.Resultado_I > 55 || serieTotal.Resultado_I > 55)
                    trIMore.Visible = true;
                if (serieMore.Resultado_S > 55 || serieLess.Resultado_S > 55 || serieTotal.Resultado_S > 55)
                    trSMore.Visible = true;
                if (serieMore.Resultado_C > 55 || serieLess.Resultado_C > 55 || serieTotal.Resultado_C > 55)
                    trCMore.Visible = true;
                #endregion

                #region Less
                if (serieMore.Resultado_D < 45 || serieLess.Resultado_D < 45 || serieTotal.Resultado_D < 45)
                    trDLess.Visible = true;
                if (serieMore.Resultado_I < 45 || serieLess.Resultado_I < 45 || serieTotal.Resultado_I < 45)
                    trILess.Visible = true;
                if (serieMore.Resultado_S < 45 || serieLess.Resultado_S < 45 || serieTotal.Resultado_S < 45)
                    trSLess.Visible = true;
                if (serieMore.Resultado_C < 45 || serieLess.Resultado_C < 45 || serieTotal.Resultado_C < 45)
                    trCLess.Visible = true;
                #endregion
                #endregion
            }
        }

        private void VistaResultadoChaside(DataSet resultadoChasideInteres, DataSet resultadoChasideAptitud)
        {
            if (resultadoChasideInteres.Tables[0].Rows.Count > 0 && resultadoChasideAptitud.Tables[0].Rows.Count > 0)
            {
                #region Intereses
                ResultadoCHASIDE resIntereses = new ResultadoCHASIDE();
                int cant = 0;
                ResultadoPruebaChaside result;
                cant = 0;
                foreach (DataRow row in resultadoChasideInteres.Tables[0].Rows)
                {
                    cant++;
                    result = resultadoPruebaDinamicaCtrl.DataRowToResultadoPruebaChaside(row);
                    switch (result.Nombre)
                    {
                        case "C":
                            resIntereses = Percentil_C(1);
                            break;
                        case "H":
                            resIntereses = Percentil_H(1);
                            break;
                        case "A":
                            resIntereses = Percentil_A(1);
                            break;
                        case "S":
                            resIntereses = Percentil_S(1);
                            break;
                        case "I":
                            resIntereses = Percentil_I(1);
                            break;
                        case "D":
                            resIntereses = Percentil_D(1);
                            break;
                        case "E":
                            resIntereses = Percentil_E(1);
                            break;
                    }
                    if (cant < 2)
                    {
                        lblIntLetraUno.Text = resIntereses.Percentil;
                        lblIntCarreraUno.Text = resIntereses.Descripcion;
                        lblIntDescripcionUno.Text = resIntereses.AptitudInteres;
                    }
                    else
                    {
                        lblIntLetraDos.Text = resIntereses.Percentil;
                        lblIntCarreraDos.Text = resIntereses.Descripcion;
                        lblIntDescripcionDos.Text = resIntereses.AptitudInteres;
                    }
                }
                #endregion

                #region Resultado para Aptitudes
                ResultadoCHASIDE resAptitudes = new ResultadoCHASIDE();
                cant = 0;
                foreach (DataRow row in resultadoChasideAptitud.Tables[0].Rows)
                {
                    cant++;
                    result = resultadoPruebaDinamicaCtrl.DataRowToResultadoPruebaChaside(row);
                    switch (result.Nombre)
                    {
                        case "C":
                            resAptitudes = Percentil_C(2);
                            break;
                        case "H":
                            resAptitudes = Percentil_H(2);
                            break;
                        case "A":
                            resAptitudes = Percentil_A(2);
                            break;
                        case "S":
                            resAptitudes = Percentil_S(2);
                            break;
                        case "I":
                            resAptitudes = Percentil_I(2);
                            break;
                        case "D":
                            resAptitudes = Percentil_D(2);
                            break;
                        case "E":
                            resAptitudes = Percentil_E(2);
                            break;
                    }
                    if (cant < 2)
                    {
                        lblAptLetraUno.Text = resAptitudes.Percentil;
                        lblAptCarreraUno.Text = resAptitudes.Descripcion;
                        lblAptDescripcionUno.Text = resAptitudes.AptitudInteres;
                    }
                    else
                    {
                        lblAptLetraDos.Text = resAptitudes.Percentil;
                        lblAptCarreraDos.Text = resAptitudes.Descripcion;
                        lblAptDescripcionDos.Text = resAptitudes.AptitudInteres;
                    }
                }
                #endregion
            }
        }

        #region Precentiles Chaside
        public ResultadoCHASIDE Percentil_C(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "C";
            obj.Descripcion = "Carreras de las áreas administrivas y contables";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Organizativo\n" +
                        "  - Supervisión\n" +
                        "  - Orden\n" +
                        "  - Análisis y sintesis\n" +
                        "  - Colaboración\n" +
                        "  - Cálculo\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Persuasivo\n" +
                        "  - Objetivo\n" +
                        "  - Práctico\n" +
                        "  - Tolerante\n" +
                        "  - Responsable\n" +
                        "  - Ambicioso\n";
                    break;
            }
            return obj;
        }
        public ResultadoCHASIDE Percentil_H(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "H";
            obj.Descripcion = "Carreras de las áreas humanisticas y sociales";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Precisión verbal\n" +
                        "  - Organización\n" +
                        "  - Relación de hechos\n" +
                        "  - Lingüistica\n" +
                        "  - Orden\n" +
                        "  - Justicia\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Responsable\n" +
                        "  - Justo\n" +
                        "  - Conciliador\n" +
                        "  - Persuasivo\n" +
                        "  - Sagaz\n" +
                        "  - Imaginativo\n";
                    break;
            }
            return obj;
        }
        public ResultadoCHASIDE Percentil_A(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "A";
            obj.Descripcion = "Carreras de las áreas Artisticas";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Estético\n" +
                        "  - Armónico\n" +
                        "  - Manual\n" +
                        "  - Visual\n" +
                        "  - Auditivo\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Sensible\n" +
                        "  - Imaginativo\n" +
                        "  - Creativo\n" +
                        "  - Detallista\n" +
                        "  - Innovador\n" +
                        "  - Intuitivo\n";
                    break;
            }
            return obj;
        }
        public ResultadoCHASIDE Percentil_S(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "S";
            obj.Descripcion = "Carreras de las áreas de Medicina y Ciencias de la salud";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Asistir\n" +
                        "  - Investigativo\n" +
                        "  - Precisión\n" +
                        "  - Senso-Perceptivo\n" +
                        "  - Analítio\n" +
                        "  - Ayudar\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Altruista\n" +
                        "  - Solidario\n" +
                        "  - Paciente\n" +
                        "  - Comprensivo\n" +
                        "  - Respetuoso\n" +
                        "  - Persuasivo\n";
                    break;
            }
            return obj;
        }
        public ResultadoCHASIDE Percentil_I(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "I";
            obj.Descripcion = "Carreras de las áreas de Ingeniería y computación";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Cálculo\n" +
                        "  - Científico\n" +
                        "  - Manual\n" +
                        "  - Exacto\n" +
                        "  - Planificar\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Preciso\n" +
                        "  - Práctivo\n" +
                        "  - Crítico\n" +
                        "  - Analítico\n" +
                        "  - Rígido\n";
                    break;
            }
            return obj;
        }
        public ResultadoCHASIDE Percentil_D(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "D";
            obj.Descripcion = "Carreras de las áreas de Defensa y Seguridad";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Justicia\n" +
                        "  - Equidad\n" +
                        "  - Colaboración\n" +
                        "  - Espíritu de equipo\n" +
                        "  - Liderazgo\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Arriesgado\n" +
                        "  - Solidario\n" +
                        "  - Valiente\n" +
                        "  - Agresivo\n" +
                        "  - Persuasivo\n";
                    break;
            }
            return obj;
        }
        public ResultadoCHASIDE Percentil_E(int tipo)
        {
            ResultadoCHASIDE obj = new ResultadoCHASIDE();
            obj.Percentil = "E";
            obj.Descripcion = "Carreras de las áreas de Ciencias exactas y Agrarias";
            switch (tipo)
            {
                case 1:
                    obj.AptitudInteres = "Las carreras profesionales en esta área de tu interés se caracterizan por: \n" +
                        "  - Investigación\n" +
                        "  - Orden\n" +
                        "  - Organizacion\n" +
                        "  - Analísis y sintesis\n" +
                        "  - Numérico\n" +
                        "  - Clasificar\n";
                    break;
                case 2:
                    obj.AptitudInteres = "Para las carreras de esta área, tu tienes las siguientes aptitudes: \n" +
                        "  - Metódico\n" +
                        "  - Analítico\n" +
                        "  - Observador\n" +
                        "  - Introvertido\n" +
                        "  - PAciente\n" +
                        "  - Seguro\n";
                    break;
            }
            return obj;
        }

        public class ResultadoCHASIDE
        {
            public string Percentil { get; set; }
            public String Descripcion { get; set; }
            public String AptitudInteres { get; set; }
        }
        #endregion

        private void VistaResultadoRotter(int resultadoRotter)
        {
            string locusCapital = string.Empty;
            string locusIndice = string.Empty;
            string locusTexto = string.Empty;
            string locusDescripcion = string.Empty;
            string salto = Server.HtmlDecode("</br>");

            if (resultadoRotter >= 0 && resultadoRotter <= 11)
            {
                locusCapital = "I";
                locusTexto = "Internalidad del Locus de Control";
                locusDescripcion = "Los individuos con un locus de control interno creen que los eventos son el resultado principalmente de su propia conducta y de sus " +
                                          "propias acciones, por lo que hace que se perciban como capaces de influir en su propio destino, de transformar una situación " +
                                          "desfavorable, o de incrementar su probabilidad de éxito. La percepción de control sobre la situación aumenta la motivación para " +
                                          "enfrentarla, de esta manera se espera que las personas con un locus de control interno se sientan más comprometidas, y actúen de " +
                                          "forma más activa ante la situación."
                                          + salto +
                                          "Estar convencido de la posibilidad de afectar el funcionamiento del mundo a nuestro alrededor (locus de control interno) ha " +
                                          "demostrado ser un elemento positivo en el rendimiento educativo, el campo laboral  y la prevención de la salud."
                                          + salto +
                                          "En el campo de las relaciones interpersonales, estas personas tienen un buen control de su comportamiento y tienden a expresarse " +
                                          "mejor socialmente.";
            }
            else if (resultadoRotter >= 12)
            {
                locusCapital = "E";
                locusTexto = "Externalidad del Locus de control";
                locusDescripcion = "Aquellos con un locus de control externo creen que son las otras personas (o situaciones) quienes determinan el rumbo de los " +
                                          "eventos. Este tipo de personas apreciarían que los resultados de sus conductas obedecen a factores ajenos a su control, como la " +
                                          "suerte, el destino o la participación de otras personas, no reconociendo en ellas mismas la capacidad de afectar el curso de los " +
                                          "eventos y de influir mediante sus acciones en el control de las contingencias de refuerzo que seguirán a su comportamiento."
                                          + salto +
                                          "Las personas con un locus de control externo pueden funcionar muy bien en el campo de lo laboral, incluso con las metas adecuadas " +
                                          "y los estímulos adecuados en el medio, pueden llegar a niveles muy elevados."
                                          + salto +
                                          "En el plano de las relaciones interpersonales, aquellos con un locus de control externo es más probable que intenten influir en " +
                                          "las personas y mantener cercanía (incluso dependencia), ya que son las personas las que ofrecen los refuerzos que la persona " +
                                          "requiere.";
            }

            if (resultadoRotter >= 0 && resultadoRotter <= 5)
            {
                locusIndice = "A";
            }
            else if (resultadoRotter >= 6 && resultadoRotter <= 9)
            {
                locusIndice = "M";
            }
            else if (resultadoRotter >= 10 && resultadoRotter <= 11)
            {
                locusIndice = "B";
            }
            else if (resultadoRotter >= 12 && resultadoRotter <= 14)
            {
                locusIndice = "B";
            }
            else if (resultadoRotter >= 15 && resultadoRotter <= 18)
            {
                locusIndice = "M";
            }
            else if (resultadoRotter >= 19 && resultadoRotter <= 23)
            {
                locusIndice = "A";
            }

            lblLocusCapital.Text = locusCapital;
            lblLocusIndice.Text = locusIndice;
            lblLocusTexto.Text = locusTexto;
            lblLocusDescripcion.Text = locusDescripcion;
        }

        private void VistaResultadoRaven(DiagnosticoRaven diagnostico, string fechaAplicacion)
        {
            if (diagnostico != null)
            {
                lblEdadRaven.Text = diagnostico.Edad + " años";
                lblRavenPercentil.Text = diagnostico.Percentil.ToString();
                lblRavenRango.Text = diagnostico.Rango;
                lblRavenDiagnostico.Text = "Capacidad Intelectual " + diagnostico.Diagnostico;

                if (diagnostico.Validez)
                {
                    lblValidezRaven.Text = "Prueba válida";
                    lblValidezRaven.Style.Value = "color: #fff !important;";
                    tdvalidezraben.Style.Value = "background-color: #17DA0A; color: #fff !important; text-align:center;";
                    lblRavenDiscrepancia.Text = "SI";
                }
                else
                {
                    lblValidezRaven.Text = "Riesgo en la validez";
                    lblValidezRaven.Style.Value = "color: #fff !important;";
                    tdvalidezraben.Style.Value = "background-color: #ff9e19; color: #fff !important; text-align:center;";
                    lblRavenDiscrepancia.Text = "NO";
                }

                #region Obtencion Discrepancias
                #region Puntuacion Directa
                if (diagnostico.PuntuacionDirecta != null || diagnostico.PuntuacionDirecta.Count > 0)
                {
                    foreach (var item in diagnostico.PuntuacionDirecta)
                    {
                        switch (item.Key)
                        {
                            case "A":
                                lblRavenpDirectaA.Text = item.Value.ToString();
                                break;
                            case "B":
                                lblRavenpDirectaB.Text = item.Value.ToString();
                                break;
                            case "C":
                                lblRavenpDirectaC.Text = item.Value.ToString();
                                break;
                            case "D":
                                lblRavenpDirectaD.Text = item.Value.ToString();
                                break;
                            case "E":
                                lblRavenpDirectaE.Text = item.Value.ToString();
                                break;
                            case "Total":
                                lblRavenTotal.Text = item.Value.ToString();
                                lblRavenPuntaje.Text = item.Value.ToString();
                                break;
                        }
                    }
                }
                #endregion
                #region Puntuacion Esperada
                if (diagnostico.PuntuacionEsperada != null || diagnostico.PuntuacionEsperada.Count > 0)
                {
                    foreach (var item in diagnostico.PuntuacionEsperada)
                    {
                        switch (item.Key)
                        {
                            case "A":
                                lblRavenpEsperadaA.Text = item.Value.ToString();
                                break;
                            case "B":
                                lblRavenpEsperadaB.Text = item.Value.ToString();
                                break;
                            case "C":
                                lblRavenpEsperadaC.Text = item.Value.ToString();
                                break;
                            case "D":
                                lblRavenpEsperadaD.Text = item.Value.ToString();
                                break;
                            case "E":
                                lblRavenpEsperadaE.Text = item.Value.ToString();
                                break;
                        }
                    }
                }
                #endregion
                #region Discrepacias
                if (diagnostico.Discrepancias != null || diagnostico.Discrepancias.Count > 0)
                {
                    foreach (var item in diagnostico.Discrepancias)
                    {
                        switch (item.Key)
                        {
                            case "A":
                                lblRavenDiscrepanciaA.Text = item.Value.ToString();
                                break;
                            case "B":
                                lblRavenDiscrepanciaB.Text = item.Value.ToString();
                                break;
                            case "C":
                                lblRavenDiscrepanciaC.Text = item.Value.ToString();
                                break;
                            case "D":
                                lblRavenDiscrepanciaD.Text = item.Value.ToString();
                                break;
                            case "E":
                                lblRavenDiscrepanciaE.Text = item.Value.ToString();
                                break;
                        }
                    }
                }
                #endregion
                #endregion
            }
        }

        private void VistaResultadoFrasesVocacionales(SumarioGeneralFrasesVocacionales sumarioGeneralFrasesVocacionales)
        {
            if (sumarioGeneralFrasesVocacionales.SumarioGeneralFrasesID != null)
            {
                lblOrganizacionPersonalidad.Text = sumarioGeneralFrasesVocacionales.SumarioOrganizacionPersonalidad;
                lblPerspectivaOpciones.Text = sumarioGeneralFrasesVocacionales.SumarioPerspectivaOpciones;
                lblFuentesConflicto.Text = sumarioGeneralFrasesVocacionales.SumarioFuentesConflicto;
            }
        }

        private void VistaResultadoZavic(Dictionary<string, string> resultadoZavic)
        {
            if (resultadoZavic.Count > 0 || resultadoZavic != null)
            {
                foreach (var item in resultadoZavic)
                {
                    switch (item.Key)
                    {
                        #region Valores
                        case "Moral":
                            lblZavicMoral.Text = item.Value;
                            break;
                        case "Legalidad":
                            lblZavicLegalidad.Text = item.Value;
                            break;
                        case "Indiferencia":
                            lblZavicIndiferencia.Text = item.Value;
                            break;
                        case "Corrupto":
                            lblZavicCorrupto.Text = item.Value;
                            break;
                        #endregion
                        #region Intereses
                        case "Económico":
                            lblZavicEconomico.Text = item.Value;
                            break;
                        case "Político":
                            lblZavicPolitico.Text = item.Value;
                            break;
                        case "Social":
                            lblZavicSocial.Text = item.Value;
                            break;
                        case "Religioso":
                            lblZavicReligioso.Text = item.Value;
                            break;
                        #endregion
                    }
                }
            }
        }

        public void VistaResultadoMultiples(Decimal sumaCalificacion)
        {
            divTablaResultadosMultiples.Visible = true;
            string backColor = "background-color: #33acfd; color: #fff !important;";


            Alumno alumnoEstudiante = new Alumno();
            alumnoEstudiante.AlumnoID = long.Parse(QS_Alumno);

            //Nos conectamos a la base de datos
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["POV"].ConnectionString);

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            //Consulta hacia la base
            sqlCommand.CommandText = "SELECT * FROM [dbo].[ViewResultadoPruebaInteligenciasMultiples] WHERE AlumnoID='" + alumnoEstudiante.AlumnoID + "'";
            //Se abre la conexión a la base de datos
            sqlConnection.Open();
            var dataReader = sqlCommand.ExecuteReader();

            #region Variables auxilires para almacenar los resutados de cada reactivo

            //Inteligencia Verbal
            int ReactivoMultiples09 = 0;
            int ReactivoMultiples10 = 0;
            int ReactivoMultiples17 = 0;
            int ReactivoMultiples22 = 0;
            int ReactivoMultiples30 = 0;
            int sumaIntVerbal = 0;

            //Inteligencia Logico - Matemática
            int ReactivoMultiples05 = 0;
            int ReactivoMultiples07 = 0;
            int ReactivoMultiples15 = 0;
            int ReactivoMultiples20 = 0;
            int ReactivoMultiples25 = 0;
            int sumaIntLogicomatematica = 0;

            //Inteligencia Visual Espacial
            int ReactivoMultiples01 = 0;
            int ReactivoMultiples11 = 0;
            int ReactivoMultiples14 = 0;
            int ReactivoMultiples23 = 0;
            int ReactivoMultiples27 = 0;
            int sumaIntVisual = 0;

            //Inteligencia Kinestesica
            int ReactivoMultiples08 = 0;
            int ReactivoMultiples16 = 0;
            int ReactivoMultiples19 = 0;
            int ReactivoMultiples21 = 0;
            int ReactivoMultiples29 = 0;
            int sumaIntKinestesica = 0;

            //Inteligencia Musical
            int ReactivoMultiples03 = 0;
            int ReactivoMultiples04 = 0;
            int ReactivoMultiples13 = 0;
            int ReactivoMultiples24 = 0;
            int ReactivoMultiples28 = 0;
            int sumaIntMusical = 0;

            //Inteligecia Intrapersonal
            int ReactivoMultiples02 = 0;
            int ReactivoMultiples06 = 0;
            int ReactivoMultiples26 = 0;
            int ReactivoMultiples31 = 0;
            int ReactivoMultiples33 = 0;
            int sumaIntIntrapersonal = 0;

            //Inteligencia Interpersonal
            int ReactivoMultiples12 = 0;
            int ReactivoMultiples18 = 0;
            int ReactivoMultiples32 = 0;
            int ReactivoMultiples34 = 0;
            int ReactivoMultiples35 = 0;
            int sumaIntInterpersonal = 0;

            #endregion

            #region Lectura de registros de la base de datos
            //Ciclo para leer los datos de la tabla 
            while (dataReader.Read())
            {
                //Se compara con el valor del la columna ClasificadorID que esta en la base de datos
                    if ((int)dataReader[5] == 156)
                    {
                        //Se lee y se compara el valor de la columna NombreReactivo que esta en la base de datos
                        //con cada valor de esta misma columna
                        if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.10")
                        {
                           //El valor almacenado de la columna Valor se guarda en una variable auxiliar
                           // para poder trabajarla después
                            ReactivoMultiples10 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.22")
                        {
                           
                            ReactivoMultiples22 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.30")
                        {
                           
                            ReactivoMultiples30 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.09")
                        {
                           
                            ReactivoMultiples09 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.17")
                        {
                            
                            ReactivoMultiples17 = System.Convert.ToInt32(dataReader["Valor"]);
                        }

                        //Se suman los valores guardados de todas las variables auxiliares y se guarda en una sola  variable
                        sumaIntVerbal = ReactivoMultiples10 + ReactivoMultiples22 + ReactivoMultiples30 + ReactivoMultiples09 + ReactivoMultiples17;

                    }
                    else if ((int)dataReader[5] == 157)
                    {
                        //Mismo procedimiento descrito anteriormente 
                        if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.20")
                        {
                            //Mismo procedimiento descrito anteriormente
                            
                            ReactivoMultiples20 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.05")
                        {
                          
                            ReactivoMultiples05 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.07")
                        {
                           
                            ReactivoMultiples07 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.15")
                        {
                        
                            ReactivoMultiples15 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.25")
                        {
                           
                            ReactivoMultiples25 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        //Mismo procedimiento descrito anteriormente
                        sumaIntLogicomatematica = ReactivoMultiples20 + ReactivoMultiples05 + ReactivoMultiples07 + ReactivoMultiples15 + ReactivoMultiples25;

                    }
                    else if ((int)dataReader[5] == 158)
                    {
                        //Mismo procedimiento descrito anteriormente
                        if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.14")
                        {
                            //Mismo procedimiento descrito anteriormente
                            
                            ReactivoMultiples14 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.01")
                        {
                       
                            ReactivoMultiples01 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.11")
                        {
                          
                            ReactivoMultiples11 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.23")
                        {
                        
                            ReactivoMultiples23 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.27")
                        {
                        
                            ReactivoMultiples27 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        //Mismo procedimiento descrito anteriormente
                        sumaIntVisual = ReactivoMultiples14 + ReactivoMultiples01 + ReactivoMultiples11 + ReactivoMultiples23 + ReactivoMultiples27;

                    }
                    else if ((int)dataReader[5] == 159)
                    {
                        //Mismo procedimiento descrito anteriormente
                        if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.08")
                        {
                            //Mismo procedimiento descrito anteriormente
                            ReactivoMultiples08 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.16")
                        {
                          
                            ReactivoMultiples16 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.19")
                        {
                           
                            ReactivoMultiples19 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.21")
                        {
                    
                            ReactivoMultiples21 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.29")
                        {
                            
                            ReactivoMultiples29 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        //Mismo procedimiento descrito anteriormente
                        sumaIntKinestesica = ReactivoMultiples08 + ReactivoMultiples16 + ReactivoMultiples19 + ReactivoMultiples21 + ReactivoMultiples29;
                    }
                    else if ((int)dataReader[5] == 160)
                    {
                        //Mismo procedimiento descrito anteriormente
                        if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.04")
                        {
                            //Mismo procedimiento descrito anteriormente
                            ReactivoMultiples04 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.24")
                        {
                           
                            ReactivoMultiples24 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.28")
                        {
                            
                            ReactivoMultiples28 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.03")
                        {
                            
                            ReactivoMultiples03 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.13")
                        {
                           
                            ReactivoMultiples13 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        //Mismo procedimiento descrito anteriormente
                        sumaIntMusical = ReactivoMultiples04 + ReactivoMultiples24 + ReactivoMultiples28 + ReactivoMultiples03 + ReactivoMultiples13;

                    }
                    else if ((int)dataReader[5] == 161)
                    {
                        //Mismo procedimiento descrito anteriormente
                        if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.02")
                        {
                            //Mismo procedimiento descrito anteriormente
                            ReactivoMultiples02 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.06")
                        {
                            
                            ReactivoMultiples06 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.26")
                        {
                           
                            ReactivoMultiples26 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.31")
                        {
                           
                            ReactivoMultiples31 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.33")
                        {
                            
                            ReactivoMultiples33 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        //Mismo procedimiento descrito anteriormente
                        sumaIntIntrapersonal = ReactivoMultiples02 + ReactivoMultiples06 + ReactivoMultiples26 + ReactivoMultiples31 + ReactivoMultiples33;

                    }
                    else if ((int)dataReader[5] == 162)
                    {
                        //Mismo procedimiento descrito anteriormente
                        if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.12")
                        {
                            //Mismo procedimiento descrito anteriormente
                            ReactivoMultiples12 = System.Convert.ToInt32(dataReader["Valor"]);

                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.18")
                        {
                          
                            ReactivoMultiples18 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.32")
                        {
                           
                            ReactivoMultiples32 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.34")
                        {
                           
                            ReactivoMultiples34 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        else if ((string)dataReader["NombreReactivo"] == "InteligenciasMultiples01.35")
                        {
                            
                            ReactivoMultiples35 = System.Convert.ToInt32(dataReader["Valor"]);
                        }
                        //Mismo procedimiento descrito anteriormente
                        sumaIntInterpersonal = ReactivoMultiples12 + ReactivoMultiples18 + ReactivoMultiples32 + ReactivoMultiples34 + ReactivoMultiples35;

                    }

            }

            #endregion

            #region colorear inteligencias sobresalientes
            //Si el valor almacenado en la variable sumaIntVerbal, sumaIntLogicoMatematica, etc
            //es igual o mayor a 3, entonces se trata de una inteligencia destacada y 
            //se procede a coloear su respectiva fila en la tabla que aparece en el historial de servicios
            //de cada estudiante en el panel Orientador
            if (sumaIntVerbal >= 3)
            {
                tdVerbal.Style.Value = backColor;
                tdIntVerbal.Style.Value = backColor;

            } if (sumaIntLogicomatematica >= 4)
            {
                tdLogicoMatematica.Style.Value = backColor;
                tdIntLogicoMatematica.Style.Value = backColor;

            } if (sumaIntVisual >= 3)
            {
                tdVisualEspacial.Style.Value = backColor;
                tdIntVisualEspacial.Style.Value = backColor;

            } if (sumaIntKinestesica >= 3)
            {
                tdKinestesica.Style.Value = backColor;
                tdIntKinestesica.Style.Value = backColor;

            } if (sumaIntMusical >= 3)
            {
                tdMusical.Style.Value = backColor;
                tdIntMusical.Style.Value = backColor;

            } if (sumaIntIntrapersonal >= 3)
            {
                tdIntrapersonal.Style.Value = backColor;
                tdIntIntrapersonal.Style.Value = backColor;

            } if (sumaIntInterpersonal >= 3)
            {
                tdInterpersonal.Style.Value = backColor;
                tdIntInterpersonal.Style.Value = backColor;
            }

            #endregion

            //Cerramos la conexión a la base de datos
            sqlConnection.Close();



            }




        }

  
        #endregion
    }
//}