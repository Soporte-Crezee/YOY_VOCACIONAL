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
    /// Guarda un registro de RespuestaReactivoDinamica en la BD
    /// </summary>
    internal class RespuestaReactivoDinamicaInsHlp
    {
        /// <summary>
        /// Crea un registro de RespuestaReactivoDinamica en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaReactivoDinamica">RespuestaReactivoDinamica que desea crear</param>
        public void Action(IDataContext dctx, RegistroPruebaDinamica registroPrueba, RespuestaReactivoDinamica respuestaReactivo)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (registroPrueba == null)
                sError += ", RespuesRegistroPruebaDinamicataReactivoDinamica";
            if (sError.Length > 0)
                throw new Exception("RespuestaReactivoDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (registroPrueba.RegistroPruebaID == null)
                sError += ", RegistroPruebaID";
            if (sError.Length > 0)
                throw new Exception("RespuestaReactivoDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaReactivo == null)
                sError += ", RespuestaReactivoDinamica";
            if (sError.Length > 0)
                throw new Exception("RespuestaReactivoDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaReactivo.Reactivo == null)
                sError += ", Reactivo";
            if (sError.Length > 0)
                throw new Exception("RespuestaReactivoDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaReactivo.Reactivo.ReactivoID == null)
                sError += ", ReactivoID";
            if (sError.Length > 0)
                throw new Exception("RespuestaReactivoDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaReactivo.EstadoReactivo == null)
                sError += ", EstadoReactivo";
            if (respuestaReactivo.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (sError.Length > 0)
                throw new Exception("RespuestaReactivoDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaReactivoDinamicaInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaReactivoDinamicaInsHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaReactivoDinamicaInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO RespuestaReactivoDinamica (RegistroPruebaID, ReactivoID, EstadoReactivo, Tiempo, FechaRegistro) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // registroPrueba.RegistroPruebaID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (registroPrueba.RegistroPruebaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = registroPrueba.RegistroPruebaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaReactivo.Reactivo.ReactivoID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (respuestaReactivo.Reactivo.ReactivoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaReactivo.Reactivo.ReactivoID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaReactivo.EstadoReactivo
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (respuestaReactivo.EstadoReactivo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaReactivo.EstadoReactivo;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaReactivo.Tiempo
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (respuestaReactivo.Tiempo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaReactivo.Tiempo;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaReactivo.FechaRegistro
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (respuestaReactivo.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaReactivo.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
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
                throw new Exception("RespuestaReactivoDinamicaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RespuestaReactivoDinamicaInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
