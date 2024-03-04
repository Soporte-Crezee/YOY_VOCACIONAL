using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.BO;
using POV.ConfiguracionActividades.BO;
using POV.Content.MasterPages;
using POV.Core.HerramientasDocente.Implement;
using POV.Core.HerramientasDocente.Interfaces;
using POV.Web.ServiciosActividades.Controllers;
using POV.Modelo.BO;
using System.Collections;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Expediente.Service;
using POV.Expediente.BO;
using Framework.Base.DataAccess;
using POV.Web.Helper;
using GP.SocialEngine.DAO;
using GP.SocialEngine.BO;
using POV.Modelo.Service;
using POV.Prueba.Diagnostico.Dinamica.Service;

namespace POV.Web.Pages.Actividades
{
    public partial class ConsultarAlumnosUC : System.Web.UI.UserControl
    {
        private ConsultarAlumnosGrupoController controller;
        IUserSession userSession = new UserSession();
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private UsuarioByAreaConocimientoRetHlp usuarioAreaRetHlp;
        private ModeloCtrl modeloCtrl;
        private PruebaDinamicaCtrl pruebaDinamicaCtrl;

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            controller = new ConsultarAlumnosGrupoController();
            usuarioAreaRetHlp = new UsuarioByAreaConocimientoRetHlp();
            modeloCtrl = new ModeloCtrl();
            pruebaDinamicaCtrl = new PruebaDinamicaCtrl();

            try
            {
                if (!IsPostBack)
                {

                    ListaGrupos = null;
                    ListaGrados = null;
                    ListaGrupGrad = null;
                    Session["ListaSeleccionados"] = null;
                    LoadAreasConocimiento();
                    ConsultarAlumnos();

                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error al cargar la pagina", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        #region Modificacion CG, LA
        private void LoadAreasConocimiento()
        {
            ddlAreaConocimiento.DataSource = GetAreasConocimientoAlumno();
            ddlAreaConocimiento.DataValueField = "ClasificadorID";
            ddlAreaConocimiento.DataTextField = "Nombre";
            ddlAreaConocimiento.DataBind();
        }

        private DataSet GetAreasConocimientoAlumno()
        {

            ArrayList arrAreaConocimiento = new ArrayList();
            Escuela escuela = userSession.CurrentEscuela;
            CicloEscolar cicloEscolar = userSession.CurrentCicloEscolar;
            Contrato contrato = userSession.Contrato;

            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = cicloEscolar });
            PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);

            var pruebaDinamica = (PruebaDinamica)pruebaPivoteContrato.Prueba;

            PruebaDinamica pruebaDinamicaModelo = pruebaDinamicaCtrl.RetrieveComplete(dctx, pruebaDinamica, false);
            DataSet DsClasificadores = modeloCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, new ModeloDinamico { ModeloID = pruebaDinamicaModelo.Modelo.ModeloID });

            return DsClasificadores;
        }

        #endregion

        /// <summary>
        /// Evento que envia a la asignación de actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAsignarActividades_Click(object sender, EventArgs e)
        {
            try
            {

                KeepSelection(gvAlumnosEnfasis);
                List<int> listaAsignacionId = (List<int>)Session["ListaSeleccionados"];
                DataTable dt = dtEnfasis.Clone();
                dt.Clear();
                foreach (int i in listaAsignacionId)
                {
                    DataRow dr = dtEnfasis.AsEnumerable().Select(r => r).Where(r => Convert.ToInt64(r["AlumnoID"]) == i).FirstOrDefault();
                    dt.ImportRow(dr);
                }
                Session["dtSeleccionados"] = dt;

                ClasificadorID = int.Parse(ddlAreaConocimiento.SelectedValue);

                //////se elimina la sesion
                Session["ListaSeleccionados"] = null;
                if (listaAsignacionId.Count > 0)
                {
                    ListaGrupos = null;
                    ListaGrados = null;
                    ListaGrupGrad = null;
                    Response.Redirect("~/Pages/Actividades/AsignarActividadesUI.aspx");
                }
                else
                {
                    ShowMessage("Por favor selecciona al menos un alumno.", Content.MasterPages.Site.EMessageType.Information);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error al enviar a realizar la asignación", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        /// <summary>
        /// Evento RowCommand del Gris de Alumnos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAlumnosEnfasis_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Page") return;
                List<AsignacionActividad> bo = new List<AsignacionActividad>();
                switch (e.CommandName)
                {
                    case "Detalle":
                        try
                        {
                            bo = controller.ConsultarActividadesAlumno(new Alumno()
                                {
                                    AlumnoID = Convert.ToInt64(e.CommandArgument)
                                }, userSession.CurrentDocente);

                            asignacionAct = bo;
                            ltvActividades.DataSource = asignacionAct;
                            ltvActividades.DataBind();

                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "AbrirConfirmacion", "AbrirConfirmacion();", true);
                        }
                        catch (Exception ex)
                        {
                            ShowMessage("Error al llenar el grid de la consulta", Content.MasterPages.Site.EMessageType.Error);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error al llenar el grid de la consulta", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        /// <summary>
        /// Paginado del Grid de alumnos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAlumnosEnfasis_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                KeepSelection((GridView)sender);
                gvAlumnosEnfasis.PageIndex = e.NewPageIndex;
                LlenarGrid();
            }
            catch (Exception ex)
            {
                ShowMessage("Error al realizar el paginado los resultados de la consulta", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        /// <summary>
        /// Paginado del Gris de Alumnos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAlumnosEnfasis_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RestoreSelection((GridView)sender);
            }
            catch (Exception ex)
            {
                ShowMessage("Error en el paginado del grid", Content.MasterPages.Site.EMessageType.Error);
            }

        }

        /// <summary>
        /// RowDataBaun del Grid de alumnos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAlumnosEnfasis_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    string AsignacionEnfasisId = e.Row.Cells[0].Text;
                    CheckBox cbSeleccionado = (CheckBox)e.Row.FindControl("cbSeleccionado");
                    cbSeleccionado.Attributes.Add("AlumnoID", AsignacionEnfasisId);

                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error al llenar el grid de la consulta", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        /// <summary>
        /// Evento del boton consultar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                ConsultarAlumnos();

            }
            catch (Exception ex)
            {
                ShowMessage("Error al consultar la información", Content.MasterPages.Site.EMessageType.Error);
            }
        }


        #endregion

        #region Métodos

        /// <summary>
        /// Metodo para mantener el seleccionado del grid
        /// </summary>
        /// <param name="grid"></param>
        public void RestoreSelection(GridView grid)
        {

            List<int> listaSeleccionados = Session["ListaSeleccionados"] as List<int>;

            if (listaSeleccionados == null)
                return;

            //
            // se comparan los registros de la pagina del grid con los recuperados de la Session
            // los coincidentes se devuelven para ser seleccionados
            //
            List<GridViewRow> result = (from item in grid.Rows.Cast<GridViewRow>()
                                        join item2 in listaSeleccionados
                                        on Convert.ToInt32(grid.DataKeys[item.RowIndex].Value) equals item2 into g
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
            // se obtienen los id de las notificaciones checkeadas de la pagina actual
            List<int> checkedAlumnos = (from item in grid.Rows.Cast<GridViewRow>()
                                        let check = (CheckBox)item.FindControl("cbSeleccionado")
                                        where check.Checked
                                        select Convert.ToInt32(grid.DataKeys[item.RowIndex].Value)).ToList();

            //
            // se recupera de session la lista de seleccionados previamente
            //
            List<int> listaSeleccionados = Session["ListaSeleccionados"] as List<int> ?? new List<int>();

            //
            // se cruzan todos los registros de la pagina actual del gridview con la lista de seleccionados,
            // si algun item de esa pagina fue marcado previamente no se devuelve
            //
            listaSeleccionados = (from item in listaSeleccionados
                                  join item2 in grid.Rows.Cast<GridViewRow>()
                                     on item equals Convert.ToInt32(grid.DataKeys[item2.RowIndex].Value) into g
                                  where !g.Any()
                                  select item).ToList();

            //
            // se agregan los seleccionados
            //
            listaSeleccionados.AddRange(checkedAlumnos);
            Session["ListaSeleccionados"] = listaSeleccionados;


        }

        /// <summary>
        /// Consulta la infromarcion de los grupos dependiendo la escuela con la que inicies sesion
        /// </summary>
        public void ConsultarGrupos()
        {

            ListaGrupGrad = controller.ConsultarGruposEscuelaDocente(userSession.CurrentEscuela, userSession.CurrentCicloEscolar, userSession.CurrentDocente);
            ListaGrados = new List<Grupo>();
            ListaGrupos = new List<Grupo>();

            foreach (GrupoCicloEscolar dr in ListaGrupGrad)
            {
                if (!ListaGrados.Exists(x => x.Grado == dr.Grupo.Grado))
                    ListaGrados.Add(dr.Grupo);
                if (!ListaGrupos.Exists(x => x.Nombre == dr.Grupo.Nombre))
                    ListaGrupos.Add(dr.Grupo);
            }
        }

        /// <summary>
        /// POblado del grid
        /// </summary>
        /// <param name="lstAlumnosEnfasis"></param>
        private void LlenarGrid()
        {
            gvAlumnosEnfasis.DataSource = dtEnfasis;
            gvAlumnosEnfasis.DataBind();
        }

        /// <summary>
        /// Consulta los alumnos de la base de datos de acuerdo al filtro proporcionado
        /// </summary>
        protected void ConsultarAlumnos()
        {
            dtEnfasis = controller.ConsultarAlumnos(userSession.CurrentCicloEscolar,
                                                    userSession.CurrentEscuela,
                                                    userSession.CurrentDocente,
                                                    InterfaceToFiltroAlumno(),
                                                    InterfaceToFiltroGrupo(),
                                                    InterfaceToFiltroAreaConocimiento(),
                                                    userSession.CurrentUser);
            LlenarGrid();
        }

        /// <summary>
        /// Obtiene los datos de la interfaz filtros de alumnos
        /// </summary>
        /// <returns>Alumno</returns>
        public Alumno InterfaceToFiltroAlumno()
        {
            Alumno alumnoFiltro = new Alumno();
            if (txtNombres.Text.Trim() != string.Empty)
                alumnoFiltro.Nombre = "%" + txtNombres.Text.Trim() + "%";
            alumnoFiltro.Estatus = true;
            return alumnoFiltro;
        }

        /// <summary>
        /// Obtiene los datos de la interfaz del Grupo
        /// </summary>
        /// <returns>Grupo</returns>
        public Grupo InterfaceToFiltroGrupo()
        {
            Grupo grupo = new Grupo();
            return grupo;
        }

        public AreaConocimiento InterfaceToFiltroAreaConocimiento()
        {
            AreaConocimiento areaConocimiento = new AreaConocimiento();
            if (ddlAreaConocimiento.SelectedValue != string.Empty)
                areaConocimiento.AreaConocimentoID = int.Parse(ddlAreaConocimiento.SelectedValue);
            return areaConocimiento;
        }

        private void ShowMessage(string message, Site.EMessageType messageType)
        {
            Site site = (Site)Page.Master;
            site.ShowMessage(message, messageType);
        }

        #endregion

        #region Sesiones

        /// <summary>
        /// Grupos y Grados
        /// </summary>
        private List<GrupoCicloEscolar> ListaGrupGrad
        {
            get { return Session["ListaGrupGrad"] as List<GrupoCicloEscolar>; }
            set { Session["ListaGrupGrad"] = value; }
        }
        /// <summary>
        /// Sesion de los Grupos
        /// </summary>
        private List<Grupo> ListaGrupos
        {
            get { return (List<Grupo>)Session["ListaGrupos"]; }
            set { Session["ListaGrupos"] = value; }
        }
        /// <summary>
        /// Sesion de los Grados
        /// </summary>
        private List<Grupo> ListaGrados
        {
            get { return (List<Grupo>)Session["ListaGrados"]; }
            set { Session["ListaGrados"] = value; }
        }

        /// <summary>
        /// Sesion de las actividades y sus tareas
        /// </summary>
        private List<AsignacionActividad> asignacionAct
        {
            get
            {
                return (List<AsignacionActividad>)Session["asignacionAct"];
            }
            set
            {
                Session["asignacionAct"] = value;
            }
        }

        DataTable dtEnfasis
        {
            set
            {
                Session["dtEnfasis"] = value;
            }
            get
            {
                return Session["dtEnfasis"] as DataTable;
            }
        }

        DataSet dtAreas
        {
            set
            {
                Session["dtAreas"] = value;
            }
            get
            {
                return Session["dtAreas"] as DataSet;
            }
        }

        private int? ClasificadorID
        {
            get { return Session["ClasificadorID"] as int?; }
            set { Session["ClasificadorID"] = value; }
        }
        #endregion
    }
}