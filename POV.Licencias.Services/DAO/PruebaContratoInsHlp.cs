using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.Prueba.BO;

namespace POV.Licencias.DAO { 
   /// <summary>
   /// Guarda un registro de PruebaContrato en la BD
   /// </summary>
   internal class PruebaContratoInsHlp { 
      /// <summary>
      /// Crea un registro de PruebaContrato en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="pruebaContrato">PruebaContrato que desea crear</param>
       public void Action(IDataContext dctx, RecursoContrato recursoContrato, PruebaContrato pruebaContrato)
       {
         object myFirm = new object();
         string sError = String.Empty;
         if (recursoContrato == null)
            sError += ", Contrato";
         if (sError.Length > 0)
            throw new Exception("PruebaContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (recursoContrato.RecursoContratoID == null)
            sError += ", ContratoID";
         if (sError.Length > 0)
            throw new Exception("PruebaContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (pruebaContrato == null)
            sError += ", PruebaContrato";
         if (sError.Length > 0)
            throw new Exception("PruebaContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (pruebaContrato.Prueba == null)
            sError += ", Prueba";
         if (sError.Length > 0)
            throw new Exception("PruebaContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (pruebaContrato.Prueba.PruebaID == null)
            sError += ", PruebaID";
         if (pruebaContrato.TipoPruebaContrato == null)
            sError += ", PruebaID";
         if (pruebaContrato.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (pruebaContrato.Activo == null)
            sError += ", Activo";
         if (sError.Length > 0)
            throw new Exception("PruebaContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", 
         "PruebaContratoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PruebaContratoInsHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO", 
         "PruebaContratoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO PruebaContrato (RecursoContratoID, PruebaID, TipoPruebaContrato, FechaRegistro, Activo) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // recursoContrato.RecursoContratoID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (recursoContrato.RecursoContratoID == null)
            sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = recursoContrato.RecursoContratoID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // pruebaContrato.Prueba.PruebaID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (pruebaContrato.Prueba.PruebaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pruebaContrato.Prueba.PruebaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // pruebaContrato.TipoPruebaContrato
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (pruebaContrato.TipoPruebaContrato == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pruebaContrato.TipoPruebaContrato;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         // pruebaContrato.FechaRegistro
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (pruebaContrato.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pruebaContrato.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // pruebaContrato.Activo
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (pruebaContrato.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pruebaContrato.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PruebaContratoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PruebaContratoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
