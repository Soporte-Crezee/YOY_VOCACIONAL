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
   /// Actualiza un registro de RecursoContrato en la BD
   /// </summary>
   public class RecursoContratoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de RecursoContratoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="recursoContratoUpdHlp">RecursoContratoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">RecursoContratoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, RecursoContrato recursoContrato, RecursoContrato anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (recursoContrato == null)
            sError += ", RecursoContrato";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("RecursoContratoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.RecursoContratoID == null)
            sError += ", Anterior RecursoContratoID";
         if (sError.Length > 0)
            throw new Exception("RecursoContratoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", 
         "RecursoContratoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RecursoContratoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", 
         "RecursoContratoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE RecursoContrato ");
         if (recursoContrato.EsAsignacionManual == null)
            sCmd.Append(" SET EsAsignacionManual = NULL ");
         else{ 
            // recursoContrato.EsAsignacionManual
            sCmd.Append(" SET EsAsignacionManual = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = recursoContrato.EsAsignacionManual;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (recursoContrato.EsPaquetePorPruebaPivote == null)
            sCmd.Append(" ,EsPaquetePorPruebaPivote = NULL ");
         else{ 
            // recursoContrato.EsPaquetePorPruebaPivote
            sCmd.Append(" ,EsPaquetePorPruebaPivote = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = recursoContrato.EsPaquetePorPruebaPivote;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.RecursoContratoID == null)
            sCmd.Append(" WHERE RecursoContratoID IS NULL ");
         else{ 
            // anterior.RecursoContratoID
            sCmd.Append(" WHERE RecursoContratoID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = anterior.RecursoContratoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RecursoContratoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RecursoContratoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
