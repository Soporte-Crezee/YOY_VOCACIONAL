using System;
using System.Collections.Generic;
using System.Text;
using POV.Modelo.BO;

namespace POV.Modelo.BO { 
   /// <summary>
   /// PropiedadClasificador
   /// </summary>
   public class PropiedadClasificador { 
      private PropiedadPersonalizada propiedad;
      /// <summary>
      /// PropiedadPersonalizada de la PropiedadClasificador
      /// </summary>
      public PropiedadPersonalizada Propiedad{
         get{ return this.propiedad; }
         set{ this.propiedad = value; }
      }
      private String descripcion;
      /// <summary>
      /// Descripción de la PropiedadClasificador
      /// </summary>
      public String Descripcion{
         get{ return this.descripcion; }
         set{ this.descripcion = value; }
      }
      private bool? activo;
      /// <summary>
      /// Estatus de la PropiedadClasificador Activo/Inactivo
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
