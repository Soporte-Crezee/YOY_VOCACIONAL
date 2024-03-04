using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;

namespace GP.SocialEngine.Service { 
   /// <summary>
   /// Controlador del objeto Ranking
   /// </summary>
   public class RankingCtrl { 
      /// <summary>
      /// Consulta registros de RankingRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="rankingRetHlp">RankingRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de RankingRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Ranking ranking){
         RankingRetHlp da = new RankingRetHlp();
         DataSet ds = da.Action(dctx, ranking);
         return ds;
      }

      public Ranking RetrieveComplete(IDataContext dctx, Ranking ranking)
      {
          ranking = LastDataRowToRanking(this.Retrieve(dctx, ranking));
          ranking.ListaPuntuaciones = RetrieveUsuariosSocialRanking(dctx, ranking, null);
          
          return ranking;
      }

      public List<UsuarioSocialRanking> RetrieveUsuariosSocialRanking(IDataContext dctx, Ranking ranking, EPuntuacionRanking? puntuacion)
      {
          List<UsuarioSocialRanking> usuariosRanking = new List<UsuarioSocialRanking>();

          UsuarioSocialRankingCtrl usuarioSocialRankingCtrl = new UsuarioSocialRankingCtrl();
          UsuarioSocialRanking usuarioRanking = new UsuarioSocialRanking();
          if (puntuacion != null) usuarioRanking.Puntuacion = (int)puntuacion;

          DataSet ds = usuarioSocialRankingCtrl.Retrieve(dctx, usuarioRanking, ranking);
          foreach (DataRow dr in ds.Tables[0].Rows)
          {
              usuarioRanking.UsuarioSocial = new UsuarioSocial();
              usuarioRanking.UsuarioSocial.UsuarioSocialID = Convert.ToInt64(dr["UsuarioSocialID"]);
              usuariosRanking.Add(usuarioSocialRankingCtrl.RetrieveComplete(dctx, usuarioRanking, ranking));
          }

          return usuariosRanking;
      }
      /// <summary>
      /// Crea un registro de RankingInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="rankingInsHlp">RankingInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, Ranking  ranking){
         RankingInsHlp da = new RankingInsHlp();
         da.Action(dctx,  ranking);
      }

      public void DeleteComplete(IDataContext dctx, Ranking ranking)
      {
          UsuarioSocialRankingCtrl usuarioSocialRankingCtrl = new UsuarioSocialRankingCtrl();
          ranking = RetrieveComplete(dctx, ranking);

          foreach(UsuarioSocialRanking usuarioRanking in ranking.ListaPuntuaciones)
          {
              usuarioSocialRankingCtrl.Delete(dctx, usuarioRanking, ranking);
          }

          RankingDelHlp da = new RankingDelHlp();

          da.Action(dctx, ranking);

      }
      /// <summary>
      /// Crea un objeto de Ranking a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Ranking</param>
      /// <returns>Un objeto de Ranking creado a partir de los datos</returns>
      public Ranking LastDataRowToRanking(DataSet ds) {
         if (!ds.Tables.Contains("Ranking"))
            throw new Exception("LastDataRowToRanking: DataSet no tiene la tabla Ranking");
         int index = ds.Tables["Ranking"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToRanking: El DataSet no tiene filas");
         return this.DataRowToRanking(ds.Tables["Ranking"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Ranking a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Ranking</param>
      /// <returns>Un objeto de Ranking creado a partir de los datos</returns>
      public Ranking DataRowToRanking(DataRow row){
         Ranking ranking = new Ranking();
         ranking.ListaPuntuaciones = new List<UsuarioSocialRanking>();
         if (row.IsNull("RankingID"))
            ranking.RankingID = null;
         else
            ranking.RankingID = (Guid)Convert.ChangeType(row["RankingID"], typeof(Guid));
         return ranking;
      }
   } 
}
