using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.ContenidosDigital.BO
{
    public class InstitucionOrigen : ICloneable
    {
        private string nombre;

        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
