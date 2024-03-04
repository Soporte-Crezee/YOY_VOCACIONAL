using POV.CentroEducativo.BO;
using POV.Logger.Service;
using POV.Seguridad.BO;
using POV.Web.PortalUniversidad.AppCode.Page;
using Framework.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.Services;

namespace POV.Web.PortalUniversidad.Eventos
{
    public partial class NuevoEvento : PageBase
    {

        private EventoUniversidadCtrl eventoUniversidadCtrl;

        #region *** propiedades de clase ***
        #endregion

        public NuevoEvento()
        {
            eventoUniversidadCtrl = new EventoUniversidadCtrl(null);
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
                    
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
                LoggerHlp.Default.Error(this, ex);
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

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            redirector.GoToConsultarEventos(false);
        }
        #endregion

        #region *** validaciones ***
        private void ValidateData()
        {
            string sError = string.Empty;

            //Valores Requeridos.
            if (txtNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre";
            if (txtDescripcion.Text.Trim().Length <= 0)
                sError += " ,Descripcion";
            if (txtFechaInicio.Text.Trim().Length <= 0)
                sError += " ,Fecha inicio";
            if (txtFechaFin.Text.Trim().Length <= 0)
                sError += " ,Fecha fin";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son requeridos : {0}", sError));
            }


            //Valores con Incorrectos.
            if (txtNombre.Text.Trim().Length > 500)
                sError += " ,Nombre";
            if (txtDescripcion.Text.Trim().Length > 2000)
                sError += " ,Descripcion";
            if (txtFechaInicio.Text != string.Empty && txtFechaFin.Text != string.Empty)
            {
                var getDate = DateTime.Now;
                string day = (getDate.Day < 10 ? "0" + getDate.Day.ToString() : getDate.Day.ToString());
                string month = (getDate.Month < 10 ? "0" + getDate.Month.ToString() : getDate.Month.ToString());
                var today = day + "/" + month + "/" + getDate.Year;

                if (DateTime.Parse(txtFechaFin.Text) < DateTime.Parse(txtFechaInicio.Text))
                    sError += " , la fecha de inicio no puede ser mayor a la fecha fin";
                if(DateTime.Parse(txtFechaInicio.Text) < DateTime.Parse(today))
                    sError += " , la fecha de inicio no puede ser menor a la fecha actual";
                if (DateTime.Parse(txtFechaFin.Text) < DateTime.Parse(today))
                    sError += " , la fecha fin no puede ser menor a la fecha actual";
            }
            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos : {0}", sError));
            }


            //Formatos Inválidos
            DateTime fn;
            if (!DateTime.TryParseExact(txtFechaInicio.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fn))
                sError += " ,Fecha inicio";

            if (!DateTime.TryParseExact(txtFechaFin.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fn))
                sError += " ,Fecha fin";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros tienen un formato no valido : {0}", sError));
            }

        }

        #endregion

        #region *** Data to UserInterface ***

        #endregion

        #region *** UserInterface to Data ***
        private EventoUniversidad UserInterfaceToData()
        {
            EventoUniversidad obj = new EventoUniversidad();
            obj.Nombre = txtNombre.Text.Trim();
            obj.Descripcion = txtDescripcion.Text.Trim();
            obj.FechaInicio = DateTime.ParseExact(txtFechaInicio.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            obj.FechaFin = DateTime.ParseExact(txtFechaFin.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            obj.UniversidadId = userSession.CurrentUniversidad.UniversidadID;
            return obj;
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
                var obj = UserInterfaceToData();
                eventoUniversidadCtrl.Insert(obj);

                // Se recarga las relaciones de la universidad-carrera
                UniversidadCtrl sessionUpdCtrl = new UniversidadCtrl(null);
                userSession.CurrentUniversidad = sessionUpdCtrl.RetrieveWithRelationship(userSession.CurrentUniversidad, false).FirstOrDefault();

                redirector.GoToConsultarEventos(false);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegiosUniversidad.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOORIENTADOR) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOORIENTADOR) != null;

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