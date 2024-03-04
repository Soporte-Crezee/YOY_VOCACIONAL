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
    /// Elimina un registro de OpcionRespuestaModeloGenerico en la BD
    /// </summary>
    internal class OpcionRespuestaPlantillaDinamicoDelHlp
    {
        /// <summary>
        /// Elimina un registro de OpcionRespuestaPlantillaDinamicoDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="opcionRespuestaPlantillaDinamicoDelHlp">OpcionRespuestaPlantillaDinamicoDelHlp que desea eliminar</param>
        public void Action(IDataContext dctx, OpcionRespuestaModeloGenerico opcionRespuestaPlantilla)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (opcionRespuestaPlantilla == null)
                sError += ", OpcionRespuestaPlantillaDinamico";
            if (sError.Length > 0)
                throw new Exception("OpcionRespuestaPlantillaDinamicoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (opcionRespuestaPlantilla.OpcionRespuestaPlantillaID == null)
                sError += ", OpcionRespuestaPlantillaID";
            if (sError.Length > 0)
                throw new Exception("OpcionRespuestaPlantillaDinamicoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "OpcionRespuestaPlantillaDinamicoDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "OpcionRespuestaPlantillaDinamicoDelHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "OpcionRespuestaPlantillaDinamicoDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE OpcionRespuestaPlantillaDinamico ");
            sCmd.Append(" SET Activo = 0 ");
            if (opcionRespuestaPlantilla.OpcionRespuestaPlantillaID == null)
                sCmd.Append(" WHERE OpcionRespuestaPlantillaID IS NULL ");
            else
            {
                // opcionRespuestaPlantilla.OpcionRespuestaPlantillaID
                sCmd.Append(" WHERE OpcionRespuestaPlantillaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = opcionRespuestaPlantilla.OpcionRespuestaPlantillaID;
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
                throw new Exception("OpcionRespuestaPlantillaDinamicoDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("OpcionRespuestaPlantillaDinamicoDelHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
