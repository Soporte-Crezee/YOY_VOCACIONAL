using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Reactivos.DAO;

namespace POV.Reactivos.DAO
{
    /// <summary>
    /// Guarda un registro de RespuestaPlantillaOpcionMultiple en la BD
    /// </summary>
    internal class RespuestaPlantillaOpcionMultipleFinalInsHlp
    {
        /// <summary>
        /// Crea un registro de RespuestaPlantillaOpcionMultipeFinal en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPlantillaOpcionMultipe">RespuestaPlantillaOpcionMultipe que desea crear</param>
        public void Action(IDataContext dctx, RespuestaPlantillaOpcionMultiple respuestaPlantillaOpcionMultiple)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (respuestaPlantillaOpcionMultiple == null)
                sError += ", RespuestaPlantillaOpcionMultiple";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaOpcionMultipleFinalInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPlantillaOpcionMultiple.RespuestaPlantillaID == null)
                sError += ", RespuestaPlantillaOpcionMultiple";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaOpcionMultipleFinalInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMaximo == null)
                sError += ", NumeroSeleccionablesMaximo";
            if (respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMinimo == null)
                sError += ", NumeroSeleccionablesMinimo";
            if (respuestaPlantillaOpcionMultiple.ModoSeleccion == null)
                sError += ", ModoSeleccion";
            if (respuestaPlantillaOpcionMultiple.PresentacionOpcion == null)
                sError += ", PresentacionOpcion";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaOpcionMultipleFinalInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "RespuestaPlantillaOpcionMultipleFinalInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPlantillaOpcionMultipleFinalInsHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "RespuestaPlantillaOpcionMultipleFinalInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO RespuestaPlantillaOpcionMultipleFinal (RespuestaPlantillaID, NumeroSeleccionablesMaximo, NumeroSeleccionablesMinimo, ModoSeleccion, PresentacionOpcion) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (respuestaPlantillaOpcionMultiple.RespuestaPlantillaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPlantillaOpcionMultiple.RespuestaPlantillaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMaximo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMaximo;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMinimo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMinimo;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (respuestaPlantillaOpcionMultiple.ModoSeleccion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPlantillaOpcionMultiple.ModoSeleccion;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (respuestaPlantillaOpcionMultiple.PresentacionOpcion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPlantillaOpcionMultiple.PresentacionOpcion;
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
                throw new Exception("RespuestaPlantillaOpcionMultipleFinalInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RespuestaPlantillaOpcionMultipleFinalInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
