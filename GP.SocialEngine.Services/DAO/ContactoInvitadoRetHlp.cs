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
    /// ContactoInvitadoRetHlp
    /// </summary>
    public class ContactoInvitadoRetHlp
    {
        /// <summary>
        /// Consulta registros de ContactoInvitadoRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="contactoInvitadoRetHlp">ContactoInvitadoRetHlp que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de ContactoInvitadoRetHlp generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, ContactoInvitado contactoInvitado)
        {
            object myFirm = new object();
            string sError = string.Empty;
            if (contactoInvitado == null)
                sError += ", ContactoInvitado";
            if (sError.Length > 0)
                throw new Exception("ContactoInvitadoRetHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "ContactoInvitadoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ContactoInvitadoRetHlp: Ocurrio un error al conectarse a la base de datos", "GP.SocialEngine.DAO",
                   "ContactoInvitadoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ContactoInvitadoID, NombreCompleto, CorreoElectronico ");
            sCmd.Append(" FROM ContactoInvitado ");
            sCmd.Append(" WHERE ");
            StringBuilder s_Var = new StringBuilder();
            if (contactoInvitado.ContactoInvitadoID != null)
            {
                s_Var.Append(" ContactoInvitadoID = @contactoInvitado_ContactoInvitadoID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "contactoInvitado_ContactoInvitadoID";
                sqlParam.Value = contactoInvitado.ContactoInvitadoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contactoInvitado.NombreCompleto != null)
            {
                s_Var.Append(" AND NombreCompleto = @contactoInvitado_NombreCompleto ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "contactoInvitado_NombreCompleto";
                sqlParam.Value = contactoInvitado.NombreCompleto;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contactoInvitado.CorreoElectronico != null)
            {
                s_Var.Append(" AND CorreoElectronico = @contactoInvitado_CorreoElectronico ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "contactoInvitado_CorreoElectronico ";
                sqlParam.Value = contactoInvitado.CorreoElectronico;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contactoInvitado.Mensaje != null)
            {
                s_Var.Append(" AND Mensaje = @contactoInvitado_Mensaje ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "contactoInvitado_Mensaje ";
                sqlParam.Value = contactoInvitado.Mensaje;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            s_Var.Append("  ");
            string s_Varres = s_Var.ToString().Trim();
            if (s_Varres.Length > 0)
            {
                if (s_Varres.StartsWith("AND "))
                    s_Varres = s_Varres.Substring(4);
                else if (s_Varres.StartsWith("OR "))
                    s_Varres = s_Varres.Substring(3);
                else if (s_Varres.StartsWith(","))
                    s_Varres = s_Varres.Substring(1);
                sCmd.Append("  " + s_Varres);
            }
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "ContactoInvitado");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ContactoInvitado: Ocurrio un Error al Consultar los Registros en la Base de Datos. " + exmsg);
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
