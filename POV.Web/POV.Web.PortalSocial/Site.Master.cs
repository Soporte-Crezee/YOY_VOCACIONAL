using System;
using System.Collections.Generic;
using System.Linq;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using System.Data;
using POV.Web.Administracion.Helper;
using POV.ConfiguracionActividades.BO;
using POV.ServiciosActividades.Controllers;
using POV.Expediente.BO;
using POV.Expediente.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Modelo.BO;
using System.Collections;
using Framework.Base.DataAccess;
using POV.Web.PortalSocial.AppCode;
using System.Web.UI;
using System.Web;
using System.Text.RegularExpressions;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Prueba.Diagnostico.Dinamica.Service;
using System.Configuration;
using POV.CentroEducativo.Services;
using POV.Logger.Service;
using POV.Administracion.Services;
using POV.Administracion.BO;

namespace POV.Web.PortalSocial
{
    public partial class Site : System.Web.UI.MasterPage
    {
        private PruebaDiagnosticoCtrl pruebaDiagnosticoCtrl;
        private PruebaDinamicaCtrl pruebaDinamicaCtrl;

        private IUserSession userSession;
        private IRedirector redirector;
        private AlumnoCtrl alumnoCtrl;
        private Alumno alumno;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        public string tiempoEsperaNotificacion;
        public string tiempoEsperaSesion;

        private RealizarActividadesController controlador
        {
            get
            {
                return Session["ControladorUI"] as RealizarActividadesController;
            }
            set
            {
                Session["ControladorUI"] = value;
            }
        }

        #region Compra
        public int? Session_PruebaID {
            get { return Session["Session_PruebaID"] as int?; }
            set { Session["Session_PruebaID"] = value; }
        }

        public int? Session_CostoProductoID
        {
            get { return Session["Session_CostoProductoID"] as int?; }
            set { Session["Session_CostoProductoID"] = value; } 
        }

        private double? Session_Costo {
            get { return Session["Session_Costo"] as double?; }
            set { Session["Session_Costo"] = value; }
        }

        #endregion

        public Site()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            alumnoCtrl = new AlumnoCtrl();
            alumno = new Alumno();
            pruebaDiagnosticoCtrl = new PruebaDiagnosticoCtrl();
            tiempoEsperaNotificacion = System.Configuration.ConfigurationManager.AppSettings["POVtiempoEsperaNotificacion"];
            tiempoEsperaSesion = System.Configuration.ConfigurationManager.AppSettings["POVtiempoEsperaSesion"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Header.DataBind();

            if (!IsPostBack)
            {
                if (userSession.IsLogin())
                {
                    LblNombreUsuarioTop.Text = userSession.CurrentUsuarioSocial.ShortScreenName;
                    Label1.Text = userSession.CurrentUsuarioSocial.ShortScreenName;

                    if (userSession.IsAlumno()) //es alumno
                    {
                        txtSesionAlumno.Text = "True";
                        InitMenuAlumno();
                        ShowMenuAlumno();
                        alumno = getDataAlumnoToObject();

                        if (alumno.DatosCompletos != null)
                        {
                            txtDatosCompletos.Text = alumno.DatosCompletos.ToString();
                        }
                        else
                        {
                            txtDatosCompletos.Text = "False";
                        }
                        if (alumno.CorreoConfirmado != null)
                        {
                            txtCorreoConfirmado1.Text = alumno.CorreoConfirmado.ToString();
                        }
                        else
                        {
                            txtCorreoConfirmado1.Text = "False";
                        }

                        ShowMenuAreasConocimientoPosts();

                    }
                    else
                    {
                        LblNombreDcoenteTop.Text = userSession.CurrentUsuarioSocial.ShortScreenName;
                        LoadDocente();
                        ImgThumbDocenteTop.ImageUrl = UrlHelper.GetImagenPerfilURL("thumb", (long)userSession.CurrentUsuarioSocial.UsuarioSocialID);
                        InitMenuDocente();
                        ShowMenuDocente();
                        txtSesionDocente.Text = "True";
                    }
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
            else
            {
                if (userSession.IsAlumno()) //es alumno
                {
                    ShowMenuAreasConocimientoPosts();
                }                
            }
        }

        private void CargaAsignaciones(Docente docente)
        {
            List<AsignacionActividad> asignaciones = controlador.ConsultarActividadesAsignadas(userSession.CurrentAlumno);
            List<AsignacionActividad> asignacionesUniversidad = new List<AsignacionActividad>();
            List<AsignacionActividad> asignacionesDocente = new List<AsignacionActividad>();
            List<AsignacionActividad> asignacionesAreasConocimiento = new List<AsignacionActividad>();

            if (docente != null && docente.DocenteID != null)
            {
                asignacionesDocente = asignaciones.Where(item => item.Actividad is ActividadDocente && (item.Actividad as ActividadDocente).DocenteId == docente.DocenteID && item.Actividad.EscuelaId == userSession.CurrentEscuela.EscuelaID && item.AsignadoPor == 2).ToList();
                asignacionesAreasConocimiento = asignaciones.Where(item => item.Actividad is ActividadDocente && (item.Actividad as ActividadDocente).DocenteId == docente.DocenteID && item.Actividad.EscuelaId == userSession.CurrentEscuela.EscuelaID && item.AsignadoPor == 3).ToList();
            }
            else
            {
                asignacionesDocente = asignaciones.Where(item => item.Actividad is ActividadDocente && item.Actividad.EscuelaId == userSession.CurrentEscuela.EscuelaID && item.AsignadoPor == 2).ToList();
                asignacionesAreasConocimiento = asignaciones.Where(item => item.Actividad is ActividadDocente && item.Actividad.EscuelaId == userSession.CurrentEscuela.EscuelaID && item.AsignadoPor == 3).ToList();
            }
            int asignacionOrientador = int.Parse(asignacionesDocente.Where(item => item.FechaInicio <= DateTime.Now && item.FechaFin >= DateTime.Now).Count(item => item.TareasRealizadas.Any(item2 => item2.Estatus != EEstatusTarea.Finalizado)).ToString());
            int areasConocimiento = int.Parse(asignacionesAreasConocimiento.Where(item => item.FechaInicio <= DateTime.Now && item.FechaFin >= DateTime.Now).Count(item => item.TareasRealizadas.Any(item2 => item2.Estatus != EEstatusTarea.Finalizado)).ToString());
            actividadesSummary.Text = "(" + (asignacionOrientador + areasConocimiento) + ")";

            
            orientadorSummary.Text = "(" + asignacionOrientador + ")";
            areasConocimientoSummary.Text = "(" + areasConocimiento + ")";

        }

        private Alumno getDataAlumnoToObject()
        {

            alumno.AlumnoID = userSession.CurrentAlumno.AlumnoID;
            DataSet ds = alumnoCtrl.Retrieve(ConnectionHlp.Default.Connection, alumno);

            if (ds.Tables[0].Rows.Count == 1)
                alumno = alumnoCtrl.LastDataRowToAlumno(ds);

            return alumno;
        }

        #region *** Data to UserInterface ***
        private void LoadDocente()
        {
            Docente docente = new EFDocenteCtrl(null).Retrieve(new Docente { DocenteID = userSession.CurrentDocente.DocenteID }, false).First();
        }
        #endregion

        private void ShowMenuAreasConocimientoPosts()
        {
            var areasConocimiento = GetAreasConocimientoAlumno();
            foreach (var areaConocimiento in areasConocimiento)
            {
                string id = areaConocimiento.Nombre.Replace(" ", "");
                id = QuitarAcentos(id);
                ulPosts.Controls.Add(new LiteralControl(@"
                            <li id='" + id + @"' >
								<a href='" + Page.ResolveClientUrl("~/PortalAlumno/ViewBlog.aspx?AreaConocimientoBlog=") + id + @"'>
									&nbsp;&nbsp;&nbsp;&nbsp;" + areaConocimiento.Nombre + @"
									<i id='" + id + @"_Summary'>
							    		<span id='" + id + @"Summary' ></span>
									</i>
							    </a>
							</li>"
                         )
                  );
            }

            ulPosts.Controls.Add(new LiteralControl(@"
                            <li class='divider'></li>
							<li class='dropdown-header'>Blog</li>
							<li class='divider'></li>
							<li>
							    <a href='" + Page.ResolveClientUrl("~/PortalAlumno/ViewBlog.aspx") + @"'>
								    &nbsp;&nbsp;&nbsp;&nbsp;Ir al blog															
								</a>
							</li>"));
        }

        public List<Clasificador> GetAreasConocimientoAlumno()
        {
            ArrayList arrAreaConocimiento = new ArrayList();
            List<Clasificador> areasConocimiento = new List<Clasificador>();
            Alumno alumno = userSession.CurrentAlumno;
            CicloEscolar cicloEscolar = userSession.CurrentCicloEscolar;
            Contrato contrato = userSession.Contrato;

            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = cicloEscolar });
            PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);

            var pruebaDinamica = (PruebaDinamica)pruebaPivoteContrato.Prueba;

            ExpedienteEscolarCtrl expCtrl = new ExpedienteEscolarCtrl();
            List<InteresAspirante> interesAspirante = expCtrl.RetrieveInteresesAspirante(dctx, alumno, pruebaDinamica).Distinct().ToList();

            foreach (InteresAspirante clas in interesAspirante)
            {
                if (arrAreaConocimiento.IndexOf(clas.clasificador.ClasificadorID) == -1)
                {
                    arrAreaConocimiento.Add(clas.clasificador.ClasificadorID);
                    areasConocimiento.Add(clas.clasificador);
                }
            }

            return areasConocimiento;
        }

        public void GoToDiagnostica(int pruebaID)
        {
            if (userSession.IsLogin())
            {
                Session_Costo = null;
                Session_CostoProductoID = null;
                Session_PruebaID = null;

                var prueba = pruebaDiagnosticoCtrl.RetrievePruebaPendiente(dctx, userSession.Contrato, userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, false).ToList();

                prueba[0] = prueba.FirstOrDefault(itm => itm.PruebaID == pruebaID);
                
                if (prueba[0] != null)
                {
                        string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, prueba[0]);
                        Session["UrlRedireccion"] = UrlRedireccion;
                        Session["PruebaPendiente"] = prueba[0];
                        redirector.GoToDiagnostica(true);
                }
            }
            else
            {
                redirector.GoToLoginPage(true);
            }
        }

        private string GenerarTokenYUrl(Alumno alumno, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, APrueba prueba)
        {

            string UrlDiagnostica;
            string formatoFecha = "yyyyMMddHHmmss.fff";
            DateTime fecha = System.DateTime.Now;
            string nombre = alumno.Nombre;
            nombre = nombre.Trim();
            string apellido = alumno.PrimerApellido;
            apellido = apellido.Trim();
            string curp = alumno.Curp;
            DateTime fechaNacimiento = (DateTime)alumno.FechaNacimiento;
            curp = curp.Trim();
            string token = fechaNacimiento.ToString(formatoFecha) + nombre + apellido + curp + fecha.ToString(formatoFecha);
            byte[] clave = POV.Seguridad.Utils.EncryptHash.SHA1encrypt(token);
            token = POV.Seguridad.Utils.EncryptHash.byteArrayToStringBase64(clave);
            token = System.Web.HttpUtility.UrlEncode(token);
            UrlDiagnostica = "?alumno=" + alumno.Curp + "&escuela=" + escuela.EscuelaID + "&grupo=" + grupoCicloEscolar.GrupoCicloEscolarID + "&fechahora=" + fecha.ToString(formatoFecha) + "&prueba=" + prueba.PruebaID + "&token=" + token;
            return UrlDiagnostica;
        }

        public string QuitarAcentos(string texto)
        {
            Regex a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            Regex n = new Regex("[ñ|Ñ]", RegexOptions.Compiled);

            texto = a.Replace(texto, "a");
            texto = e.Replace(texto, "e");
            texto = i.Replace(texto, "i");
            texto = o.Replace(texto, "o");
            texto = u.Replace(texto, "u");
            texto = n.Replace(texto, "n");

            return texto;
        }

        private void InitMenuAlumno()
        {
            //topmenu
            this.HplMensajes.NavigateUrl = UrlHelper.GetMensajesURL();
            this.HplNotificaciones.NavigateUrl = UrlHelper.GetNotificacionesURL();
            this.HplNameUsuario.NavigateUrl = UrlHelper.GetAlumnoMuroURL((long)userSession.CurrentUsuarioSocial.UsuarioSocialID);
            this.HplNameUsuario.ToolTip = userSession.CurrentUsuarioSocial.ScreenName;

            this.HyperLink11.NavigateUrl = UrlHelper.GetAlumnoMuroURL((long)userSession.CurrentUsuarioSocial.UsuarioSocialID);
            this.HyperLink11.ToolTip = userSession.CurrentUsuarioSocial.ScreenName;

            this.HplLogout.NavigateUrl = UrlHelper.GetLogoutURL();
            this.HlpEditarPerfil.NavigateUrl = UrlHelper.GetEditarPerfilURL();
            this.HlpCambiarPassword.NavigateUrl = UrlHelper.GetEditarPassURL();
            this.HlpMisIntereses.NavigateUrl = UrlHelper.GetMisInteresesURL();
            this.HlpExpediente.NavigateUrl = UrlHelper.GetExpedienteURL();

            this.HplPruebas.NavigateUrl = UrlHelper.GetPruebasURL();


            //toolbar
            this.HplMiPortal.NavigateUrl = UrlHelper.GetAlumnoNoticiasURL();
            this.HplMiGrupo.NavigateUrl = UrlHelper.GetGrupoListaURL();
            this.HplUniversidades.NavigateUrl = UrlHelper.GetCarrerasEventosUniversidadURL();
        }

        private void InitMenuDocente()
        {
            //topmenu
            this.HplNotificacionesDocente.NavigateUrl = UrlHelper.GetNotificacionesURL();
            this.HplCambiarEscuela.NavigateUrl = UrlHelper.GetDocenteCambiarEscuelaURL();
            this.HplLogoutDocente.NavigateUrl = UrlHelper.GetLogoutURL();
            this.HplNameDocente.NavigateUrl = UrlHelper.GetDocenteInicioURL();
            this.HplNameDocente.ToolTip = userSession.CurrentUsuarioSocial.ScreenName;
            this.HlpEditarPerfilDocente.NavigateUrl = UrlHelper.GetEditarPerfilURL();
            this.HlpCambiarPasswordDocente.NavigateUrl = UrlHelper.GetEditarPassURL();
            this.HplReporteAbuso.NavigateUrl = UrlHelper.GetReportesAbusoURL();
            if (userSession.LicenciasDocente.Count <= 1)
            {
                HplCambiarEscuela.Visible = false;
                HplCambiarEscuela.Enabled = false;
            }
            else
            {
                this.HplCambiarEscuela.NavigateUrl = UrlHelper.GetDocenteCambiarEscuelaURL();
            }
            //toolbar
            this.HplInicioDocente.NavigateUrl = UrlHelper.GetDefaultURL();
            this.HplDocenteReactivos.NavigateUrl = UrlHelper.GetReactivosURL();

        }

        private void ShowMenuAlumno()
        {
            MultiViewTopMenu.SetActiveView(ViewTopMenuAlumno);
            MultiViewToolBarMenu.SetActiveView(ViewToolBarMenuAlumno);
        }

        private void ShowMenuDocente()
        {

            MultiViewTopMenu.SetActiveView(ViewTopMenuDocente);
            MultiViewToolBarMenu.SetActiveView(ViewToolBarMenuDocente);
        }

        protected void HlpConfiguracion_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewModalConfiguracion();", true);
        }
    }
}