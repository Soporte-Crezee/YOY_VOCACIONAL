using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Web.PortalOperaciones.Helper;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;
using POV.Modelo.Estandarizado.BO;
using POV.Modelo.Estandarizado.Service;

namespace POV.Web.PortalOperaciones.CatalogoAsignaturas
{
    public partial class EditarPlanEducativo : PageBase
    {
        private NivelEducativoCtrl nivelEducativoCtrl;
        private AreaAplicacionCtrl areaAplicacionCtrl;
        private PlanEducativoCtrl planCtrl;
        private MateriaCtrl materiaCtrl;

        #region *** propiedades de clase ***
        private PlanEducativo LastObject
        {
            set { Session["lastPlanEducativo"] = value; }
            get { return Session["lastPlanEducativo"] as PlanEducativo; }
        }

        private Materia LastMateria
        {
            set { Session["lastMateria"] = value; }
            get { return Session["LastMateria"] as Materia; }
        }

        private string ClaveOriginal
        {
            set { Session["claveOriginal"] = value; }
            get { return Session["claveOriginal"] as string; }
        }

        private List<Materia> ListMaterias
        {
            get
            {
                return Session["listMateriasPlan"] != null ? Session["listMateriasPlan"] as
                    List<Materia> : null;
            }
            set { Session["listMateriasPlan"] = value; }
        }

        private DataTable DtMateriasEstatus
        {
            get
            {
                return Session["DtMateriasPlan"] != null ? Session["DtMateriasPlan"] as
                    DataTable : null;
            }
            set { Session["DtMateriasPlan"] = value; }
        }

        private DataTable DtMaterias
        {
            get
            {
                return Session["DtMaterias"] != null ? Session["DtMaterias"] as
                    DataTable : null;
            }
            set { Session["DtMaterias"] = value; }
        }
        #endregion

        public EditarPlanEducativo() 
        {
            nivelEducativoCtrl = new NivelEducativoCtrl();
            areaAplicacionCtrl = new AreaAplicacionCtrl();
            planCtrl = new PlanEducativoCtrl();
            materiaCtrl = new MateriaCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (LastObject != null)
                {
                    TxtID.Enabled = false;
                    cbGradoAsignatura.Enabled = false;
                    if (LastObject.Materias.Count() == 0)
                    {
                        LastObject = planCtrl.RetriveComplete(ConnectionHlp.Default.Connection, LastObject);
                    }
                    CargarDesdeTabla();
                    DataToUser(LastObject);
                    FillBack();
                }
                else
                {
                    ListMaterias = null;
                    LastMateria = null;
                    ClaveOriginal = null;
                    DtMateriasEstatus = null;
                    DtMaterias = null;
                    txtRedirect.Value = string.Empty;
                    Response.Redirect("~/CatalogoAsignaturas/BuscarProgramaEducativo.aspx", true);
                }

            }
        }

        protected void BtnGuardar_OnClick(Object sender, EventArgs e)
        {
            string validateMessage = validateDataPlan();
            if (validateMessage.Length < 1)
            {
                string messageEdit = DoEditPlan();
                if (messageEdit.Length < 1)
                {
                    try
                    {
                        planCtrl.UpdateComplete(ConnectionHlp.Default.Connection, LastObject, DtMateriasEstatus);
                        ListMaterias = null;
                        LastObject = null;
                        LastMateria = null;
                        DtMateriasEstatus = null;
                        ClaveOriginal = null;
                        DtMaterias = null;
                        Response.Redirect("~/CatalogoAsignaturas/BuscarProgramaEducativo.aspx", true);
                    }
                    catch (DataException ex)
                    {
                        txtRedirect.Value = string.Empty;
                        ShowMessage("Ocurrió un error al guardar la información", MessageType.Error);
                    }
                }
                else
                {
                    txtRedirect.Value = string.Empty;
                    ShowMessage(messageEdit, MessageType.Error);
                }
            }
            else
            {
                txtRedirect.Value = string.Empty;
                ShowMessage(validateMessage, MessageType.Error);
            }
        }

        protected void BtnAgregar_OnClick(Object sender, EventArgs e)
        {
            DoAdd();
        }

        protected void BtnCancelar_OnClick(object sender, EventArgs e)
        {
            ListMaterias = null;
            LastObject = null;
            LastMateria = null;
            ClaveOriginal = null;
            DtMateriasEstatus = null;
            DtMaterias = null;
            Response.Redirect("~/CatalogoAsignaturas/BuscarProgramaEducativo.aspx", true);
        }

        protected void grdAsignaturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        Materia materia = new Materia();
                        string materiaClv = (e.CommandArgument).ToString();
                        materia.Clave = materiaClv;
                        DoDelete(materia);
                        showDataMaterias();
                        break;
                    }
                case "editar":
                    {
                        Materia materia = new Materia();
                        string materiaClv = (e.CommandArgument).ToString();
                        materia.Clave = materiaClv;
                        DoEditMaterias(materia);
                        break;
                    }
                case "activar":
                    {
                        Materia materia = new Materia();
                        string materiaClv = (e.CommandArgument).ToString();
                        materia.Clave = materiaClv;
                        DoActivate(materia);
                        showDataMaterias();
                        ShowMessage("La materia se activó con éxito", MessageType.Information);
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }
            }
        }

        protected void grdAsignaturas_DataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void grdAsignaturas_Sorting(object sender, GridViewSortEventArgs e)
        {
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
        #endregion

        #region *** validaciones ***
        private string validateDataPlan()
        {
            string sError = string.Empty;

            string titulo = TxtTitulo.Text.Trim();
            string descripcion = TxtDescripcion.Text.Trim();
            string validoDesde = TxtValidoDesde.Text.Trim();
            string validoHasta = TxtValidoHasta.Text.Trim();

            if (string.IsNullOrEmpty(titulo))
                sError += ", Titulo del plan";

            if (string.IsNullOrEmpty(descripcion))
                sError += ", Descripción";

            if (string.IsNullOrEmpty(validoDesde))
                sError += ", Inicio de vigencia";

            if (string.IsNullOrEmpty(validoHasta))
                sError += ", Fin de vigencia";

            if (cbNivelEducativo.SelectedIndex <= 0)
                sError += ", Nivel educativo";

            if (sError.Length > 0)
                sError = "Los siguientes campos no pueden ser vacios: " + sError.Substring(2);
            return sError;
        }

        private string ValidateDataMateria()
        {
            string sError = string.Empty;

            string clave = TxtClaveAsignatura.Text.Trim();
            string descripcion = TxtTituloAsignatura.Text.Trim();

            if (string.IsNullOrEmpty(clave))
                sError += ", Clave de la materia";

            if (string.IsNullOrEmpty(descripcion))
                sError += ", Descripción de la materia";

            if (cbGradoAsignatura.SelectedIndex <= 0)
                sError += ", Grado de asignatura";

            if (cbAreaConocimiento.SelectedIndex <= 0)
                sError += ", Area de conocimiento";

            if (sError.Length > 0)
                sError = "Los siguientes campos no pueden ser vacios: " + sError.Substring(2);
            return sError;
        }

        private string ValidateInfoMateria()
        {
            string sError = string.Empty;
            Materia mat = new Materia();
            mat.Clave = TxtClaveAsignatura.Text.Trim();

            DataSet ds = materiaCtrl.Retrieve(ConnectionHlp.Default.Connection, mat);
            if (ds.Tables["Materia"].Rows.Count > 0)
            {
                sError = "Una asignatura similar ya se encuentra registrada, por favor verifique";
            }
            else
            {
                mat.Titulo = TxtTituloAsignatura.Text.Trim();
                mat.Grado = Convert.ToByte(cbGradoAsignatura.SelectedValue);
                mat.AreaAplicacion = GetAreaAplicativaFromUI();

                if (ListMaterias == null)
                    ListMaterias = new List<Materia>();

                Materia materiaExistente = ListMaterias.FirstOrDefault(item => item.Clave == mat.Clave);
                if (materiaExistente != null)
                {
                    sError = "Una asignatura similar ya se encuentra registrada, por favor verifique";
                }
                else
                {
                    ListMaterias.Add(mat);
                }

            }
            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void DataToUser(PlanEducativo planEducativo)
        {
            TxtID.Text = planEducativo.PlanEducativoID.Value.ToString();
            TxtTitulo.Text = planEducativo.Titulo;
            TxtDescripcion.Text = planEducativo.Descripcion;
            TxtValidoDesde.Text = string.Format("{0:dd/MM/yyyy}", planEducativo.ValidoDesde.Value);
            TxtValidoHasta.Text = string.Format("{0:dd/MM/yyyy}", planEducativo.ValidoHasta.Value);
            if (planEducativo.NivelEducativo.NivelEducativoID != null)
            {
                cbNivelEducativo.SelectedValue = planEducativo.NivelEducativo.NivelEducativoID.ToString();
                cbGradoAsignatura.Enabled = true;
                loadGrados(planEducativo.NivelEducativo);
            }
            if (ListMaterias == null)
            {
                ListMaterias = new List<Materia>();
                foreach (Materia mat in planEducativo.Materias)
                {
                    ListMaterias.Add(mat);
                }
            }

            if (ListMaterias.Count == 0)
            {
                foreach (Materia mat in planEducativo.Materias)
                {
                    ListMaterias.Add(mat);
                }
            }

            if (planEducativo.Estatus == false)
            {
                cbEstatus.SelectedValue = "false";
            }
            else
            {
                cbEstatus.SelectedValue = "true";
            }
            showDataMaterias();
        }

        protected void showDataMaterias()
        {
            if (DtMateriasEstatus == null)
            {
                DtMateriasEstatus = new DataTable();
                DtMateriasEstatus.Columns.Add("Clave", typeof(string));
                DtMateriasEstatus.Columns.Add("Estatus", typeof(string));
                DtMateriasEstatus.Columns.Add("Existente", typeof(bool));
            }


            DtMaterias = new DataTable();
            DtMaterias.Columns.Add("MateriaID", typeof(string));
            DtMaterias.Columns.Add("Clave", typeof(string));
            DtMaterias.Columns.Add("Titulo", typeof(string));
            DtMaterias.Columns.Add("Grado", typeof(string));
            DtMaterias.Columns.Add("AreaAplicacion", typeof(string));
            DtMaterias.Columns.Add("Estatus", typeof(string));
            DtMaterias.Columns.Add("Existente", typeof(bool));

            foreach (Materia mat in ListMaterias)
            {
                String status = "Nuevo";
                bool existente = true;
                if (mat.AreaAplicacion.AreaAplicacionID != null)
                {
                    if (mat.AreaAplicacion.Descripcion == null || mat.AreaAplicacion.Descripcion.Length < 1)
                    {
                        mat.AreaAplicacion = areaAplicacionCtrl.LastDataRowToAreaAplicacion(
                        areaAplicacionCtrl.Retrieve(ConnectionHlp.Default.Connection, mat.AreaAplicacion));
                    }
                }

                bool contenido = false;
                if (DtMateriasEstatus.Rows.Count > 0)
                {

                    foreach (DataRow data in DtMateriasEstatus.Rows)
                    {
                        if (data["Clave"].ToString() == mat.Clave)
                        {
                            contenido = true;
                            status = data["Estatus"].ToString();
                            existente = Boolean.Parse(data["Existente"].ToString());
                            break;
                        }
                    }
                }
                if (DtMateriasEstatus.Rows.Count == 0 || contenido == false)
                {
                    DataSet ds = materiaCtrl.retrieveMateriaPlanEducativo(ConnectionHlp.Default.Connection, mat, LastObject, null);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            if (Convert.ToBoolean(ds.Tables[0].Rows[0]["Estatus"]) == true)
                            {
                                status = "Activada";
                                existente = true;
                                DataRow drMateriasEstatus = DtMateriasEstatus.NewRow();
                                drMateriasEstatus[0] = mat.Clave;
                                drMateriasEstatus[1] = status;
                                drMateriasEstatus[2] = existente;
                                DtMateriasEstatus.Rows.Add(drMateriasEstatus);
                            }
                            else
                            {
                                existente = false;
                                status = "Desactivada";
                                DataRow drMateriasEstatus = DtMateriasEstatus.NewRow();
                                drMateriasEstatus[0] = mat.Clave;
                                drMateriasEstatus[1] = status;
                                drMateriasEstatus[2] = existente;
                                DtMateriasEstatus.Rows.Add(drMateriasEstatus);
                            }
                        }
                        catch (Exception ex)
                        {
                            status = "N/A";
                        }
                    }
                }

                DataRow dr = DtMaterias.NewRow();
                if (mat.MateriaID != null)
                {
                    dr[0] = mat.MateriaID.ToString();
                    dr[6] = existente;
                }
                else
                {
                    dr[0] = "Nuevo";
                    dr[6] = existente;
                }
                dr[1] = mat.Clave;
                dr[2] = mat.Titulo;
                dr[3] = mat.Grado;
                dr[4] = mat.AreaAplicacion.Descripcion;
                dr[5] = status;

                DtMaterias.Rows.Add(dr);
            }

            grdAsignaturas.DataSource = DtMaterias;
            grdAsignaturas.DataBind();
        }

        private void CargarDesdeTabla()
        {
            cbNivelEducativo.DataSource = configurarCbNivelEducativo();
            cbNivelEducativo.DataTextField = "Titulo";
            cbNivelEducativo.DataValueField = "NivelEducativoID";
            cbNivelEducativo.DataBind();

            cbAreaConocimiento.DataSource = configurarCbAreaConocimiento();
            cbAreaConocimiento.DataTextField = "Descripcion";
            cbAreaConocimiento.DataValueField = "AreaAplicacionID";
            cbAreaConocimiento.DataBind();
        }

        private DataSet configurarCbNivelEducativo()
        {
            DataSet ds = new DataSet();
            ds = nivelEducativoCtrl.Retrieve(ConnectionHlp.Default.Connection, new NivelEducativo());
            DataRow row = ds.Tables["NivelEducativo"].NewRow();
            row["NivelEducativoID"] = DBNull.Value;
            row["Titulo"] = "Seleccionar";
            ds.Tables["NivelEducativo"].Rows.InsertAt(row, 0);
            return ds;
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

        private DataSet configurarCbAreaConocimiento()
        {
            DataSet ds = new DataSet();
            ds = areaAplicacionCtrl.Retrieve(ConnectionHlp.Default.Connection, new AreaAplicacion());
            DataRow row = ds.Tables["AreaAplicacion"].NewRow();
            row["AreaAplicacionID"] = DBNull.Value;
            row["Descripcion"] = "Seleccionar";
            ds.Tables["AreaAplicacion"].Rows.InsertAt(row, 0);
            return ds;
        }
        #endregion

        #region *** UserInterface to Data ***
        private NivelEducativo GetNivelEducativoFromUI()
        {
            NivelEducativo nivelEducativo = new NivelEducativo();
            int nivelEducativoID = 0;

            int.TryParse(cbNivelEducativo.SelectedValue, out nivelEducativoID);

            if (nivelEducativoID > 0)
                nivelEducativo.NivelEducativoID = nivelEducativoID;

            return nivelEducativo;
        }

        private AreaAplicacion GetAreaAplicativaFromUI()
        {
            AreaAplicacion areaAplicativa = new AreaAplicacion();
            int areaAplicativaID = 0;

            int.TryParse(cbAreaConocimiento.SelectedValue, out areaAplicativaID);

            if (areaAplicativaID > 0)
                areaAplicativa.AreaAplicacionID = areaAplicativaID;
            DataSet ds = areaAplicacionCtrl.Retrieve(ConnectionHlp.Default.Connection, areaAplicativa);
            areaAplicativa = areaAplicacionCtrl.LastDataRowToAreaAplicacion(ds);
            return areaAplicativa;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoAdd()
        {
            String validateMessage = ValidateDataMateria();

            if (validateMessage.Length < 1)
            {
                String validateDataMessage = ValidateInfoMateria();
                if (validateDataMessage.Length < 1)
                {
                    showDataMaterias();
                    this.TxtClaveAsignatura.Text = "";
                    this.TxtTituloAsignatura.Text = "";
                    this.cbGradoAsignatura.SelectedIndex = 0;
                    this.cbAreaConocimiento.SelectedIndex = 0;
                }
                else
                {
                    txtRedirect.Value = string.Empty;
                    ShowMessage(validateDataMessage, MessageType.Error);
                }
            }
            else
            {
                txtRedirect.Value = string.Empty;
                ShowMessage(validateMessage, MessageType.Error);
            }
        }

        private void DoDelete(Materia materia)
        {
            materia = ListMaterias.FirstOrDefault(item => item.Clave == materia.Clave);
            if (materia != null)
            {
                DataSet dsMateria = materiaCtrl.Retrieve(ConnectionHlp.Default.Connection, materia);
                if (dsMateria.Tables[0].Rows.Count > 0)
                {
                    Materia materiaExistente = materiaCtrl.LastDataRowToMateria(dsMateria);
                    if (DtMateriasEstatus.Rows.Count > 0)
                    {
                        foreach (DataRow data in DtMateriasEstatus.Rows)
                        {
                            if (data["Clave"].ToString() == materia.Clave)
                            {
                                data["Estatus"] = "Desactivada";
                                data["Existente"] = false;
                                ShowMessage("La materia se desactivó con éxito", MessageType.Information);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    ListMaterias.Remove(materia);
                    ShowMessage("La materia se eliminó con éxito", MessageType.Information);
                }

            }
        }

        private void DoActivate(Materia materia)
        {
            materia = ListMaterias.FirstOrDefault(item => item.Clave == materia.Clave);
            if (materia != null)
            {
                DataSet dsMateria = materiaCtrl.Retrieve(ConnectionHlp.Default.Connection, materia);
                if (dsMateria.Tables[0].Rows.Count > 0)
                {
                    Materia materiaExistente = materiaCtrl.LastDataRowToMateria(dsMateria);
                    if (DtMateriasEstatus.Rows.Count > 0)
                    {
                        foreach (DataRow data in DtMateriasEstatus.Rows)
                        {
                            if (data["Clave"].ToString() == materia.Clave)
                            {
                                data["Estatus"] = "Activada";
                                data["Existente"] = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        ShowMessage("La materia no existe en el sistema", MessageType.Information);
                    }
                }

            }
        }

        private void DoEditMaterias(Materia materia)
        {
            materia = ListMaterias.FirstOrDefault(item => item.Clave == materia.Clave);
            if (materia != null)
            {
                LastMateria = materia;
                Response.Redirect("EditarAsignaturaPlanEducativo.aspx");
            }
        }

        private string DoEditPlan()
        {
            string sError = string.Empty;
            LastObject.Titulo = TxtTitulo.Text.Trim();
            LastObject.Descripcion = TxtDescripcion.Text.Trim();
            string validoDesde = TxtValidoDesde.Text.Trim();
            string validoHasta = TxtValidoHasta.Text.Trim();

            try
            {
                LastObject.ValidoDesde = Convert.ToDateTime(validoDesde);
                LastObject.ValidoHasta = Convert.ToDateTime(validoHasta);
            }
            catch
            {
                sError = "Las fechas son incorrectas.";
            }

            if (LastObject.ValidoDesde.Value.CompareTo(LastObject.ValidoHasta.Value) < 0)
            {
                LastObject.NivelEducativo = GetNivelEducativoFromUI();
                if (cbEstatus.SelectedValue == "false")
                {
                    LastObject.Estatus = false;
                }
                else
                {
                    LastObject.Estatus = true;
                }

                foreach (Materia mat in ListMaterias)
                {
                    LastObject.MateriaAgregar(mat);
                }

            }
            else
            {
                sError = "Las fechas son incorrectas.";
            }
            return sError;
        }

        private void FillBack()
        {
            lnkBack.NavigateUrl = "~/CatalogoAsignaturas/BuscarProgramaEducativo.aspx";
        }
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPLANEDUCATIVO) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARPLANEDUCATIVO) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARPLANEDUCATIVO) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARPLANEDUCATIVO) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!lectura)
                redirector.GoToHomePage(true);
            if (!edit)
                redirector.GoToHomePage(true);
            if (!delete)
                redirector.GoToHomePage(true);
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