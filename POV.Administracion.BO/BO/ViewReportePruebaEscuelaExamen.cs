using System;
using System.Text;

namespace POV.Administracion.BO { 
   /// <summary>
   /// ViewReportePruebaEscuelaExamen
   /// </summary>
   public class ViewReportePruebaEscuelaExamen { 
      private Guid? iD;
      /// <summary>
      /// Identificador correspondiente a un GrupoCicloEscolar
      /// </summary>
      public Guid? ID{
         get{ return this.iD; }
         set{ this.iD = value; }
      }
      private int? escuelaID;
      /// <summary>
      /// Identificador correspondiente a un a la escuela de los grupos
      /// </summary>
      public int? EscuelaID{
         get{ return this.escuelaID; }
         set{ this.escuelaID = value; }
      }
      private int? grado;
      /// <summary>
      /// Grado del grupo
      /// </summary>
      public int? Grado{
          get { return this.grado; }
          set { this.grado = value; }
      }
      private Guid? grupoID;
      /// <summary>
      /// Identificador correspondiente a un Grupo
      /// </summary>
      public Guid? GrupoID
      {
          get { return this.grupoID; }
          set { this.grupoID = value; }
      }
      private string grupo;
      /// <summary>
      /// Nombre del grupo
      /// </summary>
      public string Grupo{
          get { return this.grupo; }
          set { this.grupo = value; }
      }
      private string gradoGrupo;
      /// <summary>
      /// Nombre y grado del grupo
      /// </summary>
      public string GradoGrupo
      {
          get { return this.gradoGrupo; }
          set { this.gradoGrupo = value; }
      }
      private int? pruebaTerminada;
      /// <summary>
      /// Alumnos que terminaron la prueba
      /// </summary>
      public int? PruebaTerminada{
         get{ return this.pruebaTerminada; }
         set{ this.pruebaTerminada = value; }
      }
      private int? pruebaPendiente;
      /// <summary>
      /// Alumnos que tienen la prueba inconclusa o pendiente
      /// </summary>
      public int? PruebaPendiente{
         get{ return this.pruebaPendiente; }
         set{ this.pruebaPendiente = value; }
      }
      private int? numAlumnos;
      /// <summary>
      /// Alumnos en el grupo
      /// </summary>
      public int? NumAlumnos{
         get{ return this.numAlumnos; }
         set{ this.numAlumnos = value; }
      }

       private int? pruebaID;
       public int? PruebaID
       {
           get { return this.pruebaID; }
           set { this.pruebaID = value; }
       }
   } 
}
