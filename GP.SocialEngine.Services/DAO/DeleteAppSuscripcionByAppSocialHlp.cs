using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.Interfaces;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Elimina un registro de AppSuscripcion en la BD
   /// </summary>
   public class DeleteAppSuscripcionByAppSocialHlp { 
      /// <summary>
      /// Elimina un registro de DeleteAppSuscripcionByReactivoHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="deleteAppSuscripcionByReactivoHlp">DeleteAppSuscripcionByReactivoHlp que desea eliminar</param>
      public void Action(IDataContext dctx, IAppSocial appSocial){
         object myFirm = new object();
         string sError = String.Empty;
         if (appSocial == null)
            sError += ", IAppSocial";
         if (sError.Length > 0)
            throw new Exception("DeleteAppSuscripcionByAppSocialHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (appSocial.GetAppKey() == null)
            sError += ", AppKey";
         if (sError.Length > 0)
            throw new Exception("DeleteAppSuscripcionByAppSocialHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "DeleteAppSuscripcionByAppSocialHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "DeleteAppSuscripcionByAppSocialHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "DeleteAppSuscripcionByAppSocialHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE AppSuscripcion SET Estatus = 0 ");
         sCmd.Append(" WHERE AppSocialID =@appSuscripcion_AppSocial_GetAppKey ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "appSuscripcion_AppSocial_GetAppKey";
         if (appSocial.GetAppKey() == null)
            sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = appSocial.GetAppKey();
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("DeleteAppSuscripcionByAppSocialHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
      }
   } 
}
