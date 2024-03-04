using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// Consultar los Planes Educativos de la base de datos
    /// </summary>
    public class PlanEducativoRetHlp
    {
        /// <summary>
        /// Consulta registros de Mensaje en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="mensaje">Mensaje que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Mensaje generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, PlanEducativo planEducativo)
        {
            object myFirm = new object();
            string sError = "";
            if (planEducativo == null)
                sError += ", PlanEducativo";
            if (sError.Length > 0)
                throw new Exception("PlanEducativoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (planEducativo.NivelEducativo == null)
            {
                planEducativo.NivelEducativo = new NivelEducativo();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO","PlanEducativoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PlanEducativoRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO","PlanEducativoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT PLANEDUCATIVOID,TITULO,DESCRIPCION,VALIDODESDE,VALIDOHASTA,NIVELEDUCATIVOID, ESTATUS ");
            sCmd.Append(" FROM PLANEDUCATIVO ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (planEducativo.PlanEducativoID != null)
            {
                s_VarWHERE.Append(" PlanEducativoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = planEducativo.PlanEducativoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (planEducativo.Titulo != null)
            {
                s_VarWHERE.Append(" AND Titulo = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = planEducativo.Titulo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (planEducativo.Descripcion != null)
            {
                s_VarWHERE.Append(" AND Descripcion = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = planEducativo.Descripcion;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (planEducativo.ValidoDesde != null)
            {
                s_VarWHERE.Append(" AND ValidoDesde = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = planEducativo.ValidoDesde;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (planEducativo.ValidoHasta != null)
            {
                s_VarWHERE.Append(" AND ValidoHasta = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = planEducativo.ValidoHasta;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (planEducativo.NivelEducativo.NivelEducativoID != null)
            {
                s_VarWHERE.Append(" AND NivelEducativoID = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = planEducativo.NivelEducativo.NivelEducativoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (planEducativo.Estatus != null)
            {
                s_VarWHERE.Append(" AND Estatus = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = planEducativo.Estatus;
                sqlParam.DbType = DbType.Boolean;
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
            sCmd.Append(" ORDER BY PlanEducativoID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "PlanEducativo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("PlanEducativoRetHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }

        /// <summary>
        /// Consulta el PlanEducativo
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="fecha">Criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de PlanEducativo generada por la consulta</returns>
        public DataSet ActionActual(IDataContext dctx, DateTime fecha)
        {
            object myFirm = new object();

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "PlanEducativoRetHlp", "ActionActual", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PlanEducativoRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "PlanEducativoRetHlp", "ActionActual", null, null);
            }

            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT PLANEDUCATIVOID,TITULO,DESCRIPCION,VALIDODESDE,VALIDOHASTA,NIVELEDUCATIVOID, Estatus ");
            sCmd.Append(" FROM    PLANEDUCATIVO ");
            sCmd.Append(" WHERE  VALIDODESDE <= @dbp4ram1 AND @dbp4ram1 <= VALIDOHASTA ");
            sCmd.Append(" ORDER BY VALIDODESDE ASC ");

            DbParameter sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "@dbp4ram1";
            sqlParam.Value = fecha;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);

            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;

            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "PlanEducativo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("PlanEducativoRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
