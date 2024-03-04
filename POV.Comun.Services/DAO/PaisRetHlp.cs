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
    /// Consulta registros de País en la base de datos
    /// </summary>
    public class PaisRetHlp
    {
        /// <summary>
        /// Consulta registros de País en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="país">País que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de País generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Pais pais)
        {
            object myFirm = new object();
            string sError = "";
            if (pais == null)
                sError += ", País";
            if (sError.Length > 0)
                throw new Exception("PaisRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "VITADAT.Comun.DAO","PaisRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PaisRetHlp: Hubo un error al conectarse a la base de datos", "VITADAT.Comun.DAO", "PaisRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT PAISID, NOMBRE, CODIGO, FECHAREGISTRO ");
            sCmd.Append(" FROM PAIS ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (pais.PaisID != null)
            {
                s_VarWHERE.Append(" PAISID = @pais_PaisID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "pais_PaisID";
                sqlParam.Value = pais.PaisID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pais.Nombre != null)
            {
                s_VarWHERE.Append(" AND NOMBRE LIKE @pais_Nombre ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "pais_Nombre";
                sqlParam.Value = pais.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pais.Codigo != null)
            {
                s_VarWHERE.Append(" AND CODIGO = @pais_Codigo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "pais_Codigo";
                sqlParam.Value = pais.Codigo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pais.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FECHAREGISTRO = @pais_FechaRegistro ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "pais_FechaRegistro";
                sqlParam.Value = pais.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
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
                sqlAdapter.Fill(ds, "Pais");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("PaisRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
