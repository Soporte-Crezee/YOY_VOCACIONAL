using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.CentroEducativo.BO;

namespace POV.Licencias.DAO { 
   /// <summary>
   /// Elimina un registro de CicloContrato en la BD
   /// </summary>
   public class CicloContratoDelHlp { 
      /// <summary>
      /// Elimina un registro de CicloContratoDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="cicloContratoDelHlp">CicloContratoDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, CicloContrato cicloContrato){
         object myFirm = new object();
         string sError = String.Empty;
         if (cicloContrato == null)
            sError += ", CicloContrato";
         if (sError.Length > 0)
            throw new Exception("CicloContratoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (cicloContrato.CicloContratoID == null)
            sError += ", CicloContratoID";
         if (sError.Length > 0)
            throw new Exception("CicloContratoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", 
         "CicloContratoDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "CicloContratoDelHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO", 
         "CicloContratoDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE CicloContrato ");
         sCmd.Append(" SET Activo = 0 ");
         if (cicloContrato.CicloContratoID == null)
            sCmd.Append(" WHERE CicloContratoID IS NULL ");
         else{ 
            // cicloContrato.CicloContratoID
            sCmd.Append(" WHERE CicloContratoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = cicloContrato.CicloContratoID;
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
            throw new Exception("CicloContratoDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("CicloContratoDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
