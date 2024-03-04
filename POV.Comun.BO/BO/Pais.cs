using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Comun.BO
{
    /// <summary>
    /// Catálogo de Paises
    /// </summary>
    public class Pais
    {
        private int? paisID;
        /// <summary>
        /// Identificador autonumérico del País
        /// </summary>
        public int? PaisID
        {
            get { return this.paisID; }
            set { this.paisID = value; }
        }
        private string nombre;
        /// <summary>
        /// Nombre del país
        /// </summary>
        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }

        /// <summary>
        /// Codigo de país
        /// </summary>
        private string codigo;
        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha de Registro
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }
    }
}
