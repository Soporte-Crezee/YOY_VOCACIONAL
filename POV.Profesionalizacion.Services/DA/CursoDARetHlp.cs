using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using System.Data.Common;
using POV.ContenidosDigital.BO;
using POV.Profesionalizacion.BO;
using Framework.Base.Exceptions;

namespace POV.Profesionalizacion.DA
{
    /// <summary>
    /// Clase dedicada a traer registros específicos de los cursos dados de alta en la base de datos.
    /// </summary>
   internal class CursoDARetHlp
    {
        /// <summary>
        /// Consulta los registros de  cursos con paginacion
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos.</param>
        /// <param name="pageSize">Número del tamaño de la página (cantidad de registros por página)</param>
        /// <param name="currentPage">Número de página se desea visualizar.</param>
        /// <param name="sortColumn">Columna encargada del ordenamiento de la consulta.</param>
        /// <param name="sortorder">Tipo de ordenamiento (ASC o DESC)</param>
        /// <param name="parametros">Diccionario de parámetros encargados del filtro de la consulta.</param>
        /// <returns></returns>
       public DataSet Action(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder,
           Dictionary<string, string> parametros) {
               object myFirm = new object();
               string sError = String.Empty;
               if (pageSize <=0)
                   sError += ", PageSize";
               if (currentPage < 0)
                   sError+=",La página actual debe ser un número mayor que cero";
               if (parametros == null)
                   sError += ",Los parámetros no pueden ser nulos";
               if (sError.Length > 0)
                   throw new Exception("CursoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
               if (parametros.Count == 0)
                   sError += ",Debe existir por lo menos un parametro";
               if (sError.Length > 0)
                   throw new Exception("CursoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

               int pageIndex = 0;

               //variables
               string swhere = string.Empty;
               string sErrors = string.Empty;
               StringBuilder query = new StringBuilder();
               StringBuilder queryControl = new StringBuilder();
               StringBuilder queryPrivacidad = new StringBuilder();
               StringBuilder queryCols = new StringBuilder();
               DbParameter dbParameter = null;
               DbCommand dbCommand = null;
               DbCommand dbCommandControl = null;
  

               try
               {
                   dctx.OpenConnection(myFirm);
                   dbCommand = dctx.CreateCommand();
                   dbCommandControl = dctx.CreateCommand();
               }
               catch (Exception ex)
               {
                   throw new Exception(string.Format("CursoDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
               }

               pageIndex = currentPage - 1;
               DataSet ds = new DataSet();

               swhere = string.Empty;
               query.Append(" SELECT");
               query.Append(" c.AgrupadorContenidoDigitalID,c.Informacion,c.Nombre,c.Presencial,TemaCurso= t.Nombre, rownumber= ROW_NUMBER()");
               query.Append(" OVER( ORDER BY ");
               query.Append(sortColumn+ " ");
               query.Append(sortorder+ " " );
               query.Append(" )");
               query.Append(" FROM ");
               query.Append(" Curso c");
               query.Append(" INNER JOIN TemaCurso t on c.TemaCursoID = t.TemaCursoID");
        

               //Condiciones
               if (parametros.ContainsKey("CursoID"))
               {
                   if (swhere.Length > 0) swhere += " AND ";
                   swhere += "AgrupadorContenidoDigitalID =@CursoID";
                   dbParameter = dbCommand.CreateParameter();
                   dbParameter.ParameterName = "@CursoID";
                   dbParameter.DbType = DbType.Int64;
                   dbParameter.Value = parametros["CursoID"].Trim();
                   dbCommand.Parameters.Add(dbParameter);
               }
               if (parametros.ContainsKey("CursoNombre"))
               {
                   if (swhere.Length > 0) swhere += " AND ";
                      swhere += "c.Nombre COLLATE SQL_LATIN1_GENERAL_CP1_CI_AI LIKE  @paramNombre ";
                   dbParameter = dbCommand.CreateParameter();
                   dbParameter.ParameterName = "@paramNombre";
                   dbParameter.DbType = DbType.String;
                   dbParameter.Value = parametros["CursoNombre"].Trim();
                   dbCommand.Parameters.Add(dbParameter);
               }
               if (parametros.ContainsKey("CursoPresencial"))
               {
                   if (swhere.Length > 0) swhere += " AND ";
                   swhere += "c.Presencial=@CursoPresencial";
                   dbParameter = dbCommand.CreateParameter();
                   dbParameter.ParameterName = "@CursoPresencial";
                   dbParameter.DbType = DbType.String;
                   dbParameter.Value = parametros["CursoPresencial"].Trim();
                   dbCommand.Parameters.Add(dbParameter);
               }
               if (parametros.ContainsKey("CursoEstatus"))
               {
                   if (swhere.Length > 0) swhere += " AND ";
                   swhere += "c.EstatusProfesionalizacion=@CursoEstatus";
                   dbParameter = dbCommand.CreateParameter();
                   dbParameter.ParameterName = "@CursoEstatus";
                   dbParameter.DbType = DbType.String;
                   dbParameter.Value = parametros["CursoEstatus"].Trim();
                   dbCommand.Parameters.Add(dbParameter);
               }

               if (parametros.ContainsKey("CursoTemaID"))
               {
                   if (swhere.Length > 0) swhere += " AND ";
                   swhere += "t.TemaCursoID = @CursoTemaID";
                   dbParameter = dbCommand.CreateParameter();
                   dbParameter.ParameterName = "@CursoTemaID";
                   dbParameter.DbType = DbType.Int32;
                   dbParameter.Value = parametros["CursoTemaID"].Trim();
                   dbCommand.Parameters.Add(dbParameter);
               }

               if (swhere.Length > 0) swhere += " AND ";
               swhere += "c.TipoAgrupador = @TipoAgrupador";
               dbParameter = dbCommand.CreateParameter();
               dbParameter.ParameterName = "@TipoAgrupador";
               dbParameter.DbType = DbType.Int32;
               dbParameter.Value = ETipoAgrupador.COMPUESTO_CURSO;
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

               queryCols.Append("SELECT Cursos.AgrupadorContenidoDigitalID,Cursos.Informacion,Cursos.Nombre,Cursos.Presencial,Cursos.TemaCurso");
               queryCols.Append(" FROM ( ");
               queryCols.Append(query.ToString());
               queryCols.Append(" ) as Cursos");
               queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
               queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");

               try
               {
                   DbDataAdapter adapter = dctx.CreateDataAdapter();
                   dbCommand.CommandText = queryCols.ToString();
                   adapter.SelectCommand = dbCommand;
                   adapter.Fill(ds, "Curso");
               }
               catch (Exception ex)
               {
                   throw new Exception(string.Format("CursoDARetHlp: Ocurrió un problema al recuperar los cursos: {0}", ex.Message));
               }
               return ds;
       }

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
                  "CursoDARetHlp", "Action", null, null);

           DbCommand sqlCmd = null;
           try
           {
               dctx.OpenConnection(myFirm);
               sqlCmd = dctx.CreateCommand();
           }
           catch (Exception ex)
           {
               throw new StandardException(MessageType.Error, "", "CursoDARetHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DA",
                  "CursoDARetHlp", "Action", null, null);
           }
           DbParameter sqlParam;
           StringBuilder sCmd = new StringBuilder();
           sCmd.Append(" SELECT c.AgrupadorPadreID,cd.AgrupadorContenidoDigitalID, cd.ContenidoDigitalID ");
           sCmd.Append(" FROM CursoDetalle cd");
           sCmd.Append(" INNER JOIN Curso c ON c.AgrupadorContenidoDigitalID = cd.AgrupadorContenidoDigitalID");
           StringBuilder s_VarWHERE = new StringBuilder();
           if (contenidoDigital.ContenidoDigitalID != null)
           {
               s_VarWHERE.Append(" cd.ContenidoDigitalID = @dbp4ram1 ");
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
               sqlAdapter.Fill(ds, "Curso");
           }
           catch (Exception ex)
           {
               string exmsg = ex.Message;
               try { dctx.CloseConnection(myFirm); }
               catch (Exception) { }
               throw new Exception("CursoDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
