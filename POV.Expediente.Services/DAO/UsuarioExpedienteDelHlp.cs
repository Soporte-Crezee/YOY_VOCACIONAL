using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Expediente.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace POV.Expediente.DAO
{
    /// <summary>
    /// Elimina un registro UsuarioExpediente en la BD
    /// </summary>
    internal class UsuarioExpedienteDelHlp
    {
        /// <summary>
        /// Elimina un registro de UsuarioExpediente en la base de datos.
        /// </summary>
        /// <param name="dctx"> El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioExpediente"> UsuarioExpediente que desea eliminar</param>
        public void Action(IDataContext dctx, UsuarioExpediente usuarioExpediente) 
        {
            object myFirm = new object();
            string sError = String.Empty;

            if (usuarioExpediente == null)
                sError += ", UsuarioExpediente";
            if (sError.Length > 0)
                throw new Exception("UsuarioExpedienteDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));                        
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Expediente.DAO", "UsuarioExpedienteDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UsuarioExpedienteDelHlp: No se pudo conectar a la base de datos", "POV.Expediente.DAO", "UsuarioExpedienteDelHlp", "Action", null, null);
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            StringBuilder s_VarWHERE = new StringBuilder();
            sCmd.Append(" DELETE FROM UsuarioExpediente ");
            if (usuarioExpediente.UsuarioID == null)
                s_VarWHERE.Append(" UsuarioID IS NULL ");
            else
            {
                s_VarWHERE.Append(" UsuarioID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = usuarioExpediente.UsuarioID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioExpediente.AlumnoID == null)
                s_VarWHERE.Append(" AND AlumnoID IS NULL ");
            else 
            {
                s_VarWHERE.Append(" AND AlumnoID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = usuarioExpediente.AlumnoID;
                sqlParam.DbType = DbType.Int64;
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
                throw new Exception("UsuarioExpedienteDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("UsuarioExpedienteDelHlp: Ocurrió un error al ingresar el registro.");
      
        }
    }
}
