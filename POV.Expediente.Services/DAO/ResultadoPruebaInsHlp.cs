using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Expediente.BO;
using POV.Prueba.BO;

namespace POV.Expediente.DAO { 
   /// <summary>
   /// Guarda un registro de ResultadoPrueba en la BD
   /// </summary>
   public class ResultadoPruebaInsHlp { 
      /// <summary>
      /// Crea un registro de ResultadoPrueba en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="resultadoPrueba">ResultadoPrueba que desea crear</param>
      public void Action(IDataContext dctx, AResultadoPrueba resultadoPrueba, DetalleCicloEscolar detalleCicloEscolar){
         object myFirm = new object();
         string sError = String.Empty;
         if (resultadoPrueba == null)
            sError += ", ResultadoPrueba";
         if (sError.Length > 0)
            throw new Exception("ResultadoPruebaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (resultadoPrueba.Prueba == null)
             sError += ", Prueba";
         if (sError.Length > 0)
             throw new Exception("ResultadoPruebaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (resultadoPrueba.Prueba.PruebaID == null)
             sError += ", PruebaID";
         if (sError.Length > 0)
             throw new Exception("ResultadoPruebaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (detalleCicloEscolar == null)
            sError += ", DetalleCicloEscolar";
         if (sError.Length > 0)
            throw new Exception("ResultadoPruebaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (detalleCicloEscolar.DetalleCicloEscolarID == null)
            sError += ", DetalleCicloEscolarID";
         if (sError.Length > 0)
            throw new Exception("ResultadoPruebaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (resultadoPrueba.Tipo == null)
            sError += ", Tipo";
         if (resultadoPrueba.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (sError.Length > 0)
            throw new Exception("ResultadoPruebaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "ResultadoPruebaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ResultadoPruebaInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "ResultadoPruebaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO ResultadoPrueba (DetalleCicloEscolarID, FechaRegistro, Tipo, PruebaID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // detalleCicloEscolar.DetalleCicloEscolarID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (detalleCicloEscolar.DetalleCicloEscolarID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = detalleCicloEscolar.DetalleCicloEscolarID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // resultadoPrueba.FechaRegistro
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (resultadoPrueba.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = resultadoPrueba.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // resultadoPrueba.Tipo
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (resultadoPrueba.Tipo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = resultadoPrueba.Tipo;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         // resultadoPrueba.Prueba.PruebaID
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (resultadoPrueba.Prueba.PruebaID == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = resultadoPrueba.Prueba.PruebaID;
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
            throw new Exception("ResultadoPruebaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ResultadoPruebaInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
