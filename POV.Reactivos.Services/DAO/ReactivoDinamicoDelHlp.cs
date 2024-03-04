using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Modelo.BO;

namespace POV.Reactivos.DAO
{
    /// <summary>
    /// Elimina un registro de Reactivo en la BD
    /// </summary>
    internal class ReactivoDinamicoDelHlp
    {
        /// <summary>
        /// Elimina un registro de ReactivoDinamicoDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="reactivoDinamicoDelHlp">ReactivoDinamicoDelHlp que desea eliminar</param>
        public void Action(IDataContext dctx, Reactivo reactivo)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (reactivo == null)
                sError += ", Reactivo";
            if (sError.Length > 0)
                throw new Exception("ReactivoDinamicoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (reactivo.ReactivoID == null)
                sError += ", ReactivoID";
            if (sError.Length > 0)
                throw new Exception("ReactivoDinamicoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "ReactivoDinamicoDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ReactivoDinamicoDelHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "ReactivoDinamicoDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE ReactivoDinamico ");
            sCmd.Append(" SET Activo = 0 ");
            if (reactivo.ReactivoID == null)
                sCmd.Append(" WHERE  ReactivoID IS NULL ");
            else
            {
                // reactivo.ReactivoID
                sCmd.Append(" WHERE  ReactivoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = reactivo.ReactivoID;
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
                throw new Exception("ReactivoDinamicoDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ReactivoDinamicoDelHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
