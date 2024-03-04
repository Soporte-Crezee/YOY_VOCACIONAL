using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace POV.Comun.Service
{
    public class FileManagerCtrl
    {

        public void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (IOException e)
                {
                    return;
                }
            }
        }
        /// <summary>
        /// Elimina los archivos de un directorio apartir de un tiempo pasado en segundos de la fecha de creacion del archivo
        /// </summary>
        /// <param name="directory">Directorio donde se alojan los archivos</param>
        /// <param name="seconds">tiempo en segundos en que se ha creado el archivo</param>
        public void DeleteOldFilesDirectory(string directory, double seconds)
        {
            DirectoryInfo d = new DirectoryInfo(directory);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles();
            
            foreach (FileInfo file in Files)
            {
                if (DateTime.Now.Subtract(file.CreationTime).TotalSeconds >= seconds)
                {
                    DeleteFile(file.FullName);
                }
            }
        }

        public void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        public void WriteFile(string path, ref byte[] Buffer)
        {
            // Create a file
            FileStream newFile = new FileStream(path, FileMode.Create);

            // Write data to the file
            newFile.Write(Buffer, 0, Buffer.Length);

            // Close file
            newFile.Close();
        }
    }
}
