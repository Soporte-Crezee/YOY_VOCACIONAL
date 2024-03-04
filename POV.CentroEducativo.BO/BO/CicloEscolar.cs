using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using POV.CentroEducativo.BO;
using POV.Localizacion.BO;

namespace POV.CentroEducativo.BO { 
   /// <summary>
   /// Clase tipo Ciclo Escolar
   /// </summary>
   public class CicloEscolar:ICloneable { 
      private int? cicloEscolarID;
      /// <summary>
      /// Identificador del ciclo escolar
      /// </summary>
      public int? CicloEscolarID{
         get{ return this.cicloEscolarID; }
         set{ this.cicloEscolarID = value; }
      }
      private string titulo;
      /// <summary>
      /// Nombre del ciclo escolar
      /// </summary>
      public string Titulo{
         get{ return this.titulo; }
         set{ this.titulo = value; }
      }
      private string descripcion;
      /// <summary>
      /// Descripcion del ciclo escolar
      /// </summary>
      public string Descripcion{
         get{ return this.descripcion; }
         set{ this.descripcion = value; }
      }
      private DateTime? inicioCiclo;
      /// <summary>
      /// Fecha de inicio del ciclo escolar
      /// </summary>
      public DateTime? InicioCiclo{
         get{ return this.inicioCiclo; }
         set{ this.inicioCiclo = value; }
      }
      private DateTime? finCiclo;
      /// <summary>
      /// Fecha de fin del ciclo escolar
      /// </summary>
      public DateTime? FinCiclo{
         get{ return this.finCiclo; }
         set{ this.finCiclo = value; }
      }
      private Ubicacion ubicacionID;
      /// <summary>
      /// Identificador de la ubicacion
      /// </summary>
      public Ubicacion UbicacionID{
         get{ return this.ubicacionID; }
         set{ this.ubicacionID = value; }
      }

       private bool? activo;
       /// <summary>
       /// Estatus del ciclo escolar
       /// </summary>
       public bool? Activo
       {
           get { return this.activo; }
           set { this.activo = value; }
       }

       public object Clone()
       {
          return this.MemberwiseClone();
       }
   } 
}
