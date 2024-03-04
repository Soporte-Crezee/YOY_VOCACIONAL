using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;
using POV.ContenidosDigital.BO;

namespace POV.Profesionalizacion.DA { 
   /// <summary>
   /// Consulta un registro de AAgrupadorContenidoDigital en la BD
   /// </summary>
   internal class AgrupadorContenidoDARetHlp { 
      /// <summary>
      /// Consulta registros de AAgrupadorContenidoDigital en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="currentPage">numero de pagina</param>
      /// <param name="pageSize">tamaño de la pagina </param>
      /// <param name="parameters">otros filtros </param>
      /// <param name="aAgrupadorContenidoDigital">AAgrupadorContenidoDigital que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de AAgrupadorContenidoDigital generada por la consulta</returns>
       public DataSet Action(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenidoDigital, int pageSize, int currentPage, string sortColumn, string sortOrder, long contratoID)
       {
         object myFirm = new object();
         string sError = String.Empty;
         if (agrupadorContenidoDigital == null)
            sError += ", AAgrupadorContenidoDigital";
         if (sError.Length > 0)
            throw new Exception("AgrupadorContenidoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

         if (pageSize <= 0)
             throw new Exception("El tamaño de página debe ser mayor a cero");
         if (currentPage < 0)
             throw new Exception("La página actual debe ser un número mayor que cero");

      if (agrupadorContenidoDigital is AgrupadorCompuesto) {
         if ((agrupadorContenidoDigital as AgrupadorCompuesto).AgrupadorContenidoDigitalID == null)
            sError += ", AAgrupadorContenidoDigital.agrupadorContenidoDigitalID";
         if (sError.Length > 0)
            throw new Exception("AgrupadorContenidoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      }
      if (agrupadorContenidoDigital is AgrupadorSimple) {
         if ((agrupadorContenidoDigital as AgrupadorSimple).AgrupadorContenidoDigitalID == null)
            sError += ", AAgrupadorContenidoDigital.agrupadorContenidoDigitalID";
         if (sError.Length > 0)
            throw new Exception("AgrupadorContenidoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DA", 
         "AgrupadorContenidoDARetHlp", "Action", null, null);


         int recordCount = 0;
         int pageCount = 0;
         int pageIndex = 0;

         //Variables
         StringBuilder query = new StringBuilder();
         StringBuilder queryCols = new StringBuilder();
         DbParameter sqlParam;
         DbCommand sqlCmd = null;
         StringBuilder sCmd = new StringBuilder();
         StringBuilder s_Var = new StringBuilder();
         DataSet ds = new DataSet();
         
         try
         {
             dctx.OpenConnection(myFirm);
             sqlCmd = dctx.CreateCommand();
         }
         catch (Exception ex)
         {
             throw new StandardException(MessageType.Error, "", "AgrupadorContenidoDARetHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DA",
                "AgrupadorContenidoDARetHlp", "Action", null, null);
         }

         pageIndex = currentPage - 1;

         //Subconsulta de tablas
         query.Append(" SELECT aContenido.AgrupadorContenidoDigitalID,contenido.ContenidoDigitalID,aContenido.EstatusProfesionalizacion, ");
         query.Append(" contenido.TipoDocumentoID, contenido.Nombre AS NombreContenido, contenido.InstitucionOrigen,contenido.EstatusContenido, ");
         query.Append(" tipo.Nombre AS NombreTipo, tipo.ImagenDocumento, contenido.Tags, ");
         query.Append(" rowNumber = ROW_NUMBER() OVER (ORDER BY ");
         query.Append(string.Format(" {0}", sortColumn));
         query.Append(string.Format(" {0}", sortOrder));
         query.Append(" )");
         query.Append(" FROM AgrupadorContenidoDigital AS aContenido ");
         query.Append(" INNER JOIN  AgrupadorContenidoDigitalDetalle AS agrupador ON aContenido.AgrupadorContenidoDigitalID = agrupador.AgrupadorContenidoDigitalID  ");
         query.Append(" INNER JOIN ContenidoDigital AS contenido ON agrupador.ContenidoDigitalID = contenido.ContenidoDigitalID AND contenido.EstatusContenido = 1  ");
         query.Append(" INNER JOIN ContenidoDigitalAgrupador AS cda ON aContenido.AgrupadorContenidoDigitalID = cda.AgrupadorContenidoDigitalID and cda.ContenidoDigitalID = contenido.ContenidoDigitalID ");
         query.Append(" INNER JOIN TipoDocumento AS tipo ON contenido.TipoDocumentoID = tipo.TipoDocumentoID");
         query.Append(" INNER JOIN EjeContrato AS eco ON eco.EjeTematicoID = cda.EjeTematicoID  ");
         query.Append(" WHERE ");

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

         if (contratoID != null)
         {
             s_Var.Append(" AND eco.ContratoID = @dbp4ram1c ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "@dbp4ram1c";
             sqlParam.Value = contratoID;
             sqlParam.DbType = DbType.Int64;
             sqlCmd.Parameters.Add(sqlParam);
         }
         //Verificamos de que tipo es el Agrupador, si es simple o compuesto
         if (agrupadorContenidoDigital is AgrupadorCompuesto)
         {
             if ((agrupadorContenidoDigital as AgrupadorCompuesto).AgrupadorContenidoDigitalID != null)
             {
                 s_Var.Append(" AND agrupador.AgrupadorContenidoDigitalID = @dbp4ram1 ");
                 sqlParam = sqlCmd.CreateParameter();
                 sqlParam.ParameterName = "@dbp4ram1";
                 sqlParam.Value = (agrupadorContenidoDigital as AgrupadorCompuesto).AgrupadorContenidoDigitalID;
                 sqlParam.DbType = DbType.Int64;
                 sqlCmd.Parameters.Add(sqlParam);
             }

             //Verifica si el estatus es null, si lo es regresa el agrupador tanto "Activos" como en "Mantenimiento"
             if ((agrupadorContenidoDigital as AgrupadorCompuesto).Estatus != null)
             {
                 s_Var.Append(" AND aContenido.EstatusProfesionalizacion = @dbp4ram2 ");
                 sqlParam = sqlCmd.CreateParameter();
                 sqlParam.ParameterName = "@dbp4ram2";
                 sqlParam.Value = (agrupadorContenidoDigital as AgrupadorCompuesto).Estatus;
                 sqlParam.DbType = DbType.Byte;
                 sqlCmd.Parameters.Add(sqlParam);
             }
         }

         if (agrupadorContenidoDigital is AgrupadorSimple)
         {
             if ((agrupadorContenidoDigital as AgrupadorSimple).AgrupadorContenidoDigitalID != null)
             {
                 s_Var.Append(" AND agrupador.AgrupadorContenidoDigitalID = @dbp4ram1 ");
                 sqlParam = sqlCmd.CreateParameter();
                 sqlParam.ParameterName = "@dbp4ram1";
                 sqlParam.Value = (agrupadorContenidoDigital as AgrupadorSimple).AgrupadorContenidoDigitalID;
                 sqlParam.DbType = DbType.Int64;
                 sqlCmd.Parameters.Add(sqlParam);
             }
             //Verifica si el estatus es null, si lo es regresa el agrupador tanto "Activos" como en "Mantenimiento"
             if ((agrupadorContenidoDigital as AgrupadorSimple).Estatus != null)
             {
                 s_Var.Append(" AND aContenido.EstatusProfesionalizacion = @dbp4ram2 ");
                 sqlParam = sqlCmd.CreateParameter();
                 sqlParam.ParameterName = "@dbp4ram2";
                 sqlParam.Value = (agrupadorContenidoDigital as AgrupadorSimple).Estatus;
                 sqlParam.DbType = DbType.Byte;
                 sqlCmd.Parameters.Add(sqlParam);
             }
         }

         string s_Varres = s_Var.ToString().Trim();
         if (s_Varres.Length > 0)
         {
             if (s_Varres.StartsWith("AND "))
                 s_Varres = s_Varres.Substring(4);
             else if (s_Varres.StartsWith("OR "))
                 s_Varres = s_Varres.Substring(3);
             else if (s_Varres.StartsWith(","))
                 s_Varres = s_Varres.Substring(1);
             query.Append("  " + s_Varres);
         }

        
         //Consulta para paginar
         queryCols.Append(" SELECT DISTINCT AgrupadorContenidoDigitalID,ContenidoDigitalID,TipoDocumentoID,NombreContenido,InstitucionOrigen,NombreTipo,ImagenDocumento,Tags ");
         queryCols.Append(" FROM ( ");
         queryCols.Append(query.ToString());
         queryCols.Append(" ) as Contenido");
         queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
         queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");
         try
         {
             DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
             sqlCmd.CommandText = queryCols.Replace("@", dctx.ParameterSymbol).ToString();
             sqlAdapter.SelectCommand = sqlCmd;
             sqlAdapter.Fill(ds, "AAgrupadorContenidoDigital");
         }
         catch (Exception ex)
         {
             throw new Exception(string.Format("AgrupadorContenidoDARetHlp: Ocurrió un problema al recuperar los contenidos: {0}", ex.Message));
         }
            return ds;
      }
       /// <summary>
       /// Obtiene datos específicos del agrupador contenido digital.
       /// </summary>
       /// <param name="dctx">Data Context que proveerá el acceso a la base de datos.</param>
       /// <param name="contenidoDigital">Contenido digital proporcionado.</param>
       /// <returns>Regresa un dataset con los agrupadores de contenido.</returns>
       public DataSet Action(IDataContext dctx, ContenidoDigital contenidoDigital)
       {
           object myFirm = new object();
           string sError = String.Empty;
           if (contenidoDigital == null)
               sError += ", contenidoDigital";
           if (sError.Length > 0)
               throw new Exception("AgrupadorContenidoDARetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
           if (dctx == null)
               throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DA",
                  "AgrupadorContenidoDARetHlp", "Action", null, null);
           DbCommand sqlCmd = null;
           try
           {
               dctx.OpenConnection(myFirm);
               sqlCmd = dctx.CreateCommand();
           }
           catch (Exception ex)
           {
               throw new StandardException(MessageType.Error, "",
                                           "AgrupadorContenidoDARetHlp: No se pudo conectar a la base de datos",
                                           "POV.Profesionalizacion.DA",
                                           "AgrupadorContenidoDARetHlp", "Action", null, null);
           }
           DbParameter sqlParam;
           StringBuilder sCmd = new StringBuilder();
           sCmd.Append(" SELECT acd.AgrupadorPadreID,acdd.AgrupadorContenidoDigitalID, acdd.ContenidoDigitalID ");
           sCmd.Append(" FROM AgrupadorContenidoDigitalDetalle acdd ");
           sCmd.Append(" INNER JOIN AgrupadorContenidoDigital acd ON acd.AgrupadorContenidoDigitalID = acdd.AgrupadorContenidoDigitalID");
           StringBuilder s_VarWHERE = new StringBuilder();
           if (contenidoDigital.ContenidoDigitalID != null)
           {
               s_VarWHERE.Append(" AND ContenidoDigitalID = @dbp4ram2 ");
               sqlParam = sqlCmd.CreateParameter();
               sqlParam.ParameterName = "dbp4ram2";
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
               sqlAdapter.Fill(ds, "AgrupadorContenidoDigital");
           }
           catch (Exception ex)
           {
               string exmsg = ex.Message;
               try { dctx.CloseConnection(myFirm); }
               catch (Exception) { }
               throw new Exception("AgrupadorContenidoDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
