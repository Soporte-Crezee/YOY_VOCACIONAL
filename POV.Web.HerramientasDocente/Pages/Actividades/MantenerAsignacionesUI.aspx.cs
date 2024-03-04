using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.AppCode.Page;
using POV.ConfiguracionActividades.BO;
using POV.ServiciosActividades.Controllers;
using POV.Content.MasterPages;
using POV.CentroEducativo.BO;

namespace POV.Web.HerramientasDocente.Pages.Actividades
{
    public partial class MantenerAsignacionesUI : PageBase
    {
        private List<AsignacionActividad> Session_AsignacionActividadEncontradas
        {
            get { return Session["Session_AsignacionActividadEncontradas"] as List<AsignacionActividad>; }
            set { Session["Session_AsignacionActividadEncontradas"] = value; }
        }

        private MantenerAsignacionesDocenteController _controller;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    _controller = new MantenerAsignacionesDocenteController(); 

                    ActividadDocente filtroActividad = new ActividadDocente
                    {
                        EscuelaId = userSession.CurrentEscuela.EscuelaID,
                        DocenteId = userSession.CurrentDocente.DocenteID,
                        UsuarioId = userSession.CurrentUser.UsuarioID
                    };

                    List<AsignacionActividad> asignaciones = _controller.ConsultarAsignaciones(new Alumno(), filtroActividad, new AsignacionActividad());
                    Session_AsignacionActividadEncontradas = asignaciones;
                    LoadAsignaciones(asignaciones);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Problemas al cargar la página", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                Alumno alumnoFiltro = new Alumno();

                if (!string.IsNullOrEmpty(txtNombreAlumno.Text.Trim()))
                    alumnoFiltro.Nombre = txtNombreAlumno.Text.Trim();

                AsignacionActividad asignacionFiltro = new AsignacionActividad();


                ActividadDocente filtroActividad = new ActividadDocente
                {
                    EscuelaId = userSession.CurrentEscuela.EscuelaID,
                    DocenteId = userSession.CurrentDocente.DocenteID,
                    UsuarioId = userSession.CurrentUser.UsuarioID
                };
                 
                if (!string.IsNullOrEmpty(txtNombreActividad.Text.Trim()))
                    filtroActividad.Nombre = txtNombreActividad.Text.Trim();

                _controller = new MantenerAsignacionesDocenteController();
                List<AsignacionActividad> asignaciones = _controller.ConsultarAsignaciones(alumnoFiltro, filtroActividad, asignacionFiltro);
                Session_AsignacionActividadEncontradas = asignaciones;
                LoadAsignaciones(asignaciones);
            }
            catch (Exception ex)
            {
                ShowMessage("Problemas al cargar la página", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        protected void gvAsignaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var rowView = (AsignacionActividad)e.Row.DataItem;

                    var hdnAsignacionId = (HiddenField)e.Row.FindControl("hdnAsignacionId");
                    hdnAsignacionId.Value = rowView.AsignacionActividadId.Value.ToString();

                     

                    var lblActividad = (Label)e.Row.FindControl("lblActividad");
                    if (rowView.Actividad != null && rowView.Actividad.Nombre != null)
                        lblActividad.Text = rowView.Actividad.Nombre;

                    var lblArea = (Label)e.Row.FindControl("lblArea");
                    if (rowView.Actividad.Clasificador != null) 
                    {
                        string clasificador = string.Empty;
                        if (!string.IsNullOrEmpty(rowView.Actividad.Clasificador.Nombre))
                            clasificador = rowView.Actividad.Clasificador.Nombre;

                        lblArea.Text = clasificador;
                    }
                        

                    var lblAlumno = (Label)e.Row.FindControl("lblAlumno");
                    if (rowView.Alumno != null)
                    {
                        string nombreCompleto = string.Empty;

                        if (!string.IsNullOrEmpty(rowView.Alumno.Nombre))
                            nombreCompleto += rowView.Alumno.Nombre + " ";

                        if (!string.IsNullOrEmpty(rowView.Alumno.PrimerApellido))
                            nombreCompleto += rowView.Alumno.PrimerApellido + " ";

                        if (!string.IsNullOrEmpty(rowView.Alumno.SegundoApellido))
                            nombreCompleto += rowView.Alumno.SegundoApellido;

                        lblAlumno.Text = nombreCompleto.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("No fue posible llenar la lista de actividades (BIND): " + ex.InnerException.Message, Content.MasterPages.Site.EMessageType.Error);
            }
        }

        protected void gvAsignaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAsignaciones.DataSource = Session_AsignacionActividadEncontradas;
                gvAsignaciones.PageIndex = e.NewPageIndex;
                gvAsignaciones.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error al realizar el páginado los resultados de la consulta", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        protected void gvAsignaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                switch (e.CommandName.Trim())
                {
                    case "eliminar_asignacion":
                        {
                            long id = -1;
                            if (long.TryParse(e.CommandArgument.ToString(), out id))
                            {

                                AsignacionActividad asignacion = new AsignacionActividad { AsignacionActividadId = id };
                                List<AsignacionActividad> asignaciones = new List<AsignacionActividad>();
                                asignaciones.Add(asignacion);

                                _controller = new MantenerAsignacionesDocenteController();
                                EliminarAsignaciones(asignaciones);
                                
                            }
                            else
                            {
                                ShowMessage("No se pudo procesar la solicitud", Content.MasterPages.Site.EMessageType.Error);
                            }
                            break;
                        }

                    case "Sort":
                        {
                            break;
                        }
                    case "Page":
                        {
                            break;
                        }
                    default: { ShowMessage("Comando no encontrado", Content.MasterPages.Site.EMessageType.Error); break; }

                }

            }
            catch (Exception ex)
            {
                ShowMessage("Problemas al cargar la página", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        private void LoadAsignaciones(List<AsignacionActividad> actividades)
        {
            gvAsignaciones.DataSource = actividades;
            gvAsignaciones.DataBind();
        }

        private void EliminarAsignaciones(List<AsignacionActividad> eliminadas)
        {

            _controller.EliminarAsignaciones(eliminadas);


            ActividadDocente filtroActividad = new ActividadDocente
            {
                EscuelaId = userSession.CurrentEscuela.EscuelaID,
                DocenteId = userSession.CurrentDocente.DocenteID,
                UsuarioId = userSession.CurrentUser.UsuarioID
            };


            List<AsignacionActividad> asignaciones = _controller.ConsultarAsignaciones(new Alumno(), filtroActividad, new AsignacionActividad());
            Session_AsignacionActividadEncontradas = asignaciones;
            LoadAsignaciones(asignaciones);
        }
        
         

        private void ShowMessage(string message, Site.EMessageType messageType)
        {
            Site site = (Site)Page.Master;
            site.ShowMessage(message, messageType);
        }

        protected override void AuthorizeUser()
        {

        }

        protected void btnEliminarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                List<AsignacionActividad> asignaciones = new List<AsignacionActividad>();
                foreach (GridViewRow row in gvAsignaciones.Rows)
                {
                    CheckBox chkGenerar = (CheckBox)row.FindControl("chkSeleccionado");
                    if (chkGenerar.Checked)
                    {
                        HiddenField hdnAsignacionId = (HiddenField)row.FindControl("hdnAsignacionId");
                        asignaciones.Add(new AsignacionActividad { AsignacionActividadId = long.Parse(hdnAsignacionId.Value) });
                    }
                }

                if (asignaciones.Count > 0)
                {
                    _controller = new MantenerAsignacionesDocenteController();
                    EliminarAsignaciones(asignaciones);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Problemas al eliminar los registros, por favor intente más tarde.", Content.MasterPages.Site.EMessageType.Error);
            }
        }
    }
}