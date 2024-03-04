using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using POV.CentroEducativo.BO;

namespace POV.CentroEducativo.BO { 
   /// <summary>
   /// Clase Nivel Educativo
   /// </summary>
   public class NivelEducativo { 
      private int? nivelEducativoID;
      /// <summary>
      /// Identificador del nivel educativo
      /// </summary>
      public int? NivelEducativoID{
         get{ return this.nivelEducativoID; }
         set{ this.nivelEducativoID = value; }
      }
      private string titulo;
      /// <summary>
      /// titulo del nivel educativo
      /// </summary>
      public string Titulo{
         get{ return this.titulo; }
         set{ this.titulo = value; }
      }
      private string descripcion;
      /// <summary>
      /// Descripcion del nivel educativo
      /// </summary>
      public string Descripcion{
         get{ return this.descripcion; }
         set{ this.descripcion = value; }
      }
      private byte? numeroGrados;
      /// <summary>
      /// Numero de grados del nivel educativo
      /// </summary>
      public byte? NumeroGrados{
         get{ return this.numeroGrados; }
         set{ this.numeroGrados = value; }
      }
      private TipoNivelEducativo tipoNivelEducativoID;
      /// <summary>
      /// Identificador del tipo de nivel educativo
      /// </summary>
      public TipoNivelEducativo TipoNivelEducativoID{
         get{ return this.tipoNivelEducativoID; }
         set{ this.tipoNivelEducativoID = value; }
      }
   } 
}
