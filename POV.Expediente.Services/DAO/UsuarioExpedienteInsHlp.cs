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
    /// Guarda un registro de UsuarioExpediente en la BD
    /// </summary>
    internal class UsuarioExpedienteInsHlp
    {
        /// <summary>
        /// Crea un registro UsuarioExpediente en la base de datos
        /// </summary>
        /// <param name="dctx"> El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioExpediente"> UsuarioExpediente que desea crear</param>
        public void Action(IDataContext dctx, UsuarioExpediente usuarioExpediente) 
        {
            object myFirm = new object();
            string sError = String.Empty;

            if (usuarioExpediente == null)
                sError += " ,UsuarioExpediente";
            if (sError.Length > 0)
                throw new Exception("UsuarioExpedienteInsHlp: Los siguientes campos no pueden ser vacio " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Expediente.DAO", "UsuarioExpedienteInsHlp", "Action", null, null);

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UsuarioExpedienteInsHlp: No se pudo conectar a la base de datos", "POV.Expediente.DAO", "UsuarioExpedienteInsHlp", "Action", null, null);
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append("INSERT INTO UsuarioExpediente (UsuarioID, AlumnoID) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");

            //usuarioExpediente.UsuarioID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (usuarioExpediente.UsuarioID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuarioExpediente.UsuarioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            //usuarioExpediente.AlumnoID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (usuarioExpediente.AlumnoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuarioExpediente.AlumnoID;
            sqlParam.DbType = DbType.Int64;
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
                throw new Exception("UsuarioExpedienteInsHlp: Ocurrió un erro al ingresar el registro. " + exmsg);
            }
            finally 
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("UsuarioExpedienteInsHlp: Ocurrió un erro al ingresar el registro.");
        }
    }
}
