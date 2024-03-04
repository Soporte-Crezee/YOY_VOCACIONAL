using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.Busqueda.BO;

namespace POV.ContenidosDigital.Busqueda.DAO
{
    /// <summary>
    /// Consulta los registros de APalabraClaveContenido en la BD
    /// </summary>
    internal class PalabraClaveContenidoRetHlp
    {
        /// <summary>
        /// Consulta registros de APalabraClaveContenido en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aPalabraClaveContenido">APalabraClaveContenido que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de APalabraClaveContenido generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, APalabraClaveContenido palabraClaveContenido)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (palabraClaveContenido == null)
                sError += ", APalabraClaveContenido";
            if (sError.Length > 0)
                throw new Exception("PalabraClaveContenidoRetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.Busqueda.DAO",
                   "PalabraClaveContenidoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PalabraClaveContenidoRetHlp: No se pudo conectar a la base de datos", "POV.ContenidosDigital.Busqueda.DAO",
                   "PalabraClaveContenidoRetHlp", "Action", null, null);
            }

            if (palabraClaveContenido.PalabraClave == null)
                palabraClaveContenido.PalabraClave = new PalabraClave();

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT PalabraClaveContenidoID, FechaRegistro, PalabraClaveID ");
            sCmd.Append(" FROM PalabraClaveContenido ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (palabraClaveContenido.PalabraClaveContenidoID != null)
            {
                s_VarWHERE.Append(" PalabraClaveContenidoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = palabraClaveContenido.PalabraClaveContenidoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (palabraClaveContenido.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = palabraClaveContenido.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (palabraClaveContenido.PalabraClave.PalabraClaveID != null)
            {
                s_VarWHERE.Append(" AND PalabraClaveID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = palabraClaveContenido.PalabraClave.PalabraClaveID;
                sqlParam.DbType = DbType.Int64;
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
                sqlAdapter.Fill(ds, "PalabraClaveContenido");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("PalabraClaveContenidoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
