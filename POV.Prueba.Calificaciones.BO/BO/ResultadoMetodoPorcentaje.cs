using System;
using System.Collections.Generic;
using System.Text;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Calificaciones.BO { 
   /// <summary>
   /// Prueba Calificacion
   /// </summary>
   public class ResultadoMetodoPorcentaje: AResultadoMetodoCalificacion { 
      private int? numeroAciertos;
      /// <summary>
      /// Numero de Aciertos
      /// </summary>
      public int? NumeroAciertos{
         get{ return this.numeroAciertos; }
         set{ this.numeroAciertos = value; }
      }
      private int? totalAciertos;
      /// <summary>
      /// Total de Aciertos
      /// </summary>
      public int? TotalAciertos{
         get{ return this.totalAciertos; }
         set{ this.totalAciertos = value; }
      }
      private bool? esAproximado;
      /// <summary>
      /// Verifica si el valor fue aproximado
      /// </summary>
      public bool? EsAproximado{
         get{ return this.esAproximado; }
         set{ this.esAproximado = value; }
      }
      private EscalaPorcentajeDinamica escalaPorcentajeDinamica;
      /// <summary>
      /// EscalaPorcentajeDinamica del resultado metodo porcentaje
      /// </summary>
      public EscalaPorcentajeDinamica EscalaPorcentajeDinamica{
         get{ return this.escalaPorcentajeDinamica; }
         set{ this.escalaPorcentajeDinamica = value; }
      }
      /// <summary>
      /// Tipo de Resultado del metodo
      /// </summary>
      public override ETipoResultadoMetodo TipoResultadoMetodo {
          get
          {
              return ETipoResultadoMetodo.PORCENTAJE;
          } 
      }

      /// <summary>
      /// Obtiene el Clasificador de Seleccion del Metodo de Calificacion
      /// </summary>
      public override Modelo.BO.Clasificador ObtenerClasificadorPredominante()
      {
          return this.escalaPorcentajeDinamica.Clasificador;
      }
   } 
}
