using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;

namespace POV.Comun.DAO
{ 
   /// <summary>
   /// Elimina un País en la base de datos
   /// </summary>
   public class PaisDelHlp { 
      /// <summary>
      /// Elimina un registro de País en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="país">País que desea eliminar</param>
      public void Action(IDataContext dctx, Pais pais){
         object myFirm = new object();
         string sError = "";
         if (pais == null)
            sError += ", País";
         if (sError.Length > 0)
            throw new Exception("PaisDelHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (pais.PaisID == null)
            sError += ", PaisID";
         if (sError.Length > 0)
            throw new Exception("PaisDelHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "VITADAT.Comun.DAO", 
         "PaisDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PaisDelHlp: Hubo un error al conectarse a la base de datos", "VITADAT.Comun.DAO", 
         "PaisDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM PAIS ");
         if (pais.PaisID == null)
            sCmd.Append(" WHERE PAISID IS NULL ");
         else{ 
            sCmd.Append(" WHERE PAISID = @pais_PaisID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "pais_PaisID";
            sqlParam.Value = pais.PaisID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PaisDelHlp: Hubo un error al eliminar el registro o no existe. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PaisDelHlp: Hubo un error al eliminar el registro o no existe.");
      }
   } 
}
