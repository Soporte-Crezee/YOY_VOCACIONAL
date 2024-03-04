using System;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;

namespace POV.Licencias.BO
{
    /// <summary>
    /// Licencias de Escuela temporales.
    /// </summary>
    public class LicenciaEscuelaTemporal : LicenciaEscuela, ICloneable
    {
        private DateTime? fechaInicio;
        /// <summary>
        /// Fecha de inicio de la vigencia las licencias.
        /// </summary>
        public DateTime? FechaInicio
        {
            get { return this.fechaInicio; }
            set { this.fechaInicio = value; }
        }
        private DateTime? fechaFin;
        /// <summary>
        /// Fecha de fin de la vigencia de la licencias.
        /// </summary>
        public DateTime? FechaFin
        {
            get { return this.fechaFin; }
            set { this.fechaFin = value; }
        }

        /// <summary>
        /// Indica si el licenciamiento es temporal. Valor TEMPORAL para temporal, caso contrario false.
        /// </summary>
        public override ELicenciaEscuela Tipo
        {
            get { return ELicenciaEscuela.TEMPORAL; }
        }

        /// <summary>
        /// Implementación de IClonable
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
