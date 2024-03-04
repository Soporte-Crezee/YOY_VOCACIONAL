using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.CentroEducativo.BO;
using POV.Prueba.BO;

namespace POV.Licencias.DAO
{
    /// <summary>
    /// Consulta un registro de CalendarizacionPruebaGrupo en la BD
    /// </summary>
    internal class CalendarizacionPruebaGrupoRetHlp
    {
        /// <summary>
        /// Consulta registros de CalendarizacionPruebaGrupo en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="calendarizacionPruebaGrupo">CalendarizacionPruebaGrupo que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de CalendarizacionPruebaGrupo generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, CalendarizacionPruebaGrupo calendarizacionPruebaGrupo)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (calendarizacionPruebaGrupo == null)
                sError += ", GrupoCicloEscolar";
            if (sError.Length > 0)
                throw new Exception("CalendarizacionPruebaGrupoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (calendarizacionPruebaGrupo.PruebaContrato == null)
            {
                calendarizacionPruebaGrupo.PruebaContrato = new PruebaContrato();
            }
            if (calendarizacionPruebaGrupo.GrupoCicloEscolar == null)
            {
                calendarizacionPruebaGrupo.GrupoCicloEscolar = new GrupoCicloEscolar();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO",
                   "CalendarizacionPruebaGrupoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "CalendarizacionPruebaGrupoRetHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO",
                   "CalendarizacionPruebaGrupoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT CalendarizacionPruebaGrupoID, GrupoCicloEscolarID, PruebaContratoID, ConVigencia, FechaInicioVigencia, FechaFinVigencia, FechaRegistro, Activo ");
            sCmd.Append(" FROM CalendarizacionPruebaGrupo ");
            sCmd.Append(" WHERE ");
            StringBuilder s_Var = new StringBuilder();
            if (calendarizacionPruebaGrupo.CalendarizacionPruebaGrupoID != null)
            {
                s_Var.Append(" calendarizacionPruebaGrupoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = calendarizacionPruebaGrupo.CalendarizacionPruebaGrupoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (calendarizacionPruebaGrupo.GrupoCicloEscolar.GrupoCicloEscolarID != null)
            {
                s_Var.Append(" AND GrupoCicloEscolarID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = calendarizacionPruebaGrupo.GrupoCicloEscolar.GrupoCicloEscolarID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (calendarizacionPruebaGrupo.PruebaContrato.PruebaContratoID != null)
            {
                s_Var.Append(" AND PruebaContratoID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = calendarizacionPruebaGrupo.PruebaContrato.PruebaContratoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (calendarizacionPruebaGrupo.ConVigencia != null)
            {
                s_Var.Append(" AND ConVigencia = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = calendarizacionPruebaGrupo.ConVigencia;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (calendarizacionPruebaGrupo.FechaInicioVigencia != null)
            {
                s_Var.Append(" AND FechaInicioVigencia = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = calendarizacionPruebaGrupo.FechaInicioVigencia;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (calendarizacionPruebaGrupo.FechaFinVigencia != null)
            {
                s_Var.Append(" AND FechaFinVigencia = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = calendarizacionPruebaGrupo.FechaFinVigencia;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (calendarizacionPruebaGrupo.FechaRegistro != null)
            {
                s_Var.Append(" AND FechaRegistro = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = calendarizacionPruebaGrupo.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (calendarizacionPruebaGrupo.Activo != null)
            {
                s_Var.Append(" AND Activo = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = calendarizacionPruebaGrupo.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
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
                sqlAdapter.Fill(ds, "CalendarizacionPruebaGrupo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("CalendarizacionPruebaGrupoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }

        public DataSet Action(IDataContext dctx, Escuela escuela, CicloEscolar cicloEscolar, APrueba prueba)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (escuela == null)
                sError += ", Escuela";
            if (cicloEscolar == null)
                sError += ", CicloEscolar";
            if (sError.Length > 0)
                throw new Exception("CalendarizacionPruebaGrupoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (cicloEscolar.CicloEscolarID == null)
                sError += ", CicloEscolarID";
            if (sError.Length > 0)
                throw new Exception("CalendarizacionPruebaGrupoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO",
                   "CalendarizacionPruebaGrupoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "CalendarizacionPruebaGrupoRetHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO",
                   "CalendarizacionPruebaGrupoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT cpg.CalendarizacionPruebaGrupoID, cpg.GrupoCicloEscolarID, cpg.PruebaContratoID, cpg.ConVigencia, cpg.FechaInicioVigencia, cpg.FechaFinVigencia, cpg.FechaRegistro, cpg.Activo ");
            sCmd.Append(" FROM CalendarizacionPruebaGrupo AS cpg ");
            sCmd.Append(" JOIN GrupoCicloEscolar AS gce ON gce.GrupoCicloEscolarID = cpg.GrupoCicloEscolarID ");
            sCmd.Append(" JOIN PruebaContrato AS pc ON pc.PruebaContratoID = cpg.PruebaContratoID ");
            sCmd.Append(" WHERE ");
            StringBuilder s_Var = new StringBuilder();
            if (escuela.EscuelaID != null)
            {
                s_Var.Append(" gce.EscuelaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cicloEscolar.CicloEscolarID != null)
            {
                s_Var.Append(" AND gce.CicloEscolarID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = cicloEscolar.CicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            } 
            if (prueba != null && prueba.PruebaID != null)
            {
                s_Var.Append(" AND pc.PruebaID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = prueba.PruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
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
                sqlAdapter.Fill(ds, "CalendarizacionPruebaGrupo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("CalendarizacionPruebaGrupoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
