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
   /// Guarda un registro de RespuestaUsuarioOpcionMultiple en la BD
   /// </summary>
   public class RespuestaUsuarioOpcionMultipleInsHlp { 
      /// <summary>
      /// Crea un registro de RespuestaUsuarioOpcionMultiple en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaUsuarioOpcionMultiple">RespuestaUsuarioOpcionMultiple que desea crear</param>
      public void Action(IDataContext dctx, RespuestaPreguntaUsuario respuestaPreguntaUsuario, RespuestaUsuarioOpcionMultiple respuestaUsuarioOpcionMultiple){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaPreguntaUsuario == null)
            sError += ", RespuestaPreguntaUsuario";
         if (sError.Length > 0)
            throw new Exception("RespuestaUsuarioOpcionMultipleInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID == null)
            sError += ", RespuestaPreguntaUsuarioID";
         if (sError.Length > 0)
            throw new Exception("RespuestaUsuarioOpcionMultipleInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaUsuarioOpcionMultiple == null)
            sError += ", RespuestaUsuarioOpcionMultiple";
         if (sError.Length > 0)
            throw new Exception("RespuestaUsuarioOpcionMultipleInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaUsuarioOpcionMultiple.OpcionRespuestaPlantilla == null)
            sError += ", OpcionRespuestaPlantilla";
         if (sError.Length > 0)
            throw new Exception("RespuestaUsuarioOpcionMultipleInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaUsuarioOpcionMultiple.TipoRespuestaUsuario == null)
            sError += ", TipoRespuestaUsuario";
         if (sError.Length > 0)
            throw new Exception("RespuestaUsuarioOpcionMultipleInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ReactivosUsuario.DAO", 
         "RespuestaUsuarioOpcionMultipleInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaUsuarioOpcionMultipleInsHlp: No se pudo conectar a la base de datos", "POV.ReactivosUsuario.DAO", 
         "RespuestaUsuarioOpcionMultipleInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO RespuestaUsuario (RespuestaPreguntaUsuarioID,FechaRegistro, TipoRespuestaUsuario, OpcionRespuestaPlantillaID) ");
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
         // respuestaUsuarioOpcionMultiple.FechaRegistro
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (respuestaUsuarioOpcionMultiple.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaUsuarioOpcionMultiple.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaUsuarioOpcionMultiple.TipoRespuestaUsuario
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (respuestaUsuarioOpcionMultiple.TipoRespuestaUsuario == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaUsuarioOpcionMultiple.TipoRespuestaUsuario;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaUsuarioOpcionMultiple.OpcionRespuestaPlantilla.OpcionRespuestaPlantillaID
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (respuestaUsuarioOpcionMultiple.OpcionRespuestaPlantilla.OpcionRespuestaPlantillaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaUsuarioOpcionMultiple.OpcionRespuestaPlantilla.OpcionRespuestaPlantillaID;
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
            throw new Exception("RespuestaUsuarioOpcionMultipleInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaUsuarioOpcionMultipleInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
