using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace POV.Comun.BO
{
    /// <summary>
    /// Clase que envuelve las propiedades de un archivo
    /// </summary>
    public class FileWrapper
    {
        /// <summary>
        /// Arreglo que bytes que contiene la información del archivo
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// Tamanio del archivo
        /// </summary>
        public int Lenght { get; set; }
        /// <summary>
        /// Tipo de archivo
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public string Name { get; set; }

        public FileWrapper() { }
        public FileWrapper(Stream strm, int lenght, string strtype, string name)
        {
            byte[] data = new byte[lenght];
            int n = strm.Read(data, 0, lenght);


            this.Data = data;
            this.Lenght = lenght;
            this.Name = name;
            this.Type = strtype;

        }
    }
}
