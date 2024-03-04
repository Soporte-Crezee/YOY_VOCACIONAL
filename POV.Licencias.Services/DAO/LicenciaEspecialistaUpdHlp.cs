using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace POV.Licencias.DAO
{    
    public class LicenciaEspecialistaUpdHlp
    {
        public void Action(IDataContext dctx, LicenciaEspecialistaPruebas licenciaEspecialista, LicenciaEspecialistaPruebas anterior, LicenciaEscuela licenciaEscuela)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaEspecialista == null)
                sError += ", LicenciaEspecialista";
            if (anterior == null)
                sError += ", LicenciaEspecialista anterior";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaEspecialistaUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuelaID";
            if (sError.Length > 0)
                throw new Exception("LicenciaEspecialistaUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEspecialista.LicenciaID == null)
                sError += ", LicenciaID";
            if (licenciaEspecialista.Usuario == null)
                sError += ", Usuario";
            if (licenciaEspecialista.EspecialistaPruebas == null)
                sError += ", Especialista";
           if (licenciaEspecialista.Activo == null)
                sError += ", Activo";
            if (sError.Length > 0)
                throw new Exception("LicenciaEspecialistaUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEspecialista.Usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID";
            if (licenciaEspecialista.EspecialistaPruebas.EspecialistaPruebaID == null)
                sError += ", Especialista.EspecialistaID";
            if (sError.Length > 0)
                throw new Exception("LicenciaEspecialistaUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (anterior.LicenciaID == null)
                sError += ", LicenciaID anterior";
            if (anterior.Usuario == null)
                sError += ", Usuario anterior";
            if (anterior.EspecialistaPruebas == null)
                sError += ", Especialista anterior";
            if (anterior.Activo == null)
                sError += ", Activo anterior";
            if (sError.Length > 0)
                throw new Exception("LicenciaDocenteUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (anterior.Usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID anterior";
            if (anterior.EspecialistaPruebas.EspecialistaPruebaID == null)
                sError += ", Docente.DocenteID anterior";
            if (sError.Length > 0)
                throw new Exception("LicenciaEspecialistaUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", "LicenciaEspecialistaUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaEspecialistaUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", "LicenciaEspecialistaUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Licencia ");
            if (licenciaEspecialista.Activo == null)
                sCmd.Append(" SET Activo = NULL ");
            else
            {
                // licenciaDocente.Activo
                sCmd.Append(" SET Activo =@dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = licenciaEspecialista.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.LicenciaID == null)
                sCmd.Append(" WHERE LicenciaID IS NULL ");
            else
            {
                // licenciaDocente.LicenciaID
                sCmd.Append(" WHERE LicenciaID =@dbp4ram2");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = anterior.LicenciaID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela.LicenciaEscuelaID == null)
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
            
            // licenciaDocente.Tipo
            sCmd.Append(" AND Tipo =@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = anterior.Tipo;
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
                throw new Exception("LicenciaEspecialistaUpdHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("LicenciaEspecialistaUpdHlp: Hubo un error al consultar los registros.");
        }
    }
}
