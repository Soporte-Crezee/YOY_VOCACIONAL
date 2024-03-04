using Framework.Base.DataAccess;
using POV.Modelo.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.DA
{
    internal class BaremoRavenDARetHlp
    {
        /// <summary>
        /// DA para obtener la tabla de baremo raven
        /// </summary>
        /// <param name="dctx"> Proveedor de datos. </param>
        /// <param name="edad"> Edad proporcionada. </param>
        /// <returns> Retorna un dataset con los resultados. </returns>
        public DataSet Action(IDataContext dctx, string edad, long puntajeNormalizado)
        {
            #region Variables
            object myfirm = new object();
            string sError = string.Empty;

            DbCommand sqlCmd = null;
            DbParameter sqlParam;
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();

            StringBuilder sCmd = new StringBuilder();
            StringBuilder s_VarWHERE = new StringBuilder();

            DataSet ds = new DataSet();
            #endregion

            #region Validaciones
            if (string.IsNullOrEmpty(edad))
                sError += ", Edad";
            if (puntajeNormalizado < 0)
                sError += ", Puntaje Normalizado";

            if (sError.Length > 0)
                throw new Exception("BaremoRavenDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new Exception("BaremoRavenDARetHlp.Action: DataContext no puede ser nulo");
            #endregion

            try
            {
                dctx.OpenConnection(myfirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("BaremoRavenDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }

            sCmd.Append(" SELECT ");
            sCmd.Append(" BaremoID,Edad,Puntaje,Percentil ");
            sCmd.Append(" FROM TablaBaremo ");

            if (!string.IsNullOrEmpty(edad))
            {
                s_VarWHERE.Append(" AND Edad = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = edad;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (puntajeNormalizado > 0) 
            {
                s_VarWHERE.Append(" AND Puntaje = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = puntajeNormalizado;
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

            sqlAdapter.SelectCommand = sqlCmd;

            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "TablaBaremo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myfirm); }
                catch (Exception) { }
                throw new Exception("BaremoRavenDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myfirm); }
                catch (Exception) { }
            }
            return ds;
        }
    }
}
