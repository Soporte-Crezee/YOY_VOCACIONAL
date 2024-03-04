using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;
using System.IO;
using POV.Operaciones.Service;


namespace POV.Web.PortalOperaciones.Profesionalizacion.CatalogoCursos
{
    public partial class BuscarCurso : CatalogPage
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private TemaCursoCtrl temaCursoCtrl;
        private CursoCtrl cursoCtrl;
        private CatalogoCursoCtrl catalogoCursoCtrl;


        public BuscarCurso()
        {
            temaCursoCtrl = new TemaCursoCtrl();
            cursoCtrl = new CursoCtrl();
            catalogoCursoCtrl = new CatalogoCursoCtrl();
        }
        #region *** propiedades de clase ***
        public DataSet DsCursosCatalogo
        {
            set { Session["DsCursosCatalogo"] = value; }
            get { return Session["DsCursosCatalogo"] != null ? Session["DsCursosCatalogo"] as DataSet : null; }
        }

        public Curso LastObject
        {
            set { Session["lastCurso"] = value; }
            get { return Session["lastCurso"] != null ? Session["lastCurso"] as Curso : null; }
        }

        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }

        public string CursoID { get { return this.txtID.Text; } }
        public string NombreCurso { get { return this.txtNombre.Text; } }
        public string Modalidad { get { return this.DDLPresencial.SelectedValue; } }
        public string EstadoCurso { get { return this.DDLEstatusProfesionalizacion.SelectedValue; } }
        public string Tema { get { return this.DDLTema.SelectedValue; } }
        #endregion

        #region Eventos de Página
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LastObject = null;
                this.DsCursosCatalogo = null;
                this.DataToUserInterface();
                this.BuscarCursos();
            }

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.BuscarCursos();
        }

        protected void grdCursos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        Curso curso = new Curso() { AgrupadorContenidoDigitalID = long.Parse(e.CommandArgument.ToString()) };
                        DoDelete(curso);
                        break;
                    }
                case "editar":
                    {
                        Curso curso = new Curso() { AgrupadorContenidoDigitalID = long.Parse(e.CommandArgument.ToString()) };
                        LastObject = curso;
                        Response.Redirect("EditarCurso.aspx", true);
                        break;
                    }
                case "asignarcontenido":
                    {
                        Curso curso = new Curso() { AgrupadorContenidoDigitalID = long.Parse(e.CommandArgument.ToString()) };
                        LastObject = curso;
                        Response.Redirect("AsignarContenidoDigital.aspx", true);
                        break;
                    }
                case "Sort":
                    {
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }
            }
        }

        protected void grdCursos_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void grdCursos_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataSet dataSet = this.DsCursosCatalogo;
            if (dataSet != null)
            {
                DataView dataView = new DataView(dataSet.Tables[0]);
                dataView.Sort = e.SortExpression + " " + GetSortDirection();
                DsCursosCatalogo.Tables.Clear();
                DsCursosCatalogo.Tables.Add(dataView.ToTable());
                this.grdCursos.DataSource = DsCursosCatalogo;
                this.grdCursos.DataBind();
            }
        }
        #endregion

        #region *** validaciones ***
        //METODOS PARA VALIDACIONES
        private string ValidateData()
        {
            string sError = string.Empty;

            string sID = txtID.Text.Trim();
            long id = 0;
            if (!string.IsNullOrEmpty(sID) && !long.TryParse(sID, out id))
                sError += " El identificador debe ser numérico. ";

            string nombre = txtNombre.Text.Trim();
            if (nombre.Length > 200)
                sError += " El nombre no debe ser mayor a 200 caracteres";

            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        //METODOS DE CONVERSION OBJETO A INFORMACION PARA LOS CONTROLES DE UI
        private void BuscarCursos()
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                Curso curso = UserIntefaceToData();
                DsCursosCatalogo = this.cursoCtrl.Retrieve(dctx, curso);
                LoadCursos(DsCursosCatalogo);
            }
            else
                ShowMessage(validateMessage.Substring(2), MessageType.Warning);
        }

        private void DataToUserInterface()
        {
            this.LoadTema();
            this.LoadPresencial();
            this.LoadEstadoProfesionalizacion();
        }

        private void LoadTema()
        {
            DataSet ds = this.temaCursoCtrl.Retrieve(dctx, new TemaCurso() { Activo = true });
            DDLTema.DataSource = ds;
            DDLTema.DataTextField = "Nombre";
            DDLTema.DataValueField = "TemaCursoID";
            DDLTema.DataBind();
            DDLTema.Items.Insert(0, new ListItem("SELECCIONE...", "-1"));
        }

        private void LoadPresencial()
        {
            var dictionary = new Dictionary<int, string>();
            foreach (byte value in Enum.GetValues(typeof(EPresencial)))
            {
                dictionary.Add(value, Enum.GetName(typeof(EPresencial), value));
            }
            DDLPresencial.DataSource = dictionary;
            DDLPresencial.DataTextField = "Value";
            DDLPresencial.DataValueField = "Key";
            DDLPresencial.DataBind();
            DDLPresencial.Items.Insert(0, new ListItem("SELECCIONE...", "-1"));
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
            DDLEstatusProfesionalizacion.Items.Insert(0, new ListItem("SELECCIONE...", "-1"));
        }

        private void LoadCursos(DataSet ds)
        {
            this.grdCursos.DataSource = ConfigureGridResults(ds);
            this.grdCursos.DataBind();
        }

        private DataSet ConfigureGridResults(DataSet ds)
        {
            if (!ds.Tables[0].Columns.Contains("NombreTemaCurso"))
                ds.Tables[0].Columns.Add("NombreTemaCurso");
            if (!ds.Tables[0].Columns.Contains("NombreModalidad"))
                ds.Tables[0].Columns.Add("NombreModalidad");
            if (!ds.Tables[0].Columns.Contains("NombreEstatus"))
                ds.Tables[0].Columns.Add("NombreEstatus");
            List<TemaCurso> temasCurso = new List<TemaCurso>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int nInt = 0;

                TemaCurso tema;
                if (int.TryParse(row["TemaCursoID"].ToString(), out nInt))
                {
                    tema = temasCurso.SingleOrDefault(t => t.TemaCursoID == nInt);
                    if (tema == null)
                        tema = this.temaCursoCtrl.LastDataRowToTemaCurso(this.temaCursoCtrl.Retrieve(dctx, new TemaCurso() { TemaCursoID = nInt }));
                }
                else
                    tema = new TemaCurso() { Nombre = "ERROR: Tema no encontrado!!!" };
                row["NombreTemaCurso"] = tema.Nombre;

                if (int.TryParse(row["Presencial"].ToString(), out nInt))
                    row["NombreModalidad"] = (EPresencial)nInt;
                if (int.TryParse(row["EstatusProfesionalizacion"].ToString(), out nInt))
                    row["NombreEstatus"] = (EEstatusProfesionalizacion)nInt;
            }

            return ds;
        }
        #endregion

        #region *** UserInterface to Data ***
        //METODOS DE CONVERSION DE UI A OBJETO CONCRETO

        private Curso UserIntefaceToData()
        {
            Curso cursoReturn = new Curso() { TemaCurso = new TemaCurso() };

            long nLong = 0;
            int nInt = 0;

            if (this.NombreCurso.Trim() != string.Empty)
                cursoReturn.Nombre = string.Format("%{0}%", this.NombreCurso.Trim());
            if (this.CursoID.Trim() != string.Empty)
                if (long.TryParse(this.CursoID.Trim(), out nLong))
                    cursoReturn.AgrupadorContenidoDigitalID = nLong;
            if (int.TryParse(this.Tema, out nInt))
                if (nInt > -1)
                    cursoReturn.TemaCurso.TemaCursoID = nInt;
            if (int.TryParse(this.Modalidad, out nInt))
                if (nInt > -1)
                    cursoReturn.Presencial = (EPresencial)nInt;
            if (int.TryParse(this.EstadoCurso, out nInt))
                if (nInt > -1)
                    cursoReturn.Estatus = (EEstatusProfesionalizacion)nInt;

            return cursoReturn;
        }
        #endregion

        #region *** metodos auxiliares ***
        //CUALQUIER METODO AUXILIAR
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

        private void DoDelete(Curso curso)
        {
            curso = this.cursoCtrl.LastDataRowToAAgrupadorContenidoDigital(this.cursoCtrl.Retrieve(dctx, curso)) as Curso;
            if (curso != null)
            {
                string rutaDoc = string.Empty;
                string fileDel = string.Empty;
                string pathDel = string.Empty;
                if (!string.IsNullOrEmpty(curso.Informacion))
                {
                    rutaDoc = curso.Informacion;
                    if (!rutaDoc.StartsWith("~")) rutaDoc = "~" + rutaDoc;
                    fileDel = Path.GetFileName(rutaDoc);
                    pathDel = Path.GetDirectoryName(rutaDoc);
                    if (pathDel != string.Empty && !pathDel.EndsWith("\\"))
                        pathDel += "\\";
                }
                this.catalogoCursoCtrl.DeleteComplete(dctx, curso, Server.MapPath(pathDel));
            }

            this.BuscarCursos();
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
        private bool RenderEdit = false;
        private bool RenderDelete = false;

        protected override void AuthorizeUser()
        {
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCURSOS) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARCURSOS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARCURSOS) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARCURSOS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCURSOS) != null;

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


        protected override void DisplayCreateAction()
        {
            PnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {
            grdCursos.Visible = true;
        }

        protected override void DisplayUpdateAction()
        {
            RenderEdit = true;
        }

        protected override void DisplayDeleteAction()
        {
            RenderDelete = true;
        }
        #endregion


    }
}