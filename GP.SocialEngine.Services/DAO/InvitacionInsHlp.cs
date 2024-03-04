using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO
{
    /// <summary>
    /// Inserta una Invitacion en la Base de Datos
    /// </summary>
    public class InvitacionInsHlp
    {
        /// <summary>
        /// Crea un registro de Inserta Una Invitacion en la base de datos en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="insertaUnaInvitacionenlabasededatos">Inserta Una Invitacion en la base de datos que desea crear</param>
        public void Action(IDataContext dctx, Invitacion invitacion)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (invitacion == null)
                sError += ", Invitacion";
            if (sError.Length > 0)
                throw new Exception("InvitacionInsHlp: Los siguientes Campos no pueden ser vacios: " + sError.Substring(2));
            if (invitacion.Remitente == null)
                sError += ", Remitente";
            if (invitacion.Invitado == null)
                sError += ", Invitado";
            if (sError.Length > 0)
                throw new Exception("Los siguientes campos no pueden ser Vacios " + sError.Substring(2));
            if (invitacion.Remitente.UsuarioSocialID == null)
                sError += ", UsuarioSocialID";
            if (invitacion.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (invitacion.Estatus == null)
                sError += ", Estatus";
            if (invitacion.EsSolicitud == null)
                sError += ", EsSolicitud";
            if (invitacion.InvitadoID == null)
                sError += ", InvitadoID";
            if (sError.Length > 0)
                throw new Exception("Los siguientes campos no pueden ser Vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "InvitacionInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "InvitacionInsHlp: Ocurrio Un error al conectarse a la base de datos", "GP.SocialEngine.DAO",
                   "InvitacionInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Invitacion (InvitacionID,RemitenteID, InvitadoID, EsSolicitud, Estatus, FechaRegistro,Saludo) ");
            sCmd.Append(" VALUES ( ");
            sCmd.Append(" @invitacion_InvitacionID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "invitacion_InvitacionID";
            if (invitacion.InvitacionID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = invitacion.InvitacionID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@invitacion_Remitente_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "invitacion_Remitente_UsuarioSocialID";
            if (invitacion.Remitente.UsuarioSocialID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = invitacion.Remitente.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@invitacion_InvitadoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "invitacion_InvitadoID";
            if (invitacion.InvitadoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = invitacion.InvitadoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@invitacion_EsSolicitud ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "invitacion_EsSolicitud";
            if (invitacion.EsSolicitud == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = invitacion.EsSolicitud;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@invitacion_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "invitacion_Estatus";
            if (invitacion.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = invitacion.Estatus;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@invitacion_FechaRegistro ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "invitacion_FechaRegistro";
            if (invitacion.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = invitacion.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@invitacion_Saludo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "invitacion_Saludo";
            if (invitacion.Saludo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = invitacion.Saludo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ) ");
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
                throw new Exception("InvitacionInsHlp: Ocurrio un error al insertar en la Base de datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("InvitacionInsHlp: Ocurrio un error al insertar en la Base de datos.");
        }
    }
}
