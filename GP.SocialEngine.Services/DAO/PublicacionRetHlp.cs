using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Consultar Registros de Publicacion en la base de datos
   /// </summary>
   internal class PublicacionRetHlp { 
      /// <summary>
      /// Consulta registros de Publicacion en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="publicacion">Publicacion que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Publicacion generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Publicacion publicacion){
         object myFirm = new object();
         string sError = String.Empty;
         if (publicacion == null)
            sError += ", publicacion";
         if (sError.Length > 0)
            throw new Exception("PublicacionRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (publicacion.UsuarioSocial == null) {
         publicacion.UsuarioSocial = new UsuarioSocial();
      }
      if (publicacion.Ranking == null) {
         publicacion.Ranking = new Ranking();
      }
      if (publicacion.SocialHub == null) {
         publicacion.SocialHub = new SocialHub();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "PublicacionRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PublicacionRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "PublicacionRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT PublicacionID, Contenido, FechaPublicacion, Estatus, ");
         sCmd.Append(" RankingID, SocialHubID, UsuarioSocialID,AppSocialID,TipoPublicacion,JuegoID, LibroID, ContenidoDigitalID ");
         sCmd.Append(" FROM Publicacion ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (publicacion.PublicacionID != null){
            s_VarWHERE.Append(" PublicacionID = @publicacion_PublicacionID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_PublicacionID";
            sqlParam.Value = publicacion.PublicacionID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.FechaPublicacion != null){
            s_VarWHERE.Append(" AND FechaPublicacion = @publicacion_FechaPublicacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_FechaPublicacion";
            sqlParam.Value = publicacion.FechaPublicacion;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.Contenido != null){
            s_VarWHERE.Append(" AND Contenido = @publicacion_Contenido ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_Contenido";
            sqlParam.Value = publicacion.Contenido;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.Estatus != null){
            s_VarWHERE.Append(" AND Estatus = @publicacion_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_Estatus";
            sqlParam.Value = publicacion.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.UsuarioSocial.UsuarioSocialID != null){
            s_VarWHERE.Append(" AND UsuarioSocialID = @publicacion_UsuarioSocial_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_UsuarioSocial_UsuarioSocialID";
            sqlParam.Value = publicacion.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.Ranking.RankingID != null){
            s_VarWHERE.Append(" AND RankingID = @publicacion_Ranking_RankingID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_Ranking_RankingID";
            sqlParam.Value = publicacion.Ranking.RankingID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.SocialHub.SocialHubID != null){
            s_VarWHERE.Append(" AND SocialHubID = @publicacion_SocialHub_SocialHubID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_SocialHub_SocialHubID";
            sqlParam.Value = publicacion.SocialHub.SocialHubID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.TipoPublicacion != null){
            s_VarWHERE.Append(" AND TipoPublicacion = @publicacion_TipoPublicacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_TipoPublicacion";
            sqlParam.Value = publicacion.TipoPublicacion;
            sqlParam.DbType = DbType.Int16;
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
         sCmd.Append(" ORDER BY FechaPublicacion ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "Publicacion");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PublicacionRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
