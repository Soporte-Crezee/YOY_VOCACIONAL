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
    /// Consulta un registro de OpcionRespuestaPlantilla en la BD
    /// </summary>
    internal class OpcionRespuestaPlantillaDinamicoRetHlp
    {
        /// <summary>
        /// Consulta registros de OpcionRespuestaPlantilla en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="opcionRespuestaPlantilla">OpcionRespuestaPlantilla que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de OpcionRespuestaPlantilla generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, RespuestaPlantillaOpcionMultiple respuestaPlantilla, OpcionRespuestaModeloGenerico opcionRespuestaPlantilla)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (opcionRespuestaPlantilla == null)
                sError += ", OpcionRespuestaPlantilla";
            if (sError.Length > 0)
                throw new Exception("OpcionRespuestaPlantillaDinamicoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPlantilla == null)
                sError += ", RespuestaPlantillaOpcionMultiple";
            if (sError.Length > 0)
                throw new Exception("OpcionRespuestaPlantillaDinamicoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "OpcionRespuestaPlantillaDinamicoRetHlp", "Action", null, null);
            if (opcionRespuestaPlantilla.Modelo == null)
                opcionRespuestaPlantilla.Modelo = new ModeloDinamico();
            if (opcionRespuestaPlantilla.Clasificador == null)
                opcionRespuestaPlantilla.Clasificador = new Clasificador();
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "OpcionRespuestaPlantillaDinamicoRetHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "OpcionRespuestaPlantillaDinamicoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT OpcionRespuestaPlantillaID, Texto, ImagenUrl, EsPredeterminado, EsOpcionCorrecta, RespuestaPlantillaID, Activo, PorcentajeCalificacion, ModeloID, ClasificadorID, EsInteres");
            sCmd.Append(" FROM OpcionRespuestaPlantillaDinamico ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (opcionRespuestaPlantilla.OpcionRespuestaPlantillaID != null)
            {
                s_VarWHERE.Append(" OpcionRespuestaPlantillaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = opcionRespuestaPlantilla.OpcionRespuestaPlantillaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (opcionRespuestaPlantilla.Texto != null)
            {
                s_VarWHERE.Append(" AND Texto = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = opcionRespuestaPlantilla.Texto;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (opcionRespuestaPlantilla.EsPredeterminado != null)
            {
                s_VarWHERE.Append(" AND EsPredeterminado = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = opcionRespuestaPlantilla.EsPredeterminado;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (opcionRespuestaPlantilla.EsOpcionCorrecta != null)
            {
                s_VarWHERE.Append(" AND EsOpcionCorrecta = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = opcionRespuestaPlantilla.EsOpcionCorrecta;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (respuestaPlantilla.RespuestaPlantillaID != null)
            {
                s_VarWHERE.Append(" AND RespuestaPlantillaID = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = respuestaPlantilla.RespuestaPlantillaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (opcionRespuestaPlantilla.Activo != null)
            {
                s_VarWHERE.Append(" AND Activo = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = opcionRespuestaPlantilla.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (opcionRespuestaPlantilla.PorcentajeCalificacion != null)
            {
                s_VarWHERE.Append(" AND PorcentajeCalificacion = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = opcionRespuestaPlantilla.PorcentajeCalificacion;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if ((opcionRespuestaPlantilla as OpcionRespuestaModeloGenerico).Modelo.ModeloID != null)
            {
                s_VarWHERE.Append(" AND ModeloID = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = (opcionRespuestaPlantilla as OpcionRespuestaModeloGenerico).Modelo.ModeloID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if ((opcionRespuestaPlantilla as OpcionRespuestaModeloGenerico).Clasificador.ClasificadorID != null)
            {
                s_VarWHERE.Append(" AND ClasificadorID = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = (opcionRespuestaPlantilla as OpcionRespuestaModeloGenerico).Clasificador.ClasificadorID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (opcionRespuestaPlantilla.EsInteres != null)
            {
                s_VarWHERE.Append(" AND EsInteres = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = opcionRespuestaPlantilla.EsInteres;
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
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "OpcionRespuestaPlantilla");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("OpcionRespuestaPlantillaDinamicoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
