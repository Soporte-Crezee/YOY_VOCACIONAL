using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Elimina un registro de AppSuscripcion en la BD
   /// </summary>
   public class AppSuscripcionDelHlp { 
      /// <summary>
      /// Elimina un registro de AppSuscripcionDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="appSuscripcionDelHlp">AppSuscripcionDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, AppSuscripcion appSuscripcion, SocialHub socialHub){
         object myFirm = new object();
         string sError = String.Empty;
         if (appSuscripcion == null)
            sError += ", AppSuscripcion";
         if (sError.Length > 0)
            throw new Exception("AppSuscripcionDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (appSuscripcion.AppSuscripcionID == null)
            sError += ", AppSuscripcionID";
         if (sError.Length > 0)
            throw new Exception("AppSuscripcionDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (socialHub == null)
            sError += ", SocialHub";
         if (sError.Length > 0)
            throw new Exception("AppSuscripcionDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (socialHub.SocialHubID == null)
            sError += ", SocialHubID";
         if (sError.Length > 0)
            throw new Exception("AppSuscripcionDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "AppSuscripcionDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AppSuscripcionDelHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "AppSuscripcionDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM AppSuscripcion ");
         sCmd.Append(" WHERE AppSuscripcionID=@appSuscripcion_AppSuscripcionID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "appSuscripcion_AppSuscripcionID";
         if (appSuscripcion.AppSuscripcionID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = appSuscripcion.AppSuscripcionID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         if (appSuscripcion.AppSuscripcionID == null)
            sCmd.Append(" AND SocialHubID IS NULL ");
         else{ 
            sCmd.Append(" AND SocialHubID = @appSuscripcion_AppSuscripcionID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "appSuscripcion_AppSuscripcionID";
            sqlParam.Value = appSuscripcion.AppSuscripcionID;
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
            throw new Exception("AppSuscripcionDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AppSuscripcionDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
