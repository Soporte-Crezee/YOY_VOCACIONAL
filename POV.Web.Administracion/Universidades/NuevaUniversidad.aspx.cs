using POV.Administracion.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.Service;
using POV.Logger.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Utils;
using POV.Web.Administracion.AppCode.Page;
using POV.Web.Administracion.Helper;
using Framework.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.Services;
using Framework.Base.DataAccess;
using POV.Localizacion.Service;
using POV.Comun.BO;
using POV.Localizacion.BO;
using System.Data;

namespace POV.Web.Administracion.Universidades
{
    public partial class NuevaUniversidad : PageBase
    {

        private EscuelaCtrl escuelaCtrl;
        private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;

        private UniversidadCtrl universidadCtrl;
        private CatalogoUniversidadesCtrl catalogoUniversidadesCtrl;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;
        private UbicacionCtrl ubicacionCtrl;
        private PaisCtrl paisCtrl;
        private EstadoCtrl estadoCtrl;
        private CiudadCtrl ciudadCtrl;

        public NuevaUniversidad()
        {
            grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
            escuelaCtrl = new EscuelaCtrl();
            catalogoUniversidadesCtrl = new CatalogoUniversidadesCtrl();
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalUniversidad"]))
                catalogoUniversidadesCtrl.UrlPortalSocial =
                    System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalUniversidad"];

            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["POVUrlImgPOV"]))
                catalogoUniversidadesCtrl.UrlImgPOV =
                    System.Configuration.ConfigurationManager.AppSettings["POVUrlImgPOV"];

            universidadCtrl = new UniversidadCtrl(null);
            catalogoUniversidadesCtrl = new CatalogoUniversidadesCtrl();
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
                    LoadPaises(new Ubicacion { Pais = new Pais() });
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
            //Valores Requeridos.
            if (txtNombreUniversidad.Text.Trim().Length <= 0)
                sError += " ,Nombre";
            if (txtDireccion.Text.Trim().Length <= 0)
                sError += " ,Direccion";
            if (txtTelefono.Text.Trim().Length <= 0)
                sError += " ,Telefono";
            if (txtCorreo.Text.Trim().Length <= 0)
                sError += " ,Correo";
            if (CbPais.SelectedIndex == 0)
                sError += " ,Pais";
            if (String.IsNullOrEmpty(CbEstado.SelectedValue))
                sError += " ,Estado";
            if (String.IsNullOrEmpty(CbMunicipio.SelectedValue))
                sError += " ,Cuidad";
            if (this.txtNombreUsuario.Text.Trim().Length <= 0)
                sError += " ,Nombre usuario";
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
                sError += " , Teléfono";
            if (txtCorreo.Text.Trim().Length > 100)
                sError += " ,Correo Electrónico";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos :{0}", sError));
            }


            if (!ValidateEmailRegex(txtCorreo.Text.Trim()))
                sError += " ,Correo Electrónico";
        }

        private bool ValidateEmailRegex(string email)
        {
            string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reLenient = new Regex(patternLenient);
            bool match = reLenient.IsMatch(email);
            return match;
        }
        #endregion

        #region *** User Interface to Data ***
        private Universidad UniversidadUserInterfaceToData()
        {
            Universidad universidad = new Universidad();
            universidad.NombreUniversidad = txtNombreUniversidad.Text.Trim();
            universidad.Direccion = txtDireccion.Text.Trim();
            universidad.ClaveEscolar = txtClaveEscolar.Text.Trim();
            universidad.Siglas = txtSiglas.Text.Trim();
            universidad.NivelEscolar = ddlNivelEscolar.SelectedIndex > 0 ? (ENivelEscolar?)byte.Parse(ddlNivelEscolar.SelectedItem.Value) : null;
            #region insertar ubicacion
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
            #endregion
            return universidad;
        }

        private Usuario UsuarioUserInterface()
        {
            Usuario usuario = new Usuario();
            usuario.Email = txtCorreo.Text.Trim();
            usuario.NombreUsuario = txtNombreUsuario.Text.Trim();
            usuario.TelefonoReferencia = txtTelefono.Text.Trim();
            return usuario;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoInsert()
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
                Universidad universidad = UniversidadUserInterfaceToData();
                Usuario usuario = UsuarioUserInterface();
                string password = new PasswordProvider(8).GetNewPassword();
                Universidad existe = universidadCtrl.Retrieve(new Universidad { ClaveEscolar = universidad.ClaveEscolar }, false).FirstOrDefault();
                if (existe == null)
                {
                    Usuario universidadRegistroCorrecto = catalogoUniversidadesCtrl.InsertUniversidad(dctx, universidad, userSession.CurrentEscuela, userSession.CurrentCicloEscolar, usuario, password);
                    universidad = universidadCtrl.Retrieve(new Universidad { UniversidadID = universidad.UniversidadID }, false).First();
                    EnviarCorreo(usuario, universidad, password);

                    redirector.GoToConsultarUniversidad(false);
                }
                else
                    this.ShowMessage("La escuela que intenta registrar ya existe.", MessageType.Error);
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

        private void EnviarCorreo(Usuario usuario, Universidad universidad, string pws)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string imgalt = "YOY - Orientacion vocacional para estudiantes";
            const string titulo = "Registro exitoso";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalUniversidad"];
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateNewUniversidad.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{user}", usuario.NombreUsuario);
            cuerpo = cuerpo.Replace("{password}", pws);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);

            List<string> tos = new List<string>();
            tos.Add(usuario.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Registro exitoso", cuerpo, texto, archivos, copias);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('¡Registro exitoso! Revisa tu bandeja de entrada y/o bandeja de correo no deseado, serás redirigido al portal.'); window.location='" + linkportal + "';", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentalo mas tarde.');window.location='" + ConfigurationManager.AppSettings["POVUrlPortalSocial"] + "';", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }

        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegiosDirector.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESODOCENTES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAINSERTARDOCENTES) != null;

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