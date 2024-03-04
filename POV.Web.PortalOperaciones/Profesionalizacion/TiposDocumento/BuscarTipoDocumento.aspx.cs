using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.Service;
using POV.Seguridad.BO;
using System.Data;

namespace POV.Web.PortalOperaciones.Profesionalizacion.TiposDocumento
{
    public partial class BuscarTipoDocumento : CatalogPage
    {
        private TipoDocumentoCtrl tipoDocumentoCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        #region *** propiedades de la clase ***
        private TipoDocumento LastObject
        {
            set { Session["lastTipoDocumento"] = value; }
            get { return Session["lastTipoDocumento"] != null ? Session["lastTipoDocumento"] as TipoDocumento : null; }
        }
        private DataSet DsTipoDocumento
        {
            set { Session["DsTipoDocumento"] = value; }
            get { return Session["DsTipoDocumento"] != null ? Session["DsTipoDocumento"] as DataSet : null; }
        }
        #endregion

        public BuscarTipoDocumento()
        {
            tipoDocumentoCtrl = new TipoDocumentoCtrl();
        }
        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                DatatoUserInterface();
            }

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string ValidateMessage = ValidateData();
            if (string.IsNullOrEmpty(ValidateMessage))
            {
                TipoDocumento tipoDocumento = UserInterfaceToData();
                DsTipoDocumento = tipoDocumentoCtrl.Retrieve(dctx, tipoDocumento);
                grdTiposDocumento.DataSource = DsTipoDocumento;
                grdTiposDocumento.DataBind();

            }
            else
            {

                ShowMessage(ValidateMessage, MessageType.Error);
            }

        }
        protected void grdTipoDocumento_RowComand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        TipoDocumento tipoDocumento = new TipoDocumento();
                        tipoDocumento.TipoDocumentoID = int.Parse(e.CommandArgument.ToString());
                        DoDelete(tipoDocumento);
                        DatatoUserInterface();
                        ShowMessage("El Tipo de Documento se ha eliminado con éxito", MessageType.Information);
                    }
                    break;

                case "editar":
                    {
                        TipoDocumento tipoDocumento = new TipoDocumento();
                        tipoDocumento.TipoDocumentoID = int.Parse(e.CommandArgument.ToString());
                        LastObject = tipoDocumento;
                        Response.Redirect("EditarTipoDocumento.aspx");
                    }
                    break;
                case "Sort":
                    {
                        break;
                    }
                default:
                    {
                        ShowMessage("Comando no Encontrado", MessageType.Error);
                        break;
                    }
            }
        }

        #endregion

        #region *** validaciones ***

        private string ValidateData()
        {
            string sError = string.Empty;
            int tipoDocumentoID;

            if (!string.IsNullOrEmpty(txtID.Text.Trim()))
            {
                bool result = int.TryParse(txtID.Text.Trim(), out tipoDocumentoID);
                if (!result)
                    sError = "El identificador debe ser numero entero";
            }
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                if (txtNombre.Text.Trim().Length > 50)
                    sError = "El tamaño del texto no debe ser mayor a 50 caracteres";
            }
            if (!string.IsNullOrEmpty(txtExtension.Text.Trim()))
            {
                if (txtExtension.Text.Trim().Length > 30)
                    sError = "El tamaño del texto no debe ser mayor a 30 caracteres";
            }
            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***

        private void DatatoUserInterface()
        {
            DsTipoDocumento = tipoDocumentoCtrl.Retrieve(dctx, new TipoDocumento { Activo = true });
            grdTiposDocumento.DataSource = DsTipoDocumento;
            grdTiposDocumento.DataBind();
        }

        #endregion

        #region *** UserInterface to Data ***
        private TipoDocumento UserInterfaceToData()
        {
            TipoDocumento tipoDocumento = new TipoDocumento();
            int tipoDocumentoID;
            tipoDocumento.Activo = true;
            if (int.TryParse(txtID.Text.Trim(), out tipoDocumentoID))
            {
                tipoDocumento.TipoDocumentoID = tipoDocumentoID;
            }
            if (txtNombre.Text.Trim().Length > 0)
            {
                tipoDocumento.Nombre = string.Format("%{0}%", txtNombre.Text.Trim());
                
            }
            if (txtExtension.Text.Trim().Length > 0)
            {
                tipoDocumento.Extension = txtExtension.Text.Trim();
            }
           
            return tipoDocumento;
        }

        #endregion

        #region *** metodos auxiliares***

        private void DoDelete(TipoDocumento tipoDocumento)
        {
            tipoDocumentoCtrl.Delete(dctx, tipoDocumento);
        }


        #endregion

        #region *****Message  Showing*****
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="typeNotification">1: Error, 2: Advertencia, 3: Información</param>
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

        protected void grdTipoDocumento_DataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dataRowView = e.Row.DataItem as DataRowView;
                bool EsEditable = (Boolean)Convert.ChangeType(dataRowView["EsEditable"], typeof(Boolean));

                if (RenderEdit && EsEditable)
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                    btnEdit.Visible = true;
                }
                if (RenderDelete && EsEditable)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");
                    btnDelete.Visible = true;
                }
            }
        }

        private bool RenderEdit = false;
        private bool RenderDelete = false;

        protected override void DisplayCreateAction()
        {
            pnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {
            grdTiposDocumento.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOTIPODOCUMENTO) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARTIPODOCUMENTO) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARTIPODOCUMENTO) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARTIPODOCUMENTO) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARTIPODOCUMENTO) != null;

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