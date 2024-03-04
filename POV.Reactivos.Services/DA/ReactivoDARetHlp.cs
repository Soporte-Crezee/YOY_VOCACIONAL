using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;

namespace POV.Reactivos.DA
{
    public class ReactivoDARetHlp
    {
        public DataSet Action(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder, Dictionary<string, string> parametros)
        {

            if (pageSize <= 0)
                throw new Exception("El tamaño de página debe ser mayor a cero");
            if (currentPage < 0)
                throw new Exception("La página actual debe ser un número mayor que cero");
            if (parametros == null)
                throw new Exception("Los parámetros no pueden ser nulos");
            
            int recordCount = 0;
            int pageCount = 0;
            int pageIndex = 0;

            //variables
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
                throw new Exception(string.Format("ReactivoDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }
            #region ControlData
            queryControl.Append(" SELECT");
            queryControl.Append(" COUNT(ReactivoID) as RecordCount,");
            queryControl.Append(" Ceiling(cast(count(ReactivoID) as float) / cast (@PageSize as float)) as PageCount");
            queryControl.Append(" FROM Reactivo ");

            //parametros
            dbParameter = dbCommandControl.CreateParameter();
            dbParameter.ParameterName = "@PageSize";
            dbParameter.DbType = DbType.Int32;
            dbParameter.Value = pageSize;
            dbCommandControl.Parameters.Add(dbParameter);

            if (parametros.ContainsKey("NombreReactivo"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "NombreReactivo LIKE  @NombreReactivo";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@NombreReactivo";
                dbParameter.DbType = DbType.String;
                dbParameter.Value = parametros["NombreReactivo"].Trim();
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("AreaAplicacionID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "AreaAplicacionID =  @AreaAplicacionID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@AreaAplicacionID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = parametros["AreaAplicacionID"].Trim();
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("TipoComplejidadID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "TipoComplejidadID =  @TipoComplejidadID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@TipoComplejidadID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = parametros["TipoComplejidadID"].Trim();
                dbCommandControl.Parameters.Add(dbParameter);
            }

            if (swhere.Length > 0) swhere += " AND ";
            swhere += "Activo =  @Activo";
            dbParameter = dbCommand.CreateParameter();
            dbParameter.ParameterName = "@Activo";
            dbParameter.DbType = DbType.Boolean;
            dbParameter.Value = true;
            dbCommandControl.Parameters.Add(dbParameter);


            string notinwhere = string.Empty;
            if (swhere.Length > 0)
            {
                if (notinwhere.Length > 0)
                {
                    swhere += " AND ";
                    swhere += " ReactivoID NOT IN (Select AppSocialID FROM AppSuscripcion WHERE " + notinwhere + ") ";
                }

                queryControl.AppendFormat(" WHERE {0}", swhere);

            }
            else
            {
                if (notinwhere.Length > 0)
                {
                    swhere += " ReactivoID NOT IN (Select AppSocialID FROM AppSuscripcion WHERE " + notinwhere + ") ";
                    queryControl.AppendFormat(" WHERE {0}", swhere);
                }

            }

            DataSet ds = new DataSet();
            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter();
                dbCommandControl.CommandText = queryControl.ToString();
                adapter.SelectCommand = dbCommandControl;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ReactivoDARetHlp: Ocurrió un problema al recuperar los datos de control: {0}", ex.Message));
            }


            #endregion


            pageIndex = currentPage - 1;

            swhere = string.Empty;
            query.Append(" SELECT");
            query.Append(" re.ReactivoID, re.NombreReactivo, re.Valor, re.Vigencia, re.NumeroIntentos, re.Descripcion, re.Retroalimentacion, re.Activo, ");
            query.Append(" re.PlantillaReactivo, re.TipoComplejidadID, re.AreaAplicacionID, rownumber= ROW_NUMBER()");
            query.Append(" OVER( ORDER BY ");
            query.Append(string.Format(" {0}", sortColumn));
            query.Append(string.Format(" {0}", sortorder));
            query.Append(" )");
            query.Append(" FROM ");
            query.Append(" Reactivo re ");


            //Condiciones
            if (parametros.ContainsKey("NombreReactivo"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "NombreReactivo LIKE  @NombreReactivo ";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@NombreReactivo";
                dbParameter.DbType = DbType.String;
                dbParameter.Value = parametros["NombreReactivo"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("AreaAplicacionID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "AreaAplicacionID =  @AreaAplicacionID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@AreaAplicacionID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = parametros["AreaAplicacionID"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("TipoComplejidadID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "TipoComplejidadID =  @TipoComplejidadID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@TipoComplejidadID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = parametros["TipoComplejidadID"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }

            if (swhere.Length > 0) swhere += " AND ";
            swhere += "Activo =  @Activo";
            dbParameter = dbCommand.CreateParameter();
            dbParameter.ParameterName = "@Activo";
            dbParameter.DbType = DbType.Boolean;
            dbParameter.Value = true;
            dbCommand.Parameters.Add(dbParameter);

            if (swhere.Length > 0) swhere += " AND ";
            swhere += "DocenteID IS NULL";

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
                if (notinwhere.Length > 0)
                {
                    swhere += " AND ";
                    swhere += " ReactivoID NOT IN (Select AppSocialID FROM AppSuscripcion WHERE " + notinwhere + ") ";
                }

                query.Append(" WHERE " + swhere + "  ");
                
            }
            else
            {
                if (notinwhere.Length > 0)
                {
                    swhere += " ReactivoID NOT IN (Select AppSocialID FROM AppSuscripcion WHERE " + notinwhere + ") ";
                    query.Append(" WHERE " + swhere + "  ");
                }

            }



            queryCols.Append("SELECT ReactivoID, NombreReactivo, Valor, Vigencia,NumeroIntentos, PlantillaReactivo, TipoComplejidadID, AreaAplicacionID, Descripcion, Retroalimentacion, Activo, FechaRegistro=Vigencia");
            queryCols.Append(" FROM ( ");
            queryCols.Append(query.ToString());
            queryCols.Append(" ) as Reactivos");
            queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
            queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");



            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter();
                dbCommand.CommandText = queryCols.ToString();
                adapter.SelectCommand = dbCommand;
                adapter.Fill(ds, "Reactivo");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ReactivoDARetHlp: Ocurrió un problema al recuperar las publicaciones: {0}", ex.Message));
            }
            return ds;
        }
    }
}
