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
    /// Consulta Invitaciones de la Base de datos
    /// </summary>
    public class InvitacionRetHlp
    {
        /// <summary>
        /// Consulta registros de Consulta Invitaciones de la Base de datos en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="consultaInvitacionesdelaBasededatos">Consulta Invitaciones de la Base de datos que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de Consulta Invitaciones de la Base de datos generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Invitacion invitacion)
        {
            object myFirm = new object();
            if (invitacion.Remitente == null)
            {
                invitacion.Remitente = new UsuarioSocial();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "InvitacionRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "InvitacionRetHlp: Ocurrio Un error al conectarse a la Base de Datos", "GP.SocialEngine.DAO",
                   "InvitacionRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT InvitacionID, RemitenteID, Estatus, FechaRegistro, InvitadoID, EsSolicitud, Saludo ");
            sCmd.Append(" FROM Invitacion ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (invitacion.InvitacionID != null)
            {
                s_VarWHERE.Append(" InvitacionID = @invitacion_InvitacionID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "invitacion_InvitacionID";
                sqlParam.Value = invitacion.InvitacionID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (invitacion.Remitente.UsuarioSocialID != null)
            {
                s_VarWHERE.Append(" AND RemitenteID = @invitacion_Remitente_UsuarioSocialID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "invitacion_Remitente_UsuarioSocialID";
                sqlParam.Value = invitacion.Remitente.UsuarioSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (invitacion.Estatus != null)
            {
                s_VarWHERE.Append(" AND Estatus = @invitacion_Estatus ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "invitacion_Estatus";
                sqlParam.Value = invitacion.Estatus;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (invitacion.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @invitacion_FechaRegistro ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "invitacion_FechaRegistro";
                sqlParam.Value = invitacion.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (invitacion.InvitadoID != null)
            {
                s_VarWHERE.Append(" AND InvitadoID = @invitacion_InvitadoID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "invitacion_InvitadoID";
                sqlParam.Value = invitacion.InvitadoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (invitacion.EsSolicitud != null)
            {
                s_VarWHERE.Append(" AND EsSolicitud = @invitacion_EsSolicitud ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "invitacion_EsSolicitud";
                sqlParam.Value = invitacion.EsSolicitud;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (invitacion.Saludo != null)
            {
                s_VarWHERE.Append(" AND Saludo = @invitacion_Saludo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "invitacion_Saludo";
                sqlParam.Value = invitacion.Saludo;
                sqlParam.DbType = DbType.String;
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
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Invitacion");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("InvitacionRetHlp: Ocurrio un Error al Consultar los registros en la Base de Datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }
    }
}
