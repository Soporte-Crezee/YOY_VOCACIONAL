using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Modelo.BO;

namespace POV.Reactivos.DAO
{
    /// <summary>
    /// Consulta un registro de RespuestaPlantillaOpcionMultiple en la BD
    /// </summary>
    internal class RespuestaPlantillaOpcionMultipleDinamicoRetHlp
    {
        /// <summary>
        /// Consulta registros de RespuestaPlantillaOpcionMultiple en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPlantillaOpcionMultiple">RespuestaPlantillaOpcionMultiple que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de RespuestaPlantillaOpcionMultiple generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Pregunta pregunta, RespuestaPlantillaOpcionMultiple respuestaPlantilla)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (respuestaPlantilla == null)
                sError += ", RespuestaPlantillaOpcionMultiple";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaOpcionMultipleDinamicoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (pregunta == null)
                sError += ", Pregunta";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaOpcionMultipleDinamicoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "RespuestaPlantillaOpcionMultipleDinamicoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPlantillaOpcionMultipleDinamicoRetHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "RespuestaPlantillaOpcionMultipleDinamicoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT rpod.RespuestaPlantillaID, rpod.NumeroSeleccionablesMaximo, rpod.NumeroSeleccionablesMinimo, rpod.ModoSeleccion, rpod.PresentacionOpcion, rpd.Estatus, rpd.TipoRespuestaPlantilla, rpd.PreguntaID, rpd.FechaRegistro, rpd.TipoPuntaje ");
            sCmd.Append(" FROM RespuestaPlantillaOpcionMultipleDinamico rpod JOIN RespuestaPlantillaDinamico rpd ON rpd.RespuestaPlantillaID = rpod.RespuestaPlantillaID");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (respuestaPlantilla.RespuestaPlantillaID != null)
            {
                s_VarWHERE.Append(" rpod.RespuestaPlantillaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = respuestaPlantilla.RespuestaPlantillaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.NumeroSeleccionablesMaximo != null)
            {
                s_VarWHERE.Append(" AND rpod.NumeroSeleccionablesMaximo = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = respuestaPlantilla.NumeroSeleccionablesMaximo;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.NumeroSeleccionablesMinimo != null)
            {
                s_VarWHERE.Append(" AND rpod.NumeroSeleccionablesMinimo = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = respuestaPlantilla.NumeroSeleccionablesMinimo;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.ModoSeleccion != null)
            {
                s_VarWHERE.Append(" AND rpod.ModoSeleccion = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = respuestaPlantilla.ModoSeleccion;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.PresentacionOpcion != null)
            {
                s_VarWHERE.Append(" AND rpod.PresentacionOpcion = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = respuestaPlantilla.PresentacionOpcion;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.Estatus != null)
            {
                s_VarWHERE.Append(" AND rpd.Estatus = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = respuestaPlantilla.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.TipoRespuestaPlantilla != null)
            {
                s_VarWHERE.Append(" AND rpd.TipoRespuestaPlantilla = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = respuestaPlantilla.TipoRespuestaPlantilla;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.PreguntaID != null)
            {
                s_VarWHERE.Append(" AND rpd.PreguntaID = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = pregunta.PreguntaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND rpd.FechaRegistro = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = respuestaPlantilla.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.TipoPuntaje != null)
            {
                s_VarWHERE.Append(" AND rpd.TipoPuntaje = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
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
                throw new Exception("RespuestaPlantillaOpcionMultipleDinamicoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
