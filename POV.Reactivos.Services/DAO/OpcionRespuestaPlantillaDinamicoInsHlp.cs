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
    /// Guarda un registro de OpcionRespuestaPlantilla en la BD
    /// </summary>
    internal class OpcionRespuestaPlantillaDinamicoInsHlp
    {
        /// <summary>
        /// Crea un registro de OpcionRespuestaPlantilla en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="opcionRespuestaPlantilla">OpcionRespuestaPlantilla que desea crear</param>
        public void Action(IDataContext dctx, RespuestaPlantillaOpcionMultiple respuestaPlantilla, OpcionRespuestaModeloGenerico opcionRespuestaPlantilla)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (opcionRespuestaPlantilla == null)
                sError += ", OpcionRespuestaPlantilla";
            if (sError.Length > 0)
                throw new Exception("OpcionRespuestaPlantillaDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPlantilla == null)
                sError += ", RespuestaPlantillaOpcionMultiple";
            if (sError.Length > 0)
                throw new Exception("OpcionRespuestaPlantillaDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (respuestaPlantilla.RespuestaPlantillaID == null)
                sError += ", RespuestaPlantillaID";
            if (sError.Length > 0)
                throw new Exception("OpcionRespuestaPlantillaDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (opcionRespuestaPlantilla.Texto == null)
                sError += ", Texto";
            if (opcionRespuestaPlantilla.EsPredeterminado == null)
                sError += ", EsPredeterminado";
            if (opcionRespuestaPlantilla.EsOpcionCorrecta == null)
                sError += ", EsOpcionCorrecta";
            if (opcionRespuestaPlantilla.PorcentajeCalificacion == null)
                sError += ", PorcentajeCalificacion";
            if (sError.Length > 0)
                throw new Exception("OpcionRespuestaPlantillaDinamicoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "OpcionRespuestaPlantillaDinamicoInsHlp", "Action", null, null);

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
                throw new StandardException(MessageType.Error, "", "OpcionRespuestaPlantillaDinamicoInsHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "OpcionRespuestaPlantillaDinamicoInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO OpcionRespuestaPlantillaDinamico (Texto, ImagenUrl, EsPredeterminado, EsOpcionCorrecta, RespuestaPlantillaID, Activo, PorcentajeCalificacion, ModeloID, ClasificadorID, EsInteres) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (opcionRespuestaPlantilla.Texto == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = opcionRespuestaPlantilla.Texto;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // opcionRespuestaPlantilla.ImagenUrl
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (opcionRespuestaPlantilla.ImagenUrl == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = opcionRespuestaPlantilla.ImagenUrl;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // opcionRespuestaPlantilla.EsPredeterminado
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (opcionRespuestaPlantilla.EsPredeterminado == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = opcionRespuestaPlantilla.EsPredeterminado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // opcionRespuestaPlantilla.EsOpcionCorrecta
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (opcionRespuestaPlantilla.EsOpcionCorrecta == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = opcionRespuestaPlantilla.EsOpcionCorrecta;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // respuestaPlantilla.RespuestaPlantillaID
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (respuestaPlantilla.RespuestaPlantillaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = respuestaPlantilla.RespuestaPlantillaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // opcionRespuestaPlantilla.Activo
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (opcionRespuestaPlantilla.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = opcionRespuestaPlantilla.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // opcionRespuestaPlantilla.PorcentajeCalificacion
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (opcionRespuestaPlantilla.PorcentajeCalificacion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = opcionRespuestaPlantilla.PorcentajeCalificacion;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
            // opcionRespuestaPlantilla.Modelo.ModeloID
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (opcionRespuestaPlantilla.Modelo.ModeloID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = opcionRespuestaPlantilla.Modelo.ModeloID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // opcionRespuestaPlantilla.Clasificador.ClasificadorID
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if (opcionRespuestaPlantilla.Clasificador.ClasificadorID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = opcionRespuestaPlantilla.Clasificador.ClasificadorID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // opcionRespuestaPlantilla.EsInteres
            sCmd.Append(" ,@dbp4ram10 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram10";
            if (opcionRespuestaPlantilla.EsInteres == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = opcionRespuestaPlantilla.EsInteres;
            sqlParam.DbType = DbType.Boolean;
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
                throw new Exception("OpcionRespuestaPlantillaDinamicoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("OpcionRespuestaPlantillaDinamicoInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
