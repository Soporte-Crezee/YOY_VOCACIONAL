using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.BO{ 
   /// <summary>
   /// Escala de Porcentaje Dinamica
   /// </summary>
   public class EscalaPorcentajeDinamica: AEscalaDinamica {
       public override ETipoEscalaDinamica? TipoEscalaDinamica
       {
           get { return ETipoEscalaDinamica.PORCENTA; }
       }
   } 
}
