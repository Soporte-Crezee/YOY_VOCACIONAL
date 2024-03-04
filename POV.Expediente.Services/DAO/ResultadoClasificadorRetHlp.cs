using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;
using POV.Expediente.BO;

namespace POV.Expediente.DAO
{
    /// <summary>
    /// Consulta un registro de AResultadoClasificador en la BD
    /// </summary>
    internal class ResultadoClasificadorRetHlp
    {
        /// <summary>
        /// Consulta registros de AResultadoClasificador en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aResultadoClasificador">AResultadoClasificador que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AResultadoClasificador generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, AResultadoClasificador resultadoClasificador)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (resultadoClasificador == null)
                sError += ", AResultadoClasificador";
            if (sError.Length > 0)
                throw new Exception("ResultadoClasificadorRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Expediente.DAO",
                   "ResultadoClasificadorRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ResultadoClasificadorRetHlp: No se pudo conectar a la base de datos", "POV.Expediente.DAO",
                   "ResultadoClasificadorRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT rc.ResultadoClasificadorID, rc.ResultadoPruebaID, rc.ModeloID, rc.Tipo, rc.FechaRegistro ");
            sCmd.Append(" FROM ResultadoClasificador rc ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (resultadoClasificador.ResultadoClasificadorID != null)
            {
                s_VarWHERE.Append(" rc.ResultadoClasificadorID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = resultadoClasificador.ResultadoClasificadorID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoClasificador.ResultadoPrueba != null)
                if (resultadoClasificador.ResultadoPrueba.ResultadoPruebaID != null)
                {
                    s_VarWHERE.Append(" AND rc.ResultadoPruebaID =@dbp4ram2 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram2";
                    sqlParam.Value = resultadoClasificador.ResultadoPrueba.ResultadoPruebaID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            if (resultadoClasificador.Modelo != null)
                if (resultadoClasificador.Modelo.ModeloID != null)
                {
                    s_VarWHERE.Append(" AND rc.ModeloID =@dbp4ram3 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram3";
                    sqlParam.Value = resultadoClasificador.Modelo.ModeloID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            if (resultadoClasificador.TipoResultadoClasificador != null)
            {
                s_VarWHERE.Append(" AND rc.Tipo =@dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = resultadoClasificador.TipoResultadoClasificador;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoClasificador.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND rc.FechaRegistro =@dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = resultadoClasificador.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
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
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "AResultadoClasificador");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ResultadoClasificadorRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
