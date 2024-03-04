using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Comun.BO
{
    /// <summary>
    /// Catálogo de Ciudad
    /// </summary>
    public class Ciudad
    {
        private int? ciudadID;
        /// <summary>
        /// Identificador autonumérico de la Ciudad
        /// </summary>
        public int? CiudadID
        {
            get { return this.ciudadID; }
            set { this.ciudadID = value; }
        }
        private string nombre;
        /// <summary>
        /// Nombre de la Ciudad
        /// </summary>
        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }

        /// <summary>
        /// Codigo de la ciudad
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
        private Estado estado;
        /// <summary>
        /// Objeto Estado
        /// </summary>
        public Estado Estado
        {
            get { return this.estado; }
            set { this.estado = value; }
        }
    }
}
