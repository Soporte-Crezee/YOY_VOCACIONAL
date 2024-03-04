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
    public partial class RegistrarUsuario : PageBase
    {
        private UsuarioCtrl usuarioCtrl;

        #region *** propiedades de clase ***

        #endregion

        public RegistrarUsuario()
        {
            usuarioCtrl = new UsuarioCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBack();
            }

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DoInsert();
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

        #endregion

        #region *** UserInterface to Data ***
        private Usuario UserInterfaceToData()
        {
            Usuario usuario = new Usuario();
            if (txtNombre.Text.Trim().Length > 0)
                usuario.NombreUsuario = txtNombre.Text.Trim();
            if (txtContrasena.Text.Trim().Length > 0)
                usuario.Password = EncryptHash.SHA1encrypt(txtContrasena.Text);
            if (txtEmail.Text.Trim().Length > 0)
                usuario.Email = txtEmail.Text.Trim();
            usuario.EsActivo = true;
            usuario.AceptoTerminos = false;
            usuario.EmailVerificado = true;
            usuario.FechaCreacion = DateTime.Now;
            usuario.PasswordTemp = false;
            usuario.Termino = new GP.SocialEngine.BO.Termino { TerminoID = 1 };

            return usuario;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Usuarios/BuscarUsuarios.aspx";
        }

        private void DoInsert()
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
                        throw new Exception("Nombre de Usuario no disponible seleccione otro");
                    if (!string.IsNullOrEmpty(usuario.Email))
                    {
                        dsUsuario = usuarioCtrl.Retrieve(dctx, new Usuario { Email = usuario.Email });
                        if (dsUsuario.Tables[0].Rows.Count > 0)
                            throw new Exception("Correo electrónico no disponible seleccione otro");
                    }

                    usuarioCtrl.Insert(dctx, usuario);

                    usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));

                    UsuarioPrivilegios usuarioPrivilegios = new UsuarioPrivilegios();
                    usuarioPrivilegios.Usuario = usuario;
                    usuarioPrivilegios.Estado = true;
                    usuarioPrivilegios.FechaCreacion = DateTime.Now;

                    new UsuarioPrivilegiosCtrl().Insert(dctx, usuarioPrivilegios);

                    dctx.CommitTransaction(myFirm);
                    txtRedirect.Value = "BuscarUsuarios.aspx";
                    ShowMessage("El usuario se ha registrado con éxito", MessageType.Information);
                }
                catch (Exception ex)
                {
                    dctx.RollbackTransaction(myFirm);
                    dctx.CloseConnection(myFirm);
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
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARUSUARIOS) != null;

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