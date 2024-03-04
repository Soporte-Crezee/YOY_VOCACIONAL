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
    /// Consulta un registro de Pregunta en la BD
    /// </summary>
    internal class PreguntaDinamicoRetHlp
    {
        /// <summary>
        /// Consulta registros de Pregunta en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="pregunta">Pregunta que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Pregunta generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Reactivo reactivo, Pregunta pregunta)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (reactivo == null)
                sError += ", reactivo";
            if (sError.Length > 0)
                throw new Exception("PreguntaDinamicoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            
            if (pregunta == null)
                sError += ", Pregunta";
            if (sError.Length > 0)
                throw new Exception("PreguntaDinamicoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO",
                   "PreguntaDinamicoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PreguntaDinamicoRetHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO",
                   "PreguntaDinamicoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT PreguntaID, ReactivoID, Orden, TextoPregunta, PlantillaPregunta, FechaRegistro, Valor, PuedeOmitir, Activo, PresentacionPlantilla ");
            sCmd.Append(" FROM PreguntaDinamico ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (pregunta.PreguntaID != null)
            {
                s_VarWHERE.Append(" PreguntaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = pregunta.PreguntaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (reactivo.ReactivoID != null)
            {
                s_VarWHERE.Append(" AND ReactivoID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = reactivo.ReactivoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.Orden != null)
            {
                s_VarWHERE.Append(" AND Orden = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = pregunta.Orden;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.TextoPregunta != null)
            {
                s_VarWHERE.Append(" AND TextoPregunta = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = pregunta.TextoPregunta;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = pregunta.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.Valor != null)
            {
                s_VarWHERE.Append(" AND Valor = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = pregunta.Valor;
                sqlParam.DbType = DbType.Decimal;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.PuedeOmitir != null)
            {
                s_VarWHERE.Append(" AND PuedeOmitir = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = pregunta.PuedeOmitir;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.Activo != null)
            {
                s_VarWHERE.Append(" AND Activo = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = pregunta.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (pregunta.PresentacionPlantilla != null)
            {
                s_VarWHERE.Append(" AND PresentacionPlantilla = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = pregunta.PresentacionPlantilla;
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
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Pregunta");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("PreguntaDinamicoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
