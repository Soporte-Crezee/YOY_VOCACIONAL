using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Expediente.BO
{
    public class UniversidadCarreraAspirante
    {
        public ExpedienteEscolar Expediente { get; set; }
        public long? ExpedienteEscolarID { get; set; }
        public UniversidadCarrera UniversidadCarrera { get; set; }
        public long? UniversidadCarreraID { get; set; }
    }
}
