using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Modelo.BO;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    /// <summary>
    /// Elimina un registro de AEscalaDinamica en la BD
    /// </summary>
    internal class EscalaDinamicaDelHlp
    {
        /// <summary>
        /// Elimina un registro de EscalaDinamicaDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escalaDinamicaDelHlp">EscalaDinamicaDelHlp que desea eliminar</param>
        public void Action(IDataContext dctx, AEscalaDinamica escalaDinamica)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (escalaDinamica == null)
                sError += ", AEscalaDinamica";
            if (sError.Length > 0)
                throw new Exception("EscalaDinamicaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escalaDinamica.PuntajeID == null)
                sError += ", AEscalaDinamicaID";
            if (sError.Length > 0)
                throw new Exception("EscalaDinamicaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "EscalaDinamicaDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EscalaDinamicaDelHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "EscalaDinamicaDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE EscalaDinamica ");
            sCmd.Append(" SET Activo = 0 ");
            // escalaDinamica.PuntajeID
            sCmd.Append(" WHERE PuntajeID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (escalaDinamica.PuntajeID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escalaDinamica.PuntajeID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
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
                throw new Exception("EscalaDinamicaDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("EscalaDinamicaDelHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
