using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Consulta un registro de Comentario en la BD.
   /// </summary>
   public class ComentarioRetHlp { 
      /// <summary>
      /// Consulta registros de Comentario en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="comentario">Comentario que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Comentario generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Comentario comentario, Publicacion publicacion){
         object myFirm = new object();
         string sError = String.Empty;
         if (comentario == null)
            sError += ", Comentario";
         if (sError.Length > 0)
            throw new Exception("ComentarioRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (comentario.UsuarioSocial == null) {
         comentario.UsuarioSocial = new UsuarioSocial();
      }
      if (comentario.Ranking == null) {
         comentario.Ranking = new Ranking();
      }
      if (publicacion == null) {
         publicacion = new Publicacion();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "ComentarioRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ComentarioRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "ComentarioRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT ");
         sCmd.Append(" c.ComentarioID, c.Cuerpo, c.FechaComentario, c.Estatus, ");
         sCmd.Append(" c.UsuarioSocialID, c.RankingID, c.PublicacionID,c.TipoPublicacion,c.ReactivoID,c.JuegoID,c.ContenidoDigitalID ");
         sCmd.Append(" FROM Comentario c ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (comentario.ComentarioID != null){
            s_VarWHERE.Append(" c.ComentarioID = @comentario_ComentarioID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_ComentarioID";
            sqlParam.Value = comentario.ComentarioID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (comentario.Cuerpo != null){
            s_VarWHERE.Append(" AND c.Cuerpo = @comentario_Cuerpo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_Cuerpo";
            sqlParam.Value = comentario.Cuerpo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (comentario.FechaComentario != null){
            s_VarWHERE.Append(" AND c.FechaComentario = @comentario_FechaComentario ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_FechaComentario";
            sqlParam.Value = comentario.FechaComentario;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (comentario.Estatus != null){
            s_VarWHERE.Append(" AND c.Estatus = @comentario_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_Estatus";
            sqlParam.Value = comentario.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (comentario.UsuarioSocial.UsuarioSocialID != null){
            s_VarWHERE.Append(" AND c.UsuarioSocialID = @comentario_UsuarioSocial_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_UsuarioSocial_UsuarioSocialID";
            sqlParam.Value = comentario.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (comentario.Ranking.RankingID != null){
            s_VarWHERE.Append(" AND c.RankingID = @comentario_Ranking_RankingID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_Ranking_RankingID";
            sqlParam.Value = comentario.Ranking.RankingID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.PublicacionID != null){
            s_VarWHERE.Append(" AND c.PublicacionID = @publicacion_PublicacionID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_PublicacionID";
            sqlParam.Value = publicacion.PublicacionID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (comentario.TipoPublicacion != null)
         {
             s_VarWHERE.Append(" AND c.TipoPublicacion = @comentario_TipoPublicacion ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "comentario_TipoPublicacion";
             sqlParam.Value = comentario.TipoPublicacion;
             sqlParam.DbType = DbType.Byte;
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
         sCmd.Append(" ORDER BY c.FechaComentario ASC ");


         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "comentario");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ComentarioRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
