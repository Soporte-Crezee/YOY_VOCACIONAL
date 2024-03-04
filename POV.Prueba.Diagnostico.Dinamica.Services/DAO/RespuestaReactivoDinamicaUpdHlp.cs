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
    /// Actualiza un registro de RespuestaReactivoDinamica en la BD
    /// </summary>
    internal class RespuestaReactivoDinamicaUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de RespuestaReactivoDinamicaUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaReactivoDinamicaUpdHlp">RespuestaReactivoDinamicaUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RespuestaReactivoDinamicaUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, RespuestaReactivoDinamica respuestaReactivo, RespuestaReactivoDinamica anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (respuestaReactivo == null)
                sError += ", RespuestaReactivoDinamica";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("RespuestaReactivoDinamicaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.RespuestaReactivoID == null)
                sError += ", Anterior RespuestaReactivoDinamicaID";
            if (sError.Length > 0)
                throw new Exception("RespuestaReactivoDinamicaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaReactivoDinamicaUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaReactivoDinamicaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RespuestaReactivoDinamicaUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE RespuestaReactivoDinamica ");
            if (respuestaReactivo.EstadoReactivo != null)
            {
                sCmd.Append(" SET EstadoReactivo = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = respuestaReactivo.EstadoReactivo;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaReactivo.Tiempo != null)
            {
                sCmd.Append(" ,Tiempo = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = respuestaReactivo.Tiempo;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            StringBuilder s_VarWHERE = new StringBuilder();
            if (anterior.RespuestaReactivoID == null)
                s_VarWHERE.Append(" RespuestaReactivoID IS NULL ");
            else
            {
                // anterior.RespuestaReactivoID
                s_VarWHERE.Append(" RespuestaReactivoID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = anterior.RespuestaReactivoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.EstadoReactivo == null)
                s_VarWHERE.Append(" AND EstadoReactivo IS NULL ");
            else
            {
                // anterior.EstadoReactivo
                s_VarWHERE.Append(" AND EstadoReactivo = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = anterior.EstadoReactivo;
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
                throw new Exception("RespuestaReactivoDinamicaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RespuestaReactivoDinamicaUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
