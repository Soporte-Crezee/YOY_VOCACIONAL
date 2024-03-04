using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.BO{ 
   /// <summary>
   /// Escala de Seleccion Dinamica
   /// </summary>
   public class EscalaSeleccionDinamica: AEscalaDinamica {

       public override ETipoEscalaDinamica? TipoEscalaDinamica
       {
           get { return ETipoEscalaDinamica.SELECCION; }
       }
   } 
}
