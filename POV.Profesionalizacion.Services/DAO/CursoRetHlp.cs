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
    /// Consulta un registro de Curso en la BD
    /// </summary>
    internal class CursoRetHlp
    {
        /// <summary>
        /// Consulta registros de Curso en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenido">Curso que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Curso generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (agrupadorContenido == null)
                sError += ", Curso";
            if (sError.Length > 0)
                throw new Exception("CursoRetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "CursoRetHlp", "Action", null, null);

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "CursoRetHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO",
                   "CursoRetHlp", "Action", null, null);
            }

            if (agrupadorContenido is Curso)
                if (((Curso)agrupadorContenido).TemaCurso == null)
                    ((Curso)agrupadorContenido).TemaCurso = new TemaCurso();

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT AgrupadorContenidoDigitalID, AgrupadorPadreID, Nombre, EsPredeterminado, FechaRegistro, EstatusProfesionalizacion, TipoAgrupador, TemaCursoID, Presencial, Informacion ");
            sCmd.Append(" FROM Curso ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (agrupadorContenido.AgrupadorContenidoDigitalID != null)
            {
                s_VarWHERE.Append(" AgrupadorContenidoDigitalID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = agrupadorContenido.AgrupadorContenidoDigitalID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorContenido.Nombre != null)
            {
                s_VarWHERE.Append(" AND Nombre LIKE @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = agrupadorContenido.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorContenido.EsPredeterminado != null)
            {
                s_VarWHERE.Append(" AND EsPredeterminado = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = agrupadorContenido.EsPredeterminado;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorContenido.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = agrupadorContenido.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorContenido.Estatus != null)
            {
                s_VarWHERE.Append(" AND EstatusProfesionalizacion = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = agrupadorContenido.Estatus;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            else
            {
                s_VarWHERE.Append(string.Format(" AND EstatusProfesionalizacion IN ({0},{1}) ",
                    (byte)EEstatusProfesionalizacion.ACTIVO, (byte)EEstatusProfesionalizacion.MANTENIMIENTO));
            }
            if (agrupadorContenido.TipoAgrupador != null)
            {
                s_VarWHERE.Append(" AND TipoAgrupador = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = agrupadorContenido.TipoAgrupador;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (agrupadorContenido is Curso)
            {
                if (((Curso)agrupadorContenido).TemaCurso.TemaCursoID != null)
                {
                    s_VarWHERE.Append(" AND TemaCursoID = @dbp4ram7 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram7";
                    sqlParam.Value = ((Curso)agrupadorContenido).TemaCurso.TemaCursoID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (((Curso)agrupadorContenido).Presencial != null)
                {
                    s_VarWHERE.Append(" AND Presencial = @dbp4ram8 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram8";
                    sqlParam.Value = ((Curso)agrupadorContenido).Presencial;
                    sqlParam.DbType = DbType.Byte;
                    sqlCmd.Parameters.Add(sqlParam);
                }
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
                sqlAdapter.Fill(ds, "Curso");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("CursoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }
        /// <summary>
        /// Consulta los Registros Hijos de un Curso en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorPadreID">agrupadorPadreID que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de agrupadorContenidoDigitalID generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Int64 agrupadorPadreID)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (agrupadorPadreID == null)
                sError += ", agrupadorContenidoDigitalID";
            if (sError.Length > 0)
                throw new Exception("CursoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "CursoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "CursoRetHlp: No se pudo conectar a la base de datos\n"+ex.Message, "POV.Profesionalizacion.DAO",
                   "CursoRetHlp", "Action", null, ex.InnerException);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT AgrupadorContenidoDigitalID, AgrupadorPadreID, Nombre, EsPredeterminado, FechaRegistro, EstatusProfesionalizacion, TipoAgrupador, TemaCursoID, Presencial, Informacion ");
            sCmd.Append(" FROM Curso ");
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
                sqlAdapter.Fill(ds, "Curso");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("CursoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
