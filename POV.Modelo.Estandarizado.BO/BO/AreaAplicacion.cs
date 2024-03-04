using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Modelo.Estandarizado.BO
{
    /// <summary>
    /// AreaAplicacion
    /// </summary>
    public class AreaAplicacion : ICloneable
    {
        private int? areaAplicacionID;
        /// <summary>
        /// identificador de la area de aplicacion
        /// </summary>
        public int? AreaAplicacionID
        {
            get { return this.areaAplicacionID; }
            set { this.areaAplicacionID = value; }
        }
        private string descripcion;
        /// <summary>
        /// descripcion de la area de aplicacion
        /// </summary>
        public string Descripcion
        {
            get { return this.descripcion; }
            set { this.descripcion = value; }
        }

        public string Thumb
        {
            get
            {
                if (Descripcion != null)
                {
                    if (this.descripcion.CompareTo("MATEMATICAS") == 0 || this.descripcion.CompareTo("MATEMÁTICAS") == 0)
                    {
                        return "icon_matematicas.png";
                    }
                    if (this.descripcion.CompareTo("CIENCIAS") == 0)
                    {
                        return "icon_ciencias.png";
                    }
                    if (this.descripcion.CompareTo("LECTURA") == 0)
                    {
                        return "icon_lectura.png";
                    }
                }
                return "";
            }
            set
            {

            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
