using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Core.Operaciones;
using POV.Core.Operaciones.Implements;
using POV.Core.Operaciones.Interfaces;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;

namespace POV.Web.PortalOperaciones.Profesionalizacion.CatalogoAsistencias
{
    public partial class EditarAsistencia : PageBase
    {
        #region Propiedades

        private AsistenciaCtrl asistenciaCtrl = null;
        private TemaAsistenciaCtrl temaAsistenciaCtrl;
        private IDataContext dctx = null;
        Asistencia LastObject
        {
            get { return Session["lastAsistencia"] != null ? Session["lastAsistencia"] as Asistencia : null; }
            set { Session["lastAsistencia"] = value; }
        }

        #endregion

        public EditarAsistencia()
        {
            temaAsistenciaCtrl=new TemaAsistenciaCtrl();
            asistenciaCtrl=new AsistenciaCtrl();
        }

        #region ****Eventos de pagina****

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (LastObject == null || LastObject.AgrupadorContenidoDigitalID == null)
                    Response.Redirect(UrlHelper.GetConsultarAsistenciasURL());

                if (!Page.IsPostBack)
                {
                    LoadTemaAsistencia();
                    LoadEstatusProfesionalizacion();
                    LoadAsistencia();
                }

            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                Response.Redirect(UrlHelper.GetConsultarAsistenciasURL());

            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
              Response.Redirect(UrlHelper.GetConsultarAsistenciasURL());
            }
            catch (Exception ex)
            {
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DoUpdate();

               
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message,MessageType.Error);
            }
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            try
            {
                ClearSession();
                Response.Redirect(UrlHelper.GetConsultarAsistenciasURL());
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region ****Data to UserInterface****

        private void DataToUserInterface(Asistencia asistencia)
        {
            txtNombre.Text = asistencia.Nombre;
            txtDescripcion.Text = asistencia.Descripcion;

            if (asistencia.Estatus != null)
                ddlEstatus.SelectedValue = ((byte)asistencia.Estatus.Value).ToString();

            if (asistencia.TemaAsistencia != null && asistencia.TemaAsistencia.TemaAsistenciaID != null)
                ddlTemaAsistencia.SelectedValue = asistencia.TemaAsistencia.TemaAsistenciaID.ToString();
        }
        
        #endregion

        #region ****UserInterface to Data******

        private Asistencia UserInterfaceToData()
        {
            Asistencia asistencia = new Asistencia();
            asistencia.Nombre = !string.IsNullOrEmpty(txtNombre.Text.Trim()) ? txtNombre.Text.Trim() : null;
            asistencia.Descripcion = !string.IsNullOrEmpty(txtDescripcion.Text.Trim()) ? txtDescripcion.Text.Trim() : null;

            if (ddlEstatus.SelectedIndex != -1)
            {
                byte estatus = byte.Parse(ddlEstatus.SelectedItem.Value);
                asistencia.Estatus = (EEstatusProfesionalizacion?)estatus;
            }
            if (ddlTemaAsistencia.SelectedIndex != -1)
            {
                asistencia.TemaAsistencia = new TemaAsistencia { TemaAsistenciaID = int.Parse(ddlTemaAsistencia.SelectedItem.Value) };
            }

            return asistencia;
        }
        #endregion

        #region ****Validaciones****
        private void ValidateData()
        {
            string sError = string.Empty;

            //Campos requeridos
            if (txtNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre ";

            if (ddlEstatus.SelectedIndex <= -1)
                sError += " ,Estado ";

            if (ddlTemaAsistencia.SelectedIndex <= -1)
                sError += " ,Tema Asistencia";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son requeridos {0}", sError));
            }

            //valores incorrectos
            if (txtNombre.Text.Trim().Length > 100)
                sError += " ,Nombre";

            if (txtDescripcion.Text.Trim().Length > 500)
                sError += " ,Descripción";


            if (int.Parse(ddlTemaAsistencia.SelectedItem.Value) <= 0)
                sError += " ,Tema Asistencia";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format(" Los siguientes parámetros son inválidos: {0} ", sError));
            }
        }

        #endregion

        #region ****Metodos auxiliares*****

        private void LoadTemaAsistencia()
        {
            dctx = Helper.ConnectionHlp.Default.Connection;
            DataSet ds = temaAsistenciaCtrl.Retrieve(dctx, new TemaAsistencia { Activo = true });
            ddlTemaAsistencia.DataSource = ds;
            ddlTemaAsistencia.DataTextField = "Nombre";
            ddlTemaAsistencia.DataValueField = "TemaAsistenciaID";
            ddlTemaAsistencia.DataBind();
            ddlTemaAsistencia.Items.Insert(0, new ListItem("SELECCIONE...", "-1"));
        }
        private void LoadEstatusProfesionalizacion()
        {
            Array items = Enum.GetValues(typeof(EEstatusProfesionalizacion));

            ddlEstatus.Items.Clear();
            ddlEstatus.Items.Add(new ListItem("SELECCIONE...", "-1"));
            foreach (byte item in items)
            {
                if (item != (byte)EEstatusProfesionalizacion.INACTIVO)
                {
                    var itemname = Enum.GetName(typeof(EEstatusProfesionalizacion), item);
                    ddlEstatus.Items.Add(new ListItem(itemname, item.ToString()));
                }

            }
        }
        private void LoadAsistencia()
        {
            dctx = Helper.ConnectionHlp.Default.Connection;
            LastObject = (Asistencia)asistenciaCtrl.RetrieveComplete(dctx, LastObject);
            DataToUserInterface(LastObject);
        }
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
                    ShowMessage(ex.Message, MessageType.Error);
                    return;
                }

                dctx = Helper.ConnectionHlp.Default.Connection;
                Asistencia asistencia = UserInterfaceToData();
                asistencia.FechaRegistro = LastObject.FechaRegistro;
                asistencia.EsPredeterminado = LastObject.EsPredeterminado;
                asistencia.AgrupadorContenidoDigitalID = LastObject.AgrupadorContenidoDigitalID;
                foreach (AAgrupadorContenidoDigital agrupadorPrevio in LastObject.AgrupadoresContenido)
                {
                    AAgrupadorContenidoDigital agrupadorSimple = new AgrupadorSimple();

                    agrupadorSimple.AgrupadorContenidoDigitalID = agrupadorPrevio.AgrupadorContenidoDigitalID;
                    agrupadorSimple.EsPredeterminado = asistencia.EsPredeterminado;
                    agrupadorSimple.Estatus = asistencia.Estatus;
                    agrupadorSimple.FechaRegistro = asistencia.FechaRegistro;
                    agrupadorSimple.Nombre = asistencia.Nombre;
                    asistencia.Agregar(agrupadorSimple);
                }

                asistenciaCtrl.UpdateComplete(dctx, asistencia, LastObject);
                ShowMessage("Actualización exitosa", MessageType.Information);
                txtRedirect.Value = UrlHelper.GetConsultarAsistenciasURL();
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw new Exception("Ocurrió un error al editar la asistencia");
            }


        }
        private void ClearSession()
        {
            LastObject = null;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOASISTENCIA) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARASISTENCIA) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARASISTENCIA) != null;

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