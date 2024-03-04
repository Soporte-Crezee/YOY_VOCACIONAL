using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Modelo.BO;

namespace POV.Reactivos.DAO
{
    /// <summary>
    /// Consulta un registro de Reactivo en la BD
    /// </summary>
    internal class ReactivoDinamicoRetHlp
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
                throw new Exception("ReactivoDinamicoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (reactivo.Caracteristicas == null)
                reactivo.Caracteristicas = new CaracteristicasModeloGenerico();

            if (reactivo.Caracteristicas is CaracteristicasModeloGenerico)
            {
                if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo == null)
                    (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo = new ModeloDinamico();
                if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador == null)
                    (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador = new Clasificador();
            }
            else
            {
                throw new Exception("ReactivoDinamicoRetHlp: Se espera un tipo CaracteristicasModeloGenerico ");

            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "ReactivoDinamicoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ReactivoDinamicoRetHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "ReactivoDinamicoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ReactivoID, NombreReactivo, Valor, PlantillaReactivo, Descripcion, Activo, Asignado, Clave, TipoReactivo, PresentacionPlantilla, ModeloID, ClasificadorID, FechaRegistro, Grupo ");
            sCmd.Append(" FROM ReactivoDinamico ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (reactivo.ReactivoID != null)
            {
                s_VarWHERE.Append(" ReactivoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = reactivo.ReactivoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.NombreReactivo != null)
            {
                s_VarWHERE.Append(" AND NombreReactivo like @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = reactivo.NombreReactivo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (reactivo.Activo != null)
            {
                s_VarWHERE.Append(" AND Activo = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = reactivo.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.Asignado != null)
            {
                s_VarWHERE.Append(" AND Asignado = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = reactivo.Asignado;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.Clave != null)
            {
                s_VarWHERE.Append(" AND Clave = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = reactivo.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.TipoReactivo != null)
            {
                s_VarWHERE.Append(" AND TipoReactivo = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = reactivo.TipoReactivo;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.PresentacionPlantilla != null)
            {
                s_VarWHERE.Append(" AND PresentacionPlantilla = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = reactivo.PresentacionPlantilla;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.ModeloID != null)
            {
                s_VarWHERE.Append(" AND ModeloID = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Modelo.ModeloID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if ((reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador.ClasificadorID != null)
            {
                s_VarWHERE.Append(" AND ClasificadorID = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = (reactivo.Caracteristicas as CaracteristicasModeloGenerico).Clasificador.ClasificadorID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = reactivo.FechaRegistro;
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
                throw new Exception("ReactivoDinamicoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
