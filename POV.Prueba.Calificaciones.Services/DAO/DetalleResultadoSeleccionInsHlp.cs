using System;
using System.Text;
using System.Data;
using System.Data.Common;
using POV.Prueba.Calificaciones.BO;
using Framework.Base.DataAccess;

namespace POV.Prueba.Calificaciones.DAO
{
	internal class DetalleResultadoSeleccionInsHlp
	{
		public void Action(IDataContext dctx,ResultadoMetodoSeleccion resultadoMetodoSeleccion, DetalleResultadoSeleccion detalleResultadoSeleccion)
		{
			object myFirm = new object();
			var sError = string.Empty;
            if (resultadoMetodoSeleccion== null)
                sError += ", resultadoMetodoSeleccion";
            if (detalleResultadoSeleccion == null)
				sError += ", detalleResultadoSeleccion";
            if (sError.Length > 0)
                throw new Exception("DetalleResultadoSeleccionInsHlp: Los siguientes objetos no pueden ser vacios: " + sError.Substring(2));
            if (resultadoMetodoSeleccion.ResultadoMetodoCalificacionID == null)
				sError += ", ResultadoMetodoCalificacionID";
			if (detalleResultadoSeleccion.Valor == null)
				sError += ", Valor";
			if (detalleResultadoSeleccion.EsAproximado == null)
				sError += ", EsAproximado";
            if (detalleResultadoSeleccion.EscalaSeleccionDinamica == null || detalleResultadoSeleccion.EscalaSeleccionDinamica.PuntajeID == null)
				sError += ", EscalaSeleccionDinamica.PuntajeID";
			if (sError.Length > 0)
				throw new Exception("DetalleResultadoSeleccionInsHlp: Los siguientes campos no pueden ser vacios: " + sError.Substring(2));

			if (dctx == null)
				throw new Exception("DetalleResultadoSeleccionInsHlp.Action: DataContext no puede ser nulo");

			DbCommand sqlCmd = null;
			try
			{
				dctx.OpenConnection(myFirm);
				sqlCmd = dctx.CreateCommand();
			}
			catch (Exception ex)
			{
				throw new Exception("DetalleResultadoSeleccionInsHlp.Action: Hubo un error al conectarse a la Base de Datos");
			}

			DbParameter sqlParam;
			StringBuilder sCmd = new StringBuilder();
			sCmd.Append(" INSERT INTO DetalleResultadoSeleccion(ResultadoMetodoCalificacionID,Valor,EsAproximado,PuntajeID) ");
			sCmd.Append(" VALUES( ");

			sCmd.Append(" @dbp4ram1 ");
			sqlParam = sqlCmd.CreateParameter();
			sqlParam.ParameterName = "dbp4ram1";
			if (resultadoMetodoSeleccion.ResultadoMetodoCalificacionID != null)
				sqlParam.Value = resultadoMetodoSeleccion.ResultadoMetodoCalificacionID;
			else
				sqlParam.Value = DBNull.Value;
			sqlParam.DbType = DbType.Int32;
			sqlCmd.Parameters.Add(sqlParam);

			sCmd.Append(" ,@dbp4ram2 ");
			sqlParam = sqlCmd.CreateParameter();
			sqlParam.ParameterName = "dbp4ram2";
			if (detalleResultadoSeleccion.Valor != null)
				sqlParam.Value = detalleResultadoSeleccion.Valor;
			else
				sqlParam.Value = DBNull.Value;
			sqlParam.DbType = DbType.Decimal;
			sqlCmd.Parameters.Add(sqlParam);

			sCmd.Append(" ,@dbp4ram3 ");
			sqlParam = sqlCmd.CreateParameter();
			sqlParam.ParameterName = "dbp4ram3";
			if (detalleResultadoSeleccion.EsAproximado != null)
				sqlParam.Value = detalleResultadoSeleccion.EsAproximado;
			else
				sqlParam.Value = DBNull.Value;
			sqlParam.DbType = DbType.Boolean;
			sqlCmd.Parameters.Add(sqlParam);

			sCmd.Append(" ,@dbp4ram4 ");
			sqlParam = sqlCmd.CreateParameter();
			sqlParam.ParameterName = "dbp4ram4";
			if (detalleResultadoSeleccion.EscalaSeleccionDinamica.PuntajeID != null)
				sqlParam.Value = detalleResultadoSeleccion.EscalaSeleccionDinamica.PuntajeID;
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
			catch(Exception ex)
			{
				string exmsg = ex.Message;
				try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
				throw new Exception("DetalleResultadoSeleccionInsHlp: Hubo un error al insertar los registros. " + exmsg);
			}
			finally
			{
				try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
			}
			if (iRes < 1)
				throw new Exception("DetalleResultadoSeleccionInsHlp: Hubo un error al insertar los registros, No se Inserto ninguno");

		}
	}
}
