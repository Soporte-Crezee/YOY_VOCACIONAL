using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    /// <summary>
    /// Consulta un registro de ResultadoClasificadorDinamica en la BD
    /// </summary>
    internal class ResultadoClasificadorDinamicaRetHlp
    {
        /// <summary>
        /// Consulta registros de ResultadoClasificadorDinamica en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoClasificadorDinamica">ResultadoClasificadorDinamica que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ResultadoClasificadorDinamica generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, ResultadoClasificadorDinamica resultadoClasificadorDinamica)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (resultadoClasificadorDinamica == null)
                sError += ", ResultadoClasificadorDinamica";
            if (sError.Length > 0)
                throw new Exception("ResultadoClasificadorDinamicaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "ResultadoClasificadorDinamicaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ResultadoClasificadorDinamicaRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "ResultadoClasificadorDinamicaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT rc.ResultadoClasificadorID, rc.ResultadoPruebaID, rc.ModeloID, rc.Tipo, rc.FechaRegistro, rcf.ClasificadorID ");
            sCmd.Append(" FROM ResultadoClasificador rc ");
            sCmd.Append(" JOIN ResultadoClasificadorDinamica rcf ON rcf.ResultadoClasificadorID = rc.ResultadoClasificadorID  ");
            StringBuilder s_Var = new StringBuilder();
            if (resultadoClasificadorDinamica.ResultadoClasificadorID != null)
            {
                s_Var.Append(" rc.ResultadoClasificadorID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = resultadoClasificadorDinamica.ResultadoClasificadorID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoClasificadorDinamica.ResultadoPrueba != null)
                if (resultadoClasificadorDinamica.ResultadoPrueba.ResultadoPruebaID != null)
                {
                    s_Var.Append(" AND rc.ResultadoPruebaID = @dbp4ram2 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram2";
                    sqlParam.Value = resultadoClasificadorDinamica.ResultadoPrueba.ResultadoPruebaID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            if (resultadoClasificadorDinamica.Modelo != null)
                if (resultadoClasificadorDinamica.Modelo.ModeloID != null)
                {
                    s_Var.Append(" AND rc.ModeloID = @dbp4ram3 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram3";
                    sqlParam.Value = resultadoClasificadorDinamica.Modelo.ModeloID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            if (resultadoClasificadorDinamica.TipoResultadoClasificador != null)
            {
                s_Var.Append(" AND rc.Tipo = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = resultadoClasificadorDinamica.TipoResultadoClasificador;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoClasificadorDinamica.FechaRegistro != null)
            {
                s_Var.Append(" AND rc.FechaRegistro = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = resultadoClasificadorDinamica.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoClasificadorDinamica.Clasificador != null)
                if (resultadoClasificadorDinamica.Clasificador.ClasificadorID != null)
                {
                    s_Var.Append(" AND rcf.ClasificadorID = @dbp4ram6 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram6";
                    sqlParam.Value = resultadoClasificadorDinamica.Clasificador.ClasificadorID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            s_Var.Append("  ");
            string s_Varres = s_Var.ToString().Trim();
            if (s_Varres.Length > 0)
            {
                if (s_Varres.StartsWith("AND "))
                    s_Varres = s_Varres.Substring(4);
                else if (s_Varres.StartsWith("OR "))
                    s_Varres = s_Varres.Substring(3);
                else if (s_Varres.StartsWith(","))
                    s_Varres = s_Varres.Substring(1);
                sCmd.Append(" WHERE " + s_Varres);
            }
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "ResultadoClasificador");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ResultadoClasificadorDinamicaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
