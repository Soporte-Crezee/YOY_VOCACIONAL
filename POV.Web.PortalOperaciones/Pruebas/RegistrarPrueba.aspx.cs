using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;
using Framework.Base.DataAccess;
using POV.Modelo.Service;
using POV.Prueba.Diagnostico.Service;
using POV.Web.PortalOperaciones.Helper;
using System.Data;
using POV.Modelo.BO;
using POV.Prueba.BO;
using Framework.Base.Exceptions;
using POV.Modelo.Estandarizado.BO;

using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Web.PortalOperaciones.Pruebas
{
    public partial class RegistrarPrueba : PageBase
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        private CatalogoPruebaCtrl catalogoPruebaCtrl;
        private ModeloCtrl modeloCtrl;

        public RegistrarPrueba()
        {
            this.catalogoPruebaCtrl = new CatalogoPruebaCtrl();
            this.modeloCtrl = new ModeloCtrl();
        }

        #region Eventos de Página
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadModelos();
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
                    APrueba pruebaInsert = this.UserInterfaceToData(modelo);

                    this.catalogoPruebaCtrl.Insert(dctx, pruebaInsert);
                }
                catch (Exception ex)
                {
                    sError += ex.Message;
                }
            }

            if (string.IsNullOrEmpty(sError))
            {
                txtRedirect.Value = "BuscarPruebas.aspx";
                ShowMessage("Registro exitoso.", MessageType.Information);
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



            List<ETipoPrueba> tiposPrueba = new List<ETipoPrueba>();
            foreach (ETipoPrueba animal in Enum.GetValues(typeof(ETipoPrueba)))
            {
                tiposPrueba.Add(animal);
            }

            #region TipoPruebaPresentacion
            if (DDLTipoPruebaPresentacion.SelectedValue == "4")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.HabitosEstudio;

            if (DDLTipoPruebaPresentacion.SelectedValue == "5")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Dominos;

            if (DDLTipoPruebaPresentacion.SelectedValue == "6")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.TermanMerrill;

            if (DDLTipoPruebaPresentacion.SelectedValue == "7")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Kuder;

            if (DDLTipoPruebaPresentacion.SelectedValue == "8")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Allport;

            if (DDLTipoPruebaPresentacion.SelectedValue == "9")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.FrasesIncompletasSacks;


            if (DDLTipoPruebaPresentacion.SelectedValue == "10")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Cleaver;


            if (DDLTipoPruebaPresentacion.SelectedValue == "11")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Chaside;


            if (DDLTipoPruebaPresentacion.SelectedValue == "12")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Rotter;


            if (DDLTipoPruebaPresentacion.SelectedValue == "13")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.FrasesIncompletasVocacionales;


            if (DDLTipoPruebaPresentacion.SelectedValue == "14")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Zavic;


            if (DDLTipoPruebaPresentacion.SelectedValue == "15")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Raven;


            if (DDLTipoPruebaPresentacion.SelectedValue == "16")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Kostick;

            if (DDLTipoPruebaPresentacion.SelectedValue == "17")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Bullying;

            if (DDLTipoPruebaPresentacion.SelectedValue == "18")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Socioeconomico;

            if (DDLTipoPruebaPresentacion.SelectedValue == "19")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Autoconcepto;

            if (DDLTipoPruebaPresentacion.SelectedValue == "20")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Actitudes;

            if (DDLTipoPruebaPresentacion.SelectedValue == "21")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Empatia;

            if (DDLTipoPruebaPresentacion.SelectedValue == "22")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Humor;

            if (DDLTipoPruebaPresentacion.SelectedValue == "23")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Victimizacion;

            if (DDLTipoPruebaPresentacion.SelectedValue == "24")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Ciberbullying;

            if (DDLTipoPruebaPresentacion.SelectedValue == "25")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Victimizacion;

            if (DDLTipoPruebaPresentacion.SelectedValue == "26")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Comunicacion;

            if (DDLTipoPruebaPresentacion.SelectedValue == "27")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.ImagenCorporal;

            if (DDLTipoPruebaPresentacion.SelectedValue == "28")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Ansiedad;

            if (DDLTipoPruebaPresentacion.SelectedValue == "29")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Depresion;

            if (DDLTipoPruebaPresentacion.SelectedValue == "30")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.EstilosdeAprendizaje;

            if (DDLTipoPruebaPresentacion.SelectedValue == "31")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.InteligengiasMultiples;

            if (DDLTipoPruebaPresentacion.SelectedValue == "32")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.InventarioDeIntereses;
           
            if (DDLTipoPruebaPresentacion.SelectedValue == "33")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.InventarioHerreraMontes;

            if (DDLTipoPruebaPresentacion.SelectedValue == "34")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.SucesosDeVida;

            if (DDLTipoPruebaPresentacion.SelectedValue == "35")
                prueba.TipoPruebaPresentacion = ETipoPruebaPresentacion.Luscher;

            #endregion

            if (!string.IsNullOrEmpty(this.txtClave.Text.Trim()))
                prueba.Clave = this.txtClave.Text.Trim();

            if (!string.IsNullOrEmpty(this.txtNombre.Text.Trim()))
                prueba.Nombre = this.txtNombre.Text.Trim();

            if (!string.IsNullOrEmpty(this.txtInstrucciones.Text.Trim()))
                prueba.Instrucciones = this.txtInstrucciones.Text.Trim();

            Decimal nDecimal = 0;

            prueba.EsPremium = this.chkPruebaPremium.Checked;

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
                sError += "Imposible crear la prueba sin modelo";

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

        #region AUTORIZACION DE LA PÁGINA
        protected override void AuthorizeUser()
        {
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCATPRUEBAS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARCATPRUEBA) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!creacion)
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