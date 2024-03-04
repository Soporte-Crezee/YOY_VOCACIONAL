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
   /// Guarda un registro de Notificacion en la BD
   /// </summary>
   public class NotificacionInsHlp { 
      /// <summary>
      /// Crea un registro de Notificacion en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="notificacion">Notificacion que desea crear</param>
      public void Action(IDataContext dctx, Notificacion notificacion){
         object myFirm = new object();
         string sError = String.Empty;
         if (notificacion == null)
            sError += ", Notificacion";
         if (sError.Length > 0)
            throw new Exception("NotificacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (notificacion.Emisor == null)
            sError += ", Emisor";
         if (sError.Length > 0)
            throw new Exception("NotificacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (notificacion.Emisor.UsuarioSocialID == null)
            sError += ", Emisor.UsuarioSocialID";
         if (sError.Length > 0)
            throw new Exception("NotificacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (notificacion.Receptor == null)
            sError += ", Receptor";
         if (sError.Length > 0)
            throw new Exception("NotificacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (notificacion.Receptor.UsuarioSocialID == null)
            sError += ", Receptor.UsuarioSocialID";
         if (sError.Length > 0)
            throw new Exception("NotificacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (notificacion.Notificable == null)
            sError += ", Notificable";
         if (sError.Length > 0)
            throw new Exception("NotificacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (notificacion.Notificable.GUID == null)
            sError += ", Notificable.GUID";
         if (sError.Length > 0)
            throw new Exception("NotificacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (notificacion.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (notificacion.TipoNotificacion == null)
            sError += ", TipoNotificacion";
         if (notificacion.EstatusNotificacion == null)
            sError += ", EstatusNotificacion";
         if (sError.Length > 0)
            throw new Exception("NotificacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "NotificacionInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "NotificacionInsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "NotificacionInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO Notificacion (NotificacionID, FechaRegistro, EmisorID, ReceptorID, NotificableID, TipoNotificacion, EstatusNotificacion) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         sCmd.Append(" @notificacion_NotificacionID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "notificacion_NotificacionID";
         if (notificacion.NotificacionID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = notificacion.NotificacionID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@notificacion_FechaRegistro ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "notificacion_FechaRegistro";
         if (notificacion.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = notificacion.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@notificacion_Emisor_UsuarioSocialID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "notificacion_Emisor_UsuarioSocialID";
         if (notificacion.Emisor.UsuarioSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = notificacion.Emisor.UsuarioSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@notificacion_Receptor_UsuarioSocialID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "notificacion_Receptor_UsuarioSocialID";
         if (notificacion.Receptor.UsuarioSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = notificacion.Receptor.UsuarioSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@notificacion_Notificable_GUID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "notificacion_Notificable_GUID";
         if (notificacion.Notificable.GUID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = notificacion.Notificable.GUID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@notificacion_TipoNotificacion ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "notificacion_TipoNotificacion";
         if (notificacion.TipoNotificacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = notificacion.TipoNotificacion;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@notificacion_EstatusNotificacion ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "notificacion_EstatusNotificacion";
         if (notificacion.EstatusNotificacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = notificacion.EstatusNotificacion;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("NotificacionInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("NotificacionInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
