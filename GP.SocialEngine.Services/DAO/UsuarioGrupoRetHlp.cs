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
   /// Consulta un registro de UsuarioGrupo en la BD
   /// </summary>
   public class UsuarioGrupoRetHlp { 
      /// <summary>
      /// Consulta registros de UsuarioGrupo en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioGrupo">UsuarioGrupo que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de UsuarioGrupo generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, UsuarioGrupo usuarioGrupo, GrupoSocial grupoSocial){
         object myFirm = new object();
      if (usuarioGrupo == null) {
         usuarioGrupo = new UsuarioGrupo();
      }
      if (usuarioGrupo.UsuarioSocial == null) {
         usuarioGrupo.UsuarioSocial = new UsuarioSocial();
      }
      if (grupoSocial == null) {
         grupoSocial = new GrupoSocial();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "UsuarioGrupoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UsuarioGrupoRethlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "UsuarioGrupoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT UsuarioGrupoID, FechaAsignacion, Estatus, GrupoSocialID, UsuarioSocialID,EsModerador, null as DocenteID ");
         sCmd.Append(" FROM UsuarioGrupo ");
         sCmd.Append(" WHERE ");
         StringBuilder s_Var = new StringBuilder();
         if (usuarioGrupo.UsuarioGrupoID != null){
            s_Var.Append(" UsuarioGrupoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = usuarioGrupo.UsuarioGrupoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioGrupo.UsuarioSocial.UsuarioSocialID != null){
            s_Var.Append(" AND UsuarioSocialID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = usuarioGrupo.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (grupoSocial.GrupoSocialID != null){
            s_Var.Append(" AND GrupoSocialID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = grupoSocial.GrupoSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioGrupo.FechaAsignacion != null){
            s_Var.Append(" AND FechaAsignacion = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = usuarioGrupo.FechaAsignacion;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioGrupo.Estatus != null){
            s_Var.Append(" AND Estatus = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = usuarioGrupo.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioGrupo.EsModerador != null){
            s_Var.Append(" AND EsModerador = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = usuarioGrupo.EsModerador;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         s_Var.Append("  ");
         string s_Varres = s_Var.ToString().Trim();
         if (s_Varres.Length > 0){
            if (s_Varres.StartsWith("AND "))
               s_Varres = s_Varres.Substring(4);
            else if (s_Varres.StartsWith("OR "))
               s_Varres = s_Varres.Substring(3);
            else if (s_Varres.StartsWith(","))
               s_Varres = s_Varres.Substring(1);
            sCmd.Append("  " + s_Varres);
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
            throw new Exception("UsuarioGrupoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
