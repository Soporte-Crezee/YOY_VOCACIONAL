using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using POV.AppCode.Page;
using POV.CentroEducativo.BO;
using POV.ConfiguracionActividades.BO;
using POV.Content.MasterPages;
using POV.Core.HerramientasDocente.Implement;
using POV.Core.HerramientasDocente.Interfaces;
using POV.ServiciosActividades.Controllers;
using POV.Web.Helper;
using System;
using POV.Web.ServiciosActividades.Controllers;
using POV.Licencias.Service;
using POV.Licencias.BO;
using POV.CentroEducativo.Service;

namespace POV.Web.HerramientasDocente.Pages.Actividades
{

    public partial class AsignarActividadGrupoUI : PageBase
    {
        private MantenerActividadesDocenteController _controllerMantenerActividad;
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private ConsultarAlumnosGrupoController _controllerConsultarAlumnosGrupo;
        private AsignarActividadGrupoController _controllerAsignarActividadGrupo;
        private AreaConocimiento areaConocimiento;
        //LA: Decalred 
        private ContratoCtrl contratoCtrl;
        private CicloContratoCtrl cicloContratoCtrl;
        private CicloEscolarCtrl cicloEscolarCtrl;
        private CicloEscolar cicloEscolar;

        #region session

        IUserSession userSession = new UserSession();

        private ActividadDocente Session_ActividadActual
        {
            get { return (ActividadDocente)Session["Session_ActividadActual"]; }
            set { Session["Session_ActividadActual"] = value; }
        }

        private string Session_AsignacionCorrecta
        {
            get { return (string)Session["Session_AsignacionCorrecta"]; }
            set { Session["Session_AsignacionCorrecta"] = value; }
        }

        private string Session_AsignacionError
        {
            get { return (string)Session["Session_AsignacionError"]; }
            set { Session["Session_AsignacionError"] = value; }
        }

        private List<GrupoCicloEscolar> Session_GruposCicloEscolar
        {
            get
            {
                if (Session["GruposCicloEscolar"] != null)
                    return Session["GruposCicloEscolar"] as List<GrupoCicloEscolar>;

                return null;
            }
            set
            {
                Session["GruposCicloEscolar"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _controllerMantenerActividad = new MantenerActividadesDocenteController();
                _controllerConsultarAlumnosGrupo = new ConsultarAlumnosGrupoController();
                _controllerAsignarActividadGrupo = new AsignarActividadGrupoController();
                contratoCtrl = new ContratoCtrl();
                cicloContratoCtrl = new CicloContratoCtrl();
                cicloEscolarCtrl = new CicloEscolarCtrl();
                cicloEscolar = new CicloEscolar();

                //TODO: Validar si el usuario ya inicio sesion
                areaConocimiento = new AreaConocimiento();

                if (!IsPostBack)
                {

                    if (Session_ActividadActual != null && Session_ActividadActual.ActividadID != null)
                    {

                        LoadActividad();
                        ConsultarGrupos();

                        btnAsignarActividad_OnClick(sender, e);
                        Response.Redirect("MantenerActividadesUI.aspx");
                    }
                    else
                    {
                        Response.Redirect("MantenerActividadesUI.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
                ShowMessage("Problemas al cargar la página", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        #region GrupoCicloEscolar
        /// <summary>
        /// RowDataBound del Grid de grupos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvGrupos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    string grupoCicloEscolarID = e.Row.Cells[0].Text;
                    CheckBox cbSeleccionado = (CheckBox)e.Row.FindControl("cbSeleccionado");
                    cbSeleccionado.Attributes.Add("GrupoCicloEscolarID", grupoCicloEscolarID);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error al llenar el grid de la consulta", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        #region MÉTODOS

        /// <summary>
        /// Mantiene el seleccionado del grid
        /// </summary>
        /// <param name="grid"></param>
        public void KeepSelection(GridView grid)
        {
            List<AsignacionActividadGrupo> asignacionesGrupos = new List<AsignacionActividadGrupo>();

            // se obtienen los id de las notificaciones checkeadas de la pagina actual
            List<GridViewRow> checkedRows = (from item in grid.Rows.Cast<GridViewRow>()
                                             let check = (CheckBox)item.FindControl("cbSeleccionado")
                                             where check.Checked
                                             select item).ToList();

            foreach (var gridViewRow in checkedRows)
            {
                var dataKey = grid.DataKeys[gridViewRow.RowIndex];
                var fechaInicio = (HiddenField)gridViewRow.FindControl("hdFechaInicio");
                var fechaFin = (HiddenField)gridViewRow.FindControl("hdFechaFin");
                if (dataKey != null)
                    asignacionesGrupos.Add(new AsignacionActividadGrupo()
                    {

                        GrupoCicloEscolarID = Guid.Parse(Convert.ToString(dataKey.Value)),
                        FechaInicio = DateTime.Parse(fechaInicio.Value),
                        FechaFin = DateTime.Parse(fechaFin.Value)
                    });
            }

            // se obtienen los id de las notificaciones checkeadas de la pagina actual
            List<string> checkedGrupos = (from item in grid.Rows.Cast<GridViewRow>()
                                          let check = (CheckBox)item.FindControl("cbSeleccionado")
                                          where check.Checked
                                          select Convert.ToString(grid.DataKeys[item.RowIndex].Value)).ToList();

            //
            // se recupera de session la lista de seleccionados previamente
            //
            List<string> listaGruposSeleccionados = Session["ListaGruposSeleccionados"] as List<string> ?? new List<string>();

            //
            // se cruzan todos los registros de la pagina actual del gridview con la lista de seleccionados,
            // si algun item de esa pagina fue marcado previamente no se devuelve
            //
            listaGruposSeleccionados = (from item in listaGruposSeleccionados
                                        join item2 in grid.Rows.Cast<GridViewRow>()
                                           on item equals Convert.ToString(grid.DataKeys[item2.RowIndex].Value) into g
                                        where !g.Any()
                                        select item).ToList();

            //
            // se agregan los seleccionados
            //
            listaGruposSeleccionados.AddRange(checkedGrupos);
            Session["ListaGruposSeleccionados"] = listaGruposSeleccionados;


        }

        /// <summary>
        /// Consulta la información de los grupos dependiendo la escuela con la que inicies sesion
        /// </summary>
        public void ConsultarGrupos()
        {
            Session_GruposCicloEscolar = _controllerConsultarAlumnosGrupo.ConsultarGruposEscuelaDocente(userSession.CurrentEscuela, userSession.CurrentCicloEscolar, userSession.CurrentDocente);
            gvGrupos.DataSource = Session_GruposCicloEscolar;
            gvGrupos.DataBind();

        }

        #endregion
        #endregion

        #region
        /// <summary>
        /// Evento que controla crear la actividad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAsignarActividad_OnClick(object sender, EventArgs e)
        {
            areaConocimiento.AreaConocimentoID = Session_ActividadActual.ClasificadorID;
            InsertAsignacionesActividadesGrupos(Session_ActividadActual, areaConocimiento);
        }




        /// <summary>
        /// Evento que controla la cancelación de la captura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("MantenerActividadesUI.aspx");
        }

        private void LoadActividad()
        {

            Session_ActividadActual = _controllerMantenerActividad.RetrieveActividadWithRelationship(Session_ActividadActual);
            if (Session_ActividadActual != null && Session_ActividadActual.ActividadID != null)
            {
                hdnControlActividadId.Value = Session_ActividadActual.ActividadID.ToString();
                txtNombreActividad.Text = Session_ActividadActual.Nombre;
                txtDescripcionActividad.Text = Session_ActividadActual.Descripcion;
            }
            else
            {
                Response.Redirect("MantenerActividadesUI.aspx", true);
            }

        }

        /// <summary>
        /// Metodo que controla la inserción de las Asignaciones de Actividades de Grupos en la base de datos
        /// </summary>
        private void InsertAsignacionesActividadesGrupos(ActividadDocente actividadDocente, AreaConocimiento areaConocimiento)
        {
            try
            {
                //LA: Asignacion de Ultimo contrato registrado para la obtencion del ciclo escolar
                var SS_LastObject = (contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(ConnectionHlp.Default.Connection, new Contrato() { })));
                SS_LastObject = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(ConnectionHlp.Default.Connection, SS_LastObject));
                DataSet ds = cicloContratoCtrl.Retrieve(ConnectionHlp.Default.Connection, SS_LastObject, new CicloContrato { Activo = true });
                CicloContrato cicloContrato = cicloContratoCtrl.LastDataRowToCicloContrato(ds);
                cicloEscolar = new CicloEscolar { CicloEscolarID = cicloContrato.CicloEscolar.CicloEscolarID };
                cicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(ConnectionHlp.Default.Connection, cicloEscolar));

                var mensaje = ValidateGrupos();
                if (string.IsNullOrEmpty(mensaje))
                {
                    // se obtienen los id de los grupos seleccionados de la pagina actual
                    List<GridViewRow> checkedRows = (from item in gvGrupos.Rows.Cast<GridViewRow>()
                                                     let check = (CheckBox)item.FindControl("cbSeleccionado")
                                                     where check.Checked
                                                     select item).ToList();

                    //Consulta de la fecha de inicio del Ciclo Escolar  y la fecha final


                    //se generan las asignaciones en base a los grupos seleccionados
                    foreach (var row in checkedRows)
                    {
                        string dataKey = Convert.ToString(gvGrupos.DataKeys[row.RowIndex].Value);
                        GrupoCicloEscolar grupoCicloEscolar =
                            Session_GruposCicloEscolar.Where(x => x.GrupoCicloEscolarID == Guid.Parse(dataKey))
                                                      .FirstOrDefault();
                        var hdFechaInicio = cicloEscolar.InicioCiclo.ToString().Split(' ')[0];//(HiddenField)row.FindControl("hdFechaInicio");
                        var hdFechaFin = cicloEscolar.FinCiclo.ToString().Split(' ')[0];//(HiddenField)row.FindControl("hdFechaFin");
                        var fechaInicio = DateTime.ParseExact(hdFechaInicio.Trim() + " 00:00", "dd/MM/yyyy HH:mm",
                                                              CultureInfo.InvariantCulture);
                        var fechaFin = DateTime.ParseExact(hdFechaFin.Trim() + " 23:59", "dd/MM/yyyy HH:mm",
                                                           CultureInfo.InvariantCulture);
                        var fechaCreacion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                        #region AsignacionesActividadesGrupo

                        var asignacionActividadGrupo = new AsignacionActividadGrupo()
                        {
                            GrupoCicloEscolarID = Guid.Parse(dataKey),
                            ActividadID = actividadDocente.ActividadID,
                            FechaInicio = fechaInicio,
                            FechaFin = fechaFin,
                            FechaCreacion = fechaCreacion,
                            AsignacionesActividades = new List<AsignacionActividad>()
                        };

                        DataTable dtAlumnos =
                            _controllerConsultarAlumnosGrupo.ConsultarAlumnos(userSession.CurrentCicloEscolar,
                                                                              userSession.CurrentEscuela,
                                                                              userSession.CurrentDocente,
                                                                              new Alumno() { Estatus = true },
                                                                              new Grupo()
                                                                              {
                                                                                  Grado = grupoCicloEscolar.Grupo.Grado,
                                                                                  Nombre =
                                                                                      grupoCicloEscolar.Grupo.Nombre
                                                                              }, areaConocimiento, userSession.CurrentUser);

                        #region AsignacionActividadGrupo.AsignacionesActividades

                        foreach (DataRow rowAlumno in dtAlumnos.Rows)
                        {
                            var asignacionActividad = new AsignacionActividad
                            {
                                FechaInicio = fechaInicio,
                                FechaFin = fechaFin,
                                FechaCreacion = fechaCreacion,
                                ActividadId = actividadDocente.ActividadID,
                                AlumnoId = Convert.ToInt64(rowAlumno["AlumnoID"]),
                                EsManual = true,
                                TareasRealizadas = new List<TareaRealizada>(),
                                AsignadoPor = 3
                            };


                            #region AsignacionActividad.TareasRealizadas

                            foreach (Tarea tarea in actividadDocente.Tareas)
                            {
                                var tareaRealizada = new TareaRealizada
                                {
                                    TareaId = tarea.TareaId,
                                    Estatus = EEstatusTarea.No_Iniciado,
                                    Acumulado = 0,
                                };

                                asignacionActividad.TareasRealizadas.Add(tareaRealizada);
                            }

                            #endregion

                            asignacionActividadGrupo.AsignacionesActividades.Add(asignacionActividad);

                        }

                        #endregion

                        if (asignacionActividadGrupo.AsignacionesActividades.Count > 0)
                        {
                            _controllerAsignarActividadGrupo.InsertAsignacionActividadGrupo(asignacionActividadGrupo);
                        }
                        #endregion
                    }

                    Session_AsignacionError = string.Empty;
                    Session_AsignacionCorrecta = "true";
                    txtRedirect.Value = "MantenerActividadesUI.aspx";
                }
                else
                {
                    ShowMessage("<div>Error:" + mensaje + "</div>", Content.MasterPages.Site.EMessageType.Error);
                    Session_AsignacionError = mensaje;
                    Session_AsignacionCorrecta = string.Empty;
                    txtRedirect.Value = "MantenerActividadesUI.aspx";
                }
            }
            catch (Exception exception)
            {
                Session_AsignacionError = exception.Message.ToString();
                Session_AsignacionCorrecta = string.Empty;
                txtRedirect.Value = "MantenerActividadesUI.aspx";
            }
        }

        /// <summary>
        /// Método para comparar las fechas que se quieren asignar
        /// </summary>
        /// <returns>Mensaje del resultdo</returns>
        private String ValidateDates(string strFechaInicio, string strFechaFin, string nombreGrupo)
        {
            var mensaje = String.Empty;
            var dtFechaInicio = new DateTime();
            var dtFechaFin = new DateTime();

            if (!String.IsNullOrEmpty(strFechaInicio.Trim()))
                dtFechaInicio = DateTime.ParseExact(strFechaInicio.Trim() + " 00:00", "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture);
            if (!String.IsNullOrEmpty(strFechaFin.Trim()))
                dtFechaFin = DateTime.ParseExact(strFechaFin.Trim() + " 23:59", "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture);

            if (dtFechaInicio.CompareTo(dtFechaFin.Date) == 1)
                mensaje = mensaje + " Grupo " + nombreGrupo + ": La fecha de inicio no puede ser mayor a la fecha fin.";

            if (DateTime.Now.Date.CompareTo(dtFechaInicio.Date) == 1)
                mensaje = mensaje + " Grupo " + nombreGrupo + ": La fecha de inicio no puede ser menor al día de hoy.";

            if (DateTime.Now.Date.CompareTo(dtFechaFin.Date) == 1)
                mensaje = mensaje + " Grupo " + nombreGrupo + ": La fecha de fin no puede ser menor al día de hoy.";

            return mensaje;
        }

        /// <summary>
        /// Método para validar los campos para asignar Actividad a Grupo
        /// </summary>
        /// <returns>Mensaje del resultado</returns>
        private string ValidateGrupos()
        {

            var mensaje = String.Empty;
            List<AsignacionActividadGrupo> asignacionesGrupos = new List<AsignacionActividadGrupo>();

            // se obtienen los id de los grupos seleccionados de la pagina actual
            List<GridViewRow> checkedRows = (from item in gvGrupos.Rows.Cast<GridViewRow>()
                                             let check = (CheckBox)item.FindControl("cbSeleccionado")
                                             where check.Checked
                                             select item).ToList();

            foreach (var row in checkedRows)
            {
                string dataKey = Convert.ToString(gvGrupos.DataKeys[row.RowIndex].Value);
                GrupoCicloEscolar grupo = Session_GruposCicloEscolar.Where(x => x.GrupoCicloEscolarID == Guid.Parse(dataKey)).FirstOrDefault();
                var nombreGrupo = grupo.Grupo.Grado.ToString() + " " + grupo.Grupo.Nombre;

            }

            return mensaje;
        }
        #endregion

        private void ShowMessage(string message, Site.EMessageType messageType)
        {
            Site site = (Site)Page.Master;
            site.ShowMessage(message, messageType);
        }



        protected override void AuthorizeUser()
        {

        }
    }
}