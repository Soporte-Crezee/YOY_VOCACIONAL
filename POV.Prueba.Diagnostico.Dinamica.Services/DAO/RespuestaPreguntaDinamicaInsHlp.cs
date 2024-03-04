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
    /// Guarda un registro de RespuestaPreguntaDinamica en la BD
    /// </summary>
    internal class RespuestaPreguntaDinamicaInsHlp
    {
        /// <summary>
        /// Crea un registro de RespuestaPreguntaDinamica en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPreguntaDinamica">RespuestaPreguntaDinamica que desea crear</param>
        public void Action(IDataContext dctx, RespuestaReactivoDinamica respuestaReactivo, 
            RespuestaPreguntaDinamica respuestaPreguntaDinamica, RespuestaDinamicaAbierta respuestaAbierta)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (respuestaReactivo == null)
                sError += ", RespuestaReactivoDinamica";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaReactivo.RespuestaReactivoID == null)
                sError += ", RespuestaReactivoID";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPreguntaDinamica == null)
                sError += ", RespuestaPreguntaDinamica";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPreguntaDinamica.Pregunta == null)
                sError += ", Pregunta";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPreguntaDinamica.Pregunta.PreguntaID == null)
                sError += ", PreguntaID";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPreguntaDinamica.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (respuestaPreguntaDinamica.EstadoRespuesta == null)
                sError += ", EstadoRespuesta";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaAbierta == null)
                sError += ", RespuestaDinamicaAbierta";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaAbierta.RespuestaPlantilla == null)
                sError += ", RespuestaPlantilla";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaAbierta.RespuestaPlantilla.RespuestaPlantillaID == null)
                sError += ", RespuestaPlantillaID";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaAbierta.TipoRespuestaPlantilla == null)
                sError += ", TipoRespuestaPlantilla";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaPreguntaDinamicaInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPreguntaDinamicaInsHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaPreguntaDinamicaInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO RespuestaPreguntaDinamica (RespuestaReactivoID, PreguntaID, RespuestaPlantillaID, FechaRegistro, TipoRespuestaPlantilla, Tiempo, TextoRespuesta, ValorRespuesta, EstadoRespuesta) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // respuestaReactivo.RespuestaReactivoID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (respuestaReactivo.RespuestaReactivoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaReactivo.RespuestaReactivoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaPreguntaDinamica.Pregunta.PreguntaID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (respuestaPreguntaDinamica.Pregunta.PreguntaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPreguntaDinamica.Pregunta.PreguntaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaAbierta.RespuestaPlantilla.RespuestaPlantillaID
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (respuestaAbierta.RespuestaPlantilla.RespuestaPlantillaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaAbierta.RespuestaPlantilla.RespuestaPlantillaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaPreguntaDinamica.FechaRegistro
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (respuestaPreguntaDinamica.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPreguntaDinamica.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaPreguntaDinamica.TipoRespuestaPlantilla
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (respuestaAbierta.TipoRespuestaPlantilla == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaAbierta.TipoRespuestaPlantilla;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaAbierta.Tiempo
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (respuestaAbierta.Tiempo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaAbierta.Tiempo;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaAbierta.TextoRespuesta
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (respuestaAbierta.TextoRespuesta == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaAbierta.TextoRespuesta;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaAbierta.ValorRespuesta
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (respuestaAbierta.ValorRespuesta == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaAbierta.ValorRespuesta;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaPreguntaDinamica.EstadoRespuesta
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if (respuestaPreguntaDinamica.EstadoRespuesta == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPreguntaDinamica.EstadoRespuesta;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ) ");
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
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Ocurrió un error al ingresar el registro.");
        }
        /// <summary>
        /// Crea un registro de RespuestaPreguntaDinamica en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPreguntaDinamica">RespuestaPreguntaDinamica que desea crear</param>
        public void Action(IDataContext dctx, RespuestaReactivoDinamica respuestaReactivo, RespuestaPreguntaDinamica respuestaPreguntaDinamica, RespuestaDinamicaOpcionMultiple respuestaOpcionMultiple)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (respuestaReactivo == null)
                sError += ", RespuestaReactivoDinamica";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaReactivo.RespuestaReactivoID == null)
                sError += ", RespuestaReactivoID";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPreguntaDinamica == null)
                sError += ", RespuestaPreguntaDinamica";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPreguntaDinamica.Pregunta == null)
                sError += ", Pregunta";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPreguntaDinamica.Pregunta.PreguntaID == null)
                sError += ", PreguntaID";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPreguntaDinamica.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (respuestaPreguntaDinamica.EstadoRespuesta == null)
                sError += ", EstadoRespuesta";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaOpcionMultiple.TipoRespuestaPlantilla == null)
                sError += ", TipoRespuestaPlantilla";
            if (sError.Length > 0)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaPreguntaDinamicaInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPreguntaDinamicaInsHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaPreguntaDinamicaInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO RespuestaPreguntaDinamica (RespuestaReactivoID, PreguntaID, FechaRegistro, TipoRespuestaPlantilla, Tiempo,  EstadoRespuesta) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // respuestaReactivo.RespuestaReactivoID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (respuestaReactivo.RespuestaReactivoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaReactivo.RespuestaReactivoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaPreguntaDinamica.Pregunta.PreguntaID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (respuestaPreguntaDinamica.Pregunta.PreguntaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPreguntaDinamica.Pregunta.PreguntaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaPreguntaDinamica.FechaRegistro
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (respuestaPreguntaDinamica.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPreguntaDinamica.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaPreguntaDinamica.TipoRespuestaPlantilla
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (respuestaOpcionMultiple.TipoRespuestaPlantilla == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaOpcionMultiple.TipoRespuestaPlantilla;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaOpcionMultiple.Tiempo
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (respuestaOpcionMultiple.Tiempo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaOpcionMultiple.Tiempo;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaPreguntaDinamica.EstadoRespuesta
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (respuestaPreguntaDinamica.EstadoRespuesta == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPreguntaDinamica.EstadoRespuesta;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ) ");
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
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RespuestaPreguntaDinamicaInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
