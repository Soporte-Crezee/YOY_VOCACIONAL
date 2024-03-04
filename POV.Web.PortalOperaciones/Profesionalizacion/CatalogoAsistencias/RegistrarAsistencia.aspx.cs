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
using POV.Core.Operaciones.Interfaces;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;

namespace POV.Web.PortalOperaciones.Profesionalizacion.CatalogoAsistencias
{
    public partial class RegistrarAsistencia : PageBase
    {
        #region Propiedades

        private AsistenciaCtrl asistenciaCtrl = null;
        private TemaAsistenciaCtrl temaAsistenciaCtrl = null;
        private IDataContext dctx = null;
      

        #endregion

        public RegistrarAsistencia()
        {
        temaAsistenciaCtrl=new TemaAsistenciaCtrl();
         asistenciaCtrl= new AsistenciaCtrl();   
        }

        #region ****Eventos de pagina****

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    LoadTemaAsistencia();
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
                DoInsert();
                
            }
            catch (Exception ex)
            {

                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

 
        #endregion

        #region ****UserInterface to Data****

        private Asistencia UserInterfaceToData()
        {
            Asistencia asistencia = new Asistencia();
            asistencia.Nombre = !string.IsNullOrEmpty(txtNombre.Text.Trim()) ? txtNombre.Text.Trim() : null;
            asistencia.Descripcion = !string.IsNullOrEmpty(txtDescripcion.Text.Trim()) ? txtDescripcion.Text.Trim() : null;

            if (ddlTemaAsistencia.SelectedIndex != -1)
            {
                asistencia.TemaAsistencia = new TemaAsistencia { TemaAsistenciaID = int.Parse(ddlTemaAsistencia.SelectedItem.Value) };
            }

            return asistencia;
        }
        #endregion

        #region ****validaciones****
        
        private void ValidateData()
        {
            string sError = string.Empty;

            //Campos requeridos
            if (txtNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre ";

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

        #region ****Metodos de la pagina****


        private void LoadTemaAsistencia()
        {
            dctx = Helper.ConnectionHlp.Default.Connection;
            DataSet ds = temaAsistenciaCtrl.Retrieve(dctx, new TemaAsistencia { Activo = true });
            ddlTemaAsistencia.DataSource = ds;
            ddlTemaAsistencia.DataTextField = "Nombre";
            ddlTemaAsistencia.DataValueField = "TemaAsistenciaID";
            ddlTemaAsistencia.DataBind();
            ddlTemaAsistencia.Items.Insert(0, new ListItem("SELECCIONE..", "-1"));
        }

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
                    this.ShowMessage(ex.Message,MessageType.Error);
                    return;
                }
                dctx = Helper.ConnectionHlp.Default.Connection;
                Asistencia asistencia = UserInterfaceToData();
                asistencia.FechaRegistro = DateTime.Now;
                asistencia.EsPredeterminado = true;
                asistencia.Estatus = EEstatusProfesionalizacion.MANTENIMIENTO;

                AgrupadorSimple agrupadorSimple = new AgrupadorSimple{
                        Estatus = asistencia.Estatus,Nombre = asistencia.Nombre,
                        EsPredeterminado = asistencia.EsPredeterminado,FechaRegistro = asistencia.FechaRegistro
                    };
                asistencia.Agregar(agrupadorSimple);
                asistenciaCtrl.InsertComplete(dctx,asistencia);

                ShowMessage("Registro exitoso", MessageType.Information);
                this.txtRedirect.Value = UrlHelper.GetConsultarAsistenciasURL();
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw new Exception("Ocurrió un error al registrar la asistencia");
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOASISTENCIA) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARASISTENCIA) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARASISTENCIA) != null;


            if (!acceso)
                redirector.GoToHomePage(true);
            if (!lectura)
                redirector.GoToHomePage(true);
            if (!creacion)
                redirector.GoToHomePage(true);

        }

        #endregion
    }
}