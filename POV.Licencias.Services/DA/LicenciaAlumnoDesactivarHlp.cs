// Licencias de escuela
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;

namespace POV.Licencias.DA
{
    /// <summary>
    /// LicenciaAlumno
    /// </summary>
    public class LicenciaAlumnoDesactivarHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de LicenciaAlumno en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaAlumno">LicenciaAlumno que tiene los datos nuevos</param>
        public void Action(IDataContext dctx, LicenciaAlumno licenciaAlumno)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaAlumno == null)
                sError += ", LicenciaAlumno";
            if (sError.Length > 0)
                throw new Exception("LicenciaAlumnoDesactivarHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaAlumno.LicenciaID == null)
                sError += ", LicenciaID";
            if (licenciaAlumno.Alumno == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("LicenciaAlumnoDesactivarHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaAlumno.Alumno.AlumnoID == null)
                sError += ", Alumno.AlumnoID";
            if (sError.Length > 0)
                throw new Exception("LicenciaAlumnoDesactivarHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "PISA1021.Licencias.DAO","LicenciaAlumnoDesactivarHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaAlumnoDesactivarHlp: Hubo un error al conectarse a la base de datos", "PISA1021.Licencias.DAO","LicenciaAlumnoDesactivarHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Licencia ");
           // licenciaAlumno.Activo
            sCmd.Append(" SET Activo = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = false;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            //licenciaAlumno.Alumno.AlumnoID
            sCmd.Append(" WHERE LicenciaID IN ( ");
            sCmd.Append("    SELECT lic.LicenciaID ");
            sCmd.Append("    FROM Licencia lic ");
            sCmd.Append("    INNER JOIN LicenciaReferencia licRef ON (licRef.LicenciaID = lic.LicenciaID) ");
            sCmd.Append("    WHERE licRef.AlumnoID =@dbp4ram2 ) ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = licenciaAlumno.Alumno.AlumnoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaAlumno.LicenciaID
            sCmd.Append(" AND LicenciaID <> @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = licenciaAlumno.LicenciaID;
            sqlParam.DbType = DbType.Guid;
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
                throw new Exception("LicenciaAlumnoDesactivarHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
        }
    }
}
