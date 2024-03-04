using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.Localidades
{
    public partial class RegistrarLocalidad : PageBase
    {
        private LocalidadCtrl localidadCtrl;
        private CiudadCtrl ciudadCtrl;
        private EstadoCtrl estadoCtrl;
        private PaisCtrl paisCtrl;

        #region *** propiedades de clase ***

        #endregion

        public RegistrarLocalidad()
        {
            localidadCtrl = new LocalidadCtrl();
            ciudadCtrl= new CiudadCtrl();
            paisCtrl = new PaisCtrl();
            estadoCtrl = new EstadoCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            LoadPaises();
            ddlPais.Enabled = ddlPais.Items.Count > 0;
            ddlEstado.Enabled = ddlEstado.Items.Count > 0;
            ddlCiudad.Enabled = ddlCiudad.Items.Count > 0;
            if (ddlPais.Items.Count == 0)
                ShowMessage("No existen paises en el sistema para asignarle a la localidad", MessageType.Information);
            FillBack();
            if (ddlPais.Items.Count > 0)
            {
                Pais pais = GetPaisFromUI();
                if (pais.PaisID != null)
                {
                    LoadEstados(pais);
                    ddlEstado.Enabled = ddlEstado.Items.Count > 0;
                    ddlCiudad.Enabled = ddlCiudad.Items.Count > 0;

                    if (ddlEstado.Items.Count == 0)
                        ShowMessage("No existen estados en el sistema para asignarle a la localidad", MessageType.Information);
                    FillBack();
                    if (ddlEstado.Items.Count > 0)
                    {
                        Estado estado = GetEstadoFromUI();
                        if (estado.EstadoID != null)
                        {
                            LoadCiudades(estado);
                            ddlCiudad.Enabled = ddlCiudad.Items.Count > 0;
                        }
                    }

                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DoInsert();
        }

        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pais pais = GetPaisFromUI();
            if (pais.PaisID != null)
                LoadEstados(pais);
            ddlEstado.Enabled = ddlEstado.Items.Count > 0;
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            Estado estado = GetEstadoFromUI();
            if (estado.EstadoID > 0)
            {
                LoadCiudades(estado);
                if (ddlCiudad.Items.Count > 0)
                    ddlCiudad.Enabled = true;
            }
            else
            {
                ddlCiudad.Items.Clear();
                ddlCiudad.Enabled = false;
            }

        }

        protected void ddlCiudad_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
        #endregion

        #region *** validaciones ***
        private string validateData()
        {
            string sError = string.Empty;
            Pais pais = GetPaisFromUI();
            Estado estado = GetEstadoFromUI();
            Ciudad ciudad = GetCiudadFromUI();
            if (pais.PaisID == null)
                sError += ", debe seleccionar un pais";
            if (estado.EstadoID == null)
                sError += ", debe seleccionar un estado";
            if (ciudad.CiudadID == null)
                sError += ", debe seleccionar una ciudad";
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                if (txtNombre.Text.Trim().Length > 30)
                    sError = ", el tamaño del texto no debe ser mayor a 30 caracteres)";
            }
            if (sError.Length > 0)
            {
                sError = sError.Substring(2);
                sError = char.ToUpper(sError[0]) + sError.Substring(1);
                return sError;
            }
            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void LoadPaises()
        {
            ddlPais.DataSource = paisCtrl.Retrieve(ConnectionHlp.Default.Connection, new Pais());
            ddlPais.DataTextField = "Nombre";
            ddlPais.DataValueField = "PaisID";
            ddlPais.DataBind();
        }

        private void LoadEstados(Pais pais)
        {
            if (pais.PaisID != null)
            {
                ddlEstado.DataSource = estadoCtrl.Retrieve(ConnectionHlp.Default.Connection, new Estado { Pais = pais });
                ddlEstado.DataTextField = "Nombre";
                ddlEstado.DataValueField = "EstadoID";
                ddlEstado.DataBind();
            }
        }

        private void LoadCiudades(Estado estado)
        {
            if (estado.EstadoID != null)
            {
                ddlCiudad.DataSource = ciudadCtrl.Retrieve(ConnectionHlp.Default.Connection, new Ciudad { Estado = estado });
                ddlCiudad.DataTextField = "Nombre";
                ddlCiudad.DataValueField = "CiudadID";
                ddlCiudad.DataBind();
            }
        }

        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Localidades/BuscarLocalidades.aspx";
        }
        #endregion

        #region *** UserInterface to Data ***
        private Pais GetPaisFromUI()
        {
            Pais pais = new Pais();
            int paisId = 0;
            int.TryParse(ddlPais.SelectedValue, out paisId);
            if (paisId > 0)
            {
                pais.PaisID = paisId;
            }
            return pais;
        }
        private Estado GetEstadoFromUI()
        {
            Estado estado = new Estado();
            int estadoId = 0;
            int.TryParse(ddlEstado.SelectedValue, out estadoId);
            if (estadoId > 0)
            {
                estado.EstadoID = estadoId;
            }
            return estado;
        }
        private Ciudad GetCiudadFromUI()
        {
            Ciudad ciudad = new Ciudad();
            int ciudadId = 0;
            bool result = int.TryParse(ddlCiudad.SelectedValue, out ciudadId);
            if (ciudadId > 0)
            {
                ciudad.CiudadID = ciudadId;
            }
            return ciudad;
        }
        private Localidad UserInterfaceToData()
        {
            Localidad localidad = new Localidad();
            localidad.Ciudad = GetCiudadFromUI();
            localidad.Nombre = txtNombre.Text.Trim();
            localidad.FechaRegistro = DateTime.Now;
            localidad.Codigo = txtCodigo.Text.Trim();
            return localidad;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoInsert()
        {
            string messageValidate = validateData();
            if (string.IsNullOrEmpty(messageValidate))
            {
                try
                {
                    Localidad localidad = UserInterfaceToData();
                    localidadCtrl.Insert(ConnectionHlp.Default.Connection, localidad);
                    txtRedirect.Value = "BuscarLocalidades.aspx";
                    ShowMessage("La localidad se ha registrado con éxito", MessageType.Information);

                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message, MessageType.Error);
                }

            }
            else
            {
                ShowMessage(messageValidate, MessageType.Information);
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
        
        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOLOCALIDADES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARLOCALIDADES) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!creacion)
                redirector.GoToHomePage(true);
        }
        #endregion
    }
}