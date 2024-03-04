using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.Profesionalizacion.BO;

namespace POV.Licencias.DA
{
    /// <summary>
    /// Elimina un registro de ProfesionalizacionContrato en la BD
    /// </summary>
    internal class ProfesionalizacionContratoDADelHlp
    {
        /// <summary>
        /// Elimina un registro de ProfesionalizacionContratoDADelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="profesionalizacionContratoDADelHlp">ProfesionalizacionContratoDADelHlp que desea eliminar</param>
        public void Action(IDataContext dctx, Contrato contrato, EjeTematico ejeTematico)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (contrato == null)
                sError += ", Contrato";
            if (sError.Length > 0)
                throw new Exception("ProfesionalizacionContratoDADelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (contrato.ContratoID == null)
                sError += ", Contrato";
            if (sError.Length > 0)
                throw new Exception("ProfesionalizacionContratoDADelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (ejeTematico == null)
                sError += ", EjeTematico";
            if (sError.Length > 0)
                throw new Exception("ProfesionalizacionContratoDADelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA",
                   "ProfesionalizacionContratoDADelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ProfesionalizacionContratoDADelHlp: No se pudo conectar a la base de datos", "POV.Licencias.DA",
                   "ProfesionalizacionContratoDADelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" DELETE FROM EjeContrato ");
            if (contrato.ContratoID == null)
                sCmd.Append(" WHERE ContratoID IS NULL ");
            else
            {
                // contrato.ContratoID
                sCmd.Append(" WHERE ContratoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = contrato.ContratoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (ejeTematico.EjeTematicoID != null)
            {
                sCmd.Append(" AND EjeTematicoID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = ejeTematico.EjeTematicoID;
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
                throw new Exception("ProfesionalizacionContratoDADelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ProfesionalizacionContratoDADelHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
