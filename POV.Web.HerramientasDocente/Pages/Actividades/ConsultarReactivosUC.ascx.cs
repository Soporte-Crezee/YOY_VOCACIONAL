using POV.Content.MasterPages;
using POV.Reactivos.BO;
using System;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

namespace POV.Web.HerramientasDocente.Pages.Actividades
{
   

    public partial class ConsultarReactivosUC : System.Web.UI.UserControl
    {

        private const string CMD_ADD_REACTIVO = "CMD_ADD_REACTIVO";
        public string PaginaContenedora { get; set; }
        /// <summary>
        /// Evento del Control de Usuario de Consultar Reactivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Session["ListaSeleccionadosReactivos"] = null;
                }

            }
            catch (Exception ex)
            {

                ShowMessage("Error al cargar los reactivos consultados", Content.MasterPages.Site.EMessageType.Error);
            }
        }


        #region Sessiones

        private List<ReactivoDocente> ReactivosEncontrados
        {
            get { return Session["ReactivosEncontrados"] as List<ReactivoDocente>; }
            set { Session["ReactivosEncontrados"] = value; }
        }

        private List<ReactivoDocente> ReactivoParaAsignar
        {
            get { return Session["ReactivoParaAsignar"] as List<ReactivoDocente>; }
            set { Session["ReactivoParaAsignar"] = value; }
        }

        private List<Guid> ListaSeleccionadosReactivos
        {
            get { return Session["ListaSeleccionadosReactivos"] as List<Guid>; }
            set { Session["ListaSeleccionadosReactivos"] = value; }
        }
        #endregion


        #region Eventos
        /// <summary>
        /// Evento para agregar los reactivos Seleccionados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarReactivo_Click(object sender, EventArgs e)
        {
            try
            {
                string sError = string.Empty;

                KeepSelection(gvReactivos);
                List<ReactivoDocente> reactivosAsignados = new List<ReactivoDocente>();
                if (ListaSeleccionadosReactivos != null)
                {
                    List<Guid> listaReactivosId = ListaSeleccionadosReactivos;
                    foreach (Guid seleccionado in listaReactivosId)
                    {
                        reactivosAsignados.Add(ReactivosEncontrados.Where(r => r.ReactivoID == seleccionado).ToList().FirstOrDefault());
                    }

                }
                if (reactivosAsignados.Count < 1)
                    sError += ", Se debe seleccionar al menos un reactivo";

                string instruccion = txtInstruccionesReactivos.Text.Trim();
                if (string.IsNullOrEmpty(instruccion))
                    sError += " , El campo Instrucción es requerido ";
                else if (instruccion.Length > 200)
                    sError += " , El campo Instrucción solo admite 200 caracteres ";

                if (sError.Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "cerrarDialogo", "CerrarDialogo();", true);
                    ReactivoParaAsignar = reactivosAsignados;
                    //se elimina la sesion
                    Session["ListaSeleccionadosReactivos"] = null;

                    if (PaginaContenedora == "CrearActividadUI")
                        (this.Page as CrearActividadUI).AgregarTareasFromConsulta(CMD_ADD_REACTIVO, instruccion);
                    else if (PaginaContenedora == "EditarActividadUI")
                        (this.Page as EditarActividadUI).AgregarTareasFromConsulta(CMD_ADD_REACTIVO, instruccion);
                }
                else
                {
                    lblErrorAsignarReactivo.Text = "Error: " + sError.Substring(2);
                }
                
            }
            catch (Exception ex)
            {
                ShowMessage("Ocurrio un error al agregar los Reactivos", Content.MasterPages.Site.EMessageType.Error);
            }

        }

        /// <summary>
        /// Evento que Regresa la selecciona de Reactivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReactivos_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RestoreSelection(sender as GridView);
            }
            catch (Exception ex)
            {
                ShowMessage("error en el paginado del grid", Content.MasterPages.Site.EMessageType.Error);
            }
        }
        /// <summary>
        /// Evento para el cambio de paginado de los Reactivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReactivos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                KeepSelection(sender as GridView);
                gvReactivos.DataSource = ReactivosEncontrados;
                gvReactivos.PageIndex = e.NewPageIndex;
                gvReactivos.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error al realizar el páginado los resultados de la consulta", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        /// <summary>
        /// Evento de llenado de carcateristicas de los Reactivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReactivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ReactivoDocente rowView = e.Row.DataItem as ReactivoDocente;
                    string reactivoId = rowView.ReactivoID.ToString();
                    CheckBox cbSeleccionado = e.Row.FindControl("cbSeleccionado") as CheckBox;
                    cbSeleccionado.Attributes.Add("ReactivoID", reactivoId);
                    if (rowView.Activo == true)
                    {
                        e.Row.Cells[4].Text = "ACTIVO";
                    }
                    else
                    {
                        e.Row.Cells[4].Text = "INACTIVO";
                    }


                }
            }
            catch (Exception ex)
            {

                ShowMessage("Error al llenar los datos de los Reactivos", Content.MasterPages.Site.EMessageType.Error);
            }
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Llena el grid con los Reactivos encontrados
        /// </summary>
        /// <param name="reactivosencontrados"></param>
        public void LlenarGrid(List<ReactivoDocente> reactivosencontrados)
        {
            lblErrorAsignarReactivo.Text = "";
            txtInstruccionesReactivos.Text = "";
            gvReactivos.DataSource = ReactivosEncontrados;
            gvReactivos.DataBind();
        }

        /// <summary>
        /// Regresa la seleccion de reactivos encontrados
        /// </summary>
        /// <param name="grid"></param>
        public void RestoreSelection(GridView grid)
        {
            List<Guid> reactivosEncontrados =ListaSeleccionadosReactivos;
            if (reactivosEncontrados == null)
                return;
            List<GridViewRow> resultados = (from item in grid.Rows.Cast<GridViewRow>()
                                            join item2 in reactivosEncontrados
                                            on Guid.Parse(grid.DataKeys[item.RowIndex].Value.ToString()) equals item2 into g
                                            where g.Any()
                                            select item).ToList();
            resultados.ForEach(x => (x.FindControl("cbSeleccionado") as CheckBox).Checked = true);
        }

        /// <summary>
        /// Guarda la seleccion de los reactivos encontrados
        /// </summary>
        /// <param name="grid"></param>
        private void KeepSelection(GridView grid)
        {
            
            // se obtienen los id de las notificaciones checkeadas de la pagina actual
            List<Guid> checkedReactivos = (from item in grid.Rows.Cast<GridViewRow>()
                                       let check = item.FindControl("cbSeleccionado") as CheckBox
                                       where check.Checked
                                       select Guid.Parse(grid.DataKeys[item.RowIndex].Value.ToString())).ToList();

            //
            // se recupera de session la lista de seleccionados previamente
            //
            List<Guid> listaSeleccionados = ListaSeleccionadosReactivos;

            if (listaSeleccionados == null)
                listaSeleccionados = new List<Guid>();

            //
            // se cruzan todos los registros de la pagina actual del gridview con la lista de seleccionados,
            // si algun item de esa pagina fue marcado previamente no se devuelve
            //
            listaSeleccionados = (from item in listaSeleccionados
                                  join item2 in grid.Rows.Cast<GridViewRow>()
                                     on item equals Guid.Parse(grid.DataKeys[item2.RowIndex].Value.ToString()) into g
                                  where !g.Any()
                                  select item).ToList();

            //
            // se agregan los seleccionados
            //
            listaSeleccionados.AddRange(checkedReactivos);

            ListaSeleccionadosReactivos = listaSeleccionados;
        }

        /// <summary>
        /// Muestra mensaje al usuario
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        private void ShowMessage(string message, Site.EMessageType messageType)
        {
            Site site = (Site)Page.Master;
            site.ShowMessage(message, messageType);
        }
        #endregion
    }
}