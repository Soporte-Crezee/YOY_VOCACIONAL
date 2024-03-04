using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Prueba.Reportes.BO { 
   /// <summary>
   /// Representa un detalle de resultado de prueba del Reporte.
   /// </summary>
   public class PruebaDinamicaDetail { 
      private string clave;
      /// <summary>
      /// Clave de la prueba.
      /// </summary>
      public string Clave{
         get{ return this.clave; }
         set{ this.clave = value; }
      }
      private string nombre;
      /// <summary>
      /// Nombre de la prueba.
      /// </summary>
      public string Nombre{
         get{ return this.nombre; }
         set{ this.nombre = value; }
      }
      private string modelo;
      /// <summary>
      /// Modelo de la prueba.
      /// </summary>
      public string Modelo{
         get{ return this.modelo; }
         set{ this.modelo = value; }
      }
      private string metodo;
      /// <summary>
      /// Método de calificación de la prueba.
      /// </summary>
      public string Metodo{
         get{ return this.metodo; }
         set{ this.metodo = value; }
      }
      private List<ResultadoDinamicaGrupoDetail> listaResultadoDinamicaGrupoDetail;
      /// <summary>
      /// Lista de resultado de la escuela.
      /// </summary>
      public List<ResultadoDinamicaGrupoDetail> ListaResultadoDinamicaGrupoDetail{
         get{ return this.listaResultadoDinamicaGrupoDetail; }
         set{ this.listaResultadoDinamicaGrupoDetail = value; }
      }
   } 
}
