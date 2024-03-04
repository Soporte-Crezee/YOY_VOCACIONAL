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
    /// LicenciaTutorInsHlp
    /// </summary>
    public class LicenciaTutorInsHlp
    {
        /// <summary>
        /// Crea un registro de LicenciaTutor en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaTutor">LicenciaTutor que desea crear</param>
        public void Action(IDataContext dctx, LicenciaTutor licenciaTutor, LicenciaEscuela licenciaEscuela)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaTutor == null)
                sError += ", LicenciaTutor";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaTutorInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaTutor.LicenciaID == null)
                sError += ", LicenciaID";
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuelaID";
            if (licenciaTutor.Usuario == null)
                sError += ", Usuario";
            if (licenciaTutor.Tutor == null)
                sError += ", Tutor";
            if (licenciaTutor.Activo == null)
                sError += ", Activo";
            if (sError.Length > 0)
                throw new Exception("LicenciaTutorInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaTutor.Usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID";
            if (licenciaTutor.Tutor.TutorID == null)
                sError += ", Tutor.TutorID";
            if (sError.Length > 0)
                throw new Exception("LicenciaTutorInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO","LicenciaTutorInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaTutorInsHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO","LicenciaTutorInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Licencia ( LicenciaID, LicenciaEscuelaID, UsuarioID, Tipo, Activo) ");
            // licenciaTutor.LicenciaID
            sCmd.Append(" VALUES ( @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (licenciaTutor.LicenciaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaTutor.LicenciaID;
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
            // licenciaTutor.Usuario.UsuarioID
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (licenciaTutor.Usuario.UsuarioID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaTutor.Usuario.UsuarioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaTutor.Tipo
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = licenciaTutor.Tipo;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaTutor.Activo
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (licenciaTutor.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaTutor.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ) ");

            sCmd.AppendLine();
            sCmd.Append(" INSERT INTO LicenciaReferencia ( LicenciaID, TutorID) ");
            // licenciaTutor.LicenciaID
            sCmd.Append(" VALUES ( @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (licenciaTutor.LicenciaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaTutor.LicenciaID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaTutor.Tutor.TutorID
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (licenciaTutor.Tutor.TutorID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaTutor.Tutor.TutorID;
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
                throw new Exception("LicenciaTutorInsHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("LicenciaTutorInsHlp: Hubo un error al consultar los registros.");
        }
    }
}
