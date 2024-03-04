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
   /// Guarda un registro de EvaluacionRango en la BD
   /// </summary>
   public class EvaluacionRangoInsHlp { 
      /// <summary>
      /// Crea un registro de EvaluacionRango en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="evaluacionRango">EvaluacionRango que desea crear</param>
      public void Action(IDataContext dctx, EvaluacionRango evaluacionRango, Reactivo reactivo){
         object myFirm = new object();
         string sError = String.Empty;
         if (evaluacionRango == null)
            sError += ", EvaluacionRango";
         if (sError.Length > 0)
            throw new Exception("EvaluacionRangoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (evaluacionRango.EvaluacionID == null)
            sError += ", EvaluacionRangoID";
         if (sError.Length > 0)
            throw new Exception("EvaluacionRangoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (reactivo == null)
            sError += ", Reactivo";
         if (sError.Length > 0)
            throw new Exception("EvaluacionRangoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (reactivo.ReactivoID == null)
            sError += ", ReactivoID";
         if (sError.Length > 0)
            throw new Exception("EvaluacionRangoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.DAO", 
         "EvaluacionRangoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "EvaluacionRangoInsHlp: No se pudo conectar a la base de datos", "POV.DAO", 
         "EvaluacionRangoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO EvaluacionRango (EvaluacionRangoID,Inicio, Fin, PorcentajeCalificacion, ReactivoDiagnosticoID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (evaluacionRango.EvaluacionID == null)
            sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = evaluacionRango.EvaluacionID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (evaluacionRango.Inicio == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = evaluacionRango.Inicio;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (evaluacionRango.Fin == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = evaluacionRango.Fin;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (evaluacionRango.PorcentajeCalificacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = evaluacionRango.PorcentajeCalificacion;
         sqlParam.DbType = DbType.Decimal;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (reactivo.ReactivoID == null)
            sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = reactivo.ReactivoID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("EvaluacionRangoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("EvaluacionRangoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
