using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.Operaciones.Implements;
using POV.Core.Operaciones.Interfaces;
using POV.Localizacion.Service;
using POV.Localizacion.BO;
using POV.Comun.Service;
using POV.Comun.BO;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;
using POV.Licencias.BO;
using POV.Licencias.Service;

namespace POV.Web.PortalOperaciones.CiclosEscolares
{
    public partial class EditarCicloEscolar : PageBase
    {
        private CicloEscolarCtrl cicloEscolarCtrl;
        private CicloContratoCtrl cicloContratoCtrl;
        private UbicacionCtrl ubicacionCtrl;
        private PaisCtrl paisCtrl;
        private EstadoCtrl estadoCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        #region *** propiedades de clase ***
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

        public EditarCicloEscolar()
        {
            cicloEscolarCtrl = new CicloEscolarCtrl();
            ubicacionCtrl = new UbicacionCtrl();
            paisCtrl = new PaisCtrl();
            estadoCtrl = new EstadoCtrl();
            cicloContratoCtrl = new CicloContratoCtrl();
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
                        if (LastObject != null && SS_LastObject != null && LastObject.CicloContratoID != null && SS_LastObject.ContratoID != null)
                        {
                            LoadCicloEscolar(LastObject);
                        }
                        else
                        {
                            txtRedirect.Value = "../Contratos/BuscarContrato.aspx";
                            ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                            return;
                        }
                        
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

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DoUpdate();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void CbPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CbPais.SelectedIndex != -1 && CbPais.SelectedItem.Value.Length >= 0)
                {
                    Ubicacion ubicacion = new Ubicacion();
                    ubicacion.Pais = new Pais { PaisID = int.Parse(CbPais.SelectedItem.Value) };

                    LoadPaises(ubicacion);
                    LoadEstados(ubicacion);
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        #endregion

        #region *** validaciones ***
        private void CicloEscolarValidateData()
        {
            string sError = string.Empty;
            int outind;
            DateTime outdate;

            //Validar campos requeridos
            if (txtNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre";
            if (txtDescripcion.Text.Trim().Length <= 0)
                sError += " ,Descripción";
            if (txtInicioCiclo.Text.Trim().Length <= 0)
                sError += " ,Fecha Inicio";
            if (txtFinCiclo.Text.Trim().Length <= 0)
                sError += " ,Fecha Fin";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son requeridos:{0}", sError));
            }


            if (txtInicioCiclo.Text.Trim().Length > 0)
                if (!DateTime.TryParse(txtInicioCiclo.Text.Trim(), out outdate))
                {
                    sError += " ,Fecha Inicio";
                }
            if (txtFinCiclo.Text.Trim().Length > 0)
                if (!DateTime.TryParse(txtFinCiclo.Text.Trim(), out outdate))
                {
                    sError += " ,Fecha Fin";
                }
            //Validar formato
            if (sError.Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros tienen un formato no valido: {0}", sError));
            }

            //Validar contenido
            if (txtNombre.Text.Trim().Length > 100)
                sError += " ,Nombre";
            if (sError.Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos: {0}", sError));
            }

            DateTime dinicio = DateTime.ParseExact(txtInicioCiclo.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dfin = DateTime.ParseExact(txtFinCiclo.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (dfin <= dinicio)
                sError += " ,Fecha Fin";
            if (sError.Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("La {0} no puede ser menor o igual a la Fecha Inicio proporcionada ", sError));
            }

            //Validar ubicación
            sError = string.Empty;
            if (CbPais.SelectedIndex != -1 && CbPais.SelectedItem.Value.Length > 0)
            {
                if (CbEstado.SelectedIndex == -1 || CbEstado.SelectedItem.Value.Length <= 0)
                    sError += " ,Estado";
            }
            if (sError.Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Debe seleccionar un estado del país {0} para continuar", CbPais.SelectedItem.Text));
            }


        }
        #endregion

        #region *** Data to UserInterface ***
        private void LoadCicloEscolar(CicloContrato cicloContrato)
        {
            cicloContrato = cicloContratoCtrl.RetrieveComplete(dctx, SS_LastObject, cicloContrato);
            ChkEstaLiberado.Checked = cicloContrato.EstaLiberado.Value;
            //Consultar ubicación
            LoadPaises(cicloContrato.CicloEscolar.UbicacionID);
            LoadEstados(cicloContrato.CicloEscolar.UbicacionID);
            DataToUserInterface(cicloContrato.CicloEscolar);

        }

        private void DataToUserInterface(CicloEscolar ciclo)
        {
            if (ciclo == null)
                return;
            txtCicloEscolarID.Text = ciclo.CicloEscolarID.ToString();
            txtNombre.Text = ciclo.Titulo;
            txtDescripcion.Text = ciclo.Descripcion;
            txtInicioCiclo.Text = string.Format("{0:dd/MM/yyyy}", ciclo.InicioCiclo);
            txtFinCiclo.Text = string.Format("{0:dd/MM/yyyy}", ciclo.FinCiclo);
            CbPais.SelectedValue = ciclo.UbicacionID.Pais.PaisID.ToString();
            CbEstado.SelectedValue = ciclo.UbicacionID.Estado.EstadoID.ToString();


        }

        private void LoadPaises(Ubicacion ubicacion)
        {
            if (ubicacion == null || ubicacion.Pais == null)
                return;
            DataSet ds = paisCtrl.Retrieve(ConnectionHlp.Default.Connection, ubicacion.Pais);
            CbPais.DataSource = ds;
            CbPais.DataTextField = "Nombre";
            CbPais.DataValueField = "PaisID";
            CbPais.DataBind();
            CbPais.Items.Insert(0, new ListItem("", ""));
        }

        private void LoadEstados(Ubicacion ubicacion)
        {
            if (ubicacion == null || ubicacion.Pais == null)
                return;
            DataSet ds = estadoCtrl.Retrieve(ConnectionHlp.Default.Connection, new Estado { Pais = ubicacion.Pais });
            CbEstado.DataSource = ds;
            CbEstado.DataTextField = "Nombre";
            CbEstado.DataValueField = "EstadoID";
            CbEstado.DataBind();
            CbEstado.Items.Insert(0, new ListItem("", ""));
        }
        #endregion

        #region *** UserInterface to Data ***
        private CicloContrato UserInterfaceToData()
        {
            CicloContrato cicloContrato = cicloContratoCtrl.RetrieveComplete(dctx, SS_LastObject, LastObject);

            cicloContrato.EstaLiberado = ChkEstaLiberado.Checked;
            cicloContrato.CicloEscolar.Titulo = !string.IsNullOrEmpty(txtNombre.Text.Trim()) ? txtNombre.Text.Trim() : string.Empty;
            cicloContrato.CicloEscolar.InicioCiclo = !string.IsNullOrEmpty(txtInicioCiclo.Text.Trim()) ? DateTime.ParseExact(txtInicioCiclo.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            cicloContrato.CicloEscolar.FinCiclo = !string.IsNullOrEmpty(txtFinCiclo.Text.Trim()) ? DateTime.ParseExact(txtFinCiclo.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            cicloContrato.CicloEscolar.UbicacionID = new Ubicacion { Pais = new Pais(), Estado = new Estado() };
            cicloContrato.CicloEscolar.UbicacionID.Pais.PaisID = CbPais.SelectedIndex != -1 && !string.IsNullOrEmpty(CbPais.SelectedItem.Value) ? int.Parse(CbPais.SelectedItem.Value) : (int?)null;
            cicloContrato.CicloEscolar.UbicacionID.Estado.EstadoID = CbEstado.SelectedIndex != -1 && !string.IsNullOrEmpty(CbEstado.SelectedItem.Value) ? int.Parse(CbEstado.SelectedItem.Value) : (int?)null;
            cicloContrato.CicloEscolar.Descripcion = !string.IsNullOrEmpty(txtDescripcion.Text.Trim()) ? txtDescripcion.Text.Trim() : string.Empty;
            return cicloContrato;

        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoUpdate()
        {
            object myFirm = new object();

            try
            {
                try
                {
                    CicloEscolarValidateData();
                }
                catch (Exception ex)
                {

                    this.ShowMessage(ex.Message, MessageType.Error);
                    return;
                }

                dctx.OpenConnection(myFirm);
                dctx.BeginTransaction(myFirm);
                CicloContrato anterior = cicloContratoCtrl.RetrieveComplete(dctx, SS_LastObject, LastObject);

                CicloContrato cicloContrato = UserInterfaceToData();

                cicloContratoCtrl.UpdateComplete(dctx, SS_LastObject, cicloContrato, anterior);

                dctx.CommitTransaction(myFirm);


                this.LastObject = null;
                redirector.GoToConsultarCiclosEscolares(false);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }
        #endregion

        #region AUTORIZACION DE LA PAGINA

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