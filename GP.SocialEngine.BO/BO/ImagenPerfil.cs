using System;
using POV.Comun.BO;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// ImagenPerfill
   /// </summary>
   public class ImagenPerfil { 
      private AdjuntoImagen adjuntoImagen;
      /// <summary>
      /// AdjuntoImagen
      /// </summary>
      public AdjuntoImagen AdjuntoImagen{
         get{ return this.adjuntoImagen; }
         set{ this.adjuntoImagen = value; }
      }
      private bool? estatus;
      /// <summary>
      /// Estatus
      /// </summary>
      public bool? Estatus{
         get{ return this.estatus; }
         set{ this.estatus = value; }
      }
   } 
}
