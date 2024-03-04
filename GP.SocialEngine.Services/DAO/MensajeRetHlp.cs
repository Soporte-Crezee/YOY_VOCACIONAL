using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Consultar los Mensajes de la base de datos
   /// </summary>
   public class MensajeRetHlp { 
      /// <summary>
      /// Consulta registros de Mensaje en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="mensaje">Mensaje que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Mensaje generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Mensaje mensaje){
         object myFirm = new object();
         string sError = "";
         if (mensaje == null)
            sError += ", Mensaje";
         if (sError.Length > 0)
            throw new Exception("MensajeRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
      if (mensaje.Remitente == null) {
         mensaje.Remitente = new UsuarioSocial();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "MensajeRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "MensajeRetHlp: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "MensajeRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT MENSAJEID,GUIDCONVERSACION,CONTENIDO,REMITENTEID,ESTATUS,FECHAMENSAJE,ASUNTO ");
         sCmd.Append(" FROM MENSAJE ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (mensaje.MensajeID != null){
            s_VarWHERE.Append(" MENSAJEID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = mensaje.MensajeID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (mensaje.GuidConversacion != null){
            s_VarWHERE.Append(" AND GUIDCONVERSACION = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = mensaje.GuidConversacion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (mensaje.Contenido != null){
            s_VarWHERE.Append(" AND CONTENIDO = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = mensaje.Contenido;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (mensaje.FechaMensaje != null){
            s_VarWHERE.Append(" AND FECHAMENSAJE = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = mensaje.FechaMensaje;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (mensaje.Remitente.UsuarioSocialID != null){
            s_VarWHERE.Append(" AND REMITENTEID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = mensaje.Remitente.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (mensaje.Estatus != null){
            s_VarWHERE.Append(" AND ESTATUS = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = mensaje.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (mensaje.Asunto != null){
            s_VarWHERE.Append(" AND ASUNTO = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = mensaje.Asunto;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            else if (s_VarWHEREres.StartsWith(","))
               s_VarWHEREres = s_VarWHEREres.Substring(1);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         sCmd.Append(" ORDER BY FECHAMENSAJE ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "Mensaje");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("MensajeRetHlp: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
      /// <summary>
      /// Consulta registros de Mensaje en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="mensaje">Mensaje que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Mensaje generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Mensaje mensaje,UsuarioSocial destinatario,bool? activo){
         object myFirm = new object();
         string sError = "";
         if (mensaje == null)
            sError += ", Mensaje";
         if (sError.Length > 0)
            throw new Exception("MensajeRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
      if (mensaje.Remitente == null) {
         mensaje.Remitente = new UsuarioSocial();
      }
      if (destinatario == null) {
         destinatario = new UsuarioSocial();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "MensajeRetHlp", "ActionRetriveDestinatarios", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "MensajeRetHlp: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "MensajeRetHlp", "ActionRetriveDestinatarios", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT MENSAJEUSUARIOSSOCIALES.MENSAJEID, MENSAJEUSUARIOSSOCIALES.USUARIOSOCIALID,MENSAJEUSUARIOSSOCIALES.ACTIVO ");
         sCmd.Append(" FROM MENSAJEUSUARIOSSOCIALES ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (mensaje.MensajeID != null){
            s_VarWHERE.Append(" MENSAJEUSUARIOSSOCIALES.MENSAJEID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = mensaje.MensajeID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (destinatario.UsuarioSocialID != null){
            s_VarWHERE.Append(" AND MENSAJEUSUARIOSSOCIALES.USUARIOSOCIALID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = destinatario.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (activo != null){
            s_VarWHERE.Append(" AND MENSAJEUSUARIOSSOCIALES.ACTIVO =@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            else if (s_VarWHEREres.StartsWith(","))
               s_VarWHEREres = s_VarWHEREres.Substring(1);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         sCmd.Append(" ORDER BY MENSAJEID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "Destinatarios");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("MensajeRetHlp: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
