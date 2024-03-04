using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO
{
    /// <summary>
    /// Modifica el estado de las notificaciones recibidas por un usuario
    /// </summary>
    public  class EstadoNotificacionesUpdHlp
    {

        /// <summary>
        ///  Actualiza el estado de las notificaciones recibidas por un usuario proporcionado
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="nuevo"></param>
        /// <param name="previo"></param>
        /// <param name="usuarioReceptor"></param>
        public void Action(IDataContext dctx,EEstatusNotificacion nuevo,EEstatusNotificacion previo,UsuarioSocial usuarioReceptor)
        {
            object myFirm = new object();
            String sError = string.Empty;
            int estado = 0;
            if (usuarioReceptor == null)
                sError += ", usuarioReceptor";
            if (usuarioReceptor.UsuarioSocialID == null)
                sError += ", UsuarioSocialID";

            if (sError.Length > 0)
                throw new Exception("EstadoNotificacionesUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));


            if((int)nuevo <=0 && (int)nuevo > 3)
                sError += ", Nuevo Estado de Notificacion ";

            if ((int)previo <= 0 && (int)nuevo > 3)
                sError += ", Previo Estado de Notificacion ";

            if (sError.Length > 0)
                throw new Exception("EstadoNotificacionesUpdHlp: Formato incorrecto  " + sError.Substring(2));
           
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "EstadoNotificacionesUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EstadoNotificacionesUpdHlp: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO",
                   "NotificacionUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Notificacion ");
            sCmd.Append(" SET EstatusNotificacion = @notificacion_EstatusNotificacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "notificacion_EstatusNotificacion";
            sqlParam.Value = nuevo;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);          

            sCmd.Append(" WHERE ReceptorID = @actual_ReceptorID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "actual_ReceptorID";
            sqlParam.Value = usuarioReceptor.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" AND EstatusNotificacion = @previo_EstatusNotificacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "previo_EstatusNotificacion";
            sqlParam.Value = (int) previo;
            sqlParam.DbType= DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
        
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
                throw new Exception("EstadoNotificacionesUpdHlp: Hubo  un Error al Actualizar el estado de los Registros . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("EstadoNotificacionesUpdHlp: Hubo  un Error al Actualizar el estado de los Registros .");
        }
    }
}
