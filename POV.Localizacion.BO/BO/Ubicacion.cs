using System;
using System.Text;
using System.Data;
using POV.Comun.BO;

namespace POV.Localizacion.BO
{
    /// <summary>
    /// Catalogo de alumnos disponibles en el sistema
    /// </summary>
    public class Ubicacion : ICloneable
    {
        private long? ubicacionID;
        /// <summary>
        /// Identificador automatico de Ubicacion
        /// </summary>
        public long? UbicacionID
        {
            get { return this.ubicacionID; }
            set { this.ubicacionID = value; }
        }
        private Pais pais;
        /// <summary>
        /// Pais
        /// </summary>
        public Pais Pais
        {
            get { return this.pais; }
            set { this.pais = value; }
        }
        private Estado estado;
        /// <summary>
        /// Estado
        /// </summary>
        public Estado Estado
        {
            get { return this.estado; }
            set { this.estado = value; }
        }
        private Ciudad ciudad;
        /// <summary>
        /// Ciudad
        /// </summary>
        public Ciudad Ciudad
        {
            get { return this.ciudad; }
            set { this.ciudad = value; }
        }
        private Localidad localidad;
        /// <summary>
        /// Localidad
        /// </summary>
        public Localidad Localidad
        {
            get { return this.localidad; }
            set { this.localidad = value; }
        }
        private Colonia colonia;
        /// <summary>
        /// Colonia
        /// </summary>
        public Colonia Colonia
        {
            get { return this.colonia; }
            set { this.colonia = value; }
        }

        private DateTime? fechaRegistro;
        public DateTime? FechaRegistro
        {
            get { return this.fechaRegistro; }
            set { this.fechaRegistro = value; }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
