using System;
using System.Collections.Generic;
using System.Text;
using POV.Modelo.BO;

namespace POV.Modelo.BO { 
   /// <summary>
   /// Clasificador
   /// </summary>
   public class Clasificador { 
      private Int32? clasificadorID;
      /// <summary>
      /// Identificador del Clasificador
      /// </summary>
      public Int32? ClasificadorID{
         get{ return this.clasificadorID; }
         set{ this.clasificadorID = value; }
      }
      private String nombre;
      /// <summary>
      /// Nombre del Clasificador
      /// </summary>
      public String Nombre{
         get{ return this.nombre; }
         set{ this.nombre = value; }
      }
      private String descripcion;
      /// <summary>
      /// Descripción del Clasificador
      /// </summary>
      public String Descripcion{
         get{ return this.descripcion; }
         set{ this.descripcion = value; }
      }
      private bool? activo;
      /// <summary>
      /// Estatus del Clasificador Activo/Inactivo
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
      private List<PropiedadClasificador> listaPropiedadClasificador;
      /// <summary>
      /// lista de Propiedades de Clasificador
      /// </summary>
      public List<PropiedadClasificador> ListaPropiedadClasificador{
         get{ return this.listaPropiedadClasificador; }
         set{ this.listaPropiedadClasificador = value; }
      }
   } 
}
