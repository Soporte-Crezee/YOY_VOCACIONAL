using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;

namespace POV.Modelo.DAO
{
    /// <summary>
    /// Elimina un registro de Modelo Dinámico en la BD
    /// </summary>
    internal class ClasificadorDelHlp
    {
        /// <summary>
        /// Elimina un registro de ModeloPrueba en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="modelo">ModeloDinamico que desea eliminar</param>
        public void Action(IDataContext dctx, Clasificador clasificadorModelo)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (clasificadorModelo == null)
                sError += ", Clasificador";
            if (sError.Length > 0)
                throw new Exception("ClasificadorDelHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (clasificadorModelo.ClasificadorID == null)
                sError += ", ClasificadorID";
            if (sError.Length > 0)
                throw new Exception("ClasificadorDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Modelo.DAO",
                   "ClasificadorDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ClasificadorDelHlp: No se pudo conectar a la base de datos", "POV.Modelo.DAO",
                   "ClasificadorDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Clasificador ");
            sCmd.Append(" SET Activo = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = 0;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);

            if (clasificadorModelo.ClasificadorID == null)
                sCmd.Append(" WHERE ClasificadorID IS NULL ");
            else
            {
                sCmd.Append(" WHERE ClasificadorID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = clasificadorModelo.ClasificadorID;
                sqlParam.DbType = DbType.Int64;
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
                throw new Exception("ClasificadorDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ClasificadorDelHlp: Ocurrió un error al ingresar el registro.");
        }

    }
}
