using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.DAO { 
   /// <summary>
   /// Elimina un registro de APrueba en la BD
   /// </summary>
   internal class PruebaDelHlp { 
      /// <summary>
      /// Elimina un registro de PruebaDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="pruebaDelHlp">PruebaDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, APrueba prueba){
         object myFirm = new object();
         string sError = String.Empty;
         if (prueba == null)
            sError += ", Prueba";
         if (sError.Length > 0)
            throw new Exception("PruebaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (prueba.PruebaID == null)
            sError += ", PruebaID";
         if (sError.Length > 0)
            throw new Exception("PruebaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.DAO", 
         "PruebaDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PruebaDelHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.DAO", 
         "PruebaDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.AppendFormat(" UPDATE Prueba SET EstadoLiberacion = {0} ", (byte)EEstadoLiberacionPrueba.INACTIVA);
         if (prueba.PruebaID == null)
            sCmd.Append(" WHERE PruebaID IS NULL ");
         else{ 
            // prueba.PruebaID
            sCmd.Append(" WHERE PruebaID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = prueba.PruebaID;
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
            throw new Exception("PruebaDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PruebaDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
