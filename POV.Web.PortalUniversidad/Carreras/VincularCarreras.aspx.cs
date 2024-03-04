using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.Administracion.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.Universidades.Implements;
using POV.Core.Universidades.Interfaces;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalUniversidad.Helper;
using POV.Web.PortalUniversidad.AppCode.Page;
using POV.CentroEducativo.Services;
using System.Collections;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Modelo.BO;
using Framework.Base.DataAccess;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Modelo.Service;
using System.Web;
using POV.Modelo.Context;
using System.Globalization;

namespace POV.Web.PortalUniversidad.Carreras
{
    public partial class VincularCarreras : CatalogPage
    {
        #region propiedades de la clase

        public DataSet DsCarrerasUniversidad
        {
            get { return Session["CarreraUniversidad"] != null ? Session["CarreraUniversidad"] as DataSet : null; }
            set { Session["CarreraUniversidad"] = value; }
        }

        private List<long> ListaSeleccionados
        {
            get { return Session["ListaSeleccionados"] as List<long>; }
            set { Session["ListaSeleccionados"] = value; }
        }
        
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private CarreraCtrl carreraCtrl;
        private UniversidadCtrl universidadCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private PruebaDinamicaCtrl pruebaDinamicaCtrl;
        private ModeloCtrl modeloCtrl;
        #endregion

        public VincularCarreras()
        {
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            carreraCtrl = new CarreraCtrl(null);
            universidadCtrl = new UniversidadCtrl(null);
            pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
            modeloCtrl = new ModeloCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession != null && userSession.CurrentUniversidad != null && userSession.CurrentCicloEscolar != null)
                {
                    if (!IsPostBack)
                    {
                        ConsultarCarreras(new Carrera());
                        LoadAreasConocimiento();
                    }
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Carrera carrera = UserInterfaceToData();
                RecorrerGridView();
                ConsultarCarreras(carrera); 
                RestoreSelection(grdCarreras);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void grdCarreras_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                KeepSelection((GridView)sender);
                grdCarreras.PageIndex = e.NewPageIndex;
                grdCarreras.DataSource = DsCarrerasUniversidad;
                grdCarreras.DataBind(); 
            }
            catch (Exception ex)
            {
                ShowMessage("Error al llenar el grid de la consulta", MessageType.Error);
            }
        }

        protected void grdCarreras_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RestoreSelection((GridView)sender);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "reloadControlsGrid();", true);
            }
            catch (Exception ex)
            {
                ShowMessage("Error al llenar el grid de la consulta", MessageType.Error);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            // Recorre el grid para que la ListaSeleccionados obtenga los id's
            RecorrerGridView();

            if (ListaSeleccionados.Count() == 0)
            {
                ShowMessage("Debes seleccionar al menos una carrera", MessageType.Error);
            }
            else
            {
                #region *** Vincular Tutor-Alumno
                var objeto = new object();
                var contexto = new Contexto(objeto);

                UniversidadCtrl universidadCtrl = new UniversidadCtrl(contexto);
                var universidadSave = universidadCtrl.RetrieveWithRelationship(userSession.CurrentUniversidad, true).FirstOrDefault();

                CarreraCtrl carreraCtrl = new CarreraCtrl(contexto);
                List<Carrera> converCarrerra = new List<Carrera>();
                foreach (var item in ListaSeleccionados)
                {
                    Carrera carrera = new Carrera();
                    carrera.CarreraID = Convert.ToInt64(item);
                    Carrera itemCarrera = carreraCtrl.Retrieve(carrera, false).First();

                    converCarrerra.Add(itemCarrera);
                }

                foreach (Carrera tut in converCarrerra)
                {
                    Carrera carreraSeleccionado = carreraCtrl.Retrieve(tut, true).FirstOrDefault();
                    if (universidadSave.Carreras.FirstOrDefault(x => x.CarreraID == tut.CarreraID) == null)
                    {
                        universidadSave.Carreras.Add(carreraSeleccionado);
                    }
                }
                universidadCtrl.Update(universidadSave);
                contexto.Commit(objeto);
                contexto.Dispose();

                // Se recarga las relaciones de la universidad-carrera
                UniversidadCtrl upSEssion = new UniversidadCtrl(null);
                userSession.CurrentUniversidad = upSEssion.RetrieveWithRelationship(universidadSave, false).FirstOrDefault();
                ListaSeleccionados = null;
                redirector.GoToConsultarCarreras(false);
                #endregion
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            redirector.GoToConsultarCarreras(false);
        }
        #endregion

        private void ConsultarCarreras( Carrera filter)
        {
            List<Carrera> listaCarreras = carreraCtrl.Retrieve(filter, false);
            
            List<Carrera> listaVinculados = userSession.CurrentUniversidad.Carreras.ToList();

            List<Carrera> unificada = (from item in listaCarreras
                                       join item2 in listaVinculados
                                       on item.CarreraID equals item2.CarreraID into g
                                       where !g.Any()
                                       select item).ToList();

            // Formar dataset
            DataSet dsCompose = new DataSet();
            dsCompose.Tables.Add(new DataTable());
            dsCompose.Tables[0].Columns.Add(new DataColumn("CarreraID", typeof(long)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("NombreCarrera", typeof(string)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("Descripcion", typeof(string)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("Clasificador.Nombre", typeof(string)));

            if (unificada != null) 
            {
                foreach (Carrera car in unificada)
                {
                    DataRow dr = dsCompose.Tables[0].NewRow();
                    dr.SetField("CarreraID", car.CarreraID);
                    dr.SetField("NombreCarrera", car.NombreCarrera);
                    dr.SetField("Descripcion", car.Descripcion);
                    dr.SetField("Clasificador.Nombre", car.Clasificador.Nombre);

                    dsCompose.Tables[0].Rows.Add(dr);
                }
            }
            if (filter != null) 
            {
                if (!string.IsNullOrEmpty(filter.NombreCarrera) || (filter.Clasificador != null && filter.Clasificador.Nombre != string.Empty)) 
                {
                    // agregar filtros
                    string strFilter = string.Empty;
                    if (!string.IsNullOrEmpty(filter.NombreCarrera))
                        strFilter += string.Format("AND NombreCarrera LIKE '%{0}%' ", EscapeLike(filter.NombreCarrera));

                    if (filter.Clasificador != null && filter.Clasificador.Nombre != string.Empty)
                        strFilter += string.Format("AND Clasificador.Nombre = '{0}' ", EscapeLike(filter.Clasificador.Nombre));

                    if (strFilter.StartsWith("AND"))
                        strFilter = strFilter.Substring(4);

                    if (strFilter.Trim().Length > 0) 
                    {
                        DataView dataView = new DataView(dsCompose.Tables[0]);
                        dataView.RowFilter = strFilter;
                        strFilter = string.Empty;
                        dsCompose.Tables.Clear();
                        dsCompose.Tables.Add(dataView.ToTable());
                    }
                }
            }

            DsCarrerasUniversidad = dsCompose;
            grdCarreras.DataSource = dsCompose;
            grdCarreras.DataBind(); 
        }

        #region *** UserInterface to Data ***
        private Carrera UserInterfaceToData()
        {
            Carrera carrera = new Carrera();
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
                carrera.NombreCarrera = txtNombre.Text.Trim();

            if (ddlAreaConocimiento.SelectedValue != string.Empty)
            {
                carrera.Clasificador = new Clasificador() { Nombre = ddlAreaConocimiento.SelectedItem.Text };
            }

            return carrera;
        }
        #endregion

        #region *** metodos auxiliares ***

        private void LoadAreasConocimiento() 
        {
            ddlAreaConocimiento.DataSource = GetAreasConocimiento();
            ddlAreaConocimiento.DataValueField = "ClasificadorID";
            ddlAreaConocimiento.DataTextField = "Nombre";
            ddlAreaConocimiento.DataBind();
            ddlAreaConocimiento.Items.Insert(0, new ListItem("Selecciona Area Conocimiento", ""));
        }

        private DataSet GetAreasConocimiento()
        {
            ArrayList arrAreaConocimiento = new ArrayList();
            Escuela escuela = userSession.CurrentEscuela;
            CicloEscolar cicloEscolar = userSession.CurrentCicloEscolar;
            Contrato contrato = new Contrato() { ContratoID = 2 };

            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = cicloEscolar });
            PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);

            var pruebaDinamica = (PruebaDinamica)pruebaPivoteContrato.Prueba;
            PruebaDinamica pruebaDinamicaModelo = pruebaDinamicaCtrl.RetrieveComplete(dctx, pruebaDinamica, false);
            DataSet DsClasificadores = modeloCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, new ModeloDinamico { ModeloID = pruebaDinamicaModelo.Modelo.ModeloID });

            return DsClasificadores;
        }

        private string EscapeLike(string valueWithoutWildcards)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueWithoutWildcards.Length; i++)
            {
                char c = valueWithoutWildcards[i];
                if (c == '*' || c == '%' || c == '[' || c == ']')
                    sb.Append("[").Append(c).Append("]");
                else if (c == '\'')
                    sb.Append("''");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private void RecorrerGridView() 
        {
            List<long> checkedProd = (from item in grdCarreras.Rows.Cast<GridViewRow>()
                                        let check = (CheckBox)item.FindControl("chkSeleccionado")
                                        where check.Checked
                                        select Convert.ToInt64(grdCarreras.DataKeys[item.RowIndex].Value)).ToList();

            // se recupera de session la lista de seleccionados previamente
            List<long> productsIdSel = HttpContext.Current.Session["ListaSeleccionados"] as List<long>;

            if (productsIdSel == null)
                productsIdSel = new List<long>();


            // se cruzan todos los registros de la pagina actual del gridview con la lista de seleccionados,
            // si algun item de esa pagina fue marcado previamente no se devuelve
            productsIdSel = (from item in productsIdSel
                             join item2 in grdCarreras.Rows.Cast<GridViewRow>()
                                on item equals Convert.ToInt64(grdCarreras.DataKeys[item2.RowIndex].Value) into g
                             where !g.Any()
                             select item).ToList();

            // se agregan los seleccionados
            productsIdSel.AddRange(checkedProd);

            Session["ListaSeleccionados"] = productsIdSel;
            ListaSeleccionados = productsIdSel;
        }

        /// <summary>
        /// Metodo para mantener el seleccionado del grid
        /// </summary>
        /// <param name="grid"></param>
        public void RestoreSelection(GridView grid) 
        {
            List<long> productsIdSel = Session["ListaSeleccionados"] as List<long>;

            if (productsIdSel == null)
                return;

            // se comparan los registros de la pagina del grid con los recuperados de la Session
            // los coincidentes se devuelven para ser seleccionados
            List<GridViewRow> result = (from item in grid.Rows.Cast<GridViewRow>()
                                        join item2 in productsIdSel
                                     on Convert.ToInt64(grid.DataKeys[item.RowIndex].Value) equals item2 into g
                                     where g.Any()
                                     select item).ToList();

            // se recorre cada item para marcarlo
            result.ForEach(x => ((CheckBox)x.FindControl("chkSeleccionado")).Checked = true);
        }

        /// <summary>
        /// Mantiene el seleccionado del grid
        /// </summary>
        /// <param name="grid"></param>
        public void KeepSelection(GridView grid) 
        {
            // se obtienen los id's de las notificaciones checkeadas de la pagina actual
            List<long> chekedCarreras = (from item in grid.Rows.Cast<GridViewRow>()
                                         let check = (CheckBox)item.FindControl("chkSeleccionado")
                                         where check.Checked
                                         select Convert.ToInt64(grid.DataKeys[item.RowIndex].Value)).ToList();

            // se recupera la session de la lista de seleccionados previamente
            List<long> productsIdSel = Session["ListaSeleccionados"] as List<long> ?? new List<long>();

            if (productsIdSel == null)
                productsIdSel = new List<long>();

            // se cruzan todos los registros de la pagina actual del gridview con la lista de seleccionados,
            // si algun item de esa pagina fue marcado previamente no se devuelve
            productsIdSel = (from item in productsIdSel
                                  join item2 in grid.Rows.Cast<GridViewRow>()
                                  on item equals Convert.ToInt64(grid.DataKeys[item2.RowIndex].Value) into g
                                  where !g.Any()
                                  select item).ToList();

            // se agregan los seleccionados
            productsIdSel.AddRange(chekedCarreras);

            Session["ListaSeleccionados"] = productsIdSel;
            ListaSeleccionados = productsIdSel;
        }
        #endregion

        #region AUTORIZACION DE LA PAGINA


        protected void grdCarreras_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    string AsignacionEnfasisId = e.Row.Cells[0].Text;
                    CheckBox cbSeleccionado = (CheckBox)e.Row.FindControl("chkSeleccionado");
                    cbSeleccionado.Attributes.Add("CarreraID", AsignacionEnfasisId);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error al llenar el grid de la consulta", MessageType.Error);
            }
        }

        private bool RenderEdit = false;
        private bool RenderDelete = false;

        protected override void DisplayCreateAction()
        {
            PnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {

            grdCarreras.Visible = true;
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
            List<Permiso> permisos = userSession.CurrentPrivilegiosUniversidad.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOCARRERAS) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOCARRERAS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOCARRERAS) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOCARRERAS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOCARRERAS) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (lectura)
                DisplayReadAction();
            if (creacion)
                DisplayCreateAction();
            if (delete)
                DisplayDeleteAction();
            if (edit)
                DisplayUpdateAction();
        }
        #endregion

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

        #region Compementos de paginacion
        protected void grdCarreras_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                SetPagerButtonStates(grdCarreras, e.Row, this, "ddlPageSelector");
            }

            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Page_PreRender(grdCarreras, e.Row, this, "DropDownListPageSize");
            }
        }

        protected void ddlPageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeepSelection(grdCarreras);
            grdCarreras.PageIndex = ((DropDownList)sender).SelectedIndex;
            grdCarreras.DataSource = DsCarrerasUniversidad;
            grdCarreras.DataBind();
            RestoreSelection(grdCarreras);
        }

        protected void Page_PreRender(GridView gridView, GridViewRow gvPagerRow, Page page, string DDlPager)
        {
            if (gridView != null)
            {
                DropDownList DropDownListPageSize = (DropDownList)gvPagerRow.FindControl(DDlPager);
                DropDownListPageSize.SelectedValue = grdCarreras.PageSize.ToString(CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Setting the Pager
        /// </summary>
        /// <param name="gridView">Gridview Control ID</param>
        /// <param name="gvPagerRow">GridView Row</param>
        /// <param name="page">Page</param>
        /// <param name="DDlPager">Drop down list ID</param>
        public void SetPagerButtonStates(GridView gridView, GridViewRow gvPagerRow, Page page, string DDlPager)
        {
            // to Get No of pages and Page Navigation
            int pageIndex = gridView.PageIndex;
            int pageCount = gridView.PageCount;
            ImageButton btnFirst = (ImageButton)gvPagerRow.FindControl("ImgeBtnFirst");
            ImageButton btnPrevious = (ImageButton)gvPagerRow.FindControl("ImgbtnPrevious");
            ImageButton btnNext = (ImageButton)gvPagerRow.FindControl("ImgbtnNext");
            ImageButton btnLast = (ImageButton)gvPagerRow.FindControl("ImgbtnLast");
            btnFirst.Enabled = btnPrevious.Enabled = (pageIndex != 0);
            btnNext.Enabled = btnLast.Enabled = (pageIndex < (pageCount - 1));
            DropDownList ddlPageSelector = (DropDownList)gvPagerRow.FindControl(DDlPager);
            ddlPageSelector.Items.Clear();
            for (int i = 1; i <= gridView.PageCount; i++)
            {
                ddlPageSelector.Items.Add(i.ToString());
            }
            ddlPageSelector.SelectedIndex = pageIndex;
            string strPgeIndx = Convert.ToString(gridView.PageIndex + 1) + " de "
                                + gridView.PageCount.ToString();

            Label lblpageindx = (Label)gvPagerRow.FindControl("lblpageindx");
            lblpageindx.Text += strPgeIndx;
                
        }
        
        protected void DropDownListPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeepSelection(grdCarreras);
            DropDownList dropdownlistpagersize = (DropDownList)sender;
            
            grdCarreras.PageSize = Convert.ToInt32(dropdownlistpagersize.SelectedValue, CultureInfo.CurrentCulture);
            int pageindex = grdCarreras.PageIndex;
            grdCarreras.DataSource = DsCarrerasUniversidad;
            grdCarreras.DataBind();
            grdCarreras.BottomPagerRow.Visible = true;
            RestoreSelection(grdCarreras);

            if (grdCarreras.PageIndex != pageindex)
            {
                //Si el índice de la página cambió, significa que la página anterior no era válida y se ha ajustado. Vuelva a enlazar al control de relleno con la página ajustada
                KeepSelection(grdCarreras);
                grdCarreras.DataSource = DsCarrerasUniversidad;
                grdCarreras.DataBind();
                
                grdCarreras.BottomPagerRow.Visible = true;
                RestoreSelection(grdCarreras);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "reloadControlsGrid();", true);
        }
        #endregion
    }
}