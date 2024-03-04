using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Reactivos.BO { 
   /// <summary>
   /// Clase especializada de caracteristicas diagnostico
   /// </summary>
   public class CaracteristicasDiagnostico: Caracteristicas { 
      private int? version;
      /// <summary>
      /// Versión actual del reactivo
      /// </summary>
      public int? Version{
         get{ return this.version; }
         set{ this.version = value; }
      }
      private Guid? reactivoBaseID;
      /// <summary>
      /// Identificador del reactivo base
      /// </summary>
      public Guid? ReactivoBaseID{
         get{ return this.reactivoBaseID; }
         set{ this.reactivoBaseID = value; }
      }     
      private List<EvaluacionRango> listaEvaluacionRango;
      /// <summary>
      /// Rangos para la evaluacion
      /// </summary>
      public List<EvaluacionRango> ListaEvaluacionRango{
         get{ return this.listaEvaluacionRango; }
         set{ this.listaEvaluacionRango = value; }
      }
   } 
}
