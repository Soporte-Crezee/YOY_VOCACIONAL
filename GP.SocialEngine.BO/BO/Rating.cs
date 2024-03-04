using System;
using System.Text;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// Rating
   /// </summary>
   public class Rating { 
      private long? ratingID;
      /// <summary>
      /// Identificador autonumérico del Rating
      /// </summary>
      public long? RatingID{
         get{ return this.ratingID; }
         set{ this.ratingID = value; }
      }
      private int? puntuacion;
      /// <summary>
      /// Puntuacón
      /// </summary>
      public int? Puntuacion{
         get{ return this.puntuacion; }
         set{ this.puntuacion = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// Fecha de Registro
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      private UsuarioSocial usuarioSocial;
      /// <summary>
      /// UsuarioSocial
      /// </summary>
      public UsuarioSocial UsuarioSocial{
         get{ return this.usuarioSocial; }
         set{ this.usuarioSocial = value; }
      }
   } 
}
