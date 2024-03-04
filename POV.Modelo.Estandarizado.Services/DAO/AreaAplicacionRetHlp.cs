using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.Estandarizado.BO;

namespace POV.Modelo.Estandarizado.DAO
{
    /// <summary>
    /// Consulta un registro de AreaAplicacion en la BD
    /// </summary>
    public class AreaAplicacionRetHlp
    {
        /// <summary>
        /// Consulta registros de AreaAplicacion en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="areaAplicacion">AreaAplicacion que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de AreaAplicacion generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, AreaAplicacion areaAplicacion)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (areaAplicacion == null)
                sError += ", AreaAplicacion";
            if (sError.Length > 0)
                throw new Exception("AreaAplicacionRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "AreaAplicacionRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AreaAplicacionRetHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "AreaAplicacionRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT AreaAplicacionID, Descripcion ");
            sCmd.Append(" FROM AreaAplicacion ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (areaAplicacion.AreaAplicacionID != null)
            {
                s_VarWHERE.Append(" AreaAplicacionID = @areaAplicacion_AreaAplicacionID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "areaAplicacion_AreaAplicacionID";
                sqlParam.Value = areaAplicacion.AreaAplicacionID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (areaAplicacion.Descripcion != null)
            {
                s_VarWHERE.Append(" AND Descripcion LIKE @areaAplicacion_Descripcion ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "areaAplicacion_Descripcion";
                sqlParam.Value = areaAplicacion.Descripcion;
                sqlParam.DbType = DbType.String;
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
            sCmd.Append(" ORDER BY Descripcion ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "AreaAplicacion");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AreaAplicacionRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
