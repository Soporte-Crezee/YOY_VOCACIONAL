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
    /// Actualiza un registro de OpcionRespuestaPlantillaDinamico en la BD
    /// </summary>
    internal class OpcionRespuestaPlantillaDinamicoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de OpcionRespuestaPlantillaDinamicoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="opcionRespuestaPlantillaDinamicoUpdHlp">OpcionRespuestaPlantillaDinamicoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">OpcionRespuestaPlantillaDinamicoUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, OpcionRespuestaModeloGenerico opcionRespuestaPlantilla, OpcionRespuestaModeloGenerico anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (opcionRespuestaPlantilla == null)
                sError += ", OpcionRespuestaPlantillaDinamico";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("OpcionRespuestaPlantillaDinamicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.OpcionRespuestaPlantillaID == null)
                sError += ", Anterior OpcionRespuestaPlantillaDinamicoID";
            if (sError.Length > 0)
                throw new Exception("OpcionRespuestaPlantillaDinamicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "OpcionRespuestaPlantillaDinamicoUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "OpcionRespuestaPlantillaDinamicoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Reactivos.DAO",
                   "OpcionRespuestaPlantillaDinamicoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE OpcionRespuestaPlantillaDinamico ");
            if (opcionRespuestaPlantilla.Texto != null)
            {
                sCmd.Append(" SET Texto = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = opcionRespuestaPlantilla.Texto;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (opcionRespuestaPlantilla.ImagenUrl != null)
            {
                sCmd.Append(" ,ImagenUrl = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = opcionRespuestaPlantilla.ImagenUrl;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (opcionRespuestaPlantilla.EsOpcionCorrecta != null)
            {
                sCmd.Append(" ,EsOpcionCorrecta = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = opcionRespuestaPlantilla.EsOpcionCorrecta;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (opcionRespuestaPlantilla.Activo != null)
            {
                sCmd.Append(" ,Activo = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = opcionRespuestaPlantilla.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (opcionRespuestaPlantilla.PorcentajeCalificacion != null)
            {
                sCmd.Append(" ,PorcentajeCalificacion = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = opcionRespuestaPlantilla.PorcentajeCalificacion;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (opcionRespuestaPlantilla.Clasificador != null && opcionRespuestaPlantilla.Clasificador.ClasificadorID != null)
            {
                sCmd.Append(" ,ClasificadorID = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = opcionRespuestaPlantilla.Clasificador.ClasificadorID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (opcionRespuestaPlantilla.EsInteres != null)
            {
                sCmd.Append(" ,EsInteres = @dbp4ram14 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram14";
                sqlParam.Value = opcionRespuestaPlantilla.EsInteres;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            StringBuilder s_VarWHERE = new StringBuilder();
            if (anterior.OpcionRespuestaPlantillaID == null)
                s_VarWHERE.Append(" OpcionRespuestaPlantillaID IS NULL ");
            else
            {
                // anterior.OpcionRespuestaPlantillaDinamicoID
                s_VarWHERE.Append(" OpcionRespuestaPlantillaID = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = anterior.OpcionRespuestaPlantillaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Texto != null)
            {
                s_VarWHERE.Append(" AND Texto = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = anterior.Texto;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.ImagenUrl != null)
            {
                s_VarWHERE.Append(" AND ImagenUrl = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = anterior.ImagenUrl;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.EsOpcionCorrecta != null)
            {
                s_VarWHERE.Append(" AND EsOpcionCorrecta = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = anterior.EsOpcionCorrecta;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Activo != null)
            {
                s_VarWHERE.Append(" AND Activo = @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = anterior.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.PorcentajeCalificacion != null)
            {
                s_VarWHERE.Append(" AND PorcentajeCalificacion = @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = anterior.PorcentajeCalificacion;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Clasificador != null && anterior.Clasificador.ClasificadorID != null)
            {
                s_VarWHERE.Append(" AND ClasificadorID = @dbp4ram13 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram13";
                sqlParam.Value = anterior.Clasificador.ClasificadorID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.EsInteres != null)
            {
                s_VarWHERE.Append(" AND EsInteres = @dbp4ram15 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram15";
                sqlParam.Value = anterior.EsInteres;
                sqlParam.DbType = DbType.Boolean;
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
                throw new Exception("OpcionRespuestaPlantillaDinamicoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("OpcionRespuestaPlantillaDinamicoUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
