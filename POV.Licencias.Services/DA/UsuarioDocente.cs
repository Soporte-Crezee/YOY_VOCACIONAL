// Licencias de Docente
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;

using POV.CentroEducativo.BO;

namespace POV.Licencias.DA
{
    /// <summary>
    /// LicenciasDocente
    /// </summary>
    internal class UsuarioDocente
    {
        /// <summary>
        /// Consulta registros de LicenciaDocente en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="docente">Director que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de LicenciaDirector generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Docente docente, bool valid = true)
        {
            object myFirm = new object();
            string sError = "";
            if (docente == null)
                sError += ", docente";
            if (sError.Length > 0)
                throw new Exception("UsuarioDocente: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (valid)
            {
                if (docente.DocenteID == null && docente.Curp == null && string.IsNullOrEmpty(docente.Curp.Trim()) && docente.Correo == null && string.IsNullOrEmpty(docente.Correo.Trim()))
                    sError += ", DocenteID, CURP, Correo";
            }
            if (sError.Length > 0)
                throw new Exception("UsuarioDocente: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA", "UsuarioDocente", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UsuarioDocente: Hubo un error al conectarse a la base de datos", "POV.Licencias.DA", "UsuarioDocente", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT DISTINCT UsuarioID");
            sCmd.Append(" FROM Licencia lic ");
            sCmd.Append(" INNER JOIN LicenciaReferencia licRef ON (licRef.LicenciaID = lic.LicenciaID) ");
            sCmd.Append(" INNER JOIN Docente doc ON (doc.DocenteID = licRef.DocenteID) ");
            StringBuilder s_VarWHERE = new StringBuilder();

            if (docente.DocenteID != null)
            {
                s_VarWHERE.Append(" AND doc.DocenteID =@dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = docente.DocenteID;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (docente.Curp != null)
            {
                s_VarWHERE.Append(" AND doc.Curp =@dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = docente.Curp;
                sqlParam.DbType = DbType.String;
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
            sCmd.Append(" ORDER BY UsuarioID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "UsuarioDocente");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UsuarioDocente: Hubo un error al consultar los registros. " + exmsg);
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
