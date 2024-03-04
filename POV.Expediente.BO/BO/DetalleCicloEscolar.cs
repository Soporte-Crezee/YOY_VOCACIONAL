using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using POV.CentroEducativo.BO;

namespace POV.Expediente.BO
{ 
   /// <summary>
   /// Detalle de un ciclo escolar
   /// </summary>
   public class DetalleCicloEscolar { 
      private int? detalleCicloEscolarID;
      /// <summary>
      /// Identificador del detalle del ciclo escolar
      /// </summary>
      public int? DetalleCicloEscolarID{
         get{ return this.detalleCicloEscolarID; }
         set{ this.detalleCicloEscolarID = value; }
      }
      private GrupoCicloEscolar grupoCicloEscolar;
      /// <summary>
      /// Grupo ciclo escolar del detalle
      /// </summary>
      public GrupoCicloEscolar GrupoCicloEscolar{
         get{ return this.grupoCicloEscolar; }
         set{ this.grupoCicloEscolar = value; }
      }
      private Escuela escuela;
      /// <summary>
      /// Escuela del detalle
      /// </summary>
      public Escuela Escuela{
         get{ return this.escuela; }
         set{ this.escuela = value; }
      }
      private List<AResultadoPrueba> resultadosPrueba;
      /// <summary>
      /// Lista de resultados de pruebas
      /// </summary>
      public List<AResultadoPrueba> ResultadosPrueba{
         get{ return this.resultadosPrueba; }
         set{ this.resultadosPrueba = value; }
      }

      private List<AAsignacionRecurso> asignacionesRecurso;
       /// <summary>
       /// Lista de asignaciones de recursos
       /// </summary>
      public List<AAsignacionRecurso> AsignacionesRecurso
      {
          get { return this.asignacionesRecurso; }
          set { this.asignacionesRecurso = value; }
      }

      private bool? activo;
      /// <summary>
      /// Activo
      /// </summary>
      public bool? Activo{
         get{ return this.activo; }
         set{ this.activo = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// Fecha de Registro
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
   } 
}
