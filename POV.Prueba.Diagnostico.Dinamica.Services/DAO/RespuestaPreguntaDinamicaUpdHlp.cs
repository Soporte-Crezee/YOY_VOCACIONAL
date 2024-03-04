using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    /// <summary>
    /// Actualiza un registro de RespuestaPreguntaDinamica en la BD
    /// </summary>
    internal class RespuestaPreguntaDinamicaUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de RespuestaPreguntaDinamicaUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPreguntaDinamicaUpdHlp">RespuestaPreguntaDinamicaUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RespuestaPreguntaDinamicaUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, RespuestaPreguntaDinamica respuestaPregunta, 
            RespuestaDinamicaAbierta respuestaAbierta, RespuestaPreguntaDinamica anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (respuestaPregunta == null)
                sError += ", RespuestaPreguntaDinamica";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.RespuestaPreguntaID == null)
                sError += ", Anterior RespuestaPreguntaDinamicaID";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaPreguntaDinamicaUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPreguntaDinamicaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaPreguntaDinamicaUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE RespuestaPreguntaDinamica ");
            if (respuestaAbierta.Tiempo != null)
            {
                sCmd.Append(" SET Tiempo = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = respuestaAbierta.Tiempo;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaAbierta.TextoRespuesta != null)
            {
                sCmd.Append(" ,TextoRespuesta = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = respuestaAbierta.TextoRespuesta;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaAbierta.ValorRespuesta != null)
            {
                sCmd.Append(" ,ValorRespuesta = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = respuestaAbierta.ValorRespuesta;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPregunta.EstadoRespuesta != null)
            {
                sCmd.Append(" ,EstadoRespuesta = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = respuestaPregunta.EstadoRespuesta;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.RespuestaPreguntaID == null)
                sCmd.Append(" WHERE RespuestaPreguntaID IS NULL ");
            else
            {
                // anterior.RespuestaPreguntaDinamicaID
                sCmd.Append(" WHERE RespuestaPreguntaID = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = anterior.RespuestaPreguntaID;
                sqlParam.DbType = DbType.Int32;
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
                throw new Exception("RespuestaPreguntaDinamicaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RespuestaPreguntaDinamicaUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de RespuestaPreguntaDinamicaUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPreguntaDinamicaUpdHlp">RespuestaPreguntaDinamicaUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RespuestaPreguntaDinamicaUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, RespuestaPreguntaDinamica respuestaPregunta,
            RespuestaDinamicaOpcionMultiple respuestaOpcionMultiple, RespuestaPreguntaDinamica anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (respuestaPregunta == null)
                sError += ", RespuestaPreguntaDinamica";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.RespuestaPreguntaID == null)
                sError += ", Anterior RespuestaPreguntaDinamicaID";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaPreguntaDinamicaUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPreguntaDinamicaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaPreguntaDinamicaUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE RespuestaPreguntaDinamica ");
            if (respuestaOpcionMultiple.Tiempo != null)
            {
                sCmd.Append(" SET Tiempo = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = respuestaOpcionMultiple.Tiempo;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPregunta.EstadoRespuesta != null)
            {
                sCmd.Append(" ,EstadoRespuesta = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = respuestaPregunta.EstadoRespuesta;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.RespuestaPreguntaID == null)
                sCmd.Append(" WHERE RespuestaPreguntaID IS NULL ");
            else
            {
                // anterior.RespuestaPreguntaDinamicaID
                sCmd.Append(" WHERE RespuestaPreguntaID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = anterior.RespuestaPreguntaID;
                sqlParam.DbType = DbType.Int32;
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
                throw new Exception("RespuestaPreguntaDinamicaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RespuestaPreguntaDinamicaUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
