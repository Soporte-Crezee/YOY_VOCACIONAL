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
    /// Actualiza un registro de RespuestaPlantillaAbierta en la BD
    /// </summary>
    internal class RespuestaPlantillaAbiertaUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de RespuestaPlantillaAbiertaUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="respuestaPlantillaAbiertaUpdHlp">RespuestaPlantillaAbiertaUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RespuestaPlantillaAbiertaUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, RespuestaPlantillaAbierta respuestaPlantillaAbierta, RespuestaPlantillaAbierta anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (respuestaPlantillaAbierta == null)
                sError += ", respuestaPlantillaAbierta";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaAbiertaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.RespuestaPlantillaID == null)
                sError += ", Anterior RespuestaPlantillaID";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaAbiertaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "RespuestaPlantillaAbiertaUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPlantillaAbiertaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Reactivos.DAO",
                   "RespuestaPlantillaAbiertaUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE RespuestaPlantillaAbiertaDinamico ");
            sCmd.Append(" SET ");
            if (respuestaPlantillaAbierta.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.ABIERTA)
            {
                RespuestaPlantillaTexto respuestaPlantillaTexto = (RespuestaPlantillaTexto) respuestaPlantillaAbierta;
                if (respuestaPlantillaTexto.MaximoCaracteres == null)
                    sCmd.Append(" MaximoCaracteres = NULL ");
                else
                {
                    sCmd.Append(" MaximoCaracteres = @respuestaPlantillaAbierta_MaximoCaracteres ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "respuestaPlantillaAbierta_MaximoCaracteres";
                    sqlParam.Value = respuestaPlantillaTexto.MaximoCaracteres;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (respuestaPlantillaTexto.MinimoCaracteres == null)
                    sCmd.Append(" ,MinimoCaracteres = NULL ");
                else
                {
                    sCmd.Append(" ,MinimoCaracteres = @respuestaPlantillaAbierta_MinimoCaracteres ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "respuestaPlantillaAbierta_MinimoCaracteres";
                    sqlParam.Value = respuestaPlantillaTexto.MinimoCaracteres;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (respuestaPlantillaTexto.EsSensibleMayusculaMinuscula == null)
                    sCmd.Append(" ,EsSensibleMayusculaMinuscula = NULL ");
                else
                {
                    sCmd.Append(" ,EsSensibleMayusculaMinuscula = @respuestaPlantillaAbierta_EsSensibleMayusculaMinuscula ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "respuestaPlantillaAbierta_EsSensibleMayusculaMinuscula";
                    sqlParam.Value = respuestaPlantillaTexto.EsSensibleMayusculaMinuscula;
                    sqlParam.DbType = DbType.Boolean;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (respuestaPlantillaTexto.EsRespuestaCorta == null)
                    sCmd.Append(" ,EsRespuestaCorta = NULL ");
                else
                {
                    sCmd.Append(" ,EsRespuestaCorta = @respuestaPlantillaAbierta_EsRespuestaCorta ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "respuestaPlantillaAbierta_EsRespuestaCorta";
                    sqlParam.Value = respuestaPlantillaTexto.EsRespuestaCorta;
                    sqlParam.DbType = DbType.Boolean;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (respuestaPlantillaTexto.Ponderacion == null)
                    sCmd.Append(" ,Ponderacion = NULL ");
                else
                {
                    sCmd.Append(" ,Ponderacion = @respuestaPlantillaAbierta_Ponderacion ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "respuestaPlantillaAbierta_Ponderacion";
                    sqlParam.Value = respuestaPlantillaTexto.Ponderacion;
                    sqlParam.DbType = DbType.Decimal;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (respuestaPlantillaTexto.Modelo == null)
                    sCmd.Append(" ,ModeloId = NULL ");
                else
                {
                    sCmd.Append(" ,ModeloId = @respuestaPlantillaAbierta_Modelo ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "respuestaPlantillaAbierta_Modelo";
                    sqlParam.Value = respuestaPlantillaTexto.Modelo.ModeloID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (respuestaPlantillaTexto.Clasificador == null)
                    sCmd.Append(" ,ClasificadorId = NULL ");
                else
                {
                    sCmd.Append(" ,ClasificadorId = @respuestaPlantillaAbierta_Clasificador ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "respuestaPlantillaAbierta_Clasificador";
                    sqlParam.Value = respuestaPlantillaTexto.Clasificador.ClasificadorID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            if (respuestaPlantillaAbierta.TipoRespuestaPlantilla == ETipoRespuestaPlantilla.ABIERTA_NUMERICO)
            {
                RespuestaPlantillaNumerico respuestaPlantillaNumerico =
                    (RespuestaPlantillaNumerico) respuestaPlantillaAbierta;
                if (respuestaPlantillaNumerico.Estatus == null)
                    sCmd.Append(" ,Estatus = NULL ");
                else
                {
                    sCmd.Append(" ,Estatus = @respuestaPlantillaAbierta_Estatus ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "respuestaPlantillaAbierta_Estatus";
                    sqlParam.Value = respuestaPlantillaNumerico.Estatus;
                    sqlParam.DbType = DbType.Boolean;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }

            if (anterior.RespuestaPlantillaID == null)
                sCmd.Append(" WHERE RespuestaPlantillaID IS NULL ");
            else
            {
                sCmd.Append(" WHERE RespuestaPlantillaID = @anterior_RespuestaPlantillaID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "anterior_RespuestaPlantillaID";
                sqlParam.Value = anterior.RespuestaPlantillaID;
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
                throw new Exception("RespuestaPlantillaAbiertaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RespuestaPlantillaAbiertaUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
