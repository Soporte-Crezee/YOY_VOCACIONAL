using System;


namespace POV.Expediente.BO { 
   /// <summary>
   /// AsignacionRecurso
   /// </summary>
   public abstract class AAsignacionRecurso { 
      private long? asignacionRecursoID;
      /// <summary>
      /// Identificador autonumerico
      /// </summary>
      public long? AsignacionRecursoID{
         get{ return this.asignacionRecursoID; }
         set{ this.asignacionRecursoID = value; }
      }
      private ARecurso recurso;
      /// <summary>
      /// RecursoAsignado
      /// </summary>
      public ARecurso Recurso{
         get{ return this.recurso; }
         set{ this.recurso = value; }
      }
       /// <summary>
       /// TipoAsignacionRecurso
       /// </summary>
     public abstract ETipoAsignacionRecurso TipoAsignacionRecurso { get;}
   
      private bool? estaConfirmado;
      /// <summary>
      /// EstaConfirmado
      /// </summary>
      public bool? EstaConfirmado{
         get{ return this.estaConfirmado; }
         set{ this.estaConfirmado = value; }
      }
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
