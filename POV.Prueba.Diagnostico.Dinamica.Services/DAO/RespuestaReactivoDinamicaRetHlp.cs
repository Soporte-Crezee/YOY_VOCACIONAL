using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Reactivos.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    /// <summary>
    /// Consulta un registro de RespuestaReactivoDinamica en la BD
    /// </summary>
    internal class RespuestaReactivoDinamicaRetHlp
    {
        /// <summary>
        /// Consulta registros de RespuestaReactivoDinamica en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaReactivoDinamica">RespuestaReactivoDinamica que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de RespuestaReactivoDinamica generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, RegistroPruebaDinamica registroPrueba, RespuestaReactivoDinamica respuestaReactivo)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (respuestaReactivo == null)
                sError += ", RespuestaReactivoDinamica";
            if (sError.Length > 0)
                throw new Exception("RespuestaReactivoDinamicaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (registroPrueba == null)
                sError += ", RegistroPruebaDinamica";
            if (sError.Length > 0)
                throw new Exception("RespuestaReactivoDinamicaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaReactivo.Reactivo == null)
            {
                respuestaReactivo.Reactivo = new Reactivo();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaReactivoDinamicaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaReactivoDinamicaRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaReactivoDinamicaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT RespuestaReactivoID, RegistroPruebaID, ReactivoID, EstadoReactivo, Tiempo, FechaRegistro ");
            sCmd.Append(" FROM RespuestaReactivoDinamica ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (respuestaReactivo.RespuestaReactivoID != null)
            {
                s_VarWHERE.Append(" RespuestaReactivoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = respuestaReactivo.RespuestaReactivoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (registroPrueba.RegistroPruebaID != null)
            {
                s_VarWHERE.Append(" AND RegistroPruebaID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = registroPrueba.RegistroPruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaReactivo.Reactivo.ReactivoID != null)
            {
                s_VarWHERE.Append(" AND ReactivoID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = respuestaReactivo.Reactivo.ReactivoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaReactivo.EstadoReactivo != null)
            {
                s_VarWHERE.Append(" AND EstadoReactivo = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = respuestaReactivo.EstadoReactivo;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaReactivo.Tiempo != null)
            {
                s_VarWHERE.Append(" AND Tiempo = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = respuestaReactivo.Tiempo;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaReactivo.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = respuestaReactivo.FechaRegistro;
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
                sqlAdapter.Fill(ds, "RespuestaReactivo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("RespuestaReactivoDinamicaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
