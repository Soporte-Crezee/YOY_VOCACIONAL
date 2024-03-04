using System;
using System.Text;
using System.Data;
using System.Data.Common;
using POV.Prueba.Calificaciones.BO;
using Framework.Base.DataAccess;

namespace POV.Prueba.Calificaciones.DAO
{
	internal class DetalleResultadoClasificacionRetHlp
	{
		public DataSet Action(IDataContext dctx,ResultadoMetodoClasificacion resultadoMetodoClasificacion, DetalleResultadoClasificacion detalleResultadoClasificacion)
		{
            if (resultadoMetodoClasificacion == null)
                resultadoMetodoClasificacion = new ResultadoMetodoClasificacion();
            if (detalleResultadoClasificacion == null)
                detalleResultadoClasificacion = new DetalleResultadoClasificacion();

			object myFirm = new object();
			if (dctx == null)
				throw new Exception("DetalleResultadoClasificacionRetHlp.Action: DataContext no puede ser nulo");
			DbCommand sqlCmd = null;
			try
			{
				dctx.OpenConnection(myFirm);
				sqlCmd = dctx.CreateCommand();
			}
			catch (Exception ex)
			{
				throw new Exception("DetalleResultadoClasificacionRetHlp.Action: Hubo un error al conectarse a la Base de Datos");
			}

			DbParameter sqlParam;
			StringBuilder sCmd = new StringBuilder();
			sCmd.Append(" SELECT DetalleResultadoID,ResultadoMetodoCalificacionID,Valor,EsAproximado,PuntajeID ");
			sCmd.Append(" FROM DetalleResultadoClasificacion ");
			StringBuilder s_VarWHERE = new StringBuilder();
			if (detalleResultadoClasificacion.DetalleResultadoID != null)
			{
				s_VarWHERE.Append(" AND DetalleResultadoID = @dbp4ram1 ");
				sqlParam = sqlCmd.CreateParameter();
				sqlParam.ParameterName = "dbp4ram1";
				sqlParam.Value = detalleResultadoClasificacion.DetalleResultadoID;
				sqlParam.DbType = DbType.Int32;
				sqlCmd.Parameters.Add(sqlParam);
			}
			if (resultadoMetodoClasificacion.ResultadoMetodoCalificacionID != null)
			{
				s_VarWHERE.Append(" AND ResultadoMetodoCalificacionID = @dbp4ram2 ");
				sqlParam = sqlCmd.CreateParameter();
				sqlParam.ParameterName = "dbp4ram2";
				sqlParam.Value = resultadoMetodoClasificacion.ResultadoMetodoCalificacionID;
				sqlParam.DbType = DbType.Int32;
				sqlCmd.Parameters.Add(sqlParam);
			}
			if (detalleResultadoClasificacion.Valor != null)
			{
				s_VarWHERE.Append(" AND Valor = @dbp4ram3 ");
				sqlParam = sqlCmd.CreateParameter();
				sqlParam.ParameterName = "dbp4ram3";
				sqlParam.Value = detalleResultadoClasificacion.Valor;
				sqlParam.DbType = DbType.Decimal;
				sqlCmd.Parameters.Add(sqlParam);
			}
			if (detalleResultadoClasificacion.EsAproximado != null)
			{
				s_VarWHERE.Append(" AND EsAproximado = @dbp4ram4 ");
				sqlParam = sqlCmd.CreateParameter();
				sqlParam.ParameterName = "dbp4ram4";
				sqlParam.Value = detalleResultadoClasificacion.EsAproximado;
				sqlParam.DbType = DbType.Boolean;
				sqlCmd.Parameters.Add(sqlParam);
			}
            if (detalleResultadoClasificacion.EscalaClasificacionDinamica != null && detalleResultadoClasificacion.EscalaClasificacionDinamica.PuntajeID != null)
			{
				s_VarWHERE.Append(" AND PuntajeID = @dbp4ram5 ");
				sqlParam = sqlCmd.CreateParameter();
				sqlParam.ParameterName = "dbp4ram5";
				sqlParam.Value = detalleResultadoClasificacion.EscalaClasificacionDinamica.PuntajeID;
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
				sqlCmd.CommandText = sCmd.ToString();
				sqlAdapter.Fill(ds, "DetalleResultadoClasificacion");
			}
			catch(Exception ex)
			{
				string exmsg = ex.Message;
				try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
				throw new Exception("DetalleResultadoClasificacionRetHlp: Hubo un error al consultar los registros. " + exmsg);
			}
			finally
			{
				try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
			}
			return ds;

		}
	}
}
