using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.BO;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;

namespace POV.Web.PortalOperaciones.Profesionalizacion.CatalogoAreas
{
    public partial class EditarArea : PageBase
    {
        #region Propiedades
        private AreaProfesionalizacionCtrl areaProfesionalizacionCtrl = null;
        private NivelEducativoCtrl nivelEducativoCtrl;
        private IDataContext dctx = null;
        private List<MateriaProfesionalizacion> ListaMaterias
        {
            get
            {
                return Session["ListaMateriasEdit"] != null
                           ? Session["ListaMateriasEdit"] as List<MateriaProfesionalizacion>
                           : null;
            }
            set { Session["ListaMateriasEdit"] = value; }
        }

        private List<MateriaProfesionalizacion> ListaMateriasEdit
        {
            get
            {
                return Session["ListaMateriasE"] != null
                           ? Session["ListaMateriasE"] as List<MateriaProfesionalizacion>
                           : null;
            }
            set { Session["ListaMateriasE"] = value; }
        }
        AreaProfesionalizacion LastObject
        {
            get { return Session["lastAreaProfesionalizacion"] != null ? Session["lastAreaProfesionalizacion"] as AreaProfesionalizacion : null; }
            set { Session["lastAreaProfesionalizacion"] = value; }
        }
        MateriaProfesionalizacion LastObjectMateria
        {
            get { return Session["lastMateriaProfesionalizacion"] != null ? Session["lastMateriaProfesionalizacion"] as MateriaProfesionalizacion : null; }
            set { Session["lastMateriaProfesionalizacion"] = value; }
        }
        private DataSet DsMateriaProfesionalizacion
        {
            get { return Session["lastMateriaProfesionalizacion"] != null ? Session["lastMateriaProfesionalizacion"] as DataSet : null; }
            set { Session["lastMateriaProfesionalizacion"] = value; }
        }
        private DataTable DtMateriaProfesionalizacion
        {
            get { return Session["DsMateriasProf"] != null ? Session["DsMateriasProf"] as DataTable : null; }
            set { Session["DsMateriasProf"] = value; }
        }
        #endregion

        public EditarArea()
        {
            areaProfesionalizacionCtrl = new AreaProfesionalizacionCtrl();
            nivelEducativoCtrl = new NivelEducativoCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (LastObject != null || LastObject.AreaProfesionalizacionID != null)
                    {
                        ListaMaterias = new List<MateriaProfesionalizacion>();
                        ListaMateriasEdit = new List<MateriaProfesionalizacion>();
                        configurarCbNivelEducativo();
                        LoadAreaProfesionalizacion();
                        LoadMateriaProfesionalizacion((int)LastObject.AreaProfesionalizacionID);
                        //Se cargan los datos en las tablas.
                        LoadListaMateria(ListaMaterias);
                    }
                    else
                    {
                        Response.Redirect("BuscarAreas.aspx");
                    }                    
                }
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                Response.Redirect("BuscarAreas.aspx");
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string sError = ValidateMateria();

                if (string.IsNullOrEmpty(sError))
                {
                    MateriaProfesionalizacion materia = GetMateriaFromUI();
                    materia.MateriaEstado = EObjetoEstado.NUEVO;
                    ListaMaterias.Add(materia);
                    ListaMateriasEdit.Add(materia);
                    LoadListaMateria(ListaMaterias);
                    txtNombreMateria.Text = "";
                }
                else
                {
                    txtRedirect.Value = "";
                    ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            dctx = Helper.ConnectionHlp.Default.Connection;
            string sError = ValidateArea();

            if (string.IsNullOrEmpty(sError))
            {
                AreaProfesionalizacion area = GetAreaFromUI();
                try
                {
                    areaProfesionalizacionCtrl.UpdateComplete(dctx,area,LastObject);
                }
                catch (Exception ex)
                {
                    txtRedirect.Value = "";
                    ShowMessage("Error: " + ex.Message, MessageType.Error);
                }
                LimpiarObjetosSesion();

                txtRedirect.Value = "BuscarAreas.aspx";
                ShowMessage("Actualización exitosa.", MessageType.Information);
            }
            else
            {
                txtRedirect.Value = "";
                ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
            }
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

        #region Métodos del Grid.
        protected void grdMaterias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName.Trim())
                {
                    case "Eliminar":
                        MateriaProfesionalizacion materia = new MateriaProfesionalizacion();
                        materia.MateriaID = int.Parse(e.CommandArgument.ToString());
                        ListaMaterias.RemoveAll(item => item.MateriaID == materia.MateriaID);
                        LoadListaMateria(ListaMaterias);
                        break;
                    default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void grdMaterias_DataBound(object sender, GridViewEditEventArgs e)
        {
            grdMaterias.EditIndex = e.NewEditIndex;

            LoadMateriaProfesionalizacion(Convert.ToInt32(LastObject.AreaProfesionalizacionID));
        }
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
            var sId = grdMaterias.Rows[e.RowIndex].Cells[0].Text;
            int id = int.Parse(sId);
            string sError = string.Empty;



            if (string.IsNullOrEmpty(nombre))
                sError = ", Nombre requerido";

            if (string.IsNullOrEmpty(sError))
            {
                MateriaProfesionalizacion materia = ListaMaterias.First(item => item.MateriaID == id);
                int index = ListaMaterias.IndexOf(materia);
                materia.Nombre = nombre;
                materia.MateriaEstado = EObjetoEstado.EDITADO;
                ListaMateriasEdit[index] = materia;

                ListaMaterias[index] = materia;
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
            var sId = grdMaterias.Rows[e.RowIndex].Cells[0].Text;
            int id = int.Parse(sId);

            MateriaProfesionalizacion materia = ListaMaterias.First(item => item.MateriaID == id);
            int index = ListaMaterias.IndexOf(materia);
            materia.Activo = false;
            materia.MateriaEstado = EObjetoEstado.ELIMINADO;
            ListaMateriasEdit.Add(materia);
            ListaMateriasEdit.RemoveAt(index);
            ListaMaterias.RemoveAt(index);
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

        #region Métodos de la página
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

        private void DataToUserInterface(AreaProfesionalizacion area)
        {
            txtNombre.Text = area.Nombre.Trim();
            txtDescripcion.Text = area.Descripcion;
            if (area.Activo != null)
                ddlEstatus.SelectedValue = ((bool) area.Activo.Value).ToString();
            if (area.NivelEducativo.NivelEducativoID != null)
            {
                cbNivelEducativo.SelectedValue = area.NivelEducativo.NivelEducativoID.ToString();
                cbGradoAsignatura.Enabled = true;
                loadGrados(area.NivelEducativo);
            }
            if (area.Grado != null)
            {
                cbGradoAsignatura.SelectedValue = area.Grado.ToString();
            }
        }
        private void LoadAreaProfesionalizacion()
        {
            dctx = Helper.ConnectionHlp.Default.Connection;
            LastObject = (AreaProfesionalizacion) areaProfesionalizacionCtrl.RetrieveComplete(dctx, LastObject);
            DataToUserInterface(LastObject);
        }
        private void LoadMateriaProfesionalizacion(int areaid)
        {
            if(areaid == null)
                return;
            AreaProfesionalizacion materias = new AreaProfesionalizacion();
            materias.AreaProfesionalizacionID = areaid;
            DsMateriaProfesionalizacion = areaProfesionalizacionCtrl.MateriaProfesionalizacionRetrieve(dctx, new MateriaProfesionalizacion(), 
                                                                                                       materias);
            if (DsMateriaProfesionalizacion.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in DsMateriaProfesionalizacion.Tables[0].Rows)
                {
                    if (Convert.ToBoolean(row["Activo"]) == true)
                    {
                        ListaMaterias.Add(areaProfesionalizacionCtrl.DataRowToMateriaProfesionalizacion(row));
                        ListaMateriasEdit.Add(areaProfesionalizacionCtrl.DataRowToMateriaProfesionalizacion(row));
                    }
                }

            }
        }
        private void LoadListaMateria(List<MateriaProfesionalizacion> materia)
        {
            DtMateriaProfesionalizacion = new DataTable();
            DtMateriaProfesionalizacion.Columns.Add("MateriaID", typeof(string));
            DtMateriaProfesionalizacion.Columns.Add("Nombre", typeof(string));
            DtMateriaProfesionalizacion.Columns.Add("Activo", typeof(bool));
            foreach (MateriaProfesionalizacion mt in materia)
            {
                if (mt.Activo == true)
                {
                    DataRow dtr = DtMateriaProfesionalizacion.NewRow();
                    dtr[0] = mt.MateriaID;
                    dtr[1] = mt.Nombre;
                    dtr[2] = mt.Activo;
                    DtMateriaProfesionalizacion.Rows.Add(dtr);
                }
            }
            grdMaterias.DataSource = DtMateriaProfesionalizacion;
            grdMaterias.DataBind();
        }
        private MateriaProfesionalizacion GetMateriaFromUI()
        {
            dctx = Helper.ConnectionHlp.Default.Connection;
            MateriaProfesionalizacion materia = new MateriaProfesionalizacion();
            MateriaProfesionalizacion materias = areaProfesionalizacionCtrl.MateriaProfesionalizacionRetrieve(dctx, new MateriaProfesionalizacion());
            MateriaProfesionalizacion materiaexiste = ListaMaterias.Find(item => item.MateriaID == materias.MateriaID);

            if (materiaexiste != null)
            {
                MateriaProfesionalizacion lastmateria = ListaMaterias.Last();
                materia.MateriaID = lastmateria.MateriaID + 1;
            }
            else
            {
                materia.MateriaID = materias.MateriaID + 1;
            }

            materia.Nombre = txtNombreMateria.Text;
            materia.Activo = true;
            materia.FechaRegistro = DateTime.Now;

            return materia;
        }
        private string ValidateArea()
        {
            string sError = string.Empty;
            string nombre = txtNombre.Text;
            string descripcion = txtDescripcion.Text;

            if (string.IsNullOrEmpty(nombre))
                sError += ", Nombre Requerido ";
            if (ListaMaterias.Count == 0)
                sError += ", Al menos un bloque es requerido ";
            if (nombre.Length > 100)
                sError += ", Nombre de la asignatura excede 100 caracteres ";
            if (descripcion.Length > 500)
                sError += ", Descripción excede 500 caracteres ";
            if (cbNivelEducativo.SelectedIndex <= 0)
                sError += ", Nivel educativo es requerido ";
            if (cbGradoAsignatura.SelectedIndex <= 0)
                sError += ", Grado es requerido ";

            return sError;
        }
        private string ValidateMateria()
        {
            string sError = string.Empty;
            string nombremateria = txtNombreMateria.Text;
            MateriaProfesionalizacion materiaexiste = ListaMaterias.Find(item => item.Nombre == nombremateria);

            if (string.IsNullOrEmpty(nombremateria))
                sError += ", Nombre del bloque requerido ";
            if (nombremateria.Length > 50)
                sError += ", Nombre del bloque excede 50 caracteres ";
            if (materiaexiste != null)
                sError += ", Bloque repetido para la asignatura actual";

            return sError;
        }

        private AreaProfesionalizacion GetAreaFromUI()
        {
            AreaProfesionalizacion area = new AreaProfesionalizacion();
            area.AreaProfesionalizacionID = LastObject.AreaProfesionalizacionID;
            area.Nombre = txtNombre.Text;
            area.Descripcion = txtDescripcion.Text;
            area.NivelEducativo = GetNivelEducativoFromUI();
            area.Grado = GetGradoFromUI();
            area.Activo = Convert.ToBoolean(ddlEstatus.SelectedValue);

            foreach (MateriaProfesionalizacion materia in ListaMateriasEdit)
            {
                area.MateriaProfesionalizacionAgregar(materia);
            }

            return area;
        }

        private void LimpiarObjetosSesion()
        {
            ListaMaterias = null;
            LastObject = null;
            LastObjectMateria = null;
        }

        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOAREASPROFESIONALIZACION) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARAREASPROFESIONALIZACION) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARAREASPROFESIONALIZACION) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!lectura)
                redirector.GoToHomePage(true);
            if (!edit)
                redirector.GoToHomePage(true);
        }
        #endregion
    }
}