using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;
using POV.Expediente.BO;

namespace POV.Expediente.DAO { 
   /// <summary>
   /// Guarda un registro de AResultadoClasificador en la BD
   /// </summary>
    internal class ResultadoClasificadorInsHlp
    { 
      /// <summary>
      /// Crea un registro de AResultadoClasificador en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="aResultadoClasificador">AResultadoClasificador que desea crear</param>
      public void Action(IDataContext dctx, AResultadoClasificador resultadoClasificador){
         object myFirm = new object();
         string sError = String.Empty;
         if (resultadoClasificador == null)
            sError += ", AResultadoClasificador";
         if (sError.Length > 0)
            throw new Exception("ResultadoClasificadorInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (resultadoClasificador.Modelo == null)
             sError += ", Modelo";
         if (sError.Length > 0)
             throw new Exception("ResultadoClasificadorInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (resultadoClasificador.Modelo.ModeloID == null)
             sError += ", ModeloID";
         if (sError.Length > 0)
             throw new Exception("ResultadoClasificadorInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

         if (resultadoClasificador.ResultadoPrueba == null)
             sError += ", ResultadoPrueba";
         if (sError.Length > 0)
             throw new Exception("ResultadoClasificadorInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (resultadoClasificador.ResultadoPrueba.ResultadoPruebaID == null)
             sError += ", ResultadoPruebaID";
         if (sError.Length > 0)
             throw new Exception("ResultadoClasificadorInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));


         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Expediente.DAO", 
         "ResultadoClasificadorInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ResultadoClasificadorInsHlp: No se pudo conectar a la base de datos", "POV.Expediente.DAO", 
         "ResultadoClasificadorInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO ResultadoClasificador (ResultadoPruebaID, ModeloID, Tipo, FechaRegistro) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // resultadoClasificador.ResultadoPrueba.ResultadoPruebaID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (resultadoClasificador.ResultadoPrueba.ResultadoPruebaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = resultadoClasificador.ResultadoPrueba.ResultadoPruebaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // resultadoClasificador.Modelo.ModeloID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (resultadoClasificador.Modelo.ModeloID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = resultadoClasificador.Modelo.ModeloID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // resultadoClasificador.TipoResultadoClasificador
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (resultadoClasificador.TipoResultadoClasificador == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = resultadoClasificador.TipoResultadoClasificador;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         // resultadoClasificador.FechaRegistro
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (resultadoClasificador.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = resultadoClasificador.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ResultadoClasificadorInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ResultadoClasificadorInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
