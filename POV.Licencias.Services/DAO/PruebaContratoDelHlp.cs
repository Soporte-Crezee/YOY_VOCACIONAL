using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;

namespace POV.Licencias.DAO { 
   /// <summary>
   /// Elimina un registro de PruebaContrato en la BD
   /// </summary>
   internal class PruebaContratoDelHlp { 
      /// <summary>
      /// Elimina un registro de PruebaContratoDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="pruebaContratoDelHlp">PruebaContratoDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, PruebaContrato pruebaContrato){
         object myFirm = new object();
         string sError = String.Empty;
         if (pruebaContrato == null)
            sError += ", PruebaContrato";
         if (sError.Length > 0)
            throw new Exception("PruebaContratoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (pruebaContrato.PruebaContratoID == null)
             sError += ", PruebaContratoID";
         if (sError.Length > 0)
            throw new Exception("PruebaContratoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", 
         "PruebaContratoDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PruebaContratoDelHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO", 
         "PruebaContratoDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM PruebaContrato ");
         if (pruebaContrato.PruebaContratoID == null)
             sCmd.Append(" WHERE PruebaContratoID IS NULL ");
         else{ 
            sCmd.Append(" WHERE PruebaContratoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = pruebaContrato.PruebaContratoID;
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
            throw new Exception("PruebaContratoDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PruebaContratoDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
