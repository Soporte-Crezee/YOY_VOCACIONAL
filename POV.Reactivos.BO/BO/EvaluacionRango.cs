using System;
using System.Collections.Generic;

namespace POV.Reactivos.BO { 
   /// <summary>
   /// Determina el rango para la evaluacion
   /// </summary>
   public class EvaluacionRango { 
      private Guid? evaluacionID;
      /// <summary>
      /// Identificador del rango de evaluacion
      /// </summary>
      public Guid? EvaluacionID{
         get{ return this.evaluacionID; }
         set{ this.evaluacionID = value; }
      }
      private short? inicio;
      /// <summary>
      /// Determina el inicio del rango de evaluacion
      /// </summary>
      public short? Inicio{
         get{ return this.inicio; }
         set{ this.inicio = value; }
      }
      private short? fin;
      /// <summary>
      /// Determina el fin del rango de evaluacion
      /// </summary>
      public short? Fin{
         get{ return this.fin; }
         set{ this.fin = value; }
      }
      private decimal? porcentajeCalificacion;
      /// <summary>
      /// Indica el porcentaje de calificacion
      /// </summary>
      public decimal? PorcentajeCalificacion{
         get{ return this.porcentajeCalificacion; }
         set{ this.porcentajeCalificacion = value; }
      }
   } 
}
