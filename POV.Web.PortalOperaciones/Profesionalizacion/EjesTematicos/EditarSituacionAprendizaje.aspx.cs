using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.Seguridad.BO;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using System.Data;

namespace POV.Web.PortalOperaciones.Profesionalizacion.EjesTematicos
{
    public partial class EditarSituacionAprendizaje : PageBase
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private SituacionAprendizajeCtrl situacionAprendizajeCtrl;

        #region Propiedades de la clase

        private EjeTematico lastEje
        {
            get { return Session["lastEjeTematico"] != null ? Session["lastEjeTematico"] as EjeTematico : null; }
        }

        private SituacionAprendizaje lastObject
        {
            set { Session["lastSituacionAprendizaje"] = value; }
            get { return Session["lastSituacionAprendizaje"] != null ? Session["lastSituacionAprendizaje"] as SituacionAprendizaje : null; }
        }

        private List<AgrupadorSimple> agrupadores
        {
            set { Session["Agrupadores"] = value; }
            get { return Session["Agrupadores"] != null ? Session["Agrupadores"] as List<AgrupadorSimple> : null; }
        }

        public string EstadoSituacion
        {
            get { return this.DDLEstatusProfesionalizacion.SelectedValue; }
            set { this.DDLEstatusProfesionalizacion.SelectedValue = value; }
        }
        #endregion

        public EditarSituacionAprendizaje()
        {
            situacionAprendizajeCtrl = new SituacionAprendizajeCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtEjeTematico.Enabled = false;

                if (this.lastEje == null || this.lastObject == null || this.lastObject.SituacionAprendizajeID == null)
                {
                    txtRedirect.Value = "ConfigurarSituacionesAprendizaje.aspx";
                    ShowMessage("No se ha seleccionado un tema válido para edición", MessageType.Error);
                    return;
                }

                this.lastObject = this.situacionAprendizajeCtrl.RetrieveSimple(dctx, this.lastEje, this.lastObject);

                this.agrupadores = new List<AgrupadorSimple>();

                if (this.lastObject.AgrupadorContenidoDigital is AgrupadorCompuesto)
                    this.agrupadores = ((AgrupadorCompuesto)this.lastObject.AgrupadorContenidoDigital).AgrupadoresContenido.ConvertAll(x => x as AgrupadorSimple);

                EjeTematicoCtrl ctrl = new EjeTematicoCtrl();

                DataToUserInterface(ctrl.RetrieveComplete(dctx, this.lastEje), this.lastObject);
                LoadListClasificadores(this.agrupadores);

                FillBack();

            }

        }

        protected void btnAgregarClasificador_Click(object sender, EventArgs e)
        {
            DoInserClasificador();

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DoUpdate();
        }

        #region *** eventos del gridview de clasificadores ***
        protected void grdClasificadores_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdClasificadores.EditIndex = -1;
            LoadListClasificadores(this.agrupadores);
        }

        protected void grdClasificadores_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdClasificadores.EditIndex = e.NewEditIndex;
            LoadListClasificadores(this.agrupadores);
        }

        protected void grdClasificadores_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string nombre = (grdClasificadores.Rows[e.RowIndex].Cells[0].Controls[0] as TextBox).Text.Trim();
            string competencias = (grdClasificadores.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text.Trim();
            string aprendizajes = (grdClasificadores.Rows[e.RowIndex].Cells[2].Controls[0] as TextBox).Text.Trim();
            bool esPredeterminado = (grdClasificadores.Rows[e.RowIndex].Cells[3].Controls[0] as CheckBox).Checked;
            

            AgrupadorSimple agrupadorSimple = this.agrupadores.ElementAt(e.RowIndex);
            string sError = string.Empty;

            if (string.IsNullOrEmpty(nombre))
                sError += ", nombre del contenido requerido";

            if (nombre != agrupadorSimple.Nombre && !IsValidNombre(nombre))
                sError += (", el nombre de un contenido similar ya se encuentra registrado, porfavor verifique");
            if (esPredeterminado && !agrupadorSimple.EsPredeterminado.Value)
            {
                if (ExistPredeterminado())
                    sError += (", Solo puede seleccionar un contenido como predeterminado");
            }

            if (nombre.Length > 500)
                sError += ", nombre del contenido no puede ser mayor de 500 caracteres";

            if (competencias.Length > 800)
                sError += ", las competencias del contenido no puede ser mayor de 800 caracteres";

            if (aprendizajes.Length > 800)
                sError += ", los aprendizajes esperados del contenido no puede ser mayor de 800 caracteres";


            if (string.IsNullOrEmpty(sError))
            {
                agrupadorSimple.Nombre = nombre;
                agrupadorSimple.Competencias = competencias;
                agrupadorSimple.Aprendizajes = aprendizajes;
                agrupadorSimple.EsPredeterminado = esPredeterminado;

                this.agrupadores[e.RowIndex] = agrupadorSimple;
                grdClasificadores.EditIndex = -1;
                LoadListClasificadores(this.agrupadores);
            }
            else
            {
                txtRedirect.Value = string.Empty;
                ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
            }
        }

        protected void grdClasificadores_Deleting(object sender, GridViewDeleteEventArgs e)
        {
            agrupadores.RemoveAt(e.RowIndex);
            grdClasificadores.EditIndex = -1;
            LoadListClasificadores(agrupadores);
        }

        private void LoadListClasificadores(List<AgrupadorSimple> clasificadores)
        {
            DataTable dtClasificadores = new DataTable();

            dtClasificadores.Columns.Add("Orden", typeof(string));
            dtClasificadores.Columns.Add("Nombre", typeof(string));
            dtClasificadores.Columns.Add("Competencias", typeof(string));
            dtClasificadores.Columns.Add("Aprendizajes", typeof(string));
            dtClasificadores.Columns.Add("EsPredeterminado", typeof(bool));
            dtClasificadores.Columns.Add("AgrupadorID", typeof(long));

            int index = 0;

            foreach (AgrupadorSimple agrupadorContenidoDigital in clasificadores)
            {
                DataRow dr = dtClasificadores.NewRow();
                dr[0] = index;
                dr[1] = agrupadorContenidoDigital.Nombre;
                dr[2] = agrupadorContenidoDigital.Competencias;
                dr[3] = agrupadorContenidoDigital.Aprendizajes;
                dr[4] = agrupadorContenidoDigital.EsPredeterminado.Value;
                if (agrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
                    dr[5] = -1;
                else
                    dr[5] = agrupadorContenidoDigital.AgrupadorContenidoDigitalID.Value;
                dtClasificadores.Rows.Add(dr);
                index++;
            }
            grdClasificadores.DataSource = dtClasificadores;
            grdClasificadores.DataBind();
        }
        #endregion
        #endregion

        #region *** validaciones ***
        private string ValidateDataSituacion()
        {
            string sError = string.Empty;
            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();

            //Campos requeridos
            if (string.IsNullOrEmpty(nombre))
                sError += ", nombre del tema es requerido";
            if (agrupadores.Count == 0)
                sError += ", al menos un contenido es requerido";
            if (!ExistPredeterminado())
                sError += ", al menos un contenido debe ser predeterminado";

            if (sError.Length > 0)
                return sError;

            //Valores incorrectos

            if (nombre.Length > 200)
                sError += ", nombre del tema no puede ser mayor de 200 caracteres";

            if (descripcion.Length > 500)
                sError += ", descripción no puede ser mayor de 500 caracteres";

            return sError;

        }

        private string ValidateDataClasificador()
        {

            string sError = string.Empty;
            string clasificador = txtClasificador.Text.Trim();

            //Campos Requeridos
            if (string.IsNullOrEmpty(clasificador))
                sError += ", nombre del contenido es requerido";

            if (sError.Trim().Length > 0)
                return sError;

            //Valores incorrectos
            if (clasificador.Length > 500)
                sError += ", nombre del contenido no puede ser mayor de 500 caracteres";

            if (txtCompetencias.Text.Trim().Length > 800)
                sError += ", las competencias del contenido no puede ser mayor de 800 caracteres";

            if (txtAprendizajes.Text.Trim().Length > 800)
                sError += ", los aprendizajes esperados del contenido no puede ser mayor de 800 caracteres";

            return sError;
        }

        private string ValidateDataClasificadorExistente()
        {
            string sError = string.Empty;

            string agrupadorSimple = txtClasificador.Text.Trim();
            bool esPredeterminado = ChkEsDPredeterminado.Checked;

            if (!IsValidNombre(agrupadorSimple))
                sError += (", el nombre de un contenido similar ya se encuentra registrado, porfavor verifique");
            if (esPredeterminado == true)
            {
                if (ExistPredeterminado())
                    sError += (", solo puede seleccionar un contenido como predeterminado");
            }
            return sError;
        }
        private bool IsValidNombre(string nombre)
        {
            if (agrupadores.Count > 0)
                return agrupadores.FirstOrDefault(item => item.Nombre == nombre) == null;
            else
                return true;
        }
        private bool ExistPredeterminado()
        {
            return agrupadores.Any(p => p.EsPredeterminado == true);
        }

        #endregion

        #region *** Data to UserInterface ***

        private void DataToUserInterface(EjeTematico ejeTematico, SituacionAprendizaje situacion)
        {
            txtNivelEducativo.Text = ejeTematico.AreaProfesionalizacion.NivelEducativo.Descripcion;
            txtGrado.Text = ejeTematico.AreaProfesionalizacion.Grado.ToString() + "°";
            txtAsignatura.Text = ejeTematico.AreaProfesionalizacion.Nombre;
            txtBloque.Text = ejeTematico.MateriasProfesionalizacion.FirstOrDefault().Nombre;
            this.txtEjeTematico.Text = string.Format("{0}",  ejeTematico.Nombre.Trim());
            this.txtSituacionID.Text = situacion.SituacionAprendizajeID.ToString();
            this.txtNombre.Text = situacion.Nombre.Trim();
            this.txtDescripcion.Text = situacion.Descripcion.Trim();

            this.LoadEstadoProfesionalizacion();
            this.EstadoSituacion = ((Byte)situacion.EstatusProfesionalizacion).ToString();
        }

        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Profesionalizacion/SituacionesAprendizaje/ConfigurarSituacionesAprendizaje.aspx";
        }

        private void LoadEstadoProfesionalizacion()
        {
            var dictionary = new Dictionary<int, string>();
            foreach (byte value in Enum.GetValues(typeof(EEstatusProfesionalizacion)))
            {
                if (value != (Byte)EEstatusProfesionalizacion.INACTIVO)
                    dictionary.Add(value, Enum.GetName(typeof(EEstatusProfesionalizacion), value));
            }
            DDLEstatusProfesionalizacion.DataSource = dictionary;
            DDLEstatusProfesionalizacion.DataTextField = "Value";
            DDLEstatusProfesionalizacion.DataValueField = "Key";
            DDLEstatusProfesionalizacion.DataBind();
        }
        #endregion

        #region *** UserInterface to Data ***

        private SituacionAprendizaje GetSituacionAprendizajeFromUI()
        {
            SituacionAprendizaje situacionAprendizaje = new SituacionAprendizaje();
            situacionAprendizaje.SituacionAprendizajeID = this.lastObject.SituacionAprendizajeID;   // se toma el mismo ID original
            situacionAprendizaje.Nombre = txtNombre.Text.Trim();
            situacionAprendizaje.Descripcion = txtDescripcion.Text.Trim();
            situacionAprendizaje.EstatusProfesionalizacion = (EEstatusProfesionalizacion)int.Parse(this.EstadoSituacion);
            situacionAprendizaje.FechaRegistro = this.lastObject.FechaRegistro; // se toma la fecha de registro original

            return situacionAprendizaje;
        }

        private AgrupadorCompuesto GetAgrupadorCompuestoFromUI()
        {
            AgrupadorCompuesto agrupadorCompuesto = new AgrupadorCompuesto();
            agrupadorCompuesto = this.lastObject.AgrupadorContenidoDigital.Clone() as AgrupadorCompuesto;
            agrupadorCompuesto.InitList();

            agrupadorCompuesto.Nombre = txtNombre.Text.Trim();

            foreach (AgrupadorSimple agrupador in agrupadores)
            {
                agrupadorCompuesto.Agregar(agrupador);
            }

            return agrupadorCompuesto;
        }

        private AgrupadorSimple GetAgrupadorSimpleFromUI()
        {
            AgrupadorSimple agrupadorSimple = new AgrupadorSimple();
            agrupadorSimple.Nombre = txtClasificador.Text.Trim();
            if (!String.IsNullOrEmpty(txtCompetencias.Text.Trim()))
                agrupadorSimple.Competencias = txtCompetencias.Text.Trim();
            if (!String.IsNullOrEmpty(txtAprendizajes.Text.Trim()))
                agrupadorSimple.Aprendizajes = txtAprendizajes.Text.Trim();
            agrupadorSimple.EsPredeterminado = ChkEsDPredeterminado.Checked;
            agrupadorSimple.FechaRegistro = DateTime.Now;
            agrupadorSimple.Estatus = EEstatusProfesionalizacion.ACTIVO;

            return agrupadorSimple;
        }

        #endregion

        #region ***metodos auxiliarres ***
        private void LimpiarSesion()
        {
            agrupadores = null;
        }

        private void DoUpdate()
        {
            string ValidateMessage = ValidateDataSituacion();

            if (string.IsNullOrEmpty(ValidateMessage))
            {
                SituacionAprendizaje situacionAprendizaje = GetSituacionAprendizajeFromUI();
                situacionAprendizaje.AgrupadorContenidoDigital = GetAgrupadorCompuestoFromUI();
                try
                {
                    situacionAprendizajeCtrl.UpdateComplete(dctx, this.lastEje, situacionAprendizaje, this.lastObject);

                    LimpiarSesion();
                    txtRedirect.Value = "ConfigurarSituacionesAprendizaje.aspx";
                    ShowMessage("El tema se ha modificado con éxito", MessageType.Information);
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message, MessageType.Error);
                }

            }
            else
            {
                txtRedirect.Value = string.Empty;
                ShowMessage("Errores en los campos: " + ValidateMessage.Substring(2), MessageType.Error);
            }
        }

        private void DoInserClasificador()
        {
            string sError = ValidateDataClasificador();
            string sErrorRepetido = ValidateDataClasificadorExistente();

            if (string.IsNullOrEmpty(sError))
            {
                if (string.IsNullOrEmpty(sErrorRepetido))
                {
                    AgrupadorSimple agrupadorSimple = GetAgrupadorSimpleFromUI();

                    agrupadores.Add(agrupadorSimple);
                    LoadListClasificadores(agrupadores);

                    txtClasificador.Text = "";
                    txtCompetencias.Text = "";
                    txtAprendizajes.Text = "";
                    ChkEsDPredeterminado.Checked = false;
                }
                else
                {
                    txtRedirect.Value = string.Empty;
                    ShowMessage("Errores en los campos: " + sErrorRepetido.Substring(2), MessageType.Error);
                }

            }
            else
            {
                txtRedirect.Value = string.Empty;
                ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
            }
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOSITUACIONES) != null;
            bool edicion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARSITUACIONES) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!edicion)
                redirector.GoToHomePage(true);
        }
        #endregion
    }
}