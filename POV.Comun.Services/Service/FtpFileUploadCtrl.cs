using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Comun.BO;
using System.IO;
using System.Net;

namespace POV.Comun.Service
{
    /// <summary>
    /// Controlador encargado del envio de archivos atraves de FTP
    /// </summary>
    public class FtpFileUploadCtrl : AFileUploadCtrl
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="serverUrl">URL del servidor FTP</param>
        /// <param name="username">nombre de usuario de conexion al servidor</param>
        /// <param name="password">contraseña de conexion al servidor</param>
        /// <param name="file">Archivo</param>
        /// <param name="filename">Nombre del archivo</param>
        public FtpFileUploadCtrl(string serverUrl, string username, string password)
            : base( serverUrl,  username,  password)
        {
        }
        /// <summary>
        /// Aloja el archivo en el servidor FTP
        /// </summary>
        public override void UploadFile(FileWrapper File, string Filename)
        {
            try
            {
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(this.ServerURL + File.Name);
                ftp.Credentials = new NetworkCredential(this.Username, this.Password);

                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;
                ftp.ContentLength = File.Lenght;

                byte[] data = File.Data;
                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(data, 0, data.Length);
                ftpstream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void DeleteFile(string Filename)
        {
            try
            {

                FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(this.ServerURL + Filename);
                ftp.Credentials = new NetworkCredential(this.Username, this.Password);
                ftp.Method = WebRequestMethods.Ftp.DeleteFile;

                FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Crea un directorio en el servidor FTP
        /// </summary>
        /// <param name="dir"></param>
        public override void MakeDir(string dir)
        {
            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(this.ServerURL + dir);
            ftp.Credentials = new NetworkCredential(this.Username, this.Password);
            ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
            FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();
            response.Close(); 
        }
    }
}
