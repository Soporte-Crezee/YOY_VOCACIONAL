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
   /// Elimina un registro de UsuarioSocialRanking en la BD
   /// </summary>
   public class UsuarioSocialRankingDelHlp { 
      /// <summary>
      /// Elimina un registro de UsuarioSocialRankingDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocialRankingDelHlp">UsuarioSocialRankingDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, UsuarioSocialRanking usuarioSocialRanking, Ranking ranking){
         object myFirm = new object();
         String sError = string.Empty;
         if (usuarioSocialRanking == null)
            sError += ", UsuarioSocialRanking";
         if (ranking == null)
            sError += ", ranking";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (ranking.RankingID == null)
            sError += ", RankingID";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (usuarioSocialRanking.UsuarioSocial == null)
            sError += ", UsuarioSocial";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (usuarioSocialRanking.UsuarioSocial.UsuarioSocialID == null)
            sError += ", UsuarioSocialID";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "UsuarioSocialRankingDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UsuarioSocialRankingDelHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "UsuarioSocialRankingDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM UsuarioSocialRanking ");
         if (ranking.RankingID == null)
            sCmd.Append(" WHERE  RankingID IS NULL ");
         else{ 
            sCmd.Append(" WHERE  RankingID = @ranking_RankingID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "ranking_RankingID";
            sqlParam.Value = ranking.RankingID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioSocialRanking.UsuarioSocial.UsuarioSocialID == null)
            sCmd.Append(" AND UsuarioSocialID IS NULL ");
         else{ 
            sCmd.Append(" AND UsuarioSocialID = @usuarioSocialRanking_UsuarioSocial_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioSocialRanking_UsuarioSocial_UsuarioSocialID";
            sqlParam.Value = usuarioSocialRanking.UsuarioSocial.UsuarioSocialID;
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
            throw new Exception("UsuarioSocialRankingDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("UsuarioSocialRankingDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
