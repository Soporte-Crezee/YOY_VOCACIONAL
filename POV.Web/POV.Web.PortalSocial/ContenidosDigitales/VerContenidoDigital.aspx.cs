using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Framework.Base.DataAccess;
using POV.ConfiguracionActividades.BO;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.ServiciosActividades.Controllers;
using POV.Web.PortalSocial.AppCode;
using POV.Seguridad.Utils;

namespace POV.Web.PortalSocial.ContenidosDigitales
{

    public partial class VerContenidoDigital : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private ContenidoDigitalCtrl contenidoDigitalCtrl;
        private VisorCtrl visorCtrl;

        #region QueryString
        private string QS_Fuente
        {
            get
            {
                return this.Request.QueryString["src"];
            }
        }
        private string QS_Token
        {
            get
            {
                return this.Request.QueryString["token"];
            }
        }
        private string QS_EjeID
        {
            get
            {
                return this.Request.QueryString["ejeid"];
            }
        }
        private string QS_CursoID
        {
            get
            {
                return this.Request.QueryString["cursoid"];
            }
        }
        private string QS_AsistenciaID
        {
            get
            {
                return this.Request.QueryString["asistenciaid"];
            }
        }
        private string QS_ContenidoID
        {
            get
            {
                return this.Request.QueryString["contid"];
            }
        }

        long? AsignacionId
        {
            get
            {
                if (Request.QueryString["asid"] != null)
                    return Convert.ToInt64(Request.QueryString["asid"]);
                return null;
            }
            set
            {
                Session["AsignacionId"] = value;
            }
        }

        long? TareaId
        {
            get
            {
                if (Request.QueryString["tid"] != null)
                    return Convert.ToInt64(Request.QueryString["tid"]);
                return null;
            }
            set
            {
                Session["TareaId"] = value;
            }
        }

        #endregion


        public VerContenidoDigital()
        {
            contenidoDigitalCtrl = new ContenidoDigitalCtrl();
            visorCtrl = new VisorCtrl();
            userSession = new UserSession();
            redirector = new Redirector();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (userSession.IsLogin())
                        if ((userSession.IsAlumno() || userSession.IsDocente()) && userSession.CurrentEscuela != null &&
                            userSession.Contrato != null)
                        {
                            if (userSession.IsAlumno())
                            {
                                if ((bool)userSession.CurrentAlumno.CorreoConfirmado)
                                {
                                    lnkVerEjeA.Visible = false;
                                    lnkVerEje.Visible = false;

                                    if (QS_Fuente.CompareTo("share") != 0)
                                    {
                                        TareaId = TareaId;
                                        AsignacionId = AsignacionId;

                                        AsignacionActividad asig = new AsignacionActividad { AsignacionActividadId = AsignacionId };
                                        RealizarActividadesController ctrl = new RealizarActividadesController();
                                        asig = ctrl.ConsultarActividadAsignada(asig);

                                        lnkVerEjeA.Visible = asig.TareasRealizadas.FirstOrDefault(t => t.TareaRealizadaId == TareaId).Estatus !=
                                                         EEstatusTarea.Finalizado;
                                    }
                                }
                                else
                                    redirector.GoToHomeAlumno(true);
                            }
                            else
                            {
                                lnkVerEje.Visible = true;
                                lnkVerEjeA.Visible = false;
                            }

                            if (!string.IsNullOrEmpty(QS_Fuente))
                            {

                                long contenidoID = 0;
                                //validamos que nos llegue el id del contenido
                                if (!long.TryParse(QS_ContenidoID, out contenidoID))
                                {
                                    redirector.GoToHomePage(false);
                                    return;
                                }

                                ContenidoDigital miContenido = contenidoDigitalCtrl.RetrieveComplete(dctx, new ContenidoDigital
                                                                                                           {
                                                                                                               ContenidoDigitalID = contenidoID,
                                                                                                               EstatusContenido =
                                                                                                                   EEstatusContenido.ACTIVO
                                                                                                           });
                                //validamos que exista el contenido en la base de datos
                                if (miContenido == null)
                                {
                                    redirector.GoToHomePage(false);
                                    return;
                                }
                                //procesamos el comando que nos llega por GET




                                switch (QS_Fuente)
                                {
                                    case "eje":
                                        LoadContenidoEjeTematico(miContenido);
                                        break;
                                    case "curso":
                                        LoadContenidoCurso(miContenido);
                                        break;
                                    case "asistencia":
                                        LoadContenidoAsistencia(miContenido);
                                        break;
                                    case "share":
                                        LoadContenidoCompartido(miContenido);
                                        break;
                                    case "tcd":
                                        LoadContenidoDigital(miContenido);
                                        break;
                                    default:
                                        redirector.GoToHomePage(false);
                                        return;
                                }


                            }
                            else
                                redirector.GoToHomePage(false);
                        }
                        else
                            redirector.GoToHomePage(false);
                    else
                        redirector.GoToLoginPage(false);
                }
                catch (Exception ex)
                {
                    Logger.Service.LoggerHlp.Default.Error(this, ex);
                }
            }
        }

        private void LoadContenidoDigital(ContenidoDigital miContenido)
        {
            LoadContenidoVisor(miContenido, true);
            //mostramos las variables
            LoadInformacionContenido(miContenido);
        }

        /// <summary>
        /// Consulta y muestra la informacion del contenido digital asignado a la estructura de un eje tematico
        /// </summary>
        /// <param name="miContenido"></param>
        private void LoadContenidoEjeTematico(ContenidoDigital miContenido)
        {

            long ejeID = 0;
            if (!long.TryParse(QS_EjeID, out ejeID))
            {
                redirector.GoToHomePage(false);
                return;
            }


            //validamos el contenido dentro del eje

            ContenidoDigitalAgrupadorCtrl contenidoDigitalAgrupadorCtrl = new ContenidoDigitalAgrupadorCtrl();
            DataSet dsContenidoAgrupador = contenidoDigitalAgrupadorCtrl.Retrieve(dctx, new ContenidoDigitalAgrupador
            {
                EjeTematico = new EjeTematico
                {
                    EjeTematicoID = ejeID
                },
                ContenidoDigital = miContenido
            });
            if (dsContenidoAgrupador.Tables[0].Rows.Count <= 0) //no existe el contenido para el eje
            {
                redirector.GoToHomePage(false);
                return;
            }

            LoadContenidoVisor(miContenido);
            //mostramos las variables
            LoadInformacionContenido(miContenido);
        }

        private void LoadContenidoCompartido(ContenidoDigital miContenido)
        {
            string qsToken = QS_Token;
            if (string.IsNullOrEmpty(qsToken))
            {
                redirector.GoToHomePage(false);
                return;
            }

            string cadenaToken = miContenido.ContenidoDigitalID.Value.ToString() + userSession.CurrentUsuarioSocial.ScreenName.ToLower().Replace(" ", "");
            byte[] bytes = EncryptHash.SHA1encrypt(cadenaToken);
            string token = EncryptHash.byteArrayToStringBase64(bytes);
            if (qsToken.Contains(" "))
                qsToken = qsToken.Replace(" ", "+");
            if (token.CompareTo(qsToken) != 0)
            {
                redirector.GoToHomePage(false);
                return;
            }

            LoadContenidoVisor(miContenido);
            //mostramos las variables
            LoadInformacionContenido(miContenido);
        }

        private void LoadContenidoCurso(ContenidoDigital miContenido)
        {
            long cursoID = 0;
            if (!long.TryParse(QS_CursoID, out cursoID))
            {
                redirector.GoToHomePage(false);
                return;
            }
            //validamos el contenido dentro del curso

            CursoCtrl cursoCtrl = new CursoCtrl();
            Curso curso = cursoCtrl.RetrieveComplete(dctx, new Curso
            {
                AgrupadorContenidoDigitalID = cursoID,
                Estatus = EEstatusProfesionalizacion.ACTIVO
            }) as Curso;

            if (curso == null)
            {
                redirector.GoToHomePage(false);
                return;
            }
            else
            {
                AAgrupadorContenidoDigital agrupadorSimple = curso.AgrupadoresContenido.FirstOrDefault(item => item is AgrupadorSimple);
                //si es un agrupador valido y su lista de contenidos no esta vacia y lo encontramos en la lista
                //presentamos el clasificador
                if (agrupadorSimple != null && agrupadorSimple.ContenidosDigitales.Count > 0 && agrupadorSimple.ContenidosDigitales.FirstOrDefault(item => item.ContenidoDigitalID == miContenido.ContenidoDigitalID) != null)
                {
                    LoadContenidoVisor(miContenido);
                    //mostramos las variables
                    LoadInformacionContenido(miContenido);
                }
                else
                {
                    redirector.GoToHomePage(false);
                    return;
                }
            }


        }

        private void LoadContenidoAsistencia(ContenidoDigital miContenido)
        {
            long asistenciaID = 0;
            if (!long.TryParse(QS_AsistenciaID, out asistenciaID))
            {
                redirector.GoToHomePage(false);
                return;
            }
            //validamos el contenido dentro del curso
            AsistenciaCtrl asistenciaCtrl = new AsistenciaCtrl();
            Asistencia asistencia = asistenciaCtrl.RetrieveComplete(dctx, new Asistencia
            {
                AgrupadorContenidoDigitalID = asistenciaID,
                Estatus = EEstatusProfesionalizacion.ACTIVO
            }) as Asistencia;


            if (asistencia == null)
            {
                redirector.GoToHomePage(false);
                return;
            }
            else
            {
                AAgrupadorContenidoDigital agrupadorSimple = asistencia.AgrupadoresContenido.FirstOrDefault(item => item is AgrupadorSimple);
                //si es un agrupador valido y su lista de contenidos no esta vacia y lo encontramos en la lista
                //presentamos el clasificador
                if (agrupadorSimple != null && agrupadorSimple.ContenidosDigitales.Count > 0 && agrupadorSimple.ContenidosDigitales.FirstOrDefault(item => item.ContenidoDigitalID == miContenido.ContenidoDigitalID) != null)
                {
                    LoadContenidoVisor(miContenido);
                    //mostramos las variables
                    LoadInformacionContenido(miContenido);
                }
                else
                {
                    redirector.GoToHomePage(false);
                    return;
                }
            }
        }

        private void LoadContenidoVisor(ContenidoDigital miContenido, bool esContenidoDocente = false)
        {
            //calculamos el visor registrado
            List<AVisor> visores = visorCtrl.RetrieveAllVisores(dctx);

            AccionContenidoCmd accionCmd = new AccionContenidoCmd(visores);

            EAccionContenido? accion = accionCmd.CalcularAccion(miContenido);
            hdnEsReproducible.Value = "false";
            if (accion != null && accion == EAccionContenido.REPRODUCIR)
            {
                hdnTipoVisor.Value = accionCmd.VisorPredeterminado.Clave;
                hdnEsReproducible.Value = "true";
            }
            string servidorContenidos = "";
            if (esContenidoDocente)
                servidorContenidos = @ConfigurationManager.AppSettings["POVURLServidorContenidosDocente"];
            else
                servidorContenidos = @ConfigurationManager.AppSettings["POVURLServidorContenidos"];


            hdnEsDescargable.Value = miContenido.EsDescargable.Value.ToString().ToLower();
            hdnEsRedireccion.Value = miContenido.EsInterno.Value ? "false" : "true";
            string url = miContenido.ListaURLContenido.FirstOrDefault(item => item.EsPredeterminada.Value).URL;

            hdnUrlContenido.Value = miContenido.EsInterno.Value ? servidorContenidos + url : url;
        }

        /// <summary>
        /// Carga la informacion del contenido digital en los controles de la pagina
        /// </summary>
        /// <param name="miContenido"></param>
        private void LoadInformacionContenido(ContenidoDigital miContenido)
        {
            lblNombreContenido.Text = miContenido.Nombre;
            lblTipoDocumento.Text = miContenido.TipoDocumento.Nombre;
            lblTags.Text = miContenido.Tags;
            divImagenDocumento.Attributes["class"] = "icon_prof " + (string.IsNullOrEmpty(miContenido.TipoDocumento.ImagenDocumento) ? "" : miContenido.TipoDocumento.ImagenDocumento);
        }

        protected void lnkVerEjeA_Click(object sender, EventArgs e)
        {
            try
            {
                AsignacionActividad asignacion = new AsignacionActividad
                                                 {
                                                     AsignacionActividadId = AsignacionId,
                                                     TareasRealizadas = new List<TareaRealizada>
					                                                    {
						                                                    new TareaRealizada
						                                                    {
							                                                    TareaRealizadaId = TareaId
						                                                    }
					                                                    }
                                                 };
                RealizarActividadesController ctrl = new RealizarActividadesController();
                ctrl.FinalizarTarea(asignacion);
                Response.Redirect("~/Actividades/RealizarActividadesDocenteUI.aspx");
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                    Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
    }
}