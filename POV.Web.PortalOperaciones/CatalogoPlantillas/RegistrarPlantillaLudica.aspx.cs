using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.ConfiguracionesPlataforma.BO;
using POV.Seguridad.BO;
using POV.Web.Controllers.ServiciosPlataforma.Controllers;
using POV.Web.PortalOperaciones.AppCode.Page;

namespace POV.Web.PortalOperaciones.CatalogoPlantillas
{
    public partial class RegistrarPlantillaLudica : PageBase
    {
        private CatalogoPlantillaLudicaController controller;

        private ConfiguracionGeneral configuracionGeneral
        {
            set { Session["configuracionGeneral"] = value; }
            get { return Session["configuracionGeneral"] as ConfiguracionGeneral; }
        }

        /// <summary>
        /// Ruta donde se alojan de manera temporal las imagenes
        /// </summary>
        private string RUTA_IMG_TEMPORAL = "~/Files/Images/PlantillasTemp/";
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            controller = new CatalogoPlantillaLudicaController(null);
            try
            {
                if (!IsPostBack)
                {
                    configuracionGeneral = new ConfiguracionGeneral();                    
                    List<ConfiguracionGeneral> lstConf = new List<ConfiguracionGeneral>();
                    lstConf = controller.ConsultarConfiguraciónGeneral(configuracionGeneral);

                    if (lstConf.Count > 0)
                    {
                        configuracionGeneral = lstConf.FirstOrDefault();
                        FillBack();
                    }
                    else
                        ShowMessage("No se ha configurado la ruta del servidor de contenidos. Por favor verifique", MessageType.Information);
                }

            }
            catch (Exception)
            {
                //txtRedirect.Value = "BuscarPlantilla.aspx";
                ShowMessage("Ocurrió un error al cargar la página", MessageType.Error);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileUploadImageFondo.HasFile)
            {
                string guidImagen = Guid.NewGuid().ToString("N");
                string extension = fileUploadImageFondo.FileName.Substring(fileUploadImageFondo.FileName.LastIndexOf('.') + 1).ToLower();
                guidImagen += "." + extension;

                if (!string.IsNullOrEmpty(hdnImagenFondo.Text))
                {
                    FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();
                    fileManagerCtrl.DeleteFile(Server.MapPath(RUTA_IMG_TEMPORAL) + hdnImagenFondo.Text);
                }

                fileUploadImageFondo.SaveAs(Server.MapPath(RUTA_IMG_TEMPORAL) + guidImagen);
                imgFondo.ImageUrl = RUTA_IMG_TEMPORAL + guidImagen;
                hdnImagenFondo.Text = guidImagen;

                pnlwork.Attributes.Add("style", " background-image:url('" + Page.ResolveClientUrl(RUTA_IMG_TEMPORAL + guidImagen) + "');width: 450px; height: 650px; position:relative;");

            }
        }

        protected void btnEliminarImagenFondo_Click(object sender, EventArgs e)
        {
            TextBox hdnImagenEliminar = hdnImagenFondo;
            EliminarImagen(hdnImagenEliminar, imgFondo);
        }

        protected void btnAgregarPosicion_Click(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string sError = ValidatePlantilla();
                bool registrado = false;
                if (string.IsNullOrEmpty(sError))
                {
                    ValidateEsPredeterminada();
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
                    List<FileWrapper> fileWrappers = CrearListaFileWrapper(InterfaceToData());
                    registrado = controller.RegistrarPlantillaLudica(InterfaceToData(), fileWrappers, servidorContenidos, userServidor, passervidor, metodoServidor);
                    EliminarArchivosTemporales(fileWrappers);
                    txtRedirect.Value = "BuscarPlantilla.aspx";
                    ShowMessage("La plantilla lúdica se guardó correctamente.", MessageType.Information);
                }
                else
                {
                    txtRedirect.Value = "";
                    ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                txtRedirect.Value = "";
                ShowMessage("Inconsistencias al guardar la información." , MessageType.Error);
            }
        }

        #region Flechas
        protected void btnUploadFlechaArriba_Click(object sender, EventArgs e)
        {
            if (fileUploadFlechaArriba.HasFile)
            {
                string guidImagen = Guid.NewGuid().ToString("N");
                string extension = fileUploadFlechaArriba.FileName.Substring(fileUploadFlechaArriba.FileName.LastIndexOf('.') + 1).ToLower();
                guidImagen += "." + extension;

                if (!string.IsNullOrEmpty(hdnFlechaArriba.Text))
                {
                    FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();
                    fileManagerCtrl.DeleteFile(Server.MapPath(RUTA_IMG_TEMPORAL) + hdnFlechaArriba.Text);
                }

                fileUploadFlechaArriba.SaveAs(Server.MapPath(RUTA_IMG_TEMPORAL) + guidImagen);
                imgFlechaArriba.ImageUrl = RUTA_IMG_TEMPORAL + guidImagen;
                hdnFlechaArriba.Text = guidImagen;
            }
        }

        protected void btnEliminarFlechaArriba_Click(object sender, EventArgs e)
        {
            TextBox hdnImagenEliminar = hdnFlechaArriba;
            EliminarImagen(hdnImagenEliminar, imgFlechaArriba);
        }

        protected void btnUploadFlechaAbajo_Click(object sender, EventArgs e)
        {
            if (fileUploadFlechaAbajo.HasFile)
            {
                string guidImagen = Guid.NewGuid().ToString("N");
                string extension = fileUploadFlechaAbajo.FileName.Substring(fileUploadFlechaAbajo.FileName.LastIndexOf('.') + 1).ToLower();
                guidImagen += "." + extension;

                if (!string.IsNullOrEmpty(hdnFlechaAbajo.Text))
                {
                    FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();
                    fileManagerCtrl.DeleteFile(Server.MapPath(RUTA_IMG_TEMPORAL) + hdnFlechaAbajo.Text);
                }

                fileUploadFlechaAbajo.SaveAs(Server.MapPath(RUTA_IMG_TEMPORAL) + guidImagen);
                imgFlechaAbajo.ImageUrl = RUTA_IMG_TEMPORAL + guidImagen;
                hdnFlechaAbajo.Text = guidImagen;
            }
        }

        protected void btnEliminarFlechaAbajo_Click(object sender, EventArgs e)
        {
            TextBox hdnImagenEliminar = hdnFlechaAbajo;
            EliminarImagen(hdnImagenEliminar, imgFlechaAbajo);
        }
        #endregion

        #region Actividades
        protected void btnUploadPendiente_Click(object sender, EventArgs e)
        {
            if (fileUploadActPendiente.HasFile)
            {
                string guidImagen = Guid.NewGuid().ToString("N");
                string extension = fileUploadActPendiente.FileName.Substring(fileUploadActPendiente.FileName.LastIndexOf('.') + 1).ToLower();
                guidImagen += "." + extension;

                if (!string.IsNullOrEmpty(hdnActPendiente.Text))
                {
                    FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();
                    fileManagerCtrl.DeleteFile(Server.MapPath(RUTA_IMG_TEMPORAL) + hdnActPendiente.Text);
                }

                fileUploadActPendiente.SaveAs(Server.MapPath(RUTA_IMG_TEMPORAL) + guidImagen);
                imgPendiente.ImageUrl = RUTA_IMG_TEMPORAL + guidImagen;
                hiddenPosicion.Text = Page.ResolveClientUrl(RUTA_IMG_TEMPORAL + guidImagen);
                hdnActPendiente.Text = guidImagen;
            }
        }

        protected void btnEliminarImagenPendiente_Click(object sender, EventArgs e)
        {
            TextBox hdnImagenEliminar = hdnActPendiente;
            EliminarImagen(hdnImagenEliminar, imgPendiente);
        }

        protected void btnUploadActIniciada_Click(object sender, EventArgs e)
        {
            if (fileUploadActIniciada.HasFile)
            {
                string guidImagen = Guid.NewGuid().ToString("N");
                string extension = fileUploadActIniciada.FileName.Substring(fileUploadActIniciada.FileName.LastIndexOf('.') + 1).ToLower();
                guidImagen += "." + extension;

                if (!string.IsNullOrEmpty(hdnActIniciada.Text))
                {
                    FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();
                    fileManagerCtrl.DeleteFile(Server.MapPath(RUTA_IMG_TEMPORAL) + hdnActIniciada.Text);
                }

                fileUploadActIniciada.SaveAs(Server.MapPath(RUTA_IMG_TEMPORAL) + guidImagen);
                imgIniciada.ImageUrl = RUTA_IMG_TEMPORAL + guidImagen;
                hdnActIniciada.Text = guidImagen;

            }
        }

        protected void btnEliminarImagenIniciada_Click(object sender, EventArgs e)
        {
            TextBox hdnImagenEliminar = hdnActIniciada;
            EliminarImagen(hdnImagenEliminar, imgIniciada );
        }

        protected void btnUploadActFinalizada_Click(object sender, EventArgs e)
        {
            if (fileUploadActFinalizada.HasFile)
            {
                string guidImagen = Guid.NewGuid().ToString("N");
                string extension = fileUploadActFinalizada.FileName.Substring(fileUploadActFinalizada.FileName.LastIndexOf('.') + 1).ToLower();
                guidImagen += "." + extension;

                if (!string.IsNullOrEmpty(hdnFinalizada.Text))
                {
                    FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();
                    fileManagerCtrl.DeleteFile(Server.MapPath(RUTA_IMG_TEMPORAL) + hdnFinalizada.Text);
                }

                fileUploadActFinalizada.SaveAs(Server.MapPath(RUTA_IMG_TEMPORAL) + guidImagen);
                imgFinalizada.ImageUrl = RUTA_IMG_TEMPORAL + guidImagen;
                hdnFinalizada.Text = guidImagen;

            }
        }

        protected void btnEliminarImagenFinalizada_Click(object sender, EventArgs e)
        {
            TextBox hdnImagenEliminar = hdnFinalizada;
            EliminarImagen(hdnImagenEliminar, imgFinalizada);
        }

        protected void btnUploadActNoDisponible_Click(object sender, EventArgs e)
        {
            if (fileUploadActNoDisponible.HasFile)
            {
                string guidImagen = Guid.NewGuid().ToString("N");
                string extension = fileUploadActNoDisponible.FileName.Substring(fileUploadActNoDisponible.FileName.LastIndexOf('.') + 1).ToLower();
                guidImagen += "." + extension;

                if (!string.IsNullOrEmpty(hdnNoDisponible.Text))
                {
                    FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();
                    fileManagerCtrl.DeleteFile(Server.MapPath(RUTA_IMG_TEMPORAL) + hdnNoDisponible.Text);
                }

                fileUploadActNoDisponible.SaveAs(Server.MapPath(RUTA_IMG_TEMPORAL) + guidImagen);
                imgNoDisponible.ImageUrl = RUTA_IMG_TEMPORAL + guidImagen;
                hdnNoDisponible.Text = guidImagen;

            }
        }

        protected void btnEliminarImagenNoDisponible_Click(object sender, EventArgs e)
        {
            TextBox hdnImagenEliminar = hdnNoDisponible;
            EliminarImagen(hdnImagenEliminar, imgNoDisponible);
        }
        #endregion
        #endregion

        #region Metodos
        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/CatalogoPlantillas/BuscarPlantilla.aspx";
        }

        private PlantillaLudica InterfaceToData()
        {
            PlantillaLudica plantilla = new PlantillaLudica();

            //Nombre
            if (txtNombrePLantilla.Text != string.Empty)
                plantilla.Nombre = txtNombrePLantilla.Text;

            //Imagen Fondo
            if (hdnImagenFondo.Text != string.Empty)
                plantilla.ImagenFondo = hdnImagenFondo.Text;

            //Imagen Pendiente
            if (hdnActPendiente.Text != string.Empty)
                plantilla.ImagenPendiente = hdnActPendiente.Text;

            //Imaen Iniciado
            if (hdnActIniciada.Text != string.Empty)
                plantilla.ImagenIniciado = hdnActIniciada.Text;

            //Imagen Finalizado
            if (hdnFinalizada.Text != string.Empty)
                plantilla.ImagenFinalizado = hdnFinalizada.Text;

            //Imagen No Disponible
            if (hdnNoDisponible.Text != string.Empty)
                plantilla.ImagenNoDisponible = hdnNoDisponible.Text;

            //Imagen Flecha Arriba
            if (hdnFlechaArriba.Text != string.Empty)
                plantilla.ImagenFlechaArriba = hdnFlechaArriba.Text;

            //Imagen Flecha Abajo
            if (hdnFlechaAbajo.Text != string.Empty)
                plantilla.ImagenFlechaAbajo = hdnFlechaAbajo.Text;

            //Imagen Es Predeterminado
            if (ddlEsPredeterminado.SelectedValue != string.Empty)
                plantilla.EsPredeterminado = ddlEsPredeterminado.SelectedValue == "true";

            //Imagen Fecha de Registro
            plantilla.FechaRegistro = DateTime.Now;

            //Imagen Activo
            plantilla.Activo = true;

            //Posiciones de actividades
            plantilla.PosicionesActividades = ObtenerPuntos(PuntosA.Text);

            return plantilla;
        }

        private List<PosicionActividad> ObtenerPuntos(string Puntos)
        {
           
            List<PosicionActividad> posicionesAct = new List<PosicionActividad>();
            string[] coordenadasXY = Puntos.Split('|');
            int cont =0;
            foreach (string coordenada in coordenadasXY)
            {
                PosicionActividad posicion = new PosicionActividad();
                posicion.Orden = cont;
                string[] dato = coordenada.Split(',');
                posicion.PosicionX = decimal.Parse(dato[0]);
                posicion.PosicionY = decimal.Parse(dato[1]);
                cont++;
                posicionesAct.Add(posicion);
            }
            return posicionesAct;
        }

        private void ValidateEsPredeterminada()
        {
            PlantillaLudica plantillaPredeterminada = new PlantillaLudica();
            if (ddlEsPredeterminado.SelectedValue != string.Empty)
                plantillaPredeterminada.EsPredeterminado = ddlEsPredeterminado.SelectedValue == "true";

            if (plantillaPredeterminada.EsPredeterminado == true)
            {
                List<PlantillaLudica> plantillaEncontrada = controller.ConsultarPlantillasLudicas(plantillaPredeterminada);
                if (plantillaEncontrada.Count > 0)
                    ShowMessage("La plantilla lúdica " + plantillaEncontrada.FirstOrDefault().Nombre + "se encuentra como predeterminada, se realizará el cambio para esta nueva plantilla como predeterminada", MessageType.Information);
            }
        }

        private string ValidatePlantilla()
        {
            string sError = string.Empty;
            string sNombre = txtNombrePLantilla.Text.Trim();
            string sEsPredeterminado = ddlEsPredeterminado.SelectedValue;
            string sImagenFondo = hdnImagenFondo.Text.Trim();
            string sImagenPendiente = hdnActPendiente.Text.Trim();
            string sImagenIniciada = hdnActIniciada.Text.Trim();
            string sImagenFinalizada = hdnFinalizada.Text.Trim();
            string sImagenNoDisponible = hdnNoDisponible.Text.Trim();
            string sImagenFlechaArriba = hdnFlechaArriba.Text.Trim();
            string sImagenFlechaAbajo = hdnFlechaAbajo.Text.Trim();
            string sPosicionActividad = PuntosA.Text.Trim();
            

           if (string.IsNullOrEmpty(sNombre))
                sError += ", Nombre ";
            else if (sNombre.Length > 200)
                sError += ", Nombre máximo 200 caracteres";

           if (string.IsNullOrEmpty(sEsPredeterminado))
                sError += ", Es predeterminado ";

           if (string.IsNullOrEmpty(sImagenFondo))
               sError += ", Imagen fondo ";

           if (string.IsNullOrEmpty(sImagenPendiente))
               sError += ", Imagen actividad pendiente ";

           if (string.IsNullOrEmpty(sImagenIniciada))
               sError += ", Imagen actividad iniciada ";

           if (string.IsNullOrEmpty(sImagenFinalizada))
               sError += ", Imagen actividad finalizada ";

           if (string.IsNullOrEmpty(sImagenNoDisponible))
               sError += ", Imagen actividad no disponible ";

           if (string.IsNullOrEmpty(sImagenFlechaArriba))
               sError += ", Imagen flecha arriba ";

           if (string.IsNullOrEmpty(sImagenFlechaAbajo))
               sError += ", Imagen flecha abajo ";

           if (string.IsNullOrEmpty(sPosicionActividad))
               sError += ", Posiciones de las actividades ";

           if (sError != string.Empty)
               sError += "son Requeridos";
           return sError;
        }

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCONFIGURACIONPLATAFORMA) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARCONFIGURACIONPLATAFORMA) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARCONFIGURACIONPLATAFORMA) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!creacion)
                redirector.GoToHomePage(true);
            if (!lectura)
                redirector.GoToHomePage(true);
        }

        /// <summary>
        /// Crea un listado de FileWrapper a partir de los archivos en la información de la plantilla
        /// </summary>
        /// <param name="plantilla">Plantilla ludica de donde se tomaran la lista de archivos</param>
        /// <returns>Lista de FileWrapper</returns>
        private List<FileWrapper> CrearListaFileWrapper(PlantillaLudica plantilla)
        {
            List<FileWrapper> listaArchivos = new List<FileWrapper>();

            listaArchivos.Add(CrearFileWrapper(plantilla.ImagenFondo));
            listaArchivos.Add(CrearFileWrapper(plantilla.ImagenFlechaArriba));
            listaArchivos.Add(CrearFileWrapper(plantilla.ImagenFlechaAbajo));
            listaArchivos.Add(CrearFileWrapper(plantilla.ImagenPendiente));
            listaArchivos.Add(CrearFileWrapper(plantilla.ImagenIniciado));
            listaArchivos.Add(CrearFileWrapper(plantilla.ImagenFinalizado));
            listaArchivos.Add(CrearFileWrapper(plantilla.ImagenNoDisponible));

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

            FileStream fileStream = File.OpenRead(path);

            Byte[] arreglo = new Byte[fileStream.Length];
            BinaryReader reader = new BinaryReader(fileStream);
            arreglo = reader.ReadBytes(Convert.ToInt32(fileStream.Length));

            file.Data = arreglo;
            file.Lenght = Convert.ToInt32(fileStream.Length);

            string fName = string.Empty;
            string carpetaImages = configuracionGeneral.RutaPlantillas;
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

        private void EliminarImagen(TextBox hdnImage, Image img)
        {
            if (!string.IsNullOrEmpty(hdnImage.Text))
            {
                Image imgElim = img;

                imgElim.ImageUrl = "";
                hdnImage.Text = "";
                FileManagerCtrl fileManagerCtrl = new FileManagerCtrl();
                fileManagerCtrl.DeleteFile(Server.MapPath(RUTA_IMG_TEMPORAL) + hdnImage.Text);
            }
        }


        #endregion

        #region Sesiones
        #endregion
    }
}