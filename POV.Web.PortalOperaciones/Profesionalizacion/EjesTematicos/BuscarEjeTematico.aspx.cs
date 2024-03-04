using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.Operaciones;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;
using Framework.Base.Exceptions;
using System.Data;
using System.Text;
using POV.Licencias.Service;
using POV.Core.Operaciones.Interfaces;
using POV.Core.Operaciones.Implements;
using POV.Licencias.BO;
using Framework.Base.DataAccess;

namespace POV.Web.PortalOperaciones.Profesionalizacion.EjesTematicos
{
    public partial class BuscarEjeTematico : CatalogPage
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        #region ***Urls***
        private const string CONFIGURARSITUACIONAPRENDIZAJEURL = "~/Profesionalizacion/EjesTematicos/ConfigurarSituacionesAprendizaje.aspx";
        private const string EDITAREJESTEMATICOSURL = "~/Profesionalizacion/EjesTematicos/EditarEjeTematico.aspx";
        #endregion
        private EjeTematicoCtrl ejeTematicoCtrl;
        private AreaProfesionalizacionCtrl areaProfesionalizacionCtrl;
        private NivelEducativoCtrl nivelEducativoCtrl;
        private MateriaProfesionalizacion materiaProfesionalizacion;
        private EjeTematico ejeTematico;
        private IUserSession userSession;

        #region Propiedades de la Clase
        public DataSet DsEjesTematicos
        {
            set { Session["dsEjesTematicos"] = value; }
            get { return Session["dsEjesTematicos"] != null ? Session["dsEjesTematicos"] as DataSet : null; }
        }
        private EjeTematico LastObject
        {
            set { Session["lastEjeTematico"] = value; }
            get { return Session["lastEjeTematico"] != null ? Session["lastEjeTematico"] as EjeTematico : null; }
        }
        #endregion

        public BuscarEjeTematico() {
            materiaProfesionalizacion = new MateriaProfesionalizacion();
            areaProfesionalizacionCtrl = new AreaProfesionalizacionCtrl();
            ejeTematicoCtrl = new EjeTematicoCtrl();
            nivelEducativoCtrl = new NivelEducativoCtrl();
            ejeTematico = new EjeTematico();            
            userSession = new UserSession();
           
        }
             
        #region Eventos de la Página
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                configurarCbNivelEducativo();
                LoadEjesTematicos();
                PintarGridViewEjesTematicos();
                LoadEstatusProfesionalizacion();
              
            }
        }

        protected void grdEjesTematicos_RowDataBound(object sender, GridViewRowEventArgs e)
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
                if (RenderAccesoSituacionesAprendizaje)
                {
                    ImageButton btnConfig = (ImageButton)e.Row.FindControl("btnConfigurar");
                    btnConfig.Visible = true;
                }

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string errors = ValidateData();
            if (string.IsNullOrEmpty(errors)) {
                List<EjeTematico> ejesTematicos;
                EjeTematico ejeTematico = UserInterfaceToData();

                 ejesTematicos = ejeTematicoCtrl.RetrieveComplete(dctx, ejeTematico, this.materiaProfesionalizacion);
                ejesTematicos =
                    ejesTematicos.Where(x => x.AreaProfesionalizacion != null && x.AreaProfesionalizacion.Activo == true)
                                 .ToList();
                this.DsEjesTematicos = ListEjeTematicoToDataSet(ejesTematicos);
                PintarGridViewEjesTematicos();  
            }else
                ShowMessage(errors.Substring(2), MessageType.Warning);
        }
        
        protected void cbNivelEducativo_SelectedIndexChanged(object sender, EventArgs e)
        {

            NivelEducativo nivelEducativo = GetNivelEducativoFromUI();
            if (nivelEducativo.NivelEducativoID != null)
            {
                DataSet ds = nivelEducativoCtrl.Retrieve(ConnectionHlp.Default.Connection, nivelEducativo);
                nivelEducativo = nivelEducativoCtrl.LastDataRowToNivelEducativo(ds);
                loadGrados(nivelEducativo);
                if (cbGradoAsignatura.Items.Count > 0)
                    cbGradoAsignatura.Enabled = true;
            }
            else
            {
                cbGradoAsignatura.Items.Clear();
                cbGradoAsignatura.Enabled = false;
            }
            ddlAreasProfesionalizacion.Items.Clear();
            ddlAreasProfesionalizacion.Enabled = false;
            ddlMateriasProfesionalizacion.Items.Clear();
            ddlMateriasProfesionalizacion.Enabled = false;

        }
        protected void cbGradoAsignatura_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAreasProfesionalizacion.Items.Clear();
            NivelEducativo nivelEducativo = GetNivelEducativoFromUI();
            byte? grado = GetGradoFromUI();
            if (grado != null)
            {
                LoadAreasProfesionalizacion(areaProfesionalizacionCtrl.Retrieve(dctx,
                    new AreaProfesionalizacion() { NivelEducativo = nivelEducativo, Grado = grado, Activo = true }));
            }
            else
            {
                ddlAreasProfesionalizacion.Items.Clear();
                ddlAreasProfesionalizacion.Enabled = false;
            }
            ddlMateriasProfesionalizacion.Items.Clear();
            ddlMateriasProfesionalizacion.Enabled = false;
        }

        protected void ddlAreasProfesionalizacion_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(ddlAreasProfesionalizacion.SelectedValue))
            {
                int areaProfesionzalizacionSelected = Convert.ToInt32(ddlAreasProfesionalizacion.SelectedValue);

                AreaProfesionalizacion area = new AreaProfesionalizacion() { AreaProfesionalizacionID = areaProfesionzalizacionSelected };
                DataSet dsMaterias = areaProfesionalizacionCtrl.MateriaProfesionalizacionRetrieve(dctx, new MateriaProfesionalizacion() { Activo = true }, area);
                LoadMaterias(dsMaterias);

                if (ddlMateriasProfesionalizacion.Items.Count > 1)
                    ddlMateriasProfesionalizacion.Enabled = true;

            }
            else
            {
                ddlMateriasProfesionalizacion.Items.Clear();
                ddlMateriasProfesionalizacion.Enabled = false;
            }
        }
        protected void grdEjesTematicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        try
                        {
                            EjeTematico ejeTematico = new EjeTematico();
                            long ejeTematicoID = Convert.ToInt64(e.CommandArgument);
                            ejeTematico = GetEjeTematico(ejeTematicoID);
                            EjeTieneContratosActivos(ejeTematico);
                            DoDelete(ejeTematico);
                            ShowMessage("El registro se eliminó con éxito", MessageType.Information);
                            LoadEjesTematicos();
                            PintarGridViewEjesTematicos();
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message, MessageType.Error);
                        }
                        
                        break;
                    }
                case "editar":
                    {
                        EjeTematico ejeTematico = new EjeTematico();
                        ejeTematico.EjeTematicoID = long.Parse(e.CommandArgument.ToString());
                        LastObject = ejeTematico;
                        Response.Redirect(EDITAREJESTEMATICOSURL, true);
                        this.LastObject = ejeTematico;
                        break;
                    }
                case "ConfigurarSituaciones":
                    {
                        EjeTematico ejeTematico = new EjeTematico();
                        ejeTematico.EjeTematicoID = long.Parse(e.CommandArgument.ToString());
                        LastObject = ejeTematico;
                        Response.Redirect(CONFIGURARSITUACIONAPRENDIZAJEURL, true);
                      
                    } break;
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }

            }
        }

        #endregion
        #region Validaciones
        private void EjeTieneContratosActivos(EjeTematico ejeTematico)
        {
            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<Contrato> contratos = contratoCtrl.RetrieveContratoByEje(dctx, ejeTematico);


            foreach (Contrato contrato in contratos)
            {
                Contrato completo = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(dctx, contrato));
                if (completo.Estatus != null)
                    if (completo.Estatus == true)
                    {
                        throw new Exception("No se puede eliminar el eje o ámbito debido a que está asignado a un contrato activo, por favor verifique");
                    }

            }
        }
        private string ValidateData() {
            string sError = string.Empty;
            if (!string.IsNullOrEmpty(txtID.Text.Trim()))
            {
                int ejeTematicoID = 0;
                bool result = int.TryParse(txtID.Text.Trim(), out ejeTematicoID);
                if (!result)
                    sError += ", El identificador debe ser un número entero";
            }
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                if (txtNombre.Text.Trim().Length > 200)
                    sError = ", El tamaño del texto no debe ser mayor a 200 caracteres)";
            }
            return sError;
        }

        #endregion
             
        #region UserInterface To Data

        private NivelEducativo GetNivelEducativoFromUI()
        {
            NivelEducativo nivelEducativo = new NivelEducativo();
            int nivelEducativoID = 0;

            int.TryParse(cbNivelEducativo.SelectedValue, out nivelEducativoID);

            if (nivelEducativoID > 0)
                nivelEducativo.NivelEducativoID = nivelEducativoID;

            return nivelEducativo;
        }

        private byte? GetGradoFromUI()
        {
            byte grado = 0;

            byte.TryParse(cbGradoAsignatura.SelectedValue, out grado);

            if (grado > 0)
                return grado;

            return null;
        }
        private List<MateriaProfesionalizacion> GetMateriasProfesionalizacion()
        {
            List<MateriaProfesionalizacion> materias = new List<MateriaProfesionalizacion>();

            int areaProfesionalizacionID = Convert.ToInt32(ddlAreasProfesionalizacion.SelectedValue);
            int materiaID = Convert.ToInt32(ddlMateriasProfesionalizacion.SelectedValue);

            DataSet ds = areaProfesionalizacionCtrl.MateriaProfesionalizacionRetrieve(dctx,
                new MateriaProfesionalizacion() { MateriaID = materiaID },
                new AreaProfesionalizacion() { AreaProfesionalizacionID = areaProfesionalizacionID });

            materias.Add(areaProfesionalizacionCtrl.LastDataRowToMateriaProfesionalizacion(ds));

            return materias;
        }

        private EjeTematico UserInterfaceToData()
        {

            ejeTematico = new EjeTematico();
            ejeTematico.AreaProfesionalizacion = new AreaProfesionalizacion();
            
            //ID
            if (!string.IsNullOrEmpty(this.txtID.Text.Trim()))
                ejeTematico.EjeTematicoID = Convert.ToInt64(this.txtID.Text.Trim());
            
            //NOMBRE
            if (!string.IsNullOrEmpty(this.txtNombre.Text.Trim()))
            {
                ejeTematico.Nombre = this.txtNombre.Text.Trim();
                StringBuilder builder = new StringBuilder("%" + ejeTematico.Nombre + "%");
                ejeTematico.Nombre = builder.ToString();
            }

            //ESTATUS
            if (EEstatusProfesionalizacion.ACTIVO.ToString().CompareTo(this.ddlEstatusProfesionalizacion.SelectedValue) == 0)
            {
                ejeTematico.EstatusProfesionalizacion = EEstatusProfesionalizacion.ACTIVO;
            }
            else if (EEstatusProfesionalizacion.MANTENIMIENTO.ToString().CompareTo(this.ddlEstatusProfesionalizacion.SelectedValue) == 0)
            {
                ejeTematico.EstatusProfesionalizacion = EEstatusProfesionalizacion.MANTENIMIENTO;
            }
            else ejeTematico.EstatusProfesionalizacion = null;

            //NIVEL EDUCATIVO
            if (cbNivelEducativo.SelectedIndex > 0)
            {
                ejeTematico.AreaProfesionalizacion.NivelEducativo = new NivelEducativo();
                ejeTematico.AreaProfesionalizacion.NivelEducativo.NivelEducativoID = Convert.ToInt32(cbNivelEducativo.SelectedValue);
            }

            //GRADO
            if (cbGradoAsignatura.SelectedIndex > 0)
                ejeTematico.AreaProfesionalizacion.Grado = Convert.ToByte(cbGradoAsignatura.SelectedValue);

            //AREA PROFESIONALIZACION
            if (ddlAreasProfesionalizacion.SelectedIndex > 0)
                ejeTematico.AreaProfesionalizacion.AreaProfesionalizacionID = Convert.ToInt32(this.ddlAreasProfesionalizacion.SelectedValue);
            
            //MATERIA
            if (this.ddlMateriasProfesionalizacion.SelectedIndex > 0)
                this.materiaProfesionalizacion = new MateriaProfesionalizacion() { MateriaID = Convert.ToInt32(ddlMateriasProfesionalizacion.SelectedValue) };

            return ejeTematico;
        }
        #endregion

        #region Loads
        private void PintarGridViewEjesTematicos()
        {
            this.grdEjesTematicos.DataSource = this.DsEjesTematicos;
            this.grdEjesTematicos.DataBind();
        }
        private void LoadEjesTematicos()
        {
            DataSet dsEjesTematicos = null;
            dsEjesTematicos = ejeTematicoCtrl.Retrieve(dctx, new EjeTematico() { EstatusProfesionalizacion = null });
            DataTable dtable = dsEjesTematicos.Tables[0];
            dtable.Columns.Add("Materias", typeof(string));
            dtable.Columns.Add("Area", typeof(string));
            List<DataRow> rowsToDelete = new List<DataRow>();

            foreach (DataRow drEjeTematico in dtable.Rows)
            {
                EjeTematico eje = ejeTematicoCtrl.DataRowToEjeTematico(drEjeTematico);
                eje = ejeTematicoCtrl.RetrieveComplete(dctx, eje);
                if (eje.AreaProfesionalizacion != null && eje.AreaProfesionalizacion.Activo == true)
                {
                    drEjeTematico["Materias"] = GetMateriasText(eje.MateriasProfesionalizacion.ToList());
                    drEjeTematico["Area"] = eje.AreaProfesionalizacion.Nombre;
                }
                else
                {
                    rowsToDelete.Add(drEjeTematico);
                }
            }

            foreach (var dataRow in rowsToDelete)
            {
                dtable.Rows.Remove(dataRow);
                dataRow.Delete();                    
            }
            dsEjesTematicos.Tables.Clear();
            dsEjesTematicos.Tables.Add(dtable);
            this.DsEjesTematicos = dsEjesTematicos;

        }
        
        private void configurarCbNivelEducativo()
        {
            DataSet ds = new DataSet();
            ds = nivelEducativoCtrl.Retrieve(ConnectionHlp.Default.Connection, new NivelEducativo());
            DataRow row = ds.Tables["NivelEducativo"].NewRow();
            row["NivelEducativoID"] = DBNull.Value;
            row["Titulo"] = "TODOS";
            ds.Tables["NivelEducativo"].Rows.InsertAt(row, 0);

            cbNivelEducativo.DataSource = ds;
            cbNivelEducativo.DataTextField = "Titulo";
            cbNivelEducativo.DataValueField = "NivelEducativoID";
            cbNivelEducativo.DataBind();
        }
        private void loadGrados(NivelEducativo nivelEducativo)
        {
            if (nivelEducativo.NumeroGrados != null)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                ds.Tables[0].Columns.Add("Grado", typeof(string));
                ds.Tables[0].Columns.Add("Nombre", typeof(string));

                DataRow row = ds.Tables[0].NewRow();
                row["Grado"] = DBNull.Value;
                row["Nombre"] = "TODOS";
                ds.Tables[0].Rows.InsertAt(row, 0);

                for (int i = 1; i <= nivelEducativo.NumeroGrados.Value; i++)
                {
                    DataRow newRow = ds.Tables[0].NewRow();
                    newRow["Grado"] = i.ToString();
                    newRow["Nombre"] = i + "°";
                    ds.Tables[0].Rows.Add(newRow);
                }
                cbGradoAsignatura.DataSource = ds;
                cbGradoAsignatura.DataTextField = "Nombre";
                cbGradoAsignatura.DataValueField = "Grado";
                cbGradoAsignatura.DataBind();
            }
        }
        private void LoadAreasProfesionalizacion(DataSet dsAreas)
        {
            if (dsAreas.Tables.Count > 0 && dsAreas.Tables[0].Rows.Count > 0)
            {
                DataRow row = dsAreas.Tables[0].NewRow();
                row["AreaProfesionalizacionID"] = DBNull.Value;
                row["Nombre"] = "TODOS";
                dsAreas.Tables[0].Rows.InsertAt(row, 0);

                ddlAreasProfesionalizacion.DataSource = dsAreas;
                ddlAreasProfesionalizacion.DataTextField = "Nombre";
                ddlAreasProfesionalizacion.DataValueField = "AreaProfesionalizacionID";
                ddlAreasProfesionalizacion.DataBind();

                ddlAreasProfesionalizacion.Enabled = true;
            }
            else
                ddlAreasProfesionalizacion.Enabled = false;
        }

        private void LoadMaterias(DataSet dsMaterias)
        {
            DataRow row = dsMaterias.Tables[0].NewRow();
            row["MateriaID"] = DBNull.Value;
            row["Nombre"] = "TODOS";
            dsMaterias.Tables[0].Rows.InsertAt(row, 0);

            ddlMateriasProfesionalizacion.DataSource = dsMaterias;
            ddlMateriasProfesionalizacion.DataTextField = "Nombre";
            ddlMateriasProfesionalizacion.DataValueField = "MateriaID";
            ddlMateriasProfesionalizacion.DataBind();
        }
        private void LoadEstatusProfesionalizacion() {
         ListItem item=null;
                ddlEstatusProfesionalizacion.Items.Add(new ListItem("TODOS","-1"));
                foreach (EEstatusProfesionalizacion r in Enum.GetValues(typeof(EEstatusProfesionalizacion)))
                {
                    if (r.ToString().CompareTo("INACTIVO") != 0)
                    {
                        item = new ListItem(Enum.GetName(typeof(EEstatusProfesionalizacion), r), r.ToString());
                        ddlEstatusProfesionalizacion.Items.Add(item);
                    }
                }

        }
        #endregion 
        #region Auxiliares

        private string GetMateriasText(List<MateriaProfesionalizacion> materias)
        {
            StringBuilder materiasText = new StringBuilder();
            foreach (MateriaProfesionalizacion materia in materias)
            {
                materiasText.Append(materia.Nombre + ",");
            }
            if (materiasText.Length > 0)
                return materiasText.ToString().Substring(0, materiasText.Length - 1);
            return "";
        }
        private AreaProfesionalizacion GetAreaProfesionalizacion(int AreaProfesionalizacionID) { 
           return areaProfesionalizacionCtrl.RetrieveComplete(dctx,new AreaProfesionalizacion{AreaProfesionalizacionID = AreaProfesionalizacionID});
        }
        private EjeTematico GetEjeTematico(long EjeTematicoID)
        {
            return ejeTematicoCtrl.RetrieveComplete(dctx, new EjeTematico() { EjeTematicoID = EjeTematicoID });
        }
        private DataSet ListEjeTematicoToDataSet(List<EjeTematico> ejesTematicos) {

            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            ds.Tables[0].Columns.Add("EjeTematicoID",typeof(long));
            ds.Tables[0].Columns.Add("Nombre", typeof(string));
            ds.Tables[0].Columns.Add("Area", typeof(string));
            ds.Tables[0].Columns.Add("Materias", typeof(string));
            

            foreach (EjeTematico eje in ejesTematicos) {

                DataRow dr= ds.Tables[0].NewRow();
                
                dr["EjeTematicoID"] = eje.EjeTematicoID;
                dr["Nombre"] = eje.Nombre;


                dr["Area"] = this.GetAreaProfesionalizacion((int)eje.AreaProfesionalizacion.AreaProfesionalizacionID).Nombre;
                dr["Materias"] = this.GetMateriasText(eje.MateriasProfesionalizacion.ToList());
                ds.Tables[0].Rows.Add(dr);
                          
            }
            return ds;
        }

        
        private void DoDelete(EjeTematico eje) {
            try
            {
                if (eje == null)
                    throw new Exception("No se puede eliminar");
                if (eje.EjeTematicoID == null)
                    throw new Exception("No se puede eliminar");


                ejeTematicoCtrl.DeleteComplete(dctx, eje);
            }
            catch (Exception e) {
                throw e;
            }
        }
        #endregion
        
        #region AUTORIZACION DE LA PAGINA


        private bool RenderEdit = false;
        private bool RenderDelete = false;
        private bool RenderAccesoSituacionesAprendizaje = false;

        protected override void DisplayCreateAction()
        {
            pnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {

            grdEjesTematicos.Visible = true;
        }

        protected override void DisplayUpdateAction()
        {
            RenderEdit = true;
        }

        protected override void DisplayDeleteAction()
        {
            RenderDelete = true;
        }
        protected void DisplayAccesoConfigSituacionesAprendizajeAction() {

            RenderAccesoSituacionesAprendizaje = true;        
        }

        protected override void AuthorizeUser()
        {

            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOEJETEMATICO) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTAREJETEMATICO) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTAREJETEMATICO) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINAREJETEMATICO) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITAREJETEMATICO) != null;
            bool configSituacionesAprendizaje= permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOSITUACIONES) != null;

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
            if (configSituacionesAprendizaje)
                DisplayAccesoConfigSituacionesAprendizajeAction();
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

       
      
   


    }
}
