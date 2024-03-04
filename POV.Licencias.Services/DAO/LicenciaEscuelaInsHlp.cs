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
    /// LicenciaEscuelaInsHlp
    /// </summary>
    public class LicenciaEscuelaInsHlp
    {
        /// <summary>
        /// Crea un registro de LicenciaEscuela en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaEscuela">LicenciaEscuela que desea crear</param>
        public void Action(IDataContext dctx, LicenciaEscuela licenciaEscuela)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEscuela.NumeroLicencias == null)
                sError += ", NumeroLicencias";
            if (licenciaEscuela.Activo == null)
                sError += ", Activo";
            if (licenciaEscuela.CicloEscolar == null)
                sError += ", CicloEscolar";
            if (licenciaEscuela.Contrato == null)
                sError += ", Contrato";
            if (licenciaEscuela.Escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEscuela.CicloEscolar.CicloEscolarID == null)
                sError += ", CicloEscolar.CicloEscolarID";
            if (licenciaEscuela.Escuela.EscuelaID == null)
                sError += ", Escuela.EscuelaID";
            if (licenciaEscuela.Contrato.ContratoID == null)
                sError += ", Contrato.ContratoID";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "PISA1021.Licencias.DAO","LicenciaEscuelaInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaEscuelaInsHlp: Hubo un error al conectarse a la base de datos", "PISA1021.Licencias.DAO","LicenciaEscuelaInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO LicenciaEscuela ( NumeroLicencias, CicloEscolarID, EscuelaID, Tipo, Activo, ContratoID ) ");
            // licenciaEscuela.NumeroLicencias
            sCmd.Append(" VALUES ( @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (licenciaEscuela.NumeroLicencias == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaEscuela.NumeroLicencias;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaEscuela.CicloEscolar.CicloEscolarID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (licenciaEscuela.CicloEscolar.CicloEscolarID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaEscuela.CicloEscolar.CicloEscolarID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaEscuela.Escuela.EscuelaID
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (licenciaEscuela.Escuela.EscuelaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaEscuela.Escuela.EscuelaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaEscuela.Tipo
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = (byte)licenciaEscuela.Tipo;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaEscuela.Activo
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (licenciaEscuela.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaEscuela.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // licenciaEscuela.ContratoID
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (licenciaEscuela.Contrato.ContratoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaEscuela.Contrato.ContratoID;
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
                throw new Exception("LicenciaEscuelaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("LicenciaEscuelaInsHlp: Ocurrió un error al ingresar el registro.");
        }

        /// <summary>
        /// Crea un registro de LicenciaEscuelaModuloFuncional en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaEscuela">LicenciaEscuela que desea crear</param>
        /// <param name="moduloFuncional">ModuloFuncional que desea crear</param>
        public void Action(IDataContext dctx, LicenciaEscuela licenciaEscuela, ModuloFuncional moduloFuncional)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (moduloFuncional == null)
                sError += ", ModuloFuncional";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuelaID";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (moduloFuncional.ModuloFuncionalId == null)
                sError += ", ModuloFuncionalId";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO",
                   "LicenciaEscuelaInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaEscuelaModeloFuncionalInsHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO",
                   "LicenciaEscuelaInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO LicenciaEscuelaModuloFuncional (LicenciaEscuelaID, ModuloFuncionalID) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // licenciaEscuela.LicenciaEscuelaId
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = licenciaEscuela.LicenciaEscuelaID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // moduloFuncional.ModuloFuncionalId 
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (moduloFuncional.ModuloFuncionalId == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = moduloFuncional.ModuloFuncionalId;
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
                throw new Exception("LicenciaEscuelaModeloFuncionalInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("LicenciaEscuelaModeloFuncionalInsHlp: Ocurrió un error al ingresar el registro.");
        }  
    
    }
}
