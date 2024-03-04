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
    /// LicenciaUniversidadInsHlp
    /// </summary>
    public class LicenciaUniversidadInsHlp
    {
        /// <summary>
        /// Crea un registro de LicenciaUniversidad en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaTutor">LicenciaUniversidad que desea crear</param>
        public void Action(IDataContext dctx, LicenciaUniversidad licenciaUniversidad, LicenciaEscuela licenciaEscuela)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaUniversidad == null)
                sError += ", LicenciaUniversidad";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaUniversidadInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaUniversidad.LicenciaID == null)
                sError += ", LicenciaID";
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuelaID";
            if (licenciaUniversidad.Usuario == null)
                sError += ", Usuario";
            if (licenciaUniversidad.Universidad == null)
                sError += ", Universidad";
            if (licenciaUniversidad.Activo == null)
                sError += ", Activo";
            if (sError.Length > 0)
                throw new Exception("LicenciaUniversidadInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaUniversidad.Usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID";
            if (licenciaUniversidad.Universidad.UniversidadID == null)
                sError += ", Universidad.UniversidadID";
            if (sError.Length > 0)
                throw new Exception("LicenciaUniversidadInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", "LicenciaUniversidadInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaUniversidadInsHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", "LicenciaUniversidadInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Licencia ( LicenciaID, LicenciaEscuelaID, UsuarioID, Tipo, Activo) ");
            // licenciaUniversidad.LicenciaID
            sCmd.Append(" VALUES ( @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (licenciaUniversidad.LicenciaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaUniversidad.LicenciaID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaUniversidad.LicenciaEscuelaID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaEscuela.LicenciaEscuelaID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaUniversidad.Usuario.UsuarioID
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (licenciaUniversidad.Usuario.UsuarioID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaUniversidad.Usuario.UsuarioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaUniversidad.Tipo
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = licenciaUniversidad.Tipo;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaUniversidad.Activo
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (licenciaUniversidad.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaUniversidad.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ) ");

            sCmd.AppendLine();
            sCmd.Append(" INSERT INTO LicenciaReferencia ( LicenciaID, UniversidadID) ");
            // licenciaUniversidad.LicenciaID
            sCmd.Append(" VALUES ( @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (licenciaUniversidad.LicenciaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaUniversidad.LicenciaID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaUniversidad.Universidad.UniversidadID
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (licenciaUniversidad.Universidad.UniversidadID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaUniversidad.Universidad.UniversidadID;
            sqlParam.DbType = DbType.Int32;
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
                throw new Exception("LicenciaUniversidadInsHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("LicenciaUniversidadInsHlp: Hubo un error al consultar los registros.");
        }
    }
}
