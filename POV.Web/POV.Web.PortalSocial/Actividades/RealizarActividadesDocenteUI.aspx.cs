using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using POV.ConfiguracionActividades.BO;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Logger.Service;
using POV.ServiciosActividades.Controllers;
using POV.CentroEducativo.BO;
using POV.Web.PortalSocial.AppCode;
using POV.CentroEducativo.Service;
namespace POV.Web.PortalSocial.Actividades
{
    public partial class RealizarActividadesDocenteUI : System.Web.UI.Page
    {
        #region QueryString
        private string QS_ASIGNADOPOR
        {
            get { return this.Request.QueryString["AsignadoPor"]; }
        }
        #endregion


        #region atributos
        private IUserSession userSession;

        private IRedirector redirector;
        #endregion

        #region Propiedades
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

        AsignacionActividad AsignacionSeleccionada
        {
            set
            {
                Session["AsignacionSeleccionada"] = value;
            }
        }
        #endregion

        #region Constructores
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    if (userSession.IsLogin() && userSession.CurrentEscuela != null)
                    {
                        if (userSession.IsAlumno())
                        {
                            if ((bool)userSession.CurrentAlumno.CorreoConfirmado)
                            {
                                ((PortalAlumno.PortalAlumno)this.Master).LoadControlsAlumnoMaster((long)userSession.CurrentUsuarioSocial.UsuarioSocialID, false);

                                this.LblNombreUsuario.Text = userSession.CurrentUsuarioSocial.ScreenName;
                                CargarDocentes();
                                controlador = new RealizarActividadesController();
                                CargaAsignaciones(new Docente());
                            }
                            else redirector.GoToHomeAlumno(true);
                        }
                        else redirector.GoToHomePage(true);


                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                redirector.GoToHomePage(true);
            }

        }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public RealizarActividadesDocenteUI()
        {

            userSession = new UserSession();
            redirector = new Redirector();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Realiza la carga de las actividades asignadas al alumno 
        /// </summary>
        private void CargaAsignaciones(Docente docente)
        {
            List<AsignacionActividad> asignaciones = controlador.ConsultarActividadesAsignadas(userSession.CurrentAlumno);
            List<AsignacionActividad> asignacionesDocente = new List<AsignacionActividad>();

            if (docente != null && docente.DocenteID != null)
            {
                asignacionesDocente = asignaciones.Where(item => item.Actividad is ActividadDocente && (item.Actividad as ActividadDocente).DocenteId == docente.DocenteID && item.Actividad.EscuelaId == userSession.CurrentEscuela.EscuelaID && item.AsignadoPor == int.Parse(QS_ASIGNADOPOR)).ToList();
            }
            else
            {
                asignacionesDocente = asignaciones.Where(item => item.Actividad is ActividadDocente && item.Actividad.EscuelaId == userSession.CurrentEscuela.EscuelaID && item.AsignadoPor == int.Parse(QS_ASIGNADOPOR)).ToList();
            }

            lblTotalActividadesDocentePendientes.Text = asignacionesDocente.Where(item => item.FechaInicio <= DateTime.Now && item.FechaFin >= DateTime.Now).Count(item => item.TareasRealizadas.Any(item2 => item2.Estatus != EEstatusTarea.Finalizado)).ToString();

            LvwActividadDocentes.DataSource = asignacionesDocente;
            LvwActividadDocentes.DataBind();
        }

        private void CargarDocentes()
        {
            try
            {
                IDataContext dctx = ConnectionHelper.Default.Connection;
                GrupoCicloEscolarCtrl grupoCtrl = new GrupoCicloEscolarCtrl();

                List<Docente> docentes = grupoCtrl.RetrieveDocentes(dctx, new GrupoCicloEscolar { GrupoCicloEscolarID = userSession.CurrentGrupoCicloEscolar.GrupoCicloEscolarID });

                var item = new ListItem("TODOS", "") { Selected = true };
                ddlDocentes.Items.Clear();
                ddlDocentes.Items.Add(item);
                docentes.ForEach(a => ddlDocentes.Items.Add(new ListItem(a.Nombre + " " + a.PrimerApellido + " " + ( a.SegundoApellido != null? a.SegundoApellido : ""), a.DocenteID.ToString())));
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                redirector.GoToHomePage(true);
            }
        }

        private DataTable ConfigurarTabsDocente(List<AsignacionActividad> asignaciones)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Tipo");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Titulo");
            dt.Columns.Add("TotalActividades");

            DataRow row = dt.NewRow();
            row["Tipo"] = "1";
            row["Nombre"] = "Actividades mis maestros";
            row["Titulo"] = "Actividades que tus maestros te asignaron para que participes";
            var totalActividadesDocente = asignaciones.Where(item => item.Actividad is ActividadDocente).ToList().Count(item => item.TareasRealizadas.Any(item2 => item2.Estatus != EEstatusTarea.Finalizado));
            row["TotalActividades"] = totalActividadesDocente;

            dt.Rows.Add(row);

            string moduloTalentosKey = @System.Configuration.ConfigurationManager.AppSettings["MODULO_TALENTOS_KEY"];
            //AN4 -Validación Si el usuario tiene configurado el modulo de talentos para su escuela y contrato
            if (userSession.ModulosFuncionales != null &&
                userSession.ModulosFuncionales.FirstOrDefault(m => m.ModuloFuncionalId == int.Parse(moduloTalentosKey)) != null)
            {
                DataRow rowTalentos = dt.NewRow();
                rowTalentos["Tipo"] = "2";
                rowTalentos["Nombre"] = "Actividades generales";
                rowTalentos["Titulo"] = "Actividades generales que te asignaron para que participes";
                var totalActividadesGenerales = asignaciones.Where(item => !(item.Actividad is ActividadDocente)).ToList().Count(item => item.TareasRealizadas.Any(item2 => item2.Estatus != EEstatusTarea.Finalizado));
                rowTalentos["TotalActividades"] = totalActividadesGenerales;
                dt.Rows.Add(rowTalentos);
            }

            return dt;
        }
        #endregion

        #region Eventos

        protected void LvwActividad_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem) return;
            AsignacionActividad asignacion = e.Item.DataItem as AsignacionActividad;
            Label txtNombreActividad = e.Item.FindControl("lblNombreActividad") as Label;
            txtNombreActividad.Text = asignacion.Actividad.Nombre;

            bool estaVencida = (asignacion.FechaFin.Value.CompareTo(DateTime.Now) < 0);
            bool estaFinalizada = asignacion.TareasRealizadas.All(item => item.Estatus == EEstatusTarea.Finalizado);

            Label txtVigencia = e.Item.FindControl("lblVigenciaActividad") as Label;
            txtVigencia.Text = ((DateTime)asignacion.FechaInicio).ToShortDateString() + " - " +
                               ((DateTime)asignacion.FechaFin).ToShortDateString();

            Label lblEstadoActividad = e.Item.FindControl("lblEstadoActividad") as Label;

            if (estaVencida && !estaFinalizada)
            {
                txtVigencia.ControlStyle.CssClass = "error";
                txtVigencia.ToolTip = "El período de vigencia de la actividad ha terminado";
                lblEstadoActividad.Text = "No disponible";
            }
            else if (estaFinalizada)
            {
                lblEstadoActividad.Text = "Finalizada";
            }
            else
            {
                lblEstadoActividad.Text = "Pendiente";
            }

            if (asignacion.Actividad is ActividadDocente)
            {
                Label txtDocente = e.Item.FindControl("lblNombreDocente") as Label;
                Docente d = (asignacion.Actividad as ActividadDocente).Docente;
                txtDocente.Text = d.Nombre + " " + d.PrimerApellido + (string.IsNullOrEmpty(d.SegundoApellido) ? "" : " " + d.SegundoApellido);

            }


            GridView grdTareas = e.Item.FindControl("grdTareas") as GridView;

            DataTable tareas = new DataTable();
            tareas.Columns.Add("ActividadId");
            tareas.Columns.Add("TareaRealizadaId");
            tareas.Columns.Add("Nombre");
            tareas.Columns.Add("Instruccion");
            tareas.Columns.Add("Estatus");
            tareas.Columns.Add("EsEjecutable");
            tareas.Columns.Add("Orden");
            tareas.Columns.Add("Negrita");

            foreach (TareaRealizada tareaRealizada in asignacion.TareasRealizadas)
            {
                DataRow row = tareas.NewRow();
                row["ActividadId"] = asignacion.AsignacionActividadId;
                row["TareaRealizadaId"] = tareaRealizada.TareaRealizadaId;
                row["Nombre"] = System.Web.HttpUtility.HtmlDecode(tareaRealizada.Tarea.Nombre);
                row["Instruccion"] = tareaRealizada.Tarea.Instruccion;
                row["Estatus"] = Enum.GetName(typeof(EEstatusTarea), (tareaRealizada.Estatus)).Replace('_', ' ');
                row["EsEjecutable"] = !estaVencida;



                if (tareaRealizada.Tarea.GetType().BaseType == typeof(TareaPrueba))
                {
                    row["EsEjecutable"] =
                        asignacion.TareasRealizadas.Where(tr => tr.Tarea.GetType().BaseType != typeof(TareaPrueba))
                            .All(tr => tr.Estatus == EEstatusTarea.Finalizado) && tareaRealizada.Estatus != EEstatusTarea.Finalizado;
                    row["Orden"] = "2";
                    row["Negrita"] = true;

                }

                else
                {
                    row["Orden"] = "1";
                    row["Negrita"] = false;
                }
                tareas.Rows.Add(row);

            }

            grdTareas.DataSource = (from r in tareas.AsEnumerable() orderby r["Orden"] ascending select r).AsDataView().ToTable();
            grdTareas.DataBind();
        }

        protected void grdTareas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("RealizarTarea", StringComparison.InvariantCultureIgnoreCase))
                {
                    string[] paramStrings = e.CommandArgument.ToString().Split('&');
                    if (paramStrings.Count() < 2) return;
                    AsignacionActividad asignacionActividad = new AsignacionActividad
                                                              {
                                                                  AsignacionActividadId = Convert.ToInt64(paramStrings[0])
                                                              };
                    TareaRealizada tareaRealizada = new TareaRealizada
                                                    {
                                                        TareaRealizadaId = Convert.ToInt64(paramStrings[1])
                                                    };
                    asignacionActividad.TareasRealizadas = new List<TareaRealizada> { tareaRealizada };
                    AsignacionSeleccionada = asignacionActividad;
                    Response.Redirect("ManejadorTareasUI.aspx?Accion=" + Convert.ToInt32(ManejadorTareasUI.EAccion.INICIAR_TAREA));
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw;
            }
        }

        #endregion

        protected void ddlDocentes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int docenteId = 0;

                if (int.TryParse(ddlDocentes.SelectedValue.Trim(), out docenteId))
                {
                    CargaAsignaciones(new Docente { DocenteID = docenteId });
                }
                else
                {
                    CargaAsignaciones(new Docente());
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw;
            }
             
        }
    }
}