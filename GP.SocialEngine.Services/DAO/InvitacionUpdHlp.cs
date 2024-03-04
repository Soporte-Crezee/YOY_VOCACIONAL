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
    /// Actualiza una Invitacion en la Base de Datos
    /// </summary>
    public class InvitacionUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de Actualizar Una invitacion en la base de datos en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="actualizarUnainvitacionenlabasededatos">Actualizar Una invitacion en la base de datos que tiene los datos nuevos</param>
        /// <param name="anterior">Actualizar Una invitacion en la base de datos que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, Invitacion invitacion, Invitacion anterior)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (invitacion == null)
                sError += ", Invitacion";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("InvitacionUpdHlp: Los Siguientes Campos No pueden ser vacios : " + sError.Substring(2));
            if (anterior.InvitacionID == null)
                sError += ", Anterior.InvitacionID";
            if (invitacion.Estatus == null)
                sError += ", Estatus Invitacion";
            if (sError.Length > 0)
                throw new Exception("InvitacionUpdHlp: Los Siguientes Campos No pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "InvitacionUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "InvitacionUpdHlp: Ocurrio Un error al Conectarse a la base de datos", "GP.SocialEngine.DAO",
                   "InvitacionUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Invitacion ");
            if (invitacion.Estatus == null)
                sCmd.Append(" SET Estatus = NULL ");
            else
            {
                sCmd.Append(" SET Estatus = @invitacion_Estatus ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "invitacion_Estatus";
                sqlParam.Value = invitacion.Estatus;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.InvitacionID == null)
                sCmd.Append(" WHERE InvitacionID IS NULL ");
            else
            {
                sCmd.Append(" WHERE InvitacionID = @anterior_InvitacionID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "anterior_InvitacionID";
                sqlParam.Value = anterior.InvitacionID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
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
                throw new Exception("InvitacionUpdHlp: Ocurrio un Error al Actualizar la invitacion. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("InvitacionUpdHlp: Ocurrio un Error al Actualizar la invitacion.");
        }
    }
}
