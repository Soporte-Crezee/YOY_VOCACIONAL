using System;

namespace POV.Profesionalizacion.BO { 
   /// <summary>
   /// Agrupador Simple
   /// </summary>
   public class AgrupadorSimple: AAgrupadorContenidoDigital {

       public override ETipoAgrupador TipoAgrupador
       {
           get
           {
               return ETipoAgrupador.SIMPLE;
           }
       }

       public override void Agregar(AAgrupadorContenidoDigital agrupadorContenido)
       {
           throw new NotImplementedException();
       }

       public override void Eliminar(AAgrupadorContenidoDigital agrupadorContenido)
       {
           throw new NotImplementedException();
       }
   } 
}
