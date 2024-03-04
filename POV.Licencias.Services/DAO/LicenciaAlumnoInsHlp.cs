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
    /// LicenciaAlumnoInsHlp
    /// </summary>
    public class LicenciaAlumnoInsHlp
    {
        /// <summary>
        /// Crea un registro de LicenciaDirector en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaAlumno">LicenciaDirector que desea crear</param>
        public void Action(IDataContext dctx, LicenciaAlumno licenciaAlumno, LicenciaEscuela licenciaEscuela)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaAlumno == null)
                sError += ", LicenciaAlumno";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaAlumnoInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaAlumno.LicenciaID == null)
                sError += ", LicenciaID";
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuelaID";
            if (licenciaAlumno.Usuario == null)
                sError += ", Usuario";
            if (licenciaAlumno.Alumno == null)
                sError += ", Alumno";
            if (licenciaAlumno.UsuarioSocial == null)
                sError += ", UsuarioSocial";
            if (licenciaAlumno.Activo == null)
                sError += ", Activo";
            if (sError.Length > 0)
                throw new Exception("LicenciaAlumnoInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaAlumno.Usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID";
            if (licenciaAlumno.Alumno.AlumnoID == null)
                sError += ", Alumno.AlumnoID";
            if (licenciaAlumno.UsuarioSocial.UsuarioSocialID == null)
                sError += ", UsuarioSocial.UsuarioSocialID";
            if (sError.Length > 0)
                throw new Exception("LicenciaAlumnoInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO","LicenciaAlumnoInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaAlumnoInsHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO","LicenciaAlumnoInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Licencia ( LicenciaID, LicenciaEscuelaID, UsuarioID, UsuarioSocialID, Tipo, Activo) ");
            // licenciaAlumno.LicenciaID
            sCmd.Append(" VALUES ( @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (licenciaAlumno.LicenciaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaAlumno.LicenciaID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaEscuela.LicenciaEscuelaID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaEscuela.LicenciaEscuelaID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaAlumno.Usuario.UsuarioID
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (licenciaAlumno.Usuario.UsuarioID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaAlumno.Usuario.UsuarioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaAlumno.UsuarioSocial.UsuarioSocialID
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (licenciaAlumno.UsuarioSocial.UsuarioSocialID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaAlumno.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaAlumno.Tipo
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = licenciaAlumno.Tipo;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaAlumno.Activo
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (licenciaAlumno.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaAlumno.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ) ");

            sCmd.AppendLine();
            sCmd.Append(" INSERT INTO LicenciaReferencia ( LicenciaID, AlumnoID) ");
            // licenciaAlumno.LicenciaID
            sCmd.Append(" VALUES ( @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (licenciaAlumno.LicenciaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaAlumno.LicenciaID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaAlumno.Alumno.AlumnoID
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (licenciaAlumno.Alumno.AlumnoID== null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaAlumno.Alumno.AlumnoID;
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
                throw new Exception("LicenciaAlumnoInsHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("LicenciaAlumnoInsHlp: Hubo un error al consultar los registros.");
        }
    }
}
