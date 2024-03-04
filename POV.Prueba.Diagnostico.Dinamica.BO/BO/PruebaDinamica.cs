using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.Dinamica.BO
{
    public class PruebaDinamica : APrueba
    {
        public PruebaDinamica()
            : base()
        {
        }
        public PruebaDinamica(IEnumerable<APuntaje> listaEscalaDinamica)
            : base(listaEscalaDinamica)
        {
        }
        public override ETipoPrueba TipoPrueba
        {
            get { return ETipoPrueba.Dinamica; }
        }
    }
}
