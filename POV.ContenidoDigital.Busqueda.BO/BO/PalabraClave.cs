// Clase PalabraClave para los Tags del buscador de contenido
using System;

namespace POV.ContenidosDigital.Busqueda.BO { 
   /// <summary>
   /// Palabra Clave
   /// </summary>
   public class PalabraClave { 
      private Int64? palabraClaveID;
      /// <summary>
      /// Identificador de la Palabra Clave
      /// </summary>
      public Int64? PalabraClaveID{
         get{ return this.palabraClaveID; }
         set{ this.palabraClaveID = value; }
      }
      private String tag;
      /// <summary>
      /// Palabra Clave o Etiqueta
      /// </summary>
      public String Tag{
         get{ return this.tag; }
         set{ this.tag = value; }
      }
      private ETipoPalabraClave? tipoPalabraClave;
      /// <summary>
      /// Tipo de la Palabra Clave
      /// </summary>
      public ETipoPalabraClave? TipoPalabraClave{
         get{ return this.tipoPalabraClave; }
         set{ this.tipoPalabraClave = value; }
      }
   } 
}
