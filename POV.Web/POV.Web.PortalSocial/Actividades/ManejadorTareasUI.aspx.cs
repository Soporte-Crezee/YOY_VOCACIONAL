using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.CentroEducativo.BO;
using POV.ConfiguracionActividades.BO;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Expediente.BO;
using POV.Logger.Service;
using POV.Modelo.Estandarizado.Service;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Prueba.Diagnostico.Service;

using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.ServiciosActividades.Controllers;
using POV.Web.PortalSocial.AppCode;
using POV.Prueba.Diagnostico.Dinamica.BO;


namespace POV.Web.PortalSocial.Actividades
{
    public partial class ManejadorTareasUI : System.Web.UI.Page
    {

        #region Atributos
        IUserSession userSession = new UserSession();
        IRedirector redirector = new Redirector();

        private IDataContext dctx = ConnectionHelper.Default.Connection;
        private PruebaDiagnosticoCtrl pruebaDiagnosticaCtrl;
        public enum EAccion
        {
            INICIAR_TAREA = 1,
            FINALIZAR_TAREA = 2
        }

        #endregion

        #region Propiedades

        private EAccion? Accion
        {
            get
            {
                if (Request.QueryString["accion"] != null)
                    return (EAccion)Convert.ToInt32(Request.QueryString["accion"]);
                return null;
            }
        }

        RealizarActividadesController controller
        {
            get
            {
                return Session["TareasController"] as RealizarActividadesController;
            }
            set
            {
                Session["TareasController"] = value;
            }
        }

        AsignacionActividad AsignacionSeleccionada
        {
            get
            {
                return Session["AsignacionSeleccionada"] as AsignacionActividad;
            }
        }

        private IUserSession usserSesion;

        private AResultadoPrueba Session_ResultadoPrueba
        {
            get
            {
                return (AResultadoPrueba)this.Session["ResultadoPrueba"];
            }
            set
            {
                this.Session["ResultadoPrueba"] = value;
            }
        }

        private APrueba Session_Prueba
        {
            get
            {
                return (APrueba)this.Session["Prueba"];
            }
            set
            {
                this.Session["Prueba"] = value;
            }
        }

        private long? SS_PRUEBAID
        {
            get
            {
                return Session["SS_PRUEBAID"] != null ? Session["SS_PRUEBAID"] as long? : null;
            }
            set
            {
                Session["SS_PRUEBAID"] = value;
            }

        }

        long? AsignacionId
        {
            get
            {
                if (Session["AsignacionId"] != null) return Convert.ToInt64(Session["AsignacionId"]);
                return null;
            }
            set { Session["AsignacionId"] = value; }

        }

        long? TareaRealizadaId
        {
            get
            {
                if (Session["TareaId"] != null) return Convert.ToInt64(Session["TareaId"]);
                return null;
            }
            set { Session["AsignacionId"] = value; }

        }

        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    controller = new RealizarActividadesController();
                    if (Accion == EAccion.INICIAR_TAREA)
                        IniciarTarea();
                    else if (Accion == EAccion.FINALIZAR_TAREA)
                        FinalizarTarea();
                }
                catch (Exception ex)
                {
                    LoggerHlp.Default.Error(this, ex);
                }
            }
        }
        #endregion

        #region Métodos

        public ManejadorTareasUI()
        {

            pruebaDiagnosticaCtrl = new PruebaDiagnosticoCtrl();
        }


        #region Reactivos
        private void IniciarTareaReactivo(TareaReactivo tarea, bool esActividadDocente = false)
        {
            ReactivoCtrl reactivoCtrl = new ReactivoCtrl();
            SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
            AreaAplicacionCtrl areaAplicacionCtrl = new AreaAplicacionCtrl();
            Reactivo reactivo = null;
            Reactivo filtro = null;
            if (esActividadDocente)
                filtro = new ReactivoDocente { ReactivoID = tarea.ReactivoId, TipoReactivo = ETipoReactivo.Estandarizado, Activo = true };
            else
                filtro = new Reactivo { ReactivoID = tarea.ReactivoId, TipoReactivo = ETipoReactivo.Estandarizado, Activo = true };
            DataSet dsReactivo = reactivoCtrl.Retrieve(dctx, filtro);

            if (dsReactivo.Tables[0].Rows.Count > 0)
            {
                reactivo = reactivoCtrl.LastDataRowToReactivo(dsReactivo, ETipoReactivo.Estandarizado);
            }

            //validamos que sea un reactivo registrado
            if (reactivo == null)
            {
                redirector.GoToHomePage(true);
                return;
            }
            DataSet ds = socialHubCtrl.RetrieveSocialHubReactivo(dctx, new SocialHub { SocialProfile = reactivo, SocialProfileType = ESocialProfileType.REACTIVO });

            if (ds.Tables[0].Rows.Count > 0)
            {
                SocialHub socialHubReactivo = socialHubCtrl.LastDataRowToSocialHub(ds);
                
                userSession.SetInSession("CURRENT_REACTIVO_KEY", reactivo);
                userSession.SetInSession("CURRENT_SH_REACTIVO_KEY", socialHubReactivo);
                if (esActividadDocente)
                    userSession.SetInSession("CURRENT_REACTIVO_CMD_KEY", "CMD_TAREA_DOCENTE");
                else
                    userSession.SetInSession("CURRENT_REACTIVO_CMD_KEY", "CMD_TAREA");

                userSession.SetInSession("CURRENT_TareaRealizadaID_KEY", AsignacionSeleccionada.TareasRealizadas.First().TareaRealizadaId.ToString());
                userSession.SetInSession("CURRENT_ActividadRealizadaID_KEY", AsignacionSeleccionada.AsignacionActividadId.Value.ToString());


                Response.Redirect("~/Reactivos/ResolverReactivo.aspx", false);
            }
            else
                redirector.GoToHomePage(true);


        }
        #endregion

        #region Prueba
        private void IniciarTareaPrueba(TareaRealizada tareaRealizada)
		{
            TareaPrueba tarea = tareaRealizada.Tarea as TareaPrueba;

			if (userSession.CurrentAlumno == null)
				throw new Exception("Informacion del estudiante requerida.");
			if (userSession.CurrentEscuela == null)
				throw new Exception("Informacion de la escuela requerida.");
			if (userSession.CurrentGrupoCicloEscolar == null)
				throw new Exception("Informacion de la escuela requerida.");

			SS_PRUEBAID = new long();
			SS_PRUEBAID = tarea.PruebaId;

			CatalogoPruebaCtrl catalogoPruebaCtrl = new CatalogoPruebaCtrl();
			DataSet ds = catalogoPruebaCtrl.Retrieve(dctx, int.Parse(SS_PRUEBAID.ToString()), EEstadoLiberacionPrueba.LIBERADA, null);
			if (ds.Tables[0].Rows.Count != 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					if (int.Parse(row["Tipo"].ToString()) == (int) ETipoPrueba.Dinamica)
					{
						PruebaDinamicaCtrl pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
						Session_Prueba = pruebaDinamicaCtrl.DataRowToPruebaDinamica(row);
					}
				}

                if (tareaRealizada.Estatus == EEstatusTarea.No_Iniciado && tareaRealizada.ResultadoPruebaId == null)
                {
                    Session_ResultadoPrueba = pruebaDiagnosticaCtrl.InsertResultadoPruebaAsignada(dctx, Session_Prueba, userSession.CurrentAlumno,
                                        userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, ETipoResultadoPrueba.PRUEBA_TAREA);


                    controller.IniciarTarea(new AsignacionActividad
                    {
                        AsignacionActividadId = AsignacionSeleccionada.AsignacionActividadId,
                        TareasRealizadas = new List<TareaRealizada> { new TareaRealizada { TareaRealizadaId = tareaRealizada.TareaRealizadaId,
                        ResultadoPruebaId = Session_ResultadoPrueba.ResultadoPruebaID
                        } }

                    });
                }
                else
                {
                    AResultadoPrueba resultadoPruebaFiltro = null;
                    switch(Session_Prueba.TipoPrueba)
                    {
                        case ETipoPrueba.Dinamica:
                            resultadoPruebaFiltro = new ResultadoPruebaDinamica {
                                ResultadoPruebaID = tareaRealizada.ResultadoPruebaId,
                                Tipo = ETipoResultadoPrueba.PRUEBA_TAREA,
                                Prueba = Session_Prueba
                            };
                            break;
                    }
                    Session_ResultadoPrueba = pruebaDiagnosticaCtrl.RetrieveCompleteResultadoPrueba(dctx, resultadoPruebaFiltro);
                }

				string UrlRedireccion = GenerarTokenYUrl(userSession.CurrentAlumno, userSession.CurrentEscuela, userSession.CurrentGrupoCicloEscolar, Session_Prueba, AsignacionSeleccionada, Session_ResultadoPrueba);
				Session["UrlRedireccion"] = UrlRedireccion;
				Session["PruebaPendiente"] = Session_Prueba;
				redirector.GoToDiagnostica(true);
			}
		}

        private string GenerarTokenYUrl(Alumno alumno, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, APrueba prueba, AsignacionActividad asignacionActividad, AResultadoPrueba resultadoPrueba)
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
            UrlDiagnostica = "?alumno=" + alumno.Curp + "&escuela=" + escuela.EscuelaID + "&grupo=" + grupoCicloEscolar.GrupoCicloEscolarID +
                "&fechahora=" + fecha.ToString(formatoFecha) + "&cmd=Talentos" + "&prueba=" + prueba.PruebaID + "&actividad="
                + asignacionActividad.AsignacionActividadId
                + "&tarea=" + asignacionActividad.TareasRealizadas.First().TareaRealizadaId.ToString()
                + "&resultado=" + Session_ResultadoPrueba.ResultadoPruebaID + "&token=" + token;
            return UrlDiagnostica;
        }
        #endregion

        #region EjeTematico
        private void IniciarTareaEjeTematico(TareaEjeTematico tarea)
        {



            Response.Redirect("~/ContenidosDigitales/VerContenidoDigital.aspx?"
                + "src=eje&ejeid="
                + tarea.GetIdentificadorEjeTematico()
                + "&contid=" + tarea.GetIdentificador()
                + "&asid=" + AsignacionSeleccionada.AsignacionActividadId
                + "&tid=" + AsignacionSeleccionada.TareasRealizadas.FirstOrDefault().TareaRealizadaId, false);

        }
        #endregion

        #region contenido
        private void IniciarTareaContenidoDigital(TareaContenidoDigital tarea)
        {
            Response.Redirect("~/ContenidosDigitales/VerContenidoDigital.aspx?"
                + "src=tcd"
                + "&contid=" + tarea.GetIdentificador()
                + "&asid=" + AsignacionSeleccionada.AsignacionActividadId
                + "&tid=" + AsignacionSeleccionada.TareasRealizadas.FirstOrDefault().TareaRealizadaId, false);
        }
        #endregion

        public void IniciarTarea()
        {
            try
            {
                if (AsignacionSeleccionada == null || AsignacionSeleccionada.AsignacionActividadId == null || AsignacionSeleccionada.TareasRealizadas == null || AsignacionSeleccionada.TareasRealizadas.Count == 0 || AsignacionSeleccionada.TareasRealizadas.FirstOrDefault().TareaRealizadaId == null)
                {
                    Response.Redirect("RealizarActividadesUI.aspx");
                    return;
                }
                AsignacionActividad asignacion = controller.ConsultarActividadAsignada(AsignacionSeleccionada);
                bool esActividadDocente = asignacion.Actividad is ActividadDocente;
                TareaRealizada tarea =
                    asignacion.TareasRealizadas.FirstOrDefault(
                        t => t.TareaRealizadaId == AsignacionSeleccionada.TareasRealizadas.FirstOrDefault().TareaRealizadaId);

                if (tarea.Tarea.GetType().BaseType != typeof(TareaPrueba))
                {
                    controller.IniciarTarea(AsignacionSeleccionada);

                    if (tarea.Tarea.GetType().BaseType == typeof(TareaEjeTematico))
                    {
                        IniciarTareaEjeTematico(tarea.Tarea as TareaEjeTematico);
                    }
                    else if (tarea.Tarea.GetType().BaseType == typeof(TareaReactivo))
                    {
                        IniciarTareaReactivo(tarea.Tarea as TareaReactivo, esActividadDocente);
                    }
                    else if (tarea.Tarea.GetType().BaseType == typeof(TareaContenidoDigital))
                    {
                        IniciarTareaContenidoDigital(tarea.Tarea as TareaContenidoDigital);
                    }
                }
                else
                {
                    IniciarTareaPrueba(tarea);

                }


            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw;
            }
        }

        private bool ValidarTerminacionTarea()
        {
            if (Accion != EAccion.FINALIZAR_TAREA || TareaRealizadaId == null || AsignacionId == null) return false;
            if (AsignacionSeleccionada != null && AsignacionSeleccionada.AsignacionActividadId != null || AsignacionSeleccionada.TareasRealizadas != null && AsignacionSeleccionada.TareasRealizadas.Count > 0 && AsignacionSeleccionada.TareasRealizadas.FirstOrDefault().TareaRealizadaId != null)
                return AsignacionSeleccionada.AsignacionActividadId == AsignacionId &&
                       AsignacionSeleccionada.TareasRealizadas.FirstOrDefault().TareaRealizadaId == TareaRealizadaId;
            return false;
        }

        private void FinalizarTarea()
        {
            try
            {
                if (ValidarTerminacionTarea())
                    controller.FinalizarTarea(AsignacionSeleccionada);
                AsignacionId = null;
                TareaRealizadaId = null;
                Response.Redirect("~/AptitudesSobresalientes/RealizarActividadesUI.aspx");
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
            }
        }
        #endregion
    }
}