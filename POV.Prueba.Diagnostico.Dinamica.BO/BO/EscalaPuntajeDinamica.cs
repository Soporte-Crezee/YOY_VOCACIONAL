using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.BO{ 
   /// <summary>
   /// Escala de Puntaje Dinamica
   /// </summary>
   public class EscalaPuntajeDinamica: AEscalaDinamica {

       public override ETipoEscalaDinamica? TipoEscalaDinamica
       {
           get { return ETipoEscalaDinamica.PUNTAJE; }
       }
   } 
}
