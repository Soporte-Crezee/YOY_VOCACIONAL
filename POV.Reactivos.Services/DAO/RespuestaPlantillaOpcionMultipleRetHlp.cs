using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Reactivos.DAO;

namespace POV.Reactivos.DAO
{
    /// <summary>
    /// Consulta un registro de RespuestaPlantillaOpcionMultiple en la BD
    /// </summary>
    public class RespuestaPlantillaOpcionMultipleRetHlp
    {
        /// <summary>
        /// Consulta registros de RespuestaPlantillaOpcionMultiple en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="respuestaPlantillaOpcionMultiple">RespuestaPlantillaOpcionMultiple que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de RespuestaPlantillaOpcionMultiple generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, RespuestaPlantillaOpcionMultiple respuestaPlantilla, Pregunta pregunta)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (pregunta == null)
            {
                pregunta = new Pregunta();
            }
            if (respuestaPlantilla == null)
                sError += ", RespuestaPlantillaOpcionMultiple";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaOpcionMultipleRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "RespuestaPlantillaOpcionMultipleRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPlantillaOpcionMultipleRetHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "RespuestaPlantillaOpcionMultipleRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT rp.RespuestaPlantillaID, rp.Estatus, rp.TipoRespuestaPlantilla, rp.PreguntaID, rp.FechaRegistro, rpom.NumeroSeleccionablesMaximo, rpom.NumeroSeleccionablesMinimo, rpom.ModoSeleccion ");
            sCmd.Append(" FROM RespuestaPlantillaOpcionMultiple rpom JOIN RespuestaPlantilla rp ON rp.RespuestaPlantillaID = rpom.RespuestaPlantillaID ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (respuestaPlantilla.RespuestaPlantillaID != null)
            {
                s_VarWHERE.Append(" rpom.RespuestaPlantillaID = @respuestaPlantilla_RespuestaPlantillaID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "respuestaPlantilla_RespuestaPlantillaID";
                sqlParam.Value = respuestaPlantilla.RespuestaPlantillaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.NumeroSeleccionablesMaximo != null)
            {
                s_VarWHERE.Append(" AND rpom.NumeroSeleccionablesMaximo = @respuestaPlantilla_NumeroSeleccionablesMaximo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "respuestaPlantilla_NumeroSeleccionablesMaximo";
                sqlParam.Value = respuestaPlantilla.NumeroSeleccionablesMaximo;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.NumeroSeleccionablesMinimo != null)
            {
                s_VarWHERE.Append(" AND rpom.NumeroSeleccionablesMinimo= @respuestaPlantilla_NumeroSeleccionablesMinimo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "respuestaPlantilla_NumeroSeleccionablesMinimo";
                sqlParam.Value = respuestaPlantilla.NumeroSeleccionablesMinimo;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.ModoSeleccion != null)
            {
                s_VarWHERE.Append(" AND rpom.ModoSeleccion = @respuestaPlantilla_ModoSeleccion ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "respuestaPlantilla_ModoSeleccion";
                sqlParam.Value = respuestaPlantilla.ModoSeleccion;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.PreguntaID != null)
            {
                s_VarWHERE.Append(" AND rp.PreguntaID = @pregunta_PreguntaID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "pregunta_PreguntaID";
                sqlParam.Value = pregunta.PreguntaID;
                sqlParam.DbType = DbType.Int32;
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
                sqlAdapter.Fill(ds, "RespuestaPlantillaOpcionMultiple");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("RespuestaPlantillaOpcionMultipleRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
