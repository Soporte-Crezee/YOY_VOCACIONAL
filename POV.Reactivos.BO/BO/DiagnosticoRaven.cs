using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Reactivos.BO
{
    public class DiagnosticoRaven
    {
        public long Puntaje { get; set; }
        public int Percentil { get; set; }
        public string Rango { get; set; }
        public string Diagnostico { get; set; }
        public bool Validez { get; set; }
        public string Edad { get; set; }
        public Dictionary<string, int> PuntuacionDirecta { get; set; }
        public Dictionary<string, int> PuntuacionEsperada { get; set; }
        public Dictionary<string, int> Discrepancias { get; set; }
    }
}
