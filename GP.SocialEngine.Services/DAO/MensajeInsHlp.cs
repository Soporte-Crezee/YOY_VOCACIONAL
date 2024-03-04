using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Inserta objeto Mensaje en la base de datos
   /// </summary>
   public class MensajeInsHlp { 
      /// <summary>
      /// Crea un registro de Mensaje en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="mensaje">Mensaje que desea crear</param>
      public void Action(IDataContext dctx, Mensaje mensaje){
         object myFirm = new object();
         string sError = String.Empty;
         if (mensaje == null)
            sError += ", Mensaje";
         if (sError.Length > 0)
            throw new Exception("MensajeInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (mensaje.MensajeID == null)
            sError += ", MensajeID";
         if (mensaje.Asunto == null || mensaje.Asunto.Trim().Length == 0)
            sError += ", Asunto";
         if (mensaje.Contenido == null || mensaje.Contenido.Trim().Length == 0)
            sError += ", Contenido";
         if (mensaje.FechaMensaje == null)
            sError += ", FechaMensaje";
         if (mensaje.Remitente == null)
            sError += ", mensaje.Remitente";
         if (mensaje.Estatus == null)
            sError += ", Estado";
         if (sError.Length > 0)
            throw new Exception("MensajeInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (mensaje.Remitente.UsuarioSocialID == null)
            sError += ", RemitenteID";
         if (sError.Length > 0)
            throw new Exception("MensajeInsHlp:Los siguientes campos no pueden ser vacios" + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "MensajeInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "InsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "MensajeInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO MENSAJE (MENSAJEID,ASUNTO,CONTENIDO,FECHAMENSAJE,ESTATUS,GUIDCONVERSACION,REMITENTEID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // mensaje.MensajeID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (mensaje.MensajeID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = mensaje.MensajeID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         // mensaje.Asunto
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (mensaje.Asunto == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = mensaje.Asunto;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // mensaje.Contenido
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (mensaje.Contenido == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = mensaje.Contenido;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // mensaje.FechaMensaje
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (mensaje.FechaMensaje == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = mensaje.FechaMensaje;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // mensaje.Estatus
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (mensaje.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = mensaje.Estatus;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // mensaje.GuidConversacion
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (mensaje.GuidConversacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = mensaje.GuidConversacion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // mensaje.Remitente.UsuarioSocialID
         sCmd.Append(" ,@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (mensaje.Remitente.UsuarioSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = mensaje.Remitente.UsuarioSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("MensajeInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("MensajeInsHlp: Ocurrio un error al ingresar el registro.");
      }
      /// <summary>
      /// Crea un registro de Mensaje en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="mensaje">Mensaje que desea crear</param>
      public void Action(IDataContext dctx, Mensaje mensaje,UsuarioSocial destinatario,bool? activo){
         object myFirm = new object();
         string sError = String.Empty;
         if (mensaje == null)
            sError += ", Mensaje";
         if (destinatario == null)
            sError += ", Mensaje";
         if (sError.Length > 0)
            throw new Exception("MensajeInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (activo == null) {
         activo = true;
      }
         if (mensaje.GuidConversacion == null || mensaje.GuidConversacion.Trim().Length == 0)
            sError += ", GuidConversacion";
         if (destinatario.UsuarioSocialID == null)
            sError += ", DestinatarioID";

          if (sError.Length > 0)
            throw new Exception("MensajeInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "MensajeInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "InsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "MensajeInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO MENSAJEUSUARIOSSOCIALES (MENSAJEID,USUARIOSOCIALID,ACTIVO) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // mensaje.GuidConversacion
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (mensaje.GuidConversacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = mensaje.GuidConversacion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // destinatario.UsuarioSocialID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (destinatario.UsuarioSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = destinatario.UsuarioSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // activo
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("MensajeInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("MensajeInsHlp: Ocurrio un error al ingresar el registro.");
      }
   } 
}
