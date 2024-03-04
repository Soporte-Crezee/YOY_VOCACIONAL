using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;

namespace POV.Reactivos.DAO
{
    /// <summary>
    /// Consulta un registro de RespuestaPlantilla en la BD
    /// </summary>
    internal class RespuestaPlantillaDinamicoRetHlp
    {
        /// <summary>
        /// Consulta registros de RespuestaPlantilla en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPlantilla">RespuestaPlantilla que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de RespuestaPlantilla generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Pregunta pregunta, RespuestaPlantilla respuestaPlantilla)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (respuestaPlantilla == null)
                sError += ", RespuestaPlantilla";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaDinamicoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (pregunta == null)
                sError += ", pregunta";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaDinamicoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "RespuestaPlantillaDinamicoRetHlp", "Action", null, null);

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPlantillaDinamicoRetHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "RespuestaPlantillaDinamicoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT RespuestaPlantillaID, Estatus, TipoRespuestaPlantilla, PreguntaID, FechaRegistro, TipoPuntaje ");
            sCmd.Append(" FROM RespuestaPlantillaDinamico ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (respuestaPlantilla.RespuestaPlantillaID != null)
            {
                s_VarWHERE.Append(" RespuestaPlantillaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = respuestaPlantilla.RespuestaPlantillaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.Estatus != null)
            {
                s_VarWHERE.Append(" AND Estatus = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = respuestaPlantilla.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.TipoRespuestaPlantilla != null)
            {
                s_VarWHERE.Append(" AND TipoRespuestaPlantilla = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = respuestaPlantilla.TipoRespuestaPlantilla;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.PreguntaID != null)
            {
                s_VarWHERE.Append(" AND PreguntaID = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = pregunta.PreguntaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = respuestaPlantilla.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.TipoPuntaje != null)
            {
                s_VarWHERE.Append(" AND TipoPuntaje = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                if (pregunta.RespuestaPlantilla.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.ABIERTA)
                    sqlParam.Value = 0;
                else
                    sqlParam.Value = respuestaPlantilla.TipoPuntaje;
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
                sqlAdapter.Fill(ds, "RespuestaPlantilla");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("RespuestaPlantillaDinamicoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
