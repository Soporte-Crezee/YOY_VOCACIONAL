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
    public class UsuarioSocialDARetHlp
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
            string queryContactos = "SELECT ug.UsuarioSocialID FROM SocialHub sh";
            queryContactos += " JOIN GrupoSocial gs ON sh.SocialHubID=gs.SocialHubID ";
            queryContactos += " JOIN UsuarioGrupo ug ON ug.GrupoSocialID=gs.GrupoSocialID ";
            queryContactos += " WHERE sh.UsuarioSocialID="+parametros["UsuarioSessionID"]+" ";

            string queryInvitacion = "SELECT inv.InvitadoID FROM Invitacion inv WHERE inv.RemitenteID=" + parametros["UsuarioSessionID"] + " AND inv.Estatus=1 AND inv.EsSolicitud=1 ";
            string queryRemitente = "SELECT inv.RemitenteID FROM Invitacion inv WHERE inv.InvitadoID=" + parametros["UsuarioSessionID"] + " AND inv.Estatus=1 AND inv.EsSolicitud=1 ";
            try
            {
                dctx.OpenConnection(myFirm);
                dbCommand = dctx.CreateCommand();
                dbCommandControl = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("UsuarioSocialDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }

            #region ControlData
            queryControl.Append(" SELECT");
            queryControl.Append(" COUNT(UsuarioSocialID) as RecordCount,");
            queryControl.Append(" Ceiling(cast(count(UsuarioSocialID) as float) / cast (@PageSize as float)) as PageCount");
            queryControl.Append(" FROM UsuarioSocial");

            //parametros
            dbParameter = dbCommandControl.CreateParameter();
            dbParameter.ParameterName = "@PageSize";
            dbParameter.DbType = DbType.Int32;
            dbParameter.Value = pageSize;
            dbCommandControl.Parameters.Add(dbParameter);

            if (parametros.ContainsKey("ScreenName"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " ScreenName LIKE  @ScreenName ";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@ScreenName";
                dbParameter.DbType = DbType.String;
                dbParameter.Value = parametros["ScreenName"].Trim();
                dbCommandControl.Parameters.Add(dbParameter);
            }
            
            if (swhere.Length > 0) queryControl.AppendFormat(" WHERE {0}", swhere +
                " AND UsuarioSocialID NOT IN (" + queryContactos + ") "
                + " AND UsuarioSocialID NOT IN (" + queryInvitacion + ") "
                 + " AND UsuarioSocialID NOT IN (" + queryRemitente + ") ");
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
                throw new Exception(string.Format("UsuarioSocialDARetHlp: Ocurrió un problema al recuperar los datos de control: {0}", ex.Message));
            }
            #endregion

            swhere = string.Empty;
            query.Append(" SELECT");
            query.Append(" us.UsuarioSocialID,us.Email,us.LoginName,us.ScreenName,us.Estatus,us.ImagenPerfil, rownumber= ROW_NUMBER()");
            query.Append(" OVER( ORDER BY ");
            query.Append(string.Format(" {0}", sortColumn));
            query.Append(string.Format(" {0}", sortorder));
            query.Append(" )");
            query.Append(" FROM ");
            query.Append(" UsuarioSocial us ");

            //Condiciones
            if (parametros.ContainsKey("ScreenName"))
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ScreenName LIKE  @ScreenName";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@ScreenName";
                dbParameter.DbType = DbType.String;
                dbParameter.Value = parametros["ScreenName"].Trim();
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


            if (swhere.Length > 0) query.Append(" WHERE " + swhere 
            + " AND us.UsuarioSocialID NOT IN (" + queryContactos + ")" +
            " AND us.UsuarioSocialID NOT IN (" + queryInvitacion + ")"
            + " AND us.UsuarioSocialID NOT IN (" + queryRemitente + ") ");

            queryCols.Append("SELECT UsuarioSocialID, Email, LoginName, ScreenName, Estatus,ImagenPerfil ");
            queryCols.Append(" FROM ( ");
            queryCols.Append(query.ToString());
            queryCols.Append(" ) as Usuarios ");
            queryCols.Append(" WHERE rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
            queryCols.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");

            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter();
                dbCommand.CommandText = queryCols.ToString();
                adapter.SelectCommand = dbCommand;
                adapter.Fill(ds, "UsuarioSocial");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("UsuarioSocialDARetHlp: Ocurrió un problema al recuperar las publicaciones: {0}", ex.Message));
            }
            return ds;
        }
    }
}
