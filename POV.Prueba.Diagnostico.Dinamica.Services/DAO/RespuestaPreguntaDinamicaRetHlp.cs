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
    /// Consulta un registro de RespuestaPreguntaDinamica en la BD
    /// </summary>
    internal class RespuestaPreguntaDinamicaRetHlp
    {
        /// <summary>
        /// Consulta registros de RespuestaPreguntaDinamica en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPreguntaDinamica">RespuestaPreguntaDinamica que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de RespuestaPreguntaDinamica generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, RespuestaReactivoDinamica respuestaReactivo, RespuestaPreguntaDinamica respuestaPregunta)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (respuestaPregunta == null)
                sError += ", RespuestaPreguntaDinamica";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPregunta == null)
                sError += ", RespuestaPreguntaDinamica";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPregunta.Pregunta == null)
            {
                respuestaPregunta.Pregunta = new Pregunta();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaPreguntaDinamicaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPreguntaDinamicaRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaPreguntaDinamicaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT RespuestaPreguntaID, RespuestaReactivoID, PreguntaID, RespuestaPlantillaID, FechaRegistro, TipoRespuestaPlantilla, Tiempo, TextoRespuesta, ValorRespuesta, EstadoRespuesta ");
            sCmd.Append(" FROM RespuestaPreguntaDinamica ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (respuestaPregunta.RespuestaPreguntaID != null)
            {
                s_VarWHERE.Append(" RespuestaPreguntaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = respuestaPregunta.RespuestaPreguntaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaReactivo.RespuestaReactivoID != null)
            {
                s_VarWHERE.Append(" AND RespuestaReactivoID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = respuestaReactivo.RespuestaReactivoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPregunta.Pregunta.PreguntaID != null)
            {
                s_VarWHERE.Append(" AND PreguntaID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = respuestaPregunta.Pregunta.PreguntaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPregunta.Pregunta.RespuestaPlantilla != null)
            {
                if (respuestaPregunta.Pregunta.RespuestaPlantilla.RespuestaPlantillaID != null)
                {
                    s_VarWHERE.Append(" AND RespuestaPlantillaID = @dbp4ram4 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram4";
                    sqlParam.Value = respuestaPregunta.Pregunta.RespuestaPlantilla.RespuestaPlantillaID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            if (respuestaPregunta.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = respuestaPregunta.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPregunta.RespuestaAlumno != null)
            {
                if (respuestaPregunta.RespuestaAlumno.TipoRespuestaPlantilla != null)
                {
                    s_VarWHERE.Append(" AND TipoRespuestaPlantilla = @dbp4ram6 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram6";
                    sqlParam.Value = respuestaPregunta.RespuestaAlumno.TipoRespuestaPlantilla;
                    sqlParam.DbType = DbType.Byte;
                    sqlCmd.Parameters.Add(sqlParam);
                }

                if (respuestaPregunta.RespuestaAlumno.Tiempo != null)
                {
                    s_VarWHERE.Append(" AND Tiempo = @dbp4ram7 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram7";
                    sqlParam.Value = respuestaPregunta.RespuestaAlumno.Tiempo;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (respuestaPregunta.RespuestaAlumno is RespuestaDinamicaAbierta)
                {
                    if ((respuestaPregunta.RespuestaAlumno as RespuestaDinamicaAbierta).TextoRespuesta != null)
                    {
                        s_VarWHERE.Append(" AND TextoRespuesta = @dbp4ram8 ");
                        sqlParam = sqlCmd.CreateParameter();
                        sqlParam.ParameterName = "dbp4ram8";
                        sqlParam.Value = (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaAbierta).TextoRespuesta;
                        sqlParam.DbType = DbType.String;
                        sqlCmd.Parameters.Add(sqlParam);
                    }
                    if ((respuestaPregunta.RespuestaAlumno as RespuestaDinamicaAbierta).ValorRespuesta != null)
                    {
                        s_VarWHERE.Append(" AND ValorRespuesta = @dbp4ram9 ");
                        sqlParam = sqlCmd.CreateParameter();
                        sqlParam.ParameterName = "dbp4ram9";
                        sqlParam.Value = (respuestaPregunta.RespuestaAlumno as RespuestaDinamicaAbierta).ValorRespuesta;
                        sqlParam.DbType = DbType.Decimal;
                        sqlCmd.Parameters.Add(sqlParam);
                    }
                }
            }

            if (respuestaPregunta.EstadoRespuesta != null)
            {
                s_VarWHERE.Append(" AND EstadoRespuesta = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = respuestaPregunta.EstadoRespuesta;
                sqlParam.DbType = DbType.Byte;
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
                sqlAdapter.Fill(ds, "RespuestaPregunta");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("RespuestaPreguntaDinamicaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
