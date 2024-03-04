using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Comun.BO
{
    /// <summary>
    /// Catálogo de Localidad
    /// </summary>
    public class Localidad
    {
        private int? localidadID;
        /// <summary>
        /// Identificador autonumérico de la Localidad
        /// </summary>
        public int? LocalidadID
        {
            get { return this.localidadID; }
            set { this.localidadID = value; }
        }

        private string nombre;
        /// <summary>
        /// Nombre de la Localidad
        /// </summary>
        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }

        /// <summary>
        /// Codigo de la Localidad
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
        private Ciudad ciudad;
        /// <summary>
        /// Objeto Ciudad
        /// </summary>
        public Ciudad Ciudad
        {
            get { return this.ciudad; }
            set { this.ciudad = value; }
        }
    }
}
