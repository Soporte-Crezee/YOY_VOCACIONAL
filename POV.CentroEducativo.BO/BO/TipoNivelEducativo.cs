using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using POV.CentroEducativo.BO;

namespace POV.CentroEducativo.BO { 
   /// <summary>
   /// Clase para el tipo de nivel educativo
   /// </summary>
   public class TipoNivelEducativo { 
      private int? tipoNivelEducativoID;
      /// <summary>
      /// Identificador del tipo de nivel educativo
      /// </summary>
      public int? TipoNivelEducativoID{
         get{ return this.tipoNivelEducativoID; }
         set{ this.tipoNivelEducativoID = value; }
      }
      private string clave;
      /// <summary>
      /// Clave del tipo de nivel educativo
      /// </summary>
      public string Clave{
         get{ return this.clave; }
         set{ this.clave = value; }
      }
      private string nombre;
      /// <summary>
      /// Nombre del tipo de nivel educativo
      /// </summary>
      public string Nombre{
         get{ return this.nombre; }
         set{ this.nombre = value; }
      }
   } 
}
