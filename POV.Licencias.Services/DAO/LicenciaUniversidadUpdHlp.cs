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
    /// LicenciaUniversidadUpdHlp
    /// </summary>
    public class LicenciaUniversidadUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de LicenciaUniversidad en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaTutor">LicenciaUniversidad que tiene los datos nuevos</param>
        /// <param name="anterior">LicenciaUniversidad que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, LicenciaUniversidad licenciaUniversidad, LicenciaUniversidad anterior, LicenciaEscuela licenciaEscuela)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaUniversidad == null)
                sError += ", LicenciaUniversidad";
            if (anterior == null)
                sError += ", LicenciaUniversidad anterior";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaUniversidadUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuelaID";
            if (sError.Length > 0)
                throw new Exception("LicenciaUniversidadUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaUniversidad.LicenciaID == null)
                sError += ", LicenciaID";
            if (licenciaUniversidad.Usuario == null)
                sError += ", Usuario";
            if (licenciaUniversidad.Universidad == null)
                sError += ", Universidad";
            if (licenciaUniversidad.Activo == null)
                sError += ", Activo";
            if (sError.Length > 0)
                throw new Exception("LicenciaUniversidadUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaUniversidad.Usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID";
            if (licenciaUniversidad.Universidad.UniversidadID == null)
                sError += ", Universidad.UniversidadID";
            if (sError.Length > 0)
                throw new Exception("LicenciaUniversidadUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (anterior.LicenciaID == null)
                sError += ", LicenciaID anterior";
            if (anterior.Usuario == null)
                sError += ", Usuario anterior";
            if (anterior.Universidad == null)
                sError += ", Universidad anterior";
            if (anterior.Activo == null)
                sError += ", Activo anterior";
            if (sError.Length > 0)
                throw new Exception("LicenciaUniversidadUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (anterior.Usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID anterior";
            if (anterior.Universidad.UniversidadID == null)
                sError += ", Universidad.UniversidadID anterior";
            if (sError.Length > 0)
                throw new Exception("LicenciaUniversidadUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", "LicenciaUniversidadUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaUniversidadUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", "LicenciaUniversidadUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Licencia ");
            if (licenciaUniversidad.Activo == null)
                sCmd.Append(" SET Activo = NULL ");
            else
            {
                // licenciaUniversidad.Activo
                sCmd.Append(" SET Activo =@dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = licenciaUniversidad.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaUniversidad.LicenciaID == null)
                sCmd.Append(" WHERE LicenciaID IS NULL ");
            else
            {
                // licenciaUniversidad.LicenciaID
                sCmd.Append(" WHERE LicenciaID =@dbp4ram2");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = anterior.LicenciaID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaUniversidad.LicenciaID == null)
                sCmd.Append(" AND LicenciaEscuelaID IS NULL ");
            else
            {
                // licenciaUniversidad.LicenciaEscuelaID
                sCmd.Append(" AND LicenciaEscuelaID =@dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = licenciaEscuela.LicenciaEscuelaID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaUniversidad.Usuario.UsuarioID == null)
                sCmd.Append(" AND UsuarioID IS NULL ");
            else
            {
                // licenciaUniversidad.Usuario.UsuarioID
                sCmd.Append(" AND UsuarioID =@dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = anterior.Usuario.UsuarioID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // licenciaUniversidad.Tipo
            sCmd.Append(" AND Tipo =@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.Tipo;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaUniversidad.Activo
            if (licenciaUniversidad.Activo == null)
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
                throw new Exception("LicenciaUniversidadUpdHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("LicenciaUniversidadUpdHlp: Hubo un error al consultar los registros.");
        }
    }
}
