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
   /// Actualiza un registro de RespuestaUsuarioOpcionMultiple en la BD
   /// </summary>
   public class RespuestaUsuarioOpcionMultipleUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de RespuestaUsuarioOpcionMultipleUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaUsuarioOpcionMultipleUpdHlp">RespuestaUsuarioOpcionMultipleUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">RespuestaUsuarioOpcionMultipleUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, RespuestaPreguntaUsuario respuestaPreguntaUsuario, RespuestaUsuarioOpcionMultiple respuestaUsuarioOpcionMultiple, RespuestaUsuarioOpcionMultiple anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (respuestaUsuarioOpcionMultiple == null)
            sError += ", RespuestaUsuarioOpcionMultiple";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("RespuestaUsuarioOpcionMultipleUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ReactivosUsuario.DAO", 
         "RespuestaUsuarioOpcionMultipleUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaUsuarioOpcionMultipleUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.ReactivosUsuario.DAO", 
         "RespuestaUsuarioOpcionMultipleUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE RespuestaUsuario ");
         if (respuestaUsuarioOpcionMultiple.OpcionRespuestaPlantilla.OpcionRespuestaPlantillaID == null)
            sCmd.Append(" SET OpcionRespuestaPlantillaID = NULL ");
         else{ 
            // respuestaUsuarioOpcionMultiple.OpcionRespuestaPlantilla.OpcionRespuestaPlantillaID
            sCmd.Append(" SET OpcionRespuestaPlantillaID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = respuestaUsuarioOpcionMultiple.OpcionRespuestaPlantilla.OpcionRespuestaPlantillaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID == null)
            sCmd.Append(" WHERE RespuestaPreguntaUsuarioID IS NULL ");
         else{ 
            // respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID
            sCmd.Append(" WHERE RespuestaPreguntaUsuarioID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = respuestaPreguntaUsuario.RespuestaPreguntaUsuarioID;
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
            throw new Exception("RespuestaUsuarioOpcionMultipleUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaUsuarioOpcionMultipleUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
