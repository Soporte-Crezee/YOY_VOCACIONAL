using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Core.Operaciones;
using POV.Core.Operaciones.Implements;
using POV.Core.Operaciones.Interfaces;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;

namespace POV.Web.PortalOperaciones.Profesionalizacion.CatalogoAsistencias
{
    public partial class BuscarAsistencias : CatalogPage
    {

        #region Propiedades

        Asistencia LastObject
        {
            get { return Session["lastAsistencia"] != null ? Session["lastAsistencia"] as Asistencia : null; }
            set { Session["lastAsistencia"] = value; }
        }
        DataSet DsAsistencias
        {
            get { return Session["DsAsistencias"] != null ? Session["DsAsistencias"] as DataSet : null; }
            set { Session["DsAsistencias"] = value; }
        }

        private AsistenciaCtrl asistenciaCtrl;
        private TemaAsistenciaCtrl temaAsistenciaCtrl;
        private IDataContext dctx;
        private IRedirector redirector;
       
        #endregion

        public BuscarAsistencias()
        {
            redirector = new Redirector();
            asistenciaCtrl = new AsistenciaCtrl();
            temaAsistenciaCtrl= new TemaAsistenciaCtrl();
        }

        #region *****Eventos de pagina*****


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    LastObject = null;
                    DsAsistencias = null;
                    LoadEstatusProfesionalizacion();
                    LoadTemaAsistencia();
                    DoSearch(); 
                }
            }
            catch (Exception ex)
            {

                Logger.Service.LoggerHlp.Default.Error(this, ex);
                redirector.GoToHomePage(true);
            }

        }
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                DoSearch();
            }
            catch (Exception ex)
            {

                Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        protected void grdAsistencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "asignarcontenido":
                        LastObject = new Asistencia { AgrupadorContenidoDigitalID = long.Parse(e.CommandArgument.ToString()) };
                        Response.Redirect("~/Profesionalizacion/CatalogoAsistencias/AsignarContenidoAsistencia.aspx", true);
                        break;
                    case "editar":
                        LastObject = new Asistencia { AgrupadorContenidoDigitalID = long.Parse(e.CommandArgument.ToString()) };
                        Response.Redirect(UrlHelper.GetEditarAsistenciaURL(),true);
                        break;
                    case "eliminar":
                        Asistencia  asistencia= new Asistencia { AgrupadorContenidoDigitalID = long.Parse(e.CommandArgument.ToString()) };
                        DoDelete(asistencia);
                        break;
                    case "ver":
                        break;
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message,MessageType.Error);
            }
        }
        protected void grdAsistencias_DataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (RenderEdit)
                    {
                        ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                        btnEdit.Visible = true;
                        LinkButton btnasingarContenido = (LinkButton)e.Row.FindControl("lnkbtnAsignarContenido");
                        btnasingarContenido.Visible = true;
                    }

                    if (RenderDelete)
                    {
                        ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");
                        btnDelete.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Validaciones

        private void ValidateData()
        {
            string sError = string.Empty;
            //valores incorrectos
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()) && txtNombre.Text.Trim().Length > 100)
                sError += " ,Nombre por favor, no escribas más de 100 caracteres ";

            long id;
            if (!string.IsNullOrEmpty(txtIdentificador.Text.Trim()) && !long.TryParse(txtIdentificador.Text.Trim(),out id))
                sError += " ,Identificador por favor, escribe un número entero válido.";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format(" Los siguientes parámetros son inválidos: {0} ", sError));
            }
        }

        #endregion
        #region *****UserInterface to Data *****

        private Asistencia UserInterfaceToData()
        {
            Asistencia asistencia = new Asistencia();
            asistencia.AgrupadorContenidoDigitalID = !string.IsNullOrEmpty(txtIdentificador.Text.Trim()) ? long.Parse(txtIdentificador.Text.Trim()) : (long?)null;
            asistencia.Nombre = !string.IsNullOrEmpty(txtNombre.Text.Trim()) ? txtNombre.Text.Trim() : null;
            if (ddlEstatus.SelectedIndex >= 1)
            {
                byte estatus = byte.Parse(ddlEstatus.SelectedItem.Value);
                asistencia.Estatus = (EEstatusProfesionalizacion?)estatus;
            }
            if (ddlTemaAsistencia.SelectedIndex >= 1)
            {
                asistencia.TemaAsistencia = new TemaAsistencia { TemaAsistenciaID = int.Parse(ddlTemaAsistencia.SelectedItem.Value) };
            }

            return asistencia;
        }

        #endregion

        #region*****Metodos auxiliares*****

        private void LoadTemaAsistencia()
        {
            dctx = Helper.ConnectionHlp.Default.Connection;
            DataSet ds = temaAsistenciaCtrl.Retrieve(dctx, new TemaAsistencia { Activo = true });
            ddlTemaAsistencia.DataSource = ds;
            ddlTemaAsistencia.DataTextField = "Nombre";
            ddlTemaAsistencia.DataValueField = "TemaAsistenciaID";
            ddlTemaAsistencia.DataBind();
            ddlTemaAsistencia.Items.Insert(0, new ListItem("SELECCIONE...", "-1"));
        }
        private void LoadEstatusProfesionalizacion()
        {
            Array items = Enum.GetValues(typeof(EEstatusProfesionalizacion));

            ddlEstatus.Items.Clear();
            ddlEstatus.Items.Add(new ListItem("TODOS", "-1"));
            foreach (byte item in items)
            {
                if (item != (byte)EEstatusProfesionalizacion.INACTIVO)
                {
                    var itemname = Enum.GetName(typeof(EEstatusProfesionalizacion), item);
                    ddlEstatus.Items.Add(new ListItem(itemname, item.ToString()));
                }
            }
        }
        private void DoDelete(Asistencia asistencia)
        {
            try
            {
                dctx = Helper.ConnectionHlp.Default.Connection;
                asistenciaCtrl.DeleteComplete(dctx, asistencia);
                this.ShowMessage("Asistencia eliminada exitosamente", MessageType.Information);
                this.DoSearch();

            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw new Exception("Ocurrió un error al eliminar la asistencia");

            }
        }
        private void DoSearch()
        {
            try
            {
                ValidateData();
            }
            catch (Exception ex)
            {
              
                ShowMessage(ex.Message, MessageType.Error);
                return;
            }

            Asistencia asistencia = UserInterfaceToData();
            dctx = Helper.ConnectionHlp.Default.Connection;

            //Agregar comodines de ser necesarios
            asistencia.Nombre = !string.IsNullOrEmpty(asistencia.Nombre) ? string.Format("%{0}%", asistencia.Nombre) : null;

            DataSet ds = asistenciaCtrl.Retrieve(dctx, asistencia);
            DsAsistencias = ConfigureGridResults(ds);
            grdAsistencias.DataSource = DsAsistencias;
            grdAsistencias.DataBind();
        }
        private DataSet ConfigureGridResults(DataSet ds)
        {
            if (!ds.Tables[0].Columns.Contains("NombreTemaAsistencia"))
                ds.Tables[0].Columns.Add(new DataColumn("NombreTemaAsistencia", typeof(string)));

            if (!ds.Tables[0].Columns.Contains("NombreEstatus"))
                ds.Tables[0].Columns.Add(new DataColumn("NombreEstatus", typeof(string)));
            foreach (DataRow row in ds.Tables[0].Rows)
            {

                Asistencia asistencia = asistenciaCtrl.DataRowToAsistencia(row);
                //Agregar el Estado
                row["NombreEstatus"] = asistencia.Estatus.ToString();

                //Agregar TemaAsistencia
                DataSet aux = temaAsistenciaCtrl.Retrieve(dctx, asistencia.TemaAsistencia);
                int index = aux.Tables[0].Rows.Count;
                if (index > 0)
                {
                    asistencia.TemaAsistencia = temaAsistenciaCtrl.DataRowToTemaAsistencia(aux.Tables[0].Rows[index - 1]);
                    row["NombreTemaAsistencia"] = asistencia.TemaAsistencia.Nombre;
                }
            }

            return ds;
        }
        private void ClearControls()
        {
            txtIdentificador.Text = "";
            txtNombre.Text = "";
            ddlTemaAsistencia.SelectedIndex = -1;
            ddlEstatus.SelectedIndex = -1;
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

        protected override void DisplayCreateAction()
        {
            PnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {
            grdAsistencias.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOASISTENCIA) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARASISTENCIA) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARASISTENCIA) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARASISTENCIA) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARASISTENCIA) != null;

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