using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Operaciones.BO
{
    public class MatrizCalificacionTemporal
    {
        public decimal PorcentajeOpcionCorrecta { get; set; }

        public decimal PorcentajeIntervalo1 { get; set; }
        public decimal PorcentajeIntervalo2 { get; set; }
        public decimal PorcentajeIntervalo3 { get; set; }

        public short FinIntervalo1 { get; set; }
        public short FinIntervalo2 { get; set; }

        public List<BloqueCalificacionTemporal> bloques { get; set; }
    }
}
