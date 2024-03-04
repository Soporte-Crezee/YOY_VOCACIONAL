using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using POV.Reactivos.BO;

namespace GP.SocialEngine.DAO
{
    /// <summary>
    /// Consulta un registro de AppSuscripcion en la BD
    /// </summary>
    internal class AppSuscripcionRetHlp
    {
        /// <summary>
        /// Consulta registros de AppSuscripcion en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="appSuscripcion">AppSuscripcion que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de AppSuscripcion generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, SocialHub socialHub, AppSuscripcion appSuscripcion)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (appSuscripcion == null)
                sError += ", AppSuscripcion";
            if (sError.Length > 0)
                throw new Exception("AppSuscripcionRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (appSuscripcion.AppType == null)
                sError += ", AppType";
            if (sError.Length > 0)
                throw new Exception("AppSuscripcionRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (socialHub == null)
            {
                socialHub = new SocialHub();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "AppSuscripcionRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AppSuscripcionRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO",
                   "AppSuscripcionRetHlp", "Action", null, null);
            }
          
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ");
            sCmd.Append(" AppSuscripcionID, FechaRegistro, Estatus, AppSocialID, JuegoID, AppType ");
            sCmd.Append(" FROM AppSuscripcion ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (appSuscripcion.AppSuscripcionID != null)
            {
                s_VarWHERE.Append(" AppSuscripcionID = @appSuscripcion_AppSuscripcionID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "appSuscripcion_AppSuscripcionID";
                sqlParam.Value = appSuscripcion.AppSuscripcionID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (appSuscripcion.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @appSuscripcion_FechaRegistro ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "appSuscripcion_FechaRegistro";
                sqlParam.Value = appSuscripcion.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (appSuscripcion.Estatus != null)
            {
                s_VarWHERE.Append(" AND Estatus = @appSuscripcion_Estatus ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "appSuscripcion_Estatus";
                sqlParam.Value = appSuscripcion.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (socialHub.SocialHubID != null)
            {
                s_VarWHERE.Append(" AND SocialHubID = @socialHub_SocialHubID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "socialHub_SocialHubID";
                sqlParam.Value = socialHub.SocialHubID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (appSuscripcion.AppSocial != null)
            {
                if (appSuscripcion.AppSocial is Reactivo && (appSuscripcion.AppSocial as Reactivo).ReactivoID != null)
                {
                    s_VarWHERE.Append(" AND AppSocialID = @appSuscripcion_Reactivo ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "appSuscripcion_Reactivo";
                    sqlParam.Value = ((Reactivo)appSuscripcion.AppSocial).ReactivoID;
                    sqlParam.DbType = DbType.Guid;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }

            if (appSuscripcion.AppType != null)
            {
                s_VarWHERE.Append(" AND AppType = @appSuscripcion_AppType ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "appSuscripcion_AppType";
                sqlParam.Value = appSuscripcion.AppType;
                sqlParam.DbType = DbType.Int16;
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
                sqlAdapter.Fill(ds, "AppSuscripcion");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AppSuscripcionRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
