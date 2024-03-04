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
   /// Elimina un registro de Ranking en la BD
   /// </summary>
   public class RankingDelHlp { 
      /// <summary>
      /// Elimina un registro de RankingDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="rankingDelHlp">RankingDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, Ranking ranking){
         object myFirm = new object();
         string sError = String.Empty;
         if (ranking == null)
            sError += ", Ranking";
         if (sError.Length > 0)
            throw new Exception("RankingDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (ranking.RankingID == null)
            sError += ", RankingID";
         if (sError.Length > 0)
            throw new Exception("RankingDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "RankingDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RankingDelHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "RankingDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM Ranking ");
         if (ranking.RankingID == null)
            sCmd.Append(" WHERE RankingID IS NULL ");
         else{ 
            sCmd.Append(" WHERE RankingID = @ranking_RankingID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "ranking_RankingID";
            sqlParam.Value = ranking.RankingID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RankingDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RankingDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
