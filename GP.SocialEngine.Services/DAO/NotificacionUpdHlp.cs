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
   /// Actualiza un registro de Notificacion en la BD
   /// </summary>
   public class NotificacionUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de NotificacionUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="notificacionUpdHlp">NotificacionUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">NotificacionUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Notificacion notificacion, Notificacion anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (notificacion == null)
            sError += ", Notificacion";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("NotificacionUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.NotificacionID == null)
            sError += ", Anterior NotificacionID";
         if (sError.Length > 0)
            throw new Exception("NotificacionUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (notificacion.EstatusNotificacion == null)
            sError += ", EstatusNotificacion";
         if (sError.Length > 0)
            throw new Exception("NotificacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "NotificacionUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "NotificacionUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "NotificacionUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Notificacion ");
         if (notificacion.EstatusNotificacion == null)
            sCmd.Append(" SET EstatusNotificacion = NULL ");
         else{ 
            sCmd.Append(" SET EstatusNotificacion = @notificacion_EstatusNotificacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "notificacion_EstatusNotificacion";
            sqlParam.Value = notificacion.EstatusNotificacion;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.NotificacionID == null)
            sCmd.Append(" WHERE NotificacionID IS NULL ");
         else{ 
            sCmd.Append(" WHERE NotificacionID = @anterior_NotificacionID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_NotificacionID";
            sqlParam.Value = anterior.NotificacionID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("NotificacionUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("NotificacionUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
