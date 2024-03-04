using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Actualiza un registro de Comentario en la BD
   /// </summary>
   public class ComentarioUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de ComentarioUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="comentarioUpdHlp">ComentarioUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">ComentarioUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Comentario comentario, Comentario anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (comentario == null)
            sError += ", Comentario";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("ComentarioUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (comentario.Estatus == null)
            sError += ", Estatus";
      if (comentario.Ranking == null) {
         comentario.Ranking = new Ranking();
      }
      if (comentario.UsuarioSocial == null) {
         comentario.UsuarioSocial = new UsuarioSocial();
      }
         if (anterior.ComentarioID == null)
            sError += ", Anterior ComentarioID";
         if (anterior.Cuerpo == null)
            sError += ", Anterio Cuerpo";
         if (anterior.FechaComentario == null)
            sError += ", Anterior FechaComentario";
         if (anterior.Estatus == null)
            sError += ", Anterior Estatus";
      if (anterior.Ranking == null) {
         anterior.Ranking = new Ranking();
      }
      if (anterior.Ranking.RankingID == null)
          sError += ", Anterio RankingID";
      if (anterior.UsuarioSocial == null) {
         anterior.UsuarioSocial = new UsuarioSocial();
      }
         if (anterior.UsuarioSocial.UsuarioSocialID == null)
            sError += ", Anterior UsuarioSocialID";
         if (sError.Length > 0)
            throw new Exception("ComentarioUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "ComentarioUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ComentarioUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "ComentarioUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Comentario SET ");
         sCmd.Append(" estatus = @comentario_Estatus ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "comentario_Estatus";
         if (comentario.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = comentario.Estatus;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         if (anterior.ComentarioID == null)
            sCmd.Append(" WHERE comentarioID IS NULL ");
         else{ 
            sCmd.Append(" WHERE comentarioID = @anterior_ComentarioID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_ComentarioID";
            sqlParam.Value = anterior.ComentarioID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Cuerpo == null)
            sCmd.Append(" AND Cuerpo IS NULL ");
         else{ 
            sCmd.Append(" AND Cuerpo = @anterior_Cuerpo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_Cuerpo";
            sqlParam.Value = anterior.Cuerpo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.FechaComentario == null)
            sCmd.Append(" AND FechaComentario IS NULL ");
         else{ 
            sCmd.Append(" AND FechaComentario = @anterior_FechaComentario ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_FechaComentario";
            sqlParam.Value = anterior.FechaComentario;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Estatus == null)
            sCmd.Append(" AND Estatus IS NULL ");
         else{ 
            sCmd.Append(" AND Estatus = @anterior_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_Estatus";
            sqlParam.Value = anterior.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.UsuarioSocial.UsuarioSocialID == null)
            sCmd.Append(" AND UsuarioSocialID IS NULL ");
         else{ 
            sCmd.Append(" AND UsuarioSocialID = @anterior_UsuarioSocial_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_UsuarioSocial_UsuarioSocialID";
            sqlParam.Value = anterior.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Ranking.RankingID == null)
            sCmd.Append(" AND RankingID IS NULL ");
         else{ 
            sCmd.Append(" AND RankingID = @anterior_Ranking_RankingID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_Ranking_RankingID";
            sqlParam.Value = anterior.Ranking.RankingID;
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
            throw new Exception("ComentarioUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ComentarioUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
