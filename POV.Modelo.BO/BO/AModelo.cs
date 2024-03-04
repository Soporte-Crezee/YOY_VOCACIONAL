using System;
using System.Collections.Generic;
using System.Text;
using POV.Modelo.BO;

namespace POV.Modelo.BO { 
   /// <summary>
   /// Representa un modelo de pruebas
   /// </summary>
   public abstract class AModelo { 
      private int? modeloID;
      /// <summary>
      /// Identificador del modelo de pruebas
      /// </summary>
      public int? ModeloID{
         get{ return this.modeloID; }
         set{ this.modeloID = value; }
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
      /// Fecha de Registro
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      private bool? estatus;
      /// <summary>
      /// Estatus
      /// </summary>
      public bool? Estatus{
         get{ return this.estatus; }
         set{ this.estatus = value; }
      }

       private bool? esEditable;

       /// <summary>
       /// EsEditable
       /// </summary>
       public bool? EsEditable
       {
           get { return this.esEditable; }
           set { this.esEditable = value; }
       }


       /// <summary>
       /// ETipoModelo
       /// </summary>
       public abstract ETipoModelo? TipoModelo { get; }

   } 
}
