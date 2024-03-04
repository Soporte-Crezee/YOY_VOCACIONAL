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
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Seguridad.Utils;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;
using POV.Seguridad.Service;

namespace POV.Web.PortalOperaciones.Directores
{
    public partial class RegistrarDirector : PageBase
    {
        private DirectorCtrl directorCtrl;
        private UsuarioCtrl usuarioCtrl;

        #region *** propiedades de clase ***

        #endregion

        public RegistrarDirector()
        {
            directorCtrl = new DirectorCtrl();
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

            if (!string.IsNullOrEmpty(txtCurp.Text.Trim()))
            {
                if (txtCurp.Text.Trim().Length > 18)
                    sError += ", La CURP no debe ser mayor de 18 caracteres";
            }
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                if (txtNombre.Text.Trim().Length > 200)
                    sError += ",El Nombre no debe ser mayor de 200 caracteres";
            }
            if (!string.IsNullOrEmpty(txtPApellido.Text.Trim()))
            {
                if (txtPApellido.Text.Trim().Length > 100)
                    sError += ",El primer apellido no debe ser mayor de 100 caracteres";
            }
            if (!string.IsNullOrEmpty(txtSApellido.Text.Trim()))
            {
                if (txtSApellido.Text.Trim().Length > 100)
                    sError += ",El segundo apellido no debe ser mayor de 100 caracteres";
            }
            if (!string.IsNullOrEmpty(txtNivelEscolar.Text.Trim()))
            {
                if (txtNivelEscolar.Text.Trim().Length > 50)
                    sError += ",El nivel escolar no debe ser mayor de 50 caracteres";
            }
            if (!string.IsNullOrEmpty(txtCorreo.Text.Trim()))
            {
                if (txtCorreo.Text.Trim().Length > 50)
                    sError += ",El correo no debe ser mayor de 50 caracteres";
            }
            if (!string.IsNullOrEmpty(txtTelefono.Text.Trim()))
            {
                if (txtTelefono.Text.Trim().Length > 20)
                    sError += ",El Teléfono no debe ser mayor de 20 caracteres";
            }
            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***

        #endregion

        #region *** UserInterface to Data ***
        private Director UserInterfaceToData()
        {
            Director director = new Director();
            if (txtCurp.Text.Trim().Length > 0)
                director.Curp = txtCurp.Text.Trim();
            if (txtNombre.Text.Trim().Length > 0)
                director.Nombre = txtNombre.Text.Trim();
            if (txtPApellido.Text.Trim().Length > 0)
                director.PrimerApellido = txtPApellido.Text.Trim();
            if (txtSApellido.Text.Trim().Length > 0)
                director.SegundoApellido = txtSApellido.Text.Trim();
            if (txtFechaNacimiento.Text.Trim().Length > 0)
                director.FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text.Trim());
            if (CbSexo.SelectedValue == "true")
                director.Sexo = true;
            else
                director.Sexo = false;

            if (txtNivelEscolar.Text.Trim().Length > 0)
                director.NivelEscolar = txtNivelEscolar.Text.Trim();
            if (txtCorreo.Text.Trim().Length > 0)
                director.Correo = txtCorreo.Text.Trim();
            if (txtTelefono.Text.Trim().Length > 0)
                director.Telefono = txtTelefono.Text.Trim();

            director.Estatus = true;
            director.FechaRegistro = DateTime.Now;
            director.EstatusIdentificacion = false;
            director.Clave = new PasswordProvider(10).GetNewPassword();
            return director;
        }
        #endregion

        #region *** metodos auxiliares ***
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

                    Director director = UserInterfaceToData();

                    DataSet dsDirector = directorCtrl.Retrieve(dctx, new Director { Curp = director.Curp });
                    if (dsDirector.Tables[0].Rows.Count > 0)
                    {
                        Director dirTemp = directorCtrl.LastDataRowToDirector(dsDirector);
                        if (dirTemp.Estatus == true)
                        {
                            throw new Exception("Ya se ha registrado un director con esta Curp");
                        }
                        else
                        {
                            directorCtrl.Update(dctx, director, dirTemp);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(director.Correo))
                        {
                            DataSet dsUsuario = usuarioCtrl.Retrieve(dctx, new Usuario{Email = director.Correo, EsActivo = true});
                            if(dsUsuario.Tables[0].Rows.Count>0)
                                throw new Exception("El correo ya está en uso, proporcione otro.");
                            dsDirector = directorCtrl.Retrieve(dctx, new Director { Correo = director.Correo, Estatus = true});
                            if (dsDirector.Tables[0].Rows.Count > 0)
                                throw new Exception("El correo ya está en uso, proporcione otro.");
                        }

                        directorCtrl.Insert(dctx, director);
                    }
                    dctx.CommitTransaction(myFirm);
                    txtRedirect.Value = "BuscarDirectores.aspx";
                    ShowMessage("El director se ha registrado con éxito", MessageType.Information);
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
        }
        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Directores/BuscarDirectores.aspx";
        }
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESODIRECTORES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARDIRECTORES) != null;

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