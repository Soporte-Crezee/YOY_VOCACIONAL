using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.BO{ 
   /// <summary>
   /// Escala de Clasificacion Dinamica
   /// </summary>
   public class EscalaClasificacionDinamica: AEscalaDinamica {
       public override ETipoEscalaDinamica? TipoEscalaDinamica
       {
           get { return ETipoEscalaDinamica.CLASIFICACION; }
       }
   } 
}
