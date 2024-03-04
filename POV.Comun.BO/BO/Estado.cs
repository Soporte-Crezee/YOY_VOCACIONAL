using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Comun.BO
{
    /// <summary>
    /// Catálogo de Estados
    /// </summary>
    public class Estado
    {
        private int? estadoID;
        /// <summary>
        /// Identificador autonumérico del Estado
        /// </summary>
        public int? EstadoID
        {
            get { return this.estadoID; }
            set { this.estadoID = value; }
        }
        private string nombre;
        /// <summary>
        /// Nombre del estado
        /// </summary>
        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }

        /// <summary>
        /// Codigo del estado
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
        private Pais pais;
        /// <summary>
        /// Objeto País
        /// </summary>
        public Pais Pais
        {
            get { return this.pais; }
            set { this.pais = value; }
        }
    }
}
