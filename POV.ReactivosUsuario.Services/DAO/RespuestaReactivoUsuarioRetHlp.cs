using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using GP.SocialEngine.BO;
using POV.ReactivosUsuario.BO;
using POV.ReactivosUsuario.DAO;

namespace POV.ReactivosUsuario.DAO { 
   /// <summary>
   /// Consulta un registro de RespuestaReactivoUsuario en la BD
   /// </summary>
   public class RespuestaReactivoUsuarioRetHlp { 
      /// <summary>
      /// Consulta registros de RespuestaReactivoUsuario en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaReactivoUsuario">RespuestaReactivoUsuario que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de RespuestaReactivoUsuario generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivoUsuario){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaReactivoUsuario == null)
            sError += ", RespuestaReactivoUsuario";
         if (sError.Length > 0)
            throw new Exception("RespuestaReactivoUsuarioRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (respuestaReactivoUsuario.Reactivo == null) {
         respuestaReactivoUsuario.Reactivo = new Reactivo();
      }
      if (respuestaReactivoUsuario.UsuarioSocial == null) {
         respuestaReactivoUsuario.UsuarioSocial = new UsuarioSocial();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ReactivosUsuario.DAO", 
         "RespuestaReactivoUsuarioRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaReactivoUsuarioRetHlp: No se pudo conectar a la base de datos", "POV.ReactivosUsuario.DAO", 
         "RespuestaReactivoUsuarioRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT RespuestaReactivoUsuarioID, ReactivoID, UsuarioSocialID, PrimeraCalificacion, UltimaCalificacion, FechaRegistro,UltimaActualizacion, NumeroIntentos,EstadoReactivoUsuario ");
         sCmd.Append(" FROM RespuestaReactivoUsuario ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (respuestaReactivoUsuario.RespuestaReactivoUsuarioID != null){
            s_VarWHERE.Append(" RespuestaReactivoUsuarioID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = respuestaReactivoUsuario.RespuestaReactivoUsuarioID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaReactivoUsuario.Reactivo.ReactivoID != null){
            s_VarWHERE.Append(" AND ReactivoID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = respuestaReactivoUsuario.Reactivo.ReactivoID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaReactivoUsuario.UsuarioSocial.UsuarioSocialID != null){
            s_VarWHERE.Append(" AND UsuarioSocialID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = respuestaReactivoUsuario.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaReactivoUsuario.FechaRegistro != null){
            s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = respuestaReactivoUsuario.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaReactivoUsuario.UltimaActualizacion != null){
            s_VarWHERE.Append(" AND UltimaActualizacion = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = respuestaReactivoUsuario.UltimaActualizacion;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
          if (respuestaReactivoUsuario.EstadoReactivoUsuario != null){
            s_VarWHERE.Append(" AND EstadoReactivoUsuario = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = respuestaReactivoUsuario.EstadoReactivoUsuario;
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
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "RespuestaReactivoUsuario");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RespuestaReactivoUsuarioRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
