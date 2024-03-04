using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using POV.AppCode.Page;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.ConfiguracionActividades.BO;
using POV.Content.MasterPages;
using POV.Core.HerramientasDocente.Implement;
using POV.Core.HerramientasDocente.Interfaces;
using POV.Licencias.BO;
using POV.Profesionalizacion.BO;
using POV.ServiciosActividades.Controllers;
using POV.Web.Helper;
using System.IO;
using POV.Comun.BO;
using POV.Comun.Service;
using System.Configuration;
using POV.Web.ServiciosActividades.Controllers;
using POV.Reactivos.BO;
using POV.Modelo.Service;
using POV.Modelo.DAO;
using POV.Modelo.BO;
using System.Collections;
using POV.Licencias.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Expediente.Service;
using POV.Expediente.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;

namespace POV.Web.HerramientasDocente.Pages.Actividades
{

    public partial class CrearActividadUI : PageBase
    {
        private const string CMD_ADD_PRUEBA = "CMD_ADD_PRUEBA";
        private const string CMD_ADD_EJE = "CMD_ADD_EJE";
        private const string CMD_ADD_REACTIVO = "CMD_ADD_REACTIVO";
        
        private ConsultarEjesTematicosController _controllerEjesTematicosController;
        private CrearActividadDocenteController _controllerCrearActividadDocente;
        private ConsultarAlumnosGrupoController _controllerConsultarAlumnosGrupo;
        private AsignarActividadGrupoController _controllerAsignarActividadGrupo;
        private ModeloCtrl modeloCtrl;
        private PruebaDinamicaCtrl pruebaDinamicaCtrl;

        // Areas de conocimiento   
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        #region session

        IUserSession userSession = new UserSession();

        private List<Tarea> Session_TareasAAsignar
        {
            get { return (List<Tarea>)Session["Session_TareasAAsignar"]; }
            set { Session["Session_TareasAAsignar"] = value; }
        }

        private List<GrupoCicloEscolar> Session_GruposCicloEscolar
        {
            get
            {
                if (Session["GruposCicloEscolar"] != null)
                    return Session["GruposCicloEscolar"] as List<GrupoCicloEscolar>;

                return null;
            }
            set
            {
                Session["GruposCicloEscolar"] = value;
            }
        }

        public DataSet DsClasificadores
        {
            set { Session["clasificadores"] = value; }
            get { return Session["clasificadores"] != null ? Session["clasificadores"] as DataSet : null; }
        }

        AModelo LastObject
        {
            get { return Session["lastModelo"] != null ? Session["lastModelo"] as ModeloDinamico : null; }
            set { Session["lastModelo"] = value; }
        }
       
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //TODO: Validar si el usuario ya inicio sesion

                _controllerEjesTematicosController = new ConsultarEjesTematicosController();
                _controllerCrearActividadDocente = new CrearActividadDocenteController();
                _controllerConsultarAlumnosGrupo = new ConsultarAlumnosGrupoController();
                _controllerAsignarActividadGrupo = new AsignarActividadGrupoController();
                modeloCtrl = new ModeloCtrl();
                pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
           
                if (!IsPostBack)
                {
                    //TODO: Validar si es especialista, tiene escuela y si tiene un contrato
                    TipoServicioCtrl tipoServicioCtrl = new TipoServicioCtrl();
                    userSession.CurrentEscuela.TipoServicio = tipoServicioCtrl.RetriveComplete(dctx, userSession.CurrentEscuela.TipoServicio);

                    loadGrados(userSession.CurrentEscuela.TipoServicio.NivelEducativoID);
                    // primera vista
                    mtvTareas.SetActiveView(viewAgregarContenido);
                    Session_TareasAAsignar = new List<Tarea>();
                    gvTareasActividad.DataSource = Session_TareasAAsignar;
                    gvTareasActividad.DataBind();
                    Session["ListaGruposSeleccionados"] = null;
                    LoadAreasConocimiento();
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
                ShowMessage("Problemas al cargar la página", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        #region CG, LA
        private void LoadAreasConocimiento() 
        {
            ddlArea.DataSource = GetAreasConocimientoAlumno();
            ddlArea.DataValueField = "ClasificadorID";
            ddlArea.DataTextField = "Nombre";
            ddlArea.DataBind();
        }

        private DataSet GetAreasConocimientoAlumno()
        {

            ArrayList arrAreaConocimiento = new ArrayList();
            Escuela escuela = userSession.CurrentEscuela;
            CicloEscolar cicloEscolar = userSession.CurrentCicloEscolar;
            Contrato contrato = userSession.Contrato;

            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = cicloEscolar });
            PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);

            var pruebaDinamica = (PruebaDinamica)pruebaPivoteContrato.Prueba;
            PruebaDinamica pruebaDinamicaModelo = pruebaDinamicaCtrl.RetrieveComplete(dctx, pruebaDinamica, false);
            DataSet DsClasificadores = modeloCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, new ModeloDinamico { ModeloID = pruebaDinamicaModelo.Modelo.ModeloID });

            return DsClasificadores;
        }
        #endregion

        #region GrupoCicloEscolar
        /// <summary>
        /// RowDataBound del Grid de grupos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvGrupos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    string grupoCicloEscolarID = e.Row.Cells[0].Text;
                    CheckBox cbSeleccionado = (CheckBox)e.Row.FindControl("cbSeleccionado");
                    cbSeleccionado.Attributes.Add("ClasificadorID", grupoCicloEscolarID);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error al llenar el grid de la consulta", Content.MasterPages.Site.EMessageType.Error);
            }
        }

        #region MÉTODOS

        /// <summary>
        /// Mantiene el seleccionado del grid
        /// </summary>
        /// <param name="grid"></param>
        public void KeepSelection(GridView grid)
        {
            // se obtienen los id de las notificaciones checkeadas de la pagina actual
            List<GridViewRow> checkedRows = (from item in grid.Rows.Cast<GridViewRow>()
                                             let check = (CheckBox)item.FindControl("cbSeleccionado")
                                             where check.Checked
                                             select item).ToList();

            foreach (var gridViewRow in checkedRows)
            {
                var dataKey = grid.DataKeys[gridViewRow.RowIndex];
            }

            // se obtienen los id de las notificaciones checkeadas de la pagina actual
            List<string> checkedGrupos = (from item in grid.Rows.Cast<GridViewRow>()
                                          let check = (CheckBox)item.FindControl("cbSeleccionado")
                                          where check.Checked
                                          select Convert.ToString(grid.DataKeys[item.RowIndex].Value)).ToList();

            //
            // se recupera de session la lista de seleccionados previamente
            //
            List<string> listaGruposSeleccionados = Session["ListaGruposSeleccionados"] as List<string> ?? new List<string>();

            //
            // se cruzan todos los registros de la pagina actual del gridview con la lista de seleccionados,
            // si algun item de esa pagina fue marcado previamente no se devuelve
            //
            listaGruposSeleccionados = (from item in listaGruposSeleccionados
                                        join item2 in grid.Rows.Cast<GridViewRow>()
                                           on item equals Convert.ToString(grid.DataKeys[item2.RowIndex].Value) into g
                                        where !g.Any()
                                        select item).ToList();

            //
            // se agregan los seleccionados
            //
            listaGruposSeleccionados.AddRange(checkedGrupos);
            Session["ListaGruposSeleccionados"] = listaGruposSeleccionados;


        }

        /// <summary>
        /// Consulta la información de los grupos dependiendo la escuela con la que inicies sesion
        /// </summary>
        public void ConsultarGrupos()
        {

        }

        #endregion

        #endregion

        #region

        #region PopulateData
        /// <summary>
        /// Carga El estatus del Reactivo: ACTIVO/INACTIVO
        /// </summary>
        public void LoadEstatusReactivoDocente()
        {

        }
        #endregion

        #region InterfaceToData
        /// <summary>
        /// Obtiene el objeto Reactivo de la UI de Consulta
        /// </summary>
        /// <returns></returns>
        public ReactivoDocente InterfaceToDataReactivoDocente()
        {
            var reactivo = new ReactivoDocente();
            reactivo.TipoReactivo = ETipoReactivo.Estandarizado;
            reactivo.Activo = true;
            reactivo.Docente = userSession.CurrentDocente;
            return reactivo;
        }


        #endregion

        #endregion
        #region

        protected void btnBuscarPruebas_OnClick(object sender, EventArgs e)
        {

        }
        #endregion

        #region
        #region Events
        
        protected void cbGradoAsignatura_SelectedIndexChanged(object sender, EventArgs e)
        {
            NivelEducativo nivelEducativo = userSession.CurrentEscuela.TipoServicio.NivelEducativoID;
            byte? grado = GetGradoFromUI();
            if (grado != null)
            {
                LoadAsignaturas(_controllerEjesTematicosController.ConsultarAreaProfesionalizacion(
                    new AreaProfesionalizacion() { NivelEducativo = nivelEducativo, Grado = grado, Activo = true }));
            }
        }

        #endregion

        private byte? GetGradoFromUI()
        {
            byte grado = 0;


            if (grado > 0)
                return grado;

            return null;
        }


        #region Populate Methods
        private void loadGrados(NivelEducativo nivelEducativo)
        {
            if (nivelEducativo.NumeroGrados != null)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(new DataTable());
                ds.Tables[0].Columns.Add("Grado", typeof(string));
                ds.Tables[0].Columns.Add("Nombre", typeof(string));

                DataRow row = ds.Tables[0].NewRow();
                row["Grado"] = DBNull.Value;
                row["Nombre"] = "TODOS";
                ds.Tables[0].Rows.InsertAt(row, 0);

                for (int i = 1; i <= nivelEducativo.NumeroGrados.Value; i++)
                {
                    DataRow newRow = ds.Tables[0].NewRow();
                    newRow["Grado"] = i.ToString();
                    newRow["Nombre"] = i + "°";
                    ds.Tables[0].Rows.Add(newRow);
                }
            }
        }
        private void LoadAsignaturas(DataSet dsAreas)
        {
            if (dsAreas.Tables.Count > 0 && dsAreas.Tables[0].Rows.Count > 0)
            {
                DataRow row = dsAreas.Tables[0].NewRow();
                row["AreaProfesionalizacionID"] = DBNull.Value;
                row["Nombre"] = "TODOS";
                dsAreas.Tables[0].Rows.InsertAt(row, 0);
            }
        }

        private void LoadBloques(List<MateriaProfesionalizacion> listMaterias)
        {
            var item = new ListItem("TODOS", "") { Selected = true };
        }

        #endregion 
        #endregion

        #region
        /// <summary>
        /// Evento que controla crear la actividad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrearActividad_OnClick(object sender, EventArgs e)
        {
            DoInsert();
        }

        /// <summary>
        /// Evento que controla la cancelación de la captura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            LimpiarCampos();
            Response.Redirect("MantenerActividadesUI.aspx", true);
        }

        /// <summary>
        /// Evento que controla cuando un usuario cambia de pestaña de busqueda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void changeView_OnClick(object sender, EventArgs e)
        {
            LinkButton btnClicked = (LinkButton)sender;
            hdnDialogResultado.Value = "";
            switch (btnClicked.ID)
            {
                case "lnkBtnAgregarcontenido":
                    mtvTareas.SetActiveView(viewAgregarContenido);
                    hdnTabSeleccionado.Value = "td-subir";
                    break;
            }
        }

        /// <summary>
        /// Evento que controla cuando se agrega las filas a la lista de tareas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTareasActividad_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var rowView = (Tarea)e.Row.DataItem;
                    var tipoTarea = rowView.GetTypeDescription();
                    var lblTipoTarea = (Label)e.Row.FindControl("lblTipoTarea");
                    lblTipoTarea.Text = tipoTarea;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("No fue posible llenar la lista de tareas: " + ex.InnerException.Message, Content.MasterPages.Site.EMessageType.Error);
            }
        }

        /// <summary>
        /// Evento que controla cuando un usuario selecciona un acción de la lista de tareas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTareasActividad_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            switch (e.CommandName.Trim())
            {
                case "quitar":
                    {
                        int index = -1;
                        if (int.TryParse(e.CommandArgument.ToString(), out index))
                        {

                            RemoveTareaFromList(index);
                        }
                        break;
                    }

                case "Sort":
                    {
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", Content.MasterPages.Site.EMessageType.Error); break; }

            }
        }

        /// <summary>
        /// Quita una tarea de la lista de tareas con base a un indice
        /// </summary>
        /// <param name="index">indice de la tarea que se quiere quitar de la lista</param>
        private void RemoveTareaFromList(int index)
        {
            if (Session_TareasAAsignar.ElementAtOrDefault(index) != null)
            {
                Session_TareasAAsignar.RemoveAt(index);
                gvTareasActividad.DataSource = Session_TareasAAsignar;
                gvTareasActividad.DataBind();
            }
        }
        
        /// <summary>
        /// Agrega la lista de elementos consultados de las ventanas auxiliares de consulta y
        /// crea una Tarea especifica para ser agregada a la lista de tareas de la Actividad
        /// </summary>
        /// <param name="comandoActividad"></param>
        /// <param name="instruccion"></param>
        public void AgregarTareasFromConsulta(string comandoActividad, string instruccion)
        {
            gvTareasActividad.DataSource = Session_TareasAAsignar;
            gvTareasActividad.DataBind();
        }

        /// <summary>
        /// Agrega una tarea registrada al gridview de las tareas
        /// </summary>
        /// <param name="comandoActividad"></param>
        /// <param name="tarea"></param>
        public void AgregarTareasFromRegistro(string comandoActividad, Tarea tarea)
        {
            Session_TareasAAsignar.Add(tarea);

            gvTareasActividad.DataSource = Session_TareasAAsignar;
            gvTareasActividad.DataBind();
        }

        /// <summary>
        /// Metodo que controla la inserción de la actividad en la base de datos
        /// </summary>
        private void DoInsert()
        {
            try
            {
                string sError = ValidateFields();

                if (string.IsNullOrEmpty(sError))
                {
                    string servidorContenidos = @ConfigurationManager.AppSettings["POVURLServidorContenidos"];
                    string metodoServidor = @ConfigurationManager.AppSettings["POVMetodoServidorContenidos"];
                    string userServidor = @ConfigurationManager.AppSettings["POVUserServidorContenidos"];
                    string passervidor = @ConfigurationManager.AppSettings["POVPassServidorContenidos"];
                    //si el metodo es local, mapeamos la ubicacion en el servidor
                    if (metodoServidor.ToUpper().CompareTo("LOCAL") == 0)
                        servidorContenidos = Server.MapPath(servidorContenidos);
                    else if (metodoServidor.ToUpper().CompareTo("AMAZONS3") == 0)
                    {
                        userServidor = @ConfigurationManager.AppSettings["AWSAccessKey"];
                        passervidor = @ConfigurationManager.AppSettings["AWSSecretKey"];
                        servidorContenidos = @ConfigurationManager.AppSettings["BucketName"];
                    }
                    

                    ActividadDocente actividadDocente = GetActividadDocenteFromUI();
                    

                    List<FileWrapper> fileWrappers = CrearListaFileWrapper(actividadDocente);


                    _controllerCrearActividadDocente.InsertActividadDocente(actividadDocente, fileWrappers, servidorContenidos, userServidor, passervidor, metodoServidor);
                    EliminarArchivosTemporales(fileWrappers);
                    ShowMessage("La actividad se registró correctamente", Content.MasterPages.Site.EMessageType.Information);

                    LimpiarCampos();

                    Response.Redirect("MantenerActividadesUI.aspx", true);
                }
                else
                {
                    ShowMessage("<div>Error:" + sError + "</div>", Content.MasterPages.Site.EMessageType.Error);
                }
                
            }
            catch (Exception ex)
            {
                ShowMessage("Error inesperado " + ex.Message, Content.MasterPages.Site.EMessageType.Error);
            }
        }

        /// <summary>
        /// Método para comparar las fechas que se quieren asignar
        /// </summary>
        /// <returns>Mensaje del resultdo</returns>
        private String ValidateDates(string strFechaInicio, string strFechaFin)
        {
            var mensaje = String.Empty;
            var dtFechaInicio = new DateTime();
            var dtFechaFin = new DateTime();

            if (!String.IsNullOrEmpty(strFechaInicio.Trim()))
                dtFechaInicio = DateTime.ParseExact(strFechaInicio.Trim() + " 00:00", "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture);
            if (!String.IsNullOrEmpty(strFechaFin.Trim()))
                dtFechaFin = DateTime.ParseExact(strFechaFin.Trim() + " 23:59", "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture);
            return mensaje;
        }

       
        /// <summary>
        /// Metodo auxiliar que valida la información de captura de la interfaz
        /// </summary>
        /// <returns>Cadena que indica los errores encontrados, en caso contrario una cadena vacia</returns>
        private string ValidateFields()
        {
            string sError = string.Empty;
            string nombre = txtNombreActividad.Text.Trim();
            if (string.IsNullOrEmpty(nombre))
                sError += " El nombre de actividad es requerido.";
            else if (nombre.Length > 100)
                sError += " El nombre de la actividad debe ser menor de 100 caracteres.";

            string descripcion = txtDescripcionActividad.Text.Trim();
            if (string.IsNullOrEmpty(descripcion))
                sError += " Las instrucciones para el alumno son requeridas.";
            else if (descripcion.Length > 500)
                sError += " Las instrucciones para el alumno deben ser menores de 500 caracteres.";

            if (!Session_TareasAAsignar.Any())
                sError += " Se debe asignar al menos una tarea a la actividad.";
            
            return sError;
        }

        /// <summary>
        /// Obtiene y crea un objeto Actividad de la UI
        /// </summary>
        /// <returns>Actividad captura a traves de la UI</returns>
        private ActividadDocente GetActividadDocenteFromUI()
        {
            ActividadDocente actividadDocente = new ActividadDocente();
            actividadDocente.Nombre = txtNombreActividad.Text.Trim();
            actividadDocente.Activo = true;
            actividadDocente.Descripcion = txtDescripcionActividad.Text.Trim();
            actividadDocente.EscuelaId = userSession.CurrentEscuela.EscuelaID;
            actividadDocente.DocenteId = userSession.CurrentDocente.DocenteID;
            actividadDocente.FechaCreacion = DateTime.Now;
            actividadDocente.Tareas = Session_TareasAAsignar;
            actividadDocente.ClasificadorID = int.Parse(ddlArea.SelectedValue);
            actividadDocente.UsuarioId = userSession.CurrentUser.UsuarioID;

            return actividadDocente;
        }

        /// <summary>
        /// Metodo auxiliar que limpia los campos
        /// </summary>
        private void LimpiarCampos()
        {
            txtNombreActividad.Text = "";
            txtDescripcionActividad.Text = "";
            Session_TareasAAsignar = new List<Tarea>();
            hdnTabSeleccionado.Value = "td-juegos";
            hdnDialogResultado.Value = "";
            gvTareasActividad.DataSource = Session_TareasAAsignar;
            gvTareasActividad.DataBind();
            this.ConsultarGrupos();
        }
        #endregion

        #region ajuste agregar contenido digital
        protected void btnAgrarContenido_OnClick(object sender, EventArgs evt)
        {
            AgregarContenidoUC.IniciarRegistro();
            hdnDialogResultado.Value = "AgregarContenidoDigital";
        }

        private List<FileWrapper> CrearListaFileWrapper(ActividadDocente actividad)
        {
            List<FileWrapper> lista = new List<FileWrapper>();

            var tareasContenido = actividad.Tareas.Where(item => item is TareaContenidoDigital).ToList();

            foreach (Tarea t in tareasContenido)
            {
                if ((t as TareaContenidoDigital).ContenidoDigital.EsInterno.Value)
                {
                    var contenido = (t as TareaContenidoDigital).ContenidoDigital.ListaURLContenido.First();
                    lista.Add(CrearFileWrapper(contenido.URL, contenido.Nombre));
                }
            }
            
            return lista;
        }

        /// <summary>
        /// Crea un FileWrapper a partir de un archivo
        /// </summary>
        /// <param name="filename">nombre del archivo</param>
        /// <returns>FileWrapper</returns>
        private FileWrapper CrearFileWrapper(string filename, string contentType)
        {
            FileWrapper file = new FileWrapper();

            string path = Server.MapPath(@System.Configuration.ConfigurationManager.AppSettings["RUTA_CONTENIDOS_TEMP"]) + filename;

            FileStream fileStream = File.OpenRead(path);

            Byte[] arreglo = new Byte[fileStream.Length];
            BinaryReader reader = new BinaryReader(fileStream);
            arreglo = reader.ReadBytes(Convert.ToInt32(fileStream.Length));

            file.Data = arreglo;
            file.Lenght = Convert.ToInt32(fileStream.Length);

            string fName = string.Empty;
            string carpetaImages = @System.Configuration.ConfigurationManager.AppSettings["POVRutaContenidosDocente"];
            fName = carpetaImages + filename;


            file.Name = fName;
            file.Type = contentType;

            fileStream.Close();

            return file;
        }
        /// <summary>
        /// Elimina del sistema los archivos de imagenes que se crearon de manera temporal
        /// </summary>
        /// <param name="files">Lista de archivos que se eliminaran en el disco del sistema</param>
        private void EliminarArchivosTemporales(List<FileWrapper> files = null)
        {
            if (files != null)
            {
                FileManagerCtrl fileManager = new FileManagerCtrl();
                foreach (FileWrapper file in files)
                {
                    fileManager.DeleteFile(Server.MapPath(@ConfigurationManager.AppSettings["RUTA_CONTENIDOS_TEMP"]) + file.Name);
                }
            }

            EliminarArchivosAntiguos();
        }
        /// <summary>
        /// Elimina los archivos temporales antiguos de la carpeta de archivos temporal
        /// </summary>
        private void EliminarArchivosAntiguos()
        {
            FileManagerCtrl fileManager = new FileManagerCtrl();
            //tiempo 4 horas en segundos
            fileManager.DeleteOldFilesDirectory(Server.MapPath(@ConfigurationManager.AppSettings["RUTA_CONTENIDOS_TEMP"]), 14400);
        }
        #endregion
        
        private void ShowMessage(string message, Site.EMessageType messageType)
        {
            Site site = (Site)Page.Master;
            site.ShowMessage(message, messageType);
        }



        protected override void AuthorizeUser()
        {
            
        }

    }
}