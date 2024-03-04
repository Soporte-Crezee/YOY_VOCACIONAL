using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.Administracion.Implements;
using POV.Core.Administracion.Interfaces;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.Administracion.Helper;
using POV.Administracion.Service;
using POV.Web.Administracion.AppCode.Page;
using POV.CentroEducativo.Services;
using POV.Localizacion.BO;
using POV.Comun.BO;
using POV.Localizacion.Service;
using POV.Comun.Service;
using Framework.Base.DataAccess;

namespace POV.Web.Administracion.Universidades
{
    public partial class EditarUniversidad : PageBase
    {
        private EscuelaCtrl escuelaCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private UsuarioCtrl usuarioCtrl;

        private UniversidadCtrl universidadCtrl;
        private UbicacionCtrl ubicacionCtrl;
        private PaisCtrl paisCtrl;
        private EstadoCtrl estadoCtrl;
        private CiudadCtrl ciudadCtrl;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;

        #region *** propiedades de clase ***
        public Universidad LastObjectUniversidad
        {
            get { return Session["LastUniversidad"] != null ? (Universidad)Session["LastUniversidad"] : null; }
            set { Session["LastUniversidad"] = value; }
        }
        #endregion

        public EditarUniversidad()
        {
            escuelaCtrl = new EscuelaCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            usuarioCtrl = new UsuarioCtrl();

            universidadCtrl = new UniversidadCtrl(null);
            ubicacionCtrl = new UbicacionCtrl();
            paisCtrl = new PaisCtrl();
            estadoCtrl = new EstadoCtrl();
            ciudadCtrl = new CiudadCtrl();

        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession == null || !userSession.IsLogin())
                    redirector.GoToLoginPage(true);

                if (userSession.CurrentEscuela == null || userSession.CurrentCicloEscolar == null)
                    redirector.GoToError(true);

                if (!IsPostBack)
                {
                    if (LastObjectUniversidad != null)
                        LoadUniversidad();
                    else
                        redirector.GoToConsultarUniversidad(true);
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
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
                if (CbPais.SelectedIndex > 0)
                {
                    LoadEstados(new Ubicacion { Estado = new Estado { Pais = new Pais { PaisID = int.Parse(CbPais.SelectedItem.Value) } } });
                }
                else
                {
                    CbEstado.ClearSelection();
                    CbMunicipio.ClearSelection();
                }

            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void CbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CbEstado.SelectedIndex > 0)
                {
                    LoadCiudades(new Ubicacion { Ciudad = new Ciudad { Estado = new Estado { EstadoID = int.Parse(CbEstado.SelectedItem.Value) } } });
                }
                else
                {
                    CbMunicipio.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        #endregion

        #region *** validaciones ***
        private void ValidateData()
        {
            string sError = string.Empty;

            //Valores Requeridos
            if (txtNombreUniversidad.Text.Trim().Length <= 0)
                sError += " ,Nombre Universidad";
            if (txtDireccion.Text.Trim().Length <= 0)
                sError += " ,Dirección";
            if (txtTelefono.Text.Trim().Length <= 0)
                sError += " ,Telefono";
            if (txtCorreo.Text.Trim().Length <= 0)
                sError += " ,Correo";
            if (txtUsuario.Text.Length <= 0)
                sError += " ,Usuario";

            if (ddlNivelEscolar.SelectedIndex == -1 || ddlNivelEscolar.SelectedValue.Length <= 0)
                sError += " ,Nivel";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son requeridos:{0}", sError));
            }

            //Valores con Incorrectos.
            if (txtNombreUniversidad.Text.Trim().Length > 250)
                sError += " ,Nombre";
            if (txtTelefono.Text.Trim().Length > 50)
                sError += " ,Primer Apellido";
            if (txtCorreo.Text.Trim().Length > 100)
                sError += " ,Correo Electrónico";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos :{0}", sError));
            }
            if (txtUsuario.Text.Length > 50)
                sError += " ,Usuario";

            if (!ValidateEmailRegex(txtCorreo.Text.Trim()))
                sError += " ,Correo Electrónico";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros tienen un formato no valido :{0}", sError));
            }
        }

        private bool ValidateEmailRegex(string email)
        {
            string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reLenient = new Regex(patternLenient);
            bool match = reLenient.IsMatch(email);
            return match;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void LoadUniversidad()
        {
            //Carga Datos Escuela y Ciclo Escolar
            if (LastObjectUniversidad == null || LastObjectUniversidad.UniversidadID == null)
                throw new Exception("EditarUniversidad LoadUniversidad: Ocurrió un error al procesar su solicitud");

            Universidad universidad = universidadCtrl.Retrieve(new Universidad { UniversidadID = LastObjectUniversidad.UniversidadID }, false).First();
            Usuario usuarioLic = licenciaEscuelaCtrl.RetrieveUsuarioUniversidad(ConnectionHlp.Default.Connection, new Universidad { UniversidadID = universidad.UniversidadID });
            var usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuarioLic));
            universidad.Ubicacion = new Ubicacion();
            universidad.Ubicacion.UbicacionID = universidad.UbicacionID;
            LoadUbicacion(universidad);
            DataToUserInterface(universidad, usuario);
        }
        private void DataToUserInterface(Universidad universidad, Usuario usuario)
        {
            txtNombreUniversidad.Text = universidad.NombreUniversidad;
            txtDireccion.Text = universidad.Direccion;
            txtClaveEscolar.Text = universidad.ClaveEscolar;
            txtSiglas.Text = universidad.Siglas;
            this.ddlNivelEscolar.SelectedValue = universidad.NivelEscolar != null ? ((byte)universidad.NivelEscolar).ToString() : null;

            txtUsuario.Enabled = false;
            txtUsuario.Text = usuario.NombreUsuario;
            txtCorreo.Text = usuario.Email;
            txtCorreoEdit.Text = usuario.Email;
            txtTelefono.Text = usuario.TelefonoReferencia;
            txtTelefonoEdit.Text = usuario.TelefonoReferencia;
        }
        #endregion

        #region *** UserInterface to Data ***
        private Universidad UserInterfaceToDataUniversidad()
        {
            Universidad universidad = new Universidad();
            universidad.NombreUniversidad = txtNombreUniversidad.Text.Trim();
            universidad.Direccion = txtDireccion.Text.Trim();
            universidad.ClaveEscolar = txtClaveEscolar.Text.Trim();
            universidad.Siglas = txtSiglas.Text.Trim();
            universidad.NivelEscolar = ddlNivelEscolar.SelectedIndex > 0 ? (ENivelEscolar?)byte.Parse(ddlNivelEscolar.SelectedItem.Value) : null;

            //Ubicación
            Ubicacion ubicacion = new Ubicacion();
            ubicacion.Pais = new Pais { PaisID = CbPais.SelectedIndex > 0 ? int.Parse(CbPais.SelectedItem.Value) : (int?)null };
            ubicacion.Estado = new Estado { EstadoID = CbEstado.SelectedIndex > 0 ? int.Parse(CbEstado.SelectedItem.Value) : (int?)null };
            ubicacion.Ciudad = new Ciudad { CiudadID = CbMunicipio.SelectedIndex > 0 ? int.Parse(CbMunicipio.SelectedItem.Value) : (int?)null };

            Ubicacion ubicacionExiste = new Ubicacion();
            DataSet dsUbicacion = ubicacionCtrl.RetrieveExacto(dctx, ubicacion);
            int index = dsUbicacion.Tables["Ubicacion"].Rows.Count;
            if (index == 1)
                ubicacionExiste = ubicacionCtrl.LastDataRowToUbicacion(dsUbicacion);
            //si no existe se inserta la ubicacion
            if (ubicacionExiste.UbicacionID == null)
            {
                if ((ubicacion.Pais.PaisID != null) && (ubicacion.Estado.EstadoID != null))
                {
                    ubicacion.FechaRegistro = DateTime.Now;
                    ubicacionCtrl.Insert(dctx, ubicacion);
                    ubicacion = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.RetrieveExacto(dctx, ubicacion));
                    universidad.UbicacionID = (long)ubicacion.UbicacionID;
                }
            }
            else
                universidad.UbicacionID = (long)ubicacionExiste.UbicacionID;
            return universidad;
        }

        private Usuario UserInterfaceToDataUsuario()
        {
            Usuario usuario = new Usuario();
            usuario.TelefonoReferencia = txtTelefono.Text.Trim();
            usuario.Email = txtCorreo.Text.Trim();
            return usuario;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoUpdate()
        {
            try
            {
                try
                {
                    ValidateData();
                }
                catch (Exception ex)
                {
                    this.ShowMessage(ex.Message, MessageType.Error);
                    return;
                }
                CatalogoUniversidadesCtrl catUniversidadesCtrl = new CatalogoUniversidadesCtrl();

                Universidad universidad = universidadCtrl.Retrieve(new Universidad { UniversidadID = LastObjectUniversidad.UniversidadID }, true).First();
                bool activo = (bool)universidad.Activo;
                universidad = UserInterfaceToDataUniversidad();

                universidad.Activo = activo;
                universidad.UniversidadID = LastObjectUniversidad.UniversidadID;


                Usuario usuarioLic = licenciaEscuelaCtrl.RetrieveUsuarioUniversidad(ConnectionHlp.Default.Connection, new Universidad { UniversidadID = LastObjectUniversidad.UniversidadID });
                var usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuarioLic));
                usuario = UserInterfaceToDataUsuario();
                usuario.UsuarioID = usuarioLic.UsuarioID;

                catUniversidadesCtrl.UpdateUniversidadEscuela(dctx, userSession.CurrentEscuela, userSession.CurrentCicloEscolar, universidad, usuario);
                LastObjectUniversidad = null;

                redirector.GoToConsultarUniversidad(false);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        #region Cargar Ubicacion
        private void LoadUbicacion(Universidad universidad)
        {
            if (universidad.Ubicacion.UbicacionID != null)
            {
                universidad.Ubicacion = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.Retrieve(dctx, universidad.Ubicacion));

                LoadPaises(new Ubicacion { Pais = new Pais { PaisID = universidad.Ubicacion.Pais.PaisID } });
                CbPais.SelectedValue = universidad.Ubicacion.Pais != null ? universidad.Ubicacion.Pais.PaisID.ToString() : null;

                LoadEstados(new Ubicacion { Estado = new Estado { Pais = universidad.Ubicacion.Pais } });
                CbEstado.SelectedValue = universidad.Ubicacion.Estado != null ? universidad.Ubicacion.Estado.EstadoID.ToString() : null;

                LoadCiudades(new Ubicacion { Ciudad = new Ciudad { Estado = universidad.Ubicacion.Estado } });
                CbMunicipio.SelectedValue = universidad.Ubicacion.Ciudad != null ? universidad.Ubicacion.Ciudad.CiudadID.ToString() : null;
            }

            else
            {
                LoadPaises(new Ubicacion { Pais = new Pais() });
            }
        }

        private void LoadPaises(Ubicacion filter)
        {
            if (filter == null || filter.Pais == null)
                return;
            DataSet ds = paisCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Pais);
            CbPais.DataSource = ds;
            CbPais.DataValueField = "PaisID";
            CbPais.DataTextField = "Nombre";
            CbPais.DataBind();
            CbPais.Items.Insert(0, new ListItem("", ""));
        }

        private void LoadEstados(Ubicacion filter)
        {
            if (filter == null || filter.Estado == null)
                return;
            DataSet ds = estadoCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Estado);
            CbEstado.DataSource = ds;
            CbEstado.DataValueField = "EstadoID";
            CbEstado.DataTextField = "Nombre";
            CbEstado.DataBind();
            CbEstado.Items.Insert(0, new ListItem("", ""));
        }

        private void LoadCiudades(Ubicacion filter)
        {
            if (filter == null || filter.Ciudad == null)
                return;
            DataSet ds = ciudadCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Ciudad);
            CbMunicipio.DataSource = ds;
            CbMunicipio.DataValueField = "CiudadID";
            CbMunicipio.DataTextField = "Nombre";
            CbMunicipio.DataBind();
            CbMunicipio.Items.Insert(0, new ListItem("", ""));
        }
        #endregion
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegiosDirector.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESODOCENTES) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PACONSULTARDOCENTES) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAEDITARDOCENTES) != null;

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