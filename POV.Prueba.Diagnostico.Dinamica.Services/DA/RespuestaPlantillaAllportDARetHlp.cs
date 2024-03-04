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
    internal class RespuestaPlantillaAllportDARetHlp
    {
        /// <summary>
        /// DA de forma para calificar cada plantilla de Kuder
        /// </summary>
        /// <param name="dctx"> Proveedor de datos. </param>
        /// <param name="clasificador"> Clasificador proporcionado. </param>
        /// <returns> Retorna un dataset con los resultados. </returns>
        public DataSet Action(IDataContext dctx, Clasificador clasificador)
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
            if (clasificador == null)
                sError += ", Clasificador";

            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaAllportDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new Exception("RespuestaPlantillaAllportDARetHlp.Action: DataContext no puede ser nulo");
            #endregion

            try
            {
                dctx.OpenConnection(myfirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("RespuestaPlantillaAllportDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }

            sCmd.Append(" SELECT ");
            sCmd.Append(" PlantillaAllportID,Plantilla,ClasificadorID,Grupo ");
            sCmd.Append(" FROM PlantillaAllport ");

            if (clasificador.ClasificadorID != null)
            {
                s_VarWHERE.Append(" AND ClasificadorID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = clasificador.ClasificadorID;
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
                sqlAdapter.Fill(ds, "PlantillaAllport");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myfirm); }
                catch (Exception) { }
                throw new Exception("RespuestaPlantillaAllportDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
