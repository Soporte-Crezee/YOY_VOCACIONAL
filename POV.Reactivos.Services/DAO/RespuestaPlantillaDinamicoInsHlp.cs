using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Modelo.BO;

namespace POV.Reactivos.DAO { 
   /// <summary>
   /// Guarda un registro de RespuestaPlantilla en la BD
   /// </summary>
   internal class RespuestaPlantillaDinamicoInsHlp { 
      /// <summary>
      /// Crea un registro de RespuestaPlantilla en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaPlantilla">RespuestaPlantilla que desea crear</param>
      public void Action(IDataContext dctx, Pregunta pregunta, RespuestaPlantilla respuestaPlantilla){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaPlantilla == null)
            sError += ", RespuestaPlantilla";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (pregunta == null)
            sError += ", Pregunta";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (pregunta.PreguntaID == null)
            sError += ", PreguntaID";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPlantilla.Estatus == null)
            sError += ", Estatus";
         if (respuestaPlantilla.TipoRespuestaPlantilla == null)
            sError += ", TipoRespuestaPlantilla";
         if (respuestaPlantilla.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (respuestaPlantilla.TipoPuntaje == null)
            sError += ", TipoPuntaje";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "RespuestaPlantillaDinamicoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaPlantillaDinamicoInsHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO", 
         "RespuestaPlantillaDinamicoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO RespuestaPlantillaDinamico (Estatus, TipoRespuestaPlantilla, PreguntaID, FechaRegistro, TipoPuntaje) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // respuestaPlantilla.Estatus
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (respuestaPlantilla.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantilla.Estatus;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPlantilla.TipoRespuestaPlantilla
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (respuestaPlantilla.TipoRespuestaPlantilla == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantilla.TipoRespuestaPlantilla;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         // pregunta.PreguntaID
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (pregunta.PreguntaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pregunta.PreguntaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPlantilla.FechaRegistro
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (respuestaPlantilla.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantilla.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // pregunta.TipoPuntaje
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (respuestaPlantilla.TipoPuntaje == null)
            sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = respuestaPlantilla.TipoPuntaje;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RespuestaPlantillaDinamicoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaPlantillaDinamicoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
