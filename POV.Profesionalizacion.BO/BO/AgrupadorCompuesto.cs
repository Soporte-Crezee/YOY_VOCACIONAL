using System;
using System.Collections.Generic;

namespace POV.Profesionalizacion.BO { 
   /// <summary>
   /// Agrupador Compuesto
   /// </summary>
   public class AgrupadorCompuesto: AAgrupadorContenidoDigital {
       /// <summary>
       /// Lista de Agrupadores de Contenido Digital
       /// </summary>
       private List<AAgrupadorContenidoDigital> agrupadoresContenido = new List<AAgrupadorContenidoDigital>();

      public override ETipoAgrupador TipoAgrupador
      {
          get
          {
              return ETipoAgrupador.COMPUESTO;
          }
      }

      public override void Agregar(AAgrupadorContenidoDigital agrupadorContenido)
      {
          agrupadoresContenido.Add(agrupadorContenido);
      }

      public override void Eliminar(AAgrupadorContenidoDigital agrupadorContenido)
      {
          agrupadoresContenido.Remove(agrupadorContenido);
      }

      public virtual List<AAgrupadorContenidoDigital> AgrupadoresContenido
      {
          get { return this.agrupadoresContenido; }
      }

      public void InitList()
      {
          this.agrupadoresContenido = new List<AAgrupadorContenidoDigital>();
      }
   } 
}
