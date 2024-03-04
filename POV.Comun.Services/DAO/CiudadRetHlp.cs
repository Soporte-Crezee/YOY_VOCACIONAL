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
    /// Consulta registros de Ciudad en la base de datos
    /// </summary>
    public class CiudadRetHlp
    {
        /// <summary>
        /// Consulta registros de Ciudad en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="ciudad">Ciudad que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Ciudad generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Ciudad ciudad)
        {
            object myFirm = new object();
            string sError = "";
            if (ciudad == null)
                sError += ", Ciudad";
            if (ciudad.Estado == null)
            {
                ciudad.Estado = new Estado();
            }
            if (sError.Length > 0)
                throw new Exception("CiudadRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Comun.DAO","CiudadRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "CiudadRetHlp: Hubo un error al conectarse a la base de datos", "VITADAT.Comun.DAO","CiudadRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT C.CIUDADID, C.NOMBRE, C.CODIGO, C.FECHAREGISTRO, C.ESTADOID ");
            sCmd.Append(" FROM CIUDAD C ");
            sCmd.Append(" INNER JOIN ESTADO E ON E.ESTADOID=C.ESTADOID ");
            sCmd.Append(" INNER JOIN PAIS P  ON P.PAISID=E.PAISID ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (ciudad.CiudadID != null)
            {
                s_VarWHERE.Append(" C.CIUDADID = @ciudad_CiudadID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "ciudad_CiudadID";
                sqlParam.Value = ciudad.CiudadID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (ciudad.Nombre != null)
            {
                s_VarWHERE.Append(" AND C.NOMBRE LIKE @ciudad_Nombre ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "ciudad_Nombre";
                sqlParam.Value = ciudad.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (ciudad.Codigo != null)
            {
                s_VarWHERE.Append(" AND C.CODIGO = @ciudad_Codigo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "ciudad_Codigo";
                sqlParam.Value = ciudad.Codigo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (ciudad.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND C.FECHAREGISTRO = @ciudad_FechaRegistro ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "ciudad_FechaRegistro";
                sqlParam.Value = ciudad.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (ciudad.Estado.EstadoID != null)
            {
                s_VarWHERE.Append(" AND C.ESTADOID = @ciudad_Estado_EstadoID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "ciudad_Estado_EstadoID";
                sqlParam.Value = ciudad.Estado.EstadoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (ciudad.Estado.Pais != null)
            {
                if (ciudad.Estado.Pais.PaisID != null)
                {
                    s_VarWHERE.Append(" AND P.PAISID = @ciudad_Estado_PaisID ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "ciudad_Estado_PaisID";
                    sqlParam.Value = ciudad.Estado.Pais.PaisID;
                    sqlParam.DbType = DbType.Int32;
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
            sCmd.Append(" ORDER BY NOMBRE ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Ciudad");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("CiudadRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
