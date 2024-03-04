using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;


namespace POV.Licencias.DA
{
    /// <summary>
    /// LicenciaAlumnoRetHlp
    /// </summary>
    internal class LicenciaDARetHlp
    {
        /// <summary>
        /// Consulta registros de LicenciaAlumno en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaId">LicenciaID  que se desea consultar </param>
        /// <param name="parameters">Criterios de consulta </param>
        /// <returns>El DataSet que contiene la información de Licencia generada por la consulta</returns>
        public DataSet Action(IDataContext dctx,Guid licenciaId,Dictionary<string,string> parameters=null ) {
            object myFirm = new object();
            string sError = "";
            
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", "LicenciaDARetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaAlumnoRetHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", "LicenciaAlumnoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT lic.LicenciaID, lic.LicenciaEscuelaID, lic.UsuarioID, lic.UsuarioSocialID, licRef.AlumnoID, lic.Tipo, lic.Activo ");
            sCmd.Append(" FROM Licencia lic ");
            sCmd.Append(" INNER JOIN LicenciaReferencia licRef ON (licRef.LicenciaID = lic.LicenciaID) ");
            sCmd.Append(" INNER JOIN LicenciaEscuela licEsc ON (licEsc.LicenciaEscuelaID = lic.LicenciaEscuelaID) ");
            sCmd.Append(" INNER JOIN CicloEscolar ciclo ON (ciclo.CicloEscolarID = licEsc.CicloEscolarID) ");
            sCmd.Append(" INNER JOIN Contrato contrato ON (contrato.ContratoID = licEsc.ContratoID) ");
            StringBuilder s_VarWHERE = new StringBuilder();

            s_VarWHERE.Append(" lic.LicenciaID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = licenciaId;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" AND contrato.InicioContrato <= @dbp4ram3 AND @dbp4ram3 <= contrato.FinContrato ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = DateTime.Now;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" AND ciclo.InicioCiclo <= @dbp4ram2 AND @dbp4ram2 <= ciclo.FinCiclo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = DateTime.Now;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" AND lic.Activo = 1 ");
            sCmd.Append(" AND licEsc.Activo = 1 ");
            sCmd.Append(" AND contrato.Estatus = 1 ");

            if(parameters!=null)
            {
                if(parameters.ContainsKey("LicenciaEscuelaID"))
                {
                    s_VarWHERE.Append(" AND lic.LicenciaEscuelaID =@dbp4ram2 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram2";
                    sqlParam.Value = long.Parse(parameters["LicenciaEscuelaID"]);
                    sqlParam.DbType = DbType.Int64;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if(parameters.ContainsKey("UsuarioID"))
                {
                    s_VarWHERE.Append(" AND lic.UsuarioID =@dbp4ram3 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram3";
                    sqlParam.Value = int.Parse(parameters["UsuarioID"]);
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if(parameters.ContainsKey("Tipo"))
                {
                    s_VarWHERE.Append(" AND lic.Tipo =@dbp4ram6 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram6";
                    sqlParam.Value = byte.Parse(parameters["Tipo"]);
                    sqlParam.DbType = DbType.Byte;
                    sqlCmd.Parameters.Add(sqlParam);
                }
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
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Licencia");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("LicenciaDARetHlp: Hubo un error al consultar los registros. " + exmsg);
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
