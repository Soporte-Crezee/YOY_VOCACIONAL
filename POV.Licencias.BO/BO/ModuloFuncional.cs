using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Licencias.BO
{
    /// <summary>
    /// Esta clase representa un modulo funcional del sistema NATWARE
    /// </summary>
    public class ModuloFuncional
    {
        private int? moduloFuncionalId;

        /// <summary>
        /// Identificador del modulo funcional
        /// </summary>
        public int? ModuloFuncionalId
        {
            get { return moduloFuncionalId; }
            set { moduloFuncionalId = value; }
        }

        private string clave;
        /// <summary>
        /// Clave del modulo funcional
        /// </summary>
        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }

        private string nombre;

        /// <summary>
        /// Nombre del modulo funcional
        /// </summary>
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string descripcion;

        /// <summary>
        /// Descripcion del modulo funcional
        /// </summary>
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

    }
}
