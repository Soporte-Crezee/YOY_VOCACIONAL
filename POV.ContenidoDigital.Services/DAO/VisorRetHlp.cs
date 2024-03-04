using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;

namespace POV.ContenidosDigital.DAO
{
    /// <summary>
    /// Consulta un registro de AVisor en la BD
    /// </summary>
    internal class VisorRetHlp
    {
        /// <summary>
        /// Consulta registros de Visor en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="visor">Visor que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Visor generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, AVisor visor)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (visor == null)
                sError += ", AVisor";
            if (sError.Length > 0)
                throw new Exception("VisorRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.DAO",
                   "VisorRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "VisorRetHlp: No se pudo conectar a la base de datos", "POV.ContenidosDigital.DAO",
                   "VisorRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT VisorID, Clave, FechaRegistro, Activo, Extension, Fuente, EsInterno ");
            sCmd.Append(" FROM Visor ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (visor.VisorID != null)
            {
                s_VarWHERE.Append(" VisorID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = visor.VisorID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (visor.Clave != null)
            {
                s_VarWHERE.Append(" AND Clave = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = visor.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (visor.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = visor.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (visor.Activo != null)
            {
                s_VarWHERE.Append(" AND Activo = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = visor.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (visor is VisorInterno)
            {
                if ((visor as VisorInterno).Extension != null)
                {
                    s_VarWHERE.Append(" AND Extension like @dbp4ram5 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram5";
                    sqlParam.Value = (visor as VisorInterno).Extension;
                    sqlParam.DbType = DbType.String;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            if (visor is VisorExterno)
            {

                if ((visor as VisorExterno).Fuente != null)
                {
                    s_VarWHERE.Append(" AND Fuente like @dbp4ram6 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram6";
                    sqlParam.Value = (visor as VisorExterno).Fuente;
                    sqlParam.DbType = DbType.String;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            if (visor.EsInterno != null)
            {
                s_VarWHERE.Append(" AND EsInterno = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = visor.EsInterno;
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
                sqlAdapter.Fill(ds, "Visor");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("VisorRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
