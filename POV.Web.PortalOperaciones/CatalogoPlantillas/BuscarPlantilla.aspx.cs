using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.ConfiguracionesPlataforma.BO;
using POV.Seguridad.BO;
using POV.Web.Controllers.ServiciosPlataforma.Controllers;
using POV.Web.PortalOperaciones.AppCode.Page;

namespace POV.Web.PortalOperaciones.CatalogoPlantillas
{
    public partial class BuscarPlantilla : CatalogPage
    {
        private CatalogoPlantillaLudicaController controller;

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            controller = new CatalogoPlantillaLudicaController(null);
            try
            {
                if (!IsPostBack)
                {
                    plantillasLudicas = controller.ConsultarPlantillasLudicas(InterfaceToFiltro());
                    LoadPlantillas();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Ocurrió un error al cargar la página", MessageType.Error);
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            plantillasLudicas = controller.ConsultarPlantillasLudicas(InterfaceToFiltro());
            LoadPlantillas();

        }

        protected void grdPlantillasLudicas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "editar":
                    {
                        PlantillaLudica plantilla = new PlantillaLudica();
                        plantilla.PlantillaLudicaId = int.Parse(e.CommandArgument.ToString());
                        lastObjectPlantilla = plantilla;
                        Response.Redirect("EditarPlantillaLudica.aspx");
                        break;
                    }
                case "eliminar":
                    {
                        PlantillaLudica plantilla = new PlantillaLudica();
                        plantilla.PlantillaLudicaId = int.Parse(e.CommandArgument.ToString());
                        lastObjectPlantilla = plantilla;
                        List<PreferenciaUsuario> preferencia = new List<PreferenciaUsuario>();
                        preferencia = controller.ConsultarPlantillaUsada(plantilla);
                        if (preferencia.Count == 0)
                        {
                            controller.EliminarPlantillaLudica(lastObjectPlantilla);
                            plantillasLudicas = controller.ConsultarPlantillasLudicas(InterfaceToFiltro());
                            LoadPlantillas();
                        }
                        else
                            ShowMessage("La plantilla lúdica a eliminar ya se encuentra en uso", MessageType.Information);
                        break;
                    }

                case "Sort":
                    {
                        break;
                    }


            }

        }

        protected void grdPlantillasLudicas_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void grdPlantillasLudicas_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (RenderEdit)
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                    btnEdit.Visible = true;
                }
                if (RenderDelete)
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnDelete");
                    btnEdit.Visible = true;
                }
            }
        }

        protected void grdPlantillasLudicas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                CambiarPagina(e.NewPageIndex);
            }
            catch (Exception ex)
            {
                ShowMessage("Inconsistencias  al cambiar la página", MessageType.Error);
            }
        }
        #endregion

        #region Métodos
        public PlantillaLudica InterfaceToFiltro()
        {
            PlantillaLudica plantilla = new PlantillaLudica();
            if (txtNombre.Text != string.Empty)
                plantilla.Nombre = txtNombre.Text;
            if (ddlEsPredeterminado.SelectedValue != string.Empty)
                plantilla.EsPredeterminado = ddlEsPredeterminado.SelectedValue == "true";
            plantilla.Activo = true;
            return plantilla;
        }

        private void LoadPlantillas()
        {
            grdPlantillasLudicas.DataSource = plantillasLudicas;
            grdPlantillasLudicas.DataBind();
        }

        protected override void DisplayCreateAction()
        {
            PnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {

            grdPlantillasLudicas.Visible = true;
        }

        protected override void DisplayUpdateAction()
        {
            RenderEdit = true;
        }

        protected override void DisplayDeleteAction()
        {
            RenderDelete = true;
        }

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCONFIGURACIONPLATAFORMA) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARCONFIGURACIONPLATAFORMA) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARCONFIGURACIONPLATAFORMA) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCONFIGURACIONPLATAFORMA) != null;
            bool eliminar = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARCONFIGURACIONPLATAFORMA) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (lectura)
                DisplayReadAction();
            if (creacion)
                DisplayCreateAction();
            if (edit)
                DisplayUpdateAction();
            if (eliminar)
                DisplayDeleteAction();
        }

        private void CambiarPagina(int nuevoIndicePagina)
        {
            grdPlantillasLudicas.PageIndex = nuevoIndicePagina;
            plantillasLudicas = controller.ConsultarPlantillasLudicas(InterfaceToFiltro());
            LoadPlantillas();
        }

        #region *****Message  Showing*****
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="messageType">Tipo de mensaje</param>
        private void ShowMessage(string message, MessageType messageType)
        {
            string type = string.Empty;

            switch (messageType)
            {
                case MessageType.Error:
                    type = "1";
                    break;
                case MessageType.Information:
                    type = "3";
                    break;
                case MessageType.Warning:
                    type = "2";
                    break;
            }

            ShowMessage(message, type);
        }
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="typeNotification">1: Error, 2: Advertencia, 3: Información</param>
        private void ShowMessage(string message, string typeNotification)
        {
            //Se ubican los controles que manejan el desplegado de error/advertencia/información
            if (Page.Master == null) return;
            Control m = Page.Master.FindControl("hdnLastMessage");
            Control t = Page.Master.FindControl("hdnShowMessage");

            if (m == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnLastMessage' en la MasterPage.\nEl error original es:\n" + message);
            if (t == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnShowMessage' en la MasterPage.\nEl error original es:\n" + message);

            if (m.GetType() != typeof(HiddenField) || t.GetType() != typeof(HiddenField))
                throw new Exception("No se pudo desplegar correctamente el error.\nAlguno de los controles de la MasterPage para el manejo de errores no es HiddenField.\nEl error original es:\n" + message);

            //Si el HiddenField del mensaje de error ya tiene un mensaje guardado, se da un 'enter' y se concatena el nuevo mensaje (errores acumulados)
            //En caso contrario, se pone el encabezado y se concatena el nuevo mensaje
            if (((HiddenField)m).Value != null && ((HiddenField)m).Value.Trim().CompareTo("") != 0)
                ((HiddenField)m).Value += "<br />";


            ((HiddenField)m).Value += message.Replace("\n", "<br />");
            ((HiddenField)t).Value = typeNotification;
        }
        #endregion
        #endregion

        #region Sesiones
        private List<PlantillaLudica> plantillasLudicas
        {
            get { return Session["plantillasLudicas"] != null ? Session["plantillasLudicas"] as List<PlantillaLudica> : null; }
            set { Session["plantillasLudicas"] = value; }
        }

        private PlantillaLudica lastObjectPlantilla
        {
            get { return Session["lastObjectPlantilla"] != null ? Session["lastObjectPlantilla"] as PlantillaLudica : null; }
            set { Session["lastObjectPlantilla"] = value; }
        }

        private bool RenderEdit = false;
        private bool RenderDelete = false;
        #endregion
    }
}