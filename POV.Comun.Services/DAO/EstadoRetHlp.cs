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
    /// Consulta registros de Estado en la base de datos
    /// </summary>
    public class EstadoRetHlp
    {
        /// <summary>
        /// Consulta registros de Estado en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="estado">Estado que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Estado generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Estado estado)
        {
            object myFirm = new object();
            string sError = "";
            if (estado == null)
                sError += ", Estado";

            if (estado.Pais == null)
            {
                estado.Pais = new Pais();
            }
            if (sError.Length > 0)
                throw new Exception("EstadoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "VITADAT.Comun.DAO","EstadoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EstadoRetHlp: Hubo un error al conectarse a la base de datos", "VITADAT.Comun.DAO","EstadoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ESTADOID, NOMBRE, CODIGO, FECHAREGISTRO, PAISID ");
            sCmd.Append(" FROM ESTADO ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (estado.EstadoID != null)
            {
                s_VarWHERE.Append(" ESTADOID = @estado_EstadoID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "estado_EstadoID";
                sqlParam.Value = estado.EstadoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (estado.Nombre != null)
            {
                s_VarWHERE.Append(" AND NOMBRE LIKE @estado_Nombre ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "estado_Nombre";
                sqlParam.Value = estado.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (estado.Codigo != null)
            {
                s_VarWHERE.Append(" AND CODIGO = @estado_Codigo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "estado_Codigo";
                sqlParam.Value = estado.Codigo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (estado.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FECHAREGISTRO = @estado_FechaRegistro ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "estado_FechaRegistro";
                sqlParam.Value = estado.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (estado.Pais.PaisID != null)
            {
                s_VarWHERE.Append(" AND PAISID = @estado_Pais_PaisID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "estado_Pais_PaisID";
                sqlParam.Value = estado.Pais.PaisID;
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
                sqlAdapter.Fill(ds, "Estado");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("EstadoRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
