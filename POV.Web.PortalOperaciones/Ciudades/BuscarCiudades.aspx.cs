using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Web.PortalOperaciones.Helper;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;


namespace POV.Web.PortalOperaciones.Ciudades
{
    public partial class BuscarCiudades : CatalogPage
    {
        private CiudadCtrl ciudadCtrl;
        private EstadoCtrl estadoCtrl;
        private PaisCtrl paisCtrl;

        #region *** propiedades de clase ***
        private Ciudad LastObject
        {
            get { return Session["lastCiudad"] != null ? Session["lastCiudad"] as Ciudad : null; }
            set { Session["lastCiudad"] = value; }
        }
        private DataSet DSCiudades
        {
            get { return Session["ciudades"] != null ? Session["ciudades"] as DataSet : null; }
            set { Session["ciudades"] = value; }
        }
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }
        #endregion

        public BuscarCiudades()
        {
            ciudadCtrl= new CiudadCtrl();
            estadoCtrl = new EstadoCtrl();
            paisCtrl = new PaisCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPaises();
                ddlEstado.Enabled = ddlEstado.Items.Count > 0;
                LoadCiudades();
            }

        }

        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pais pais = GetPaisFromUI();
            if (pais.PaisID > 0)
            {
                LoadEstados(pais);
                if (ddlEstado.Items.Count > 0)
                    ddlEstado.Enabled = true;
            }
            else
            {
                ddlEstado.Items.Clear();
                ddlEstado.Enabled = false;
            }


        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                Ciudad ciudad = UserInterfaceToData();
                DSCiudades = ciudadCtrl.Retrieve(ConnectionHlp.Default.Connection, ciudad);
                DSCiudades = ConfigureGridResults(DSCiudades);
                grdCiudades.DataSource = DSCiudades;
                grdCiudades.DataBind();
            }
            else
            {
                ShowMessage(validateMessage, MessageType.Error);
            }
        }

        protected void grdCiudades_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataSet dataSet = DSCiudades;
            if (dataSet != null)
            {
                DataView dataView = new DataView(dataSet.Tables["Ciudad"]);
                dataView.Sort = e.SortExpression + " " + GetSortDirection();
                DSCiudades.Tables.Clear();
                DSCiudades.Tables.Add(dataView.ToTable());
                grdCiudades.DataSource = DSCiudades;
                grdCiudades.DataBind();
            }
        }
        protected void grdCiudades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        try
                        {
                            Ciudad ciudad = new Ciudad();
                            int ciudadId = Convert.ToInt32(e.CommandArgument);
                            ciudad.CiudadID = ciudadId;
                            Localidad localidad = new Localidad();
                            localidad.Ciudad = ciudad;
                            LocalidadCtrl localidadCtrl = new LocalidadCtrl();

                            DataSet dslocalidades = localidadCtrl.Retrieve(ConnectionHlp.Default.Connection, localidad);
                            if (dslocalidades.Tables["Localidad"].Rows.Count > 0)
                                ShowMessage("La ciudad tiene localidades relacionadas, elimínelas antes de eliminar la ciudad", MessageType.Information);
                            else
                            {
                                UbicacionCtrl ubicacionCtrl = new UbicacionCtrl();
                                DataSet dsUbicaciones = ubicacionCtrl.Retrieve(ConnectionHlp.Default.Connection, new Ubicacion { Ciudad = ciudad });
                                if (dsUbicaciones.Tables["Ubicacion"].Rows.Count > 0)
                                    ShowMessage("La ciudad tiene ubicaciones relacionadas, elimínelas antes de eliminar la ciudad", MessageType.Information);
                                else
                                {

                                    ciudadCtrl.Delete(ConnectionHlp.Default.Connection, ciudad);
                                    RemoveCiudadFromDataSet(DSCiudades, ciudad);
                                    grdCiudades.DataSource = DSCiudades;
                                    grdCiudades.DataBind();
                                    ShowMessage("La ciudad se eliminó con éxito", MessageType.Information);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message, MessageType.Error);
                        }
                        break;
                    }
                case "editar":
                    {
                        Ciudad ciudad = new Ciudad();
                        ciudad.CiudadID = int.Parse(e.CommandArgument.ToString());
                        LastObject = ciudad;
                        Response.Redirect("EditarCiudad.aspx");
                        break;
                    }
                case "Sort":
                    {
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }

            }

        }
        #endregion

        #region *** validaciones ***
        private string ValidateData()
        {
            string sError = string.Empty;
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                if (txtNombre.Text.Trim().Length > 30)
                    sError = ", el tamaño del texto no debe ser mayor a 30 caracteres)";
            }

            if (sError.Length > 0)
            {
                sError = sError.Substring(2);
                sError = sError.Replace(sError[0], char.ToUpper(sError[0]));
            }
            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void LoadPaises()
        {
            ddlPais.DataSource = ConfigureDropDownPaises(paisCtrl.Retrieve(ConnectionHlp.Default.Connection, new Pais()));
            ddlPais.DataTextField = "Nombre";
            ddlPais.DataValueField = "PaisID";
            ddlPais.DataBind();
        }
        private void LoadEstados(Pais pais)
        {
            if (pais.PaisID != null)
            {
                ddlEstado.DataSource =
                    ConfigureDropDownEstados(estadoCtrl.Retrieve(ConnectionHlp.Default.Connection, new Estado { Pais = pais }));
                ddlEstado.DataTextField = "Nombre";
                ddlEstado.DataValueField = "EstadoID";
                ddlEstado.DataBind();
            }
        }
        private void LoadCiudades()
        {
            DSCiudades = ciudadCtrl.Retrieve(ConnectionHlp.Default.Connection, new Ciudad());
            DSCiudades = ConfigureGridResults(DSCiudades);
            grdCiudades.DataSource = DSCiudades;
            grdCiudades.DataBind();
        }
        private DataSet ConfigureDropDownPaises(DataSet ds)
        {
            if (ds.Tables["Pais"].Rows.Count == 0)
                return ds;

            DataRow row = ds.Tables["Pais"].NewRow();
            row["PaisID"] = 0;
            row["Nombre"] = "Todos";
            ds.Tables["Pais"].Rows.InsertAt(row, 0);
            return ds;
        }
        private DataSet ConfigureDropDownEstados(DataSet ds)
        {
            if (ds.Tables["Estado"].Rows.Count == 0)
                return ds;

            DataRow row = ds.Tables["Estado"].NewRow();
            row["EstadoID"] = 0;
            row["Nombre"] = "Todos";
            ds.Tables["Estado"].Rows.InsertAt(row, 0);
            return ds;

        }
        #endregion

        #region *** UserInterface to Data ***
        private Pais GetPaisFromUI()
        {
            Pais pais = new Pais();
            int paisId = 0;
            bool result = int.TryParse(ddlPais.SelectedValue, out paisId);
            if (paisId > 0)
            {
                pais.PaisID = paisId;
            }
            return pais;
        }
        private Estado GetEstadoFromUI()
        {
            Estado estado = new Estado();
            int estadoId = 0;
            bool result = int.TryParse(ddlEstado.SelectedValue, out estadoId);
            if (estadoId > 0)
            {
                estado.EstadoID = estadoId;
            }
            return estado;
        }
        private Ciudad UserInterfaceToData()
        {
            Ciudad ciudad = new Ciudad();
            Estado estado = GetEstadoFromUI();
            Pais pais = GetPaisFromUI();

            if (estado.EstadoID != null)
                ciudad.Estado = estado;
            else ciudad.Estado = new Estado();

            if (pais.PaisID != null)
                ciudad.Estado.Pais = pais;

            if (txtNombre.Text.Trim().Length > 0)
                ciudad.Nombre = txtNombre.Text.Trim() + "%";

            return ciudad;
        }
        #endregion

        #region *** metodos auxiliares ***
        private DataSet ConfigureGridResults(DataSet ds)
        {
            List<Estado> listaEstados = new List<Estado>();
            ds.Tables["Ciudad"].Columns.Add("Estado");
            foreach (DataRow row in ds.Tables["Ciudad"].Rows)
            {
                Estado edo = listaEstados.SingleOrDefault(e => e.EstadoID == int.Parse(row["EstadoID"].ToString()));
                if (edo != null)
                {
                    row["Estado"] = edo.Nombre;
                }
                else
                {
                    Estado estado = new Estado { EstadoID = int.Parse(row["EstadoID"].ToString()) };
                    estado = estadoCtrl.LastDataRowToEstado(estadoCtrl.Retrieve(ConnectionHlp.Default.Connection, estado));
                    row["Estado"] = estado.Nombre;
                    listaEstados.Add(estado);
                    continue;
                }
            }
            return ds;
        }

        private void RemoveCiudadFromDataSet(DataSet ds, Ciudad ciudad)
        {
            string query = "CiudadID =" + ciudad.CiudadID;
            DataRow[] dr = ds.Tables["Ciudad"].Select(query);

            if (dr.Count() == 1)
                ds.Tables["Ciudad"].Rows.Remove(dr[0]);
        }
        private string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";
                    break;

                case "DESC":
                    GridViewSortDirection = "ASC";
                    break;
            }

            return GridViewSortDirection;
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
        
        #region AUTORIZACION DE LA PAGINA

        protected void grdCiudades_DataBound(object sender, GridViewRowEventArgs e)
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
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btndelete");
                    btnDelete.Visible = true;
                }
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

            grdCiudades.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCIUDADES) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARCIUDADES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARCIUDADES) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARCIUDADES) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCIUDADES) != null;

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

    }
}