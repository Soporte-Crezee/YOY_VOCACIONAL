using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;

namespace GP.SocialEngine.DA
{
    public class PublicacionesVotadasDARetHlp
    {
        public DataSet Action(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder, Dictionary<string, string> parametros)
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
                throw new Exception(string.Format("PublicacionDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }
            #region ControlData
            queryControl.Append(" SELECT");
            queryControl.Append(" COUNT(PublicacionID) as RecordCount,");
            queryControl.Append(" Ceiling(cast(count(PublicacionID) as float) / cast (@PageSize as float)) as PageCount");
            queryControl.Append(" FROM Publicacion");

            //parametros
            dbParameter = dbCommandControl.CreateParameter();
            dbParameter.ParameterName = "@PageSize";
            dbParameter.DbType = DbType.Int32;
            dbParameter.Value = pageSize;
            dbCommandControl.Parameters.Add(dbParameter);

            if (parametros.ContainsKey("SocialHubID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "SocialHubID =  @SocialHubID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@SocialHubID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = parametros["SocialHubID"].Trim();
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("Estatus"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "Estatus = @Estatus";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Estatus";
                dbParameter.DbType = DbType.Boolean;
                dbParameter.Value = Convert.ToBoolean(int.Parse(parametros["Estatus"].Trim()));
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (swhere.Length > 0) queryControl.AppendFormat(" WHERE {0}", swhere);


            DataSet ds = new DataSet();
            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter();
                dbCommandControl.CommandText = queryControl.ToString();
                adapter.SelectCommand = dbCommandControl;
                adapter.Fill(ds, "ControlData");
                recordCount = (int)ds.Tables["ControlData"].Rows[0]["RecordCount"];
                pageCount = Convert.ToInt32(ds.Tables["ControlData"].Rows[0]["PageCount"]);
                pageIndex = currentPage - 1;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("PublicacionDARetHlp: Ocurrió un problema al recuperar los datos de control: {0}", ex.Message));
            }
            #endregion

            swhere = string.Empty;
            query.Append(" SELECT");
            query.Append(" pub.PublicacionID, pub.Contenido, pub.FechaPublicacion,pub.SocialHubID, pub.UsuarioSocialID, pub.RankingID, pub.Estatus,us.ScreenName,SUM(usr.Puntuacion) as Votes,pub.AppSocialID,pub.TipoPublicacion,  rownumber= ROW_NUMBER() ");
            query.Append(" OVER( ORDER BY FechaPublicacion DESC) ");
            query.Append(" FROM ");
            query.Append(" Publicacion pub inner join UsuarioSocial us on pub.UsuarioSocialID = us.UsuarioSocialID ");
            query.Append(" JOIN UsuarioSocialRanking usr ON usr.RankingID= pub.RankingID ");

            //Condiciones
            if (parametros.ContainsKey("SocialHubID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "SocialHubID =  @SocialHubID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@SocialHubID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = parametros["SocialHubID"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("UsuarioSocialID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "UsuarioSocialID =  @UsuarioSocialID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@UsuarioSocialID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = parametros["UsuarioSocialID"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("Contenido"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "Contenido LIKE  @Contenido";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Contenido";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = parametros["Contenido"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }

            if (parametros.ContainsKey("FechaInicial"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "Fecha >= @FechaInicial";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@FechaInicial";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = parametros["FechaInicial"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }

            if (parametros.ContainsKey("FechaFinal"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "FechaFinal <= @FechaFinal";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@FechaFinal";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = parametros["FechaFinal"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            if (parametros.ContainsKey("Estatus"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "pub.Estatus = @Estatus";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Estatus";
                dbParameter.DbType = DbType.Boolean;
                dbParameter.Value = Convert.ToBoolean(int.Parse(parametros["Estatus"].Trim()));
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

            string votesQuery = " GROUP BY pub.PublicacionID, pub.Contenido, pub.FechaPublicacion,pub.SocialHubID, pub.UsuarioSocialID, pub.RankingID, pub.Estatus,us.ScreenName ";
            if (swhere.Length > 0) query.Append(" WHERE " + swhere + " " + votesQuery);
            else query.Append(" WHERE " + votesQuery);

            queryCols.Append("SELECT PublicacionID, Contenido, FechaPublicacion,SocialHubID, UsuarioSocialID, RankingID, Estatus, ScreenName,AppSocialID,TipoPublicacion");
            queryCols.Append(" FROM ( ");
            queryCols.Append(query.ToString());
            queryCols.Append(" ) as Publicaciones");
            queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
            queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10)) ORDER BY  Votes DESC");

            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter();
                dbCommand.CommandText = queryCols.ToString();
                adapter.SelectCommand = dbCommand;
                adapter.Fill(ds, "Publicacion");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("PublicacionDARetHlp: Ocurrió un problema al recuperar las publicaciones: {0}", ex.Message));
            }
            return ds;
        }

    }
}
