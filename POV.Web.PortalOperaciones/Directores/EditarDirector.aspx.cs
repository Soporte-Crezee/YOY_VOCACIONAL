using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.Web.PortalOperaciones.Helper;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Licencias.Service;

namespace POV.Web.PortalOperaciones.Directores
{
    public partial class EditarDirector : PageBase
    {
        private DirectorCtrl directorCtrl;
        private UsuarioCtrl usuarioCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        private Director LastObject
        {
            set { Session["lastDirectorCatalogo"] = value; }
            get { return Session["lastDirectorCatalogo"] != null ? Session["lastDirectorCatalogo"] as Director : null; }
        }

        #region *** propiedades de clase ***

        #endregion

        public EditarDirector()
        {
            directorCtrl = new DirectorCtrl();
            usuarioCtrl= new UsuarioCtrl();
            licenciaEscuelaCtrl= new LicenciaEscuelaCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtDirectorID.Enabled = false;
            if (!IsPostBack)
            {
                if (LastObject != null)
                {
                    DataSet ds = directorCtrl.Retrieve(dctx, LastObject);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        LastObject = directorCtrl.LastDataRowToDirector(ds);
                        DataToUserInterface(LastObject);
                        FillBack();
                    }
                    else
                    {
                        txtRedirect.Value = "BuscarDirectores.aspx";
                        ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                    }
                }
                else
                {
                    txtRedirect.Value = "BuscarDirectores.aspx";
                    ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DoUpdate();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LastObject = null;
            Response.Redirect("~/Directores/BuscarDirectores.aspx", true);
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
        private void DataToUserInterface(Director director)
        {
            this.txtDirectorID.Text = director.DirectorID.ToString();
            this.txtCurp.Text = director.Curp;
            this.txtNombre.Text = director.Nombre;
            this.txtPApellido.Text = director.PrimerApellido;
            this.txtSApellido.Text = director.SegundoApellido;
            this.txtFechaNacimiento.Text = string.Format("{0:dd/MM/yyyy}", director.FechaNacimiento);
            if (director.Sexo == true)
                CbSexo.SelectedValue = "true";
            else
                CbSexo.SelectedValue = "false";
            this.txtNivelEscolar.Text = director.NivelEscolar;
            this.txtCorreo.Text = director.Correo;
            this.txtTelefono.Text = director.Telefono;

            if (director.Estatus == true)
                cbEstatus.SelectedValue = "true";
            else
                cbEstatus.SelectedValue = "false";

        }
        #endregion

        #region *** UserInterface to Data ***
        private Director UserInterfaceToData()
        {
            Director director = (Director)LastObject.Clone();

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

            if (cbEstatus.SelectedValue == "true")
                director.Estatus = true;
            else
                director.Estatus = false;

            return director;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoUpdate()
        {
            LicenciaDirector licenciaDirector= new LicenciaDirector();
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
                        if (dirTemp.DirectorID != director.DirectorID)
                        {
                            throw new Exception("La Curp no está disponible, seleccione otra");
                        }
                    }
               
                if (!string.IsNullOrEmpty(director.Correo))
                {

                    Usuario usuario = new Usuario {Email = director.Correo, EsActivo = true};
                    DataSet dsUsuario = usuarioCtrl.Retrieve(dctx, usuario);
                    if (dsUsuario.Tables[0].Rows.Count > 0) // si existe el usuario con el username
                    {
                        usuario = usuarioCtrl.LastDataRowToUsuario(dsUsuario);
                        List<LicenciaEscuela> licenciasEscuela = licenciaEscuelaCtrl.RetrieveLicencia(dctx, usuario);
                        List<LicenciaDirector> licenciasDirector = new List<LicenciaDirector>();
                        foreach (LicenciaEscuela licenciaEscuela in licenciasEscuela)
                            // se recorre la lista para encontrar las del director.
                        {
                            //buscamos la licencia de director
                            ALicencia licencia =
                                licenciaEscuela.ListaLicencia.Find(
                                    item => (bool) item.Activo && item.Tipo == ETipoLicencia.DIRECTOR);
                            licenciaDirector = (LicenciaDirector) licencia;
                            if(licenciaDirector!=null)
                            licenciasDirector.Add(licenciaDirector);
                            
                        }
                        if (licenciasDirector.Count>0)
                        {
                            if(LastObject.DirectorID == licenciaDirector.Director.DirectorID)
                            {
                                Usuario usuarioAnterior = new Usuario();
                                usuarioAnterior = usuario;
                                usuarioAnterior.Email = LastObject.Correo;
                                usuarioCtrl.Update(dctx, usuario, usuarioAnterior);
                            }
                            else
                                throw new Exception("Correo electrónico no disponible, seleccione otro");
                        }
                        else
                        {
                            throw new Exception("Correo electrónico no disponible, seleccione otro");
                        }
                        
                    }
                    dsDirector = directorCtrl.Retrieve(dctx, new Director { Correo = director.Correo });
                    if (dsDirector.Tables[0].Rows.Count > 0)
                    {
                        Director dirTemp = directorCtrl.LastDataRowToDirector(dsDirector);
                        if (dirTemp.DirectorID != LastObject.DirectorID)
                            throw new Exception("Correo electrónico no disponible, seleccione otro");
                    }
                    directorCtrl.Update(dctx, director, LastObject);

                    dctx.CommitTransaction(myFirm);
                    txtRedirect.Value = "BuscarDirectores.aspx";
                    ShowMessage("El director se ha actualizado con éxito", MessageType.Information);
                    LastObject = null;
                    
                }
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
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARDIRECTORES) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARDIRECTORES) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARDIRECTORES) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!lectura)
                redirector.GoToHomePage(true);
            if (!delete)
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