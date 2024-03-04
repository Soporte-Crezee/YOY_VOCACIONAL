using System;
using POV.Seguridad.BO;

namespace POV.Licencias.BO
{

    /// <summary>
    /// Licencia.
    /// </summary>
    public abstract class ALicencia : ICloneable
    {
        /// <summary>
        /// Identificador de la licencia.
        /// </summary>
        private Guid? licenciaID;
        public Guid? LicenciaID
        {
            get { return licenciaID; }
            set { licenciaID = value; }
        }
        /// <summary>
        /// Descontar de las licencias. Valor true para descontar, caso contrario false.
        /// </summary>
        public abstract bool Descontar { get; }
        private Usuario usuario;
        /// <summary>
        /// Usuario de la licencia.
        /// </summary>
        public Usuario Usuario
        {
            get { return this.usuario; }
            set { this.usuario = value; }
        }
        private bool? activo;
        /// <summary>
        /// Estado de la licencia. Valor true para activo, caso contrario false.
        /// </summary>
        public bool? Activo
        {
            get { return this.activo; }
            set { this.activo = value; }
        }
        /// <summary>
        /// Tipo de licencia.
        /// </summary>
        public abstract ETipoLicencia Tipo { get; }

        public abstract object Clone();
    }
}
