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
    /// Guarda un registro de RegistroPruebaDinamica en la BD
    /// </summary>
    internal class RegistroPruebaDinamicaInsHlp
    {
        /// <summary>
        /// Crea un registro de RegistroPruebaDinamica en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="registroPruebaDinamica">RegistroPruebaDinamica que desea crear</param>
        public void Action(IDataContext dctx, ResultadoPruebaDinamica resultadoPrueba, RegistroPruebaDinamica registroPrueba)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (registroPrueba == null)
                sError += ", RegistroPruebaDinamica";
            if (sError.Length > 0)
                throw new Exception("RegistroPruebaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (resultadoPrueba == null)
                sError += ", ResultadoPruebaDinamica";
            if (sError.Length > 0)
                throw new Exception("RegistroPruebaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (resultadoPrueba.ResultadoPruebaID == null)
                sError += ", ResultadoPruebaID";
            if (sError.Length > 0)
                throw new Exception("RegistroPruebaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (registroPrueba.Alumno == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("RegistroPruebaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (registroPrueba.Alumno.AlumnoID == null)
                sError += ", AlumnoID";
            if (sError.Length > 0)
                throw new Exception("RegistroPruebaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (registroPrueba.EstadoPrueba == null)
                sError += ", EstadoPrueba";
            if (registroPrueba.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (sError.Length > 0)
                throw new Exception("RegistroPruebaDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RegistroPruebaDinamicaInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RegistroPruebaDinamicaInsHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "RegistroPruebaDinamicaInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO RegistroPruebaDinamica (ResultadoPruebaID, AlumnoID, EstadoPrueba, FechaInicio, FechaFin, FechaRegistro) ");
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
            // registroPrueba.Alumno.AlumnoID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (registroPrueba.Alumno.AlumnoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = registroPrueba.Alumno.AlumnoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // registroPrueba.EstadoPrueba
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (registroPrueba.EstadoPrueba == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = registroPrueba.EstadoPrueba;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // registroPrueba.FechaInicio
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (registroPrueba.FechaInicio == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = registroPrueba.FechaInicio;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // registroPrueba.FechaFin
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (registroPrueba.FechaFin == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = registroPrueba.FechaFin;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // registroPrueba.FechaRegistro
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (registroPrueba.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = registroPrueba.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
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
                throw new Exception("RegistroPruebaDinamicaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RegistroPruebaDinamicaInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
