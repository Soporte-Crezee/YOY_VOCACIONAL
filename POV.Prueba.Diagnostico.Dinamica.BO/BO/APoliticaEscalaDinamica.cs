using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.BO
{
     public abstract class APoliticaEscalaDinamica
    {
         public abstract bool Validar(PruebaDinamica pruebadinamica, AEscalaDinamica nuevo);
    }
}
