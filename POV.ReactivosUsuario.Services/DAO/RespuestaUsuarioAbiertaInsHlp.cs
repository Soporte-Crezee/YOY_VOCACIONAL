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
   /// Guarda un registro de RespuestaUsuarioAbierta en la BD
   /// </summary>
   public class RespuestaUsuarioAbiertaInsHlp { 
      /// <summary>
      /// Crea un registro de RespuestaUsuarioAbierta en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaUsuarioAbierta">RespuestaUsuarioAbierta que desea crear</param>
      public void Action(IDataContext dctx, RespuestaPreguntaUsuario respuestaPreguntaUsuario, RespuestaUsuarioAbierta respuestaUsuarioAbierta){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaPreguntaUsuario == null)
            sError += ", RespuestaPreguntaUsuario";
         if (sError.Length > 0)
            throw new Exception("RespuestaUsuarioAbiertaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID == null)
            sError += ", RespuestaPreguntaUsuarioID";
         if (sError.Length > 0)
            throw new Exception("RespuestaUsuarioAbiertaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaUsuarioAbierta == null)
            sError += ", RespuestaUsuarioAbierta";
         if (sError.Length > 0)
            throw new Exception("RespuestaUsuarioAbiertaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaUsuarioAbierta.TipoRespuestaUsuario == null)
            sError += ", TipoRespuestaUsuario";
         if (sError.Length > 0)
            throw new Exception("RespuestaUsuarioAbiertaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ReactivosUsuario.DAO", 
         "RespuestaUsuarioAbiertaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaUsuarioAbiertaInsHlp: No se pudo conectar a la base de datos", "POV.ReactivosUsuario.DAO", 
         "RespuestaUsuarioAbiertaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO RespuestaUsuario (RespuestaPreguntaUsuarioID,FechaRegistro, TipoRespuestaUsuario, TextoRespuesta) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaUsuarioAbierta.FechaRegistro
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (respuestaUsuarioAbierta.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaUsuarioAbierta.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaUsuarioAbierta.TipoRespuestaUsuario
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (respuestaUsuarioAbierta.TipoRespuestaUsuario == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaUsuarioAbierta.TipoRespuestaUsuario;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaUsuarioAbierta.TextoRespuesta
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (respuestaUsuarioAbierta.TextoRespuesta == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaUsuarioAbierta.TextoRespuesta;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RespuestaUsuarioAbiertaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaUsuarioAbiertaInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
