using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;

namespace POV.Comun.DAO { 
   /// <summary>
   /// Elimina una Ciudad en la base de datos
   /// </summary>
   public class CiudadDelHlp { 
      /// <summary>
      /// Elimina un registro de Ciudad en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ciudad">Ciudad que desea eliminar</param>
      public void Action(IDataContext dctx, Ciudad ciudad){
         object myFirm = new object();
         string sError = "";
         if (ciudad == null)
            sError += ", Ciudad";
         if (sError.Length > 0)
            throw new Exception("CiudadDelHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (ciudad.CiudadID == null)
            sError += ", CiudadID";
         if (sError.Length > 0)
            throw new Exception("CiudadDelHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (dctx == null)
             throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Comun.DAO", 
         "CiudadDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
             throw new StandardException(MessageType.Error, "", "CiudadDelHlp: Hubo un error al conectarse a la base de datos", "POV.Comun.DAO", 
         "CiudadDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM CIUDAD ");
         if (ciudad.CiudadID == null)
            sCmd.Append(" WHERE CIUDADID IS NULL ");
         else{ 
            sCmd.Append(" WHERE CIUDADID = @ciudad_CiudadID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "ciudad_CiudadID";
            sqlParam.Value = ciudad.CiudadID;
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
            throw new Exception("CiudadDelHlp: Hubo un error al eliminar el registro o no existe. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("CiudadDelHlp: Hubo un error al eliminar el registro o no existe.");
      }
   } 
}
