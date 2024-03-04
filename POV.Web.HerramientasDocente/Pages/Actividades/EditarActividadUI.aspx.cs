using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using POV.AppCode.Page;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.ConfiguracionActividades.BO;
using POV.Content.MasterPages;
using POV.Licencias.BO;
using POV.Profesionalizacion.BO;
using POV.Reactivos.BO;
using POV.ServiciosActividades.Controllers;
using POV.Web.Helper;
using System.Collections;
using POV.Licencias.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Expediente.Service;
using POV.Expediente.BO;
using POV.Modelo.Service;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Modelo.BO;

namespace POV.Web.HerramientasDocente.Pages.Actividades
{

    public partial class EditarActividadUI : PageBase
    {

        private const string CMD_ADD_PRUEBA = "CMD_ADD_PRUEBA";
        private const string CMD_ADD_EJE = "CMD_ADD_EJE";
        private const string CMD_ADD_REACTIVO = "CMD_ADD_REACTIVO";

        private ConsultarEjesTematicosController _controllerEjesTematicosController;
        private MantenerActividadesDocenteController _controllerMantenerActividad;
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private ModeloCtrl modeloCtrl;
        private PruebaDinamicaCtrl pruebaDinamicaCtrl;
        #region session

        private List<EjeTematicoRef> Session_EjesTematicosAAsignar
        {
            get { return (List<EjeTematicoRef>)Session["EjesAAsignar"]; }
            set { Session["EjesAAsignar"] = value; }
        }
        private ActividadDocente Session_ActividadActual
        {
            get { return (ActividadDocente)Session["Session_ActividadActual"]; }
            set { Session["Session_ActividadActual"] = value; }
        }
        private List<Tarea> Session_TareasAAsignar
        {
            get { return (List<Tarea>)Session["Session_TareasAAsignar_Edit"]; }
            set { Session["Session_TareasAAsignar_Edit"] = value; }
        }
        private List<ReactivoDocente> Session_ReactivoParaAsignar
        {
            get { return Session["ReactivoParaAsignar"] as List<ReactivoDocente>; }
            set { Session["ReactivoParaAsignar"] = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //TODO: Validar si el usuario ya inicio sesion

                _controllerEjesTematicosController = new ConsultarEjesTematicosController();
                modeloCtrl = new ModeloCtrl();
                pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
                if (!IsPostBack)
                {

                    if (Session_ActividadActual != null && Session_ActividadActual.ActividadID != null)
                    {

                        _controllerMantenerActividad = new MantenerActividadesDocenteController();
                        Session_EjesTematicosAAsignar = null;
                        Session_ReactivoParaAsignar = null;
                        Session_TareasAAsignar = null;

                        TipoServicioCtrl tipoServicioCtrl = new TipoServicioCtrl();
                        userSession.CurrentEscuela.TipoServicio = tipoServicioCtrl.RetriveComplete(dctx, userSession.CurrentEscuela.TipoServicio);
                        LoadAreasConocimiento();
                        mtvTareas.SetActiveView(viewAgregarContenido);
                        LoadActividad();
                    }
                    else
                    {
                        Response.Redirect("MantenerActividadesUI.aspx");
                    }
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

        #region Events

        private void clearDropDownList(DropDownList ddl)
        {
            ListItem item = new ListItem("TODOS", "");
            ddl.Items.Clear();
            ddl.Items.Add(item);
        }

        /// <summary>
        /// Evento que controla crear la actividad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarActividad_OnClick(object sender, EventArgs e)
        {
            DoUpdate();
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
        /// Evento que controla la cancelación de la captura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {

            LimpiarCampos();
            Response.Redirect("MantenerActividadesUI.aspx");
        }

        /// <summary>
        /// Quita una tarea de la lista de tareas con base a un indice
        /// </summary>
        /// <param name="index">indice de la tarea que se quiere quitar de la lista</param>
        private void RemoveTareaFromList(int index)
        {
            var tarea = Session_TareasAAsignar.ElementAtOrDefault(index);

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
            if (comandoActividad == CMD_ADD_EJE)
            {
                Session_EjesTematicosAAsignar.ForEach(j => Session_TareasAAsignar.Add(new TareaEjeTematico
                {
                    EjeTematicoId = j.EjeTematicoId,
                    Instruccion = instruccion,
                    ContenidoDigitalId = j.ContenidoDigitalId,
                    Nombre = j.NombreContenido
                }));
                Session_EjesTematicosAAsignar = null;
            }
            else if (comandoActividad == CMD_ADD_REACTIVO)
            {
                Session_ReactivoParaAsignar.ForEach(r => Session_TareasAAsignar.Add(new TareaReactivo
                {
                    ReactivoId = r.ReactivoID,
                    Instruccion = instruccion,
                    Nombre = r.NombreReactivo
                }));
                Session_ReactivoParaAsignar = null;
            }

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
        /// Metodo que controla la actualización de la actividad en la base de datos
        /// </summary>
        private void DoUpdate()
        {
            try
            {
                if (Session_ActividadActual == null)
                    return;
                if (Session_ActividadActual.ActividadID.ToString() != hdnControlActividadId.Value)
                {
                    ShowMessage("La actividad no corresponde con la seleccionada para su edición, por favor refresque la página.", Content.MasterPages.Site.EMessageType.Error);
                    LimpiarCampos();
                    txtRedirect.Value = "MantenerActividadesUI.aspx";
                    return;
                }

                if (new MantenerActividadesDocenteController().EsActividadAsignada(new ActividadDocente { ActividadID = Session_ActividadActual.ActividadID, EscuelaId = Session_ActividadActual.EscuelaId, DocenteId = userSession.CurrentDocente.DocenteID }))
                {
                    ShowMessage("No se puede editar una actividad asignada a un alumno.", Content.MasterPages.Site.EMessageType.Error);
                    LimpiarCampos();
                    txtRedirect.Value = "MantenerActividadesUI.aspx";
                    return;
                }

                string sError = ValidateFields();

                if (string.IsNullOrEmpty(sError))
                {
                    using (_controllerMantenerActividad = new MantenerActividadesDocenteController())
                    {
                        string servidorContenidos = @ConfigurationManager.AppSettings["POVURLServidorContenidos"];
                        string metodoServidor = @ConfigurationManager.AppSettings["POVMetodoServidorContenidos"];
                        string userServidor = @ConfigurationManager.AppSettings["POVUserServidorContenidos"];
                        string passervidor = @ConfigurationManager.AppSettings["POVPassServidorContenidos"];
                        string rutaContenidos = @ConfigurationManager.AppSettings["POVRutaContenidosDocente"];
                        //si el metodo es local, mapeamos la ubicacion en el servidor
                        if (metodoServidor.ToUpper().CompareTo("LOCAL") == 0)
                            servidorContenidos = Server.MapPath(servidorContenidos);
                        else if (metodoServidor.ToUpper().CompareTo("AMAZONS3") == 0)
                        {
                            userServidor = @ConfigurationManager.AppSettings["AWSAccessKey"];
                            passervidor = @ConfigurationManager.AppSettings["AWSSecretKey"];
                            servidorContenidos = @ConfigurationManager.AppSettings["BucketName"];
                        }
                        //
                        ActividadDocente actividadDocente = _controllerMantenerActividad.RetrieveActividad(new ActividadDocente { ActividadID = Session_ActividadActual.ActividadID });

                        actividadDocente = GetActividadDocenteFromUI(actividadDocente);

                        List<long> tareasEliminadas = new List<long>();
                        foreach (Tarea tareaBD in actividadDocente.Tareas)
                        {
                            if (!Session_TareasAAsignar.Exists(sbt => sbt.TareaId == tareaBD.TareaId))
                                tareasEliminadas.Add(tareaBD.TareaId.Value);
                        }
                        actividadDocente.Tareas.AddRange(Session_TareasAAsignar.Where(t => t.TareaId == null).ToList());


                        List<FileWrapper> fileWrappers = new List<FileWrapper>();
                        fileWrappers = CrearListaFileWrapper(Session_TareasAAsignar.Where(t => t.TareaId == null).ToList());

                        //hacer el update pasando fileWrappers como parámetro
                        _controllerMantenerActividad.UpdateActividad(actividadDocente, tareasEliminadas, fileWrappers, servidorContenidos, userServidor, passervidor, metodoServidor, rutaContenidos);

                        EliminarArchivosTemporales(fileWrappers);
                        Response.Redirect("MantenerActividadesUI.aspx");

                    }

                }
                else
                {
                    ShowMessage("<div>Error: <ul>" + sError + "</ul></div>", Content.MasterPages.Site.EMessageType.Error);
                }

            }
            catch (Exception ex)
            {
                ShowMessage("Error inesperado " + ex.InnerException.Message, Content.MasterPages.Site.EMessageType.Error);
            }
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
                sError += "<li>El nombre de actividad es requerido</li>";
            else if (nombre.Length > 100)
                sError += "<li>El nombre de la actividad debe ser menor de 100 caracteres</li>";

            string descripcion = txtDescripcionActividad.Text.Trim();
            if (string.IsNullOrEmpty(descripcion))
                sError += "<li>Las instrucciones para el alumno son requeridas</li>";
            else if (descripcion.Length > 500)
                sError += "<li>Las instrucciones para el alumno deben ser menores de 500 caracteres</li>";

            if (!Session_TareasAAsignar.Any())
                sError += "<li>Se debe asignar al menos una tarea a la actividad</li>";
            return sError;
        }

        /// <summary>
        /// Obtiene y crea un objeto Actividad de la UI
        /// </summary>
        /// <returns>Actividad captura a traves de la UI</returns>
        private ActividadDocente GetActividadDocenteFromUI(ActividadDocente actividad)
        {
            actividad.Nombre = txtNombreActividad.Text.Trim();
            actividad.Descripcion = txtDescripcionActividad.Text.Trim();
            actividad.DocenteId = userSession.CurrentDocente.DocenteID;
            actividad.ClasificadorID = int.Parse(ddlArea.SelectedValue);
            actividad.UsuarioId = userSession.CurrentUser.UsuarioID;
            //actividad.Tareas = Session_TareasAAsignar;
            return actividad;
        }

        /// <summary>
        /// Metodo auxiliar que limpia los campos
        /// </summary>
        private void LimpiarCampos()
        {
            txtNombreActividad.Text = "";
            txtDescripcionActividad.Text = "";
            Session_EjesTematicosAAsignar = null;
            Session_ReactivoParaAsignar = null;
            Session_TareasAAsignar = new List<Tarea>();
            hdnTabSeleccionado.Value = "";
            hdnDialogResultado.Value = "";
            gvTareasActividad.DataSource = Session_TareasAAsignar;
            gvTareasActividad.DataBind();
        }

        private void LoadActividad()
        {

            Session_ActividadActual = _controllerMantenerActividad.RetrieveActividadWithRelationship(Session_ActividadActual);
            if (Session_ActividadActual != null && Session_ActividadActual.ActividadID != null)
            {
                hdnControlActividadId.Value = Session_ActividadActual.ActividadID.ToString();
                txtNombreActividad.Text = Session_ActividadActual.Nombre;
                txtDescripcionActividad.Text = Session_ActividadActual.Descripcion;
                Session_TareasAAsignar = Session_ActividadActual.Tareas;
                ddlArea.ClearSelection();
                ddlArea.SelectedValue = Session_ActividadActual.ClasificadorID.ToString();
                gvTareasActividad.DataSource = Session_ActividadActual.Tareas;
                gvTareasActividad.DataBind();
            }
            else
            {
                Response.Redirect("MantenerActividadesUI.aspx", true);
            }

        }

        #region ajuste agregar contenido digital

        protected void btnAgrarContenido_OnClick(object sender, EventArgs evt)
        {
            AgregarContenidoUC.IniciarRegistro();
            hdnDialogResultado.Value = "AgregarContenidoDigital";
        }

        private List<FileWrapper> CrearListaFileWrapper(List<Tarea> tareas)
        {
            List<FileWrapper> lista = new List<FileWrapper>();

            var tareasContenido = tareas.Where(item => item is TareaContenidoDigital).ToList();

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
            //Stream instream = 

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

        #endregion

        protected override void AuthorizeUser()
        {

        }
    }
}