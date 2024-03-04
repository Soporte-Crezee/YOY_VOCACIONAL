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
    /// LicenciaEscuelaUpdHlp
    /// </summary>
    public class LicenciaEscuelaUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de LicenciaEscuela en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaEscuela">LicenciaEscuela que tiene los datos nuevos</param>
        /// <param name="anterior">LicenciaEscuela que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, LicenciaEscuela licenciaEscuela, LicenciaEscuela anterior)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuelaID";
            if (licenciaEscuela.NumeroLicencias == null)
                sError += ", NumeroLicencias";
            if (licenciaEscuela.Activo == null)
                sError += ", Activo";
            if (licenciaEscuela.CicloEscolar == null)
                sError += ", CicloEscolar";
            if (licenciaEscuela.Escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEscuela.CicloEscolar.CicloEscolarID == null)
                sError += ", CicloEscolar.CicloEscolarID";
            if (licenciaEscuela.Escuela.EscuelaID == null)
                sError += ", Escuela.EscuelaID";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (anterior == null)
                sError += ", LicenciaEscuela anterior";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (anterior.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuelaID  anterior";
            if (anterior.NumeroLicencias == null)
                sError += ", NumeroLicencias  anterior";
            if (anterior.Activo == null)
                sError += ", Activo anterior";
            if (anterior.CicloEscolar == null)
                sError += ", CicloEscolar anterior";
            if (anterior.Escuela == null)
                sError += ", Escuela anterior";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (anterior.CicloEscolar.CicloEscolarID == null)
                sError += ", CicloEscolar.CicloEscolarID anterior";
            if (anterior.Escuela.EscuelaID == null)
                sError += ", Escuela.EscuelaID anterior";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "PISA1021.Licencias.DAO", "LicenciaEscuelaUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaEscuelaUpdHlp: Hubo un error al conectarse a la base de datos", "PISA1021.Licencias.DAO", "LicenciaEscuelaUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE LicenciaEscuela ");
            if (licenciaEscuela.NumeroLicencias == null)
                sCmd.Append(" SET NumeroLicencias = NULL ");
            else
            {
                // licenciaEscuela.NumeroLicencias
                sCmd.Append(" SET NumeroLicencias = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = licenciaEscuela.NumeroLicencias;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela.Activo == null)
                sCmd.Append(" ,Activo = NULL ");
            else
            {
                // licenciaEscuela.Activo
                sCmd.Append(" ,Activo = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = licenciaEscuela.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela.CicloEscolar.CicloEscolarID != null)
            {
                // licenciaEscuela.Activo
                sCmd.Append(" ,CicloEscolarID = @dbp4ram21 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram21";
                sqlParam.Value = licenciaEscuela.CicloEscolar.CicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.LicenciaEscuelaID == null)
                sCmd.Append(" WHERE LicenciaEscuelaID IS NULL ");
            else
            {
                // anterior.LicenciaEscuelaID
                sCmd.Append(" WHERE LicenciaEscuelaID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = anterior.LicenciaEscuelaID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.NumeroLicencias == null)
                sCmd.Append(" AND NumeroLicencias IS NULL ");
            else
            {
                // anterior.NumeroLicencias
                sCmd.Append(" AND NumeroLicencias = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = anterior.NumeroLicencias;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.CicloEscolar.CicloEscolarID == null)
                sCmd.Append(" AND CicloEscolarID IS NULL ");
            else
            {
                // anterior.CicloEscolar.CicloEscolarID
                sCmd.Append(" AND CicloEscolarID =@dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = anterior.CicloEscolar.CicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Escuela.EscuelaID == null)
                sCmd.Append(" AND EscuelaID IS NULL ");
            else
            {
                // anterior.Escuela.EscuelaID
                sCmd.Append(" AND EscuelaID =@dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = anterior.Escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // anterior.Tipo
            sCmd.Append(" AND Tipo =@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = (byte)anterior.Tipo;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            if (anterior.Activo == null)
                sCmd.Append(" AND Activo IS NULL ");
            else
            {
                // anterior.Activo
                sCmd.Append(" AND Activo =@dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
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
                throw new Exception("LicenciaEscuelaUpdHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("LicenciaEscuelaUpdHlp: Ocurrió un error al ingresar el registro.");
        }


        /// <summary>
        /// Elimina de manera permanente los modulos funcionales relacionados a la licencia escuela
        /// </summary>
        /// <param name="dctx">data context que provee la conexion a la base de datos</param>
        /// <param name="licenciaEscuela">Licencia de la escuela</param>
        public void Action(IDataContext dctx, LicenciaEscuela licenciaEscuela)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaModeloFuncionalDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuelaID";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaModeloFuncionalDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO",
                   "LicenciaEscuelaModeloFuncionalDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaEscuelaModeloFuncionalDelHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO",
                   "LicenciaEscuelaModeloFuncionalDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" DELETE FROM LicenciaEscuelaModuloFuncional ");
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sCmd.Append(" WHERE LicenciaEscuelaID IS NULL ");
            else
            {
                sCmd.Append(" WHERE LicenciaEscuelaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = licenciaEscuela.LicenciaEscuelaID;
                sqlParam.DbType = DbType.Int64;
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
                throw new Exception("LicenciaEscuelaModeloFuncionalDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            
        }
    }
}
