using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.Core.Operaciones.Implements;
using POV.Core.Operaciones.Interfaces;
using POV.Operaciones.Service;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.BO;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Comun.Service;
using POV.Comun.BO;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;
using POV.Licencias.BO;
using POV.Licencias.Service;

namespace POV.Web.PortalOperaciones.CiclosEscolares
{
    public partial class BuscarCicloEscolar : PageBase
    {
        private CicloEscolarCtrl cicloEscolarCtrl;
        private CicloContratoCtrl cicloContratoCtrl;
        private ContratoCtrl contratoCtrl;

        private UbicacionCtrl ubicacionCtrl;

        #region *** propiedades de clase ***
        DataSet DSCicloEscolar
        {
            get { return Session["ciclosescolares"] != null ? (DataSet)Session["ciclosescolares"] : null; }
            set { Session["ciclosescolares"] = value; }
        }
        private CicloContrato LastObject
        {
            get { return Session["lastCicloContrato"] != null ? (CicloContrato)Session["lastCicloContrato"] : null; }
            set { Session["lastCicloContrato"] = value; }
        }

        private Contrato SS_LastObject
        {
            set { Session["lastContrato"] = value; }
            get { return Session["lastContrato"] != null ? (Contrato)Session["lastContrato"] : null; }
        }

        #endregion

        public BuscarCicloEscolar()
        {
            cicloEscolarCtrl = new CicloEscolarCtrl();
            ubicacionCtrl = new UbicacionCtrl();
            cicloContratoCtrl = new CicloContratoCtrl();
            contratoCtrl = new ContratoCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession != null && userSession.IsLogin())
                {
                    if (!IsPostBack)
                    {
                        //Asignacion de Ultimo contrato registrado
                        SS_LastObject = (contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(ConnectionHlp.Default.Connection, new Contrato() { })));
                        
                        if (SS_LastObject == null)
                        {
                            txtRedirect.Value = "../Contratos/BuscarContrato.aspx";
                            ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                            return;
                        }

                        LoadCicloEscolar();
                        
                    }
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        protected void grdCiclosEscolar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "editar":
                        CicloContrato ciclo = new CicloContrato { CicloContratoID = long.Parse(e.CommandArgument.ToString()) };
                        LastObject = ciclo;
                        redirector.GoToEditarCicloEscolar(false);
                        break;
                    case "eliminar":
                        CicloContrato dtciclo = new CicloContrato { CicloContratoID = long.Parse(e.CommandArgument.ToString()) };
                        DoDelete(dtciclo);
                        LoadCicloEscolar();
                        break;
                    case "config":
                        CicloContrato ccciclo = new CicloContrato { CicloContratoID = long.Parse(e.CommandArgument.ToString()) };
                        LastObject = ccciclo;
                        Response.Redirect("~/Contratos/Recursos/ConfigurarRecursosCiclo.aspx");
                        break;
                }

            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void BtnCancelar_OnClick(object sender, EventArgs e)
        {
            LimpiarObjetosSesion();
            Response.Redirect("~/Contratos/BuscarContrato.aspx", true);
        }

        #endregion

        #region *** validaciones ***
        private void CicloEscolarValidateData()
        {
            

        }
        #endregion

        #region *** Data to UserInterface ***
        private void LoadCicloEscolar()
        {
            SS_LastObject = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(ConnectionHlp.Default.Connection,SS_LastObject));
            TxtInicioContrato.Text = String.Format("{0:dd/MM/yyyy}", SS_LastObject.InicioContrato);
            TxtFinContrato.Text = String.Format("{0:dd/MM/yyyy}", SS_LastObject.FinContrato);
            TxtClave.Text = SS_LastObject.Clave;
            TxtFechaContrato.Text = String.Format("{0:dd/MM/yyyy}", SS_LastObject.FechaContrato);

            DSCicloEscolar = cicloContratoCtrl.Retrieve(ConnectionHlp.Default.Connection, SS_LastObject, new CicloContrato { Activo = true });
            DSCicloEscolar.Tables[0].Columns.Add("Titulo");
            DSCicloEscolar.Tables[0].Columns.Add("InicioCiclo");
            DSCicloEscolar.Tables[0].Columns.Add("FinCiclo");
            DSCicloEscolar.Tables[0].Columns.Add("Descripcion");
            DSCicloEscolar.Tables[0].Columns.Add("Liberado");
            DSCicloEscolar.Tables[0].Columns.Add("EsActivo");
            if (DSCicloEscolar.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in DSCicloEscolar.Tables[0].Rows)
                {
                    CicloEscolar cicloEscolar = new CicloEscolar { CicloEscolarID = int.Parse(row["CicloEscolarID"].ToString()) };

                    cicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(ConnectionHlp.Default.Connection, cicloEscolar));

                    row["Titulo"] = cicloEscolar.Titulo;
                    row["InicioCiclo"] = String.Format("{0:dd/MM/yyyy}", cicloEscolar.InicioCiclo);
                    row["FinCiclo"] = String.Format("{0:dd/MM/yyyy}", cicloEscolar.FinCiclo);
                    row["Descripcion"] = cicloEscolar.Descripcion;

                    bool liberado = bool.Parse(row["EstaLiberado"].ToString());
                    bool activo = bool.Parse(row["Activo"].ToString());
                    row["Liberado"] = liberado ? "Si" : "No";
                    row["EsActivo"] = activo ? "Si" : "No";
                }
            }
            grdCiclosEscolar.DataSource = DSCicloEscolar;
            grdCiclosEscolar.DataBind();
        }

        
        #endregion

        #region *** UserInterface to Data ***
        private CicloEscolar UserInterfaceToData()
        {

            CicloEscolar cicloEscolar = new CicloEscolar();
          
            return cicloEscolar;

        }
        #endregion

        #region *** metodos auxiliares ***
        


        private void LimpiarObjetosSesion()
        {
            LastObject = null;
            SS_LastObject = null;
        }
        private void DoDelete(CicloContrato ciclo)
        {
            try
            {
                cicloContratoCtrl.DeleteComplete(ConnectionHlp.Default.Connection,SS_LastObject, ciclo);
            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw new Exception("Ocurrió un error mientras se eliminaba el registro");
            }
        }

        private string EscapeLike(string valueWithoutWildcards)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueWithoutWildcards.Length; i++)
            {
                char c = valueWithoutWildcards[i];
                if (c == '*' || c == '%' || c == '[' || c == ']')
                    sb.Append("[").Append(c).Append("]");
                else if (c == '\'')
                    sb.Append("''");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected void grdCiclosEscolar_DataBound(object sender, GridViewRowEventArgs e)
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
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btndelete");
                    btnDelete.Visible = true;
                }
            }
        }

        private bool RenderEdit = false;
        private bool RenderDelete = false;

        protected  void DisplayCreateAction()
        {
            PnlCreate.Visible = true;
        }

        protected  void DisplayReadAction()
        {

            grdCiclosEscolar.Visible = true;
        }

        protected  void DisplayUpdateAction()
        {
            RenderEdit = true;
        }

        protected  void DisplayDeleteAction()
        {
            RenderDelete = true;
        }

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCONTRATO) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARCONTRATOS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCONTRATOS) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!lectura)
                redirector.GoToHomePage(true);
            if (!edit)
                redirector.GoToHomePage(true);
            else
            {
                DisplayReadAction();
                DisplayCreateAction();
                DisplayDeleteAction();
                DisplayUpdateAction();
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
      
    }
}