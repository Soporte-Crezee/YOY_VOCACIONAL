using Framework.Base.Exceptions;
using POV.Administracion.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalOperaciones.Alumnos
{
    public partial class RegistrarAlumno : CatalogPage
    {
        private CargarAlumnoCtrl cargarAlumnoCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private ContratoCtrl contratoCtrl;

        #region *** propiedades de clase ***
        private Escuela EscuelaActual
        {
            get { return (Escuela)this.Session["ESCUELA_KEY"]; }
            set { this.Session["ESCUELA_KEY"] = value; }
        }

        private CicloEscolar CicloEscolarActual
        {
            get { return (CicloEscolar)this.Session["CICLO_KEY"]; }
            set { this.Session["CICLO_KEY"] = value; }
        }
        private DataTable SS_ResultadoCarga
        {
            get { return (DataTable)this.Session["ResultadoCargaAlumnos"]; }
            set { this.Session["ResultadoCargaAlumnos"] = value; }
        }

        private DataTable SS_ListadoCarga
        {
            get { return (DataTable)this.Session["ListadoCargaAlumnos"]; }
            set { this.Session["ListadoCargaAlumnos"] = value; }
        }
        CicloEscolar ciclo;
        LicenciaEscuela licencia;
        CicloEscolarCtrl cicloEscolarCtrl;
        #endregion

        public RegistrarAlumno()
        {
            cargarAlumnoCtrl = new CargarAlumnoCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            contratoCtrl = new ContratoCtrl();

            ciclo = new CicloEscolar();
            licencia = new LicenciaEscuela();
            cicloEscolarCtrl = new CicloEscolarCtrl();
        }

        #region *** eventos de la página ***
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    FillUI();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
                this.txtRedirect.Value = "~/Deafault.aspx";
                throw;
            }
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fuArchivoAlumnos.PostedFile.FileName))
                this.ShowMessage("Es requerido seleccionar la ubicación del archivo a cargar", MessageType.Warning);

            if (!fuArchivoAlumnos.HasFile || !fuArchivoAlumnos.PostedFile.FileName.EndsWith("xls", true, CultureInfo.InvariantCulture))
                if (!fuArchivoAlumnos.PostedFile.FileName.EndsWith("xlsx", true, CultureInfo.InvariantCulture))
                    this.ShowMessage("Es requerido seleccionar un archivo con extensión xls o xlsx.", MessageType.Warning);
            if (this.ExisteError())
                return;

            string strpath = string.Empty;
            try
            {
                FileManagerCtrl file = new FileManagerCtrl();
                file.CreateFolder(Server.MapPath(@"~/Content/Temp/"));
                strpath = MapPath("~/Content/Temp/") + string.Format("{0}{1}", DateTime.Now.Ticks, fuArchivoAlumnos.FileName);
                fuArchivoAlumnos.SaveAs(strpath);

                this.SS_ResultadoCarga = cargarAlumnoCtrl.CargarArchivoAlumno(ConnectionHlp.Default.Connection, strpath, EscuelaActual, CicloEscolarActual);
                this.SS_ListadoCarga = cargarAlumnoCtrl.DsAlumnos.Tables[0];

                this.Response.Redirect("~/Reportes/CargaAlumnosReport.aspx");
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, exception);
                this.ShowMessage(exception.Message, MessageType.Error);
            }

            finally
            {
                try
                {
                    if (!string.IsNullOrEmpty(strpath) && File.Exists(strpath))
                        File.Delete(strpath);
                }
                catch (Exception)
                {

                    ;
                }
            }
        }
        #endregion

        #region *** Data to UserInterface ***
        #endregion
        #region *** metodos auxiliares ***
        private void FillUI()
        {
            // Ciclo Escolar
            CicloEscolarCtrl cicloEscolarCtrl = new CicloEscolarCtrl();
            CicloEscolar ciclo = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(ConnectionHlp.Default.Connection, new CicloEscolar { CicloEscolarID = 3 }));

            // Escuela
            EscuelaCtrl escuelCtrl = new EscuelaCtrl();
            Escuela escuela = escuelCtrl.LastDataRowToEscuela(escuelCtrl.Retrieve(ConnectionHlp.Default.Connection, new Escuela { EscuelaID = 1 }));
            if (ciclo.CicloEscolarID != null)
                this.CicloEscolarActual = ciclo;
            if (escuela.EscuelaID != null)
                this.EscuelaActual = escuela;
        }

        private bool ExisteError()
        {
            // Se ubican los controles que manejan el desplegado de error/advertencia/informacion
            if (Page.Master == null)
                return false;

            Control m = Page.Master.FindControl("hdnLastMessage");
            Control t = Page.Master.FindControl("hdnShowMessage");

            if (m == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnLastMessage' en la MasterPage");
            if (t == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnShowMessage' en la MasterPage.");

            if (m.GetType() != typeof(HiddenField) || t.GetType() != typeof(HiddenField))
                throw new Exception("No se pudo desplegar correctamente el error.\nAlguno de los controles de la MasterPage para el manejo de errores no es HiddenField.");

            return ((HiddenField)m).Value.Trim().Length > 0;
        }
        #endregion

        #region AUTORIZACION DE LA PAGINA

        private bool RenderEdit = false;
        private bool RenderDelete = false;

        protected override void DisplayCreateAction()
        {
            pnlCargarAlumnos.Visible = true;
        }

        protected override void DisplayReadAction()
        {

            Contenido.Visible = true;
        }

        protected override void DisplayUpdateAction()
        {
            RenderEdit = true;
        }

        protected override void DisplayDeleteAction()
        {
            RenderDelete = true;
        }

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCATPRUEBAS) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARCATPRUEBA) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARCATPRUEBA) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARCATPRUEBA) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCATPRUEBA) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (lectura)
                DisplayReadAction();
            if (creacion)
                DisplayCreateAction();
            if (delete)
                DisplayDeleteAction();
            if (edit)
                DisplayUpdateAction();
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