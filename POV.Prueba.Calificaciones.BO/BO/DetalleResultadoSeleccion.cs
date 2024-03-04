using System;
using System.Collections.Generic;

using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Calificaciones.BO { 
   /// <summary>
   /// DetalleResultadoEstandarizado
   /// </summary>
   public class DetalleResultadoSeleccion {
       private int? detalleResultadoID;
       /// <summary>
       /// Identificador de DetalleResultadoSeleccion
       /// </summary>
       public int? DetalleResultadoID
       {
           get
           {
               return detalleResultadoID;
           }
           set
           {
               detalleResultadoID = value;
           }
       }
       private decimal? valor;
      /// <summary>
      /// valor del resultado
      /// </summary>
      public decimal? Valor{
         get{ return this.valor; }
         set{ this.valor = value; }
      }
      private bool? esAproximado;
      /// <summary>
      /// Determina si se aproximará o no, el valor hacia la siguiente escala dinámica
      /// </summary>
      public bool? EsAproximado{
         get{ return this.esAproximado; }
         set{ this.esAproximado = value; }
      }
      private EscalaSeleccionDinamica escalaSeleccionDinamica;
       /// <summary>
       /// La escala de selección dinámica
       /// </summary>
      public EscalaSeleccionDinamica EscalaSeleccionDinamica {
          get { return this.escalaSeleccionDinamica; }
          set { this.escalaSeleccionDinamica = value; }
       }
   } 
}
