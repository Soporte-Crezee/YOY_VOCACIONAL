using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Consulta registros activos de UsuarioGrupo en la BD relacionados a un usuario social
   /// </summary>
   public class UsuarioGrupoUsuarioSocialDARetHlp { 
      /// <summary>
      /// Consulta registros activos de usuarios grupo relacionados a un usuario social en sus grupos sociales en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="consultausuariosgruporelacionadosaunusuariosocialensusgrupossociales">Consulta usuarios grupo relacionados a un usuario social en sus grupos sociales que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Consulta usuarios grupo relacionados a un usuario social en sus grupos sociales generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, UsuarioSocial usuarioSocial, UsuarioSocial potentialFriend){
         object myFirm = new object();
         string sError = String.Empty;
         if (usuarioSocial == null)
            sError += ", UsuarioSocial";
         if (sError.Length > 0)
            throw new Exception("UsuarioGrupoUsuarioSocialDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (potentialFriend == null) {
         potentialFriend = new UsuarioSocial();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "UsuarioGrupoUsuarioSocialDARetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "SocialHubUsuarioGrupoDARetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "UsuarioGrupoUsuarioSocialDARetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT  ug.UsuarioGrupoID, ug.FechaAsignacion, ug.Estatus, ug.GrupoSocialID, ug.UsuarioSocialID, ug.EsModerador, null as DocenteID ");
         sCmd.Append(" FROM UsuarioGrupo ug ");
         sCmd.Append(" INNER JOIN GrupoSocial gs ON gs.GrupoSocialID = ug.GrupoSocialID ");
         sCmd.Append(" INNER JOIN SocialHubGrupoSocial shgs ON shgs.GrupoSocialID = gs.GrupoSocialID ");
         sCmd.Append(" INNER JOIN SocialHub sh ON sh.SocialHubID = shgs.SocialHubID ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (usuarioSocial.UsuarioSocialID != null){
            s_VarWHERE.Append(" sh.UsuarioSocialID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = usuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);

            s_VarWHERE.Append(" AND ug.Estatus = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = true;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (potentialFriend.UsuarioSocialID != null){
            s_VarWHERE.Append(" AND ug.UsuarioSocialID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = potentialFriend.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
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
            sqlAdapter.Fill(ds, "UsuarioGrupo");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("UsuarioGrupoUsuarioSocialDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
