using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;
using System.Data;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.Profesionalizacion.CatalogoAreas
{
    public partial class RegistrarArea : PageBase
    {
        #region Propiedades
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private AreaProfesionalizacionCtrl areaProfesionalizacionCtrl;
        private AreaProfesionalizacion areaProfesionalizacion;
        private NivelEducativoCtrl nivelEducativoCtrl;
        private List<MateriaProfesionalizacion> ListaMaterias
        {
            get { return Session["ListaMateria"] != null ? Session["ListaMateria"] as List<MateriaProfesionalizacion> : null; }
            set { Session["ListaMateria"] = value; }
        } 
        #endregion

        public RegistrarArea()
        {
            areaProfesionalizacionCtrl = new AreaProfesionalizacionCtrl();
            nivelEducativoCtrl = new NivelEducativoCtrl();
        }
        #region Eventos de la Página
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListaMaterias = new List<MateriaProfesionalizacion>();
                configurarCbNivelEducativo();
                cbGradoAsignatura.Enabled = false;
                if (cbNivelEducativo.Items.Count == 0)
                    ShowMessage("No existen niveles educativos en el sistema para la asignatura", MessageType.Information);
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            MateriaProfesionalizacion materia = GetMateriaFromUI();
            string sError = ValidarMaterias(materia);

            if (string.IsNullOrEmpty(sError))
            {
                
                ListaMaterias.Add(materia);
                LoadListaMateria(ListaMaterias);
                txtNombreMateria.Text = "";
            }
            else
            {
                txtRedirect.Value = "";
                ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            AreaProfesionalizacion area = GetAreaFromUI();
            string sError = ValidateArea();
            sError = ValidateForInsertArea(area);

            if (string.IsNullOrEmpty(sError))
            {
                try
                {
                    areaProfesionalizacionCtrl.InsertComplete(dctx, area);
                }
                catch (Exception ex)
                {
                    txtRedirect.Value = "";
                    ShowMessage("Error: " + ex.Message, MessageType.Error);
                }
                LimpiarObjetosSesion();

                txtRedirect.Value = "BuscarAreas.aspx";
                ShowMessage("Registro exitoso.", MessageType.Information);
            }
            else
            {
                txtRedirect.Value = "";
                ShowMessage("Error: " + sError.Substring(2), MessageType.Error);
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarObjetosSesion();
                Response.Redirect("BuscarAreas.aspx");
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        private MateriaProfesionalizacion GetMateriaFromUI()
        {
            MateriaProfesionalizacion materia = new MateriaProfesionalizacion();
            materia.Nombre = txtNombreMateria.Text;
            materia.Activo = true;
            materia.FechaRegistro = DateTime.Now;

            return materia;
        }
        private AreaProfesionalizacion GetAreaFromUI()
        {
            AreaProfesionalizacion area = new AreaProfesionalizacion();
            area.Nombre = txtNombre.Text;
            area.Descripcion = txtDescripcion.Text;
            area.Activo = true;
            area.NivelEducativo = GetNivelEducativoFromUI();
            area.Grado = GetGradoFromUI();
            area.FechaRegistro = DateTime.Now;

            foreach (MateriaProfesionalizacion materia in ListaMaterias)
            {
                area.MateriaProfesionalizacionAgregar(materia);
            }

            return area;
        }
        

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
        private NivelEducativo GetNivelEducativoFromUI()
        {
            NivelEducativo nivelEducativo = new NivelEducativo();
            int nivelEducativoID = 0;

            int.TryParse(cbNivelEducativo.SelectedValue, out nivelEducativoID);

            if (nivelEducativoID > 0)
                nivelEducativo.NivelEducativoID = nivelEducativoID;

            return nivelEducativo;
        }
        private byte GetGradoFromUI()
        {
            byte grado = 0;
            
            byte.TryParse(cbGradoAsignatura.SelectedValue, out grado);

            return grado;
        }
        private void LoadListaMateria(List<MateriaProfesionalizacion> materia)
        {
            DataTable dtMateria = new DataTable();
            dtMateria.Columns.Add("MateriaID", typeof(string));
            dtMateria.Columns.Add("Nombre", typeof(string));
            dtMateria.Columns.Add("Activo", typeof(bool));
            foreach (MateriaProfesionalizacion mt in materia)
            {
                DataRow dtr = dtMateria.NewRow();
                dtr[0] = mt.MateriaID;
                dtr[1] = mt.Nombre;
                dtr[2] = mt.Activo;
                dtMateria.Rows.Add(dtr);
            }
            grdMaterias.DataSource = dtMateria;
            grdMaterias.DataBind();
        }
        private string ValidateArea()
        {
            string sError = string.Empty;
            string nombre = txtNombre.Text;
            string descripcion = txtDescripcion.Text;

            if (string.IsNullOrEmpty(nombre))
                sError += ", Nombre Requerido ";
            if (string.IsNullOrEmpty(descripcion))
                sError += ", Descripción Requerida ";
            if (ListaMaterias.Count == 0)
                sError += ", Al menos un bloque es requerido ";
            if (cbNivelEducativo.SelectedIndex <= 0)
                sError += ", Nivel educativo es requerido ";
            if (cbGradoAsignatura.SelectedIndex <= 0)
                sError += ", Grado es requerido ";
            if (sError.Length > 0)
                return sError;

            if (nombre.Length > 100)
                sError += ", Nombre de la asignatura excede 100 caracteres ";
            if (descripcion.Length > 500)
                sError += ", Descripción excede 500 caracteres ";

            return sError;
        }
        private string ValidateForInsertArea(AreaProfesionalizacion area)
        {
            string sError = string.Empty;
            AreaProfesionalizacion areap = new AreaProfesionalizacion();
            areap.Nombre = area.Nombre;
            DataSet ds = areaProfesionalizacionCtrl.Retrieve(dctx,areap);
            if (ds.Tables[0].Rows.Count > 0)
                sError += ", El nombre de la asignatura ya está registrado en el sistema, por favor verifique.";
            if (ListaMaterias.Count < 1)
                sError += ", Debe ingresar al menos un bloque.";
            return sError;
        }
        private string ValidarMaterias(MateriaProfesionalizacion materia)
        {
            string sError = string.Empty;
            MateriaProfesionalizacion materiaexiste = ListaMaterias.Find(item => item.Nombre == materia.Nombre);
            if (string.IsNullOrEmpty(txtNombreMateria.Text.Trim()))
                sError += ", Nombre del bloque requerido";
            else if (txtNombreMateria.Text.Trim().Length > 50)
                sError += ", Nombre del bloque excede 50 caracteres";
            if(materiaexiste != null)
            {
                if (materiaexiste.Nombre != null)
                    sError += ", Nombre de bloque ya ha sido registrado a esta asignatura, por favor verifique";
            }
            return sError;
        }
        private void LimpiarObjetosSesion()
        {
            ListaMaterias = null;
        }
        #endregion

        #region Métodos del Grid.
        protected void grdMaterias_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdMaterias.EditIndex = e.NewEditIndex;

            LoadListaMateria(ListaMaterias);
        }
        protected void grdMaterias_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdMaterias.EditIndex = -1;

            LoadListaMateria(ListaMaterias);
        }
        protected void grdMaterias_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string nombre = (grdMaterias.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text.Trim();

            string sError = string.Empty;

            if (string.IsNullOrEmpty(nombre))
                sError = ", Nombre requerido";

            if (string.IsNullOrEmpty(sError))
            {
                MateriaProfesionalizacion materia = ListaMaterias.ElementAt(e.RowIndex);
                materia.Nombre = nombre;

                ListaMaterias[e.RowIndex] = materia;
                grdMaterias.EditIndex = -1;

                LoadListaMateria(ListaMaterias);
            }
            else
            {
                txtRedirect.Value = "";
                ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
            }
        }
        protected void grdMaterias_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ListaMaterias.RemoveAt(e.RowIndex);
            grdMaterias.EditIndex = -1;

            LoadListaMateria(ListaMaterias);

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

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso =  permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOAREASPROFESIONALIZACION) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARAREASPROFESIONALIZACION) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!creacion)
                redirector.GoToHomePage(true);
        }
        #endregion
    }
}