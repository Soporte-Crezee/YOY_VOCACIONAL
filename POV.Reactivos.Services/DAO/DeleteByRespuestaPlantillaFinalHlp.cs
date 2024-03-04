using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;

namespace POV.Reactivos.DAO
{
    /// <summary>
    /// Elimina los registros de OpcionRespuestaPlantillaFinal en la BD para una RespuestaPlantilla
    /// </summary>
    internal class DeleteByRespuestaPlantillaFinalHlp
    {
        /// <summary>
        /// Elimina un registro de DeleteByRespuestaPlantillaFinalHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="deleteByRespuestaPlantillaFinalHlp">DeleteByRespuestaPlantillaFinalHlp que desea eliminar</param>
        public void Action(IDataContext dctx, RespuestaPlantilla respuestaPlantilla)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (respuestaPlantilla == null)
                sError += ", RespuestaPlantilla";
            if (sError.Length > 0)
                throw new Exception("DeleteByRespuestaPlantillaFinalHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPlantilla.RespuestaPlantillaID == null)
                sError += ", RespuestaPlantillaID";
            if (sError.Length > 0)
                throw new Exception("DeleteByRespuestaPlantillaFinalHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "DeleteByRespuestaPlantillaFinalHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "DeleteByRespuestaPlantillaFinalHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "DeleteByRespuestaPlantillaFinalHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE OpcionRespuestaPlantillaFinal ");
            sCmd.Append(" SET Activo = 0 ");
            sCmd.Append(" WHERE RespuestaPlantillaID=@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (respuestaPlantilla.RespuestaPlantillaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPlantilla.RespuestaPlantillaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
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
                throw new Exception("DeleteByRespuestaPlantillaFinalHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("DeleteByRespuestaPlantillaFinalHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
