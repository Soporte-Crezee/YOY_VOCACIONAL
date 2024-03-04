using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;

namespace POV.Reactivos.DAO { 
   /// <summary>
   /// Consulta un registro de EvaluacionRango en la BD
   /// </summary>
   public class EvaluacionRangoRetHlp { 
      /// <summary>
      /// Consulta registros de EvaluacionRango en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="evaluacionRango">EvaluacionRango que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de EvaluacionRango generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, EvaluacionRango evaluacionRango, Reactivo reactivo){
         object myFirm = new object();
         string sError = String.Empty;
         if (evaluacionRango == null)
            sError += ", EvaluacionRango";
         if (sError.Length > 0)
            throw new Exception("EvaluacionRangoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "EvaluacionRangoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "EvaluacionRangoRetHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO", 
         "EvaluacionRangoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT EvaluacionRangoID,Inicio,Fin,PorcentajeCalificacion,ReactivoDiagnosticoID ");
         sCmd.Append(" FROM EvaluacionRango ");
         StringBuilder s_Var = new StringBuilder();
         if (evaluacionRango.EvaluacionID != null){
            s_Var.Append(" evaluacionRangoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = evaluacionRango.EvaluacionID;
            sqlParam.DbType = DbType.Guid ;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (evaluacionRango.Inicio != null){
            s_Var.Append(" AND Inicio = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = evaluacionRango.Inicio;
            sqlParam.DbType = DbType.Int16 ;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (evaluacionRango.Fin != null){
            s_Var.Append(" AND Fin = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = evaluacionRango.Fin;
            sqlParam.DbType = DbType.Int16 ;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (evaluacionRango.PorcentajeCalificacion != null){
            s_Var.Append(" AND PorcentajeCalificacion = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = evaluacionRango.PorcentajeCalificacion;
            sqlParam.DbType = DbType.Decimal ;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reactivo.ReactivoID != null){
            s_Var.Append(" AND ReactivoDiagnosticoID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = reactivo.ReactivoID;
            sqlParam.DbType = DbType.Guid ;
            sqlCmd.Parameters.Add(sqlParam);
         }
         s_Var.Append("  ");
         string s_Varres = s_Var.ToString().Trim();
         if (s_Varres.Length > 0){
            if (s_Varres.StartsWith("AND "))
               s_Varres = s_Varres.Substring(4);
            else if (s_Varres.StartsWith("OR "))
               s_Varres = s_Varres.Substring(3);
            else if (s_Varres.StartsWith(","))
               s_Varres = s_Varres.Substring(1);
            sCmd.Append(" WHERE " + s_Varres);
         }
         sCmd.Append(" ORDER BY Inicio ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "EvaluacionRango");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("EvaluacionRangoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
