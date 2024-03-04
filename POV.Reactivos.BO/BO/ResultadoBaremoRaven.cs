using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Reactivos.BO
{
    public class ResultadoBaremoRaven
    {
        public long BaremoID { get; set; }
        public string Edad { get; set; }
        public long Puntaje { get; set; }
        public long Percentil { get; set; }
    }
}
