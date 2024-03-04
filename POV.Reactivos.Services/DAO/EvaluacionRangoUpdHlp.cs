using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Reactivos.DAO;

namespace POV.Reactivos.DAO
{ 
   /// <summary>
   /// Actualiza un registro de EvaluacionRango en la BD
   /// </summary>
   public class EvaluacionRangoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de EvaluacionRangoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="evaluacionRangoUpdHlp">EvaluacionRangoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">EvaluacionRangoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, EvaluacionRango evaluacionRango, EvaluacionRango anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (evaluacionRango == null)
            sError += ", EvaluacionRango";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("EvaluacionRangoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.EvaluacionID == null)
            sError += ", Anterior EvaluacionRangoID";
         if (sError.Length > 0)
            throw new Exception("EvaluacionRangoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.DAO", 
         "EvaluacionRangoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "EvaluacionRangoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.DAO", 
         "EvaluacionRangoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE EvaluacionRango ");
         if (evaluacionRango.Inicio != null){
            sCmd.Append(" SET Inicio = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = evaluacionRango.Inicio;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (evaluacionRango.Fin != null){
            sCmd.Append(" ,Fin = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = evaluacionRango.Fin;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (evaluacionRango.PorcentajeCalificacion != null){
            sCmd.Append(" ,PorcentajeCalificacion = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = evaluacionRango.PorcentajeCalificacion;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.EvaluacionID == null)
            sCmd.Append(" WHERE EvaluacionRangoID = NULL ");
         else{ 
            sCmd.Append(" WHERE EvaluacionRangoID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = anterior.EvaluacionID;
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
            throw new Exception("EvaluacionRangoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("EvaluacionRangoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
