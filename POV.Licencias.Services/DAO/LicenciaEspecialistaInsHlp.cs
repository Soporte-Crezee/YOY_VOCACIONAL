using Framework.Base.DataAccess;
namespace POV.Licencias.DAO
{
    using Framework.Base.Exceptions;
    using POV.Licencias.BO;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class LicenciaEspecialistaInsHlp
    {
        public void Action(IDataContext dctx, LicenciaEspecialistaPruebas licenciaEspecialista, LicenciaEscuela licenciaEscuela)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaEspecialista == null)
                sError += ", LicenciaEspecialista";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaEspecialistaInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEspecialista.LicenciaID == null)
                sError += ", LicenciaID";
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuelaID";
            if (licenciaEspecialista.Usuario == null)
                sError += ", Usuario";
            if (licenciaEspecialista.EspecialistaPruebas == null)
                sError += ", Especialista";
            if (licenciaEspecialista.Activo == null)
                sError += ", Activo";
            if (sError.Length > 0)
                throw new Exception("LicenciaEspecialistaInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEspecialista.Usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID";
            if (licenciaEspecialista.EspecialistaPruebas.EspecialistaPruebaID == null)
                sError += ", EspecialistaPruebas.EspecialistaID";
            if (sError.Length > 0)
                throw new Exception("LicenciaEspecialistaInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", "LicenciaEspecialistaInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaDocenteInsHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", "LicenciaDocenteInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Licencia ( LicenciaID, LicenciaEscuelaID, UsuarioID, Tipo, Activo) ");
            // licenciaEspecialista.LicenciaID
            sCmd.Append(" VALUES ( @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (licenciaEspecialista.LicenciaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaEspecialista.LicenciaID;
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
            if (licenciaEspecialista.Usuario.UsuarioID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaEspecialista.Usuario.UsuarioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = licenciaEspecialista.Tipo;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaDocente.Activo
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (licenciaEspecialista.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaEspecialista.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ) ");

            sCmd.AppendLine();
            sCmd.Append(" INSERT INTO LicenciaReferencia ( LicenciaID, EspecialistaID) ");
            // licenciaDocente.LicenciaID
            sCmd.Append(" VALUES ( @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (licenciaEspecialista.LicenciaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaEspecialista.LicenciaID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaDocente.Docente.DocenteID
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (licenciaEspecialista.EspecialistaPruebas.EspecialistaPruebaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaEspecialista.EspecialistaPruebas.EspecialistaPruebaID;
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
                throw new Exception("LicenciaEspecialistaInsHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("LicenciaEspecialistaInsHlp: Hubo un error al consultar los registros.");
        }
    }
}
