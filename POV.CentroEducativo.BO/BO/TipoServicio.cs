using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using POV.CentroEducativo.BO;

namespace POV.CentroEducativo.BO { 
   /// <summary>
   /// Clase de tipo de servicio
   /// </summary>
   public class TipoServicio : ICloneable {
      private int? tipoServicioID;
      /// <summary>
      /// Indentificador del tipo de serviciop
      /// </summary>
      public int? TipoServicioID{
         get{ return this.tipoServicioID; }
         set{ this.tipoServicioID = value; }
      }
      private string clave;
      /// <summary>
      /// Clave del tipo de servicio
      /// </summary>
      public string Clave{
         get{ return this.clave; }
         set{ this.clave = value; }
      }
      private string nombre;
      /// <summary>
      /// Nombre del tipo de servicio
      /// </summary>
      public string Nombre{
         get{ return this.nombre; }
         set{ this.nombre = value; }
      }
      private NivelEducativo nivelEducativoID;
      /// <summary>
      /// Identificador del nivel educativo
      /// </summary>
      public NivelEducativo NivelEducativoID{
         get{ return this.nivelEducativoID; }
         set{ this.nivelEducativoID = value; }
      }

       public object Clone()
       {
           return this.MemberwiseClone();
       }
   } 
}
