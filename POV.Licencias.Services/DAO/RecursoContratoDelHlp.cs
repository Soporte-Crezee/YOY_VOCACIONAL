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
   /// Elimina un registro de RecursoContrato en la BD
   /// </summary>
   public class RecursoContratoDelHlp { 
      /// <summary>
      /// Elimina un registro de RecursoContratoDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="recursoContratoDelHlp">RecursoContratoDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, RecursoContrato recursoContrato){
         object myFirm = new object();
         string sError = String.Empty;
         if (recursoContrato == null)
            sError += ", RecursoContrato";
         if (sError.Length > 0)
            throw new Exception("RecursoContratoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (recursoContrato.RecursoContratoID == null)
            sError += ", RecursoContratoID";
         if (sError.Length > 0)
            throw new Exception("RecursoContratoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", 
         "RecursoContratoDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RecursoContratoDelHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO", 
         "RecursoContratoDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE RecursoContrato ");
         sCmd.Append(" SET Activo = 0 ");
         if (recursoContrato.RecursoContratoID == null)
            sCmd.Append(" WHERE RecursoContratoID IS NULL ");
         else{ 
            // recursoContrato.RecursoContratoID
            sCmd.Append(" WHERE RecursoContratoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = recursoContrato.RecursoContratoID;
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
            throw new Exception("RecursoContratoDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RecursoContratoDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
