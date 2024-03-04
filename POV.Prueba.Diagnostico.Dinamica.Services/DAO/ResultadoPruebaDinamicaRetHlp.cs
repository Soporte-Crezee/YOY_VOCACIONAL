using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.CentroEducativo.BO;
using POV.Expediente.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    /// <summary>
    /// Consulta un registro de ResultadoPruebaDinamica en la BD
    /// </summary>
    internal class ResultadoPruebaDinamicaRetHlp
    {
        /// <summary>
        /// Consulta registros de ResultadoPruebaDinamica en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoPruebaDinamica">ResultadoPruebaDinamica que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ResultadoPruebaDinamica generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, ResultadoPruebaDinamica resultadoPrueba)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (resultadoPrueba == null)
                sError += ", ResultadoPruebaDinamica";
            if (sError.Length > 0)
                throw new Exception("ResultadoPruebaDinamicaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (detalleCicloEscolar == null)
                sError += ", DetalleCicloEscolar";
            if (sError.Length > 0)
                throw new Exception("ResultadoPruebaDinamicaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (resultadoPrueba.Alumno == null)
            {
                resultadoPrueba.Alumno = new Alumno();
            }
            if (resultadoPrueba.Prueba == null)
            {
                resultadoPrueba.Prueba = new PruebaDinamica();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "ResultadoPruebaDinamicaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ResultadoPruebaDinamicaRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "ResultadoPruebaDinamicaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT rp.ResultadoPruebaID, rp.DetalleCicloEscolarID, rp.FechaRegistro, rp.Tipo, rp.PruebaID, rpd.AlumnoID ");
            sCmd.Append(" FROM ResultadoPrueba rp ");
            sCmd.Append(" JOIN ResultadoPruebaDinamica rpd ON rpd.ResultadoPruebaID = rp.ResultadoPruebaID ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (resultadoPrueba.ResultadoPruebaID != null)
            {
                s_VarWHERE.Append(" rp.ResultadoPruebaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = resultadoPrueba.ResultadoPruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (detalleCicloEscolar.DetalleCicloEscolarID != null)
            {
                s_VarWHERE.Append(" AND rp.DetalleCicloEscolarID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = detalleCicloEscolar.DetalleCicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoPrueba.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND rp.FechaRegistro = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = resultadoPrueba.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoPrueba.Tipo != null)
            {
                s_VarWHERE.Append(" AND rp.Tipo = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = resultadoPrueba.Tipo;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoPrueba.Prueba.PruebaID != null)
            {
                s_VarWHERE.Append(" AND rp.PruebaID = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = resultadoPrueba.Prueba.PruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoPrueba.Alumno.AlumnoID != null)
            {
                s_VarWHERE.Append(" AND rpd.AlumnoID = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = resultadoPrueba.Alumno.AlumnoID;
                sqlParam.DbType = DbType.Int64;
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
                sqlAdapter.Fill(ds, "ResultadoPrueba");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ResultadoPruebaDinamicaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
