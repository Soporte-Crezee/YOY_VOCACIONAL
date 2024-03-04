using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.DA
{
    internal class PlantillaResultadoCleaverRetHlp
    {
        /// <summary>
        /// Consulta registros de AResultadoPrueba en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aResultadoPrueba">AResultadoPrueba que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AResultadoPrueba generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, PlantillaResultadoCleaver plantillaResultadoCleaver)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (plantillaResultadoCleaver == null)
                sError += ", APrueba";
            if (sError.Length > 0)
                throw new Exception("PlantillaResultadoCleaverRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (plantillaResultadoCleaver.Tag == null)
                sError += ", Tag";
            if (plantillaResultadoCleaver.Opcion == null)
                sError += ", Opcion";
            if (plantillaResultadoCleaver.Valor == null)
                sError += ", Valor";
            if (sError.Length > 0)
                throw new Exception("PlantillaResultadoCleaverRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.PruebaDiagnostico.DA",
                   "PlantillaResultadoCleaverRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PlantillaResultadoCleaverRetHlp: No se pudo conectar a la base de datos", "POV.PruebaDiagnostico.DA",
                   "PlantillaResultadoCleaverRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append("SELECT [PlantillaResultadoCleaverID] ,[Tag] ,[Opcion] ,[Valor] ,[Porcentaje]  ");
            sCmd.Append("  FROM [PlantillaResultadoCleaver] ");

            StringBuilder s_VarWHERE = new StringBuilder();
            if (plantillaResultadoCleaver.Tag != null)
            {
                s_VarWHERE.Append(" tag = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = plantillaResultadoCleaver.Tag;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (plantillaResultadoCleaver.Opcion != null)
            {
                s_VarWHERE.Append(" AND opcion = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = plantillaResultadoCleaver.Opcion;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (plantillaResultadoCleaver.Valor != null)
            {
                s_VarWHERE.Append(" AND valor = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = plantillaResultadoCleaver.Valor;
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
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "PlantillaResultadoCleaver");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("PlantillaResultadoCleaverRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
