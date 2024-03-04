using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Licencias.BO
{
    public class Cliente : ICloneable
    {
        private string nombre;
        private string domicilio;
        private string representante;
        private string telefono;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Domicilio
        {
            get { return domicilio; }
            set { domicilio = value; }
        }

        public string Representante
        {
            get { return representante; }
            set { representante = value; }
        }

        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
