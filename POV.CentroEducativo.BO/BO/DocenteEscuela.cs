// Clase docente para la red social PISA
using System;
using POV.Localizacion.BO;

namespace POV.CentroEducativo.BO { 
   /// <summary>
   /// Catalogo de Docentes disponibles en el sistema
   /// </summary>
   public class DocenteEscuela: ICloneable { 
      private long? docenteEscuelaID;
      /// <summary>
      /// Identificador autonumerico de sistema para la el docente
      /// </summary>
      public long? DocenteEscuelaID{
         get{ return this.docenteEscuelaID; }
         set{ this.docenteEscuelaID = value; }
      }
      private int? escuelaID;
      /// <summary>
      /// Identificador de la escuela
      /// </summary>
      public int? EscuelaID{
         get{ return this.escuelaID; }
         set{ this.escuelaID = value; }
      }
      private int? docenteID;
      /// <summary>
      /// Identificador del docente
      /// </summary>
      public int? DocenteID{
         get{ return this.docenteID; }
         set{ this.docenteID = value; }
      }
      private bool? estatus;
      /// <summary>
      /// Estatus de la relacion
      /// </summary>
      public bool? Estatus{
         get{ return this.estatus; }
         set{ this.estatus = value; }
      }
        public object Clone ( )
        {
        return this.MemberwiseClone( );
        }
   } 
}
