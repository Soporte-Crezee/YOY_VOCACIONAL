using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Comun.BO;
namespace POV.Comun.Service
{
    /// <summary>
    /// Controlador encargado de alojar un archivo en el sistema de archivos local
    /// </summary>
    public class LocalFileUploadCtrl: AFileUploadCtrl
    {
        private FileManagerCtrl fileManager = new FileManagerCtrl();
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="serverUrl">ubicacion local donde se alojan los archivos</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="file">Archivo</param>
        /// <param name="filename">Nombre de archivo</param>
        public LocalFileUploadCtrl(string serverUrl, string username, string password)
            : base( serverUrl,  username,  password)
        {
        }
        /// <summary>
        /// Aloja un archivo en la ubicacion local
        /// </summary>
        public override void UploadFile(FileWrapper File, string Filename)
        {
            byte[] data = File.Data;

            fileManager.WriteFile(string.Format("{0}{1}", ServerURL, Filename), ref data);
        }
        /// <summary>
        /// Elimina el archivo de la ubicacion local
        /// </summary>
        public override void DeleteFile(string Filename)
        {
            fileManager.DeleteFile(string.Format("{0}{1}", ServerURL, Filename));
        }

        /// <summary>
        /// Crea un directorio en la ubicacion local
        /// </summary>
        /// <param name="dir"></param>
        public override void MakeDir(string dir)
        {
            fileManager.CreateFolder(dir);
        }
    }
}
