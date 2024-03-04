using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Logger.Service;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.Contratos
{
    public partial class EditarContrato : PageBase
    {
        private readonly EstadoCtrl estadoCtrl;
        private readonly PaisCtrl paisCtrl;

        #region *** propiedades de clase ***
        private const string sFechaRegex = @"(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])([13579][26]00))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))";
        private readonly StringBuilder sError = new StringBuilder();

        private Contrato SS_LastObject
        {
            set { Session["lastContrato"] = value; }
            get { return Session["lastContrato"] != null ? (Contrato)Session["lastContrato"] : null; }
        }
        #endregion

        public EditarContrato()
        {
            paisCtrl = new PaisCtrl();
            estadoCtrl = new EstadoCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (SS_LastObject == null)
                {
                    txtRedirect.Value = "BuscarContrato.aspx";
                    ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                    return;
                }

                FillBack();
                LoadPaises();

                this.DoOpen();
            }
        }

        protected void cbPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pais pais = GetPaisFromUI();
            LoadEstados(pais);
        }

        protected void cbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pais pais = GetPaisFromUI();
            Estado estado = GetEstadoFromUI();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DoUpdate();
        }
        #endregion

        #region *** validaciones ***
        private bool ValidarTodo()
        {
            bool valido = true;

            if (!PaisRequeridoEsValido())
                valido = false;

            if (!EstadoRequeridoEsValido())
                valido = false;

            if (!FechaContratoRequeridoEsValido() || !FechaContratoFormatoEsValido())
                valido = false;

            if (!ClaveRequeridoEsValido())
                valido = false;

            if (!FechaInicioRequeridoEsValido() || !FechaInicioFormatoEsValido())
                valido = false;

            if (!FechaFinalizacionRequeridoEsValido() || !FechaFinalizacionFormatoEsValido())
                valido = false;

            if (!NombreCteRequeridoEsValido())
                valido = false;

            if (!DireccionCteRequeridoEsValido())
                valido = false;

            if (!RepresentanteRequeridoEsValido())
                valido = false;

            if (!TelefonoRequeridoEsValido())
                valido = false;

            string sMensaje = sError.ToString();
            if (sMensaje.Trim().Length > 0)
                this.ShowMessage(sMensaje, MessageType.Information);

            return valido;
        }

        public bool PaisRequeridoEsValido()
        {
            bool valido = true;

            if (!(valido = this.cbPais.SelectedIndex > 0))
                sError.AppendLine("Seleccione un País");

            return valido;
        }

        public bool EstadoRequeridoEsValido()
        {
            bool valido = true;

            if (!(valido = this.cbEstado.SelectedIndex > 0))
                sError.AppendLine("Seleccione un Estado");

            return valido;
        }

        public bool FechaContratoRequeridoEsValido()
        {
            bool valido = true;

            if (!(valido = (this.txtFechaContrato.Text.Trim().Length > 0)))
                sError.AppendLine("Fecha de Contrato es Requerido.");

            return valido;
        }

        public bool FechaContratoFormatoEsValido()
        {
            bool valido = true;

            if (!(valido = (new Regex(sFechaRegex).IsMatch(this.txtFechaContrato.Text.Trim()))))
                sError.AppendLine("Fecha de Contrato requiere el formato dd/mm/aaaa.");

            return valido;
        }

        public bool ClaveRequeridoEsValido()
        {
            bool valido = true;

            if (!(valido = (this.txtClave.Text.Trim().Length > 0)))
                sError.AppendLine("Clave es Requerido.");

            return valido;
        }

        public bool FechaInicioRequeridoEsValido()
        {
            bool valido = true;

            if (!(valido = (this.txtFechaInicio.Text.Trim().Length > 0)))
                sError.AppendLine("Fecha de Inicio es Requerido.");

            return valido;
        }

        public bool FechaInicioFormatoEsValido()
        {
            bool valido = true;

            if (!(valido = (new Regex(sFechaRegex).IsMatch(this.txtFechaInicio.Text.Trim()))))
                sError.AppendLine("Fecha de Inicio requiere el formato dd/mm/yyyy.");

            return valido;
        }

        public bool FechaFinalizacionRequeridoEsValido()
        {
            bool valido = true;

            if (!(valido = (this.txtFechaFinalizacion.Text.Trim().Length > 0)))
                sError.AppendLine("Fecha de Finalizacion es Requerido.");

            return valido;
        }

        public bool FechaFinalizacionFormatoEsValido()
        {
            bool valido = true;

            if (!(valido = (new Regex(sFechaRegex).IsMatch(this.txtFechaFinalizacion.Text.Trim()))))
                sError.AppendLine("Fecha de Finalizacion requiere el formato dd/mm/yyyy.");

            return valido;
        }

        public bool NombreCteRequeridoEsValido()
        {
            bool valido = true;

            if (!(valido = (this.txtNombreCte.Text.Trim().Length > 0)))
                sError.AppendLine("Nombre de Cliente es Requerido.");

            return valido;
        }

        public bool DireccionCteRequeridoEsValido()
        {
            bool valido = true;

            if (!(valido = (this.txtDireccionCte.Text.Trim().Length > 0)))
                sError.AppendLine("Dirección de Cliente es Requerido.");

            return valido;
        }

        public bool RepresentanteRequeridoEsValido()
        {
            bool valido = true;

            if (!(valido = (this.txtRepresentante.Text.Trim().Length > 0)))
                sError.AppendLine("Representante de Cliente es Requerido.");

            return valido;
        }

        public bool TelefonoRequeridoEsValido()
        {
            bool valido = true;

            if (!(valido = (this.txtTelefono.Text.Trim().Length > 0)))
                sError.AppendLine("Telefono de Cliente es Requerido.");

            return valido;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void LoadPaises()
        {
            cbPais.DataSource = null;
            cbPais.Items.Clear();

            cbPais.Items.Add(new ListItem("Seleccionar", "0"));
            DataSet ds = paisCtrl.Retrieve(ConnectionHlp.Default.Connection, new Pais());

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbPais.DataSource = ds;
                cbPais.DataTextField = "Nombre";
                cbPais.DataValueField = "PaisID";
            }

            cbPais.DataBind();
            cbPais.SelectedValue = "0";
        }
        private void LoadEstados(Pais pais)
        {
            cbEstado.DataSource = null;
            cbEstado.Items.Clear();

            cbEstado.Items.Add(new ListItem("Seleccionar", "0"));
            DataSet ds = estadoCtrl.Retrieve(ConnectionHlp.Default.Connection, new Estado { Pais = pais });

            if (pais.PaisID != null)
            {
                cbEstado.DataSource = ds;
                cbEstado.DataTextField = "Nombre";
                cbEstado.DataValueField = "EstadoID";
            }

            cbEstado.DataBind();
            cbEstado.SelectedValue = "0";
        }

        private void DataToUSerInterface(Contrato contrato)
        {
            if (contrato.ContratoID == null)
                this.txtContratoID.Text = string.Empty;
            else
                this.txtContratoID.Text = contrato.ContratoID.ToString();
            if (contrato.Ubicacion != null)
            {
                if (contrato.Ubicacion.Pais != null && contrato.Ubicacion.Pais.PaisID != null)
                    this.cbPais.SelectedValue = contrato.Ubicacion.Pais.PaisID.ToString();
                else
                    this.cbPais.SelectedValue = "0";

                if (contrato.Ubicacion.Estado != null && contrato.Ubicacion.Estado.EstadoID != null)
                {
                    this.LoadEstados(this.GetPaisFromUI());
                    this.cbEstado.SelectedValue = contrato.Ubicacion.Estado.EstadoID.ToString();
                }
                else
                    this.cbEstado.SelectedValue = "0";
            }
            if (contrato.FechaContrato == null)
                this.txtFechaContrato.Text = string.Empty;
            else
                this.txtFechaContrato.Text = contrato.FechaContrato.Value.ToString("dd/MM/yyyy");
            if (contrato.Clave == null)
                this.txtClave.Text = string.Empty;
            else
                this.txtClave.Text = contrato.Clave;
            if (contrato.InicioContrato == null)
                this.txtFechaInicio.Text = string.Empty;
            else
                this.txtFechaInicio.Text = contrato.InicioContrato.Value.ToString("dd/MM/yyyy");
            if (contrato.FinContrato == null)
                this.txtFechaFinalizacion.Text = string.Empty;
            else
                this.txtFechaFinalizacion.Text = contrato.FinContrato.Value.ToString("dd/MM/yyyy");
            if (contrato.NumeroLicencias == null)
                this.txtNumeroLicencias.Text = string.Empty;
            else
                this.txtNumeroLicencias.Text = contrato.NumeroLicencias.ToString();
            if (contrato.LicenciasLimitadas == null)
                this.hdnIlimitadosNulo.Value = "1";
            else
            {
                this.chkIlimitadas.Checked = !contrato.LicenciasLimitadas.Value;
                this.hdnIlimitadosNulo.Value = "0";
            }
            if (contrato.Cliente != null)
            {
                if (contrato.Cliente.Nombre == null)
                    this.txtNombreCte.Text = string.Empty;
                else
                    this.txtNombreCte.Text = contrato.Cliente.Nombre;

                if (contrato.Cliente.Domicilio == null)
                    this.txtDireccionCte.Text = string.Empty;
                else
                    this.txtDireccionCte.Text = contrato.Cliente.Domicilio;
                if (contrato.Cliente.Representante == null)
                    this.txtRepresentante.Text = string.Empty;
                else
                    this.txtRepresentante.Text = contrato.Cliente.Representante;
                if (contrato.Cliente.Telefono == null)
                    this.txtTelefono.Text = string.Empty;
                else
                    this.txtTelefono.Text = contrato.Cliente.Telefono;
            }
        }
        #endregion

        #region *** UserInterface to Data ***
        public Pais GetPaisFromUI()
        {
            Pais pais = new Pais();
            int paisId = 0;
            int.TryParse(cbPais.SelectedValue, out paisId);
            if (paisId > 0)
            {
                pais.PaisID = paisId;
                pais.Nombre = cbPais.SelectedItem.Text;
            }
            return pais;
        }
        public Estado GetEstadoFromUI()
        {
            Estado estado = new Estado();
            int estadoId = 0;
            int.TryParse(cbEstado.SelectedValue, out estadoId);
            if (estadoId > 0)
            {
                estado.EstadoID = estadoId;
                estado.Nombre = cbEstado.SelectedItem.Text;
                estado.Pais = this.GetPaisFromUI();
            }
            return estado;
        }
        private Contrato UserInterfaceToData()
        {
            Contrato contrato = new Contrato();
            contrato.Cliente = new Cliente();
            contrato.Ubicacion = new Ubicacion();
            contrato.Ubicacion.Pais = new Pais();
            contrato.Ubicacion.Estado = new Estado();
            contrato.UsuarioRegistro = new Usuario();

            //ContratoID
            contrato.ContratoID = int.Parse(this.txtContratoID.Text);
            //Pais
            contrato.Ubicacion.Pais.PaisID = int.Parse(this.cbPais.SelectedValue);
            //Estado
            contrato.Ubicacion.Estado.EstadoID = int.Parse(this.cbEstado.SelectedValue);
            //Fecha de Contrato
            contrato.FechaContrato = DateTime.ParseExact(this.txtFechaContrato.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //Clave de contrato
            contrato.Clave = this.txtClave.Text.Trim();
            //Fecha de Inicio de Contrato
            contrato.InicioContrato = DateTime.ParseExact(this.txtFechaInicio.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //Fecha de Fin de Contrato
            contrato.FinContrato = DateTime.ParseExact(this.txtFechaFinalizacion.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //Licenciamiento ilimitado
            contrato.LicenciasLimitadas = !this.chkIlimitadas.Checked;
            //Numero de licencias
            contrato.NumeroLicencias = (this.txtNumeroLicencias.Text.Trim().Length == 0) ? 0 : int.Parse(this.txtNumeroLicencias.Text);
            //Nombre del Cliente
            contrato.Cliente.Nombre = this.txtNombreCte.Text.Trim();
            //Direccion del Cliente
            contrato.Cliente.Domicilio = this.txtDireccionCte.Text.Trim();
            //Representante del Cliente
            contrato.Cliente.Representante = this.txtRepresentante.Text.Trim();
            //Telefono del Cliente
            contrato.Cliente.Telefono = this.txtTelefono.Text.Trim();
            //Fecha de registro
            contrato.FechaRegistro = DateTime.Now;
            //Usuario registro
            contrato.UsuarioRegistro.UsuarioID = 10; //TODO: Aplicar el identificador del usuario de la sesion actual

            return contrato;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoOpen()
        {
            ContratoCtrl ctrl = new ContratoCtrl();
            this.SS_LastObject = ctrl.LastDataRowToContrato(ctrl.Retrieve(ConnectionHlp.Default.Connection, SS_LastObject));

            UbicacionCtrl ubicaCtrl = new UbicacionCtrl();
            this.SS_LastObject.Ubicacion = ubicaCtrl.LastDataRowToUbicacion(ubicaCtrl.Retrieve(ConnectionHlp.Default.Connection, this.SS_LastObject.Ubicacion));

            this.DataToUSerInterface(SS_LastObject);
        }
        private void DoUpdate()
        {
            if (!this.ValidarTodo())
                return;

            try
            {
                Contrato contrato = UserInterfaceToData();

                //si el contrato tiene licencias limitadas, se valida que no hayan inconsistencias con el numero de licencias
                if (contrato.LicenciasLimitadas.Value)
                {
                    int disponibles = 0;
                    int asignadas = 0;
                    if (SS_LastObject.LicenciasLimitadas.Value && contrato.NumeroLicencias < SS_LastObject.NumeroLicencias)
                    {
                        new ContratoCtrl().TieneLicenciasDisponibles(ConnectionHlp.Default.Connection, SS_LastObject, out disponibles);

                        asignadas = SS_LastObject.NumeroLicencias.Value - disponibles;
                    }
                    else
                    {
                        asignadas = new ContratoCtrl().RetrieveNumeroLicenciasAsignadas(ConnectionHlp.Default.Connection, SS_LastObject);
                    }

                    if (contrato.NumeroLicencias < asignadas)
                        throw new Exception("El número de licencias no deben ser menores a " + asignadas);
                }


                contrato.FechaRegistro = this.SS_LastObject.FechaRegistro;
                contrato.UsuarioRegistro = this.SS_LastObject.UsuarioRegistro;
                contrato.Estatus = this.SS_LastObject.Estatus;
                new ContratoCtrl().Update(ConnectionHlp.Default.Connection, contrato, this.SS_LastObject);

                txtRedirect.Value = "BuscarContrato.aspx";
                ShowMessage("El contrato se ha actualizado con éxito", MessageType.Information);
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Contratos/BuscarContrato.aspx";
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
    }
}