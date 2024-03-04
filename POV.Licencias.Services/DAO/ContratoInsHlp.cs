// Licencias de escuela
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Localizacion.BO;
using POV.Seguridad.BO;
using POV.Licencias.BO;

namespace POV.Licencias.DAO
{
    /// <summary>
    /// Contrato
    /// </summary>
    internal class ContratoInsHlp
    {
        /// <summary>
        /// Crea un registro de Contrato en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Contrato que desea crear</param>
        public void Action(IDataContext dctx, Contrato contrato)
        {
            object myFirm = new object();
            string sError = "";
            if (contrato == null)
                sError += ", Contrato";
            if (sError.Length > 0)
                throw new Exception("ContratoInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (contrato.Clave == null || contrato.Clave.Trim().Length == 0)
                sError += ", Clave";
            if (contrato.FechaContrato == null)
                sError += ", FechaContrato";
            if (contrato.InicioContrato == null)
                sError += ", InicioContrato";
            if (contrato.FinContrato == null)
                sError += ", FinContrato";
            if (contrato.LicenciasLimitadas == null)
                sError += ", LicenciasLimitadas";
            if (contrato.Cliente == null)
                sError += ", Cliente";
            if (contrato.Ubicacion == null)
                sError += ", Ubicacion";
            if (contrato.UsuarioRegistro == null)
                sError += ", UsuarioRegistro";
            if (contrato.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (sError.Length > 0)
                throw new Exception("ContratoInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (contrato.Cliente.Nombre == null || contrato.Cliente.Nombre.Trim().Length == 0)
                sError += ", Cliente.Nombre";
            if (contrato.Cliente.Domicilio == null || contrato.Cliente.Domicilio.Trim().Length == 0)
                sError += ", Cliente.Domicilio";
            if (contrato.Cliente.Representante == null || contrato.Cliente.Representante.Trim().Length == 0)
                sError += ", Cliente.Representante";
            if (contrato.Cliente.Telefono == null || contrato.Cliente.Telefono.Trim().Length == 0)
                sError += ", Cliente.Telefono";
            if (contrato.Ubicacion.UbicacionID == null)
                sError += ", Ubicacion.UbicacionID";
            if (contrato.UsuarioRegistro.UsuarioID == null)
                sError += ", UsuarioRegistro.UsuarioID";
            if (sError.Length > 0)
                throw new Exception("ContratoInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", "ContratoInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ContratoInsHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", "ContratoInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Contrato (Clave, FechaContrato, InicioContrato, FinContrato, LicenciasLimitadas, NumeroLicencias, ClienteNombre, ClienteDomicilio, ClienteRepresentante, ClienteTelefono, UbicacionID, UsuarioID, FechaRegistro, Estatus) ");
            // contrato.Clave
            sCmd.Append(" VALUES (@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (contrato.Clave == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.FechaContrato
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (contrato.FechaContrato == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.FechaContrato;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.InicioContrato
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (contrato.InicioContrato == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.InicioContrato;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.FinContrato
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (contrato.FinContrato == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.FinContrato;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.LicenciasLimintadas
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (contrato.LicenciasLimitadas == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.LicenciasLimitadas;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.NumeroLicencias
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (contrato.NumeroLicencias == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.NumeroLicencias;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.Cliente.Nombre
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (contrato.Cliente.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Cliente.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.Cliente.Domicilio
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (contrato.Cliente.Domicilio == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Cliente.Domicilio;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.Cliente.Representante
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if (contrato.Cliente.Representante == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Cliente.Representante;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.Cliente.Telefono
            sCmd.Append(" ,@dbp4ram10 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram10";
            if (contrato.Cliente.Telefono == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Cliente.Telefono;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.Ubicacion.UbicacionID
            sCmd.Append(" ,@dbp4ram11 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram11";
            if (contrato.Ubicacion.UbicacionID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Ubicacion.UbicacionID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.UsuarioRegistro.UsuarioID
            sCmd.Append(" ,@dbp4ram12 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram12";
            if (contrato.UsuarioRegistro.UsuarioID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.UsuarioRegistro.UsuarioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.FechaRegistro.FechaRegistroID
            sCmd.Append(" ,@dbp4ram13 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram13";
            if (contrato.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@dbp4ram14 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram14";
            if (contrato.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Estatus;
            sqlParam.DbType = DbType.Boolean;
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
                throw new Exception("ContratoInsHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ContratoInsHlp: Hubo un error al consultar los registros.");
        }
    }
}
