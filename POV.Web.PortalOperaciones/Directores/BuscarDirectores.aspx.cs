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
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.Directores
{
    public partial class BuscarDirectores : CatalogPage
    {
        private DirectorCtrl directorCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        #region *** propiedades de clase ***
        private Director LastObject
        {
            set { Session["lastDirectorCatalogo"] = value; }
            get { return Session["lastDirectorCatalogo"] != null ? Session["lastDirectorCatalogo"] as Director : null; }
        }

        private DataSet DsDirectores
        {
            get { return Session["dsDirectores"] != null ? Session["dsDirectores"] as DataSet : null; }
            set { Session["dsDirectores"] = value; }
        }
        #endregion

        public BuscarDirectores()
        {
            directorCtrl = new DirectorCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DsDirectores = directorCtrl.Retrieve(dctx, new Director());
                FillDataGrid();
            }

        }
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                try
                {
                    Director director = UserInterfaceToData();
                    DsDirectores = directorCtrl.Retrieve(dctx, director);
                    FillDataGrid();
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
        protected void grdDirectores_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        Director director = new Director();
                        director.DirectorID = int.Parse(e.CommandArgument.ToString());
                        try
                        {
                            director = directorCtrl.LastDataRowToDirector(directorCtrl.Retrieve(dctx, director));
                            directorCtrl.Delete(dctx, director);
                            foreach (DataRow row in DsDirectores.Tables[0].Rows)
                            {
                                Director directorTemp = directorCtrl.DataRowToDirector(row);
                                if (directorTemp.DirectorID == director.DirectorID)
                                {
                                    row["Estatus"] = false;
                                    row["Estado"] = "Inactivo";
                                    break;
                                }

                            }
                            grdDirectores.DataSource = DsDirectores;
                            grdDirectores.DataBind();
                            ShowMessage("El director se desactivó con éxito", MessageType.Information);
                        }
                        catch (Exception ex)
                        {
                            ShowMessage("El director no pudo ser desactivado", MessageType.Information);
                        }

                        break;
                    }


                case "editar":
                    {
                        Director director = new Director();
                        director.DirectorID = int.Parse(e.CommandArgument.ToString());
                        LastObject = director;
                        Response.Redirect("~/Directores/EditarDirector.aspx", true);
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

            if (!string.IsNullOrEmpty(txtCurp.Text.Trim()))
            {
                if (txtCurp.Text.Trim().Length > 18)
                    sError += ", El CURP no debe ser mayor de 18 caracteres";
            }
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                if (txtNombre.Text.Trim().Length > 100)
                    sError += ",El Nombre no debe ser mayor de 200 caracteres";
            }
            if (!string.IsNullOrEmpty(txtPrimerApellido.Text.Trim()))
            {
                if (txtPrimerApellido.Text.Trim().Length > 100)
                    sError += ",El primer apellido no debe ser mayor de 100 caracteres";
            }
            if (!string.IsNullOrEmpty(txtSegundoApellido.Text.Trim()))
            {
                if (txtSegundoApellido.Text.Trim().Length > 100)
                    sError += ",El segundo apellido no debe ser mayor de 100 caracteres";
            }
            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void FillDataGrid()
        {
            if (DsDirectores.Tables.Count > 0)
            {
                DsDirectores.Tables[0].Columns.Add("NombreCompleto", typeof(string));
                DsDirectores.Tables[0].Columns.Add("Estado", typeof(string));

                foreach (DataRow row in DsDirectores.Tables[0].Rows)
                {
                    Director dir = directorCtrl.DataRowToDirector(row);
                    row["NombreCompleto"] = dir.Nombre + " " + dir.PrimerApellido + " " + dir.SegundoApellido;
                    if (dir.Estatus == true)
                        row["Estado"] = "Activo";
                    else
                        row["Estado"] = "Inactivo";
                }
                grdDirectores.DataSource = DsDirectores;
                grdDirectores.DataBind();
            }
        }
        #endregion

        #region *** UserInterface to Data ***
        private Director UserInterfaceToData()
        {
            Director director = new Director();
            string curp = txtCurp.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            string primerApellido = txtPrimerApellido.Text.Trim();
            string segundoApellido = txtSegundoApellido.Text.Trim();
            if (!string.IsNullOrEmpty(curp))
                director.Curp = curp;
            if (!string.IsNullOrEmpty(nombre))
                director.Nombre = nombre;
            if (!string.IsNullOrEmpty(primerApellido))
                director.PrimerApellido = primerApellido;
            if (!string.IsNullOrEmpty(segundoApellido))
                director.SegundoApellido = segundoApellido;

            bool activo;
            if (bool.TryParse(cbEstatus.SelectedValue, out activo))
            {
                director.Estatus = activo;
            }

            bool sexo;
            if (bool.TryParse(cbSexo.SelectedValue, out sexo))
            {
                director.Sexo = sexo;
            }


            return director;
        }
        #endregion

        #region *** metodos auxiliares ***

        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected void grdDirectores_DataBound(object sender, GridViewRowEventArgs e)
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

        private bool RenderEdit = false;
        private bool RenderDelete = false;

        protected override void DisplayCreateAction()
        {
            PnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {

            grdDirectores.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESODIRECTORES) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARDIRECTORES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARDIRECTORES) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARDIRECTORES) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARDIRECTORES) != null;

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