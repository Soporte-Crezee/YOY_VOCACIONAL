using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.CentroEducativo.BO;

namespace POV.ConfiguracionActividades.BO
{
    /// <summary>
    /// Clase que representa un bloque de periodos de fechas
    /// </summary>
    public class BloqueActividad
    {
        /// <summary>
        /// identificador del bloque
        /// </summary>
        public int? BloqueActividadId { get; set; }

        /// <summary>
        /// Nombre del bloque
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Fecha de inicio
        /// </summary>
        public DateTime? FechaInicio { get; set; }

        /// <summary>
        /// Fecha de fin
        /// </summary>
        public DateTime? FechaFin { get; set; }

        /// <summary>
        /// Activo
        /// </summary>
        public bool? Activo { get; set; }

        /// <summary>
        /// Fecha de registro
        /// </summary>
        public DateTime? FechaRegistro { get; set; }

        /// <summary>
        /// Escuela del bloque
        /// </summary>
        public virtual Escuela Escuela { get; set; }

        /// <summary>
        /// Foreign key de la escuela
        /// </summary>
        public int? EscuelaId { get; set; }

        /// <summary>
        /// Ciclo escolar
        /// </summary>
        public virtual CicloEscolar CicloEscolar { get; set; }

        /// <summary>
        /// Foreign key del ciclo escolar
        /// </summary>
        public int? CicloEscolarId { get; set; }

        /// <summary>
        /// Version para la actualizacion optimista
        /// </summary>
        public virtual byte[] Version { get; set; }
        
    }
}
