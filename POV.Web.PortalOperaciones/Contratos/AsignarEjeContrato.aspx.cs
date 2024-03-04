using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using System.Data;
using POV.Licencias.Service;
using POV.Licencias.BO;
using POV.Profesionalizacion.BO;
using POV.Profesionalizacion.Service;
using Framework.Base.DataAccess;
using POV.Core.Operaciones.Interfaces;
using POV.Core.Operaciones.Implements;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.Contratos
{
    public partial class AsignarEjeContrato : CatalogPage
    {
        private Contrato LastContrato
        {
            set { Session["lastContrato"] = value; }
            get { return Session["lastContrato"] != null ? (Contrato)Session["lastContrato"] : null; }
        }
        EjeTematico LastObject
        {
            get { return Session["lastTemaAsistencia"] != null ? Session["lastTemaAsistencia"] as EjeTematico : null; }
            set { Session["lastTemaAsistencia"] = value; }
        }
        DataSet DsEjeTematicoAsignado
         {
             get { return Session["DsEjeTematicoAsignado"] != null ? Session["DsEjeTematicoAsignado"] as DataSet : null; }
             set { Session["DsEjeTematicoAsignado"] = value; }
         }
        DataSet DsEjeTematicoDisponible
        {
            get { return Session["DsEjeTematicoDisponible"] != null ? Session["DsEjeTematicoDisponible"] as DataSet : null; }
            set { Session["DsEjeTematicoDisponible"] = value; }
        }
        private IRedirector redirector;
        private IUserSession userSession;
        private EjeTematicoCtrl ejeTematicoCtrl;
        private ContratoCtrl contratoCtrl;
        private DataSet dataSet;
        private Contrato contrato;
        protected int caracteresNombre;
        AreaProfesionalizacionCtrl AreaProfesionalizacionctrl;
        public AsignarEjeContrato()
        {
            redirector = new Redirector();
            userSession = new UserSession();
            ejeTematicoCtrl = new EjeTematicoCtrl();
            AreaProfesionalizacionctrl = new AreaProfesionalizacionCtrl();
            contratoCtrl = new ContratoCtrl();
            dataSet = new DataSet();
            dataSet.Tables.Add(new DataTable("EjeTematicoAsignados"));
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                caracteresNombre = 50;
                if (!IsPostBack)
                {
                    
                    if (LastContrato == null || LastContrato.ContratoID == null)
                        Response.Redirect("BuscarContrato.aspx",true);
                    contrato = LastContrato;
                    LastObject = null;
                    DsEjeTematicoAsignado = null;
                    DsEjeTematicoDisponible = null;
                    LoadDatosContrato();
                    DoSearchDisponibles();
                    DoSearchAsignados();
                }
            }
            catch (Exception ex)
            {

                Logger.Service.LoggerHlp.Default.Error(this, ex);
                redirector.GoToHomePage(true);
            }
        }
        protected void grdEjesTematicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "asignar":
                        EjeTematico ejeTematicoAdd = new EjeTematico {  EjeTematicoID = long.Parse(e.CommandArgument.ToString())};
                        DoAdd(ejeTematicoAdd);
                        break;
                    case "eliminar":
                        EjeTematico ejeTematicoDel = new EjeTematico { EjeTematicoID = long.Parse(e.CommandArgument.ToString()) };
                        DoDelete(ejeTematicoDel);
                        break;
                    case "ver":
                        break;
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void grdEjesTematicos1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            System.Data.DataRowView drv;
            drv = (System.Data.DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (RenderCreate)
                {
                    LinkButton btnCrear = (LinkButton)e.Row.FindControl("btnCrear");
                    btnCrear.Visible = true;
                }
            }
        }
        protected void grdEjesTematicos2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (RenderDelete)
                {
                    LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnEliminar");
                    btnDelete.Visible = true;
                }
            }
        }
        #region **********Eventos*********
        protected void BtnBuscarDisponibles_Click(object sender, EventArgs e)
        {
            try
            {
                DoSearchDisponibles();
            }
            catch (Exception ex)
            {

                Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        protected void BtnBuscarAsignados_Click(object sender, EventArgs e)
        {
            try
            {
                DoSearchAsignados();
            }
            catch (Exception ex)
            {

                Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        protected void BtnAsignarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                List<EjeTematico> ejes = DataToList(DsEjeTematicoAsignado);
                if (ejes != null)
                {
                    ejes = EjesNoAsignados(DsEjeTematicoDisponible, ejes);
                    AsignarTodos(ejes);
                }
            }
            catch (Exception ex)
            {

                Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw new Exception("Ocurrió un error al asignar TODOS los elementos");
            }
            
        }
        protected void BtnEliminarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                List<EjeTematico> ejes = DataToList(DsEjeTematicoAsignado);
                if (ejes != null)
                {
                    EliminarTodos(ejes);
                }
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw new Exception("Ocurrió un error al eliminar TODOS los elementos");
            }
        }
        #endregion
        #region *********Metodos**********
        private DataSet ConfigureGridResults(ProfesionalizacionContrato ejes)
        {
            DataSet ds = new DataSet();

            dataSet.Tables.Add(new DataTable());
            if (!dataSet.Tables[0].Columns.Contains("EjeTematicoID"))
                dataSet.Tables[0].Columns.Add(new DataColumn("EjeTematicoID", typeof(long)));
            if (!dataSet.Tables[0].Columns.Contains("Nombre"))
                dataSet.Tables[0].Columns.Add(new DataColumn("Nombre", typeof(string)));
            if (!dataSet.Tables[0].Columns.Contains("Materia"))
                dataSet.Tables[0].Columns.Add(new DataColumn("Materia", typeof(string)));
            if (!dataSet.Tables[0].Columns.Contains("Bloque"))
                dataSet.Tables[0].Columns.Add(new DataColumn("Bloque", typeof(string)));
            if (!dataSet.Tables[0].Columns.Contains("Nivel"))
                dataSet.Tables[0].Columns.Add(new DataColumn("Nivel", typeof(string)));
            if (!dataSet.Tables[0].Columns.Contains("Grado"))
                dataSet.Tables[0].Columns.Add(new DataColumn("Grado", typeof(byte)));

            foreach (EjeTematico ejeTematico in ejes.ListaEjesTematicos)
            {
                 DataRow row = dataSet.Tables[0].NewRow();
                 EjeTematico newEjeTematico = ejeTematicoCtrl.RetrieveComplete(
                        new DataContext((new DataProviderFactory()).GetDataProvider("POV")), ejeTematico);
                 row["EjeTematicoID"] = newEjeTematico.EjeTematicoID;
                 row["Nombre"] = newEjeTematico.Nombre;
                 row["Materia"] = newEjeTematico.AreaProfesionalizacion.Nombre;
                 row["Bloque"] = newEjeTematico.MateriasProfesionalizacion.FirstOrDefault().Nombre;
                 row["Nivel"] = newEjeTematico.AreaProfesionalizacion.NivelEducativo.Descripcion;
                 row["Grado"] = newEjeTematico.AreaProfesionalizacion.Grado;
                 dataSet.Tables[0].Rows.Add(row);
            }

            return dataSet;
        }
        private DataSet ConfigureGridResults(DataSet ds)
        {
            if (!ds.Tables[0].Columns.Contains("EjeTematicoID"))
                ds.Tables[0].Columns.Add(new DataColumn("EjeTematicoID", typeof(long)));
            
            if (!ds.Tables[0].Columns.Contains("Nombre"))
                ds.Tables[0].Columns.Add(new DataColumn("Nombre", typeof(string)));

            if (!ds.Tables[0].Columns.Contains("Materia"))
                ds.Tables[0].Columns.Add(new DataColumn("Materia", typeof(string)));

            if (!ds.Tables[0].Columns.Contains("Bloque"))
                ds.Tables[0].Columns.Add(new DataColumn("Bloque", typeof(string)));

            if (!ds.Tables[0].Columns.Contains("Nivel"))
                ds.Tables[0].Columns.Add(new DataColumn("Nivel", typeof(string)));
            
            if (!ds.Tables[0].Columns.Contains("Grado"))
                ds.Tables[0].Columns.Add(new DataColumn("Grado", typeof(byte)));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
               EjeTematico ejeTematico = ejeTematicoCtrl.DataRowToEjeTematico(row);
               EjeTematico newEjeTematico = ejeTematicoCtrl.RetrieveComplete(
                       new DataContext((new DataProviderFactory()).GetDataProvider("POV")), ejeTematico);
               row["EjeTematicoID"] = newEjeTematico.EjeTematicoID;
               row["Nombre"] = newEjeTematico.Nombre;
               row["Materia"] = newEjeTematico.AreaProfesionalizacion.Nombre;
               row["Bloque"] = newEjeTematico.MateriasProfesionalizacion.FirstOrDefault().Nombre;
               row["Nivel"] = newEjeTematico.AreaProfesionalizacion.NivelEducativo.Descripcion;
               row["Grado"] = newEjeTematico.AreaProfesionalizacion.Grado;
            }

            return ds;
        }
        //Busca los ejes tematicos ya asignados
        private void DoSearchAsignados()
        {
            EjeTematico ejeTematico = UserInterfaceToDataAsignados();
            if (ejeTematico.Nombre != null)
            {
                ejeTematico.Nombre = ejeTematico.Nombre.Trim();
                ejeTematico.Nombre = ejeTematico.Nombre.ToLower();
            }
            ProfesionalizacionContrato ejes = contratoCtrl.RetrieveProfesionalizacionContrato(new DataContext((new DataProviderFactory()).GetDataProvider("POV")), LastContrato);
            if (ejeTematico.Nombre != null)
            {
                ProfesionalizacionContrato proContra = new ProfesionalizacionContrato();
                //Buscamos concidencias con el nombre
                proContra.ListaEjesTematicos = ejes.ListaEjesTematicos.FindAll(
                    delegate(EjeTematico et)
                    {
                        return (et.Nombre.ToLower().Contains(ejeTematico.Nombre));
                    }
                );
                DsEjeTematicoAsignado = ConfigureGridResults(proContra);
            }
            else
            {
                DsEjeTematicoAsignado = ConfigureGridResults(ejes);
            }
            grdEjesTematicosAsignados.DataSource = DsEjeTematicoAsignado;
            grdEjesTematicosAsignados.DataBind();
        }
        //Busca los ejes tematicos disponibles para asignar
        private void DoSearchDisponibles()
        {
            EjeTematico ejeTematico = UserInterfaceToDataDisponibles();
            if (ejeTematico.Nombre != null)
            {
                ejeTematico.Nombre = ejeTematico.Nombre.Trim();
                ejeTematico.Nombre = "%" + ejeTematico.Nombre + "%";
            }
            DataSet ds = ejeTematicoCtrl.Retrieve(new DataContext((new DataProviderFactory()).GetDataProvider("POV")), ejeTematico);
            DsEjeTematicoDisponible = ConfigureGridResults(ds);
            grdEjesTematicosDisponibles.DataSource = DsEjeTematicoDisponible;
            grdEjesTematicosDisponibles.DataBind();
        }
        private EjeTematico UserInterfaceToDataDisponibles()
        {
            EjeTematico ejeTematico = new EjeTematico();
            ejeTematico.Nombre = !string.IsNullOrEmpty(txtNombreDisponible.Text.Trim()) ? txtNombreDisponible.Text.Trim() : null;
            ejeTematico.EstatusProfesionalizacion = EEstatusProfesionalizacion.ACTIVO;
            return ejeTematico;
        }
        private EjeTematico UserInterfaceToDataAsignados()
        {
            EjeTematico ejeTematico = new EjeTematico();
            ejeTematico.Nombre = !string.IsNullOrEmpty(txtNombreAsignado.Text.Trim()) ? txtNombreAsignado.Text.Trim() : null;
            return ejeTematico;
        }
        //Carga los datos del contrato para mostrar en UI
        private void LoadDatosContrato()
        {
            contrato = LastContrato;
            contrato = contratoCtrl.RetrieveComplete(new DataContext((new DataProviderFactory()).GetDataProvider("POV")),contrato);
            if (contrato != null)
            {
                txtClave.Text = contrato.Clave;
                txtFechaInicio.Text =  string.Format("{0:d}",contrato.InicioContrato.Value);
                txtFechaFin.Text = string.Format("{0:d}",contrato.FinContrato.Value);
            }
        }
        private void DoAdd(EjeTematico eje)
        {
            contratoCtrl.InsertProfesionalizacionContrato(new DataContext((new DataProviderFactory()).GetDataProvider("POV")), LastContrato, eje);
            this.ShowMessage(" El eje o ámbito se ha asignado exitosamente al contrato", MessageType.Information);
                this.DoSearchAsignados();
        }
        private void DoDelete(EjeTematico eje)
        {
            try
            {
                contratoCtrl.DeleteProfesionalizacionContrato(new DataContext((new DataProviderFactory()).GetDataProvider("POV")), LastContrato, eje);
                this.ShowMessage("El eje o ámbito se ha eliminado exitosamente del contrato", MessageType.Information);
                this.DoSearchAsignados();
                
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw new Exception("Ocurrió un error al eliminar la asignacion");
            }
        }
        private void AsignarTodos(List<EjeTematico> listaEjes)
        {
           foreach(EjeTematico eje in listaEjes){
               contratoCtrl.InsertProfesionalizacionContrato(new DataContext((new DataProviderFactory()).GetDataProvider("POV")), LastContrato, eje);
           }
           this.DoSearchAsignados();
           this.ShowMessage(" Todos los ejes o ámbitos han sido asignados exitosamente al contrato", MessageType.Information);
        }
        private void EliminarTodos(List<EjeTematico> listaEjes)
        {
            foreach (EjeTematico eje in listaEjes)
            {
                contratoCtrl.DeleteProfesionalizacionContrato(new DataContext((new DataProviderFactory()).GetDataProvider("POV")), LastContrato, eje);
            }
            DsEjeTematicoAsignado = null;
            DoSearchAsignados();
            this.ShowMessage(" Todos los ejes o ámbitos han sido eliminados del contrato", MessageType.Information);
            
        }
        //Convierte Un data set a una lista de longs
        private List<EjeTematico> DataToList(DataSet ds)
        {
            List<EjeTematico> lista = new List<EjeTematico>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                EjeTematico eje = new EjeTematico();
                eje.EjeTematicoID = Convert.ToInt64(row["EjeTematicoID"]);
                lista.Add(eje);
            }
            return lista;
        }
        //determina que ejes no han sido asignado, para la asignacion masiva
        private List<EjeTematico> EjesNoAsignados(DataSet ds,List<EjeTematico> listaEjes)
        {
            List<EjeTematico> lista = new List<EjeTematico>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row["EjeTematicoID"] != null)
                {
                    EjeTematico eje = listaEjes.FirstOrDefault(e => e.EjeTematicoID == Convert.ToInt64(row["EjeTematicoID"]));
                    if (eje == null)
                    {
                        EjeTematico ejeNuevo = new EjeTematico();
                        ejeNuevo.EjeTematicoID = Convert.ToInt64(row["EjeTematicoID"]);
                        lista.Add(ejeNuevo);
                    }
                }
            }
            return lista;
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
        private bool RenderCreate = false;
        private bool RenderDelete = false;
        protected override void DisplayCreateAction()
        {
            btnAgregar.Visible = true;
            RenderCreate = true;
        }

        protected override void DisplayReadAction()
        {
            grdEjesTematicosAsignados.Visible = true;
            grdEjesTematicosDisponibles.Visible = true;
        }

        protected override void DisplayDeleteAction()
        {
            btnDelete.Visible = true;
            RenderDelete = true;
        }
        protected override void DisplayUpdateAction()
        {
            btnAgregar.Visible = true;
        }
        protected override void AuthorizeUser()
        {

            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCONTRATO) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARCONTRATOS) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARTEMAASISTENCIA) != null;
            bool create = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARCONTRATOS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCONTRATOS) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (lectura)
                DisplayReadAction();

            if (delete)
                DisplayDeleteAction();
            if (create)
                DisplayCreateAction();
            if (edit)
                DisplayUpdateAction();
        }
        #endregion
    }
}