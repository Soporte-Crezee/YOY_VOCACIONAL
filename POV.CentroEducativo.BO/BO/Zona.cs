using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using POV.CentroEducativo.BO;
using POV.Localizacion.BO;

namespace POV.CentroEducativo.BO { 
   /// <summary>
   /// Clase de tipo Zona
   /// </summary>
   public class Zona : ICloneable { 
      private long? zonaID;
      /// <summary>
      /// Identificador de la zona
      /// </summary>
      public long? ZonaID{
         get{ return this.zonaID; }
         set{ this.zonaID = value; }
      }
      private string clave;
      /// <summary>
      /// Clave de la zona
      /// </summary>
      public string Clave{
         get{ return this.clave; }
         set{ this.clave = value; }
      }
      private string nombre;
      /// <summary>
      /// Nombre de la zona
      /// </summary>
      public string Nombre{
         get{ return this.nombre; }
         set{ this.nombre = value; }
      }
      private Ubicacion ubicacionID;
      /// <summary>
      /// Identificador de la ubicacion
      /// </summary>
      public Ubicacion UbicacionID{
         get{ return this.ubicacionID; }
         set{ this.ubicacionID = value; }
      }

       public object Clone()
       {
           return this.MemberwiseClone();
       }
   } 
}
