using System;
using System.Text;
using System.Data;
using System.Data.Common;
using POV.Prueba.Calificaciones.BO;
using POV.Expediente.BO;
using Framework.Base.DataAccess;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Calificaciones.DAO
{
    internal class ResultadoMetodoCalificacionInsHlp
    {
        public void Action(IDataContext dctx, AResultadoMetodoCalificacion resultadoMetodoCalificacion)
        {
            object myFirm = new object();
            var sError = string.Empty;
            if (resultadoMetodoCalificacion == null)
                sError += ", resultadoMetodoCalificacion";
            if (resultadoMetodoCalificacion.ResultadoPrueba == null || resultadoMetodoCalificacion.ResultadoPrueba.ResultadoPruebaID == null)
                sError += ", ResultadoPruebaID";
            if (resultadoMetodoCalificacion.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (resultadoMetodoCalificacion.TipoResultadoMetodo == null)
                sError += ", TipoResultadoMetodo";
            if (resultadoMetodoCalificacion is ResultadoMetodoPuntos)
            {
                if (((ResultadoMetodoPuntos)resultadoMetodoCalificacion).Puntos == null)
                    sError += ", Puntos";
                if (((ResultadoMetodoPuntos)resultadoMetodoCalificacion).MaximoPuntos == null)
                    sError += ", MaximoPuntos";
                if (((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EsAproximado == null)
                    sError += ", EsAproximado";
                if (((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EscalaPuntajeDinamica == null 
                    || ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EscalaPuntajeDinamica.PuntajeID == null)
                    sError += ", PuntajeID";
            }
            if ((resultadoMetodoCalificacion is ResultadoMetodoPorcentaje))
            {
                if (((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).NumeroAciertos == null)
                    sError += ", NumeroAciertos";
                if (((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).TotalAciertos == null)
                    sError += ", TotalAciertos";
                if (((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EsAproximado == null)
                    sError += ", EsAproximado";
                if (((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EscalaPorcentajeDinamica == null 
                    || ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EscalaPorcentajeDinamica.PuntajeID == null)
                    sError += ", PuntajeID";
            }
            if (sError.Length > 0)
                throw new Exception("ResultadoMetodoCalificacionInsHlp: Los siguientes campos no pueden ser vacios: " + sError.Substring(2));

            if (dctx == null)
                throw new Exception("ResultadoMetodoCalificacionInsHlp.Action: DataContext no puede ser nulo");

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception("ResultadoMetodoCalificacionInsHlp.Action: Hubo un error al conectarse a la Base de Datos");
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO ResultadoMetodoCalificacion(ResultadoPruebaID,FechaRegistro,TipoResultadoMetodo,Puntos,MaximoPuntos,NumeroAciertos,TotalAciertos,EsAproximado,PuntajeID) ");
            sCmd.Append(" VALUES( ");
            
            // resultadoMetodoCalificacion.ResultadoPrueba.ResultadoPruebaID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (resultadoMetodoCalificacion.ResultadoPrueba.ResultadoPruebaID != null)
                sqlParam.Value = resultadoMetodoCalificacion.ResultadoPrueba.ResultadoPruebaID;
            else
                sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            // resultadoMetodoCalificacion.FechaRegistro
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (resultadoMetodoCalificacion.FechaRegistro != null)
                sqlParam.Value = resultadoMetodoCalificacion.FechaRegistro;
            else
                sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);

            // resultadoMetodoCalificacion.TipoResultadoMetodo
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (resultadoMetodoCalificacion.TipoResultadoMetodo != null)
                sqlParam.Value = resultadoMetodoCalificacion.TipoResultadoMetodo;
            else
                sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);

            // resultadoMetodoCalificacion.Puntos
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if ((resultadoMetodoCalificacion is ResultadoMetodoPuntos) && ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).Puntos != null)
                sqlParam.Value = ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).Puntos;
            else
                sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);

            // resultadoMetodoCalificacion.MaximoPuntos
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if ((resultadoMetodoCalificacion is ResultadoMetodoPuntos) && ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).MaximoPuntos != null)
                sqlParam.Value = ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).MaximoPuntos;
            else
                sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);

            // resultadoMetodoCalificacion).NumeroAciertos
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if ((resultadoMetodoCalificacion is ResultadoMetodoPorcentaje) && ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).NumeroAciertos != null)
                sqlParam.Value = ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).NumeroAciertos;
            else
                sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            // resultadoMetodoCalificacion.TotalAciertos
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if ((resultadoMetodoCalificacion is ResultadoMetodoPorcentaje) && ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).TotalAciertos != null)
                sqlParam.Value = ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).TotalAciertos;
            else
                sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            // resultadoMetodoCalificacion).EsAproximado
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if ((resultadoMetodoCalificacion is ResultadoMetodoPuntos) && ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EsAproximado != null)
                sqlParam.Value = ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EsAproximado;
            else
                if ((resultadoMetodoCalificacion is ResultadoMetodoPorcentaje) && ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EsAproximado != null)
                    sqlParam.Value = ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EsAproximado;
                else
                    sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);

            // resultadoMetodoCalificacion.EscalaPuntajeDinamica.PuntajeID
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if ((resultadoMetodoCalificacion is ResultadoMetodoPuntos) && ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EscalaPuntajeDinamica.PuntajeID != null)
                sqlParam.Value = ((ResultadoMetodoPuntos)resultadoMetodoCalificacion).EscalaPuntajeDinamica.PuntajeID;
            else
                if ((resultadoMetodoCalificacion is ResultadoMetodoPorcentaje) && ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EscalaPorcentajeDinamica.PuntajeID != null)
                    sqlParam.Value = ((ResultadoMetodoPorcentaje)resultadoMetodoCalificacion).EscalaPorcentajeDinamica.PuntajeID;
                else
                    sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ) ");
            int iRes = 0;
            try
            {
                sqlCmd.CommandText = sCmd.ToString();
                iRes = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ResultadoMetodoCalificacionInsHlp: Hubo un error al insertar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ResultadoMetodoCalificacionInsHlp: Hubo un error al insertar los registros, No se Insertó ninguno");

        }
    }
}
