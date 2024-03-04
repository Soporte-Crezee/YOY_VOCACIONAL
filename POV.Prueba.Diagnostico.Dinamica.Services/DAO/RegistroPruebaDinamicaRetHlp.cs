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

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    /// <summary>
    /// Consulta un registro de RegistroPruebaDinamica en la BD
    /// </summary>
    internal class RegistroPruebaDinamicaRetHlp
    {
        /// <summary>
        /// Consulta registros de RegistroPruebaDinamica en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPruebaDinamica">RegistroPruebaDinamica que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de RegistroPruebaDinamica generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, ResultadoPruebaDinamica resultadoPrueba, RegistroPruebaDinamica registroPrueba)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (resultadoPrueba == null)
                sError += ", ResultadoPruebaDinamica";
            if (sError.Length > 0)
                throw new Exception("RegistroPruebaDinamicaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (registroPrueba == null)
                sError += ", RegistroPruebaDinamica";
            if (sError.Length > 0)
                throw new Exception("RegistroPruebaDinamicaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (registroPrueba.Alumno == null)
            {
                registroPrueba.Alumno = new Alumno();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RegistroPruebaDinamicaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RegistroPruebaDinamicaRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RegistroPruebaDinamicaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT RegistroPruebaID, ResultadoPruebaID, AlumnoID, EstadoPrueba, FechaInicio, FechaFin, FechaRegistro ");
            sCmd.Append(" FROM RegistroPruebaDinamica ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (registroPrueba.RegistroPruebaID != null)
            {
                s_VarWHERE.Append(" RegistroPruebaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = registroPrueba.RegistroPruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoPrueba.ResultadoPruebaID != null)
            {
                s_VarWHERE.Append(" AND ResultadoPruebaID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = resultadoPrueba.ResultadoPruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (registroPrueba.Alumno.AlumnoID != null)
            {
                s_VarWHERE.Append(" AND AlumnoID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = registroPrueba.Alumno.AlumnoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (registroPrueba.EstadoPrueba != null)
            {
                s_VarWHERE.Append(" AND EstadoPrueba = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = registroPrueba.EstadoPrueba;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (registroPrueba.FechaInicio != null)
            {
                s_VarWHERE.Append(" AND FechaInicio = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = registroPrueba.FechaInicio;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (registroPrueba.FechaFin != null)
            {
                s_VarWHERE.Append(" AND FechaFin = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = registroPrueba.FechaFin;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (registroPrueba.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = registroPrueba.FechaRegistro;
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
                sqlAdapter.Fill(ds, "RegistroPrueba");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("RegistroPruebaDinamicaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
