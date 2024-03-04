using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.BO
{
    public class EventoUniversidad
    {
        public long? EventoUniversidadId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public virtual Universidad Universidad { get; set; }
        public long? UniversidadId { get; set; }
    }
}
