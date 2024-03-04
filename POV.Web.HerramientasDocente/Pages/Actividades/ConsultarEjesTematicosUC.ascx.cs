using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.ConfiguracionActividades.BO;
using POV.Content.MasterPages;

namespace POV.Web.HerramientasDocente.Pages.Actividades
{
    public partial class ConsultarEjesTematicosUC : System.Web.UI.UserControl
    {
        public string PaginaContenedora { get; set; }

        /// <summary>
        /// Método que inicia la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Session["ListaSeleccionados"] = null;

                }
            }
            catch (Exception ex)
            {
                ShowMessage("No fue posible cargar los Campos de Formación: "+ex.InnerException.Message, Content.MasterPages.Site.EMessageType.Error);
            }
        }

        #region Propiedades
        private const string CMD_ADD_EJE = "CMD_ADD_EJE";

        /// <summary>
        /// Propiedad que guarda los datos encontrados de los ejes temáticos
        /// </summary>
        private List<EjeTematicoRef> EjesTematicosEncontrados
        {
            get { return (List<EjeTematicoRef>)Session["EjesEncontrados"]; }
        }

        /// <summary>
        /// Propiedad que guarda los ejes temáticos que se van a asignar
        /// </summary>
        private List<EjeTematicoRef> EjesTematicosAAsignar
        {
            get { return (List<EjeTematicoRef>) Session["EjesAAsignar"]; }
            set { Session["EjesAAsignar"] = value; }
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Método para el manejo del paginado del grid cuando cambia de página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEjesTematicos_OnPageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RestoreSelection((GridView)sender);
            }
            catch (Exception ex)
            {
                ShowMessage("No fue posible realizar el paginado: " + ex.InnerException.Message, Content.MasterPages.Site.EMessageType.Error);
            }
        }

        /// <summary>
        /// Método para el paginado del grid para obtener información antes de cambiar de página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEjesTematicos_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                KeepSelection((GridView)sender);
                gvEjesTematicos.DataSource = EjesTematicosEncontrados;
                gvEjesTematicos.PageIndex = e.NewPageIndex;
                gvEjesTematicos.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("No fue posible realizar el paginado: " + ex.InnerException.Message, Content.MasterPages.Site.EMessageType.Error);
            }
        }

        /// <summary>
        /// Método para obtener las filas seleccionadas del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEjesTematicos_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var rowView = (EjeTematicoRef)e.Row.DataItem;
                    var ContenidoDigitalId = ObtenerEjeTematicoUniqueKey(rowView);
                    var cbSeleccionado = (CheckBox)e.Row.FindControl("cbSeleccionado");
                    cbSeleccionado.Attributes.Add("ContenidoDigitalId", ContenidoDigitalId);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("No fue posible llenar la lista de recursos didácticos: " + ex.InnerException.Message, Content.MasterPages.Site.EMessageType.Error);
            }
        }

        /// <summary>
        /// Método para agregar las filas seleccionadas a la sesión y redirigir a la creación de la tarea
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarEje_OnClick(object sender, EventArgs e)
        {
            try
            {
                string sError = string.Empty;

                KeepSelection(gvEjesTematicos);
                List<EjeTematicoRef> ejesAsignados = new List<EjeTematicoRef>();
                if (Session["ListaSeleccionados"] != null)
                {
                    List<string> listaEjeId = (List<string>)Session["ListaSeleccionados"];

                    foreach (string seleccionado in listaEjeId)
                    {
                        ejesAsignados.Add(EjesTematicosEncontrados.Where(x => ObtenerEjeTematicoUniqueKey(x) == seleccionado).ToList().FirstOrDefault());
                    }
                }

                if (ejesAsignados.Count < 1)
                    sError += ", Se debe seleccionar al menos un recurso didáctico";

                string instruccion = txtInstruccionesEjes.Text.Trim();
                if (string.IsNullOrEmpty(instruccion))
                    sError += " , El campo instrucción es requerido ";
                else if (instruccion.Length > 200)
                    sError += " , El campo instrucción solo admite 200 caracteres ";

                if (sError.Length == 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "cerrarDialogo", "CerrarDialogo();", true);
                    EjesTematicosAAsignar = ejesAsignados;
                    //se elimina la sesion
                    Session["ListaSeleccionados"] = null;

                    if (PaginaContenedora == "CrearActividadUI")
                        (this.Page as CrearActividadUI).AgregarTareasFromConsulta(CMD_ADD_EJE, instruccion);
                    else if (PaginaContenedora == "EditarActividadUI")
                        (this.Page as EditarActividadUI).AgregarTareasFromConsulta(CMD_ADD_EJE, instruccion);
                }
                else
                {
                    lblErrorAsignarEje.Text = "Error: " + sError.Substring(2);
                }
                
            }
            catch (Exception ex)
            {
                ShowMessage("No fue posible realizar la asignación: " + ex.InnerException.Message, Content.MasterPages.Site.EMessageType.Error);
            }
        }
        #endregion

        #region Métodos Genéricos
        /// <summary>
        /// Método para mostrar los mensajes al usuario
        /// </summary>
        /// <param name="message">Mensaje a mostrar</param>
        /// <param name="messageType">Tipo de Mensaje</param>
        private void ShowMessage(string message, Site.EMessageType messageType)
        {
            Site site = (Site)Page.Master;
            site.ShowMessage(message, messageType);
        }

        /// <summary>
        /// Método para mantener el seleccionado del grid
        /// </summary>
        /// <param name="grid"></param>
        public void RestoreSelection(GridView grid)
        {

            List<string> listaSeleccionados = Session["ListaSeleccionados"] as List<string>;

            if (listaSeleccionados == null)
                return;

            //
            // se comparan los registros de la pagina del grid con los recuperados de la Session
            // los coincidentes se devuelven para ser seleccionados
            //
            List<GridViewRow> result = (from item in grid.Rows.Cast<GridViewRow>()
                                        join item2 in listaSeleccionados
                                        on ObtenerRowDataKey(grid, item.RowIndex) equals item2 into g
                                        where g.Any()
                                        select item).ToList();

            //
            // se recorre cada item para marcarlo
            //
            result.ForEach(x => ((CheckBox)x.FindControl("cbSeleccionado")).Checked = true);

        }

        /// <summary>
        /// Mantiene el seleccionado del grid
        /// </summary>
        /// <param name="grid"></param>
        public void KeepSelection(GridView grid)
        {
            //
            // se obtienen los id de las notificaciones checkeadas de la pagina actual
            List<string> checkedJuegos = (from item in grid.Rows.Cast<GridViewRow>()
                                       let check = (CheckBox)item.FindControl("cbSeleccionado")
                                       where check.Checked
                                          select ObtenerRowDataKey(grid, item.RowIndex)).ToList();

            //
            // se recupera de session la lista de seleccionados previamente
            //
            List<string> listaSeleccionados = Session["ListaSeleccionados"] as List<string>;

            if (listaSeleccionados == null)
                listaSeleccionados = new List<string>();

            //
            // se cruzan todos los registros de la pagina actual del gridview con la lista de seleccionados,
            // si algun item de esa pagina fue marcado previamente no se devuelve
            //
            listaSeleccionados = (from item in listaSeleccionados
                                  join item2 in grid.Rows.Cast<GridViewRow>()
                                     on item equals ObtenerRowDataKey(grid,item2.RowIndex) into g
                                  where !g.Any()
                                  select item).ToList();

            //
            // se agregan los seleccionados
            //
            listaSeleccionados.AddRange(checkedJuegos);

            Session["ListaSeleccionados"] = listaSeleccionados;

        }

        /// <summary>
        /// Método que llena el grid con la información filtrada
        /// </summary>
        /// <param name="ejesConsultados"></param>
        public void LlenarGrid(List<EjeTematicoRef> ejesConsultados)
        {
            txtInstruccionesEjes.Text = "";
            lblErrorAsignarEje.Text = "";
            gvEjesTematicos.DataSource = ejesConsultados;
            gvEjesTematicos.DataBind();
        }

        /// <summary>
        /// Metodo auxiliar que obtiene la llave unica compuesta de EjeTematicoRef
        /// </summary>
        /// <param name="eje">EjeTematicoRef</param>
        /// <returns>Llave compuesta unica</returns>
        public string ObtenerEjeTematicoUniqueKey(EjeTematicoRef eje)
        {
            return eje.EjeTematicoId + "," + eje.ContenidoDigitalId + "," + eje.SituacionAprendizajeId + "," + eje.Clasificador;
        }

        /// <summary>
        /// Obtiene La llave compuesta unica que se encuentra binding en el gridview atravez del indice seleccionado
        /// </summary>
        /// <param name="grid">Grid del que se tomara el valor de la llave unica</param>
        /// <param name="index">indice seleccionado</param>
        /// <returns>Llave compuesta unica</returns>
        public string ObtenerRowDataKey(GridView grid, int index)
        {
            return grid.DataKeys[index]["EjeTematicoId"] + "," + grid.DataKeys[index]["ContenidoDigitalId"] + "," +
                grid.DataKeys[index]["SituacionAprendizajeId"] + "," + grid.DataKeys[index]["Clasificador"];
        }
        #endregion
    }
}