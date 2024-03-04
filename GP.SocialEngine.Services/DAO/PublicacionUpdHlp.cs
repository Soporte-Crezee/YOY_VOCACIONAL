using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Actualiza un registro de Publicacion en la BD
   /// </summary>
   public class PublicacionUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de PublicacionUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="publicacionUpdHlp">PublicacionUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">PublicacionUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Publicacion publicacion, Publicacion anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (publicacion == null)
            sError += ", Publicacion";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("PublicacionUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.PublicacionID == null)
            sError += ", Anterior PublicacionID";
         if (sError.Length > 0)
            throw new Exception("PublicacionUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "PublicacionUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PublicacionUpdHlp: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "PublicacionUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Publicacion ");
         if (publicacion.Contenido != null){
            sCmd.Append(" SET Contenido = @publicacion_Contenido ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_Contenido";
            sqlParam.Value = publicacion.Contenido;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.FechaPublicacion != null){
            sCmd.Append(" ,FechaPublicacion = @publicacion_FechaPublicacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_FechaPublicacion";
            sqlParam.Value = publicacion.FechaPublicacion;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.Estatus == null)
            sCmd.Append(" ,Estatus = NULL ");
         else{ 
            sCmd.Append(" ,Estatus = @publicacion_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_Estatus";
            sqlParam.Value = publicacion.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.SocialHub.SocialHubID != null){
            sCmd.Append(" ,SocialHubID = @publicacion_SocialHub_SocialHubID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_SocialHub_SocialHubID";
            sqlParam.Value = publicacion.SocialHub.SocialHubID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.Ranking.RankingID != null){
            sCmd.Append(" ,RankingID = @publicacion_Ranking_RankingID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_Ranking_RankingID";
            sqlParam.Value = publicacion.Ranking.RankingID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.UsuarioSocial.UsuarioSocialID != null){
            sCmd.Append(" ,UsuarioSocialID = @publicacion_UsuarioSocial_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_UsuarioSocial_UsuarioSocialID";
            sqlParam.Value = publicacion.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.PublicacionID == null)
            sCmd.Append(" WHERE PublicacionID IS NULL ");
         else{ 
            sCmd.Append(" WHERE PublicacionID = @anterior_PublicacionID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_PublicacionID";
            sqlParam.Value = anterior.PublicacionID;
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
            throw new Exception("PublicacionUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PublicacionUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
