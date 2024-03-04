using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO
{
    /// <summary>
    /// Elimina un registro de Notificacion en la BD
    /// </summary>
    public class NotificacionDelHlp
    {
        /// <summary>
        /// Elimina un registro de NotificacionDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="notificacionDelHlp">NotificacionDelHlp que desea eliminar</param>
        public void Action(IDataContext dctx, Notificacion notificacion)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (notificacion == null)
                sError += ", Notificacion";
            if (sError.Length > 0)
                throw new Exception("NotificacionDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (notificacion.Emisor == null)
            {
                notificacion.Emisor = new UsuarioSocial();
            }
            if (notificacion.Receptor == null)
            {
                notificacion.Receptor = new UsuarioSocial();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "NotificacionDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "NotificacionDelHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO",
                   "NotificacionDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" DELETE FROM Notificacion ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (notificacion.NotificacionID != null)
            {
                s_VarWHERE.Append(" NotificacionID = @notificacion_NotificacionID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "notificacion_NotificacionID";
                sqlParam.Value = notificacion.NotificacionID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (notificacion.Emisor.UsuarioSocialID != null)
            {
                s_VarWHERE.Append(" AND EmisorID = @notificacion_Emisor_UsuarioSocialID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "notificacion_Emisor_UsuarioSocialID";
                sqlParam.Value = notificacion.Emisor.UsuarioSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (notificacion.Receptor.UsuarioSocialID != null)
            {
                s_VarWHERE.Append(" AND ReceptorID = @notificacion_Receptor_UsuarioSocialID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "notificacion_Receptor_UsuarioSocialID";
                sqlParam.Value = notificacion.Receptor.UsuarioSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (notificacion.TipoNotificacion != null)
            {
                s_VarWHERE.Append(" AND TipoNotificacion = @notificacion_TipoNotificacion ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "notificacion_TipoNotificacion";
                sqlParam.Value = notificacion.TipoNotificacion;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (notificacion.EstatusNotificacion != null)
            {
                s_VarWHERE.Append(" AND EstatusNotificacion = @notificacion_EstatusNotificacion ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "notificacion_EstatusNotificacion";
                sqlParam.Value = notificacion.EstatusNotificacion;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            string s_VarWHEREres = s_VarWHERE.ToString().Trim();
            if (s_VarWHEREres.Length > 0)
            {
                if (s_VarWHEREres.StartsWith("AND "))
                    s_VarWHEREres = s_VarWHEREres.Substring(4);
                else if (s_VarWHEREres.StartsWith("OR "))
                    s_VarWHEREres = s_VarWHEREres.Substring(3);
                else if (s_VarWHEREres.StartsWith(","))
                    s_VarWHEREres = s_VarWHEREres.Substring(1);
                sCmd.Append(" WHERE " + s_VarWHEREres);
            }
            int iRes = 0;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                iRes = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("NotificacionDelHlp: OcurriÃ³ un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("NotificacionDelHlp: OcurriÃ³ un error al ingresar el registro.");
        }
    }
}