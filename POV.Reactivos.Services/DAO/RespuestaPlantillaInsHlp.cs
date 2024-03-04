using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Reactivos.DAO;

namespace POV.Reactivos.DAO { 
   /// <summary>
   /// Guarda un registro de RespuestaPlantilla en la BD
   /// </summary>
   public class RespuestaPlantillaInsHlp { 
      /// <summary>
      /// Crea un registro de RespuestaPlantilla en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaPlantilla">RespuestaPlantilla que desea crear</param>
      public void Action(IDataContext dctx, RespuestaPlantilla respuestaPlantilla, Pregunta pregunta){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaPlantilla == null)
            sError += ", RespuestaPlantilla";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (pregunta == null)
            sError += ", pregunta";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (pregunta.PreguntaID == null)
            sError += ", PreguntaID";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPlantilla.TipoRespuestaPlantilla == null)
            sError += ", TipoRespuestaPlantilla";
         if (respuestaPlantilla.Estatus == null)
            sError += ", Estatus";
         if (respuestaPlantilla.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "RespuestaPlantillaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaPlantillaInsHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO", 
         "RespuestaPlantillaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO RespuestaPlantillaDinamico (Estatus, TipoRespuestaPlantilla, FechaRegistro, PreguntaID, TipoPuntaje) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         sCmd.Append(" @respuestaPlantilla_Estatus ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "respuestaPlantilla_Estatus";
         if (respuestaPlantilla.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantilla.Estatus;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@respuestaPlantilla_TipoRespuestaPlantilla ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "respuestaPlantilla_TipoRespuestaPlantilla";
         if (respuestaPlantilla.TipoRespuestaPlantilla == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantilla.TipoRespuestaPlantilla;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@respuestaPlantilla_FechaRegistro ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "respuestaPlantilla_FechaRegistro";
         if (respuestaPlantilla.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantilla.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@pregunta_PreguntaID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "pregunta_PreguntaID";
         if (pregunta.PreguntaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pregunta.PreguntaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@respuestaPlantilla_TipoPuntaje ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "respuestaPlantilla_TipoPuntaje";
         if (pregunta.PreguntaID == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = 0;
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
            throw new Exception("RespuestaPlantillaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaPlantillaInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
