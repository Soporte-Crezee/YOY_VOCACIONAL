// Licencias de escuela
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;

namespace POV.Licencias.DAO
{
    /// <summary>
    /// LicenciaAlumnoUpdHlp
    /// </summary>
    public class LicenciaAlumnoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de LicenciaAlumno en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaAlumno">LicenciaDocente que tiene los datos nuevos</param>
        /// <param name="anterior">LicenciaDocente que tiene los datos nuevos</param>
        /// <param name="licenciaEscuela">LicenciaEscuela</param>
        public void Action(IDataContext dctx, LicenciaAlumno licenciaAlumno, LicenciaAlumno anterior, LicenciaEscuela licenciaEscuela)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaAlumno == null)
                sError += ", LicenciaAlumno";
            if (anterior == null)
                sError += ", LicenciaAlumno anterior";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaAlumnoUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuelaID";
            if (sError.Length > 0)
                throw new Exception("LicenciaAlumnoUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaAlumno.LicenciaID == null)
                sError += ", LicenciaID";
            if (licenciaAlumno.Usuario == null)
                sError += ", Usuario";
            if (licenciaAlumno.Alumno == null)
                sError += ", Alumno";
            if (licenciaAlumno.UsuarioSocial == null)
                sError += ", UsuarioSocial";
            if (licenciaAlumno.Activo == null)
                sError += ", Activo";
            if (sError.Length > 0)
                throw new Exception("LicenciaAlumnoUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaAlumno.Usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID";
            if (licenciaAlumno.Alumno.AlumnoID == null)
                sError += ", Alumno.AlumnoID";
            if (licenciaAlumno.UsuarioSocial.UsuarioSocialID == null)
                sError += ", UsuarioSocial.UsuarioSocialID";
            if (sError.Length > 0)
                throw new Exception("LicenciaAlumnoUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (anterior.LicenciaID == null)
                sError += ", LicenciaID anterior";
            if (anterior.Usuario == null)
                sError += ", Usuario anterior";
            if (anterior.Alumno == null)
                sError += ", Alumno anterior";
            if (anterior.UsuarioSocial == null)
                sError += ", UsuarioSocial anterior";
            if (anterior.Activo == null)
                sError += ", Activo anterior";
            if (sError.Length > 0)
                throw new Exception("LicenciaAlumnoUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (anterior.Usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID anterior";
            if (anterior.Alumno.AlumnoID == null)
                sError += ", Alumno.AlumnoID anterior";
            if (anterior.UsuarioSocial.UsuarioSocialID== null)
                sError += ", UsuarioSocial.UsuarioSocialID anterior";
            if (sError.Length > 0)
                throw new Exception("LicenciaAlumnoUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", "LicenciaAlumnoUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaAlumnoUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", "LicenciaAlumnoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Licencia ");
            if (licenciaAlumno.Activo == null)
                sCmd.Append(" SET Activo = NULL ");
            else
            {
                // licenciaDocente.Activo
                sCmd.Append(" SET Activo =@dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = licenciaAlumno.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaAlumno.LicenciaID == null)
                sCmd.Append(" WHERE LicenciaID IS NULL ");
            else
            {
                sCmd.Append(" WHERE LicenciaID =@dbp4ram2");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = anterior.LicenciaID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela == null)
                sCmd.Append(" AND LicenciaEscuelaID IS NULL ");
            else
            {
                // licenciaEscuela.LicenciaEscuelaID
                sCmd.Append(" AND LicenciaEscuelaID =@dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = licenciaEscuela.LicenciaEscuelaID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Usuario.UsuarioID == null)
                sCmd.Append(" AND UsuarioID IS NULL ");
            else
            {
                // licenciaDocente.Usuario.UsuarioID
                sCmd.Append(" AND UsuarioID =@dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = anterior.Usuario.UsuarioID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.UsuarioSocial.UsuarioSocialID == null)
                sCmd.Append(" AND UsuarioSocialID IS NULL ");
            else
            {
                // licenciaDocente.UsuarioSocial.UsuarioSocialID
                sCmd.Append(" AND UsuarioSocialID =@dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = anterior.UsuarioSocial.UsuarioSocialID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // licenciaDocente.Tipo
            sCmd.Append(" AND Tipo =@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = (byte)anterior.Tipo;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaDocente.Activo
            if (anterior.Activo == null)
                sCmd.Append(" AND Activo IS NULL ");
            else
            {
                sCmd.Append(" AND Activo =@dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = anterior.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
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
                throw new Exception("LicenciaAlumnoUpdHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("LicenciaAlumnoUpdHlp: Hubo un error al consultar los registros.");
        }
    }
}
