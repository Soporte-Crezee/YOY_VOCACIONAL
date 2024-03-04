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

namespace POV.Web.PortalOperaciones.Perfiles
{
    public partial class BuscarPerfiles : CatalogPage
    {
        private PerfilCtrl perfilCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        #region *** propiedades de clase ***
        private Perfil LastObject
        {
            set { Session["lastPerfilCatalogo"] = value; }
            get { return Session["lastPerfilCatalogo"] != null ? Session["lastPerfilCatalogo"] as Perfil : null; }
        }

        private DataSet DsPerfiles
        {
            get { return Session["dsPerfiles"] != null ? Session["dsPerfiles"] as DataSet : null; }
            set { Session["dsPerfiles"] = value; }
        }
        #endregion

        public BuscarPerfiles()
        {
            perfilCtrl = new PerfilCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Perfil perfil = new Perfil();
                DsPerfiles = perfilCtrl.Retrieve(dctx, perfil);
                grdPerfiles.DataSource = DsPerfiles;
                grdPerfiles.DataBind();
            }
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                try
                {
                    Perfil perfil = UserInterfaceToData();
                    DsPerfiles = perfilCtrl.Retrieve(dctx, perfil);
                    grdPerfiles.DataSource = DsPerfiles;
                    grdPerfiles.DataBind();
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
        protected void GrdPerfiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        Perfil perfil = new Perfil();
                        perfil.PerfilID = int.Parse(e.CommandArgument.ToString());
                        DoDesactivar(perfil);
                        break;
                    }
                case "permisos":
                    {
                        Perfil perfil = new Perfil();
                        perfil.PerfilID = int.Parse(e.CommandArgument.ToString());
                        LastObject = perfil;
                        Response.Redirect("~/Perfiles/EditarPermisos.aspx", true);
                        break;
                    }
                case "editar":
                    {
                        Perfil perfil = new Perfil();
                        perfil.PerfilID = int.Parse(e.CommandArgument.ToString());
                        LastObject = perfil;
                        Response.Redirect("~/Perfiles/EditarPerfil.aspx", true);
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
            if (!string.IsNullOrEmpty(txtPerfilID.Text.Trim()))
            {
                int perfilID = 0;
                bool result = int.TryParse(txtPerfilID.Text.Trim(), out perfilID);
                if (!result)
                    sError += ", El identificador debe ser un número entero";
            }
            if (!string.IsNullOrEmpty(txtNombrePerfil.Text.Trim()))
            {
                if (txtNombrePerfil.Text.Trim().Length > 40)
                    sError += ",El nombre no debe ser mayor de 40 caracteres";
            }
            if (!string.IsNullOrEmpty(txtDescripcion.Text.Trim()))
            {
                if (txtDescripcion.Text.Trim().Length > 100)
                    sError += ",La descripción no debe ser mayor de 100 caracteres";
            }
            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***

        #endregion

        #region *** UserInterface to Data ***
        private Perfil UserInterfaceToData()
        {
            Perfil perfil = new Perfil();
            int idPerfil = 0;
            if (int.TryParse(txtPerfilID.Text.Trim(), out idPerfil))
            {
                perfil.PerfilID = idPerfil;
            }
            if (txtNombrePerfil.Text.Trim().Length > 0)
                perfil.Nombre = txtNombrePerfil.Text.Trim() + "%";
            if (txtDescripcion.Text.Trim().Length > 0)
                perfil.Descripcion = txtDescripcion.Text.Trim() + "%";

            bool activo;
            if (bool.TryParse(cbEstatus.SelectedValue, out activo))
            {
                perfil.Estatus = activo;
            }

            return perfil;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoDesactivar(Perfil perfil)
        {
            perfil = perfilCtrl.LastDataRowToPerfil(perfilCtrl.Retrieve(dctx, perfil));
            Perfil nuevoPerfil = new Perfil();
            nuevoPerfil = (Perfil)perfil.Clone();

            nuevoPerfil.Estatus = false;

            try
            {
                perfilCtrl.Update(dctx, nuevoPerfil, perfil);
                ShowMessage("El perfil se desactivó con éxito", MessageType.Information);
                foreach (DataRow row in DsPerfiles.Tables[0].Rows)
                {
                    if (row["PerfilID"].ToString() == perfil.PerfilID.ToString())
                    {
                        row["Estatus"] = false;
                        break;
                    }

                }
                grdPerfiles.DataSource = DsPerfiles;
                grdPerfiles.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Ocurrió un problema y no se desactivó el perfil", MessageType.Information);
            }
        }
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected void grdPerfiles_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (RenderEdit)
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                    btnEdit.Visible = true;

                    LinkButton btnPrivilegios = (LinkButton)e.Row.FindControl("btnPermisos");
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

            grdPerfiles.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOPERFILES) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARPERFILES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARPERFILES) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARPERFILES) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARPERFILES) != null;

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