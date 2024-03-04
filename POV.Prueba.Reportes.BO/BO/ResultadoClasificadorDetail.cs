using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Prueba.Reportes.BO { 
   /// <summary>
   /// Representa un detalle de resultado de prueba del Reporte.
   /// </summary>
   public class ResultadoClasificadorDetail { 
      private string clasificador;
      /// <summary>
      /// Nombre del clasificador.
      /// </summary>
      public string Clasificador{
         get{ return this.clasificador; }
         set{ this.clasificador = value; }
      }
      private string nombre;
      /// <summary>
      /// Nombre del clasificador elegido.
      /// </summary>
      public string Nombre{
         get{ return this.nombre; }
         set{ this.nombre = value; }
      }
      private int? numero;
      /// <summary>
      /// Total del resultado de prueba.
      /// </summary>
      public int? Numero{
         get{ return this.numero; }
         set{ this.numero = value; }
      }

       private int? puntajeID;
       /// <summary>
       /// Identificador del puntaje
       /// </summary>
       public int? PuntajeID {
           get { return this.puntajeID; }
           set { this.puntajeID = value; }
      }

       private List<ResultadoEscalasDetail> listaEscalas;
       /// <summary>
       /// Lista de las escalas del clasificador.
       /// </summary>
       public List<ResultadoEscalasDetail> ListaEscalas { get { return listaEscalas; } set { this.listaEscalas = value; } }

   } 
}
