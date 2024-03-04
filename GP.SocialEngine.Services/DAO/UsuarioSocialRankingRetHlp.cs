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
   /// Consulta un registro de UsuarioSocialRanking en la BD
   /// </summary>
   public class UsuarioSocialRankingRetHlp { 
      /// <summary>
      /// Consulta registros de UsuarioSocialRanking en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocialRanking">UsuarioSocialRanking que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de UsuarioSocialRanking generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, UsuarioSocialRanking usuarioSocialRanking, Ranking ranking){
         object myFirm = new object();

         if (usuarioSocialRanking.UsuarioSocial == null)
             usuarioSocialRanking.UsuarioSocial = new UsuarioSocial();
         string sError = String.Empty;
         if (usuarioSocialRanking == null)
            sError += ", UsuarioSocialRanking";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRankingRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "UsuarioSocialRankingRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UsuarioSocialRankingRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "UsuarioSocialRankingRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT RankingID, UsuarioSocialID, Puntuacion, FechaRegistro ");
         sCmd.Append(" FROM UsuarioSocialRanking ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (ranking.RankingID != null){
            s_VarWHERE.Append(" RankingID = @ranking_RankingID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "ranking_RankingID";
            sqlParam.Value = ranking.RankingID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioSocialRanking.UsuarioSocial.UsuarioSocialID != null){
            s_VarWHERE.Append(" AND UsuarioSocialID = @usuarioSocialRanking_UsuarioSocial_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioSocialRanking_UsuarioSocial_UsuarioSocialID";
            sqlParam.Value = usuarioSocialRanking.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioSocialRanking.Puntuacion != null){
            s_VarWHERE.Append(" AND Puntuacion = @usuarioSocialRanking_Puntuacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioSocialRanking_Puntuacion";
            sqlParam.Value = usuarioSocialRanking.Puntuacion;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioSocialRanking.FechaRegistro != null){
            s_VarWHERE.Append(" AND FechaRegistro = @usuarioSocialRanking_FechaRegistro ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioSocialRanking_FechaRegistro";
            sqlParam.Value = usuarioSocialRanking.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            else if (s_VarWHEREres.StartsWith(","))
               s_VarWHEREres = s_VarWHEREres.Substring(1);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "UsuarioSocialRanking");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("UsuarioSocialRankingRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
