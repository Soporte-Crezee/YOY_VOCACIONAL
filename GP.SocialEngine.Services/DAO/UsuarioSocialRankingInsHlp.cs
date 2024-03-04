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
   /// Guarda un registro de UsuarioSocialRanking en la BD
   /// </summary>
   public class UsuarioSocialRankingInsHlp { 
      /// <summary>
      /// Crea un registro de UsuarioSocialRanking en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocialRanking">UsuarioSocialRanking que desea crear</param>
      public void Action(IDataContext dctx, UsuarioSocialRanking usuarioSocialRanking, Ranking ranking){
         object myFirm = new object();
         string sError = String.Empty;
         if (usuarioSocialRanking == null)
            sError += ", UsuarioSocialRanking";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (ranking == null)
            sError += ", ranking";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (ranking.RankingID == null)
            sError += ", RankingID";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (usuarioSocialRanking.UsuarioSocial == null)
            sError += ", UsuarioSocial";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (usuarioSocialRanking.UsuarioSocial.UsuarioSocialID == null)
            sError += ", UsuarioSocialID";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (usuarioSocialRanking.Puntuacion == null)
            sError += ", Puntuacion";
         if (usuarioSocialRanking.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "UsuarioSocialRankingInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UsuarioSocialRankingInsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "UsuarioSocialRankingInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO UsuarioSocialRanking (RankingID, UsuarioSocialID, Puntuacion, FechaRegistro) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         sCmd.Append(" @ranking_RankingID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "ranking_RankingID";
         if (ranking.RankingID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ranking.RankingID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@usuarioSocialRanking_UsuarioSocial_UsuarioSocialID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioSocialRanking_UsuarioSocial_UsuarioSocialID";
         if (usuarioSocialRanking.UsuarioSocial.UsuarioSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioSocialRanking.UsuarioSocial.UsuarioSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@usuarioSocialRanking_Puntuacion ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioSocialRanking_Puntuacion";
         if (usuarioSocialRanking.Puntuacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioSocialRanking.Puntuacion;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@usuarioSocialRanking_FechaRegistro ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioSocialRanking_FechaRegistro";
         if (usuarioSocialRanking.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioSocialRanking.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("UsuarioSocialRankingInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("UsuarioSocialRankingInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
