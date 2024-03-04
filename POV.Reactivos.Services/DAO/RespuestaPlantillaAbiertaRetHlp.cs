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
    /// Consulta un registro de RespuestaPlantillaAbierta en la BD
    /// </summary>
    internal class RespuestaPlantillaAbiertaRetHlp
    {
        /// <summary>
        /// Consulta registros de RespuestaPlantillaAbierta en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="respuestaPlantillaAbierta">RespuestaPlantillaAbierta que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de RespuestaPlantillaAbierta generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, RespuestaPlantillaAbierta respuestaPlantillaAbierta, Pregunta pregunta)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (pregunta == null)
            {
                pregunta = new Pregunta();
            }
            if (respuestaPlantillaAbierta == null)
                sError += ", respuestaPlantillaAbierta";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaAbiertaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "RespuestaPlantillaAbiertaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPlantillaAbiertaRetHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "RespuestaPlantillaAbiertaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT rp.RespuestaPlantillaID, rp.Estatus, rp.TipoRespuestaPlantilla, rp.PreguntaID, rp.FechaRegistro, rp.TipoPuntaje, ");
            sCmd.Append(" rpa.Ponderacion, rpa.ValorRespuesta, rpa.MaximoCaracteres, rpa.MinimoCaracteres, rpa.EsSensibleMayusculaMinuscula, rpa.EsRespuestaCorta, ");
            sCmd.Append(" rpa.MargenError, rpa.NumeroDecimales, rpa.TipoMargen, rpa.ModeloID, rpa.ClasificadorID ");
            sCmd.Append(" FROM RespuestaPlantillaAbiertaDinamico rpa JOIN RespuestaPlantillaDinamico rp ON rp.RespuestaPlantillaID = rpa.RespuestaPlantillaID ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (respuestaPlantillaAbierta.RespuestaPlantillaID != null)
            {
                s_VarWHERE.Append(" rpa.RespuestaPlantillaID = @respuestaPlantillaAbierta_RespuestaPlantillaID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "respuestaPlantillaAbierta_RespuestaPlantillaID";
                sqlParam.Value = respuestaPlantillaAbierta.RespuestaPlantillaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.PreguntaID != null)
            {
                s_VarWHERE.Append(" AND rp.PreguntaID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = pregunta.PreguntaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            //Para saber que tipo de reactivo es.
            switch( respuestaPlantillaAbierta.TipoRespuestaPlantilla)
            {
                case ETipoRespuestaPlantilla.ABIERTA:
                    RespuestaPlantillaTexto respuestaPlantillaTexto =
                        (RespuestaPlantillaTexto) respuestaPlantillaAbierta;
                    if (respuestaPlantillaTexto.MaximoCaracteres != null)
                    {
                        s_VarWHERE.Append(" AND rpa.MaximoCaracteres = @respuestaPlantillaAbierta_MaximoCaracteres ");
                        sqlParam = sqlCmd.CreateParameter();
                        sqlParam.ParameterName = "respuestaPlantillaAbierta_MaximoCaracteres";
                        sqlParam.Value = respuestaPlantillaTexto.MaximoCaracteres;
                        sqlParam.DbType = DbType.Int32;
                        sqlCmd.Parameters.Add(sqlParam);
                    }
                    if (respuestaPlantillaTexto.MinimoCaracteres != null){
                        s_VarWHERE.Append(" AND rpa.MinimoCaracteres = @dbp4ram4 ");
                        sqlParam = sqlCmd.CreateParameter();
                        sqlParam.ParameterName = "dbp4ram4";
                        sqlParam.Value = respuestaPlantillaTexto.MinimoCaracteres;
                        sqlParam.DbType = DbType.Int32;
                        sqlCmd.Parameters.Add(sqlParam);
                    }
                    if (respuestaPlantillaTexto.EsSensibleMayusculaMinuscula != null){
                        s_VarWHERE.Append(" AND rpa.EsSensibleMayusculaMiniscula = @dbp4ram5 ");
                        sqlParam = sqlCmd.CreateParameter();
                        sqlParam.ParameterName = "dbp4ram5";
                        sqlParam.Value = respuestaPlantillaTexto.EsSensibleMayusculaMinuscula;
                        sqlParam.DbType = DbType.Boolean;
                        sqlCmd.Parameters.Add(sqlParam);
                    }
                    if (respuestaPlantillaTexto.EsRespuestaCorta != null){
                        s_VarWHERE.Append(" AND rpa.EsRespuestaCorta = @dbp4ram6 ");
                        sqlParam = sqlCmd.CreateParameter();
                        sqlParam.ParameterName = "dbp4ram6";
                        sqlParam.Value = respuestaPlantillaTexto.EsRespuestaCorta;
                        sqlParam.DbType = DbType.Boolean;
                        sqlCmd.Parameters.Add(sqlParam);
                    }
                    if (respuestaPlantillaTexto.Ponderacion != null)
                    {
                        s_VarWHERE.Append(" AND rpa.Ponderacion = @dbp4ram7 ");
                        sqlParam = sqlCmd.CreateParameter();
                        sqlParam.ParameterName = "dbp4ram7";
                        sqlParam.Value = respuestaPlantillaTexto.Ponderacion;
                        sqlParam.DbType = DbType.Decimal;
                        sqlCmd.Parameters.Add(sqlParam);
                    }
                    break;
                case ETipoRespuestaPlantilla.ABIERTA_NUMERICO:
                    RespuestaPlantillaNumerico respuestaPlantillaNumerico =
                        (RespuestaPlantillaNumerico) respuestaPlantillaAbierta;
                    if (respuestaPlantillaNumerico.Estatus != null)
                    {
                        s_VarWHERE.Append(" AND rpa.Estatus = @dbp4ram2 ");
                        sqlParam = sqlCmd.CreateParameter();
                        sqlParam.ParameterName = "dbp4ram2";
                        sqlParam.Value = respuestaPlantillaNumerico.Estatus;
                        sqlParam.DbType = DbType.Boolean;
                        sqlCmd.Parameters.Add(sqlParam);
                    }
                    break;
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
                sqlAdapter.Fill(ds, "RespuestaPlantillaAbierta");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("RespuestaPlantillaAbiertaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
