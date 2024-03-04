using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Web.PortalOperaciones.Helper;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.Web.PortalOperaciones.AppCode.Page;

namespace POV.Web.PortalOperaciones.Usuarios
{
    public partial class EditarUsuario : PageBase
    {
        private UsuarioCtrl usuarioCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        #region *** propiedades de clase ***
        private Usuario LastObject
        {
            set { Session["lastUsuarioCatalogo"] = value; }
            get { return Session["lastUsuarioCatalogo"] != null ? Session["lastUsuarioCatalogo"] as Usuario : null; }
        }
        #endregion

        public EditarUsuario()
        {
            usuarioCtrl = new UsuarioCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (LastObject != null)
                {
                    DataSet ds = usuarioCtrl.Retrieve(dctx, LastObject);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        LastObject = usuarioCtrl.LastDataRowToUsuario(ds);
                        DataToUserInterface(LastObject);
                        FillBack();
                    }
                    else
                    {
                        txtRedirect.Value = "BuscarUsuarios.aspx";
                        ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                    }
                }
                else
                {
                    txtRedirect.Value = "BuscarUsuarios.aspx";
                    ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DoUpdate();
        }
        #endregion

        #region *** validaciones ***
        private string ValidateData()
        {
            string sError = string.Empty;


            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void DataToUserInterface(Usuario usuario)
        {
            this.txtNombre.Text = usuario.NombreUsuario;
            this.txtEmail.Text = usuario.Email;
            this.chkActivo.Checked = (bool)usuario.EsActivo;
        }
        #endregion

        #region *** UserInterface to Data ***
        private Usuario UserInterfaceToData()
        {
            Usuario usuario = (Usuario)LastObject.Clone();
            if (txtNombre.Text.Trim().Length > 0)
                usuario.NombreUsuario = txtNombre.Text.Trim();

            string email = txtEmail.Text.Trim();

            if (!string.IsNullOrEmpty(email))
                usuario.Email = email;

            usuario.EsActivo = chkActivo.Checked;

            string pass = txtContrasena.Text;
            //cambio contraseña?
            if (!string.IsNullOrEmpty(pass))
                usuario.Password = EncryptHash.SHA1encrypt(pass);

            return usuario;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/UsuariosOperaciones/BuscarUsuario.aspx";
        }

        private void DoUpdate()
        {
            string validateMessage = ValidateData();

            if (string.IsNullOrEmpty(validateMessage))
            {
                IDataContext dctx = ConnectionHlp.Default.Connection;
                object myFirm = new object();
                try
                {
                    dctx.OpenConnection(myFirm);
                    dctx.BeginTransaction(myFirm);



                    Usuario usuario = UserInterfaceToData();

                    DataSet dsUsuario = usuarioCtrl.Retrieve(dctx, new Usuario { NombreUsuario = usuario.NombreUsuario });
                    if (dsUsuario.Tables[0].Rows.Count > 0)
                    {
                        Usuario userAux = usuarioCtrl.LastDataRowToUsuario(dsUsuario);
                        if (userAux.UsuarioID != LastObject.UsuarioID)
                            throw new Exception("Nombre de Usuario no disponible seleccione otro");
                    }
                    if (!string.IsNullOrEmpty(usuario.Email))
                    {
                        dsUsuario = usuarioCtrl.Retrieve(dctx, new Usuario { Email = usuario.Email, EsActivo = true});
                        if (dsUsuario.Tables[0].Rows.Count > 0)
                        {
                            Usuario userAux = usuarioCtrl.LastDataRowToUsuario(dsUsuario);
                            if (userAux.UsuarioID != LastObject.UsuarioID)
                                throw new Exception("Correo electrónico no disponible seleccione otro");
                        }
                    }

                    usuarioCtrl.Update(dctx, usuario, LastObject);



                    dctx.CommitTransaction(myFirm);
                    txtRedirect.Value = "BuscarUsuarios.aspx";
                    ShowMessage("El usuario se ha actualizado con éxito", MessageType.Information);
                    LastObject = null;
                }
                catch (Exception ex)
                {
                    dctx.RollbackTransaction(myFirm);
                    dctx.CloseConnection(myFirm);
                    txtRedirect.Value = string.Empty;
                    ShowMessage(ex.Message, MessageType.Error);
                }
                finally
                {
                    if (dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(myFirm);
                }
            }
            else
            {
                txtRedirect.Value = string.Empty;
                ShowMessage(validateMessage, MessageType.Error);
            }

        }
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOUSUARIO) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARUSUARIOS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARUSUARIOS) != null;

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