using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.CentroEducativo.BO
{
    public class UniversidadCarrera
    {
        public int? UniversidadCarreraID { get; set; }
        public Universidad Universidad { get; set; }
        public long? UniversidadID { get; set; }
        public Carrera Carrera { get; set; }
        public long? CarreraID { get; set; }
    }
}
