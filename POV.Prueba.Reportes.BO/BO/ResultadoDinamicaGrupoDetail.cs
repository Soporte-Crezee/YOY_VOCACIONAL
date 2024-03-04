using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Prueba.Reportes.BO { 
   /// <summary>
   /// Representa un detalle de resultado de escuela del Reporte.
   /// </summary>
   public class ResultadoDinamicaGrupoDetail { 
      private string escuela;
      /// <summary>
      /// Nombre de la escuela.
      /// </summary>
      public string Escuela{
         get{ return this.escuela; }
         set{ this.escuela = value; }
      }
      private string turno;
      /// <summary>
      /// Turno de la escuela.
      /// </summary>
      public string Turno{
         get{ return this.turno; }
         set{ this.turno = value; }
      }
      private byte tipoTurno;
      /// <summary>
      /// Turno de la escuela.
      /// </summary>
      public byte TipoTurno
      {
          get { return this.tipoTurno; }
          set { this.tipoTurno = value; }
      }
      private string ciclo;
      /// <summary>
      /// Nombre del ciclo escolar.
      /// </summary>
      public string Ciclo{
         get{ return this.ciclo; }
         set{ this.ciclo = value; }
      }
      private string grupo;
      /// <summary>
      /// Grupo de la escuela.
      /// </summary>
      public string Grupo{
         get{ return this.grupo; }
         set{ this.grupo = value; }
      }
      private Guid grupoCicloEscolarID;
      /// <summary>
      /// GrupoCicloEscolarID de la escuela.
      /// </summary>
      public Guid GrupoCicloEscolarID
      {
          get { return this.grupoCicloEscolarID; }
          set { this.grupoCicloEscolarID = value; }
      }
      private List<ResultadoAlumnoDetail> listaResultadoAlumnoDetail;
      /// <summary>
      /// Lista de resultados de la prueba por alumno.
      /// </summary>
      public List<ResultadoAlumnoDetail> ListaResultadoAlumnoDetail{
         get{ return this.listaResultadoAlumnoDetail; }
         set{ this.listaResultadoAlumnoDetail = value; }
      }
      private List<ResultadoClasificadorDetail> listaResultadoClasificadorDetail;
      /// <summary>
      /// Lista de resultados de la prueba por clasificador.
      /// </summary>
      public List<ResultadoClasificadorDetail> ListaResultadoClasificadorDetail
      {
          get { return this.listaResultadoClasificadorDetail; }
          set { this.listaResultadoClasificadorDetail = value; }
      }
   } 
}
