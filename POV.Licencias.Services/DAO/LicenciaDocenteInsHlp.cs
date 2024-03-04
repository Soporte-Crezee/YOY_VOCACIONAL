// Licencias de Docente
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
    /// LicenciaDocenteInsHlp
    /// </summary>
    public class LicenciaDocenteInsHlp
    {
        /// <summary>
        /// Crea un registro de LicenciaDocente en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaDocente">LicenciaDocente que desea crear</param>
        public void Action(IDataContext dctx, LicenciaDocente licenciaDocente, LicenciaEscuela licenciaEscuela)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaDocente == null)
                sError += ", LicenciaDocente";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaDocenteInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaDocente.LicenciaID == null)
                sError += ", LicenciaID";
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuelaID";
            if (licenciaDocente.Usuario == null)
                sError += ", Usuario";
            if (licenciaDocente.Docente == null)
                sError += ", Docente";
            if (licenciaDocente.UsuarioSocial == null)
                sError += ", UsuarioSocial";
            if (licenciaDocente.Activo == null)
                sError += ", Activo";
            if (sError.Length > 0)
                throw new Exception("LicenciaDocenteInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaDocente.Usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID";
            if (licenciaDocente.Docente.DocenteID == null)
                sError += ", Docente.DocenteID";
            if (licenciaDocente.UsuarioSocial.UsuarioSocialID == null)
                sError += ", UsuarioSocial.UsuarioSocialID";
            if (sError.Length > 0)
                throw new Exception("LicenciaDocenteInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO","LicenciaDocenteInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaDocenteInsHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO","LicenciaDocenteInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Licencia ( LicenciaID, LicenciaEscuelaID, UsuarioID, UsuarioSocialID, Tipo, Activo) ");
            // licenciaDocente.LicenciaID
            sCmd.Append(" VALUES ( @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (licenciaDocente.LicenciaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaDocente.LicenciaID;
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
            // licenciaDocente.Usuario.UsuarioID
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (licenciaDocente.Usuario.UsuarioID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaDocente.Usuario.UsuarioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaDocente.UsuarioSocial.UsuarioSocialID
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (licenciaDocente.UsuarioSocial.UsuarioSocialID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaDocente.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaDocente.Tipo
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = licenciaDocente.Tipo;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaDocente.Activo
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (licenciaDocente.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaDocente.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ) ");

            sCmd.AppendLine();
            sCmd.Append(" INSERT INTO LicenciaReferencia ( LicenciaID, DocenteID) ");
            // licenciaDocente.LicenciaID
            sCmd.Append(" VALUES ( @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (licenciaDocente.LicenciaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaDocente.LicenciaID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaDocente.Docente.DocenteID
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (licenciaDocente.Docente.DocenteID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaDocente.Docente.DocenteID;
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
                throw new Exception("LicenciaDocenteInsHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("LicenciaDocenteInsHlp: Hubo un error al consultar los registros.");
        }
    }
}
