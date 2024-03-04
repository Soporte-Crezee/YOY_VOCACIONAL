using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Reactivos.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DA { 
   /// <summary>
   /// Guarda un registro de RespuestaPreguntaDinamica en la BD
   /// </summary>
   internal class RespuestaOpcionDinamicaDetalleDAInsHlp { 
      /// <summary>
      /// Crea un registro de RespuestaPreguntaDinamica en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaPreguntaDinamica">RespuestaPreguntaDinamica que desea crear</param>
      public void Action(IDataContext dctx, RespuestaPreguntaDinamica respuestaPreguntaDinamica, OpcionRespuestaPlantilla opcionRespuesta){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaPreguntaDinamica == null)
            sError += ", RespuestaPreguntaDinamica";
         if (sError.Length > 0)
            throw new Exception("RespuestaOpcionDinamicaDetalleDAInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPreguntaDinamica.RespuestaPreguntaID == null)
            sError += ", RespuestaPreguntaID";
         if (sError.Length > 0)
            throw new Exception("RespuestaOpcionDinamicaDetalleDAInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (opcionRespuesta == null)
            sError += ", OpcionRespuestaPlantilla";
         if (sError.Length > 0)
            throw new Exception("RespuestaOpcionDinamicaDetalleDAInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (opcionRespuesta.OpcionRespuestaPlantillaID == null)
            sError += ", OpcionRespuestaPlantillaID";
         if (sError.Length > 0)
            throw new Exception("RespuestaOpcionDinamicaDetalleDAInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "RespuestaOpcionDinamicaDetalleDAInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaOpcionDinamicaDetalleDAInsHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "RespuestaOpcionDinamicaDetalleDAInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO RespuestaOpcionDinamicaDetalle (RespuestaPreguntaID, OpcionRespuestaPlantillaID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // respuestaPreguntaDinamica.RespuestaPreguntaID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (respuestaPreguntaDinamica.RespuestaPreguntaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPreguntaDinamica.RespuestaPreguntaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // opcionRespuesta.OpcionRespuestaPlantillaID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (opcionRespuesta.OpcionRespuestaPlantillaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = opcionRespuesta.OpcionRespuestaPlantillaID;
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
            throw new Exception("RespuestaOpcionDinamicaDetalleDAInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaOpcionDinamicaDetalleDAInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
