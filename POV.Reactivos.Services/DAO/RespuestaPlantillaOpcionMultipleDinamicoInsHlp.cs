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
    /// Guarda un registro de RespuestaPlantillaOpcionMultiple en la BD
    /// </summary>
    internal class RespuestaPlantillaOpcionMultipleDinamicoInsHlp
    {
        /// <summary>
        /// Crea un registro de RespuestaPlantillaOpcionMultiple en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPlantillaOpcionMultiple">RespuestaPlantillaOpcionMultiple que desea crear</param>
        public void Action(IDataContext dctx, RespuestaPlantillaOpcionMultiple respuestaPlantilla)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (respuestaPlantilla == null)
                sError += ", RespuestaPlantillaOpcionMultiple";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaOpcionMultipleDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPlantilla.RespuestaPlantillaID == null)
                sError += ", RespuestaPlantillaID";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaOpcionMultipleDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPlantilla.NumeroSeleccionablesMaximo == null)
                sError += ", NumeroSeleccionablesMaximo";
            if (respuestaPlantilla.NumeroSeleccionablesMinimo == null)
                sError += ", NumeroSeleccionablesMinimo";
            if (respuestaPlantilla.ModoSeleccion == null)
                sError += ", ModoSeleccion";
            if (respuestaPlantilla.PresentacionOpcion == null)
                sError += ", PresentacionOpcion";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaOpcionMultipleDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "RespuestaPlantillaOpcionMultipleDinamicoInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPlantillaOpcionMultipleDinamicoInsHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "RespuestaPlantillaOpcionMultipleDinamicoInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO RespuestaPlantillaOpcionMultipleDinamico (RespuestaPlantillaID, NumeroSeleccionablesMaximo, NumeroSeleccionablesMinimo, ModoSeleccion, PresentacionOpcion) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // respuestaPlantilla.RespuestaPlantillaID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (respuestaPlantilla.RespuestaPlantillaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPlantilla.RespuestaPlantillaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaPlantilla.NumeroSeleccionablesMaximo
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (respuestaPlantilla.NumeroSeleccionablesMaximo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPlantilla.NumeroSeleccionablesMaximo;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaPlantilla.NumeroSeleccionablesMinimo
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (respuestaPlantilla.NumeroSeleccionablesMinimo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPlantilla.NumeroSeleccionablesMinimo;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaPlantilla.ModoSeleccion
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (respuestaPlantilla.ModoSeleccion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPlantilla.ModoSeleccion;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaPlantilla.PresentacionOpcion
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (respuestaPlantilla.PresentacionOpcion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPlantilla.PresentacionOpcion;
            sqlParam.DbType = DbType.Byte;
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
                throw new Exception("RespuestaPlantillaOpcionMultipleDinamicoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RespuestaPlantillaOpcionMultipleDinamicoInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
