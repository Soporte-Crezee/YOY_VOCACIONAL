using System;
using System.IO;
using Ionic.Zip;

namespace POV.Comun.Utils
{
    public static class ZipFileUtil
    {
        /// <summary>
        /// Comprime en un zip un conjunto de archivos
        /// </summary>
        /// <param name="filePaths">Archivos a comprimir</param>
        /// <param name="zipPath">Ubicación en la que se creará el zip</param>
        /// <param name="zipName">Nombre que se le dará al zip</param>
        /// <param name="comment">Comentario del zip</param>
        public static void ZipFiles(string[] filePaths, string zipPath, string zipName, string comment)
        {
            try
            {
                if (!Directory.Exists(zipPath))
                    throw new Exception("El directorio donde se guardará el zip no existe");

                if (!zipPath.EndsWith("\\"))
                    zipPath = zipPath + "\\";

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFiles(filePaths, false, "");
                    if (comment != null)
                        zip.Comment = comment;
                    zip.Save(string.Format("{0}{1}", zipPath, zipName));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo comprimir el directorio: " + ex.Message);
            }
        }
    }
}
