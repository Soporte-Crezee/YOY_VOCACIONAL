using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;

namespace POV.Web.PortalOperaciones.Pruebas
{
    public partial class LiberarPrueba : PageBase
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        private CatalogoPruebaCtrl catalogoPruebaCtrl;
        private ModeloCtrl modeloCtrl;

        public APrueba LastObject
        {
            set { Session["lastPruebas"] = value; }
            get { return Session["lastPruebas"] != null ? Session["lastPruebas"] as APrueba : null; }
        }

        private List<AEscalaDinamica> ListaEscalasDinamicas
        {
            get { return Session["ListaEscalasEdit"] != null ? Session["ListaEscalasEdit"] as List<AEscalaDinamica> : null; }
            set { Session["ListaEscalasEdit"] = value; }
        }

        public LiberarPrueba()
        {
            this.catalogoPruebaCtrl = new CatalogoPruebaCtrl();
            this.modeloCtrl = new ModeloCtrl();
        }

        #region Eventos de página
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.LastObject != null)
                {
                    try
                    {
                        this.LoadModelos();
                        this.DataToUserInterface();
                    }
                    catch (Exception ex)
                    {
                        Logger.Service.LoggerHlp.Default.Error(this, ex);
                        redirector.GoToHomePage(true);
                    }
                }
                else
                {
                    txtRedirect.Value = "BuscarReactivos.aspx";
                    ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                }
            }
        }

        protected void ddlModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MostrarControlesDeModelo();
        }

        protected void BtnGuardar_OnClick(object sender, EventArgs e)
        {
            AModelo modelo = this.GetModeloFromUI();

            string sError = this.ValidateData(modelo);

            if (string.IsNullOrEmpty(sError))
            {
                try
                {
                    APrueba pruebaUpdate = this.UserInterfaceToData(modelo);
                    pruebaUpdate.EsDiagnostica = this.LastObject.EsDiagnostica;
                    pruebaUpdate.PruebaID = this.LastObject.PruebaID;

                    pruebaUpdate.EstadoLiberacionPrueba = EEstadoLiberacionPrueba.LIBERADA;

                    this.catalogoPruebaCtrl.Update(dctx, pruebaUpdate, this.LastObject);
                }
                catch (Exception ex)
                {
                    sError += ex.Message;
                }
            }

            if (string.IsNullOrEmpty(sError))
            {
                txtRedirect.Value = "BuscarPruebas.aspx";
                ShowMessage("Actualización exitosa.", MessageType.Information);
            }
            else
            {
                this.txtRedirect.Value = string.Empty;
                ShowMessage("Se encontraron los siguientes errores:\n" + sError, MessageType.Error);
            }

        }

        protected void BtnCancelar_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("BuscarPruebas.aspx");
        }
        #endregion

        #region Data to UserInterface

        private void DataToUserInterface()
        {
            APrueba prueba = null;
            prueba = this.catalogoPruebaCtrl.RetrieveSimple(dctx, this.LastObject, true);

            if (prueba == null)
                throw new Exception("No se pudo recuperar la prueba!!!");

            this.LastObject = prueba;

            this.txtClave.Text = prueba.Clave;
            this.txtNombre.Text = prueba.Nombre;
            this.txtInstrucciones.Text = prueba.Instrucciones;
            this.ddlModelo.SelectedValue = ((int)prueba.Modelo.ModeloID).ToString();
            this.chkPruebaPremium.Checked = (bool)prueba.EsPremium;
            this.DDLTipoPruebaPresentacion.SelectedValue = ((int)prueba.TipoPruebaPresentacion).ToString();

            this.MostrarControlesDeModelo();

            //Inhabilitar todos los campos
            this.txtClave.Enabled = false;
            this.txtNombre.Enabled = false;
            this.txtInstrucciones.Enabled = false;
            this.ddlModelo.Enabled = false;

            if (this.TieneBanco(prueba))
            {
                this.BtnGuardar.Enabled = true;
                this.lblMsgError.Text = string.Empty;
            }
            else
            {
                this.BtnGuardar.Enabled = false;
                this.lblMsgError.Text = "La prueba no está configurada correctamente o no tiene reactivos.\n ¡Imposible liberar!";
            }
        }

        private bool TieneBanco(APrueba prueba)
        {
            bool lRespuesta = true;
                ABancoReactivo banco = this.catalogoPruebaCtrl.RetrieveBancoReactivosPrueba(dctx, prueba);
                if (banco == null)
                    lRespuesta = false;
                else
                    if (banco.ListaReactivosBanco != null)
                        lRespuesta = banco.ListaReactivosBanco.Count > 0;

            return lRespuesta;
        }

        private void LoadModelos()
        {
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("Activo", "true");

            DataSet dsModelos = modeloCtrl.Retrieve(dctx, null, parametros);
            ddlModelo.DataSource = dsModelos;
            ddlModelo.DataValueField = "ModeloID";
            ddlModelo.DataTextField = "Nombre";
            ddlModelo.DataBind();
            ddlModelo.Items.Insert(0, new ListItem("SELECCIONE...", ""));
        }
        #endregion

        #region UserInterface to Data
        private APrueba UserInterfaceToData(AModelo modelo)
        {
            APrueba prueba = this.CreateObjectPrueba(modelo);

            if (!string.IsNullOrEmpty(this.txtClave.Text.Trim()))
                prueba.Clave = this.txtClave.Text.Trim();

            if (!string.IsNullOrEmpty(this.txtNombre.Text.Trim()))
                prueba.Nombre = this.txtNombre.Text.Trim();

            if (!string.IsNullOrEmpty(this.txtInstrucciones.Text.Trim()))
                prueba.Instrucciones = this.txtInstrucciones.Text.Trim();
                      
            prueba.EsPremium = chkPruebaPremium.Checked;

            return prueba;
        }

        private AModelo GetModeloFromUI()
        {
            AModelo modelo = null;

            int modeloId = 0;
            string valorModelo = ddlModelo.SelectedValue;

            if (int.TryParse(valorModelo, out modeloId))
            {
                ModeloCtrl modeloCtrl = new ModeloCtrl();
                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros.Add("Activo", "true");
                DataSet dsModelo = modeloCtrl.Retrieve(dctx, modeloId, parametros);
                if (dsModelo.Tables[0].Rows.Count > 0)
                    modelo = modeloCtrl.LastDataRowToModelo(dsModelo);
            }
            return modelo;
        }
        #endregion

        #region Métodos Auxiliares
        private string ValidateData(AModelo modelo)
        {
            string sError = string.Empty;

            if (modelo == null)
                sError += "Imposible actualizar la prueba sin modelo";

            string clave = txtClave.Text.Trim();
            if (clave.Length > 30 || clave.Length < 1)
                sError += "El campo clave no debe estar vacío y no debe ser mayor a 30 caracteres\n";

            string nombre = txtNombre.Text.Trim();
            if (nombre.Length > 100 || nombre.Length < 1)
                sError += "El campo nombre no debe estar vacío y no debe ser mayor a 100 caracteres\n";

            string instrucciones = txtInstrucciones.Text.Trim();
            if (instrucciones.Length > 1000 || instrucciones.Length < 1)
                sError += "El campo instrucciones no debe estar vacío y no debe ser mayor a 1000 caracteres\n";

            return sError;
        }

        private APrueba CreateObjectPrueba(AModelo modeloPrueba)
        {
            APrueba pruebaReturn = null;
           if (modeloPrueba is ModeloDinamico)
            {
                PruebaDinamica pruebaDinamica = new PruebaDinamica();
                pruebaReturn = pruebaDinamica;
            }

            if (pruebaReturn != null)
                pruebaReturn.Modelo = modeloPrueba;

            return pruebaReturn;
        }

        private void MostrarControlesDeModelo()
        {
            AModelo modelo = this.GetModeloFromUI();
            if (modelo != null)
            {               
                if (modelo is ModeloDinamico)
                    this.lblMetodoCalificacion.Text = (modelo as ModeloDinamico).NombreMetodoCalificacion;
                else
                    this.lblMetodoCalificacion.Text = modelo.Nombre;
            }
            else
                this.lblMetodoCalificacion.Text = "SELECCIONE UN MODELO";
        }
        #endregion

        #region AUTORIZACIÓN DE LA PÁGINA
        protected override void AuthorizeUser()
        {
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCATPRUEBAS) != null;
            bool edicion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCATPRUEBA) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!edicion)
                redirector.GoToHomePage(true);
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