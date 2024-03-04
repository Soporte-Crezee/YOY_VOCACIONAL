using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;
using System.Collections.Generic;

namespace POV.Profesionalizacion.DA { 
   /// <summary>
   /// Consultar de la base de datos
   /// </summary>
   public class AsistenciaContenidoDetalleDARetHlp { 
      /// <summary>
      /// Consulta registros de Asistencia en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="agrupador">Agrupador de asistencia que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Asistencia generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, AAgrupadorContenidoDigital agrupador,long? contenidoDigitalID=null){
         object myFirm = new object();
         string sError = "";
         if (agrupador == null)
            sError += ", Asistencia";
         if (sError.Length > 0)
            throw new Exception("AsistenciaContenidoDigitalDARetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (agrupador.AgrupadorContenidoDigitalID == null)
            sError += ", Asistencia";
         if (sError.Length > 0)
            throw new Exception("AsistenciaContenidoDigitalDARetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DA", 
         "AsistenciaContenidoDigitalDARetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AsistenciaRetHlp: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DA", 
         "AsistenciaContenidoDigitalDARetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT AgrupadorContenidoDigitalID,ContenidoDigitalID ");
         sCmd.Append(" FROM AsistenciaDetalle ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (agrupador.AgrupadorContenidoDigitalID != null){
            s_VarWHERE.Append(" AgrupadorContenidoDigitalID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = agrupador.AgrupadorContenidoDigitalID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
          if (contenidoDigitalID != null)
          {
              s_VarWHERE.Append(" AND ContenidoDigitalID = @dbp4ram2 ");
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "dbp4ram2";
              sqlParam.Value = contenidoDigitalID;
              sqlParam.DbType = DbType.Int64;
              sqlCmd.Parameters.Add(sqlParam);
          }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
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
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "Asistencia");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AsistenciaRetHlp: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
      /// <summary>
      /// Consultar registros de Asistencia específicos de la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
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
          if (currentPage <= 0)
              throw new Exception("La página actual debe ser un número mayor que cero");
          if (parametros == null)
              throw new Exception("Los parámetros no pueden ser nulos");
          if (parametros.Count == 0)
              throw new Exception("El numéro de parámetros debe ser mayor a cero");

          int pageIndex = 0;

          string swhere = string.Empty;
          StringBuilder queryCols = new StringBuilder();

          object myFirm = new object();
          if (dctx == null)
              throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DA",
                 "AsistenciaContenidoDigitalDARetHlp", "Action", null, null);
          DbCommand sqlCmd = null;
          try
          {
              dctx.OpenConnection(myFirm);
              sqlCmd = dctx.CreateCommand();
          }
          catch (Exception ex)
          {
              throw new StandardException(MessageType.Error, "", "AsistenciaRetHlp: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DA",
                 "AsistenciaContenidoDigitalDARetHlp", "Action", null, null);
          }

          pageIndex = currentPage - 1;
          DbParameter sqlParam;
          StringBuilder sCmd = new StringBuilder();
          sCmd.Append(" SELECT a.AgrupadorPadreID, a.AgrupadorContenidoDigitalID,ad.ContenidoDigitalID,cd.Nombre,cd.Clave, TipoDocumento = td.Extension ");
          sCmd.Append(",rownumber= ROW_NUMBER()");
          sCmd.Append(" OVER( ORDER BY ");
          sCmd.Append(string.Format(" a.{0}", sortColumn));
          sCmd.Append(string.Format(" {0}", sortorder));
          sCmd.Append(" )");
          sCmd.Append(" FROM Asistencia a INNER JOIN AsistenciaDetalle ad ON ad.AgrupadorContenidoDigitalID = a.AgrupadorContenidoDigitalID ");
          sCmd.Append(" INNER JOIN ContenidoDigital cd ON cd.ContenidoDigitalID = ad.ContenidoDigitalID ");
          sCmd.Append(" INNER JOIN TipoDocumento td ON td.TipoDocumentoID = cd.TipoDocumentoID ");
          StringBuilder s_VarWHERE = new StringBuilder();

          if (parametros.ContainsKey("AsistenciaID"))
          {
              swhere += " a.AgrupadorContenidoDigitalID = @dbp4ram1 ";
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "dbp4ram1";
              sqlParam.Value = parametros["AsistenciaID"].Trim();
              sqlParam.DbType = DbType.Int32;
              sqlCmd.Parameters.Add(sqlParam);
          }
          if (parametros.ContainsKey("AgrupadorPadreID"))
          {
              if (s_VarWHERE.Length > 0) swhere += " AND ";
              swhere += "a.AgrupadorPadreID =  @AgrupadorPadreID";
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "@AgrupadorPadreID";
              sqlParam.DbType = DbType.Int32;
              sqlParam.Value = parametros["AgrupadorPadreID"].Trim();
              sqlCmd.Parameters.Add(sqlParam);
          }

          if (pageSize > 0)
          {
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "@PageSize";
              sqlParam.DbType = DbType.Int32;
              sqlParam.Value = pageSize;
              sqlCmd.Parameters.Add(sqlParam);

          }
          if (pageIndex > -2)
          {
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "@PageIndex";
              sqlParam.DbType = DbType.Int32;
              sqlParam.Value = pageIndex;
              sqlCmd.Parameters.Add(sqlParam);
          }

          if (swhere.Length > 0)
          {
              sCmd.Append(" WHERE " + swhere);
          }

          queryCols.Append(" SELECT AsistenciasDetalle.AgrupadorPadreID, AsistenciasDetalle.AgrupadorContenidoDigitalID, AsistenciasDetalle.ContenidoDigitalID,");
          queryCols.Append("AsistenciasDetalle.Nombre, AsistenciasDetalle.Clave, AsistenciasDetalle.TipoDocumento ");
          queryCols.Append(" FROM ( ");
          queryCols.Append(sCmd.ToString());
          queryCols.Append(") as AsistenciasDetalle");
          queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
          queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");

          DataSet ds = new DataSet();
          DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
          sqlAdapter.SelectCommand = sqlCmd;
          try
          {
              sqlCmd.CommandText = queryCols.ToString();
              sqlAdapter.Fill(ds, "AsistenciasDetalle");
          }
          catch (Exception ex)
          {
              string exmsg = ex.Message;
              try { dctx.CloseConnection(myFirm); }
              catch (Exception) { }
              throw new Exception("AsistenciaRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
