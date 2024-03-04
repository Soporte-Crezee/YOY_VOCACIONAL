using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using System.Data.Common;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DA
{
    /// <summary>
    /// Proporciona datos específicos de asistencias dadas de alta en la base de datos.
    /// </summary>
    internal class AsistenciaDARetHlp
    {
        /// <summary>
        /// Obtiene los registros de Asistencia de la base de datos.
        /// </summary>
        /// <param name="dctx">DataContext que proveerá acceso a la base de datos.</param>
        /// <param name="pageSize">Tamaño de la página de los datos a mostrar.</param>
        /// <param name="currentPage">Página actual.</param>
        /// <param name="sortColumn">Columna por la cual se ordenarán los datos.</param>
        /// <param name="sortorder">Tipo de ordenamiento (ASC o DESC)</param>
        /// <param name="parametros">Diccionario de parámetros de búsqueda para la consulta.</param>
        /// <returns></returns>
        public DataSet Action(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder,
                              Dictionary<string, string> parametros)
        {
            if (pageSize <= 0)
                throw new Exception("El tamaño de página debe ser mayor a cero");
            if (currentPage < 0)
                throw new Exception("La página actual debe ser un número mayor que cero");
            if (parametros == null)
                throw new Exception("Los parámetros no pueden ser nulos");
            if (parametros.Count == 0)
                throw new Exception("El numéro de parámetros debe ser mayor a cero");

            int recordCount = 0;
            int pageCount = 0;
            int pageIndex = 0;

            //variables
            string swhere = string.Empty;
            string sError = string.Empty;
            StringBuilder query = new StringBuilder();
            StringBuilder queryControl = new StringBuilder();
            StringBuilder queryPrivacidad = new StringBuilder();
            StringBuilder queryCols = new StringBuilder();
            DbParameter dbParameter = null;
            DbCommand dbCommand = null;
            DbCommand dbCommandControl = null;
            object myFirm = new object();

            try
            {
                dctx.OpenConnection(myFirm);
                dbCommand = dctx.CreateCommand();
                dbCommandControl = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("AsistenciaDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }

            pageIndex = currentPage - 1;
            DataSet ds = new DataSet();

            swhere = string.Empty;
            query.Append(" SELECT");
            query.Append(" AsistenciaID = a.AgrupadorContenidoDigitalID, a.Nombre, Tema = ta.Nombre, rownumber = ROW_NUMBER()");
            query.Append(" OVER( ORDER BY ");
            query.Append(string.Format(" a.{0}", sortColumn));
            query.Append(string.Format(" {0}", sortorder));
            query.Append(" )");
            query.Append(" FROM (");
            query.Append(" SELECT DISTINCT aPadre.AgrupadorContenidoDigitalID ");
            query.Append(" FROM Asistencia aPadre ");
            query.Append(" LEFT JOIN Asistencia aHijo ON aHijo.AgrupadorPadreID = aPadre.AgrupadorContenidoDigitalID and aHijo.TipoAgrupador = 0");
            query.Append(" LEFT JOIN AsistenciaDetalle aHijoDetalle ON aHijoDetalle.AgrupadorContenidoDigitalID = aHijo.AgrupadorContenidoDigitalID");
            query.Append(" LEFT JOIN ContenidoDigital contenido ON contenido.ContenidoDigitalID = aHijoDetalle.ContenidoDigitalID");

            //Condiciones.
            if (parametros.ContainsKey("TemaID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "aPadre.TemaAsistenciaID LIKE @TemaID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@TemaID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = parametros["TemaID"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("TipoDocumentoID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "contenido.TipoDocumentoID LIKE @TipoDocumentoID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@TipoDocumentoID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = parametros["TipoDocumentoID"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("Nombre"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " aPadre.Nombre COLLATE SQL_LATIN1_GENERAL_CP1_CI_AI LIKE @Nombre ";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Nombre";
                dbParameter.DbType = DbType.String;
                dbParameter.Value = parametros["Nombre"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            if (swhere.Length > 0) swhere += " AND ";
            swhere += " aPadre.TipoAgrupador = @TipoAgrupador ";
            dbParameter = dbCommand.CreateParameter();
            dbParameter.ParameterName = "@TipoAgrupador";
            dbParameter.DbType = DbType.Int32;
            dbParameter.Value = ETipoAgrupador.COMPUESTO_ASISTENCIA;
            dbCommand.Parameters.Add(dbParameter);

            if (swhere.Length > 0) swhere += " AND ";
            swhere += " aPadre.EstatusProfesionalizacion = @EstatusProfesionAsis ";
            dbParameter = dbCommand.CreateParameter();
            dbParameter.ParameterName = "@EstatusProfesionAsis";
            dbParameter.DbType = DbType.Int16;
            dbParameter.Value = EEstatusProfesionalizacion.ACTIVO;
            dbCommand.Parameters.Add(dbParameter);

            if (pageSize > 0)
            {
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@PageSize";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = pageSize;
                dbCommand.Parameters.Add(dbParameter);

            }

            if (pageIndex > -2)
            {
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@PageIndex";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = pageIndex;
                dbCommand.Parameters.Add(dbParameter);
            }

            if (swhere.Length > 0)
            {
                query.Append(" WHERE " + swhere);
            }

            queryCols.Append("SELECT AsistenciaID, Nombre, Tema ");
            queryCols.Append(" FROM ( ");
            queryCols.Append(query.ToString());
            queryCols.Append(" ) as  AsistenciasResult");
            queryCols.Append(
                " INNER JOIN Asistencia a ON a.AgrupadorContenidoDigitalID = AsistenciasResult.AgrupadorContenidoDigitalID");
            queryCols.Append(" INNER JOIN TemaAsistencia ta ON ta.TemaAsistenciaID = a.TemaAsistenciaID");
            queryCols.Append(" ) as Asistencia");
            queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
            queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");

            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter();
                dbCommand.CommandText = queryCols.ToString();
                adapter.SelectCommand = dbCommand;
                adapter.Fill(ds, "Asistencia");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("AsistenciaDARetHlp: Ocurrió un problema al recuperar las asistencias: {0}", ex.Message));
            }
            return ds;
        }
        /// <summary>
        /// Obtiene los datos de la asistencia por medio del contenido digital proporcionado.
        /// </summary>
        /// <param name="dctx">DataContext que proveerá el acceso a la base de datos.</param>
        /// <param name="contenidoDigital">Contenido digital proporcionado.</param>
        /// <returns>Regresa un dataset con los datos de la asistencia asociados.</returns>
        public DataSet Action(IDataContext dctx, ContenidoDigital contenidoDigital)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (contenidoDigital.ContenidoDigitalID <= 0)
                sError += ", ContenidoDigitalID";
            if (sError.Length > 0)
                throw new Exception("CursoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DA",
                   "AsistenciaDARetHlp", "Action", null, null);

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "CursoDARetHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DA",
                   "AsistenciaDARetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT a.AgrupadorPadreID,ad.AgrupadorContenidoDigitalID, ad.ContenidoDigitalID ");
            sCmd.Append(" FROM AsistenciaDetalle ad");
            sCmd.Append(" INNER JOIN Asistencia a ON a.AgrupadorContenidoDigitalID = ad.AgrupadorContenidoDigitalID");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (contenidoDigital.ContenidoDigitalID != null)
            {
                s_VarWHERE.Append(" ad.ContenidoDigitalID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = contenidoDigital.ContenidoDigitalID;
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
                sqlAdapter.Fill(ds, "Asistencia");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AsistenciaDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
