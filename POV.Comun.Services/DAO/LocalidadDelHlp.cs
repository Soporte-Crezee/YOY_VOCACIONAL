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
   /// Elimina una Localidad en la base de datos
   /// </summary>
   public class LocalidadDelHlp { 
      /// <summary>
      /// Elimina un registro de Localidad en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="localidad">Localidad que desea eliminar</param>
      public void Action(IDataContext dctx, Localidad localidad){
         object myFirm = new object();
         string sError = "";
         if (localidad == null)
            sError += ", Localidad";
         if (sError.Length > 0)
            throw new Exception("LocalidadDelHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (localidad.LocalidadID == null)
            sError += ", LocalidadID";
         if (sError.Length > 0)
            throw new Exception("LocalidadDelHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "VITADAT.Comun.DAO", 
         "LocalidadDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "LocalidadDelHlp: Hubo un error al conectarse a la base de datos", "VITADAT.Comun.DAO", 
         "LocalidadDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM LOCALIDAD ");
         if (localidad.LocalidadID == null)
            sCmd.Append(" WHERE LOCALIDADID IS NULL ");
         else{ 
            sCmd.Append(" WHERE LOCALIDADID = @localidad_LocalidadID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "localidad_LocalidadID";
            sqlParam.Value = localidad.LocalidadID;
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
            throw new Exception("LocalidadDelHlp: Hubo un error al eliminar el registro o no existe. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("LocalidadDelHlp: Hubo un error al eliminar el registro o no existe.");
      }
   } 
}
