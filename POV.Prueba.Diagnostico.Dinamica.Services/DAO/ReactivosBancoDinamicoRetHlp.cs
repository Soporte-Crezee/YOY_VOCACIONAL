using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.BO;
using POV.Reactivos.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    /// <summary>
    /// Consulta un registro de ReactivosBancoDinamico en la BD
    /// </summary>
    internal class ReactivosBancoDinamicoRetHlp
    {
        /// <summary>
        /// Consulta registros de ReactivoBancoDinamico en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="reactivoBancoDinamico">ReactivoBancoDinamico que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ReactivoBancoDinamico generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, BancoReactivosDinamico bancoReactivosDinamico, ReactivoBanco reactivoBanco)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (bancoReactivosDinamico == null)
                sError += ", BancoReactivosDinamico";
            if (sError.Length > 0)
                throw new Exception("ReactivosBancoDinamicoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (reactivoBanco == null)
                sError += ", ReactivoBanco";
            if (sError.Length > 0)
                throw new Exception("ReactivosBancoDinamicoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "ReactivosBancoDinamicoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ReactivosBancoDinamicoRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "ReactivosBancoDinamicoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ReactivoBancoID,BancoReactivoID,ReactivoID,Activo,Orden,EstaSeleccionado,ReactivoOriginalID ");
            sCmd.Append(" FROM ReactivosBancoReactivosDinamico ");
            sCmd.Append(" WHERE ");
            StringBuilder s_Var = new StringBuilder();
            if (reactivoBanco.ReactivoBancoID != null)
            {
                s_Var.Append(" ReactivoBancoID = @dbp4ram21 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram21";
                sqlParam.Value = reactivoBanco.ReactivoBancoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (bancoReactivosDinamico.BancoReactivoID != null)
            {
                s_Var.Append(" AND BancoReactivoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = bancoReactivosDinamico.BancoReactivoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivoBanco.Reactivo != null)
                if (reactivoBanco.Reactivo.ReactivoID != null)
                {
                    s_Var.Append(" AND ReactivoID = @dbp4ram2 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram2";
                    sqlParam.Value = reactivoBanco.Reactivo.ReactivoID;
                    sqlParam.DbType = DbType.Guid;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            if (reactivoBanco.Activo != null)
            {
                s_Var.Append(" AND Activo = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = reactivoBanco.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivoBanco.Orden != null)
            {
                s_Var.Append(" AND Orden = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = reactivoBanco.Orden;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivoBanco.EstaSeleccionado != null)
            {
                s_Var.Append(" AND EstaSeleccionado = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = reactivoBanco.EstaSeleccionado;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivoBanco.ReactivoOriginal != null)
            {
                if (reactivoBanco.ReactivoOriginal.ReactivoID != null)
                {
                    s_Var.Append(" AND ReactivoOriginalID = @dbp4ram51 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram51";
                    sqlParam.Value = reactivoBanco.ReactivoOriginal.ReactivoID;
                    sqlParam.DbType = DbType.Guid;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            s_Var.Append("  ");
            string s_Varres = s_Var.ToString().Trim();
            if (s_Varres.Length > 0)
            {
                if (s_Varres.StartsWith("AND "))
                    s_Varres = s_Varres.Substring(4);
                else if (s_Varres.StartsWith("OR "))
                    s_Varres = s_Varres.Substring(3);
                else if (s_Varres.StartsWith(","))
                    s_Varres = s_Varres.Substring(1);
                sCmd.Append("  " + s_Varres);
            }
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "ReactivosBancoDinamico");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ReactivosBancoDinamicoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
