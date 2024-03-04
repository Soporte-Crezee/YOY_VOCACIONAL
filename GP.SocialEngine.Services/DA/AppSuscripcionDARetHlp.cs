using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DA
{
   internal class AppSuscripcionDARetHlp
    {
        public DataSet Action(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder,SocialHub socialHub ,AppSuscripcion appSuscripcion)
        {
            if (pageSize <= 0)
                throw new Exception("El tamaño de la pagina debe ser mayor q cero");
            if (currentPage < 0)
                throw new Exception("La pagina actual debe ser un numero mayor que cero");
            if (appSuscripcion == null)
                throw new Exception("appSuscripcion no puede ser nulo");
            if(socialHub == null)
                throw new Exception("soscialHub no puede ser nulo");

            int recordCount = 0;
            int pageCount = 0;
            int pageIndex = 0;

            //Variables
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
                throw new Exception(string.Format("AppSuscripcionDARetHlp: No se puedo conectar con la base de datos {0}", ex.Message));
            }

            pageIndex = currentPage - 1;
            string notinwhere = string.Empty;
            DataSet ds = new DataSet();

            query.Append(" SELECT ");
            query.Append(" app.AppSuscripcionID, app.FechaRegistro, app.Estatus, app.SocialHubID, app.AppSocialID, app.JuegoID, app.AppType, rownumber= ROW_NUMBER() ");
            query.Append(" OVER( ORDER BY ");
            query.Append(string.Format(" {0}", sortColumn));
            query.Append(string.Format(" {0}", sortorder));
            query.Append(" ) ");
            query.Append(" FROM ");
            query.Append(" AppSuscripcion app ");
    
            //Condiciones
            if (appSuscripcion.AppSuscripcionID != null) 
            {
                swhere += " AppSuscripcionID = @appSuscripcion_AppSuscripcionID ";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@appSuscripcion_AppSuscripcionID ";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = appSuscripcion.AppSuscripcionID;
                dbCommand.Parameters.Add(dbParameter);
                }

             if (appSuscripcion.Estatus != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " Estatus = @appSuscripcion_Estatus ";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@appSuscripcion_Estatus";
                dbParameter.DbType = DbType.Boolean;
                dbParameter.Value = appSuscripcion.Estatus;
                dbCommand.Parameters.Add(dbParameter);

            }

            if (socialHub.SocialHubID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " SocialHubID = @socialHub_SocialHubID ";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@socialHub_SocialHubID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = socialHub.SocialHubID;
                dbCommand.Parameters.Add(dbParameter);
            }
            if (appSuscripcion.AppType != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " AppType = @appSuscripcion_AppType ";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@appSuscripcion_AppType";
                dbParameter.DbType = DbType.Int16;
                dbParameter.Value = appSuscripcion.AppType;
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
                query.Append(" WHERE " + swhere + "");
            }
            queryCols.Append(" SELECT AppSuscripcionID, FechaRegistro, Estatus, SocialHubID, AppSocialID, JuegoID, AppType ");
            queryCols.Append(" FROM ( ");
            queryCols.Append(query.ToString());
            queryCols.Append(" ) as AppSuscripcion ");
            queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10)) ");
            queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10)) ");

            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter();
                dbCommand.CommandText = queryCols.ToString();
                adapter.SelectCommand = dbCommand;
                adapter.Fill(ds, "AppSuscripcion");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("AppSuscripcionDARetHlp: Ocurrio un problema al recuperar los juegos: {0}", ex.Message));
            }

            return ds;
        }
    }
}
