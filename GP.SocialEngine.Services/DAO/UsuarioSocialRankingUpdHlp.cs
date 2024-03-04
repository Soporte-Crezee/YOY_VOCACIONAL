using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Actualiza un registro de UsuarioSocialRanking en la BD
   /// </summary>
   public class UsuarioSocialRankingUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de UsuarioSocialRankingUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocialRankingUpdHlp">UsuarioSocialRankingUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">UsuarioSocialRankingUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, UsuarioSocialRanking usuarioSocialRanking, Ranking ranking, UsuarioSocialRanking anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (usuarioSocialRanking == null)
            sError += ", UsuarioSocialRanking";
         if (anterior == null)
            sError += ", Anterior";
         if (ranking == null)
            sError += ", ranking";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (ranking.RankingID == null)
            sError += ", RankingID";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.UsuarioSocial == null)
            sError += ", UsuarioSocial";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.UsuarioSocial.UsuarioSocialID == null)
            sError += ", UsuarioSocialID";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "UsuarioSocialRankingUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UsuarioSocialRankingUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "UsuarioSocialRankingUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE UsuarioSocialRanking ");
         if (usuarioSocialRanking.Puntuacion == null)
            sCmd.Append(" SET Puntuacion = NULL ");
         else{ 
            sCmd.Append(" SET Puntuacion = @usuarioSocialRanking_Puntuacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioSocialRanking_Puntuacion";
            sqlParam.Value = usuarioSocialRanking.Puntuacion;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ranking.RankingID == null)
            sCmd.Append(" ,WHERE RankingID = NULL ");
         else{ 
            sCmd.Append(" ,WHERE RankingID = @ranking_RankingID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "ranking_RankingID";
            sqlParam.Value = ranking.RankingID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.UsuarioSocial.UsuarioSocialID == null)
            sCmd.Append(" AND UsuarioSocialID = NULL ");
         else{ 
            sCmd.Append(" AND UsuarioSocialID = @anterior_UsuarioSocial_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_UsuarioSocial_UsuarioSocialID";
            sqlParam.Value = anterior.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("UsuarioSocialRankingUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("UsuarioSocialRankingUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
