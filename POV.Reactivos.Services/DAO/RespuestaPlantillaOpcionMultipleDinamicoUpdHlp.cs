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
    /// Actualiza un registro de RespuestaPlantillaOpcionMultiple en la BD
    /// </summary>
    internal class RespuestaPlantillaOpcionMultipleDinamicoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de RespuestaPlantillaOpcionMultipleDinamicoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="respuestaPlantillaOpcionMultipleDinamicoUpdHlp">RespuestaPlantillaOpcionMultipleDinamicoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RespuestaPlantillaOpcionMultipleDinamicoUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, RespuestaPlantillaOpcionMultiple respuestaPlantilla, RespuestaPlantillaOpcionMultiple anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (respuestaPlantilla == null)
                sError += ", RespuestaPlantillaOpcionMultiple";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaOpcionMultipleDinamicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.RespuestaPlantillaID == null)
                sError += ", Anterior RespuestaPlantillaOpcionMultipleID";
            if (sError.Length > 0)
                throw new Exception("RespuestaPlantillaOpcionMultipleDinamicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "RespuestaPlantillaOpcionMultipleDinamicoUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RespuestaPlantillaOpcionMultipleDinamicoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Reactivos.DAO",
                   "RespuestaPlantillaOpcionMultipleDinamicoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE RespuestaPlantillaOpcionMultipleDinamico ");
            if (respuestaPlantilla.NumeroSeleccionablesMaximo != null)
            {
                sCmd.Append(" SET NumeroSeleccionablesMaximo = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = respuestaPlantilla.NumeroSeleccionablesMaximo;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.NumeroSeleccionablesMinimo != null)
            {
                sCmd.Append(" ,NumeroSeleccionablesMinimo = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = respuestaPlantilla.NumeroSeleccionablesMinimo;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.ModoSeleccion != null)
            {
                sCmd.Append(" ,ModoSeleccion = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = respuestaPlantilla.ModoSeleccion;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.PresentacionOpcion != null)
            {
                sCmd.Append(" ,PresentacionOpcion = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = respuestaPlantilla.PresentacionOpcion;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            StringBuilder s_VarWHERE = new StringBuilder();
            if (anterior.RespuestaPlantillaID == null)
                s_VarWHERE.Append(" RespuestaPlantillaID IS NULL ");
            else
            {
                // anterior.RespuestaPlantillaOpcionMultipleID
                s_VarWHERE.Append(" RespuestaPlantillaID = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = anterior.RespuestaPlantillaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.NumeroSeleccionablesMaximo != null)
            {
                s_VarWHERE.Append(" AND NumeroSeleccionablesMaximo = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = anterior.NumeroSeleccionablesMaximo;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.NumeroSeleccionablesMinimo != null)
            {
                s_VarWHERE.Append(" AND NumeroSeleccionablesMinimo = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = anterior.NumeroSeleccionablesMinimo;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.ModoSeleccion != null)
            {
                s_VarWHERE.Append(" AND ModoSeleccion = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = anterior.ModoSeleccion;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.PresentacionOpcion != null)
            {
                s_VarWHERE.Append(" AND PresentacionOpcion = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = anterior.PresentacionOpcion;
                sqlParam.DbType = DbType.Byte;
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
                throw new Exception("RespuestaPlantillaOpcionMultipleDinamicoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RespuestaPlantillaOpcionMultipleDinamicoUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
