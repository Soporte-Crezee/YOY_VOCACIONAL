using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO
{
    /// <summary>
    /// Consulta un registro de AAgrupadorContenidoDigital en la BD
    /// </summary>
    internal class AgrupadorContenidoDigitalRetHlp
    {
        /// <summary>
        /// Consulta registros de AAgrupadorContenidoDigital en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aAgrupadorContenidoDigital">AAgrupadorContenidoDigital que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AAgrupadorContenidoDigital generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (aAgrupadorContenidoDigital == null)
                sError += ", AAgrupadorContenidoDigital";
            if (sError.Length > 0)
                throw new Exception("AgrupadorContenidoDigitalRetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "AgrupadorContenidoDigitalRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AgrupadorContenidoDigitalRetHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO",
                   "AgrupadorContenidoDigitalRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT AgrupadorContenidoDigitalID, AgrupadorPadreID, Nombre, EsPredeterminado, FechaRegistro, EstatusProfesionalizacion, TipoAgrupador, Competencias, Aprendizajes ");
            sCmd.Append(" FROM AgrupadorContenidoDigital ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (aAgrupadorContenidoDigital.AgrupadorContenidoDigitalID != null)
            {
                s_VarWHERE.Append(" AgrupadorContenidoDigitalID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = aAgrupadorContenidoDigital.AgrupadorContenidoDigitalID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (aAgrupadorContenidoDigital.Nombre != null)
            {
                s_VarWHERE.Append(" AND Nombre LIKE @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = aAgrupadorContenidoDigital.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (aAgrupadorContenidoDigital.EsPredeterminado != null)
            {
                s_VarWHERE.Append(" AND EsPredeterminado = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = aAgrupadorContenidoDigital.EsPredeterminado;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (aAgrupadorContenidoDigital.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = aAgrupadorContenidoDigital.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (aAgrupadorContenidoDigital.Estatus != null)
            {
                s_VarWHERE.Append(" AND EstatusProfesionalizacion = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = aAgrupadorContenidoDigital.Estatus;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            else
            {
                s_VarWHERE.Append(string.Format(" AND EstatusProfesionalizacion IN ({0},{1}) ",
                    (byte)EEstatusProfesionalizacion.ACTIVO, (byte)EEstatusProfesionalizacion.MANTENIMIENTO));
            }
            if (aAgrupadorContenidoDigital.Competencias != null)
            {
                s_VarWHERE.Append(" AND Competencias LIKE @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = aAgrupadorContenidoDigital.Competencias;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (aAgrupadorContenidoDigital.Aprendizajes != null)
            {
                s_VarWHERE.Append(" AND Aprendizajes LIKE @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = aAgrupadorContenidoDigital.Aprendizajes;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }

            // Tipo de Agrupador
            s_VarWHERE.Append(" AND TipoAgrupador = @dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            sqlParam.Value = aAgrupadorContenidoDigital.TipoAgrupador;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);

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
                sqlAdapter.Fill(ds, "AgrupadorContenidoDigital");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AgrupadorContenidoDigitalRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }
        /// <summary>
        /// Consulta registros de agrupadorContenidoDigitalID en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorPadreID">agrupadorContenidoDigitalID que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de agrupadorContenidoDigitalID generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Int64? agrupadorPadreID)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (agrupadorPadreID == null)
                sError += ", agrupadorContenidoDigitalID";
            if (sError.Length > 0)
                throw new Exception("AgrupadorContenidoDigitalRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "AgrupadorContenidoDigitalRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AgrupadorContenidoDigitalRetHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO",
                   "AgrupadorContenidoDigitalRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT AgrupadorContenidoDigitalID, AgrupadorPadreID, Nombre, EsPredeterminado, FechaRegistro, EstatusProfesionalizacion, TipoAgrupador, Competencias, Aprendizajes ");
            sCmd.Append(" FROM AgrupadorContenidoDigital ");
            sCmd.Append(" WHERE ");
            StringBuilder s_Var = new StringBuilder();

            if (agrupadorPadreID != null)
            {
                s_Var.Append(" AgrupadorPadreID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = agrupadorPadreID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }

            s_Var.Append(string.Format("AND EstatusProfesionalizacion IN ({0},{1}) ",
                (byte)EEstatusProfesionalizacion.ACTIVO, (byte)EEstatusProfesionalizacion.MANTENIMIENTO));

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
                sqlAdapter.Fill(ds, "AgrupadorContenidoDigital");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AgrupadorContenidoDigitalRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
