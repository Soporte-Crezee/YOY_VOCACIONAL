using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.CentroEducativo.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    /// <summary>
    /// Guarda un registro de ResultadoPruebaDinamica en la BD
    /// </summary>
    internal class ResultadoPruebaDinamicaInsHlp
    {
        /// <summary>
        /// Crea un registro de ResultadoPruebaDinamica en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoPruebaDinamica">ResultadoPruebaDinamica que desea crear</param>
        public void Action(IDataContext dctx, ResultadoPruebaDinamica resultadoPrueba)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (resultadoPrueba == null)
                sError += ", ResultadoPruebaDinamica";
            if (sError.Length > 0)
                throw new Exception("ResultadoPruebaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (resultadoPrueba.ResultadoPruebaID == null)
                sError += ", ResultadoPruebaID";
            if (sError.Length > 0)
                throw new Exception("ResultadoPruebaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (resultadoPrueba.Alumno == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("ResultadoPruebaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (resultadoPrueba.Alumno.AlumnoID == null)
                sError += ", AlumnoID";
            if (sError.Length > 0)
                throw new Exception("ResultadoPruebaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "ResultadoPruebaDinamicaInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ResultadoPruebaDinamicaInsHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "ResultadoPruebaDinamicaInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO ResultadoPruebaDinamica (ResultadoPruebaID, AlumnoID) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // resultadoPrueba.ResultadoPruebaID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (resultadoPrueba.ResultadoPruebaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = resultadoPrueba.ResultadoPruebaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // resultadoPrueba.Alumno.AlumnoID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (resultadoPrueba.Alumno.AlumnoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = resultadoPrueba.Alumno.AlumnoID;
            sqlParam.DbType = DbType.Int64;
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
                throw new Exception("ResultadoPruebaDinamicaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ResultadoPruebaDinamicaInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
