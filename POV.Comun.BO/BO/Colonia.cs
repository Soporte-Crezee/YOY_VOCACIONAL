using System;
using System.Text;

namespace POV.Comun.BO { 
   /// <summary>
   /// Colonia
   /// </summary>
   public class Colonia { 
      private int? coloniaID;
      /// <summary>
      /// Identificador autonumérico del Colonia
      /// </summary>
      public int? ColoniaID{
         get{ return this.coloniaID; }
         set{ this.coloniaID = value; }
      }
      private string nombre;
      /// <summary>
      /// Nombre
      /// </summary>
      public string Nombre{
         get{ return this.nombre; }
         set{ this.nombre = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// Fecha de Registro
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      private Localidad localidad;
      /// <summary>
      /// Localidad
      /// </summary>
      public Localidad Localidad{
         get{ return this.localidad; }
         set{ this.localidad = value; }
      }
   } 
}
