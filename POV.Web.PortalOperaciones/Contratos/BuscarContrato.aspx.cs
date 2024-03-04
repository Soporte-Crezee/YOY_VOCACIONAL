using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Web.PortalOperaciones.Helper;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.Contratos
{
    public partial class BuscarContrato : CatalogPage
    {
        private ContratoCtrl contratoCtrl;
        private UbicacionCtrl ubicacionCtrl;
        
        #region *** propiedades de clase ***
        private DataSet SS_Contratos
        {
            get { return (DataSet)this.Session["ResultadoContratos"]; }
            set { this.Session["ResultadoContratos"] = value; }
        }

        private Contrato SS_LastObject
        {
            set { Session["lastContrato"] = value; }
            get { return Session["lastContrato"] != null ? (Contrato)Session["lastContrato"] : null; }
        }
        #endregion

        public BuscarContrato()
        {
            contratoCtrl = new ContratoCtrl();
            ubicacionCtrl = new UbicacionCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SS_LastObject = null;
                LoadContratos(new Contrato());
            } 
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Contrato contrato = UserInterfaceToData();
            LoadContratos(contrato);
        }

        protected void grdEstados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "config":
                    {
                        Contrato contrato = new Contrato();
                        contrato.ContratoID = int.Parse(e.CommandArgument.ToString());
                        this.SS_LastObject = contrato;
                        Response.Redirect("~/CiclosEscolares/BuscarCicloEscolar.aspx");
                        break;
                    }
                case "Editar":
                    {
                        Contrato contrato = new Contrato();
                        contrato.ContratoID = int.Parse(e.CommandArgument.ToString());
                        this.SS_LastObject = contrato;
                        Response.Redirect("EditarContrato.aspx");
                        break;
                    }
                case "activar":
                    {
                        Contrato contrato = new Contrato();
                        contrato.ContratoID = int.Parse(e.CommandArgument.ToString());
                        contrato.Estatus = true;
                        DoUpdate(contrato);
                        break;
                    }
                case "desactivar":
                    {
                        Contrato contrato = new Contrato();
                        contrato.ContratoID = int.Parse(e.CommandArgument.ToString());
                        contrato.Estatus = false;
                        DoUpdate(contrato);
                        break;
                    }
                case "Sort":
                    {
                        break;
                    }
                case "asignarEjes":
                    {
                        Contrato contrato = new Contrato();
                        contrato.ContratoID = int.Parse(e.CommandArgument.ToString());
                        this.SS_LastObject = contrato;
                        Response.Redirect("AsignarEjeContrato.aspx");
                        break;
                    }               
                default:
                    {
                        ShowMessage("Comando no encontrado", MessageType.Error);
                        break;
                    }
            }
        }
        #endregion

        #region *** validaciones ***

        #endregion

        #region *** Data to UserInterface ***
        private void LoadContratos(Contrato contrato)
        {
            DataSet dsContratos = contratoCtrl.Retrieve(ConnectionHlp.Default.Connection, contrato);

            this.SS_Contratos = ConfigureGridResults(dsContratos);
            grdContratos.DataSource = this.SS_Contratos;
            grdContratos.DataBind();

        }

        private DataSet ConfigureGridResults(DataSet ds)
        {
            ds.Tables[0].Columns.Add("Consumidas");
            ds.Tables[0].Columns.Add("Disponibles");
            ds.Tables[0].Columns.Add("EstatusNombre");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Contrato contrato = contratoCtrl.DataRowToContrato(row);

                if (contrato.LicenciasLimitadas.Value)
                {
                    int disponibles = 0;
                    contratoCtrl.TieneLicenciasDisponibles(ConnectionHlp.Default.Connection, contrato, out disponibles);

                    row["Consumidas"] = contrato.NumeroLicencias - disponibles;
                    row["Disponibles"] = disponibles;
                }
                else
                {
                    row["Consumidas"] = "No aplica";
                    row["Disponibles"] = "No aplica";
                }

                row["EstatusNombre"] = contrato.Estatus.Value ? "Activo" : "Inactivo";
            }

            return ds;
        }
        #endregion

        #region *** UserInterface to Data ***
        private Contrato UserInterfaceToData()
        {
            Contrato contrato = new Contrato();
            contrato.Cliente = new Cliente();

            contrato.Clave = this.txtClave.Text.Trim();
            contrato.Cliente.Nombre = this.txtNombreCte.Text.Trim();

            return contrato;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoUpdate(Contrato contrato)
        {
            Contrato contratoAnterior = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(ConnectionHlp.Default.Connection, new Contrato { ContratoID = contrato.ContratoID }));
            contratoAnterior.Ubicacion = ubicacionCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, contratoAnterior.Ubicacion);

            Contrato contratoUpd = (Contrato)contratoAnterior.Clone();
            contratoUpd.Estatus = contrato.Estatus;

            contratoCtrl.Update(ConnectionHlp.Default.Connection, contratoUpd, contratoAnterior);

            foreach (DataRow dr in this.SS_Contratos.Tables[0].Rows)
            {
                if ((Int64)Convert.ChangeType(dr["ContratoID"], typeof(Int64)) == contratoUpd.ContratoID)
                {
                    dr["Estatus"] = contratoUpd.Estatus;
                    dr["EstatusNombre"] = contratoUpd.Estatus.Value ? "Activo" : "Inactivo";
                    break;
                }
            }

            grdContratos.DataSource = this.SS_Contratos;
            grdContratos.DataBind();
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

        protected void grdContratos_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView contrato = (DataRowView)e.Row.DataItem;
                if (RenderEdit)
                {

                    ImageButton btnConfig = (ImageButton)e.Row.FindControl("btnConfig");
                    btnConfig.Visible = true;
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");

                    btnEdit.Visible = true;
                    ImageButton btnAsignarEje = (ImageButton)e.Row.FindControl("btnAsignarEje");
                    btnAsignarEje.Visible = true;

                    if ((bool)contrato["Estatus"] == false)
                    {
                        ImageButton btnActivate = (ImageButton)e.Row.FindControl("btnActivate");
                        btnActivate.Visible = true;
                    }
                }


                if (RenderDelete && (bool)contrato["Estatus"] == true)
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

            grdContratos.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCONTRATO) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARCONTRATOS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARCONTRATOS) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARCONTRATOS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCONTRATOS) != null;

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