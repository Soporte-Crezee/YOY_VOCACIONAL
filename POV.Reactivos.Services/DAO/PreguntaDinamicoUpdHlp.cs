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
    /// Actualiza un registro de Pregunta en la BD
    /// </summary>
    internal class PreguntaDinamicoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de PreguntaDinamicoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="preguntaDinamicoUpdHlp">PreguntaDinamicoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">PreguntaDinamicoUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, Pregunta pregunta, Pregunta anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (pregunta == null)
                sError += ", Pregunta";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("PreguntaDinamicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.PreguntaID == null)
                sError += ", Anterior PreguntaID";
            if (sError.Length > 0)
                throw new Exception("PreguntaDinamicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "PreguntaDinamicoUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PreguntaDinamicoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Reactivos.DAO",
                   "PreguntaDinamicoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE PreguntaDinamico ");
            sCmd.Append(" SET ");
            if (pregunta.TextoPregunta != null)
            {
                sCmd.Append(" TextoPregunta = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = pregunta.TextoPregunta;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.PlantillaPregunta != null)
            {
                sCmd.Append(" ,PlantillaPregunta = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = pregunta.PlantillaPregunta;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.Valor != null)
            {
                sCmd.Append(" ,Valor = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = pregunta.Valor;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.PuedeOmitir != null)
            {
                sCmd.Append(" ,PuedeOmitir = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = pregunta.PuedeOmitir;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.PresentacionPlantilla != null)
            {
                sCmd.Append(" ,PresentacionPlantilla = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = pregunta.PresentacionPlantilla;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            StringBuilder s_VarWHERE = new StringBuilder();
            if (anterior.PreguntaID == null)
                s_VarWHERE.Append(" PreguntaID IS NULL ");
            else
            {
                // anterior.PreguntaID
                s_VarWHERE.Append(" PreguntaID = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = anterior.PreguntaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.TextoPregunta != null)
            {
                s_VarWHERE.Append(" AND TextoPregunta = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = anterior.TextoPregunta;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Valor != null)
            {
                s_VarWHERE.Append(" AND Valor = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = anterior.Valor;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.PuedeOmitir != null)
            {
                s_VarWHERE.Append(" AND PuedeOmitir = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = anterior.PuedeOmitir;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.PresentacionPlantilla != null)
            {
                s_VarWHERE.Append(" AND PresentacionPlantilla = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = anterior.PresentacionPlantilla;
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
                throw new Exception("PreguntaDinamicoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("PreguntaDinamicoUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
