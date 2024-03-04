using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DA {
    /// <summary>
    /// Consulta un registro de EjeTematico en la BD
    /// </summary>
   internal class EjeTematicoDARetHlp {
       /// <summary>
       /// Consulta registros de EjeTematico en la base de datos.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="ejeTematico">EjeTematico que provee el criterio de selección para realizar la consulta</param>
       /// <returns>El DataSet que contiene la información de EjeTematico generada por la consulta</returns>
       public DataSet Action(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder,
             Dictionary<string, string> parametros)
       {

           if (pageSize <= 0)
               throw new Exception("El tamaño de página debe ser mayor a cero");
           if (currentPage < 0)
               throw new Exception("La página actual debe ser un número mayor que cero");
           if (parametros == null)
               throw new Exception("Los parámetros no pueden ser nulos");
           //if (parametros.Count == 0)
           //    throw new Exception("El numéro de parámetros debe ser mayor a cero");

           //int recordCount = 0;
           //int pageCount = 0;
           int pageIndex = 0;

           //variables
           string swhere = string.Empty;
           string sError = string.Empty;
           StringBuilder query = new StringBuilder();
           StringBuilder queryControl = new StringBuilder();
           StringBuilder queryColsIn = new StringBuilder();
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
               throw new Exception(string.Format("ReactivoDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
           }
           
           pageIndex = currentPage - 1;

           swhere = string.Empty;
           query.Append(" SELECT DISTINCT e.EjeTematicoID, ");
           query.Append(" a.Nombre AS NombreArea ");           
           query.Append(" FROM EjeTematico e ");
           query.Append(" INNER JOIN AreaProfesionalizacion a ON a.AreaProfesionalizacionID = e.AreaProfesionalizacionID ");
           query.Append(" INNER JOIN EjeTematicoMateriaProfesionalizacion em ON e.EjeTematicoID = em.EjeTematicoID ");           
           query.Append(" INNER JOIN EjeContrato ec ON ec.EjeTematicoID = e.EjeTematicoID ");

           //Condiciones           
           if (parametros.ContainsKey("NombreEjeTematico"))
           {
               if (swhere.Length > 0) swhere += " AND ";
               swhere += "e.Nombre COLLATE SQL_LATIN1_GENERAL_CP1_CI_AI LIKE @NombreEjeTematico";
               dbParameter = dbCommand.CreateParameter();
               dbParameter.ParameterName = "@NombreEjeTematico";
               dbParameter.DbType = DbType.String;
               dbParameter.Value = parametros["NombreEjeTematico"].Trim();
               dbCommand.Parameters.Add(dbParameter);
           }
           if (parametros.ContainsKey("AreaProfesionalizacionID"))
           {
               if (swhere.Length > 0) swhere += " AND ";
               swhere += "a.AreaProfesionalizacionID =  @AreaProfesionalizacionID";
               dbParameter = dbCommand.CreateParameter();
               dbParameter.ParameterName = "@AreaProfesionalizacionID";
               dbParameter.DbType = DbType.Int32;
               dbParameter.Value = parametros["AreaProfesionalizacionID"].Trim();
               dbCommand.Parameters.Add(dbParameter);
           }
           if (parametros.ContainsKey("AreaProfesionalizacionActivo"))
           {
               if (swhere.Length > 0) swhere += " AND ";
               swhere += "a.Activo =  @AreaProfesionalizacionActivo";
               dbParameter = dbCommand.CreateParameter();
               dbParameter.ParameterName = "@AreaProfesionalizacionActivo";
               dbParameter.DbType = DbType.Boolean;
               dbParameter.Value = parametros["AreaProfesionalizacionActivo"].Trim();
               dbCommand.Parameters.Add(dbParameter);
           }
           if (parametros.ContainsKey("NivelEducativoID"))
           {
               if (swhere.Length > 0) swhere += " AND ";
               swhere += "a.NivelEducativoID =  @NivelEducativoID";
               dbParameter = dbCommand.CreateParameter();
               dbParameter.ParameterName = "@NivelEducativoID";
               dbParameter.DbType = DbType.Int32;
               dbParameter.Value = parametros["NivelEducativoID"].Trim();
               dbCommand.Parameters.Add(dbParameter);
           }
           if (parametros.ContainsKey("Grado"))
           {
               if (swhere.Length > 0) swhere += " AND ";
               swhere += "a.Grado =  @Grado";
               dbParameter = dbCommand.CreateParameter();
               dbParameter.ParameterName = "@Grado";
               dbParameter.DbType = DbType.Int16;
               dbParameter.Value = parametros["Grado"].Trim();
               dbCommand.Parameters.Add(dbParameter);
           }
           if (parametros.ContainsKey("MateriaID"))
           {
               if (swhere.Length > 0) swhere += " AND ";
               swhere += "em.MateriaID =  @MateriaID";
               dbParameter = dbCommand.CreateParameter();
               dbParameter.ParameterName = "@MateriaID";
               dbParameter.DbType = DbType.Int64;
               dbParameter.Value = parametros["MateriaID"].Trim();
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
           
           swhere += " AND e.EstatusProfesionalizacion = @EstatusProfesionalizacion";
           dbParameter = dbCommand.CreateParameter();
           dbParameter.ParameterName = "@EstatusProfesionalizacion";
           dbParameter.DbType = DbType.Byte;
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

           queryColsIn.Append("SELECT eje.EjeTematicoID, ");
           queryColsIn.Append("eje.Nombre AS NombreEjeTematico, ");
           queryColsIn.Append("ex.NombreArea, ");
           queryColsIn.Append("eje.Descripcion, ");
           queryColsIn.Append("eje.EstatusProfesionalizacion AS EstatusEjeTematico, ");
           queryColsIn.Append("eje.FechaRegistro, rownumber= ROW_NUMBER() ");
           queryColsIn.Append("OVER( ORDER BY ");
           queryColsIn.Append(string.Format(" {0}", "eje." + sortColumn));
           queryColsIn.Append(string.Format(" {0}", sortorder));
           queryColsIn.Append(" )");
           queryColsIn.Append(" FROM (");
           queryColsIn.Append(query.ToString());
           queryColsIn.Append(" ) as ex");
           queryColsIn.Append(" JOIN EjeTematico eje ON eje.EjeTematicoID = ex.EjeTematicoID ");

           queryCols.Append("SELECT EjeTematicoID, NombreEjeTematico, NombreArea, Descripcion, FechaRegistro, EstatusEjeTematico");
           queryCols.Append(" FROM ( ");
           queryCols.Append(queryColsIn.ToString());
           queryCols.Append(" ) as EjesTematicos");
           queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
           queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");

           DataSet ds = new DataSet();
           try
           {
               DbDataAdapter adapter = dctx.CreateDataAdapter();
               dbCommand.CommandText = queryCols.ToString();
               adapter.SelectCommand = dbCommand;
               adapter.Fill(ds, "EjeTematico");
           }
           catch (Exception ex)
           {
               throw new Exception(string.Format("PublicacionDARetHlp: Ocurrió un problema al recuperar las publicaciones: {0}", ex.Message));
           }
           return ds;
         
      }
   } 
}
