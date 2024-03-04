using System;
using POV.Modelo.BO;

namespace POV.Expediente.BO { 
   /// <summary>
   /// ResultadoClasificador
   /// </summary>
   public abstract class AResultadoClasificador { 
      private long? resultadoClasificadorID;
      /// <summary>
      /// Identificador autonumerico
      /// </summary>
      public long? ResultadoClasificadorID{
         get{ return this.resultadoClasificadorID; }
         set{ this.resultadoClasificadorID = value; }
      }
      /// <summary>
      /// TipoResultadoClasificador
      /// </summary>
      public abstract ETipoResultadoClasificador TipoResultadoClasificador
      {
          get;
      }

      private DateTime? fechaRegistro;
      /// <summary>
      /// Fecha de creacion
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      private AModelo modelo;
      /// <summary>
      /// Modelo
      /// </summary>
      public AModelo Modelo{
         get{ return this.modelo; }
         set{ this.modelo = value; }
      }
      private AResultadoPrueba resultadoPrueba;
      /// <summary>
      /// ResultadoPrueba
      /// </summary>
      public AResultadoPrueba ResultadoPrueba{
         get{ return this.resultadoPrueba; }
         set{ this.resultadoPrueba = value; }
      }
   } 
}
