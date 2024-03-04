using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.DAO
{
    /// <summary>
    /// Consulta un registro de APrueba en la BD
    /// </summary>
    internal class PruebaRetHlp
    {
        /// <summary>
        /// Consulta registros de APrueba en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aPrueba">APrueba que provee el criterio de selección para realizar la consulta</param>
        /// <param name="lTodas">Null || false: solamente trae las pruebas activas y liberadas ó aquellas que cumplan con la propiedad EstadoLiberacionPrueba; 
        /// true: recupera todas las pruebas sin importar su EstadoLiberacionPrueba</param>
        /// <returns>El DataSet que contiene la información de APrueba generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, APrueba prueba, bool? lTodas)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (prueba == null)
                sError += ", APrueba";
            if (sError.Length > 0)
                throw new Exception("PruebaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.DAO",
                   "PruebaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PruebaRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.DAO",
                   "PruebaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT p.PruebaID, p.Clave, p.Nombre, p.Instrucciones, p.FechaRegistro, p.EsDiagnostica, p.ModeloID, p.Tipo, p.EstadoLiberacion, p.EsPremium, p.TipoPruebaPresentacion ");
            sCmd.Append(" FROM Prueba p ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (prueba.PruebaID != null)
            {
                s_VarWHERE.Append(" p.PruebaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = prueba.PruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (prueba.Clave != null)
            {
                s_VarWHERE.Append(" AND p.Clave LIKE @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = prueba.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (prueba.Nombre != null)
            {
                s_VarWHERE.Append(" AND p.Nombre LIKE @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = prueba.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (prueba.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND p.FechaRegistro = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = prueba.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (prueba.EsDiagnostica != null)
            {
                s_VarWHERE.Append(" AND p.EsDiagnostica = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = prueba.EsDiagnostica;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (!(prueba is PruebaFilter))
            {
                if (prueba.TipoPrueba != null)
                {
                    s_VarWHERE.Append(" AND p.Tipo = @dbp4ram7 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram7";
                    sqlParam.Value = prueba.TipoPrueba;
                    sqlParam.DbType = DbType.Byte;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            if (prueba.Modelo != null && prueba.Modelo.ModeloID != null)
            {
                s_VarWHERE.Append(" AND p.ModeloID = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = prueba.Modelo.ModeloID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (lTodas == null || lTodas == false)
            {
                if (prueba.EstadoLiberacionPrueba != null)
                {
                    s_VarWHERE.Append(" AND p.EstadoLiberacion = @dbp4ram9 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram9";
                    sqlParam.Value = prueba.EstadoLiberacionPrueba;
                    sqlParam.DbType = DbType.Byte;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                else
                {
                    s_VarWHERE.Append(string.Format(" AND p.EstadoLiberacion IN ({0},{1}) ",
                        (Byte)EEstadoLiberacionPrueba.ACTIVA, (Byte)EEstadoLiberacionPrueba.LIBERADA));
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
                sqlAdapter.Fill(ds, "Prueba");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("PruebaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }

        public DataSet Action(IDataContext dctx, int? pruebaID, EEstadoLiberacionPrueba? estadoLiberacionPrueba, bool? esDiagnostica)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.DAO",
                   "PruebaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PruebaRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.DAO",
                   "PruebaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT p.PruebaID, p.Clave, p.Nombre, p.Instrucciones, p.FechaRegistro, p.EsDiagnostica, p.ModeloID, p.Tipo, p.EstadoLiberacion ");
            sCmd.Append(" FROM Prueba p ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (pruebaID != null)
            {
                s_VarWHERE.Append(" p.PruebaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = pruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (estadoLiberacionPrueba != null)
            {
                s_VarWHERE.Append(" AND p.EstadoLiberacion = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = estadoLiberacionPrueba;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (esDiagnostica != null)
            {
                s_VarWHERE.Append(" AND p.EsDiagnostica = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = esDiagnostica;
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
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Prueba");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("PruebaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
