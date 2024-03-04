using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Actualiza los Mensajes en la base de datos
   /// </summary>
   public class MensajeUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de Mensaje en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="mensaje">Mensaje que tiene los datos nuevos</param>
      /// <param name="anterior">Mensaje que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Mensaje mensaje,Mensaje anterior){
         object myFirm = new object();
         string sError = String.Empty;
         if (mensaje == null)
            sError += ", Mensaje";
         if (anterior == null)
            sError += ", anterior";
         if (sError.Length > 0)
            throw new Exception("MensajeUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (mensaje.Remitente == null)
            sError += ", Remitente";
         if (anterior.Remitente == null)
            sError += ", Anterior Remitente";
         if (mensaje.MensajeID == null)
            sError += ", MensajeID";
         if (mensaje.Estatus == null)
            sError += ", Estatus";
         if (anterior.MensajeID == null)
            sError += ", Anterior MensajeID";
         if (anterior.Estatus == null)
            sError += ", Anterior Estatus";
         if (sError.Length > 0)
            throw new Exception("MensajeUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (mensaje.MensajeID != anterior.MensajeID) {
         sError = "Los parametros no coinciden";
      }
         if (sError.Length > 0)
            throw new Exception("MensajeUpdHlp: " + sError.Substring(2));
         if (mensaje.Remitente.UsuarioSocialID == null)
            sError += ", Remitente ID";
         if (anterior.Remitente.UsuarioSocialID == null)
            sError += ", Anterior Remitente ID";
         if (sError.Length > 0)
            throw new Exception("MensajeUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (mensaje.Remitente.UsuarioSocialID != anterior.Remitente.UsuarioSocialID) {
         sError = "Los parametros del Remitente no coinciden";
      }
         if (sError.Length > 0)
            throw new Exception("MensajeUpdHlp: " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "MensajeUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "MensajeUpdHlp: Ocurrió un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "MensajeUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE MENSAJE ");
         sCmd.Append(" SET ");
         // mensaje.Estatus
         sCmd.Append(" Estatus =@dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (mensaje.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = mensaje.Estatus;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         if (anterior.MensajeID == null)
            sCmd.Append(" WHERE MENSAJEID IS NULL ");
         else{ 
            // anterior.MensajeID
            sCmd.Append(" WHERE MENSAJEID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = anterior.MensajeID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.GuidConversacion == null)
            sCmd.Append(" AND GUIDCONVERSACION IS NULL ");
         else{ 
            // anterior.GuidConversacion
            sCmd.Append(" AND GUIDCONVERSACION = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = anterior.GuidConversacion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         // anterior.Asunto
         sCmd.Append(" AND ASUNTO =@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (anterior.Asunto == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.Asunto;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.FechaMensaje
         sCmd.Append(" AND FECHAMENSAJE =@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (anterior.FechaMensaje == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.FechaMensaje;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.Contenido
         sCmd.Append(" AND CONTENIDO =@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (anterior.Contenido == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.Contenido;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         if (anterior.Estatus == null)
            sCmd.Append(" AND ESTATUS IS NULL ");
         else{ 
            // anterior.Estatus
            sCmd.Append(" AND ESTATUS = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = anterior.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Remitente.UsuarioSocialID == null)
            sCmd.Append(" AND REMITENTEID IS NULL ");
         else{ 
            // anterior.Remitente.UsuarioSocialID
            sCmd.Append(" AND REMITENTEID = @dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            sqlParam.Value = anterior.Remitente.UsuarioSocialID;
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
            throw new Exception("MensajeUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("MensajeUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado.");
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de Mensaje en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="mensaje">Mensaje que tiene los datos nuevos</param>
      /// <param name="anterior">Mensaje que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Mensaje mensaje,UsuarioSocial destinatario,bool? activo){
         object myFirm = new object();
         string sError = String.Empty;
         if (mensaje == null)
            sError += ", Mensaje";
         if (destinatario == null)
            sError += ", Destinatario";
         if (sError.Length > 0)
            throw new Exception("MensajeUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (destinatario.UsuarioSocialID == null)
            sError += ", Destinatario ID";
         if (mensaje.MensajeID == null)
            sError += ", MensajeID";
         if (sError.Length > 0)
            throw new Exception("MensajeUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "MensajeUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "MensajeUpdHlp: Ocurrió un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "MensajeUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE MENSAJEUSUARIOSSOCIALES ");
         sCmd.Append(" SET ");
         // activo
         sCmd.Append(" ACTIVO = @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         if (mensaje.MensajeID == null)
            sCmd.Append(" WHERE MENSAJEID IS NULL ");
         else{ 
            // mensaje.MensajeID
            sCmd.Append(" WHERE MENSAJEID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = mensaje.MensajeID.ToString();
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (destinatario.UsuarioSocialID == null)
            sCmd.Append(" AND USUARIOSOCIALID IS NULL ");
         else{ 
            // destinatario.UsuarioSocialID
            sCmd.Append(" AND USUARIOSOCIALID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = destinatario.UsuarioSocialID;
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
            throw new Exception("MensajeUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("MensajeUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado.");
      }
   } 
}
