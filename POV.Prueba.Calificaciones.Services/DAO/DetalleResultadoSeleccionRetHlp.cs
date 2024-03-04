using System;
using System.Text;
using System.Data;
using System.Data.Common;
using POV.Prueba.Calificaciones.BO;
using Framework.Base.DataAccess;

namespace POV.Prueba.Calificaciones.DAO
{
	internal class DetalleResultadoSeleccionRetHlp
	{
		public DataSet Action(IDataContext dctx,ResultadoMetodoSeleccion resultadoMetodoSeleccion, DetalleResultadoSeleccion detalleResultadoSeleccion)
		{
            if (resultadoMetodoSeleccion == null)
                resultadoMetodoSeleccion = new ResultadoMetodoSeleccion();
            if (detalleResultadoSeleccion == null)
                detalleResultadoSeleccion = new DetalleResultadoSeleccion();

			object myFirm = new object();
			if (dctx == null)
				throw new Exception("DetalleResultadoSeleccionRetHlp.Action: DataContext no puede ser nulo");
			DbCommand sqlCmd = null;
			try
			{
				dctx.OpenConnection(myFirm);
				sqlCmd = dctx.CreateCommand();
			}
			catch (Exception ex)
			{
				throw new Exception("DetalleResultadoSeleccionRetHlp.Action: Hubo un error al conectarse a la Base de Datos");
			}

			DbParameter sqlParam;
			StringBuilder sCmd = new StringBuilder();
			sCmd.Append(" SELECT DetalleResultadoID,ResultadoMetodoCalificacionID,Valor,EsAproximado,PuntajeID ");
			sCmd.Append(" FROM DetalleResultadoSeleccion ");
			StringBuilder s_VarWHERE = new StringBuilder();
			if (detalleResultadoSeleccion.DetalleResultadoID != null)
			{
				s_VarWHERE.Append(" AND DetalleResultadoID = @dbp4ram1 ");
				sqlParam = sqlCmd.CreateParameter();
				sqlParam.ParameterName = "dbp4ram1";
				sqlParam.Value = detalleResultadoSeleccion.DetalleResultadoID;
				sqlParam.DbType = DbType.Int32;
				sqlCmd.Parameters.Add(sqlParam);
			}
			if (resultadoMetodoSeleccion.ResultadoMetodoCalificacionID != null)
			{
				s_VarWHERE.Append(" AND ResultadoMetodoCalificacionID = @dbp4ram2 ");
				sqlParam = sqlCmd.CreateParameter();
				sqlParam.ParameterName = "dbp4ram2";
				sqlParam.Value = resultadoMetodoSeleccion.ResultadoMetodoCalificacionID;
				sqlParam.DbType = DbType.Int32;
				sqlCmd.Parameters.Add(sqlParam);
			}
			if (detalleResultadoSeleccion.Valor != null)
			{
				s_VarWHERE.Append(" AND Valor = @dbp4ram3 ");
				sqlParam = sqlCmd.CreateParameter();
				sqlParam.ParameterName = "dbp4ram3";
				sqlParam.Value = detalleResultadoSeleccion.Valor;
				sqlParam.DbType = DbType.Decimal;
				sqlCmd.Parameters.Add(sqlParam);
			}
			if (detalleResultadoSeleccion.EsAproximado != null)
			{
				s_VarWHERE.Append(" AND EsAproximado = @dbp4ram4 ");
				sqlParam = sqlCmd.CreateParameter();
				sqlParam.ParameterName = "dbp4ram4";
				sqlParam.Value = detalleResultadoSeleccion.EsAproximado;
				sqlParam.DbType = DbType.Boolean;
				sqlCmd.Parameters.Add(sqlParam);
			}
            if (detalleResultadoSeleccion.EscalaSeleccionDinamica != null && detalleResultadoSeleccion.EscalaSeleccionDinamica.PuntajeID != null)
			{
				s_VarWHERE.Append(" AND PuntajeID = @dbp4ram5 ");
				sqlParam = sqlCmd.CreateParameter();
				sqlParam.ParameterName = "dbp4ram5";
				sqlParam.Value = detalleResultadoSeleccion.EscalaSeleccionDinamica.PuntajeID;
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
				sqlAdapter.Fill(ds, "DetalleResultadoSeleccion");
			}
			catch(Exception ex)
			{
				string exmsg = ex.Message;
				try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
				throw new Exception("DetalleResultadoSeleccionRetHlp: Hubo un error al consultar los registros. " + exmsg);
			}
			finally
			{
				try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
			}
			return ds;

		}
	}
}
