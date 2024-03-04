using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using System.Data;
using System.Data.Common;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DA
{
    /// <summary>
    /// Realiza la consulta de publicaciones en los muros de los contactos
    /// </summary>
    internal class PublicacionesMurosDARetHlp
    {
        /// <summary>
        /// Consulta publicaciones en los muros que recibe como parametro.
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="pageSize">tamanio de la pagina</param>
        /// <param name="currentPage">pagina actual</param>
        /// <param name="sortColumn">ordenacion de la columna</param>
        /// <param name="sortorder">orden</param>
        /// <param name="parametros"></param>
        /// <param name="muros"></param>
        /// <param name="privacidades"></param>
        /// <returns></returns>
        public DataSet Action(IDataContext dctx, int pageSize, int currentPage,
            string sortColumn, string sortorder, Dictionary<string, string> parametros, List<Int64?> muros, List<IPrivacidad> privacidades)
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
            StringBuilder queryPrivacidad = new StringBuilder();
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
                throw new Exception(string.Format("PublicacionesDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }

            queryPrivacidad.Append(" SELECT priv.PublicacionID FROM Privacidad priv ");
            string sWherePrivacidad = string.Empty;

            //privacidad
            foreach (IPrivacidad privacidad in privacidades)
            {
                if (sWherePrivacidad.Length > 0) sWherePrivacidad += " OR ";

                if (privacidad is GrupoSocial)
                {
                    GrupoSocial gs = (GrupoSocial)privacidad;
                    sWherePrivacidad += " priv.GrupoSocialID = " + gs.GrupoSocialID.ToString() + " ";
                }
                else if (privacidad is UsuarioSocial)
                {
                    UsuarioSocial us = (UsuarioSocial)privacidad;
                    sWherePrivacidad += " priv.UsuarioSocialID = " + us.UsuarioSocialID.ToString() + " ";
                }
            }

            if (sWherePrivacidad.Length > 0) queryPrivacidad.AppendFormat(" WHERE {0} ", sWherePrivacidad);

            DataSet ds = new DataSet();
            pageIndex = currentPage - 1;

            swhere = string.Empty;
            query.Append(" SELECT");
            query.Append(" pub.PublicacionID, pub.Contenido, pub.FechaPublicacion,pub.SocialHubID, pub.UsuarioSocialID, pub.RankingID," + 
            "pub.Estatus,us.ScreenName,pub.AppSocialID,pub.TipoPublicacion, pub.JuegoID, pub.LibroID, pub.ContenidoDigitalID, rownumber= ROW_NUMBER()");
            query.Append(" OVER( ORDER BY ");
            query.Append(string.Format(" {0}", sortColumn));
            query.Append(string.Format(" {0}", sortorder));
            query.Append(" )");
            query.Append(" FROM ");
            query.Append(" Publicacion pub inner join UsuarioSocial us on pub.UsuarioSocialID = us.UsuarioSocialID ");
            query.Append(" INNER JOIN SocialHub shm ON shm.SocialHubID=pub.SocialHubID ");

            if (muros.Count > 0)
            {
                if (swhere.Length > 0) swhere += " AND ";
                System.Boolean primeravez = true;
                int i = 0;
                foreach (System.Int64 item in muros)
                {
                    if (primeravez)
                    {
                        primeravez = false;

                        swhere += "(pub.SocialHubID= @SocialHubID";
                        dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = "@SocialHubID";
                        dbParameter.DbType = DbType.Int64;
                        dbParameter.Value = item;
                        dbCommand.Parameters.Add(dbParameter);
                    }
                    else
                    {
                        i++;

                        if (swhere.Length > 0) swhere += " OR ";
                        swhere += "pub.SocialHubID= @SocialHubID" + i;
                        dbParameter = dbCommand.CreateParameter();
                        dbParameter.ParameterName = "@SocialHubID" + i;
                        dbParameter.DbType = DbType.Int64;
                        dbParameter.Value = item;
                        dbCommand.Parameters.Add(dbParameter);
                    }
                }
            }

            if (swhere.Length > 0) swhere += ") ";

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
            if (parametros.ContainsKey("SoloPropietario"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "shm.UsuarioSocialID = pub.UsuarioSocialID";
            }
            else
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "shm.UsuarioSocialID != pub.UsuarioSocialID";
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
                if (sWherePrivacidad.Length > 0)
                    swhere += " AND PublicacionID IN (" + queryPrivacidad.ToString() + ") ";

            }

            if (swhere.Length > 0) query.Append(" WHERE " + swhere + " AND pub.SocialHubID NOT IN (SELECT sh.SocialHubID FROM SocialHub sh WHERE sh.SocialProfileType=2) ");
            else query.Append(" WHERE pub.SocialHubID NOT IN (SELECT sh.SocialHubID FROM SocialHub sh WHERE sh.SocialProfileType=2) ");

            queryCols.Append("SELECT PublicacionID, Contenido, FechaPublicacion,SocialHubID, UsuarioSocialID, RankingID, Estatus, ScreenName,AppSocialID,TipoPublicacion, JuegoID, LibroID, ContenidoDigitalID ");
            queryCols.Append(" FROM ( ");
            queryCols.Append(query.ToString());
            queryCols.Append(" ) as Publicaciones");
            queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
            queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");

            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter();
                dbCommand.CommandText = queryCols.ToString();
                adapter.SelectCommand = dbCommand;
                adapter.Fill(ds, "Publicacion");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("PublicacionesMurosDARetHlp: Ocurrió un problema al recuperar las publicaciones: {0}", ex.Message));
            }

            return ds;
        }
    
    }
}
