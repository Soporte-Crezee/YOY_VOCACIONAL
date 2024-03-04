using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Web.PortalOperaciones.Helper;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalOperaciones.AppCode.Page;


namespace POV.Web.PortalOperaciones.Usuarios
{
    public partial class BuscarUsuarios : CatalogPage
    {
        private UsuarioCtrl usuarioCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        #region *** propiedades de clase ***
        private Usuario LastObject
        {
            set { Session["lastUsuarioCatalogo"] = value; }
            get { return Session["lastUsuarioCatalogo"] != null ? Session["lastUsuarioCatalogo"] as Usuario : null; }
        }

        private DataSet DsUsuarios
        {
            get { return Session["dsUsuarios"] != null ? Session["dsUsuarios"] as DataSet : null; }
            set { Session["dsUsuarios"] = value; }
        }
        #endregion

        public BuscarUsuarios()
        {
            usuarioCtrl = new UsuarioCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Usuario usuario = new Usuario();
                DsUsuarios = usuarioCtrl.Retrieve(dctx, usuario);
                grdUsuarios.DataSource = DsUsuarios;
                grdUsuarios.DataBind();
            }
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                try
                {
                    Usuario usuario = UserInterfaceToData();
                    DsUsuarios = usuarioCtrl.Retrieve(dctx, usuario);
                    grdUsuarios.DataSource = DsUsuarios;
                    grdUsuarios.DataBind();
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message, MessageType.Error);
                }

            }
            else
            {
                ShowMessage(validateMessage.Substring(2), MessageType.Warning);
            }
        }

        protected void GrdUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        Usuario usuario = new Usuario();
                        usuario.UsuarioID = int.Parse(e.CommandArgument.ToString());
                        DoDelete(usuario);
                        break;
                    }
                case "privilegios":
                    {
                        Usuario usuario = new Usuario();
                        usuario.UsuarioID = int.Parse(e.CommandArgument.ToString());
                        LastObject = usuario;
                        Response.Redirect("~/Usuarios/EditarPrivilegios.aspx", true);
                        break;
                    }
                case "editar":
                    {
                        Usuario usuario = new Usuario();
                        usuario.UsuarioID = int.Parse(e.CommandArgument.ToString());
                        LastObject = usuario;
                        Response.Redirect("~/Usuarios/EditarUsuario.aspx", true);
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

            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***

        #endregion

        #region *** UserInterface to Data ***
        private Usuario UserInterfaceToData()
        {
            Usuario usuario = new Usuario();
            string nombreUsuario = TxtNombreUsuario.Text.Trim();
            string email = TxtEmail.Text.Trim();

            if (!string.IsNullOrEmpty(nombreUsuario))
                usuario.NombreUsuario = nombreUsuario;
            if (!string.IsNullOrEmpty(email))
                usuario.Email = email;

            bool activo;
            if (bool.TryParse(CbActivo.SelectedValue, out activo))
            {
                usuario.EsActivo = activo;
            }

            return usuario;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoDelete(Usuario usuario)
        {
            usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));
            Usuario usuarioNuevo = new Usuario();
            usuarioNuevo = (Usuario)usuario.Clone();

            usuarioNuevo.EsActivo = false;

            try
            {
                usuarioCtrl.Update(dctx, usuarioNuevo, usuario);
                ShowMessage("El usuario se desactivó con éxito", MessageType.Information);
                foreach (DataRow row in DsUsuarios.Tables[0].Rows)
                {
                    if (row["UsuarioID"].ToString() == usuario.UsuarioID.ToString())
                    {
                        row["EsActivo"] = false;
                        break;
                    }

                }
                grdUsuarios.DataSource = DsUsuarios;
                grdUsuarios.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Ocurrió un problema y no se desactivó el perfil", MessageType.Information);
            }
        }

        #region ordenamiento
        protected void GrdUsuarios_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataSet dataSet = DsUsuarios;

            if (dataSet != null)
            {
                DataView dataView = new DataView(dataSet.Tables[0]);
                dataView.Sort = e.SortExpression + " " + GetSortDirection();
                DsUsuarios.Tables.Clear();
                DsUsuarios.Tables.Add(dataView.ToTable());
                grdUsuarios.DataSource = DsUsuarios;
                grdUsuarios.DataBind();
            }
        }

        private string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    GridViewSortDirection = "DESC";
                    break;

                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    GridViewSortDirection = "ASC";
                    break;
            }

            return newSortDirection;
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
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected void grdUsuarios_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (RenderEdit)
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                    btnEdit.Visible = true;

                    LinkButton btnPrivilegios = (LinkButton)e.Row.FindControl("btnPrivilegios");
                    btnPrivilegios.Visible = true;
                }

                if (RenderDelete)
                {
                    ImageButton boton = (ImageButton)e.Row.FindControl("btnDelete");
                    boton.Visible = true;
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

            grdUsuarios.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOUSUARIO) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARUSUARIOS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARUSUARIOS) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARUSUARIOS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARUSUARIOS) != null;

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