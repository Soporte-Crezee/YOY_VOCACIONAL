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
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalAlumno.Reportes
{
    public partial class ReporteAlumnoResultadoCleaver : System.Web.UI.Page
    {
        #region Propiedades de la clase
        private Boolean SS_PruebaRealizada
        {
            get { return (Boolean)this.Session["PruebaRealizada"]; }
            set { this.Session["PruebaRealizada"] = value; }
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

        private string SS_FechaFin
        {
            get { return (string)this.Session["FechaFin"]; }
            set { this.Session["FechaFin"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;

        ResultadoPruebaCleaver porcentajeMas;
        ResultadoPruebaCleaver porcentajeMenos;
        ResultadoPruebaCleaver porcentajeTotal;

        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        private AlumnoCtrl alumnoCtrl;
        private UsuarioCtrl usuarioCtrl;

        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        #endregion

        public ReporteAlumnoResultadoCleaver()
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
                    porcentajeMas = new ResultadoPruebaCleaver();
                    porcentajeMenos = new ResultadoPruebaCleaver();
                    porcentajeTotal = new ResultadoPruebaCleaver();
                    SS_PruebaRealizada = false;
                    SS_AlumnoCarga = null;
                    SS_UsuarioCarga = null;
                    SS_FechaFin = string.Empty;
                    CargarDatos();
                    if (SS_PruebaRealizada == false || SS_AlumnoCarga == null || SS_UsuarioCarga == null || SS_FechaFin == string.Empty)
                        redirector.GoToHomePage(false);
                    else
                    {
                        ResultadoPruebaCleaverRpt report = new ResultadoPruebaCleaverRpt(porcentajeMas, porcentajeMenos, porcentajeTotal, SS_AlumnoCarga, SS_UsuarioCarga, SS_FechaFin);
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
        
        private void CargarDatos()
        {
            #region Cargando resultado
            ResultadoPruebaDinamicaCtrl ctrl = new ResultadoPruebaDinamicaCtrl();
            var ds = ctrl.RetrieveResultadoPruebaCleaver(ConnectionHlp.Default.Connection, new CentroEducativo.BO.Alumno() { AlumnoID = userSession.CurrentAlumno.AlumnoID }, new PruebaDinamica() { PruebaID = 14 });
            if (ds.Tables[0].Rows.Count > 0)
            {
                SS_PruebaRealizada = true;
                // Datos del alumno
                Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }));
                SS_AlumnoCarga = alumno;

                // Datos de Usuario
                Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }));
                SS_UsuarioCarga = usuario;

                // Para obtener intereses
                Contrato contrato = userSession.Contrato;
                ContratoCtrl contratoCtrl = new ContratoCtrl();
                List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = userSession.CurrentCicloEscolar });

                var pruebaDinamica = new PruebaDinamica();

                #region Pruebas
                // Prueba realizada
                List<ETipoPruebaPresentacion> tipoPresentacionPruebas = new List<ETipoPruebaPresentacion>();
                tipoPresentacionPruebas.Add(ETipoPruebaPresentacion.Cleaver);
                List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumno, tipoPresentacionPruebas, EEstadoPrueba.CERRADA).Distinct().ToList();


                foreach (var prueba in respuestaAlumno)
                {
                    if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Cleaver)
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
            }
            
            ResultadoPruebaCleaver serieMas = new ResultadoPruebaCleaver();
            ResultadoPruebaCleaver serieMenos = new ResultadoPruebaCleaver();
            ResultadoPruebaCleaver serieTotal = new ResultadoPruebaCleaver();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
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
                #region Valores nulos
                #region Meno
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
                serieTotal.Resultado_D = serieMas.Resultado_D - serieMenos.Resultado_D;
                serieTotal.Resultado_I = serieMas.Resultado_I - serieMenos.Resultado_I;
                serieTotal.Resultado_S = serieMas.Resultado_S - serieMenos.Resultado_S;
                serieTotal.Resultado_C = serieMas.Resultado_C - serieMenos.Resultado_C;

                if (serieTotal.Resultado_D != null && serieTotal.Resultado_I != null && serieTotal.Resultado_S != null && serieTotal.Resultado_C != null)
                {
                    if (serieTotal.Resultado_I > 17)
                        serieTotal.Resultado_I = 17;
                    porcentajeTotal.Resultado_D = ctrl.LastDataRowToPlantillaResultadoCleaver(ctrl.RetrievePlantillaResultadoCleaver(ConnectionHlp.Default.Connection, new PlantillaResultadoCleaver() { Tag = "D", Opcion = "Total", Valor = serieTotal.Resultado_D })).Porcentaje;
                    porcentajeTotal.Resultado_I = ctrl.LastDataRowToPlantillaResultadoCleaver(ctrl.RetrievePlantillaResultadoCleaver(ConnectionHlp.Default.Connection, new PlantillaResultadoCleaver() { Tag = "I", Opcion = "Total", Valor = serieTotal.Resultado_I })).Porcentaje;
                    porcentajeTotal.Resultado_S = ctrl.LastDataRowToPlantillaResultadoCleaver(ctrl.RetrievePlantillaResultadoCleaver(ConnectionHlp.Default.Connection, new PlantillaResultadoCleaver() { Tag = "S", Opcion = "Total", Valor = serieTotal.Resultado_S })).Porcentaje;
                    porcentajeTotal.Resultado_C = ctrl.LastDataRowToPlantillaResultadoCleaver(ctrl.RetrievePlantillaResultadoCleaver(ConnectionHlp.Default.Connection, new PlantillaResultadoCleaver() { Tag = "C", Opcion = "Total", Valor = serieTotal.Resultado_C })).Porcentaje;
                }
                else
                {
                    porcentajeMas = null;
                    porcentajeMenos = null;
                    porcentajeTotal = null;
                }
            }
            #endregion
        }
        #endregion
    }
}