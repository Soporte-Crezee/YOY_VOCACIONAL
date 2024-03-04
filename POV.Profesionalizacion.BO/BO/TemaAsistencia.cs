using System;

namespace POV.Profesionalizacion.BO { 
   /// <summary>
   /// TemaAsistencia
   /// </summary>
   public class TemaAsistencia { 
      private int? temaAsistenciaID;
      /// <summary>
      /// Identificador unico
      /// </summary>
      public int? TemaAsistenciaID{
         get{ return this.temaAsistenciaID; }
         set{ this.temaAsistenciaID = value; }
      }
      private string nombre;
      /// <summary>
      /// Nombre
      /// </summary>
      public string Nombre{
         get{ return this.nombre; }
         set{ this.nombre = value; }
      }
      private string descripcion;
      /// <summary>
      /// Descripcion
      /// </summary>
      public string Descripcion{
         get{ return this.descripcion; }
         set{ this.descripcion = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// Fecha Alta
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      private bool? activo;
      /// <summary>
      /// Activo
      /// </summary>
      public bool? Activo{
         get{ return this.activo; }
         set{ this.activo = value; }
      }
   } 
}
