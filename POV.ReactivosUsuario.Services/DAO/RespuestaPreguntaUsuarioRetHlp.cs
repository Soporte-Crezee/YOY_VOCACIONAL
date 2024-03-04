using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ReactivosUsuario.BO;
using POV.ReactivosUsuario.DAO;
using POV.Reactivos.BO;
namespace POV.ReactivosUsuario.DAO { 
   /// <summary>
   /// Consulta un registro de RespuestaPreguntaUsuario en la BD
   /// </summary>
   public class RespuestaPreguntaUsuarioRetHlp { 
      /// <summary>
      /// Consulta registros de RespuestaPreguntaUsuario en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaPreguntaUsuario">RespuestaPreguntaUsuario que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de RespuestaPreguntaUsuario generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivoUsuario,RespuestaPreguntaUsuario respuestaPreguntaUsuario){
         object myFirm = new object();
         string sError = String.Empty;

         if (respuestaReactivoUsuario == null)
            sError += ", RespuestaReactivoUsuario";
         if (sError.Length > 0)
            throw new Exception("RespuestaPreguntaUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPreguntaUsuario == null)
         {
             respuestaPreguntaUsuario = new RespuestaPreguntaUsuario();
         }
          if (respuestaPreguntaUsuario.Pregunta == null)
         {
             respuestaPreguntaUsuario.Pregunta = new Pregunta();
         }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ReactivosUsuario.DAO", 
         "RespuestaPreguntaUsuarioRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaPreguntaUsuarioRetHlp: No se pudo conectar a la base de datos", "POV.ReactivosUsuario.DAO", 
         "RespuestaPreguntaUsuarioRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT RespuestaPreguntaUsuarioID,RespuestaReactivoUsuarioID, PreguntaID ");
         sCmd.Append(" FROM RespuestaPreguntaUsuario ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID != null)
         {
             s_VarWHERE.Append(" RespuestaPreguntaUsuarioID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaReactivoUsuario.RespuestaReactivoUsuarioID != null){
            s_VarWHERE.Append(" AND RespuestaReactivoUsuarioID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = respuestaReactivoUsuario.RespuestaReactivoUsuarioID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaPreguntaUsuario.Pregunta.PreguntaID != null){
            s_VarWHERE.Append(" AND PreguntaID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = respuestaPreguntaUsuario.Pregunta.PreguntaID;
            sqlParam.DbType = DbType.Int32;
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
            sqlAdapter.Fill(ds, "RespuestaPreguntaUsuario");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RespuestaPreguntaUsuarioRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
