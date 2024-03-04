using System;
using System.Text;
using System.Data;
using System.Data.Common;
using POV.Prueba.Calificaciones.BO;
using Framework.Base.DataAccess;

namespace POV.Prueba.Calificaciones.DAO
{
    internal class ResultadoMetodoCalificacionRetHlp
    {
        public DataSet Action(IDataContext dctx, AResultadoMetodoCalificacion resultadoMetodoCalificacion)
        {
            if (resultadoMetodoCalificacion == null)
                throw new Exception("ResultadoMetodoCalificacionRetHlp.Action: El resultadoMetodoCalificacion es requerido");

            object myFirm = new object();
            if (dctx == null)
                throw new Exception("ResultadoMetodoCalificacionRetHlp.Action: DataContext no puede ser nulo");
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception("ResultadoMetodoCalificacionRetHlp.Action: Hubo un error al conectarse a la Base de Datos");
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ResultadoMetodoCalificacionID,ResultadoPruebaID,FechaRegistro,TipoResultadoMetodo,Puntos,MaximoPuntos,NumeroAciertos,TotalAciertos,EsAproximado,PuntajeID ");
            sCmd.Append(" FROM ResultadoMetodoCalificacion ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (resultadoMetodoCalificacion.ResultadoMetodoCalificacionID != null)
            {
                s_VarWHERE.Append(" AND ResultadoMetodoCalificacionID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = resultadoMetodoCalificacion.ResultadoMetodoCalificacionID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoMetodoCalificacion.ResultadoPrueba != null && resultadoMetodoCalificacion.ResultadoPrueba.ResultadoPruebaID != null)
            {
                s_VarWHERE.Append(" AND ResultadoPruebaID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = resultadoMetodoCalificacion.ResultadoPrueba.ResultadoPruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoMetodoCalificacion.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = resultadoMetodoCalificacion.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoMetodoCalificacion.TipoResultadoMetodo != null)
            {
                s_VarWHERE.Append(" AND TipoResultadoMetodo = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = resultadoMetodoCalificacion.TipoResultadoMetodo;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (resultadoMetodoCalificacion is ResultadoMetodoPuntos)
            {
                if (((ResultadoMetodoPuntos)resultadoMetodoCalificacion).Puntos != null)
                {
                    s_VarWHERE.Append(" AND Puntos = @dbp4ram5 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram5";
                    sqlParam.Value = ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).Puntos;
                    sqlParam.DbType = DbType.Decimal;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (((ResultadoMetodoPuntos)resultadoMetodoCalificacion).MaximoPuntos != null)
                {
                    s_VarWHERE.Append(" AND MaximoPuntos = @dbp4ram6 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram6";
                    sqlParam.Value = ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).MaximoPuntos;
                    sqlParam.DbType = DbType.Decimal;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EsAproximado != null)
                {
                    s_VarWHERE.Append(" AND EsAproximado = @dbp4ram9 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram9";
                    sqlParam.Value = ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EsAproximado;
                    sqlParam.DbType = DbType.Boolean;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EscalaPuntajeDinamica != null 
                    && ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EscalaPuntajeDinamica.PuntajeID != null)
                {
                    s_VarWHERE.Append(" AND PuntajeID = @dbp4ram10 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram10";
                    sqlParam.Value = ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EscalaPuntajeDinamica.PuntajeID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            if (resultadoMetodoCalificacion is ResultadoMetodoPorcentaje)
            {
                if (((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).NumeroAciertos != null)
                {
                    s_VarWHERE.Append(" AND NumeroAciertos = @dbp4ram7 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram7";
                    sqlParam.Value = ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).NumeroAciertos;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).TotalAciertos != null)
                {
                    s_VarWHERE.Append(" AND TotalAciertos = @dbp4ram8 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram8";
                    sqlParam.Value = ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).TotalAciertos;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EsAproximado != null)
                {
                    s_VarWHERE.Append(" AND EsAproximado = @dbp4ram9 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram9";
                    sqlParam.Value = ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EsAproximado;
                    sqlParam.DbType = DbType.Boolean;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EscalaPorcentajeDinamica != null 
                    && ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EscalaPorcentajeDinamica.PuntajeID != null)
                {
                    s_VarWHERE.Append(" AND PuntajeID = @dbp4ram10 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram10";
                    sqlParam.Value = ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EscalaPorcentajeDinamica.PuntajeID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
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
                sqlCmd.CommandText = sCmd.ToString();
                sqlAdapter.Fill(ds, "ResultadoMetodoCalificacion");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ResultadoMetodoCalificacionRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
