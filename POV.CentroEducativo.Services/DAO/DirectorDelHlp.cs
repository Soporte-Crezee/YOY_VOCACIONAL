using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// Desactiva un registro de director de la BD
    /// </summary>
    public class DirectorDelHlp
    {
        /// <summary>
        /// Desactiva un registro de director de la BD
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="director">director que desea desactivar</param>
        public void Action(IDataContext dctx, Director director)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (director == null)
                sError += ", Director";
            if (sError.Length > 0)
                throw new Exception("DirectorDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (director.DirectorID == null)
                sError += ", DirectorID";
            if (sError.Length > 0)
                throw new Exception("DirectorDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO",
                   "DirectorDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "DirectorDelHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO",
                   "DirectorDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Director ");
            sCmd.Append(" SET Estatus=0 ");
            if (director.DirectorID == null)
                sCmd.Append(" WHERE DirectorID IS NULL ");
            else
            {
                sCmd.Append(" WHERE DirectorID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = director.DirectorID;
                sqlParam.DbType = DbType.Int32;
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
                    throw new Exception("DirectorDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
                }
                finally
                {
                    try { dctx.CloseConnection(myFirm); }
                    catch (Exception) { }
                }
                if (iRes < 1)
                    throw new Exception("DirectorDelHlp: Ocurrió un error al ingresar el registro.");
            }
        }
    }
}
