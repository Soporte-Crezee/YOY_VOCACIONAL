using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DA
{
    internal class PublicacionDARetHlp
    {
        
        public DataSet Action(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder, 
            Dictionary<string, string> parametros, List<IPrivacidad> privacidades  )
        {

            if(pageSize<=0)
                throw  new Exception("El tamaño de página debe ser mayor a cero");
            if (currentPage < 0)
                throw new Exception("La página actual debe ser un número mayor que cero");
            if(parametros==null)
                throw new Exception("Los parámetros no pueden ser nulos");
            if(parametros.Count==0)
                throw  new Exception("El numéro de parámetros debe ser mayor a cero");
            if (privacidades == null)
                privacidades = new List<IPrivacidad>();

            int recordCount = 0;
            int pageCount = 0;
            int pageIndex = 0;

            //variables
            string swhere = string.Empty;
            string sError = string.Empty;
            StringBuilder query = new StringBuilder();
            StringBuilder queryControl = new StringBuilder();
            StringBuilder queryPrivacidad = new StringBuilder();
            StringBuilder queryCols =new StringBuilder();
            DbParameter dbParameter = null;
            DbCommand dbCommand = null;
            DbCommand dbCommandControl = null;
            object myFirm = new object();

            try
            {
                dctx.OpenConnection(myFirm);
                dbCommand = dctx.CreateCommand();
                dbCommandControl = dctx.CreateCommand();
            }catch(Exception ex)
            {
                throw new Exception(string.Format("PublicacionDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }

            StringBuilder sWherePrivacidad = new StringBuilder();
            //privacidad
            foreach (IPrivacidad privacidad in privacidades)
            {
                if (sWherePrivacidad.ToString().Length > 0) sWherePrivacidad.Append(" OR "); 

                if (privacidad is GrupoSocial)
                {
                    GrupoSocial gs = (GrupoSocial)privacidad;
                    sWherePrivacidad.Append(" priv.GrupoSocialID = ");
                    sWherePrivacidad.Append(gs.GrupoSocialID.ToString());
                    sWherePrivacidad.Append(" ");
                }
                else if (privacidad is UsuarioSocial)
                {
                    UsuarioSocial us = (UsuarioSocial)privacidad;
                    sWherePrivacidad.Append(" priv.UsuarioSocialID = ");
                    sWherePrivacidad.Append(us.UsuarioSocialID.ToString());
                    sWherePrivacidad.Append(" ");
                }
            }

            queryPrivacidad.Append(" SELECT priv.PublicacionID FROM Privacidad priv ");
            if (sWherePrivacidad.Length > 0) queryPrivacidad.AppendFormat(" WHERE {0} ", sWherePrivacidad.ToString());

            pageIndex = currentPage - 1;
            DataSet ds = new DataSet();

            swhere = string.Empty;
            query.Append(" SELECT");
            query.Append(" pub.PublicacionID, pub.Contenido, pub.FechaPublicacion,pub.SocialHubID, pub.UsuarioSocialID, pub.RankingID,"+
                " pub.Estatus,us.ScreenName,pub.AppSocialID,pub.TipoPublicacion,pub.JuegoID, pub.LibroID, pub.ContenidoDigitalID, rownumber= ROW_NUMBER()");
            query.Append(" OVER( ORDER BY ");
            query.Append(string.Format(" {0}", sortColumn));
            query.Append(string.Format(" {0}",sortorder));
            query.Append(" )");
            query.Append(" FROM ");
            query.Append(" Publicacion pub inner join UsuarioSocial us on pub.UsuarioSocialID = us.UsuarioSocialID ");
            
            //Condiciones
            if(parametros.ContainsKey("SocialHubID"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "SocialHubID =  @SocialHubID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@SocialHubID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = parametros["SocialHubID"].Trim();
                dbCommand.Parameters.Add(dbParameter);
            }
            if(parametros.ContainsKey("UsuarioSocialID"))
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
            if(pageSize>0)
            {   
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@PageSize";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = pageSize;
                dbCommand.Parameters.Add(dbParameter);
            }

            if(pageIndex>-2)
            {
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@PageIndex";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = pageIndex;
                dbCommand.Parameters.Add(dbParameter);
            }


            if (swhere.Length > 0)
            {
                if (sWherePrivacidad.ToString().Length > 0)
                    swhere += " AND PublicacionID IN (" + queryPrivacidad.ToString() + ")";
                query.Append(" WHERE " + swhere);
            }

            queryCols.Append("SELECT PublicacionID, Contenido, FechaPublicacion,SocialHubID, UsuarioSocialID, RankingID, Estatus, ScreenName,AppSocialID,TipoPublicacion, JuegoID, LibroID,ContenidoDigitalID ");
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
            catch(Exception ex)
            {
             throw  new Exception(string.Format("PublicacionDARetHlp: Ocurrió un problema al recuperar las publicaciones: {0}", ex.Message));    
            }
            return ds;
        }
    }
}
