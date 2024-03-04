using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using Framework.Base.DataAccess;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Core.Operaciones;
using POV.Core.Operaciones.Interfaces;
using POV.Core.Operaciones.Implements;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Modelo.Estandarizado.BO;
using POV.Operaciones.Service;

namespace POV.Web.PortalOperaciones.CatalogoClasificador
{
    public partial class BuscarClasificadorModelo : System.Web.UI.Page
    {
        #region Propiedades
        AModelo LastObject
        {
            get { return Session["lastModelo"] != null ? Session["lastModelo"] as ModeloDinamico : null; }
            set { Session["lastModelo"] = value; }
        }

        Clasificador LastObjectClasifica
        {
            get { return Session["LastObjectClasifica"] != null ? Session["LastObjectClasifica"] as Clasificador : null; }
            set { Session["LastObjectClasifica"] = value; }
        }

        public DataSet DsClasificadores
        {
            set { Session["clasificadores"] = value; }
            get { return Session["clasificadores"] != null ? Session["clasificadores"] as DataSet : null; }
        }
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "DESC"; }
            set { ViewState["SortDirection"] = value; }
        }
        private IRedirector redirector;
        private IUserSession userSession;
        private ModeloCtrl modeloPruebaCtrl;
        private IDataContext dctx;

        public BuscarClasificadorModelo()
        {
            redirector = new Redirector();
            modeloPruebaCtrl = new ModeloCtrl();
            userSession = new UserSession();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {                                        
                    LoadClasificadoresModelo();
                }
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                redirector.GoToHomePage(true);
            }
        }

        #region DataToUserInterface
        private void LoadClasificadoresModelo()
        {
            dctx = Helper.ConnectionHlp.Default.Connection;
            DsClasificadores = modeloPruebaCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, LastObject as ModeloDinamico);
            foreach (DataRow row in DsClasificadores.Tables[0].Rows)
            {
                Clasificador clasificador = modeloPruebaCtrl.DataRowToClasificador(row);
                row["Activo"] = clasificador.Activo == true ? "True" : "False";
            }

            DsClasificadores.Tables[0].DefaultView.Sort = "ClasificadorID ASC";
            DataTable dt = DsClasificadores.Tables[0].DefaultView.ToTable();
            DsClasificadores.Tables[0].Rows.Clear();
            foreach (DataRow row in dt.Rows)
                DsClasificadores.Tables[0].Rows.Add(row.ItemArray);

            grdClasificadores.DataSource = DsClasificadores;
            grdClasificadores.DataBind();
            grdClasificadores.Visible = true;
        }
        #endregion

        #region *****> UserInterface to Data <*****
        private Clasificador UserInterfaceToData()
        {
            dctx = Helper.ConnectionHlp.Default.Connection;
            Clasificador clasifica = new Clasificador();
            clasifica.ClasificadorID = !string.IsNullOrEmpty(txtID.Text.Trim()) ? int.Parse(txtID.Text.Trim()) : (int?)null;
            clasifica.Nombre = !string.IsNullOrEmpty(txtNombre.Text.Trim()) ? string.Format("%{0}%", txtNombre.Text.Trim()) : null;
            if (string.IsNullOrEmpty(clasifica.Nombre)) clasifica.Nombre = null;            
            clasifica.Activo = true;

            return clasifica;            
        }
        #endregion

        #region *****> Métodos Auxiliares <*****
        private void LoadClasificadores()
        {
            Clasificador clasificador = UserInterfaceToData();
            dctx = Helper.ConnectionHlp.Default.Connection;
            DsClasificadores = modeloPruebaCtrl.RetrieveClasificador(dctx, clasificador, LastObject as ModeloDinamico);

            DsClasificadores.Tables[0].DefaultView.Sort = "ClasificadorID ASC";
            DataTable dt = DsClasificadores.Tables[0].DefaultView.ToTable();
            DsClasificadores.Tables[0].Rows.Clear();
            foreach (DataRow row in dt.Rows)
                DsClasificadores.Tables[0].Rows.Add(row.ItemArray);

            grdClasificadores.DataSource = DsClasificadores;
            grdClasificadores.DataBind();
            grdClasificadores.Visible = true;
        }
        #endregion

        #region *** validaciones ***
        private string ValidateData()
        {
            string sError = string.Empty;
            string sID = txtID.Text.Trim();
            int id = 0;

            if (sID.Length > 0 && !int.TryParse(sID, out id))
                sError += ", id debe ser un número valido";

            if (txtNombre.Text.Trim().Length > 200)
                sError += ", el nombre solo acepta 200 caracteres";


            return sError;
        }
        #endregion

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string validateMessage = ValidateData();
            if (string.IsNullOrEmpty(validateMessage))
            {
                LoadClasificadores();
            }
            else
                ShowMessage(validateMessage.Substring(2), MessageType.Warning);
        }
        
        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {                    
                    case "editar":
                        LastObjectClasifica = new Clasificador { ClasificadorID = int.Parse(e.CommandArgument.ToString()) };
                        Response.Redirect("~/CatalogoClasificador/EditarClasificadorModelo.aspx", true);
                        break;
                    case "eliminar":
                        dctx = Helper.ConnectionHlp.Default.Connection;
                        Clasificador clasificador = new Clasificador { ClasificadorID = int.Parse(e.CommandArgument.ToString()) };
                        try
                        {

                            CatalogoModeloCtrl catalogoModeloCtrl = new CatalogoModeloCtrl();

                            catalogoModeloCtrl.DeleteClasificador(dctx, clasificador);

                            LoadClasificadores();
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message, MessageType.Error);
                        }
                        break;
                    case "ver":
                        break;
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
       
        protected void grd_DataBound(object sender, GridViewRowEventArgs e)
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