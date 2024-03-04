using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GP.SocialEngine.BO { 
   /// <summary>
   /// Ranking
   /// </summary>
   public class Ranking { 
      private Guid? rankingID;
      /// <summary>
      /// Identificador del Ranking
      /// </summary>
      public Guid? RankingID{
         get{ return this.rankingID; }
         set{ this.rankingID = value; }
      }
      private List<UsuarioSocialRanking> listaPuntuaciones;
      /// <summary>
      /// Lista de las puntuaciones de los usuarios.
      /// </summary>
      public List<UsuarioSocialRanking> ListaPuntuaciones{
         get{ return this.listaPuntuaciones; }
         set{ this.listaPuntuaciones = value; }
      }

      public int ObtenerPuntuacion(EPuntuacionRanking tipoPuntuacion)
      {
          int puntuacion = 0;
          if (this.listaPuntuaciones != null)
          {
              puntuacion = ListaPuntuaciones.Count(item => (EPuntuacionRanking)item.TipoPuntuacion() == tipoPuntuacion);
          }
          
          return puntuacion;
      }
   } 
}
