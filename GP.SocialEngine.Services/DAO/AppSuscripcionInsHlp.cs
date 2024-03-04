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
    /// Guarda un registro de AppSuscripcion en la BD
    /// </summary>
    public class AppSuscripcionInsHlp
    {
        /// <summary>
        /// Crea un registro de AppSuscripcion en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="appSuscripcion">AppSuscripcion que desea crear</param>
        public void Action(IDataContext dctx, SocialHub socialHub, AppSuscripcion appSuscripcion)
        {
            object myFirm = new object();
            string sError = String.Empty;
          
            if (appSuscripcion == null)
                sError += ", AppSuscripcion";
            if (sError.Length > 0)
                throw new Exception("AppSuscripcionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
                if (socialHub == null)
                sError += ", SocialHub";
            if (sError.Length > 0)
                throw new Exception("AppSuscripcionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (socialHub.SocialHubID == null)
                sError += ", SocialHubID";
            if (sError.Length > 0)
                throw new Exception("AppSuscripcionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (appSuscripcion.AppSocial == null)
                sError += ", AppSocial";
            if (sError.Length > 0)
                throw new Exception("AppSuscripcionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (appSuscripcion.AppSocial is Reactivo)
            {
                if ((appSuscripcion.AppSocial as Reactivo).ReactivoID == null)
                {
                    sError += ", ReactivoID";
                }
            }
            if (sError.Length > 0)
                throw new Exception("AppSuscripcionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (appSuscripcion.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (appSuscripcion.Estatus == null)
                sError += ", Estatus";
            if (appSuscripcion.AppType == null)
                sError += ", AppType";
            if (sError.Length > 0)
                throw new Exception("AppSuscripcionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "AppSuscripcionInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AppSuscripcionInsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO",
                   "AppSuscripcionInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO AppSuscripcion (FechaRegistro, Estatus, SocialHubID, AppSocialID, JuegoID, AppType) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            sCmd.Append(" @appSuscripcion_FechaRegistro ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "appSuscripcion_FechaRegistro";
            if (appSuscripcion.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = appSuscripcion.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@appSuscripcion_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "appSuscripcion_Estatus";
            if (appSuscripcion.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = appSuscripcion.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@socialHub_SocialHubID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "socialHub_SocialHubID";
            if (socialHub.SocialHubID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = socialHub.SocialHubID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);

            if (appSuscripcion.AppSocial is Reactivo) {

                sCmd.Append(" ,@appSuscripcion_Reactivo, NULL ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "appSuscripcion_Reactivo";
                if ((appSuscripcion.AppSocial as Reactivo).ReactivoID == null)
                    sqlParam.Value = DBNull.Value;
                else
                    sqlParam.Value = (appSuscripcion.AppSocial as Reactivo).ReactivoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }

            sCmd.Append(" ,@appSuscripcion_AppType ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "appSuscripcion_AppType";
            if (appSuscripcion.AppType == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = appSuscripcion.AppType;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ) ");
            int iRes = 0;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                iRes = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AppSuscripcionInsHlp: OcurriÃ³ un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("AppSuscripcionInsHlp: OcurriÃ³ un error al ingresar el registro.");
        }
    }
}
