using System;
using System.Text;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// UsuarioSocialRanking
   /// </summary>
   public class UsuarioSocialRanking: INotificable {

       private Guid? rankingID;

       public Guid? RankingID
       {
           get { return this.rankingID; }
           set { this.rankingID = value; }
       }

      private UsuarioSocial usuarioSocial;
      /// <summary>
      /// UsuarioSocial del Ranking 
      /// </summary>
      public UsuarioSocial UsuarioSocial{
         get{ return this.usuarioSocial; }
         set{ this.usuarioSocial = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// FechaRegistro del Ranking 
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      private int? puntuacion;
      /// <summary>
      /// Puntuacion del Ranking 
      /// </summary>
      public int? Puntuacion{
         get{ return this.puntuacion; }
         set{ this.puntuacion = value; }
      }

      public void AsignarPuntuacion(int puntuacion)
      {
          if ((int)EPuntuacionRanking.BIEN == puntuacion || (int)EPuntuacionRanking.EXCELENTE == puntuacion || (int)EPuntuacionRanking.REGULAR == puntuacion)
          {
              Puntuacion = puntuacion;
          }
          else
          {
              Puntuacion = (int)EPuntuacionRanking.EXCELENTE;
          }
      }

      public EPuntuacionRanking? TipoPuntuacion()
      {
          if (Puntuacion != null)
              return (EPuntuacionRanking)Puntuacion.Value;
          else
              return null;
      }
      public Guid? GUID
      {
          get
          {
              return this.rankingID;
          }
          set
          {
             this.rankingID = value;
          }
      }

      public string TextoNotificacion
      {
          get
          {
              string textoPuntuacion =  TipoPuntuacion().ToString().ToLower();
              return string.Format("{0} {1} tu ", GP.SocialEngine.Properties.Resources.UsuarioSocialRankingNotificacion,textoPuntuacion) ;
          }
          set
          {
              throw new NotImplementedException();
          }
      }

      public string URLReferencia
      {
          get
          {
              return "VerPublicacion.aspx";
          }
          set
          {
              throw new NotImplementedException();
          }
      }
   } 
}
