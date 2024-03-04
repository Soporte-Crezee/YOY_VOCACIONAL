using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;

namespace POV.Comun.DAO
{
    /// <summary>
    /// Consulta registros de Localidad en la base de datos
    /// </summary>
    public class LocalidadRetHlp
    {
        /// <summary>
        /// Consulta registros de Localidad en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="localidad">Localidad que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Localidad generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Localidad localidad)
        {
            object myFirm = new object();
            string sError = "";
            if (localidad == null)
                sError += ", Ciudad";
            if (localidad.Ciudad == null)
            {
                localidad.Ciudad = new Ciudad();
            }
            if (sError.Length > 0)
                throw new Exception("LocalidadRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "VITADAT.Comun.DAO","LocalidadRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LocalidadRetHlp: Hubo un error al conectarse a la base de datos", "VITADAT.Comun.DAO","LocalidadRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT LOCALIDADID, NOMBRE, CODIGO, FECHAREGISTRO, CIUDADID ");
            sCmd.Append(" FROM LOCALIDAD ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (localidad.LocalidadID != null)
            {
                s_VarWHERE.Append(" LOCALIDADID = @localidad_LocalidadID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "localidad_LocalidadID";
                sqlParam.Value = localidad.LocalidadID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (localidad.Nombre != null)
            {
                s_VarWHERE.Append(" AND NOMBRE LIKE @localidad_Nombre ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "localidad_Nombre";
                sqlParam.Value = localidad.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (localidad.Codigo != null)
            {
                s_VarWHERE.Append(" AND CODIGO = @localidad_Codigo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "localidad_Codigo";
                sqlParam.Value = localidad.Codigo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (localidad.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FECHAREGISTRO = @localidad_FechaRegistro ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "localidad_FechaRegistro";
                sqlParam.Value = localidad.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (localidad.Ciudad.CiudadID != null)
            {
                s_VarWHERE.Append(" AND CIUDADID = @localidad_Ciudad_CiudadID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "localidad_Ciudad_CiudadID";
                sqlParam.Value = localidad.Ciudad.CiudadID;
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
            sCmd.Append(" ORDER BY NOMBRE ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Localidad");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("LocalidadRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
