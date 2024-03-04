using System;

namespace POV.Expediente.BO { 
   /// <summary>
   /// ARecurso
   /// </summary>
   public abstract class ARecurso { 
      private long? recursoID;
      /// <summary>
      /// Identificador autonumerico
      /// </summary>
      public long? RecursoID{
         get{ return this.recursoID; }
         set{ this.recursoID = value; }
      }

       /// <summary>
       /// TipoRecurso
       /// </summary>
      public abstract ETipoRecurso TipoRecurso { get; }

       private DateTime? fechaRegistro;
      /// <summary>
      /// Fecha de creacion
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
   } 
}
