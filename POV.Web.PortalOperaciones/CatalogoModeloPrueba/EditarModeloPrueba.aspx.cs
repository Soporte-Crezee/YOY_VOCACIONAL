using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Reactivos.BO;
using POV.Web.PortalOperaciones.Helper;

namespace POV.Web.PortalOperaciones.CatalogoModeloPrueba
{
    public partial class EditarModeloPrueba : System.Web.UI.Page
    {
        #region Propiedades
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private ModeloCtrl modeloCtrl;

        private List<PropiedadPersonalizada> ListaPropiedades
        {
            get { return Session["ListaPropiedades"] != null ? Session["ListaPropiedades"] as List<PropiedadPersonalizada> : null; }
            set { Session["ListaPropiedades"] = value; }
        }

        AModelo LastObject
        {
            get { return Session["lastModelo"] != null ? Session["lastModelo"] as ModeloDinamico : null; }
            set { Session["lastModelo"] = value; }
        }

        public List<PropiedadPersonalizada> LastPropiedadModelo
        {
            set { Session["LastPropiedadModelo"] = value; }
            get { return Session["LastPropiedadModelo"] != null ? Session["LastPropiedadModelo"] as List<PropiedadPersonalizada> : null; }
        }

        public EditarModeloPrueba() 
        {
            modeloCtrl = new ModeloCtrl();
        }
        #endregion

        #region *****> Eventos de Pagina <*****
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (LastObject != null && LastObject.ModeloID != null)
                {                    
                    Dictionary<string, string> parametros = new Dictionary<string, string>();
                    parametros.Add("Activo", "true");

                    AModelo modelo = new ModeloDinamico();
                    modelo = modeloCtrl.LastDataRowToModelo(modeloCtrl.Retrieve(dctx, LastObject.ModeloID, parametros));

                    if (modelo.EsEditable == true)
                    {                        
                        LastObject = modeloCtrl.RetrieveComplete(dctx, LastObject);                        
                        LoadModeloPrueba(LastObject as ModeloDinamico);

                        ListaPropiedades = new List<PropiedadPersonalizada>();
                        ListaPropiedades = modeloCtrl.RetrieveListaPropiedadesModeloDinamico(dctx, LastObject as ModeloDinamico);
                        
                        LoadListaPropiedades(ListaPropiedades);   
                        if (EsModeloDinamicoEnUso(LastObject as ModeloDinamico))
                        {
                            DDLMetodoCalificacion.Enabled = false;
                        }                                     
                    }
                    else 
                    {
                        txtRedirect.Value = "BuscarModeloPrueba.aspx";
                        ShowMessage("El modelo NO es editable ", MessageType.Error);
                    }                    
                }
                else
                {
                    txtRedirect.Value = "BuscarModeloPrueba.aspx";
                    ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                }
            }
        }

        protected void BtnGuardar_OnClick(object sender, EventArgs e)
        {
            string sError = ValidateModeloPrueba();

            if (string.IsNullOrEmpty(sError))
            {
                try
                {
                    ModeloDinamico modeloPrueba = GetModeloPruebaFromUI();
                    modeloCtrl.UpdateModeloDinamico(dctx, modeloPrueba, LastObject as ModeloDinamico);

                    AModelo modelo = new ModeloDinamico();
                    modelo = modeloCtrl.LastDataRowToModelo(modeloCtrl.Retrieve(dctx, modeloPrueba));

                    ActualizarListaPropiedades(LastObject as ModeloDinamico);
                    ListaPropiedades = null;

                    txtRedirect.Value = "BuscarModeloPrueba.aspx";
                    ShowMessage("Actualización exitosa.", MessageType.Information);
                }
                catch (Exception ex)
                {
                    txtRedirect.Value = "";
                    ShowMessage("Error: " + ex.Message, MessageType.Error);
                }
            }
            else
            {
                txtRedirect.Value = "";
                ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
            }

        }

        protected void BtnCancelar_OnClick(object sender, EventArgs e)
        {
            ListaPropiedades = null;
            Response.Redirect("BuscarModeloPrueba.aspx", true);
        }

        protected void BtnAgregarPropiedad_OnClick(object sender, EventArgs e)
        {
            string sError = ValidarPropiedad();

            if (string.IsNullOrEmpty(sError))
            {
                PropiedadPersonalizada propiedad = GetPropiedadFromUI();
                ListaPropiedades.Add(propiedad);
                LoadListaPropiedades(ListaPropiedades);
                txtNombrePropiedad.Text = "";
                txtDescripcionPropiedad.Text = "";
                chkEsVisible.Checked = false;
            }
            else
            {
                txtRedirect.Value = "";
                ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
            }
        }

        protected void GridPropiedades_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridPropiedades.EditIndex = -1;
            LoadListaPropiedades(ListaPropiedades);
        }

        protected void GridPropiedades_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ListaPropiedades.RemoveAt(e.RowIndex);
            GridPropiedades.EditIndex = -1;
            LoadListaPropiedades(ListaPropiedades);
        }

        protected void GridPropiedades_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridPropiedades.EditIndex = e.NewEditIndex;
            LoadListaPropiedades(ListaPropiedades);
        }

        protected void GridPropiedades_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string nombre = (GridPropiedades.Rows[e.RowIndex].Cells[0].Controls[0] as TextBox).Text.Trim();
            string describe = (GridPropiedades.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text.Trim();
            bool esVisible = (GridPropiedades.Rows[e.RowIndex].Cells[2].Controls[0] as CheckBox).Checked;

            string sError = string.Empty;

            if (string.IsNullOrEmpty(nombre))
                sError = ", Nombre requerido";

            if (nombre.Trim().Length > 100)
                sError += ", Nombre de la Propiedad excede 100 caracteres ";

            if (describe.Trim().Length > 500)
                sError += ", Descripción de la Propiedad excede 500 caracteres ";

            if (string.IsNullOrEmpty(sError))
            {
                PropiedadPersonalizada propiedad = ListaPropiedades.ElementAt(e.RowIndex);
                propiedad.Nombre = nombre;
                propiedad.Descripcion = describe;
                propiedad.EsVisible = esVisible;

                ListaPropiedades[e.RowIndex] = propiedad;
                GridPropiedades.EditIndex = -1;

                LoadListaPropiedades(ListaPropiedades);
            }
            else
            {
                txtRedirect.Value = "";
                ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
            }
        }
        #endregion

        #region *****> Métodos Auxiliares <*****
        private void LoadModeloPrueba(ModeloDinamico modelo)
        {
            if (modelo.Nombre != null)
                txtNombre.Text = modelo.Nombre;
            else
                txtNombre.Text = string.Empty;

            if (modelo.Descripcion != null)
                txtDescripcion.Text = modelo.Descripcion;
            else
                txtDescripcion.Text = string.Empty;

            hdnNombreModeloPrueba.Value = txtNombre.Text.Trim();

            DDLMetodoCalificacion.SelectedValue = ((byte)modelo.MetodoCalificacion).ToString();
            
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("Activo", "true");
            CatalogoPruebaCtrl catalogoPruebaCtrl = new CatalogoPruebaCtrl();
            DataSet ds = catalogoPruebaCtrl.RetrievePruebasModelo(dctx, modelo.ModeloID, parametros, EEstadoLiberacionPrueba.ACTIVA);

            if (ds.Tables["PruebasModelo"].Rows.Count > 0)
                DDLMetodoCalificacion.Enabled = false;
            else
                DDLMetodoCalificacion.Enabled = true;            
        }
        
        private void LoadListaPropiedades(List<PropiedadPersonalizada> propiedades)
        {
            DataTable dtPropiedades = new DataTable();

            dtPropiedades.Columns.Add("propID", typeof(string));
            dtPropiedades.Columns.Add("Propiedad", typeof(string));
            dtPropiedades.Columns.Add("Descripcion", typeof(string));
            dtPropiedades.Columns.Add("EsVisible", typeof(bool));

            foreach (PropiedadPersonalizada propiedad in propiedades)
            {
                DataRow dr = dtPropiedades.NewRow();
                dr[0] = propiedad.PropiedadID;
                dr[1] = propiedad.Nombre;
                dr[2] = propiedad.Descripcion;
                if (propiedad.EsVisible != null)
                    dr[3] = propiedad.EsVisible.Value;                
                dtPropiedades.Rows.Add(dr);
            }

            GridPropiedades.DataSource = dtPropiedades;
            GridPropiedades.DataBind();
        }

        private void ActualizarListaPropiedades(ModeloDinamico modelo)
        {
            foreach (PropiedadPersonalizada propiedad in ListaPropiedades)
            {
                if (propiedad.PropiedadID == null)
                {
                    propiedad.Activo = true;
                    modeloCtrl.InsertPropiedadPersonalizada(dctx, modelo, propiedad);
                }
                else
                {
                    PropiedadPersonalizada previous = new PropiedadPersonalizada() { PropiedadID = propiedad.PropiedadID };
                    modeloCtrl.UpdatePropiedadPersonalizada(dctx, propiedad, previous);
                }
            }

            foreach (PropiedadPersonalizada propiedad in modelo.ListaPropiedadPersonalizada)
            {
                if (ListaPropiedades.FindIndex(x => x.PropiedadID == propiedad.PropiedadID) < 0)
                {
                    propiedad.Activo = false;
                    PropiedadPersonalizada previous = new PropiedadPersonalizada() { PropiedadID = propiedad.PropiedadID };
                    modeloCtrl.UpdatePropiedadPersonalizada(dctx, propiedad, previous);
                }
            }
        }
        #endregion

        #region *** validaciones ***
        private string ValidarPropiedad()
        {
            string sError = string.Empty;

            if (string.IsNullOrEmpty(txtNombrePropiedad.Text.Trim()))
                sError = ", Nombre requerido";

            if (sError.Length > 0)
                return sError;

            if (txtNombrePropiedad.Text.Trim().Length > 100)
                sError += ", Nombre de la Propiedad excede 100 caracteres ";

            if (txtDescripcionPropiedad.Text.Trim().Length > 500)
                sError += ", Descripción de la Propiedad excede 500 caracteres ";

            return sError;
        }

        private string ValidateBuscarNombreModelo()
        {
            string sError = string.Empty;

            AModelo modeloB = new ModeloDinamico();
            modeloB.Nombre = txtNombre.Text.Trim();
            modeloB.Estatus = true;
            DataSet ds = modeloCtrl.Retrieve(dctx, modeloB);

            if (ds.Tables["Modelo"].Rows.Count > 0)
                sError += ", Nombre del Modelo de Prueba ya Existe ";

            return sError;
        }

        private string ValidateModeloPrueba()
        {
            string sError = string.Empty;
            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();

            if (string.IsNullOrEmpty(nombre))
                sError += ", Nombre del Modelo de Prueba es Requerido ";

            if (string.IsNullOrEmpty(DDLMetodoCalificacion.SelectedValue))
                sError += ", Método de Calificación es Requerido ";

            if (sError.Length > 0)
                return sError;

            if (nombre.Length > 200)
                sError += ", Nombre del Modelo excede 200 caracteres ";

            if (descripcion.Length > 1000)
                sError += ", Descripción del Modelo excede 1000 caracteres ";

            if (sError.Length > 0)
                return sError;

            if (txtNombre.Text.Trim() != hdnNombreModeloPrueba.Value.Trim())
                sError = ValidateBuscarNombreModelo();

            return sError;
        }

        private bool EsModeloDinamicoEnUso(ModeloDinamico modelo)
        {
         
            CatalogoPruebaCtrl catalogoPruebaCtrl = new CatalogoPruebaCtrl();
            
           
            DataSet ds = catalogoPruebaCtrl.Retrieve(dctx, new PruebaDinamica {Modelo = modelo}, false);
            if (ds.Tables[0].Rows.Count > 0)
                return true;
            List<Reactivo> lsReactivos = catalogoPruebaCtrl.RetrieveListaReactivos(dctx, modelo, new Reactivo { TipoReactivo = ETipoReactivo.ModeloGenerico,Activo = true}, false);
            if (lsReactivos.Count > 0)
                return true;

            return false;
        }
        #endregion

        private PropiedadPersonalizada GetPropiedadFromUI()
        {
            PropiedadPersonalizada propiedad = new PropiedadPersonalizada();
            propiedad.Nombre = txtNombrePropiedad.Text;
            propiedad.Descripcion = string.IsNullOrEmpty(txtDescripcionPropiedad.Text.Trim()) ? null : txtDescripcionPropiedad.Text.Trim();
            propiedad.EsVisible = chkEsVisible.Checked;
            propiedad.Activo = true;
            propiedad.FechaRegistro = DateTime.Now;

            return propiedad;
        }

        private ModeloDinamico GetModeloPruebaFromUI()
        {
            ModeloDinamico modelo = new ModeloDinamico();
            modelo.Nombre = txtNombre.Text.Trim();
            modelo.Descripcion = string.IsNullOrEmpty(txtDescripcion.Text.Trim()) ? null : txtDescripcion.Text.Trim();
            modelo.EsEditable = true;
            modelo.Estatus = true;
            modelo.FechaRegistro = DateTime.Now;

            switch (DDLMetodoCalificacion.SelectedValue)
            {
                case "0":
                    modelo.MetodoCalificacion = EMetodoCalificacion.PUNTOS;
                    break;
                case "1":
                    modelo.MetodoCalificacion = EMetodoCalificacion.PORCENTAJE;
                    break;
                case "2":
                    modelo.MetodoCalificacion = EMetodoCalificacion.CLASIFICACION;
                    break;
                case "3":
                    modelo.MetodoCalificacion = EMetodoCalificacion.SELECCION;
                    break;
            }

            return modelo;
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

    }
}