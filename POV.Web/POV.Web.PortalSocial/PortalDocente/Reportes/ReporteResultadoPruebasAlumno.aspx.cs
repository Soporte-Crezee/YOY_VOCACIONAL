using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.BO;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
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
using POV.Web.Portal.Pruebas.Helper;
using POV.Web.PortalSocial.AppCode;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Expediente.Services;
using POV.Expediente.BO;
using System.Data;
using POV.CentroEducativo.Services;

namespace POV.Web.PortalSocial.PortalDocente.Reportes
{
    public partial class ReporteResultadoPruebasAlumno : System.Web.UI.Page
    {
        #region Propiedades
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        private AlumnoCtrl alumnoCtrl;

        private ExpedienteEscolarCtrl expedienteEscolarCtrl;

        private UsuarioCtrl usuarioCtrl;

        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;

        private InfoAlumnoUsuarioCtrl infoAlumnoUsuarioCtrl;

        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        private ResultadoPruebasOrientacionVocacionalCtrl resultadoPruebasOrientacionVocacionalCtrl;

        private UsuarioExpedienteCtrl usuarioExpedienteCtrl;
        #endregion

        public ReporteResultadoPruebasAlumno() 
        {
            userSession = new UserSession();
            redirector = new Redirector();

            alumnoCtrl = new AlumnoCtrl();
            usuarioCtrl = new UsuarioCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();

            expedienteEscolarCtrl = new ExpedienteEscolarCtrl();
            infoAlumnoUsuarioCtrl = new InfoAlumnoUsuarioCtrl();
            respuestaAlumnoCtrl = new RespuestaAlumnoCtrl();
            resultadoPruebasOrientacionVocacionalCtrl = new ResultadoPruebasOrientacionVocacionalCtrl();

            usuarioExpedienteCtrl = new UsuarioExpedienteCtrl();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                if (!IsPostBack) 
                {
                    if (userSession.IsLogin())
                        if (!userSession.IsAlumno() && userSession.CurrentEscuela != null)
                        {
                            this.hdnSessionSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                            this.hdnSessionUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();
                            hdnSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                            hdnUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();
                            hdnTipoPublicacionSuscripcionReactivo.Value = ((short)ETipoPublicacion.REACTIVO).ToString();
                            hdnTipoPublicacionTexto.Value = ((short)ETipoPublicacion.TEXTO).ToString();
                            SS_RespuestaHabitos = 0;
                            SS_RespuestaDominos = 0;
                            SS_RespuestaTerman = null;
                            SS_RespuestaKuder = null;
                            SS_RespuestaAllport = null;
                            LlenarDropAlumnosUsuarios();
                           
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
                throw;
            }
        }

        private void LlenarDropAlumnosUsuarios()
        {
            UsuarioExpediente usuarioExpediente = new UsuarioExpediente();
            usuarioExpediente.UsuarioID = userSession.CurrentUser.UsuarioID;
            List<Alumno> listaPreAsignados = new List<Alumno>();
            DataSet ds = usuarioExpedienteCtrl.Retrieve(dctx, usuarioExpediente);
            EFAlumnoCtrl eFAlumnoCtrl = new EFAlumnoCtrl(null);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    UsuarioExpediente usExp = usuarioExpedienteCtrl.DataRowToUsuarioExpediente(ds.Tables[0].Rows[i]);
                    // Obtener el alumno
                    Alumno alumno = eFAlumnoCtrl.Retrieve(new Alumno { AlumnoID = usExp.AlumnoID }, false).FirstOrDefault();
                    if (alumno != null)
                    {
                        listaPreAsignados.Add(alumno);
                    }
                }
            }

            ddlAlumno.DataSource = listaPreAsignados.GroupBy(test => test.AlumnoID).Select(grp => grp.First()).ToList(); ;
            ddlAlumno.DataValueField = "AlumnoID";
            ddlAlumno.DataTextField = "NombreCompletoAlumno";
            ddlAlumno.DataBind();

            string r = Request[ddlAlumno.UniqueID];
            if (r != null)
                ddlAlumno.SelectedValue = r;
        }      

        private void CargarDatos()
        {
            Alumno alumnoEstudiante = new Alumno();//InterfaceToFiltroAlumno();
            alumnoEstudiante.AlumnoID = long.Parse(ddlAlumno.SelectedValue);
            string al_Curp = alumnoEstudiante.Curp;
            Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx,alumnoEstudiante));

            Usuario usuarioAlumno = licenciaEscuelaCtrl.RetrieveUsuario(ConnectionHlp.Default.Connection, new Alumno { AlumnoID = alumno.AlumnoID, Curp = alumno.Curp });
            // Para obtener intereses
            Contrato contrato = userSession.Contrato;
            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = userSession.CurrentCicloEscolar });
            PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);

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
            #endregion
            List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumno, tipoPresentacionPruebas, EEstadoPrueba.CERRADA).Distinct().ToList();

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

                if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.TermanMerrill)
                {
                    sumaSeccionesTerman = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaTerman(dctx, prueba.ResultadoPruebaID, prueba.PruebaID, alumno);
                    SS_RespuestaTerman = sumaSeccionesTerman;
                }

                if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Kuder)
                {
                    sumaCalificacionesKuder = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaKuder(dctx, prueba.ResultadoPruebaID, prueba.PruebaID, alumno);
                    SS_RespuestaKuder = sumaCalificacionesKuder;
                }

                if (prueba.TipoPruebaPresentacion == (byte)ETipoPruebaPresentacion.Allport)
                {
                    sumaCalificacionesAllport = resultadoPruebasOrientacionVocacionalCtrl.RetrieveResultadoPruebaAllport(dctx, prueba.ResultadoPruebaID, prueba.PruebaID, alumno);
                    SS_RespuestaAllport = sumaCalificacionesAllport;
                }
            }
            #endregion

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

        protected void btnBuscarAlumno_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(ddlAlumno.SelectedValue))
                {
                    CargarDatos();
                    Response.Redirect("~/PortalDocente/Reportes/DetalleReporteResultado.aspx");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}