using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Modelo.BO;

namespace POV.Reactivos.DAO
{
    /// <summary>
    /// Actualiza un registro de RespuestaPlantilla en la BD
    /// </summary>
    internal class RespuestaPlantillaDinamicoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de RespuestaPlantillaDinamicoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPlantillaDinamicoUpdHlp">RespuestaPlantillaDinamicoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RespuestaPlantillaDinamicoUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, RespuestaPlantilla respuestaPlantilla, RespuestaPlantilla anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (respuestaPlantilla == null)
                sError += ", RespuestaPlantilla";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaDinamicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.RespuestaPlantillaID == null)
                sError += ", Anterior RespuestaPlantillaID";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaDinamicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "RespuestaPlantillaDinamicoUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPlantillaDinamicoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Reactivos.DAO",
                   "RespuestaPlantillaDinamicoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE RespuestaPlantillaDinamico ");
            sCmd.Append(" SET ");
            if (respuestaPlantilla.Estatus != null)
            {
                sCmd.Append(" Estatus = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = respuestaPlantilla.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.TipoPuntaje != null)
            {
                sCmd.Append(" ,TipoPuntaje = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = respuestaPlantilla.TipoPuntaje;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.RespuestaPlantillaID == null)
                sCmd.Append(" WHERE RespuestaPlantillaID IS NULL ");
            else
            {
                // anterior.RespuestaPlantillaID
                sCmd.Append(" WHERE RespuestaPlantillaID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = anterior.RespuestaPlantillaID;
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
                throw new Exception("RespuestaPlantillaDinamicoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RespuestaPlantillaDinamicoUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
