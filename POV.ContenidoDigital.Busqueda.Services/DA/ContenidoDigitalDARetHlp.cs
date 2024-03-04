using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;
using POV.ContenidosDigital.BO;

namespace POV.ContenidosDigital.Busqueda.DA
{
    /// <summary>
    /// Consulta los registros de ContenidoDigital en la BD
    /// </summary>
    internal class ContenidoDigitalDARetHlp
    {
        /// <summary>
        /// Consulta registros de ContenidoDigital en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigital">ContenidoDigital que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ContenidoDigital generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortOrder, Dictionary<string, string> parametros)
        {
            if (pageSize <= 0)
                throw new Exception("El tamaño de página debe ser mayor a cero");
            if (currentPage < 0)
                throw new Exception("La página actual debe ser un número mayor que cero");
            if (parametros == null)
                throw new Exception("Los parámetros no pueden ser nulos");
            if (parametros.Count == 0)
                throw new Exception("El numéro de parámetros debe ser mayor a cero");

            if (!parametros.ContainsKey("ContratoID"))
                throw new Exception("El identificador del contrato es requerido");

            int pageIndex = 0;

            string swhere = string.Empty;
            string sError = string.Empty;
            StringBuilder query = new StringBuilder();
            StringBuilder queryFinal = new StringBuilder();
            StringBuilder queryCols = new StringBuilder();
            DbParameter dbParameter = null;
            DbCommand sqlCmd = null;

            object myFirm = new object();
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.Busqueda.Services.DA",
                   "ContenidoDigitalDARetHlp", "Action", null, null);
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ContenidoDigitalDARetHlp: No se pudo conectar a la base de datos", "POV.ContenidosDigital.Busqueda.Services.DA",
                   "ContenidoDigitalDARetHlp", "Action", null, null);
            }

            pageIndex = currentPage - 1;

            query.Append("SELECT DISTINCT cda.EjeTematicoID, cda.SituacionAprendizajeID, sa.Nombre as NombreSituacion,");
            query.Append(" cda.ContenidoDigitalID as ContenidoID, cd.Clave, cd.Nombre as NombreContenido, cd.EsInterno, cd.EsDescargable,");
            query.Append(" cd.InstitucionOrigen, td.Nombre as TipoDocumento,td.ImagenDocumento, cd.Tags as Etiquetas");
            query.Append(" FROM PalabraClave pc");
            query.Append(" INNER JOIN PalabraClaveContenido pcc");
            query.Append(" ON pcc.PalabraClaveID = pc.PalabraClaveID");
            query.Append(" INNER JOIN PalabraClaveContenidoDigital pccd");
            query.Append(" ON pccd.PalabraClaveContenidoID = pcc.PalabraClaveContenidoID");
            query.Append(" INNER JOIN ContenidoDigitalAgrupador cda");
            query.Append(" ON cda.ContenidoDigitalAgrupadorID = pccd.ContenidoDigitalAgrupadorID");
            query.Append(" INNER JOIN AgrupadorContenidoDigital acd");
            query.Append(" ON acd.AgrupadorContenidoDigitalID = cda.AgrupadorContenidoDigitalID");
            query.Append(" INNER JOIN ContenidoDigital cd");
            query.Append(" ON cd.ContenidoDigitalID = cda.ContenidoDigitalID");
            query.Append(" INNER JOIN TipoDocumento td");
            query.Append(" ON td.TipoDocumentoID = cd.TipoDocumentoID");
            query.Append(" INNER JOIN SituacionAprendizaje sa");
            query.Append(" ON sa.SituacionAprendizajeID = cda.SituacionAprendizajeID");
            query.Append(" INNER JOIN EjeTematico eje");
            query.Append(" ON eje.EjeTematicoID = sa.EjeTematicoID");
            query.Append(" INNER JOIN EjeContrato ejec");
            query.Append(" ON ejec.EjeTematicoID = eje.EjeTematicoID");

            swhere += "ejec.ContratoID = @parmContratoID";
            dbParameter = sqlCmd.CreateParameter();
            dbParameter.ParameterName = "@parmContratoID";
            dbParameter.DbType = DbType.Int64;
            dbParameter.Value = parametros["ContratoID"].Trim();
            sqlCmd.Parameters.Add(dbParameter);

            swhere+= " AND eje.EstatusProfesionalizacion = @parmEstatus";
            swhere += " AND sa.EstatusProfesionalizacion = @parmEstatus";
            swhere+= " AND acd.EstatusProfesionalizacion = @parmEstatus";
            dbParameter = sqlCmd.CreateParameter();
            dbParameter.ParameterName = "@parmEstatus";
            dbParameter.DbType = DbType.Byte;
            dbParameter.Value = (Byte)EEstatusProfesionalizacion.ACTIVO;
            sqlCmd.Parameters.Add(dbParameter);

            swhere += " AND cd.EstatusContenido = @parmEstatusCont";
            dbParameter = sqlCmd.CreateParameter();
            dbParameter.ParameterName = "@parmEstatusCont";
            dbParameter.DbType = DbType.Byte;
            dbParameter.Value = (Byte)EEstatusContenido.ACTIVO;
            sqlCmd.Parameters.Add(dbParameter);

            if (parametros.ContainsKey("NombreContenido"))
            {
                string nombreContenido = parametros["NombreContenido"].Trim();
                swhere += @" AND (cd.Nombre COLLATE SQL_LATIN1_GENERAL_CP1_CI_AI LIKE '%' + @parmNombreContenido + '%' ";
                swhere += @" OR pc.Tag COLLATE SQL_LATIN1_GENERAL_CP1_CI_AI LIKE '%' + @parmNombreContenido + '%' ";
                dbParameter = sqlCmd.CreateParameter();
                dbParameter.ParameterName = "@parmNombreContenido";
                dbParameter.DbType = DbType.String;
                dbParameter.Value = nombreContenido;
                sqlCmd.Parameters.Add(dbParameter);

                if (!string.IsNullOrEmpty(nombreContenido))
                {
                    string [] palabras = nombreContenido.Split(' ');
                    if (palabras.Length > 1){
                        for( int index = 0 ; index < palabras.Length; index++)
                        {
                            swhere += @" OR pc.Tag COLLATE SQL_LATIN1_GENERAL_CP1_CI_AI LIKE '%' + @EtiquetaExtra_" + index + " + '%' ";
                            dbParameter = sqlCmd.CreateParameter();
                            dbParameter.ParameterName = "@EtiquetaExtra_" + index;
                            dbParameter.DbType = DbType.String;
                            dbParameter.Value = palabras[index];
                            sqlCmd.Parameters.Add(dbParameter);
                        }
                    }
                }
                swhere += " ) ";


            }

            if (swhere.Length > 0)
            {
                query.Append(" WHERE " + swhere);
            }

            queryFinal.Append("SELECT EjeTematicoID, SituacionAprendizajeID, NombreSituacion,");
            queryFinal.Append(" ContenidoID, Clave, NombreContenido, EsInterno, EsDescargable,");
            queryFinal.Append(" InstitucionOrigen, TipoDocumento,ImagenDocumento, Etiquetas,");
            queryFinal.Append(" rownumber = ROW_NUMBER()");
            queryFinal.Append(string.Format(" OVER(ORDER BY {0} {1})", sortColumn, sortOrder));
            queryFinal.Append(" FROM (");
            queryFinal.Append(query.ToString());
            queryFinal.Append(") as query");

            if (pageSize > 0)
            {
                dbParameter = sqlCmd.CreateParameter();
                dbParameter.ParameterName = "@PageSize";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = pageSize;
                sqlCmd.Parameters.Add(dbParameter);

            }
            if (pageIndex > -2)
            {
                dbParameter = sqlCmd.CreateParameter();
                dbParameter.ParameterName = "@PageIndex";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = pageIndex;
                sqlCmd.Parameters.Add(dbParameter);
            }

            queryCols.Append("SELECT EjeTematicoID, SituacionAprendizajeID, NombreSituacion, ContenidoID, NombreContenido, InstitucionOrigen, TipoDocumento,ImagenDocumento, Etiquetas");
            queryCols.Append(" FROM ( ");
            queryCols.Append(queryFinal.ToString());
            queryCols.Append(" ) as ContenidosDigitales");
            queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
            queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");

            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = queryCols.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "ContenidoDigital");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ContenidoDigitalDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }
        /// <summary>
        /// Obtiene la palabra clave del contenido digital agrupador proporcionado
        /// </summary>
        /// <param name="dctx">DataContext que proveerá el acceso a la base de datos.</param>
        /// <param name="contenidoDigitalAgrupador">Contenido digital agrupador proporcionado</param>
        /// <returns>Regresa dataset de palabra clave de acuerdo al contenido digital agrupador proporcionado.</returns>
        public DataSet Action(IDataContext dctx, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (contenidoDigitalAgrupador == null)
                sError += ", contenidoDigitalAgrupador";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalDARetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.Busqueda.DA",
                   "ContenidoDigitalDARetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "",
                                            "ContenidoDigitalDARetHlp: No se pudo conectar a la base de datos",
                                            "POV.ContenidosDigital.Busqueda.DA",
                                            "ContenidoDigitalDARetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT PalabraClaveContenidoID, ContenidoDigitalAgrupadorID ");
            sCmd.Append(" FROM PalabraClaveContenidoDigital ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID != null)
            {
                s_VarWHERE.Append(" AND ContenidoDigitalAgrupadorID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID;
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
                sqlAdapter.Fill(ds, "PalabraClaveContenidoDigital");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ContenidoDigitalDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
