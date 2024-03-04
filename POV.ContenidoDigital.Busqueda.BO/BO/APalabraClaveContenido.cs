using System;

namespace POV.ContenidosDigital.Busqueda.BO
{
    // Clase Abstracta para Palabras Clave de Contenido
    /// <summary>
    /// Palabra Clave de Contenido
    /// </summary>
    public abstract class APalabraClaveContenido
    {
        private Int64? palabraClaveContenidoID;
        /// <summary>
        /// Identificador de PalabraClaveContenido
        /// </summary>
        public Int64? PalabraClaveContenidoID
        {
            get { return this.palabraClaveContenidoID; }
            set { this.palabraClaveContenidoID = value; }
        }
        private DateTime? fechaRegistro;
        /// <summary>
        /// Fecha de registro
        /// </summary>
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }

        private PalabraClave palabraClave;
        /// <summary>
        /// Palabra Clave
        /// </summary>
        public PalabraClave PalabraClave
        {
            get { return this.palabraClave; }
            set { this.palabraClave = value; }
        }
    }
}
