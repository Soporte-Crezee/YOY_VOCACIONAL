using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;

namespace POV.CentroEducativo.DAO { 
   /// <summary>
   /// Elimina un registro de CentroComputo en la BD
   /// </summary>
   public class CentroComputoDelHlp { 
      /// <summary>
      /// Elimina un registro de CentroComputoDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="centroComputoDelHlp">CentroComputoDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, Escuela escuela, CentroComputo centroComputo){
         object myFirm = new object();
         string sError = String.Empty;
         if (escuela == null)
            sError += ", Escuela";
         if (sError.Length > 0)
            throw new Exception("CentroComputoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (escuela.EscuelaID == null)
            sError += ", EscuelaID";
         if (sError.Length > 0)
            throw new Exception("CentroComputoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (centroComputo == null)
            sError += ", CentroComputo";
         if (sError.Length > 0)
            throw new Exception("CentroComputoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (centroComputo.CentroComputoID == null)
            sError += ", CentroComputoID";
         if (sError.Length > 0)
            throw new Exception("CentroComputoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "CentroComputoDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "CentroComputoDelHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "CentroComputoDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE CentroComputo ");
         sCmd.Append(" SET Activo = 0 ");
         // centroComputo.CentroComputoID
         sCmd.Append(" WHERE @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (centroComputo.CentroComputoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = centroComputo.CentroComputoID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         if (escuela.EscuelaID == null)
            sCmd.Append(" AND EscuelaID IS NULL ");
         else{ 
            // escuela.EscuelaID
            sCmd.Append(" AND EscuelaID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = escuela.EscuelaID;
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
            throw new Exception("CentroComputoDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("CentroComputoDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
