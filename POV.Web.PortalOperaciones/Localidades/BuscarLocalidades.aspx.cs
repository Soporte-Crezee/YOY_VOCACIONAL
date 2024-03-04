using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Comun.Service;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.Localidades
{
    public partial class BuscarLocalidades : CatalogPage
    {
        private LocalidadCtrl localidadCtrl;
        private CiudadCtrl ciudadCtrl;
        private EstadoCtrl estadoCtrl;
        private PaisCtrl paisCtrl;

        #region *** propiedades de clase ***
        private Localidad LastObject
        {
            get { return Session["lastLocalidad"] != null ? Session["lastLocalidad"] as Localidad : null; }
            set { Session["lastLocalidad"] = value; }
        }
        private DataSet DSLocalidades
        {
            get { return Session["localidades"] != null ? Session["localidades"] as DataSet : null; }
            set { Session["localidades"] = value; }
        }
        #endregion

        public BuscarLocalidades() {
            localidadCtrl = new LocalidadCtrl();
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
                ddlCiudad.Enabled = ddlCiudad.Items.Count > 0;
                LoadLocalidades();
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
                ddlCiudad.Items.Clear();
                ddlCiudad.Enabled = false;
            }


        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            Estado estado = GetEstadoFromUI();
            if (estado.EstadoID > 0)
            {
                LoadCiudades(estado);
                if (ddlCiudad.Items.Count > 0)
                    ddlCiudad.Enabled = true;
            }
            else
            {
                ddlCiudad.Items.Clear();
                ddlCiudad.Enabled = false;
            }

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                Localidad localidad = UserInterfaceToData();
                DSLocalidades = localidadCtrl.Retrieve(ConnectionHlp.Default.Connection, localidad);
                DSLocalidades = ConfigureGridResults(DSLocalidades);
                grdLocalidades.DataSource = DSLocalidades;
                grdLocalidades.DataBind();
            }
            else
            {
                ShowMessage(validateMessage, MessageType.Error);
            }
        }

        protected void grdLocalidades_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataSet dataSet = DSLocalidades;
            if (dataSet != null)
            {
                DataView dataView = new DataView(dataSet.Tables["Localidad"]);
                dataView.Sort = e.SortExpression + " " + GetSortDirection();
                DSLocalidades.Tables.Clear();
                DSLocalidades.Tables.Add(dataView.ToTable());
                grdLocalidades.DataSource = DSLocalidades;
                grdLocalidades.DataBind();
            }
        }
        protected void grdLocalidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        try
                        {
                            Localidad localidad = new Localidad();
                            int localidadID = Convert.ToInt32(e.CommandArgument);
                            localidad.LocalidadID = localidadID;

                            Ubicacion ubicacion = new Ubicacion();
                            ubicacion.Localidad = localidad;
                            UbicacionCtrl ubicacionCtrl = new UbicacionCtrl();

                            DataSet dsUbicaciones = ubicacionCtrl.Retrieve(ConnectionHlp.Default.Connection, ubicacion);
                            if (dsUbicaciones.Tables["Ubicacion"].Rows.Count > 0)
                                ShowMessage("La localidad tiene ubicaciones relacionadas, elimínelas antes de eliminar la localidad", MessageType.Information);
                            else
                            {
                                localidadCtrl.Delete(ConnectionHlp.Default.Connection, localidad);
                                RemoveLocalidadFromDataSet(DSLocalidades, localidad);
                                grdLocalidades.DataSource = DSLocalidades;
                                grdLocalidades.DataBind();
                                ShowMessage("La localidad se eliminó con éxito", MessageType.Information);
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
                        Localidad localidad = new Localidad();
                        localidad.LocalidadID = int.Parse(e.CommandArgument.ToString());
                        LastObject = localidad;
                        Response.Redirect("EditarLocalidad.aspx");
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
        private void LoadCiudades(Estado estado)
        {
            if (estado.EstadoID != null)
            {
                ddlCiudad.DataSource =
                    ConfigureDropDownCiudades(ciudadCtrl.Retrieve(ConnectionHlp.Default.Connection, new Ciudad { Estado = estado }));
                ddlCiudad.DataTextField = "Nombre";
                ddlCiudad.DataValueField = "CiudadID";
                ddlCiudad.DataBind();
            }
        }
        private void LoadLocalidades()
        {
            DSLocalidades = localidadCtrl.Retrieve(ConnectionHlp.Default.Connection, new Localidad());
            DSLocalidades = ConfigureGridResults(DSLocalidades);
            grdLocalidades.DataSource = DSLocalidades;
            grdLocalidades.DataBind();
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

        private Ciudad GetCiudadFromUI()
        {
            Ciudad ciudad = new Ciudad();
            int ciudadId = 0;
            bool result = int.TryParse(ddlCiudad.SelectedValue, out ciudadId);
            if (ciudadId > 0)
            {
                ciudad.CiudadID = ciudadId;
            }
            return ciudad;
        }
        private Localidad UserInterfaceToData()
        {
            Localidad localidad = new Localidad();
            Ciudad ciudad = GetCiudadFromUI();
            Estado estado = GetEstadoFromUI();
            Pais pais = GetPaisFromUI();
            if (ciudad.CiudadID != null)
                localidad.Ciudad = ciudad;
            else localidad.Ciudad = new Ciudad();

            if (estado.EstadoID != null)
                localidad.Ciudad.Estado = estado;
            else localidad.Ciudad.Estado = new Estado();

            if (pais.PaisID != null)
                localidad.Ciudad.Estado.Pais = pais;

            if (txtNombre.Text.Trim().Length > 0)
                localidad.Nombre = "%" + txtNombre.Text.Trim() + "%";

            return localidad;
        }
        #endregion

        #region *** metodos auxiliares ***
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
        private DataSet ConfigureDropDownCiudades(DataSet ds)
        {
            if (ds.Tables["Ciudad"].Rows.Count == 0)
                return ds;

            DataRow row = ds.Tables["Ciudad"].NewRow();
            row["CiudadID"] = 0;
            row["Nombre"] = "Todos";
            ds.Tables["Ciudad"].Rows.InsertAt(row, 0);
            return ds;

        }
        private DataSet ConfigureGridResults(DataSet ds)
        {
            List<Ciudad> listaCiudades = new List<Ciudad>();
            ds.Tables["Localidad"].Columns.Add("Ciudad");
            foreach (DataRow row in ds.Tables["Localidad"].Rows)
            {
                Ciudad cd = listaCiudades.SingleOrDefault(c => c.CiudadID == int.Parse(row["CiudadID"].ToString()));
                if (cd != null)
                {
                    row["Ciudad"] = cd.Nombre;
                }
                else
                {
                    Ciudad ciudad = new Ciudad { CiudadID = int.Parse(row["CiudadID"].ToString()) };
                    ciudad = ciudadCtrl.LastDataRowToCiudad(ciudadCtrl.Retrieve(ConnectionHlp.Default.Connection, ciudad));
                    row["Ciudad"] = ciudad.Nombre;
                    listaCiudades.Add(ciudad);
                    continue;
                }
            }
            return ds;
        }

        private void RemoveLocalidadFromDataSet(DataSet ds, Localidad localidad)
        {
            string query = "LocalidadID =" + localidad.LocalidadID;
            DataRow[] dr = ds.Tables["Localidad"].Select(query);

            if (dr.Count() == 1)
                ds.Tables["Localidad"].Rows.Remove(dr[0]);
        }
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
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

        protected void grdLocalidades_DataBound(object sender, GridViewRowEventArgs e)
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
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");
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

            grdLocalidades.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOLOCALIDADES) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARLOCALIDADES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARLOCALIDADES) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARLOCALIDADES) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARLOCALIDADES) != null;

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