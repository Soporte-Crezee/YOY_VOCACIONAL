using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using System.Data;
using System.Data.Common;
using POV.Profesionalizacion.BO;
using Framework.Base.Exceptions;

namespace POV.Profesionalizacion.DA
{
  internal  class CursoDetalleDARetHlp
    {
        /// <summary>
        /// Consulta los detalles, informacion y contenidos digitales de un curso
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos.</param>
        /// <param name="pageSize">Número del tamaño de la página (cantidad de registros por página)</param>
        /// <param name="currentPage">Número de página se desea visualizar.</param>
        /// <param name="sortColumn">Columna encargada del ordenamiento de la consulta.</param>
        /// <param name="sortorder">Tipo de ordenamiento (ASC o DESC)</param>
        /// <param name="parametros">Diccionario de parámetros encargados del filtro de la consulta.</param>
        /// <returns></returns>
      public DataSet Action(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder,
            Dictionary<string, string> parametros)
      {

          if (pageSize <= 0)
              throw new Exception("El tamaño de página debe ser mayor a cero");
          if (currentPage <= 0)
              throw new Exception("La página actual debe ser un número mayor que cero");
          if (parametros == null)
              throw new Exception("Los parámetros no pueden ser nulos");
          if (parametros.Count == 0)
              throw new Exception("El numéro de parámetros debe ser mayor a cero");

          int pageIndex = 0;

          
          string swhere = string.Empty;
          string sError = string.Empty;
          StringBuilder query = new StringBuilder();
          StringBuilder queryControl = new StringBuilder();
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
              throw new Exception(string.Format("CursoDetalleDARet: No se pudo conectar con la base de datos {0}", ex.Message));
          }

          pageIndex = currentPage - 1;

          swhere = string.Empty;
          query.Append(" SELECT AgrupadorPadreID=ag.AgrupadorPadreID, ag.AgrupadorContenidoDigitalID,cd.Nombre,cd.ContenidoDigitalID,cd.Clave,doc.Extension ");
          query.Append(",rownumber= ROW_NUMBER()");
          query.Append(" OVER( ORDER BY ");
          query.Append(string.Format(" ag.{0}", sortColumn));
          query.Append(string.Format(" {0}", sortorder));
          query.Append(" )");
          query.Append(" FROM Curso ag");
          query.Append(" INNER JOIN CursoDetalle adet on ag.AgrupadorContenidoDigitalID = adet.AgrupadorContenidoDigitalID");
          query.Append(" INNER JOIN ContenidoDigital cd on adet.ContenidoDigitalID = cd.ContenidoDigitalID");
          query.Append(" INNER JOIN TipoDocumento doc ON cd.TipoDocumentoID = doc.TipoDocumentoID ");
         

          //Condiciones
          if (parametros.ContainsKey("CursoID"))
          {
              if (swhere.Length > 0) swhere += " AND ";
              swhere += "ag.AgrupadorContenidoDigitalID =  @CursoID";
              dbParameter = dbCommand.CreateParameter();
              dbParameter.ParameterName = "@CursoID";
              dbParameter.DbType = DbType.Int32;
              dbParameter.Value = parametros["CursoID"].Trim();
              dbCommand.Parameters.Add(dbParameter);
          }
          //Condiciones
          if (parametros.ContainsKey("AgrupadorPadreID"))
          {
              if (swhere.Length > 0) swhere += " AND ";
              swhere += "ag.AgrupadorPadreID =  @AgrupadorPadreID";
              dbParameter = dbCommand.CreateParameter();
              dbParameter.ParameterName = "@AgrupadorPadreID";
              dbParameter.DbType = DbType.Int32;
              dbParameter.Value = parametros["AgrupadorPadreID"].Trim();
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

          queryCols.Append("SELECT CursosDetalle.AgrupadorPadreID,CursosDetalle.AgrupadorContenidoDigitalID,CursosDetalle.Nombre,CursosDetalle.ContenidoDigitalID,CursosDetalle.Clave,CursosDetalle.Extension ");
          queryCols.Append(" FROM ( ");
          queryCols.Append(query.ToString());
          queryCols.Append(" ) as CursosDetalle");
          queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
          queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");

          DataSet ds = new DataSet();
          try
          {
              DbDataAdapter adapter = dctx.CreateDataAdapter();
              dbCommand.CommandText = queryCols.ToString();
              adapter.SelectCommand = dbCommand;
              adapter.Fill(ds, "CursosDetalle");
          }
          catch (Exception ex)
          {
              throw new Exception(string.Format("CursoDetalleDARetHlp: Ocurrió un problema al recuperar los detalles de curso: {0}", ex.Message));
          }
          return ds;

      }

      /// <summary>
      /// Consulta registros de AAgrupadorContenidoDigital en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="aAgrupadorContenidoDigital">AAgrupadorContenidoDigital que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de AAgrupadorContenidoDigital generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido, long? contenidoDigitalID = null)
      {
          object myFirm = new object();
          string sError = String.Empty;
          if (agrupadorContenido == null)
              sError += ", AAgrupadorContenidoDigital";
          if (sError.Length > 0)
              throw new Exception("CursoDetalleRetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
          if (agrupadorContenido.AgrupadorContenidoDigitalID == null)
              sError += ", AgrupadorContenidoID";
          if (sError.Length > 0)
              throw new Exception("CursoDetalleRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
          if (dctx == null)
              throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DA",
                 "CursoDetalleDARetHlp", "Action", null, null);
          DbCommand sqlCmd = null;
          try
          {
              dctx.OpenConnection(myFirm);
              sqlCmd = dctx.CreateCommand();
          }
          catch (Exception ex)
          {
              throw new StandardException(MessageType.Error, "", "CursoDetalleRetHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DA",
                 "CursoDetalleDARetHlp", "Action", null, null);
          }
          DbParameter sqlParam;
          StringBuilder sCmd = new StringBuilder();
          sCmd.Append(" SELECT  AgrupadorContenidoDigitalID, ContenidoDigitalID ");
          sCmd.Append(" FROM CursoDetalle ");
          sCmd.Append(" WHERE ");
          StringBuilder s_Var = new StringBuilder();
          if (agrupadorContenido.AgrupadorContenidoDigitalID != null)
          {
              s_Var.Append(" AgrupadorContenidoDigitalID = @dbp4ram1 ");
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "dbp4ram1";
              sqlParam.Value = agrupadorContenido.AgrupadorContenidoDigitalID;
              sqlParam.DbType = DbType.Int64;
              sqlCmd.Parameters.Add(sqlParam);
          }
          if (contenidoDigitalID != null)
          {
              s_Var.Append(" AND ContenidoDigitalID = @dbp4ram2 ");
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "dbp4ram2";
              sqlParam.Value = contenidoDigitalID;
              sqlParam.DbType = DbType.Int64;
              sqlCmd.Parameters.Add(sqlParam);
          }
          s_Var.Append("  ");
          string s_Varres = s_Var.ToString().Trim();
          if (s_Varres.Length > 0)
          {
              if (s_Varres.StartsWith("AND "))
                  s_Varres = s_Varres.Substring(4);
              else if (s_Varres.StartsWith("OR "))
                  s_Varres = s_Varres.Substring(3);
              else if (s_Varres.StartsWith(","))
                  s_Varres = s_Varres.Substring(1);
              sCmd.Append("  " + s_Varres);
          }
          DataSet ds = new DataSet();
          DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
          sqlAdapter.SelectCommand = sqlCmd;
          try
          {
              sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
              sqlAdapter.Fill(ds, "CursoDetalle");
          }
          catch (Exception ex)
          {
              string exmsg = ex.Message;
              try { dctx.CloseConnection(myFirm); }
              catch (Exception) { }
              throw new Exception("CursoDetalleRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
