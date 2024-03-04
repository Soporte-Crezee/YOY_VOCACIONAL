using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using POV.AppCode.Page;
using POV.Comun.BO;
using POV.ConfiguracionActividades.BO;
using POV.ServiciosActividades.Controllers;
using POV.Content.MasterPages;
using POV.ConfiguracionActividades.Services;

namespace POV.Web.HerramientasDocente.Pages.Actividades
{
    public partial class MantenerActividadesDocenteUI : PageBase
    {
        private MantenerActividadesDocenteController _controllerMantenerActividadDocente;
        private AsignacionActividadGrupoCtrl asignacionActividadGrupoCtrl;

        private List<ActividadDocente> Session_ActividadesEncontradas
        {
            get { return Session["Session_ActividadesEncontradas"] as List<ActividadDocente>; }
            set { Session["Session_ActividadesEncontradas"] = value; }
        }

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

        private string Session_AsignacionEncontrada
        {
            get { return (string)Session["Session_AsignacionEncontrada"]; }
            set { Session["Session_AsignacionEncontrada"] = value; }
        }

        public MantenerActividadesDocenteUI()
        {
            _controllerMantenerActividadDocente = new MantenerActividadesDocenteController();
            asignacionActividadGrupoCtrl = new AsignacionActividadGrupoCtrl(null);
        }

        #region eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    List<ActividadDocente> actividadesDocente = _controllerMantenerActividadDocente.Retrieve(new ActividadDocente { Activo = true, EscuelaId = userSession.CurrentEscuela.EscuelaID, DocenteId = userSession.CurrentDocente.DocenteID, UsuarioId = userSession.CurrentUser.UsuarioID });
                    Session_ActividadesEncontradas = actividadesDocente;
                    LoadActividadesDocente(actividadesDocente);

                    if (!string.IsNullOrEmpty(Session_AsignacionCorrecta))
                    {
                        ShowMessage("La actividad se asignó correctamente.", Content.MasterPages.Site.EMessageType.Information);
                        Session_AsignacionCorrecta = string.Empty;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Session_AsignacionError))
                        {
                            ShowMessage("No fue posible realizar las asignaciones. " + Session_AsignacionError, Content.MasterPages.Site.EMessageType.Error);
                            Session_AsignacionError = string.Empty;
                        }
                    }

                    if (!string.IsNullOrEmpty(Session_AsignacionEncontrada))
                    {
                        Session_AsignacionEncontrada = string.Empty;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Confirm value", "ConfirmReasignacion()", true);
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
                ShowMessage("Problemas al cargar la página", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        /// <summary>
        /// Evento que controla el boton de búsqueda de actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                ActividadDocente actividadDocente = GetActividadDocenteFromUI();

                List<ActividadDocente> actividadesDocente = _controllerMantenerActividadDocente.Retrieve(actividadDocente).Where(x => x.Activo == true).ToList(); ;

                LoadActividadesDocente(actividadesDocente);
            }
            catch (Exception ex)
            {
                ShowMessage("No fue posible llenar la lista de Actividades: " + ex.InnerException.Message, Content.MasterPages.Site.EMessageType.Error);
            }
        }

        /// <summary>
        /// Evento que controla cuando se agrega las filas a la lista de actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividades_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        /// <summary>
        /// Evento que controla cuando un usuario selecciona un acción de la lista de actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "editar_actividad":
                    {
                        long id = -1;
                        if (long.TryParse(e.CommandArgument.ToString(), out id))
                        {
                            ActividadDocente actividad = new ActividadDocente
                            {
                                ActividadID = id,
                                EscuelaId = userSession.CurrentEscuela.EscuelaID,
                                DocenteId = userSession.CurrentDocente.DocenteID
                            };

                            Session_ActividadActual = actividad;
                            Session_ActividadesEncontradas = null;
                            Response.Redirect("~/Pages/Actividades/EditarActividadUI.aspx", true);

                        }
                        else
                        {
                            ShowMessage("No se pudo procesar la solicitud", Content.MasterPages.Site.EMessageType.Error);
                        }
                        break;
                    }
                case "asignar_actividad":
                    {
                        long id = -1;
                        if (long.TryParse(e.CommandArgument.ToString(), out id))
                        {
                            ActividadDocente actividad = new ActividadDocente
                            {
                                ActividadID = id,
                                EscuelaId = userSession.CurrentEscuela.EscuelaID,
                                DocenteId = userSession.CurrentDocente.DocenteID
                            };

                            Session_ActividadActual = actividad;
                            Session_ActividadesEncontradas = null;

                            AsignacionActividadGrupo asignacionActividadGrupo = new AsignacionActividadGrupo();
                            asignacionActividadGrupo.ActividadID = actividad.ActividadID;
                            var asignacionGrupo = asignacionActividadGrupoCtrl.Retrieve(asignacionActividadGrupo, true);

                            var asignacionesActividad = (from asig in asignacionGrupo where asig.AsignacionesActividades.Count > 0 select asig.AsignacionesActividades).Distinct().ToList();

                            if (asignacionesActividad.Count <= 0)
                            {
                                string confirmValue = Request.Form["confirm_value"];
                                if (confirmValue == "Yes")
                                {
                                    Response.Redirect("~/Pages/Actividades/AsignarActividadGrupoUI.aspx", false);
                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                Session_AsignacionEncontrada = "true";
                                Response.Redirect("~/Pages/Actividades/MantenerActividadesUI.aspx", true);
                            }
                        }
                        else
                        {
                            ShowMessage("No se pudo procesar la solicitud", Content.MasterPages.Site.EMessageType.Error);
                        }
                        break;
                    }
                case "eliminar_actividad":
                    {
                        long id = -1;
                        if (long.TryParse(e.CommandArgument.ToString(), out id))
                        {
                            ActividadDocente actividadDocente = new ActividadDocente
                            {
                                ActividadID = id,
                                EscuelaId = userSession.CurrentEscuela.EscuelaID,
                                DocenteId = userSession.CurrentDocente.DocenteID
                            };

                            actividadDocente = _controllerMantenerActividadDocente.RetrieveActividad(actividadDocente);
                            actividadDocente.Activo = false;
                            _controllerMantenerActividadDocente.UpdateActividad(actividadDocente, new List<long>(), new List<FileWrapper>(), null, null, null, null, null);

                            Session_ActividadActual = null;
                            Session_ActividadesEncontradas = null;

                            this.LoadActividadesDocente(_controllerMantenerActividadDocente.Retrieve(new ActividadDocente { Activo = true, EscuelaId = userSession.CurrentEscuela.EscuelaID, DocenteId = userSession.CurrentDocente.DocenteID }));

                            Response.Redirect("~/Pages/Actividades/EditarActividadUI.aspx", true);

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

        /// <summary>
        /// Evento para el cambio de paginado de las actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvActividades.DataSource = Session_ActividadesEncontradas;
                gvActividades.PageIndex = e.NewPageIndex;
                gvActividades.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error al realizar el páginado los resultados de la consulta", Content.MasterPages.Site.EMessageType.Error);
            }
        }
        #endregion

        /// <summary>
        /// Obtiene y crea un objeto Actividad de la UI
        /// </summary>
        /// <returns>Actividad captura a traves de la UI</returns>
        private ActividadDocente GetActividadDocenteFromUI()
        {
            ActividadDocente actividadDocente = new ActividadDocente();

            if (!string.IsNullOrEmpty(txtNombreActividad.Text.Trim()))
                actividadDocente.Nombre = txtNombreActividad.Text.Trim();
            if (!string.IsNullOrEmpty(txtDescripcionActividad.Text.Trim()))
                actividadDocente.Descripcion = txtDescripcionActividad.Text.Trim();
            actividadDocente.EscuelaId = userSession.CurrentEscuela.EscuelaID;
            actividadDocente.DocenteId = userSession.CurrentDocente.DocenteID;
            actividadDocente.UsuarioId = userSession.CurrentUser.UsuarioID;
            return actividadDocente;
        }

        private void ShowMessage(string message, Site.EMessageType messageType)
        {
            Site site = (Site)Page.Master;
            site.ShowMessage(message, messageType);
        }

        /// <summary>
        /// Metodo auxiliar que carga una lista de actividades en gridview
        /// </summary>
        /// <param name="actividades">Actividades</param>
        private void LoadActividadesDocente(List<ActividadDocente> actividadesDocente)
        {
            gvActividades.DataSource = actividadesDocente;
            gvActividades.DataBind();
        }


        protected override void AuthorizeUser()
        {

        }
    }
}