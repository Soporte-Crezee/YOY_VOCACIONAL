using System;
using System.Collections.Generic;
using System.Text;
using POV.Modelo.BO;

namespace POV.Modelo.BO { 
   /// <summary>
   /// PropiedadPersonalizada
   /// </summary>
   public class PropiedadPersonalizada { 
      private Int32? propiedadID;
      /// <summary>
      /// Identificador de la PropiedadPersonalizada
      /// </summary>
      public Int32? PropiedadID{
         get{ return this.propiedadID; }
         set{ this.propiedadID = value; }
      }
      private String nombre;
      /// <summary>
      /// Nombre de la PropiedadPersonalizada
      /// </summary>
      public String Nombre{
         get{ return this.nombre; }
         set{ this.nombre = value; }
      }
      private String descripcion;
      /// <summary>
      /// Descripción de la PropiedadPersonalizada
      /// </summary>
      public String Descripcion{
         get{ return this.descripcion; }
         set{ this.descripcion = value; }
      }
      private bool? esVisible;
      /// <summary>
      /// La PropiedadPersonalizada EsVisible Si/No
      /// </summary>
      public bool? EsVisible
      {
          get { return this.esVisible; }
          set { this.esVisible = value; }
      }
      private bool? activo;
      /// <summary>
      /// Estatus de la PropiedadPersonalizada Activo/Inactivo
      /// </summary>
      public bool? Activo{
         get{ return this.activo; }
         set{ this.activo = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// Fecha de registro
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
   } 
}
