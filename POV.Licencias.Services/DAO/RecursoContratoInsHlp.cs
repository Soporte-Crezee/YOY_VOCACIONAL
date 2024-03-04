using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;

namespace POV.Licencias.DAO { 
   /// <summary>
   /// Guarda un registro de RecursoContrato en la BD
   /// </summary>
   public class RecursoContratoInsHlp { 
      /// <summary>
      /// Crea un registro de RecursoContrato en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="recursoContrato">RecursoContrato que desea crear</param>
      public void Action(IDataContext dctx, CicloContrato cicloContrato, RecursoContrato recursoContrato){
         object myFirm = new object();
         string sError = String.Empty;
         if (recursoContrato == null)
            sError += ", RecursoContrato";
         if (sError.Length > 0)
            throw new Exception("RecursoContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (cicloContrato == null)
            sError += ", CicloContrato";
         if (sError.Length > 0)
            throw new Exception("RecursoContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (cicloContrato.CicloContratoID == null)
            sError += ", CicloContratoID";
         if (sError.Length > 0)
            throw new Exception("RecursoContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (recursoContrato.Activo == null)
            sError += ", Activo";
         if (recursoContrato.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (sError.Length > 0)
            throw new Exception("RecursoContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", 
         "RecursoContratoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RecursoContratoInsHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO", 
         "RecursoContratoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO RecursoContrato (CicloContratoID, EsAsignacionManual, EsPaquetePorPruebaPivote, Activo, FechaRegistro) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // cicloContrato.CicloContratoID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (cicloContrato.CicloContratoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloContrato.CicloContratoID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // recursoContrato.EsAsignacionManual
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (recursoContrato.EsAsignacionManual == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = recursoContrato.EsAsignacionManual;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // recursoContrato.EsPaquetePorPruebaPivote
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (recursoContrato.EsPaquetePorPruebaPivote == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = recursoContrato.EsPaquetePorPruebaPivote;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // recursoContrato.Activo
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (recursoContrato.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = recursoContrato.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // recursoContrato.FechaRegistro
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (recursoContrato.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = recursoContrato.FechaRegistro;
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
            throw new Exception("RecursoContratoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RecursoContratoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
