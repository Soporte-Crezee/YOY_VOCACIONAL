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
   /// Guarda un registro de RespuestaPreguntaUsuario en la BD
   /// </summary>
   public class RespuestaPreguntaUsuarioInsHlp { 
      /// <summary>
      /// Crea un registro de RespuestaPreguntaUsuario en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaPreguntaUsuario">RespuestaPreguntaUsuario que desea crear</param>
      public void Action(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivoUsuario, RespuestaPreguntaUsuario respuestaPreguntaUsuario){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaPreguntaUsuario == null)
            sError += ", RespuestaPreguntaUsuario";
         if (sError.Length > 0)
            throw new Exception("RespuestaPreguntaUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID == null)
            sError += ", RespuestaPreguntaUsuarioID";
         if (sError.Length > 0)
            throw new Exception("RespuestaPreguntaUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaReactivoUsuario == null)
            sError += ", RespuestaReactivoUsuario";
         if (sError.Length > 0)
            throw new Exception("RespuestaPreguntaUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaReactivoUsuario.RespuestaReactivoUsuarioID == null)
            sError += ", RespuestaReactivoUsuarioID";
         if (sError.Length > 0)
            throw new Exception("RespuestaPreguntaUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPreguntaUsuario.Pregunta == null)
            sError += ", Pregunta";
         if (sError.Length > 0)
            throw new Exception("RespuestaPreguntaUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPreguntaUsuario.Pregunta.PreguntaID == null)
            sError += ", PreguntaID";
         if (sError.Length > 0)
            throw new Exception("RespuestaPreguntaUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ReactivosUsuario.DAO", 
         "RespuestaPreguntaUsuarioInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaPreguntaUsuarioInsHlp: No se pudo conectar a la base de datos", "POV.ReactivosUsuario.DAO", 
         "RespuestaPreguntaUsuarioInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO RespuestaPreguntaUsuario (RespuestaPreguntaUsuarioID,RespuestaReactivoUsuarioID, PreguntaID) ");
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
         // respuestaReactivoUsuario.RespuestaReactivoUsuarioID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (respuestaReactivoUsuario.RespuestaReactivoUsuarioID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaReactivoUsuario.RespuestaReactivoUsuarioID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPreguntaUsuario.Pregunta.PreguntaID
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (respuestaPreguntaUsuario.Pregunta.PreguntaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPreguntaUsuario.Pregunta.PreguntaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RespuestaPreguntaUsuarioInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaPreguntaUsuarioInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
