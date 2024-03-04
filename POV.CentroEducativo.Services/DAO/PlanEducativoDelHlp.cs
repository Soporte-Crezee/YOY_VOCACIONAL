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
    /// Elimina un registro de PlanEducativo en la BD
    /// </summary>
    public class PlanEducativoDelHlp
    {
        /// <summary>
        /// Elimina un registro de PlanEducativoDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="planEducativoDelHlp">PlanEducativoDelHlp que desea eliminar</param>
        public void Action(IDataContext dctx, PlanEducativo planEducativo)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (planEducativo == null)
                sError += ", PlanEducativo";
            if (sError.Length > 0)
                throw new Exception("PlanEducativoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (planEducativo.PlanEducativoID == null)
                sError += ", PlanEducativoID";
            if (sError.Length > 0)
                throw new Exception("PlanEducativoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO",
                   "PlanEducativoDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PlanEducativoDelHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO",
                   "PlanEducativoDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE PlanEducativo ");
            sCmd.Append(" SET Estatus=0 ");
            if (planEducativo.PlanEducativoID == null)
                sCmd.Append(" WHERE PlanEducativoID IS NULL ");
            else
            {
                sCmd.Append(" WHERE PlanEducativoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = planEducativo.PlanEducativoID;
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
                    throw new Exception("PlanEducativoDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
                }
                finally
                {
                    try { dctx.CloseConnection(myFirm); }
                    catch (Exception) { }
                }
                if (iRes < 1)
                    throw new Exception("PlanEducativoDelHlp: Ocurrió un error al ingresar el registro.");
            }
        }
    }
}
