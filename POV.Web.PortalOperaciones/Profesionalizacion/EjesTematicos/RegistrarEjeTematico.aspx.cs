using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Profesionalizacion.Service;
using POV.Web.PortalOperaciones.Helper;
using POV.Profesionalizacion.BO;
using System.Data;
using Framework.Base.Exceptions;
using Framework.Base.DataAccess;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Core.Operaciones;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.Profesionalizacion.EjesTematicos
{
    public partial class RegistrarEjeTematico : PageBase
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private EjeTematicoCtrl ejeTematicoCtrl;
        private AreaProfesionalizacionCtrl areaProfesionalizacionCtrl;
        private NivelEducativoCtrl nivelEducativoCtrl;

        #region propiedades de la clase
        #endregion
        public RegistrarEjeTematico() { 
            ejeTematicoCtrl = new EjeTematicoCtrl();
            areaProfesionalizacionCtrl = new AreaProfesionalizacionCtrl();
            nivelEducativoCtrl = new NivelEducativoCtrl();
        }

        #region ***Eventos de la Página***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
              
                configurarCbNivelEducativo();
                cbGradoAsignatura.Enabled = false;
                ddlAreasProfesionalizacion.Enabled = false;
                ddlMaterias.Enabled = false;
            }
        }
       
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DoInsert();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
            
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlHelper.GetConsultarEjesTematicosURL(), true);
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
            ddlMaterias.Items.Clear(); 
            ddlMaterias.Enabled = false;
                
        }
        protected void cbGradoAsignatura_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAreasProfesionalizacion.Items.Clear();
            NivelEducativo nivelEducativo = GetNivelEducativoFromUI();
            byte? grado = GetGradoFromUI();
            if ( grado != null)
            {
                LoadAreasProfesionalizacion(areaProfesionalizacionCtrl.Retrieve(dctx,
                    new AreaProfesionalizacion() {NivelEducativo = nivelEducativo, Grado = grado, Activo = true}));
            }
            else
            {
                ddlAreasProfesionalizacion.Items.Clear();
                ddlAreasProfesionalizacion.Enabled = false;
            }
            ddlMaterias.Items.Clear();
            ddlMaterias.Enabled = false;
        }

        protected void ddlAreasProfesionalizacion_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(ddlAreasProfesionalizacion.SelectedValue))
            {
                int areaProfesionzalizacionSelected = Convert.ToInt32(ddlAreasProfesionalizacion.SelectedValue);

                AreaProfesionalizacion area = new AreaProfesionalizacion() { AreaProfesionalizacionID = areaProfesionzalizacionSelected };
                DataSet dsMaterias = areaProfesionalizacionCtrl.MateriaProfesionalizacionRetrieve(dctx, new MateriaProfesionalizacion() { Activo = true }, area);
                LoadMaterias(dsMaterias);

                if (ddlMaterias.Items.Count > 1)
                    ddlMaterias.Enabled = true;

            }
            else
            {
                ddlMaterias.Items.Clear();
                ddlMaterias.Enabled = false;
            }
        }

        #endregion

        #region ***validaciones***
        private void TieneMaterias(EjeTematico ejeTematico)
        {

            if (ejeTematico.MateriasProfesionalizacion == null)
                throw new Exception("El eje o ámbito debe tener una asignatura y un bloque");
            if (ejeTematico.MateriasProfesionalizacion.ToList() == null)
                throw new Exception("El eje o ámbito debe tener una asignatura  y un bloque");
            if (ejeTematico.MateriasProfesionalizacion.ToList().Count == 0)
                throw new Exception("El eje o ámbito debe tener una asignatura  y un bloque");
        }

        private string ValidateFieldsForInsert()
        {
            string errors = string.Empty;
            if (string.IsNullOrEmpty(this.txtNombre.Text.Trim()))
            {
                errors += ", Nombre es Requerido";
            }
            else { 
               if(this.txtNombre.Text.Trim().Length>200)
                   errors += ", Nombre no debe ser mayor a 200 caracteres";
            }
            if (!string.IsNullOrEmpty(this.txtDescripcion.Text.Trim()))
            {
                if (this.txtDescripcion.Text.Trim().Length > 1000)
                    errors += ", Descripción no debe ser mayor a 1000 caracteres";
            }
            if (cbNivelEducativo.SelectedIndex <= 0)
                errors += ", Nivel educativo es requerido";
            if (cbGradoAsignatura.SelectedIndex <= 0)
                errors += ", Grado es requerido";
            if (ddlAreasProfesionalizacion.SelectedIndex <= 0)
                errors += ", Asignatura es requerido";
            if (ddlMaterias.SelectedIndex <= 0)
                errors += ", Bloque es requerido";

            return errors;
        }
        
#endregion
        #region ***Loads and Prints

        private void configurarCbNivelEducativo()
        {
            DataSet ds = new DataSet();
            ds = nivelEducativoCtrl.Retrieve(ConnectionHlp.Default.Connection, new NivelEducativo());
            DataRow row = ds.Tables["NivelEducativo"].NewRow();
            row["NivelEducativoID"] = DBNull.Value;
            row["Titulo"] = "Seleccionar";
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
                row["Nombre"] = "Seleccionar";
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
                row["Nombre"] = "Seleccionar";
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
            row["Nombre"] = "Seleccionar";
            dsMaterias.Tables[0].Rows.InsertAt(row, 0);

            ddlMaterias.DataSource = dsMaterias;
            ddlMaterias.DataTextField = "Nombre";
            ddlMaterias.DataValueField = "MateriaID";
            ddlMaterias.DataBind();
        }
        #endregion

        #region ***Auxiliares ***
        private void DoInsert()
        {
            try
            {
            string s = ValidateFieldsForInsert();

            if (s.Length > 0)
                throw new Exception("Se presentaron las siguientes inconsistencias: " + s.Substring(2));
            EjeTematico ejeTematico = UserInterfaceToData();
            TieneMaterias(ejeTematico);
            
                ejeTematicoCtrl.InsertComplete(dctx, ejeTematico);
                txtRedirect.Value = "BuscarEjeTematico.aspx";
                ShowMessage("El registro se guardó con éxito", MessageType.Information);
                
            }
            catch (Exception e)
            {
                ShowMessage(e.Message, MessageType.Error);
            }
            
        }

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
            int materiaID = Convert.ToInt32(ddlMaterias.SelectedValue);

            DataSet ds = areaProfesionalizacionCtrl.MateriaProfesionalizacionRetrieve(dctx,
                new MateriaProfesionalizacion(){MateriaID = materiaID}, 
                new AreaProfesionalizacion(){AreaProfesionalizacionID = areaProfesionalizacionID});
            
            materias.Add(areaProfesionalizacionCtrl.LastDataRowToMateriaProfesionalizacion(ds));

            return materias;
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
       
        #region ***UserInterfaceToData***
        private EjeTematico UserInterfaceToData()
        {

            EjeTematico ejeTematico = new EjeTematico();
            
            ejeTematico.Nombre = this.txtNombre.Text.Trim();
            ejeTematico.Descripcion = this.txtDescripcion.Text.Trim();
            ejeTematico.AreaProfesionalizacion = new AreaProfesionalizacion();
            ejeTematico.AreaProfesionalizacion.AreaProfesionalizacionID = Convert.ToInt32(ddlAreasProfesionalizacion.SelectedValue);
            ejeTematico.AreaProfesionalizacion.NivelEducativo = GetNivelEducativoFromUI();
            ejeTematico.AreaProfesionalizacion.Grado = GetGradoFromUI();
            ejeTematico.MateriasProfesionalizacion = GetMateriasProfesionalizacion();
            ejeTematico.EstatusProfesionalizacion = EEstatusProfesionalizacion.ACTIVO;
            ejeTematico.FechaRegistro = DateTime.Now;

            return ejeTematico;
        }
        #endregion      

        #region AUTORIZACIÓN DE LA PÁGINA***
        protected override void AuthorizeUser()
        {
           
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTAREJETEMATICO) != null;
            if (creacion)
                DisplayCreateAction();
            else{
              redirector.GoToHomePage(true);
            }
           
        }

        protected  void DisplayCreateAction()
        {
            this.btnGuardar.Visible=true;
        }
        
        #endregion
    }
        
       
    
}