using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using System.Data.Common;

namespace POV.Profesionalizacion.DA
{
    internal class SituacionAprendizajeDARetHlp
    {
        /// <summary>
        /// Obtiene las situaciones de aprendizaje que pertenecen a un eje temático y que están activas
        /// </summary>
        /// <param name="dctx">DataContext que provee acceso a la base de datos</param>
        /// <param name="pageSize">El numero de registros que muestra la pagina</param>
        /// <param name="currentPage">la página actual de un paginador</param>
        /// <param name="sortColumn">Que columna se utilizará como filtro de búsqueda</param>
        /// <param name="sortOrder">Orden ascendente o descendente</param>
        /// <param name="parametros">Diccionario que contiene los parámetros y valores de la consulta</param>
        /// <returns>un dto que contiene la informacion de las situaciones de aprendizaje que cumplen los criterios de consulta</returns>
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
                throw new Exception(string.Format("SituacionAprendizajeDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }

            //Configuración de valores del parámetro sortColumn. (Solamente acepta los filtros de la tabla SituacionAprendizaje
            if (sortColumn.Contains(","))
            {
                String inter = sortColumn.Replace(",", ",sa.");
                sortColumn = inter;
            }
                        
            pageIndex = currentPage - 1;
            DataSet ds = new DataSet();

            swhere = string.Empty;
            query.Append(" SELECT");
            query.Append(" et.EjeTematicoID, et.Nombre, sa.Descripcion, SituacionAprendizaje = sa.Nombre, sa.SituacionAprendizajeID, sa.AgrupadorContenidoDigitalID, rownumber= ROW_NUMBER()");
            query.Append(" OVER( ORDER BY ");
            query.Append(string.Format(" sa.{0}", sortColumn));
            query.Append(string.Format(" {0}", sortorder));
            query.Append(" )");
            query.Append(" FROM ");
            query.Append(" SituacionAprendizaje sa INNER JOIN EjeTematico et ON et.EjeTematicoID = sa.EjeTematicoID ");
            query.Append(" INNER JOIN EjeContrato ec ON ec.EjeTematicoID = et.EjeTematicoID ");

            //Condiciones
            if (parametros.ContainsKey("EjeTematicoID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "sa.EjeTematicoID =  @EjeTematicoID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@EjeTematicoID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = parametros["EjeTematicoID"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("Nombre"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "sa.Nombre COLLATE SQL_LATIN1_GENERAL_CP1_CI_AI LIKE  @Nombre";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Nombre";
                dbParameter.DbType = DbType.String;
                dbParameter.Value = parametros["Nombre"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("ContratoID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ec.ContratoID =  @ContratoID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@ContratoID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = parametros["ContratoID"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            //Que el estatus de Situación de Aprendizaje sea Activo.
            if (parametros.ContainsKey("EstatusSituacion"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "sa.EstatusProfesionalizacion = @EstatusSituacion";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@EstatusSituacion";
                dbParameter.DbType = DbType.Byte;
                dbParameter.Value = parametros["EstatusSituacion"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            //Que el estatus del Eje Temático sea Activo.
            if (parametros.ContainsKey("EstatusEjeTematico"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "et.EstatusProfesionalizacion = @EstatusEjeTematico";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@EstatusEjeTematico";
                dbParameter.DbType = DbType.Byte;
                dbParameter.Value = parametros["EstatusEjeTematico"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }

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

            queryCols.Append("SELECT EjeTematicoID, Nombre, Descripcion, SituacionAprendizajeID, SituacionAprendizaje, AgrupadorContenidoDigitalID ");
            queryCols.Append(" FROM ( ");
            queryCols.Append(query.ToString());
            queryCols.Append(" ) as SituacionAprendizajes");
            queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
            queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");

            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter();
                dbCommand.CommandText = queryCols.ToString();
                adapter.SelectCommand = dbCommand;
                adapter.Fill(ds, "SituacionAprendizaje");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SituacionAprendizajeDARetHlp: Ocurrió un problema al recuperar las situaciones de aprendizaje: {0}", ex.Message));
            }
            return ds;
        }
    }
}
