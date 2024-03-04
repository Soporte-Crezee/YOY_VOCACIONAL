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
    internal class DiscrepanciaRavenDARetHlp
    {
        /// <summary>
        /// DA de la discrepancia Raven
        /// </summary>
        /// <param name="dctx"> Porveedor de datos. </param>
        /// <param name="puntajeTotal"> Puntaje porporcionado </param>
        /// <returns> Retorna dataset con los resultados </returns>
        public DataSet Action(IDataContext dctx, int puntajeTotal) 
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
            if (puntajeTotal < 0)
                sError += ", Puntaje Total";

            if (sError.Length > 0)
                throw new Exception("DiscrepanciaRavenDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new Exception("DiscrepanciaRavenDARetHlp.Action: DataContext no puede ser nulo");
            #endregion

            try
            {
                dctx.OpenConnection(myfirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("DiscrepanciaRavenDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }

            sCmd.Append(" SELECT ");
            sCmd.Append(" DiscrepanciaID,PuntajeTotal,Serie,PuntajeEquivalente ");
            sCmd.Append(" FROM DiscrepanciaRaven ");

            if (puntajeTotal > 0)
            {
                s_VarWHERE.Append(" AND PuntajeTotal = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = puntajeTotal;
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
                sqlAdapter.Fill(ds, "DiscrepanciaRaven");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myfirm); }
                catch (Exception) { }
                throw new Exception("DiscrepanciaRavenDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
