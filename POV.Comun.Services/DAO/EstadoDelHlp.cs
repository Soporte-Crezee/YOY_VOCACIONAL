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
   /// Elimina un Estado en la base de datos
   /// </summary>
   public class EstadoDelHlp { 
      /// <summary>
      /// Elimina un registro de Estado en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="estado">Estado que desea eliminar</param>
      public void Action(IDataContext dctx, Estado estado){
         object myFirm = new object();
         string sError = "";
         if (estado == null)
            sError += ", Estado";
         if (sError.Length > 0)
            throw new Exception("EstadoDelHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (estado.EstadoID == null)
            sError += ", EstadoID";
         if (sError.Length > 0)
            throw new Exception("EstadoDelHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (dctx == null)
             throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Comun.DAO", 
         "EstadoDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "EstadoDelHlp: Hubo un error al conectarse a la base de datos", "POV.Comun.DAO", 
         "EstadoDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM ESTADO ");
         if (estado.EstadoID == null)
            sCmd.Append(" WHERE ESTADOID IS NULL ");
         else{ 
            sCmd.Append(" WHERE ESTADOID = @estado_EstadoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "estado_EstadoID";
            sqlParam.Value = estado.EstadoID;
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
            throw new Exception("EstadoDelHlp: Hubo un error al eliminar el registro o no existe. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("EstadoDelHlp: Hubo un error al eliminar el registro o no existe.");
      }
   } 
}
