using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Expediente.BO;
using POV.Expediente.DAO;

namespace POV.Expediente.DAO
{
    /// <summary>
    /// Consulta un registro de AAsignacionRecurso en la BD
    /// </summary>
    internal class AsignacionRecursoRetHlp
    {
        /// <summary>
        /// Consulta registros de AAsignacionRecurso en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAsignacionRecurso">AAsignacionRecurso que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AAsignacionRecurso generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, AAsignacionRecurso asignacionRecurso)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (asignacionRecurso == null)
                sError += ", AAsignacionRecurso";
            if (sError.Length > 0)
                throw new Exception("AsignacionRecursoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (detalleCicloEscolar == null)
                sError += ", DetalleCicloEscolar";
            if (sError.Length > 0)
                throw new Exception("AsignacionRecursoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Expediente.DAO",
                   "AsignacionRecursoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionRecursoRetHlp: No se pudo conectar a la base de datos", "POV.Expediente.DAO",
                   "AsignacionRecursoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT AsignacionRecursoID, DetalleCicloEscolarID, FechaRegistro, Tipo, EstaConfirmado ");
            sCmd.Append(" FROM AsignacionRecurso ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (asignacionRecurso.AsignacionRecursoID != null)
            {
                s_VarWHERE.Append(" AsignacionRecursoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = asignacionRecurso.AsignacionRecursoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (detalleCicloEscolar.DetalleCicloEscolarID != null)
            {
                s_VarWHERE.Append(" AND DetalleCicloEscolarID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = detalleCicloEscolar.DetalleCicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacionRecurso.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = asignacionRecurso.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacionRecurso.TipoAsignacionRecurso != null)
            {
                s_VarWHERE.Append(" AND Tipo = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = asignacionRecurso.TipoAsignacionRecurso;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacionRecurso.EstaConfirmado != null)
            {
                s_VarWHERE.Append(" AND EstaConfirmado = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = asignacionRecurso.EstaConfirmado;
                sqlParam.DbType = DbType.Int64;
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
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "AsignacionRecurso");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AsignacionRecursoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }

        public DataSet Action(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar)
        {
            object myFirm = new object();
            string sError = String.Empty;
            
            
            if (detalleCicloEscolar == null)
                sError += ", DetalleCicloEscolar";
            if (sError.Length > 0)
                throw new Exception("AsignacionRecursoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Expediente.DAO",
                   "AsignacionRecursoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionRecursoRetHlp: No se pudo conectar a la base de datos", "POV.Expediente.DAO",
                   "AsignacionRecursoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT AsignacionRecursoID, DetalleCicloEscolarID, FechaRegistro, Tipo, EstaConfirmado ");
            sCmd.Append(" FROM AsignacionRecurso ");
            StringBuilder s_VarWHERE = new StringBuilder();
            
            if (detalleCicloEscolar.DetalleCicloEscolarID != null)
            {
                s_VarWHERE.Append(" DetalleCicloEscolarID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = detalleCicloEscolar.DetalleCicloEscolarID;
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
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "AsignacionRecurso");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AsignacionRecursoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
