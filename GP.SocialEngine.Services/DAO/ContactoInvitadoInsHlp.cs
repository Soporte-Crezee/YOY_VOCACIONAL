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
    /// ContactoInvitadoInsHlp
    /// </summary>
    public class ContactoInvitadoInsHlp
    {
        /// <summary>
        /// Crea un registro de ContactoInvitadoInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="contactoInvitadoInsHlp">ContactoInvitadoInsHlp que desea crear</param>
        public void Action(IDataContext dctx, ContactoInvitado contactoInvitado)
        {
            object myFirm = new object();
            string sError = string.Empty;
            if (contactoInvitado == null)
                sError += ", ContactoInvitado";
            if (sError.Length > 0)
                throw new Exception("ContactoInvitadoInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
            if (contactoInvitado.NombreCompleto == null || contactoInvitado.NombreCompleto.Trim().Length == 0)
                sError += ", NombreCompleto";
            if (contactoInvitado.CorreoElectronico == null || contactoInvitado.CorreoElectronico.Trim().Length == 0)
                sError += ", CorreoElectronico";
            if (contactoInvitado.Mensaje == null || contactoInvitado.Mensaje.Trim().Length == 0)
                sError += ", Mensaje";
            if (sError.Length > 0)
                throw new Exception("ContactoInvitadoInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "ContactoInvitadoInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ContactoInvitadoInsHlp: Ocurrio un Error al Conectarse a la Base de Datos", "GP.SocialEngine.DAO",
                   "ContactoInvitadoInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO ContactoInvitado (NombreCompleto, CorreoElectronico,Mensaje) ");
            sCmd.Append(" VALUES ( ");
            sCmd.Append(" @contactoInvitado_NombreCompleto ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "contactoInvitado_NombreCompleto";
            if (contactoInvitado.NombreCompleto == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contactoInvitado.NombreCompleto;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@contactoInvitado_CorreoElectronico ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "contactoInvitado_CorreoElectronico";
            if (contactoInvitado.CorreoElectronico == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contactoInvitado.CorreoElectronico;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@contactoInvitado_Mensaje ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "contactoInvitado_Mensaje";
            if (contactoInvitado.Mensaje == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contactoInvitado.Mensaje;
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
                throw new Exception("ContactoInvitadoInsHlp: Ocurrio Un error al Insertar El Registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ContactoInvitadoInsHlp: Ocurrio Un error al Insertar El Registro.");
        }
    }
}
