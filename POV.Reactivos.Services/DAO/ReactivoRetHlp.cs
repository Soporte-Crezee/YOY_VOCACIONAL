using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Modelo.Estandarizado.BO;
using POV.CentroEducativo.BO;

namespace POV.Reactivos.DAO
{
    /// <summary>
    /// Consulta un registro de Reactivo en la BD
    /// </summary>
    internal class ReactivoRetHlp
    {
        /// <summary>
        /// Consulta registros de Reactivo en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="reactivo">Reactivo que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Reactivo generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Reactivo reactivo)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (reactivo == null)
                sError += ", Reactivo";
            if (sError.Length > 0)
                throw new Exception("ReactivoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (reactivo.TipoReactivo == null)
                sError += ", Tipo Reactivo";
            if (sError.Length > 0)
                throw new Exception("ReactivoDiagnosticoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "ReactivoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ReactivoRetHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "ReactivoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ReactivoID, NombreReactivo, Valor, Vigencia, TipoComplejidadID, AreaAplicacionID, NumeroIntentos, PlantillaReactivo, Descripcion, Retroalimentacion, Activo, FechaRegistro=Vigencia, PresentacionPlantilla ");
            if (reactivo is ReactivoDocente)
            {
                if ((reactivo as ReactivoDocente).Docente != null && (reactivo as ReactivoDocente).Docente.DocenteID != null)
                {
                    sCmd.Append(", DocenteID");
                }
            }
            sCmd.Append(" FROM Reactivo ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (reactivo.ReactivoID != null)
            {
                s_VarWHERE.Append(" ReactivoID = @reactivo_ReactivoID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "reactivo_ReactivoID";
                sqlParam.Value = reactivo.ReactivoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.NombreReactivo != null)
            {
                s_VarWHERE.Append(" AND NombreReactivo LIKE @reactivo_NombreReactivo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "reactivo_NombreReactivo";
                sqlParam.Value = reactivo.NombreReactivo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.PlantillaReactivo != null)
            {
                s_VarWHERE.Append(" AND PlantillaReactivo LIKE @reactivo_PlantillaReactivo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "reactivo_PlantillaReactivo";
                sqlParam.Value = reactivo.PlantillaReactivo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.Descripcion != null && reactivo.Descripcion != String.Empty)
            {
                s_VarWHERE.Append(" AND Descripcion LIKE @reactivo_Descripcion ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "reactivo_Descripcion";
                sqlParam.Value = reactivo.Descripcion;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (reactivo.Activo != null)
            {
                s_VarWHERE.Append(" AND Activo = @reactivo_Activo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "reactivo_Activo";
                sqlParam.Value = reactivo.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo is ReactivoDocente)
            {
                if ((reactivo as ReactivoDocente).Docente != null && (reactivo as ReactivoDocente).Docente.DocenteID != null)
                {
                    s_VarWHERE.Append(" AND DocenteID = @reactivo_DocenteID ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "reactivo_DocenteID";
                    sqlParam.Value = (reactivo as ReactivoDocente).Docente.DocenteID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            else
            {
                s_VarWHERE.Append(" AND DocenteID IS NULL ");
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
            sCmd.Append(" ORDER BY NombreReactivo ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Reactivo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ReactivoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
