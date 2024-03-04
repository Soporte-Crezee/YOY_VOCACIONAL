using System;
using System.IO;
using System.Linq;
using POV.Comun.BO;

namespace POV.Operaciones.BO
{
    public class RecursoImagenTemp
    {

        public string[]  FileExtensions { get; set; }
        public int? FileSize { get; set; }
        private FileWrapper fileWrapper;
        public FileWrapper FileWrapper { get { return fileWrapper; } }

        /// <summary>
        /// Crea un archivo y valida que tenga la extensión correcta
        /// </summary>
        /// <param name="strm">Stream de bytes</param>
        /// <param name="length">Tamaño del archivo</param>
        /// <param name="strtype">tipo de archivo</param>
        /// <param name="name">nombre del archivo</param>
        public void CreateFileWrapper(Stream strm, int length, string strtype, string name)
        {
            if (FileExtensions == null || FileExtensions.Length <= 0) {
                FileExtensions = new[]{".gif",".jpg",".jpeg",".png"};
            }
            if (FileSize == null)
                FileSize = 1048576; //1MB

            string ext = System.IO.Path.GetExtension(name);
            

            if(FileExtensions.All(x=>x != ext.ToLower())){ throw new ArgumentOutOfRangeException(name,"El archivo proporcionado no tiene el formato o extensión correcta");}

            if(length>FileSize){throw new ArgumentOutOfRangeException("length","El archivo proporcionado excede el tamaño permitido");}
            
            fileWrapper = new FileWrapper(strm,length,strtype,name);
        }
    }
}
