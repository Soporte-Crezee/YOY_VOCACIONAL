using System;

namespace POV.Profesionalizacion.BO { 
   /// <summary>
   /// Asistencia
   /// </summary>
   public class Asistencia:AgrupadorCompuesto { 
      private string descripcion;
      /// <summary>
      /// Descripcion
      /// </summary>
      public string Descripcion{
         get{ return this.descripcion; }
         set{ this.descripcion = value; }
      }
      private TemaAsistencia temaAsistencia;
      /// <summary>
      /// TemaAsistencia
      /// </summary>
      public TemaAsistencia TemaAsistencia{
         get{ return this.temaAsistencia; }
         set{ this.temaAsistencia = value; }
      }

      public override ETipoAgrupador TipoAgrupador
      {
          get
          {
              return ETipoAgrupador.COMPUESTO_ASISTENCIA;
          }
      }

   } 
}
