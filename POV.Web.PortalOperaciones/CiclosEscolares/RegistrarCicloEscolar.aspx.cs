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
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Core.Operaciones.Implements;
using POV.Core.Operaciones.Interfaces;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;
using POV.Licencias.BO;
using POV.Licencias.Service;

namespace POV.Web.PortalOperaciones.CiclosEscolares
{
    public partial class RegistrarCicloEscolar : PageBase
    {
        
        private CicloEscolarCtrl cicloEscolarCtrl;
        private CicloContratoCtrl cicloContratoCtrl;
        private UbicacionCtrl ubicacionCtrl;
        private PaisCtrl paisCtrl;
        private EstadoCtrl estadoCtrl;

        #region *** propiedades de clase ***
        private Contrato SS_LastObject
        {
            set { Session["lastContrato"] = value; }
            get { return Session["lastContrato"] != null ? (Contrato)Session["lastContrato"] : null; }
        }
        #endregion

        public RegistrarCicloEscolar()
        {
            cicloEscolarCtrl = new CicloEscolarCtrl();
            cicloContratoCtrl = new CicloContratoCtrl();
            ubicacionCtrl = new UbicacionCtrl();
            paisCtrl = new PaisCtrl();
            estadoCtrl = new EstadoCtrl();
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
                        if (SS_LastObject == null)
                        {
                            txtRedirect.Value = "../Contratos/BuscarContrato.aspx";
                            ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                            return;
                        }
                        LoadPaises(new Ubicacion { Pais = new Pais() });
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

                DoInsert();

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
                if (CbPais.SelectedIndex != -1 && CbPais.SelectedItem.Value.Length > 0)
                {
                    Ubicacion ubicacion = new Ubicacion();
                    ubicacion.Pais = new Pais { PaisID = int.Parse(CbPais.SelectedItem.Value) };
                    LoadEstados(ubicacion);
                }
                else
                {
                    CbEstado.Items.Clear();
                }
            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        #endregion

        #region *** validaciones ***
        private void CicloEscolarValidateNoRepetido()
        {
            //CicloEscolar ciclo = UserInterfaceToData();
            //ciclo.Activo = true;
            //DataSet ds = cicloEscolarCtrl.Retrieve(ConnectionHlp.Default.Connection, ciclo);
            //if (ds.Tables[0].Rows.Count > 0)
            //    throw new Exception("Un ciclo escolar similar ya se encuentra registrado, por favor verifique");
        }
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
                if (!DateTime.TryParseExact(txtInicioCiclo.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out outdate))
                {
                    sError += " ,Fecha Inicio";
                }
            if (txtFinCiclo.Text.Trim().Length > 0)
                if (!DateTime.TryParseExact(txtFinCiclo.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out outdate))
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
            CicloContrato cicloContrato = new CicloContrato();
            cicloContrato.FechaRegistro = DateTime.Now;
            cicloContrato.Activo = true;

            CicloEscolar cicloEscolar = new CicloEscolar();

            cicloEscolar.Titulo = !string.IsNullOrEmpty(txtNombre.Text.Trim()) ? txtNombre.Text.Trim() : string.Empty;
            cicloEscolar.InicioCiclo = !string.IsNullOrEmpty(txtInicioCiclo.Text.Trim()) ? DateTime.ParseExact(txtInicioCiclo.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            cicloEscolar.FinCiclo = !string.IsNullOrEmpty(txtFinCiclo.Text.Trim()) ? DateTime.ParseExact(txtFinCiclo.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
            cicloEscolar.UbicacionID = new Ubicacion { Pais = new Pais(), Estado = new Estado() };
            cicloEscolar.UbicacionID.Pais.PaisID = CbPais.SelectedIndex != -1 && !string.IsNullOrEmpty(CbPais.SelectedItem.Value) ? int.Parse(CbPais.SelectedItem.Value) : (int?)null;
            cicloEscolar.UbicacionID.Estado.EstadoID = CbEstado.SelectedIndex != -1 && !string.IsNullOrEmpty(CbEstado.SelectedItem.Value) ? int.Parse(CbEstado.SelectedItem.Value) : (int?)null;
            cicloEscolar.Descripcion = !string.IsNullOrEmpty(txtDescripcion.Text.Trim()) ? txtDescripcion.Text.Trim() : string.Empty;
            cicloEscolar.Activo = true;
            cicloContrato.EstaLiberado = ChkEstaLiberado.Checked;

            cicloContrato.CicloEscolar = cicloEscolar;
            cicloContrato.RecursoContrato = new RecursoContrato
            {
                FechaRegistro = DateTime.Now,
                Activo = true
            };

            return cicloContrato;

        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoInsert()
        {
            object myFirm = new object();

            IDataContext dctx = ConnectionHlp.Default.Connection;



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
                CicloContrato cicloContrato = UserInterfaceToData();
                cicloContratoCtrl.InsertComplete(dctx, SS_LastObject, cicloContrato);
                dctx.CommitTransaction(myFirm);

                txtRedirect.Value = "BuscarCicloEscolar.aspx";
                ShowMessage("Registro exitoso.", MessageType.Information);

                
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