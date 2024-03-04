using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.ReactivosUsuario.BO;
using POV.ReactivosUsuario.DAO;

namespace POV.ReactivosUsuario.DAO { 
   /// <summary>
   /// Consulta un registro de RespuestaUsuario en la BD
   /// </summary>
   public class RespuestaUsuarioRetHlp { 
      /// <summary>
      /// Consulta registros de RespuestaUsuario en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaUsuario">RespuestaUsuario que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de RespuestaUsuario generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, RespuestaPreguntaUsuario respuestaPreguntaUsuario, RespuestaUsuario respuestaUsuario){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaPreguntaUsuario == null)
            sError += ", RespuestaPreguntaUsuario";
         if (sError.Length > 0)
            throw new Exception("RespuestaUsuarioRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID == null)
            sError += ", RespuestaPreguntaUsuarioID";
         if (sError.Length > 0)
            throw new Exception("RespuestaUsuarioRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ReactivosUsuario.DAO", 
         "RespuestaUsuarioRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaUsuarioRetHlp: No se pudo conectar a la base de datos", "POV.ReactivosUsuario.DAO", 
         "RespuestaUsuarioRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT RespuestaPreguntaUsuarioID,FechaRegistro, TipoRespuestaUsuario, TextoRespuesta, OpcionRespuestaPlantillaID ");
         sCmd.Append(" FROM RespuestaUsuario ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID != null){
            s_VarWHERE.Append(" RespuestaPreguntaUsuarioID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaUsuario != null)
         {
             if (respuestaUsuario.FechaRegistro != null)
             {
                 s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram2 ");
                 sqlParam = sqlCmd.CreateParameter();
                 sqlParam.ParameterName = "dbp4ram2";
                 sqlParam.Value = respuestaUsuario.FechaRegistro;
                 sqlParam.DbType = DbType.DateTime;
                 sqlCmd.Parameters.Add(sqlParam);
             }
             if (respuestaUsuario.TipoRespuestaUsuario != null)
             {
                 s_VarWHERE.Append(" AND TipoRespuestaUsuario = @dbp4ram3 ");
                 sqlParam = sqlCmd.CreateParameter();
                 sqlParam.ParameterName = "dbp4ram3";
                 sqlParam.Value = respuestaUsuario.TipoRespuestaUsuario;
                 sqlParam.DbType = DbType.Int16;
                 sqlCmd.Parameters.Add(sqlParam);
             }
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
            sqlAdapter.Fill(ds, "RespuestaUsuario");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RespuestaUsuarioRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
