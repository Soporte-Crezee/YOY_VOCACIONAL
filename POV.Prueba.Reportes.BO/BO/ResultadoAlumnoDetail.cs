using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Prueba.Reportes.BO { 
   /// <summary>
   /// Representa un detalle de resultado de alumno del Reporte.
   /// </summary>
   public class ResultadoAlumnoDetail { 
      private int? noAlumno;
      /// <summary>
      /// Número de alumno dentro del reporte.
      /// </summary>
      public int? NoAlumno{
         get{ return this.noAlumno; }
         set{ this.noAlumno = value; }
      }
      private string alumno;
      /// <summary>
      /// Nombre del alumno.
      /// </summary>
      public string Alumno{
         get{ return this.alumno; }
         set{ this.alumno = value; }
      }
      private string sexo;
      /// <summary>
      /// Sexo del alumno.
      /// </summary>
      public string Sexo{
         get{ return this.sexo; }
         set{ this.sexo = value; }
      }
      private int? edad;
      /// <summary>
      /// Edad del alumno.
      /// </summary>
      public int? Edad{
         get{ return this.edad; }
         set{ this.edad = value; }
      }
   } 
}
