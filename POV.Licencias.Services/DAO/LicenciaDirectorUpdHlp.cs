// Licencias de Director
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
    /// LicenciaDirectorUpdHlp
    /// </summary>
    public class LicenciaDirectorUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de LicenciaDirector en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaDirector">LicenciaDirector que tiene los datos nuevos</param>
        /// <param name="anterior">LicenciaDirector que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, LicenciaDirector licenciaDirector, LicenciaDirector anterior, LicenciaEscuela licenciaEscuela)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaDirector == null)
                sError += ", LicenciaDirector";
            if (anterior == null)
                sError += ", LicenciaDirector anterior";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaDirectorUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuelaID";
            if (sError.Length > 0)
                throw new Exception("LicenciaDirectorUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaDirector.LicenciaID == null)
                sError += ", LicenciaID";
            if (licenciaDirector.Usuario == null)
                sError += ", Usuario";
            if (licenciaDirector.Director == null)
                sError += ", Director";
            if (licenciaDirector.Activo == null)
                sError += ", Activo";
            if (sError.Length > 0)
                throw new Exception("LicenciaDirectorUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaDirector.Usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID";
            if (licenciaDirector.Director.DirectorID == null)
                sError += ", Director.DirectorID";
            if (sError.Length > 0)
                throw new Exception("LicenciaDirectorUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (anterior.LicenciaID == null)
                sError += ", LicenciaID anterior";
            if (anterior.Usuario == null)
                sError += ", Usuario anterior";
            if (anterior.Director == null)
                sError += ", Director anterior";
            if (anterior.Activo == null)
                sError += ", Activo anterior";
            if (sError.Length > 0)
                throw new Exception("LicenciaDirectorUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (anterior.Usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID anterior";
            if (anterior.Director.DirectorID == null)
                sError += ", Director.DirectorID anterior";
            if (sError.Length > 0)
                throw new Exception("LicenciaDirectorUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", "LicenciaDirectorUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaDirectorUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", "LicenciaDirectorUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Licencia ");
            if (licenciaDirector.Activo == null)
                sCmd.Append(" SET Activo = NULL ");
            else
            {
                // licenciaDirector.Activo
                sCmd.Append(" SET Activo =@dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = licenciaDirector.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaDirector.LicenciaID == null)
                sCmd.Append(" WHERE LicenciaID IS NULL ");
            else
            {
                // licenciaDirector.LicenciaID
                sCmd.Append(" WHERE LicenciaID =@dbp4ram2");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = anterior.LicenciaID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaDirector.LicenciaID == null)
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
            if (licenciaDirector.Usuario.UsuarioID == null)
                sCmd.Append(" AND UsuarioID IS NULL ");
            else
            {
                // licenciaDirector.Usuario.UsuarioID
                sCmd.Append(" AND UsuarioID =@dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = anterior.Usuario.UsuarioID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // licenciaDirector.Tipo
            sCmd.Append(" AND Tipo =@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.Tipo;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaDirector.Activo
            if (licenciaDirector.Activo == null)
                sCmd.Append(" AND Activo IS NULL ");
            else
            {
                sCmd.Append(" AND Activo =@dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
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
                throw new Exception("LicenciaDirectorUpdHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("LicenciaDirectorUpdHlp: Hubo un error al consultar los registros.");
        }
    }
}
