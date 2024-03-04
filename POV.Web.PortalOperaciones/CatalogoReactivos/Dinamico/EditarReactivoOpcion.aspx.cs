using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Seguridad.BO;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;
using POV.Modelo.Service;
using POV.Modelo.BO;
using POV.Reactivos.BO;
using POV.Reactivos.Service;
using POV.Operaciones.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using System.IO;
using System.Configuration;

namespace POV.Web.PortalOperaciones.CatalogoReactivos.Dinamico
{
    public partial class EditarReactivoOpcion : PageBase
    {

        private IDataContext dctx = ConnectionHlp.Default.Connection;

        private ModeloCtrl modeloCtrl;
        private ReactivoCtrl reactivoCtrl;
        private string RUTA_IMAGENES_SERVIDOR = "";
        public Reactivo LastObject
        {
            set { Session["lastReactivoDinamico"] = value; }
            get { return Session["lastReactivoDinamico"] != null ? Session["lastReactivoDinamico"] as Reactivo : null; }
        }

        public List<OpcionRespuestaPlantilla> ListaOpcionesEliminadas
        {
            set { Session["listOpcionesEliminadas"] = value; }
            get { return Session["listOpcionesEliminadas"] != null ? Session["listOpcionesEliminadas"] as List<OpcionRespuestaPlantilla> : null; }
        }

        public List<string> ListaImagenesEliminadas
        {
            set { Session["ListaImagenesEliminadas"] = value; }
            get { return Session["ListaImagenesEliminadas"] != null ? Session["ListaImagenesEliminadas"] as List<string> : null; }
        }

        public EditarReactivoOpcion()
        {
            modeloCtrl = new ModeloCtrl();
            reactivoCtrl = new ReactivoCtrl();
        }

        /// <summary>
        /// Ruta donde se alojan de manera temporal las imagenes
        /// </summary>
        private string RUTA_IMG_TEMPORAL = "~/Files/Images/ReactivosTemp/";

        #region *** eventos de pagina ***
        /// <summary>
        /// Evento de carga de la pagina
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            RUTA_IMAGENES_SERVIDOR = @ConfigurationManager.AppSettings["RutaContenidos"] +  @ConfigurationManager.AppSettings["POVRutaImgReactivosDinamico"];
            if (!IsPostBack)
            {
                if (LastObject != null && LastObject.ReactivoID != null)
                {
                    LastObject = reactivoCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, LastObject);

                    if (LastObject != null)
                    {
                        ListaOpcionesEliminadas = new List<OpcionRespuestaPlantilla>();
                        ListaImagenesEliminadas = new List<string>();

                        
                        LoadModelos();
                        LoadReactivo(LastObject);
                    }
                    else
                    {
                    }
                }
                else
                {
                    Response.Redirect("BuscarReactivos.aspx", true);
                }
            }
        }

        /// <summary>
        /// Evento que controla el boton de guardar de la pagina
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        protected void BtnGuardar_OnClick(object sender, EventArgs e)
        {
            string sError = ValidateReactivo();

            if (string.IsNullOrEmpty(sError))
            {
                Reactivo reactivo = GetReactivoFromUI();
                try
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

                    List<FileWrapper> fileWrappers = new List<FileWrapper>();
                    if (!cbPreguntaAbierta.Checked)
                        fileWrappers = CrearListaFileWrapper(reactivo);

                    CatalogoReactivosDinamicoCtrl catalogoReactivosCtrl = new CatalogoReactivosDinamicoCtrl();

                    catalogoReactivosCtrl.UpdateComplete(dctx, reactivo,LastObject, fileWrappers, ListaOpcionesEliminadas, ListaImagenesEliminadas,
                        servidorContenidos, userServidor, passervidor, metodoServidor);

                    EliminarArchivosTemporales(fileWrappers);
                    LimpiarObjetosSesion();
                    txtRedirect.Value = "BuscarReactivos.aspx";
                    ShowMessage("Registro exitoso.", MessageType.Information);
                }
                catch (Exception ex)
                {
                    txtRedirect.Value = "";
                    ShowMessage("Error: " + ex.Message, MessageType.Error);
                }
            }
            else
            {
                txtRedirect.Value = "";
                ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
            }
        }

        /// <summary>
        /// Evento de controla el boton de cancelar de la pagina
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arguments</param>
        protected void BtnCancelar_OnClick(object sender, EventArgs e)
        {
            
            EliminarArchivosTemporales();
            LimpiarObjetosSesion();
            Response.Redirect("BuscarReactivos.aspx");
        }

        /// <summary>
        /// Evento que controla cuando de realiza el bind de los datos en el Repeater de las opciones
        /// </summary>
        /// <param name="Sender">sender</param>
        /// <param name="e">arguments</param>
        protected void RptOpciones_DataBound(Object Sender, RepeaterItemEventArgs e)
        {


            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                ModeloDinamico modelo = GetModeloFromUI();
                // cargar el modelo desde la ui
                // consultar los clasificadores en el catalogo
                //setear en el ddl

                DataRowView dr = e.Item.DataItem as DataRowView;
                if (dr != null)
                {
                    
                    DropDownList ddlClasificador = (DropDownList)e.Item.FindControl("ddlClasificador");
                    HiddenField hdnOpcionID = (HiddenField)e.Item.FindControl("hdnOpcionID");
                    TextBox txtTextoOpcion = (TextBox)e.Item.FindControl("txtTextoOpcion");
                    TextBox txtPuntaje = (TextBox)e.Item.FindControl("txtPuntaje");
                    CheckBox checkBox = (CheckBox)e.Item.FindControl("chkEsCorrecta");
                    CheckBox checkBoxEsInteres = (CheckBox)e.Item.FindControl("chkEsInteres");
                    TextBox hdnImagen = (TextBox)e.Item.FindControl("hdnImagen");
                    Image imagenOpc = (Image)e.Item.FindControl("imgOpc");
                    HiddenField hdnImagenOpcEliminada = (HiddenField)e.Item.FindControl("hdnImagenOpcEliminada");
                    HiddenField hdnImagenOpcAnterior = (HiddenField)e.Item.FindControl("hdnImagenOpcAnterior");
                    Image imgOpcAnterior = (Image)e.Item.FindControl("imgOpcAnterior");
                    Button btnEliminarOpcAnterior = (Button)e.Item.FindControl("btnEliminarOpcAnterior");
                    Label lblImagenOpcAnterior = (Label)e.Item.FindControl("lblImagenOpcAnterior");

                    if (!string.IsNullOrEmpty(dr[0].ToString()))
                        hdnOpcionID.Value = dr[0].ToString();
                    if (!string.IsNullOrEmpty(dr[1].ToString()))
                        txtTextoOpcion.Text = dr[1].ToString();
                    if (!string.IsNullOrEmpty(dr[2].ToString()))
                        checkBox.Checked = Boolean.Parse(dr[2].ToString());
                    if (!string.IsNullOrEmpty(dr[3].ToString()))
                        checkBoxEsInteres.Checked = Boolean.Parse(dr[3].ToString());
                    if (!string.IsNullOrEmpty(dr[4].ToString()))
                    {
                        hdnImagen.Text = dr[4].ToString();
                        imagenOpc.ImageUrl = RUTA_IMG_TEMPORAL + dr[4].ToString();
                    }
                    if (!string.IsNullOrEmpty(dr[6].ToString()))
                        txtPuntaje.Text = dr[6].ToString();

                    if (modelo.ModeloID > 0)
                    {
                        DataSet dsClasificadores = modeloCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, modelo);

                        LoadClasificadoresDropDownList(ddlClasificador, dsClasificadores);

                        if (!string.IsNullOrEmpty(dr[5].ToString()))
                            ddlClasificador.SelectedValue = dr[5].ToString();
                    }

                    if (!string.IsNullOrEmpty(dr[7].ToString()))
                    {
                        hdnImagenOpcAnterior.Value = dr[7].ToString();
                        imgOpcAnterior.ImageUrl = RUTA_IMAGENES_SERVIDOR + dr[7].ToString();
                        
                        imgOpcAnterior.Visible = true;
                        btnEliminarOpcAnterior.Visible = true;
                        lblImagenOpcAnterior.Visible = true;
                    }
                    else
                    {
                        btnEliminarOpcAnterior.Visible = false;
                        imgOpcAnterior.Visible = false;
                        lblImagenOpcAnterior.Visible = false;
                    }
                    hdnImagenOpcEliminada.Value = string.IsNullOrEmpty(dr[8].ToString())? "false": dr[8].ToString();
                }
            }
        }

        /// <summary>
        /// Evento que controla cuando se realiza un cambio en el dropdownlist de Modelo
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arguments</param>
        protected void ddlModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModeloDinamico modelo = GetModeloFromUI();

            if (modelo.ModeloID > 0)
            {



                modelo = modeloCtrl.LastDataRowToModelo(modeloCtrl.Retrieve(ConnectionHlp.Default.Connection, modelo)) as ModeloDinamico;
                lblMetodoCalificacion.Text = modelo.NombreMetodoCalificacion;
                hdnTipoMetodoCalificacion.Value = ((byte)modelo.MetodoCalificacion).ToString();
                DataSet dsClasificadores = modeloCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, modelo);



                switch (modelo.MetodoCalificacion)
                {
                    case EMetodoCalificacion.PUNTOS:
                        //metodo: puntos
                        // puntaje: unico o por opcion
                        // puntaje requerido
                        // es correcta: requerido
                        // sin clasificador
                        ddlTipoPuntaje.Items.Clear();
                        ddlTipoPuntaje.Items.Add(new ListItem("Puntaje por opción", "2"));
                        ddlTipoPuntaje.Items.Add(new ListItem("Único", "1"));
                        ddlTipoPuntaje.DataBind();
                        txtPuntajeUnico.Enabled = false;
                        LoadClasificadoresRepeater();
                        break;
                    case EMetodoCalificacion.PORCENTAJE:
                        //metodo: aciertos
                        // puntaje: sin puntaje
                        // puntaje no editable
                        // es correcta: requerido
                        // sin clasificador

                        ddlTipoPuntaje.Items.Clear();
                        ddlTipoPuntaje.Items.Add(new ListItem("Sin puntaje", "0"));
                        ddlTipoPuntaje.DataBind();
                        txtPuntajeUnico.Enabled = false;
                        LoadClasificadoresRepeater();
                        break;
                    case EMetodoCalificacion.CLASIFICACION:
                        //metodo: clasificador
                        // puntaje: por opcion requerido
                        // puntaje opcion requerido
                        // es correcta: no requerido
                        // clasificador requerido
                        ddlTipoPuntaje.Items.Clear();
                        ddlTipoPuntaje.Items.Add(new ListItem("Puntaje por opción", "2"));
                        ddlTipoPuntaje.DataBind();
                        txtPuntajeUnico.Enabled = false;
                        LoadClasificadoresRepeater(dsClasificadores);
                        break;
                    case EMetodoCalificacion.SELECCION:
                        //metodo: seleccion
                        // puntaje: sin puntaje/
                        // puntaje opcion no requerido
                        // es correcta no requerido
                        // clasificador requerido
                        ddlTipoPuntaje.Items.Clear();
                        ddlTipoPuntaje.Items.Add(new ListItem("Sin puntaje", "0"));
                        ddlTipoPuntaje.DataBind();
                        txtPuntajeUnico.Enabled = false;
                        LoadClasificadoresRepeater(dsClasificadores);
                        break;
                }


            }
            else
            {
                lblMetodoCalificacion.Text = "SELECCIONE UN MODELO";
            }



        }

        /// <summary>
        /// Evento que controla cuando se realiza un cambio en el dropdownlist de tipo de puntaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoPuntaje_SelectedIndexChanged(object sender, EventArgs e)
        {
            ETipoPuntaje tipo = (ETipoPuntaje)byte.Parse(ddlTipoPuntaje.SelectedValue);

            if (tipo == ETipoPuntaje.UNICO)
            {
                txtPuntajeUnico.Enabled = true;
            }
            else
            {
                txtPuntajeUnico.Enabled = false;
            }
        }

        /// <summary>
        /// Evento que controla cuando se realiza la carga de una imagen para la pregunta
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arguments</param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileUploadImage.HasFile)
            {
                string guidImagen = Guid.NewGuid().ToString("N");
                string extension = fileUploadImage.FileName.Substring(fileUploadImage.FileName.LastIndexOf('.') + 1).ToLower();
                guidImagen += "." + extension;

                if (!string.IsNullOrEmpty(hdnImagenPregunta.Text))
                {
                    FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();
                    fileManagerCtrl.DeleteFile(Server.MapPath(RUTA_IMG_TEMPORAL) + hdnImagenPregunta.Text);
                }

                fileUploadImage.SaveAs(Server.MapPath(RUTA_IMG_TEMPORAL) + guidImagen);
                img.ImageUrl = RUTA_IMG_TEMPORAL + guidImagen;
                hdnImagenPregunta.Text = guidImagen;

            }
        }

        /// <summary>
        /// Evento que controla cuando se elimina la imagen de la pregunta
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arguments</param>
        protected void btnEliminarImgPregunta_OnClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnImagenPregunta.Text))
            {
                FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();
                fileManagerCtrl.DeleteFile(Server.MapPath(RUTA_IMG_TEMPORAL) + hdnImagenPregunta.Text);

                hdnImagenPregunta.Text = "";
                img.ImageUrl = "";
            }
        }

        /// <summary>
        /// Evento que controla cuando se elimina la imagen de la pregunta
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arguments</param>
        protected void btnEliminarImgPreguntaAnterior_OnClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnImagenPreguntaAnterior.Value))
            {
                string filename = @ConfigurationManager.AppSettings["POVRutaImgReactivosDinamico"] + hdnImagenPreguntaAnterior.Value;
                ListaImagenesEliminadas.Add(filename);
                hdnImagenPreguntaAnterior.Value = "";
                imgPreguntaAnterior.ImageUrl = "";

                imgPreguntaAnterior.Visible = false;
                btnEliminarPreguntaAnterior.Visible = false;
                lblImagenPreguntaAnterior.Visible = false;
                hdnImagenPreguntaEliminada.Value = "true";

               
            }
        }
        /// <summary>
        /// Evento que controla las acciones que se puede realizar desde el repeater como comandos
        /// </summary>
        /// <param name="Sender">sender</param>
        /// <param name="e">arguments</param>
        protected void RptOpciones_ItemCommand(Object Sender, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName == "uploadimg")
                {

                    FileUpload fiUp = e.Item.FindControl("fileUploadImageOpc") as FileUpload;
                    if (fiUp.HasFile)
                    {
                        Image imgOpc = e.Item.FindControl("imgOpc") as Image;
                        TextBox hdnImage = e.Item.FindControl("hdnImagen") as TextBox;

                        string fileName = fiUp.FileName;

                        string guidImagen = Guid.NewGuid().ToString("N");
                        string extension = fiUp.FileName.Substring(fiUp.FileName.LastIndexOf('.') + 1).ToLower();
                        guidImagen += "." + extension;

                        if (!string.IsNullOrEmpty(hdnImage.Text))
                        {
                            FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();
                            fileManagerCtrl.DeleteFile(Server.MapPath(RUTA_IMG_TEMPORAL) + hdnImage.Text);
                        }

                        fiUp.SaveAs(Server.MapPath(RUTA_IMG_TEMPORAL) + guidImagen);
                        imgOpc.ImageUrl = RUTA_IMG_TEMPORAL + guidImagen;
                        hdnImage.Text = guidImagen;

                        string op = (e.Item.ClientID + "_op_" + e.Item.ItemIndex).ToString();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "gotoOp('" + op + "');", true);
                    }

                }
                else if (e.CommandName == "deleteImagen")
                {
                    TextBox hdnImage = e.Item.FindControl("hdnImagen") as TextBox;
                    if (!string.IsNullOrEmpty(hdnImage.Text))
                    {
                        Image imgOpc = e.Item.FindControl("imgOpc") as Image;

                        imgOpc.ImageUrl = "";
                        hdnImage.Text = "";
                        FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();
                        fileManagerCtrl.DeleteFile(Server.MapPath(RUTA_IMG_TEMPORAL) + hdnImage.Text);
                    }
                }
                else if (e.CommandName == "deleteImagenOpcAnterior")
                {
                    HiddenField hdnImagenOpcAnterior = (HiddenField)e.Item.FindControl("hdnImagenOpcAnterior");
                    
                    Image imgOpcAnterior = (Image)e.Item.FindControl("imgOpcAnterior");
                    Button btnEliminarOpcAnterior = (Button)e.Item.FindControl("btnEliminarOpcAnterior");
                    Label lblImagenOpcAnterior = (Label)e.Item.FindControl("lblImagenOpcAnterior");
                    HiddenField hdnImagenOpcEliminada = (HiddenField)e.Item.FindControl("hdnImagenOpcEliminada");


                    if (!string.IsNullOrEmpty(hdnImagenOpcAnterior.Value))
                    {
                        string filename = @ConfigurationManager.AppSettings["POVRutaImgReactivosDinamico"] + hdnImagenOpcAnterior.Value;

                        ListaImagenesEliminadas.Add(filename);
                        
                        imgOpcAnterior.ImageUrl = "";
                        hdnImagenOpcAnterior.Value = "";
                        imgOpcAnterior.Visible = false;
                        btnEliminarOpcAnterior.Visible = false;
                        lblImagenOpcAnterior.Visible = false;
                        hdnImagenOpcEliminada.Value = "true";
                    }
                }
                else if (e.CommandName == "deleteOpcion")
                {
                    int index = e.Item.ItemIndex;
                    DataTable dtOpcion = GetDtOpciones();
                    DataRow dr = dtOpcion.Rows[index];

                    OpcionRespuestaPlantilla opcion = DataRowToOpcionRespuesta(dr);


                    dtOpcion.Rows.RemoveAt(index);
                    if (opcion.OpcionRespuestaPlantillaID != null)
                        ListaOpcionesEliminadas.Add(opcion);


                    rptOpciones.DataSource = dtOpcion;
                    rptOpciones.DataBind();
                }
            }

        }

        /// <summary>
        /// Evento que controla cuando se agrega una nueva opción en el repeater de opciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnAgregarOpcion_OnClick(object sender, EventArgs e)
        {
            DataTable dtOpcion = GetDtOpciones();
            if (dtOpcion != null && dtOpcion.Rows.Count > 0)
            {
                DataRow dr = dtOpcion.NewRow();

                dtOpcion.Rows.Add(dr);

                rptOpciones.DataSource = dtOpcion;
                rptOpciones.DataBind();
            }
            else
            {
                DataTable dtInicial = CrearDataTableOpciones();
                DataRow dr = dtInicial.NewRow();

                dtInicial.Rows.Add(dr);
                

                rptOpciones.DataSource = dtInicial;
                rptOpciones.DataBind();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "gotoOpNew('" + btnAgregarNuevaOpcion.ClientID + "');", true);
        }
        #endregion

        #region *** validaciones ***
        /// <summary>
        /// Valida que los datos solicitados para el reactivo sean los correctos
        /// </summary>
        /// <returns>Cadena de los errores de los campos, </returns>
        private string ValidateReactivo()
        {
            string sError = string.Empty;
            string sClave = txtClave.Text.Trim();
            string sNombre = txtNombre.Text.Trim();
            string textoPregunta = txtPregunta.Text.Trim();
            ModeloDinamico modelo = GetModeloFromUI();

            if (string.IsNullOrEmpty(sClave))
                sError += ", Clave Requerida ";
            else if (sClave.Length > 30)
                sError += ", Clave máximo 30 caracteres ";
            if (string.IsNullOrEmpty(sNombre))
                sError += ", Nombre es Requerido ";
            else if (sNombre.Length > 100)
                sError += ", Nombre máximo 100 caracteres";
            if (modelo.ModeloID == null || modelo.ModeloID == 0)
                sError += ", Modelo es requerido ";
            if (string.IsNullOrEmpty(textoPregunta))
                sError += ", Texto de pregunta es Requerido ";
            else if (textoPregunta.Length > 1000)
                sError += ", Texto de pregunta máximo 1000 caracteres";

            if (!cbPreguntaAbierta.Checked)
            {
                DataTable dtOpciones = GetDtOpciones();
                if (dtOpciones.Rows.Count < 2)
                {
                    sError += ", al menos dos opciones de respuesta es Requerido ";
                }
            }

            return sError;
        }
        #endregion

        #region *** Data to UserInterface ***

        private void LoadReactivo(Reactivo reactivo)
        {
            txtClave.Text = reactivo.Clave;
            txtNombre.Text = reactivo.NombreReactivo;

            ModeloDinamico modelo = (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo;

            txtGrupo.Text = reactivo.Grupo != null ? reactivo.Grupo.ToString() : "";

            
            //cargar info de pregunta
            ddlModelo.SelectedValue = modelo.ModeloID.ToString();
            ddlModelo_SelectedIndexChanged(null, null);

            Pregunta pregunta = reactivo.Preguntas.First();
            if (pregunta != null)
            {

                if (pregunta.RespuestaPlantilla is RespuestaPlantillaAbierta)
                {
                    cbPreguntaAbierta.Checked = true;
                    modelo = modeloCtrl.LastDataRowToModelo(modeloCtrl.Retrieve(ConnectionHlp.Default.Connection, modelo)) as ModeloDinamico;
                    lblMetodoCalificacion.Text = modelo.NombreMetodoCalificacion;
                    hdnTipoMetodoCalificacion.Value = ((byte)modelo.MetodoCalificacion).ToString();
                    DataSet dsClasificadores = modeloCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, modelo);
                    LoadClasificadoresDropDownList(ddlClasificadorPreguntaAbierta, dsClasificadores);
                    divOpciones.Visible = false;
                    divReactivoOpcionMultiple.Visible = false;
                    rowClasificadores.Visible = true;
                    ddlClasificadorPreguntaAbierta.SelectedValue = (pregunta.RespuestaPlantilla as RespuestaPlantillaAbierta).Clasificador.ClasificadorID.ToString();
                }

                var respuestaPlantilla = pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple;
                txtPregunta.Text = pregunta.TextoPregunta;
                txtPuntajeUnico.Text = pregunta.Valor != null ? pregunta.Valor.ToString() : "";
                ddlTipoPresentacionPregunta.SelectedValue = ((byte)pregunta.PresentacionPlantilla).ToString();

                if (respuestaPlantilla != null)
                {
                    ddlTipoSeleccion.SelectedValue = ((short)respuestaPlantilla.ModoSeleccion).ToString();
                    ddlPresentacionOpc.SelectedValue = ((byte)respuestaPlantilla.PresentacionOpcion).ToString();
                }
                if (!string.IsNullOrEmpty(pregunta.PlantillaPregunta))
                {
                    hdnImagenPreguntaAnterior.Value = pregunta.PlantillaPregunta;
                    imgPreguntaAnterior.ImageUrl = RUTA_IMAGENES_SERVIDOR + pregunta.PlantillaPregunta;
                    lblImagenPregunta.Visible = true;
                    lblImagenPreguntaAnterior.Visible = true;
                }
                else
                {
                    imgPreguntaAnterior.Visible = false;
                    btnEliminarPreguntaAnterior.Visible = false;
                    lblImagenPreguntaAnterior.Visible = false;
                }
                hdnImagenPreguntaEliminada.Value = "false";

                //cargar info de opciones

                DataTable dtOpciones = CrearDataTableOpcionesPregunta(pregunta);

                rptOpciones.DataSource = dtOpciones;
                rptOpciones.DataBind();

            }
        }
        /// <summary>
        /// Carga los datos de modelos en dropdownlist de modelos
        /// </summary>
        private void LoadModelos()
        {
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            parametros.Add("TipoModelo", "0");
            parametros.Add("Activo", "true");

            DataSet dsModelos = modeloCtrl.Retrieve(dctx, null, parametros);
            ddlModelo.DataSource = dsModelos;
            ddlModelo.DataValueField = "ModeloID";
            ddlModelo.DataTextField = "Nombre";
            ddlModelo.DataBind();
            ddlModelo.Items.Insert(0, new ListItem("SELECCIONE...", ""));
        }

        /// <summary>
        /// Carga los datos de los clasificadores en los dropdownlist de los clasificadores en el repeater
        /// </summary>
        /// <param name="dsClasificadores"></param>
        private void LoadClasificadoresRepeater(DataSet dsClasificadores = null)
        {
            foreach (RepeaterItem item in rptOpciones.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList ddlClasificador = (DropDownList)item.FindControl("ddlClasificador");
                    if (dsClasificadores != null)
                    {
                        LoadClasificadoresDropDownList(ddlClasificador, dsClasificadores);
                    }
                    else
                    {
                        ddlClasificador.Items.Clear();
                        ddlClasificador.Items.Insert(0, new ListItem("Sin clasificador", ""));
                    }
                }
            }
        }
        #endregion

        #region *** UserInterface to Data ***
        /// <summary>
        /// Obtiene los datos de reactivo de la UI
        /// </summary>
        /// <returns>Datos del Reactivo</returns>
        private Reactivo GetReactivoFromUI()
        {
            Reactivo reactivo = LastObject.Clone() as Reactivo;
            

            reactivo.Clave = txtClave.Text;
            reactivo.NombreReactivo = txtNombre.Text;
            // grupo
            if (!string.IsNullOrEmpty(txtGrupo.Text))
            {
                reactivo.Grupo = int.Parse(txtGrupo.Text);
            }

            Pregunta pregunta = reactivo.Preguntas.FirstOrDefault();
  
            pregunta.TextoPregunta = txtPregunta.Text;
            if (!string.IsNullOrEmpty(txtPuntajeUnico.Text))
                pregunta.Valor = decimal.Parse(txtPuntajeUnico.Text);
            else
                pregunta.Valor = 0;

            if (bool.Parse(hdnImagenPreguntaEliminada.Value)) //si fue eliminada la imagen
            {
                if (!string.IsNullOrEmpty(hdnImagenPregunta.Text))
                    pregunta.PlantillaPregunta = hdnImagenPregunta.Text;
                else
                    pregunta.PlantillaPregunta = "";
            }
            else
            {
                if (!string.IsNullOrEmpty(hdnImagenPregunta.Text))
                {
                    pregunta.PlantillaPregunta = hdnImagenPregunta.Text;
                }
            }
            
            pregunta.PresentacionPlantilla = (EPresentacionPlantilla)byte.Parse(ddlTipoPresentacionPregunta.SelectedValue);

            if (cbPreguntaAbierta.Checked)
            {
                (pregunta.RespuestaPlantilla as RespuestaPlantillaTexto).Modelo = new ModeloDinamico() { ModeloID = GetModeloFromUI().ModeloID };
                (pregunta.RespuestaPlantilla as RespuestaPlantillaTexto).Clasificador = new Clasificador() { ClasificadorID = GetClasificadorFromUI(ddlClasificadorPreguntaAbierta).ClasificadorID };
            }
            else
            {
                (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla = GetListaOpcionesFromUI((pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla);
                (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ModoSeleccion = (EModoSeleccion)short.Parse(ddlTipoSeleccion.SelectedValue);
                (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).PresentacionOpcion = (EPresentacionOpcion)byte.Parse(ddlPresentacionOpc.SelectedValue);
                (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).TipoPuntaje = (ETipoPuntaje)byte.Parse(ddlTipoPuntaje.SelectedValue);
            }
            return reactivo;
        }

        /// <summary>
        /// Obtiene el modelo seleccionado en la UI
        /// </summary>
        /// <returns>Modelo seleccionado</returns>
        private ModeloDinamico GetModeloFromUI()
        {
            ModeloDinamico modelo = new ModeloDinamico();
            int modeloId = 0;
            bool result = int.TryParse(ddlModelo.SelectedValue, out modeloId);
            if (result)
            {
                modelo.ModeloID = modeloId;
            }
            return modelo;
        }

        /// <summary>
        /// Obtiene el modelo seleccionado en la UI
        /// </summary>
        /// <param name="ddlClasificador">dropdownlist de donde se toma el clasificador</param>
        /// <returns>Clasificador seleccionado</returns>
        private Clasificador GetClasificadorFromUI(DropDownList ddlClasificador)
        {
            Clasificador clasificador = new Clasificador();
            int clasificadorID = 0;
            string valorID = ddlClasificador.SelectedValue;
            if (cbPreguntaAbierta.Checked)
                valorID = ddlClasificadorPreguntaAbierta.SelectedValue;
            else
                valorID = ddlClasificador.SelectedValue;
            if (int.TryParse(valorID, out clasificadorID))
            {
                if (clasificadorID > 0)
                    clasificador.ClasificadorID = clasificadorID;
            }

            return clasificador;
        }

        /// <summary>
        /// Obtiene un DataTable que contiene la informacion de las opciones capturadas en el repeater de opciones
        /// </summary>
        /// <returns>DataTable con la información de las opciones</returns>
        private DataTable GetDtOpciones()
        {
            DataTable dt = new DataTable();
            dt = CrearDataTableOpciones();


            foreach (RepeaterItem item in rptOpciones.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList ddlClasificador = (DropDownList)item.FindControl("ddlClasificador");
                    HiddenField hdnOpcionID = (HiddenField)item.FindControl("hdnOpcionID");
                    TextBox txtTextoOpcion = (TextBox)item.FindControl("txtTextoOpcion");
                    TextBox txtPuntaje = (TextBox)item.FindControl("txtPuntaje");
                    CheckBox checkBox = (CheckBox)item.FindControl("chkEsCorrecta");
                    CheckBox checkBoxEsInteres = (CheckBox)item.FindControl("chkEsInteres");
                    TextBox hdnImagen = (TextBox)item.FindControl("hdnImagen");
                    HiddenField hdnImagenOpcEliminada = (HiddenField)item.FindControl("hdnImagenOpcEliminada");
                    HiddenField hdnImagenOpcAnterior = (HiddenField)item.FindControl("hdnImagenOpcAnterior");


                    Clasificador clasificador = GetClasificadorFromUI(ddlClasificador);

                    DataRow dr = dt.NewRow();

                    dr[0] = hdnOpcionID.Value;
                    dr[1] = txtTextoOpcion.Text;
                    dr[2] = checkBox.Checked;
                    dr[3] = checkBoxEsInteres.Checked;
                    dr[4] = hdnImagen.Text;
                    dr[5] = clasificador.ClasificadorID;
                    dr[6] = txtPuntaje.Text;
                    dr[8] = hdnImagenOpcEliminada.Value;
                    dr[7] = hdnImagenOpcAnterior.Value;

                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// <summary>
        /// Obitne una lista de OpcionRespuestaPlantilla de la UI
        /// </summary>
        /// <returns>Lista de opciones</returns>
        private List<OpcionRespuestaPlantilla> GetListaOpcionesFromUI(List<OpcionRespuestaPlantilla> opciones)
        {
            List<OpcionRespuestaPlantilla> listaOpciones = new List<OpcionRespuestaPlantilla>();

            DataTable dtOpciones = GetDtOpciones();

            foreach (DataRow dr in dtOpciones.Rows)
            {
                OpcionRespuestaModeloGenerico opcion = new OpcionRespuestaModeloGenerico();

                if (string.IsNullOrEmpty(dr[0].ToString()))
                {

                    opcion.Activo = true;
                    opcion.EsOpcionCorrecta = bool.Parse(dr[2].ToString());
                    opcion.EsInteres = bool.Parse(dr[3].ToString());
                    opcion.EsPredeterminado = false;
                    opcion.Modelo = GetModeloFromUI();
                    

                }
                else
                {
                    int opcionid = int.Parse(dr[0].ToString());

                    OpcionRespuestaPlantilla opcionEditada = opciones.FirstOrDefault(item => item.OpcionRespuestaPlantillaID == opcionid);

                    if (opcionEditada != null)
                        opcion = opcionEditada as OpcionRespuestaModeloGenerico;
                    else
                        continue;
                }

                if (!string.IsNullOrEmpty(dr[2].ToString()))
                    opcion.EsOpcionCorrecta = bool.Parse(dr[2].ToString());

                if (!string.IsNullOrEmpty(dr[3].ToString()))
                    opcion.EsInteres = bool.Parse(dr[3].ToString());

                if (!string.IsNullOrEmpty(dr[6].ToString()))
                    opcion.PorcentajeCalificacion = decimal.Parse(dr[6].ToString());
                else
                    opcion.PorcentajeCalificacion = 0;

                opcion.Texto = dr[1].ToString();

                if (!string.IsNullOrEmpty(dr[5].ToString()))
                    opcion.Clasificador = new Clasificador { ClasificadorID = int.Parse(dr[5].ToString()) };
                else
                    opcion.Clasificador = new Clasificador();

                if (bool.Parse(dr[8].ToString())) // si fue eliminado
                {
                    if (!string.IsNullOrEmpty(dr[4].ToString()))
                        opcion.ImagenUrl = dr[4].ToString();
                    else
                        opcion.ImagenUrl = "";
                } 
                else 
                {
                    if (!string.IsNullOrEmpty(dr[4].ToString()))
                    {
                        opcion.ImagenUrl = dr[4].ToString();
                    }
                    else
                    {
                        opcion.ImagenUrl = dr[7].ToString();
                    }
                }
                
                listaOpciones.Add(opcion);
            }

            return listaOpciones;
        }
        #endregion

        #region *** metodos auxiliares ***
        /// <summary>
        /// Crea un listado de FileWrapper a partir de los archivos en la información del reactivo
        /// </summary>
        /// <param name="reactivo">Reactivo de donde se tomaran la lista de archivos</param>
        /// <returns>Lista de FileWrapper</returns>
        private List<FileWrapper> CrearListaFileWrapper(Reactivo reactivo)
        {
            List<FileWrapper> listaArchivos = new List<FileWrapper>();

            Pregunta pregunta = reactivo.Preguntas.First();
            Pregunta preguntaAnterior = LastObject.Preguntas.FirstOrDefault();

            if (!string.IsNullOrEmpty(pregunta.PlantillaPregunta) && preguntaAnterior.PlantillaPregunta != pregunta.PlantillaPregunta)
            {
                listaArchivos.Add(CrearFileWrapper(pregunta.PlantillaPregunta));
            }

            foreach (OpcionRespuestaModeloGenerico opcion in (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla)
            {
                if (opcion.OpcionRespuestaPlantillaID == null)
                {
                    if (!string.IsNullOrEmpty(opcion.ImagenUrl))
                    {
                        listaArchivos.Add(CrearFileWrapper(opcion.ImagenUrl));
                    }
                }
                else
                {
                    OpcionRespuestaPlantilla opcionAnterior =  (preguntaAnterior.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla.FirstOrDefault(item => item.OpcionRespuestaPlantillaID == opcion.OpcionRespuestaPlantillaID);

                    if (!string.IsNullOrEmpty(opcion.ImagenUrl) && opcion.ImagenUrl != opcionAnterior.ImagenUrl)
                    {
                        listaArchivos.Add(CrearFileWrapper(opcion.ImagenUrl));
                    }
                }
            }


            return listaArchivos;
        }

        /// <summary>
        /// Crea un FileWrapper a partir de un archivo
        /// </summary>
        /// <param name="filename">nombre del archivo</param>
        /// <returns>FileWrapper</returns>
        private FileWrapper CrearFileWrapper(string filename)
        {
            FileWrapper file = new FileWrapper();

            string path = Server.MapPath(RUTA_IMG_TEMPORAL) + filename;
            //Stream instream = 

            FileStream fileStream = File.OpenRead(path);

            Byte[] arreglo = new Byte[fileStream.Length];
            BinaryReader reader = new BinaryReader(fileStream);
            arreglo = reader.ReadBytes(Convert.ToInt32(fileStream.Length));

            file.Data = arreglo;
            file.Lenght = Convert.ToInt32(fileStream.Length);

            

            string fName = string.Empty;
            
            string carpetaImages = @ConfigurationManager.AppSettings["POVRutaImgReactivosDinamico"];
            fName = carpetaImages + filename;
            

            file.Name = fName;

            file.Type = FileToMimeType(filename);

            fileStream.Close();

            return file;
        }
        
        /// <summary>
        /// Obtiene el mime de un archivo
        /// </summary>
        /// <param name="filename">nombre del archivo</param>
        /// <returns>Cadena que representa el mime de un archivo</returns>
        private string FileToMimeType(string filename)
        {
            string extension = filename.Substring(filename.LastIndexOf('.') + 1).ToLower();
            Dictionary<string, string> diccionarioMimes = new Dictionary<string, string>();
            diccionarioMimes.Add("jpeg", "image/jpeg");
            diccionarioMimes.Add("jpg", "image/jpeg");
            diccionarioMimes.Add("png", "image/png");
            diccionarioMimes.Add("gif", "image/gif");
            diccionarioMimes.Add("bmp", "image/bmp");

            if (!diccionarioMimes.ContainsKey(extension))
                return "";

            return diccionarioMimes.FirstOrDefault(item => item.Key == extension).Value;
        }

        /// <summary>
        /// Crea una DataTable de los datos que se requiere para las opciones
        /// </summary>
        /// <returns>DataTable con los datos requeridos para las opciones</returns>
        private DataTable CrearDataTableOpciones()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OpcionID", typeof(string));
            dt.Columns.Add("TextoOpcion", typeof(string));
            dt.Columns.Add("EsCorrecta", typeof(string));
            dt.Columns.Add("EsInteres", typeof(string));
            dt.Columns.Add("NombreImagen", typeof(string));
            dt.Columns.Add("ClasificadorID", typeof(string));
            dt.Columns.Add("Puntaje", typeof(string));
            dt.Columns.Add("ImagenAnterior", typeof(string));
            dt.Columns.Add("eliminarAnterior", typeof(string));

            return dt;
        }

        private DataTable CrearDataTableOpcionesPregunta(Pregunta pregunta)
        {

            DataTable dt = CrearDataTableOpciones();

            if (pregunta.RespuestaPlantilla.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.OPCION_MULTIPLE)
            {
                foreach (OpcionRespuestaPlantilla opcion in (pregunta.RespuestaPlantilla as RespuestaPlantillaOpcionMultiple).ListaOpcionRespuestaPlantilla)
                {
                    OpcionRespuestaModeloGenerico opcionGenerico = opcion as OpcionRespuestaModeloGenerico;
                    DataRow dr = dt.NewRow();

                    dr[0] = opcionGenerico.OpcionRespuestaPlantillaID.ToString();
                    dr[1] = opcionGenerico.Texto;
                    dr[2] = opcionGenerico.EsOpcionCorrecta.ToString();
                    dr[3] = opcionGenerico.EsInteres.ToString();
                    dr[4] = "";
                    if (opcionGenerico.Clasificador != null && opcionGenerico.Clasificador.ClasificadorID != null)
                        dr[5] = opcionGenerico.Clasificador.ClasificadorID.ToString();
                    else
                        dr[5] = "";

                    dr[6] = opcionGenerico.PorcentajeCalificacion != null ? opcionGenerico.PorcentajeCalificacion.Value.ToString() : "";
                    dr[7] = opcionGenerico.ImagenUrl;
                    dr[8] = "false";

                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        private OpcionRespuestaPlantilla DataRowToOpcionRespuesta(DataRow dr)
        {
            OpcionRespuestaModeloGenerico opcion = new OpcionRespuestaModeloGenerico();

            if (!string.IsNullOrEmpty(dr[0].ToString()))
            {
                opcion.OpcionRespuestaPlantillaID = int.Parse(dr[0].ToString());
            }
            else
                opcion.OpcionRespuestaPlantillaID = null;


            opcion.EsOpcionCorrecta = bool.Parse(dr[2].ToString());
            opcion.EsInteres = bool.Parse(dr[3].ToString());
            
            opcion.Modelo = GetModeloFromUI();
            if (!string.IsNullOrEmpty(dr[6].ToString()))
                opcion.PorcentajeCalificacion = decimal.Parse(dr[6].ToString());
            else
                opcion.PorcentajeCalificacion = 0;

            opcion.Texto = dr[1].ToString();

            if (!string.IsNullOrEmpty(dr[5].ToString()))
                opcion.Clasificador = new Clasificador { ClasificadorID = int.Parse(dr[5].ToString()) };
            else
                opcion.Clasificador = new Clasificador();

            if (!string.IsNullOrEmpty(dr[4].ToString()))
                opcion.ImagenUrl = dr[4].ToString();

            return opcion;
        }
        /// <summary>
        /// Carga la informacion de los clasificadores en el dropdownlist
        /// </summary>
        /// <param name="ddlClasificador">dropdownlist</param>
        /// <param name="dsClasificadores">DataSet con la informacion de los clasificadores</param>
        private void LoadClasificadoresDropDownList(DropDownList ddlClasificador, DataSet dsClasificadores)
        {
            ddlClasificador.DataSource = dsClasificadores;
            ddlClasificador.DataValueField = "ClasificadorID";
            ddlClasificador.DataTextField = "Nombre";
            ddlClasificador.DataBind();
            ddlClasificador.Items.Insert(0, new ListItem("Sin clasificador ", ""));
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
                    fileManager.DeleteFile(Server.MapPath(RUTA_IMG_TEMPORAL) + file.Name);
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
            fileManager.DeleteOldFilesDirectory(Server.MapPath(RUTA_IMG_TEMPORAL), 14400);
        }

        private void LimpiarObjetosSesion()
        {
            LastObject = null;
            ListaImagenesEliminadas = null;
            ListaOpcionesEliminadas = null;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOREACTIVOSDINAMICO) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARREACTIVOSREACTIVOSDINAMICO) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!creacion)
                redirector.GoToHomePage(true);
        }
        #endregion
    }
}