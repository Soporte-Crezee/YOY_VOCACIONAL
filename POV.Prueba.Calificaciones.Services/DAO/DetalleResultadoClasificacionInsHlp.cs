using System;
using System.Text;
using System.Data;
using System.Data.Common;
using POV.Prueba.Calificaciones.BO;
using Framework.Base.DataAccess;

namespace POV.Prueba.Calificaciones.DAO
{
    internal class DetalleResultadoClasificacionInsHlp
    {
        public void Action(IDataContext dctx, ResultadoMetodoClasificacion resultadoMetodoClasificacion, DetalleResultadoClasificacion detalleResultadoClasificacion)
        {
            object myFirm = new object();
            var sError = string.Empty;
            if (resultadoMetodoClasificacion == null)
                sError += ", resultadoMetodoClasificacion";
            if (detalleResultadoClasificacion == null)
                sError += ", detalleResultadoClasificacion";
            if (sError.Length > 0)
                throw new Exception("DetalleResultadoClasificacionInsHlp: Los siguientes objetos no pueden ser vacios: " + sError.Substring(2));
            if (resultadoMetodoClasificacion.ResultadoMetodoCalificacionID == null)
                sError += ", ResultadoMetodoCalificacionID";
            if (detalleResultadoClasificacion.Valor == null)
                sError += ", Valor";
            if (detalleResultadoClasificacion.EsAproximado == null)
                sError += ", EsAproximado";
            if (detalleResultadoClasificacion.EscalaClasificacionDinamica == null || detalleResultadoClasificacion.EscalaClasificacionDinamica.PuntajeID == null)
                sError += ", PuntajeID";
            if (sError.Length > 0)
                throw new Exception("DetalleResultadoClasificacionInsHlp: Los siguientes campos no pueden ser vacios: " + sError.Substring(2));

            if (dctx == null)
                throw new Exception("DetalleResultadoClasificacionInsHlp.Action: DataContext no puede ser nulo");

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception("DetalleResultadoClasificacionInsHlp.Action: Hubo un error al conectarse a la Base de Datos");
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO DetalleResultadoClasificacion(ResultadoMetodoCalificacionID,Valor,EsAproximado,PuntajeID) ");
            sCmd.Append(" VALUES( ");

            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (resultadoMetodoClasificacion.ResultadoMetodoCalificacionID != null)
                sqlParam.Value = resultadoMetodoClasificacion.ResultadoMetodoCalificacionID;
            else
                sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (detalleResultadoClasificacion.Valor != null)
                sqlParam.Value = detalleResultadoClasificacion.Valor;
            else
                sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (detalleResultadoClasificacion.EsAproximado != null)
                sqlParam.Value = detalleResultadoClasificacion.EsAproximado;
            else
                sqlParam.Value = DBNull.Value;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (detalleResultadoClasificacion.EscalaClasificacionDinamica.PuntajeID != null)
                sqlParam.Value = detalleResultadoClasificacion.EscalaClasificacionDinamica.PuntajeID;
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
                throw new Exception("DetalleResultadoClasificacionInsHlp: Hubo un error al insertar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("DetalleResultadoClasificacionInsHlp: Hubo un error al insertar los registros, No se Inserto ninguno");

        }
    }
}
