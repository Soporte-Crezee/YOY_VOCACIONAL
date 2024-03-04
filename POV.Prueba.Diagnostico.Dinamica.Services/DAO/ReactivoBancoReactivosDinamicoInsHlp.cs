using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
	/// <summary>
	/// Guarda un registro de Reactivo-BancoReactivosDinamico en la base de datos
	/// </summary>
	internal class ReactivoBancoReactivosDinamicoInsHlp
	{
		public void Action(IDataContext dctx, BancoReactivosDinamico bancoReactivos, ReactivoBanco reactivoBanco)
		{
			object myFirm = new object();
			string sError = String.Empty;
			if (bancoReactivos == null)
				sError += ", bancoReactivos";
			if (sError.Length > 0)
				throw new Exception("ReactivoBancoReactivosDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
			if (bancoReactivos.BancoReactivoID == null)
				sError += ", bancoReactivos.bancoReactivoID";
			if (sError.Length > 0)
				throw new Exception("ReactivoBancoReactivosDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
			if (reactivoBanco.Reactivo == null)
				sError += ", reactivoBanco.Reactivo";
			if (sError.Length > 0)
				throw new Exception("ReactivoBancoReactivosDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
			if (reactivoBanco.Reactivo.ReactivoID == null)
				sError += ", reactivoBanco.Reactivo.ReactivoID";
			if (sError.Length > 0)
				throw new Exception("ReactivoBancoReactivosDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
			if (reactivoBanco.ReactivoOriginal == null)
				sError += ", reactivoBanco.ReactivoOriginal";
			if (sError.Length > 0)
				throw new Exception("ReactivoBancoReactivosEstandarizadoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
			if (reactivoBanco.ReactivoOriginal.ReactivoID == null)
				sError += ", reactivoBanco.ReactivoOriginal.ReactivoID";
			if (reactivoBanco.Orden == null)
				sError += ", reactivoBanco.Orden";
			if (reactivoBanco.EstaSeleccionado == null)
				sError += ", reactivoBanco.EstaSeleccionado";
			if (reactivoBanco.Activo == null)
				sError += ", reactivoBanco.Activo";
			if (sError.Length > 0)
				throw new Exception("ReactivoBancoReactivosDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
			if (dctx == null)
				throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
				   "ReactivoBancoReactivosDinamicoInsHlp", "Action", null, null);
			DbCommand sqlCmd = null;
			try
			{
				dctx.OpenConnection(myFirm);
				sqlCmd = dctx.CreateCommand();
			}
			catch
			{
				throw new StandardException(MessageType.Error, "", "ReactivoBancoReactivosDinamicoInsHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
				   "ReactivoBancoReactivosDinamicoInsHlp", "Action", null, null);
			}
			DbParameter sqlParam;
			StringBuilder sCmd = new StringBuilder();
			sCmd.Append(" INSERT INTO ReactivosBancoReactivosDinamico (BancoReactivoID, ReactivoID, ReactivoOriginalID, Orden, EstaSeleccionado, Activo) ");
			sCmd.Append(" VALUES ");
			sCmd.Append(" ( ");
			// bancoReactivos.bancoReactivoID
			sCmd.Append(" @dbp4ram1 ");
			sqlParam = sqlCmd.CreateParameter();
			sqlParam.ParameterName = "dbp4ram1";
			if (bancoReactivos.BancoReactivoID == null)
				sqlParam.Value = DBNull.Value;
			else
				sqlParam.Value = bancoReactivos.BancoReactivoID;
			sqlParam.DbType = DbType.Int32;
			sqlCmd.Parameters.Add(sqlParam);
			// reactivoBanco.Reactivo.ReactivoID
			sCmd.Append(" ,@dbp4ram2 ");
			sqlParam = sqlCmd.CreateParameter();
			sqlParam.ParameterName = "dbp4ram2";
			if (reactivoBanco.Reactivo.ReactivoID == null)
				sqlParam.Value = DBNull.Value;
			else
				sqlParam.Value = reactivoBanco.Reactivo.ReactivoID;
			sqlParam.DbType = DbType.Guid;
			sqlCmd.Parameters.Add(sqlParam);
			// reactivoBanco.ReactivoOriginal.ReactivoID
			sCmd.Append(" ,@dbp4ram3 ");
			sqlParam = sqlCmd.CreateParameter();
			sqlParam.ParameterName = "dbp4ram3";
			if (reactivoBanco.ReactivoOriginal.ReactivoID == null)
				sqlParam.Value = DBNull.Value;
			else
				sqlParam.Value = reactivoBanco.ReactivoOriginal.ReactivoID;
			sqlParam.DbType = DbType.Guid;
			sqlCmd.Parameters.Add(sqlParam);
			// reactivoBanco.Orden
			sCmd.Append(" ,@dbp4ram4 ");
			sqlParam = sqlCmd.CreateParameter();
			sqlParam.ParameterName = "dbp4ram4";
			if (reactivoBanco.Orden == null)
				sqlParam.Value = DBNull.Value;
			else
				sqlParam.Value = reactivoBanco.Orden;
			sqlParam.DbType = DbType.Int32;
			sqlCmd.Parameters.Add(sqlParam);
			// reactivoBanco.EstaSeleccionado
			sCmd.Append(" ,@dbp4ram5 ");
			sqlParam = sqlCmd.CreateParameter();
			sqlParam.ParameterName = "dbp4ram5";
			if (reactivoBanco.EstaSeleccionado == null)
				sqlParam.Value = DBNull.Value;
			else
				sqlParam.Value = reactivoBanco.EstaSeleccionado;
			sqlParam.DbType = DbType.Boolean;
			sqlCmd.Parameters.Add(sqlParam);
			// reactivoBanco.Activo
			sCmd.Append(" ,@dbp4ram6 ");
			sqlParam = sqlCmd.CreateParameter();
			sqlParam.ParameterName = "dbp4ram6";
			if (reactivoBanco.Activo == null)
				sqlParam.Value = DBNull.Value;
			else
				sqlParam.Value = reactivoBanco.Activo;
			sqlParam.DbType = DbType.Boolean;
			sqlCmd.Parameters.Add(sqlParam);
			sCmd.Append(" ) ");
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
				throw new Exception("ReactivoBancoReactivosDinamicoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
			}
			finally
			{
				try { dctx.CloseConnection(myFirm); }
				catch (Exception) { }
			}
			if (iRes < 1)
				throw new Exception("ReactivoBancoReactivosDinamicoInsHlp: Ocurrió un error al ingresar el registro.");
		}
	}
}
