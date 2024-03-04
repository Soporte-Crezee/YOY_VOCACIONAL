using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO
{
    /// <summary>
    /// Actualiza un registro de AppSuscripcion en la BD
    /// </summary>
    public class AppSuscripcionUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de AppSuscripcionUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="appSuscripcionUpdHlp">AppSuscripcionUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">AppSuscripcionUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, AppSuscripcion appSuscripcion, AppSuscripcion anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (appSuscripcion == null)
                sError += ", AppSuscripcion";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("AppSuscripcionUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.AppSuscripcionID == null)
                sError += ", Anterior AppSuscripcionID";
            if (sError.Length > 0)
                throw new Exception("AppSuscripcionUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "AppSuscripcionUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AppSuscripcionUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO",
                   "AppSuscripcionUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE AppSuscripcion ");
            sCmd.Append(" SET Estatus = @appSuscripcion_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "appSuscripcion_Estatus";
            if (appSuscripcion.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = appSuscripcion.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            if (anterior.AppSuscripcionID == null)
                sCmd.Append(" WHERE AppSuscripcionID IS NULL ");
            else
            {
                sCmd.Append(" WHERE AppSuscripcionID = @anterior_AppSuscripcionID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "anterior_AppSuscripcionID";
                sqlParam.Value = anterior.AppSuscripcionID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
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
                throw new Exception("AppSuscripcionUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("AppSuscripcionUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
