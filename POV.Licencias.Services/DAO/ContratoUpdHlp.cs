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
    internal class ContratoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de Contrato en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Contrato que tiene los datos nuevos</param>
        /// <param name="anterior">Contrato que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, Contrato contrato, Contrato anterior)
        {
            object myFirm = new object();
            string sError = "";
            if (contrato == null)
                sError += ", Contrato";
            if (anterior == null)
                sError += ", Contrato anterior";
            if (sError.Length > 0)
                throw new Exception("ContratoUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (contrato.ContratoID == null)
                sError += ", ContratoID";
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
            if (anterior.ContratoID == null)
                sError += ", ContratoID anterior";
            if (anterior.Clave == null || anterior.Clave.Trim().Length == 0)
                sError += ", Clave anterior";
            if (anterior.FechaContrato == null)
                sError += ", FechaContrato anterior";
            if (anterior.InicioContrato == null)
                sError += ", InicioContrato anterior";
            if (anterior.FinContrato == null)
                sError += ", FinContrato anterior";
            if (anterior.LicenciasLimitadas == null)
                sError += ", LicenciasLimitadas anterior";
            if (anterior.Cliente == null)
                sError += ", Cliente anterior";
            if (anterior.Ubicacion == null)
                sError += ", Ubicacion anterior";
            if (anterior.UsuarioRegistro == null)
                sError += ", UsuarioRegistro anterior";
            if (anterior.FechaRegistro == null)
                sError += ", FechaRegistro anterior";
            if (sError.Length > 0)
                throw new Exception("ContratoUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
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
            if (anterior.Cliente.Nombre == null || anterior.Cliente.Nombre.Trim().Length == 0)
                sError += ", Cliente.Nombre anterior";
            if (anterior.Cliente.Domicilio == null || anterior.Cliente.Domicilio.Trim().Length == 0)
                sError += ", Cliente.Domicilio anterior";
            if (anterior.Cliente.Representante == null || anterior.Cliente.Representante.Trim().Length == 0)
                sError += ", Cliente.Representante anterior";
            if (anterior.Cliente.Telefono == null || anterior.Cliente.Telefono.Trim().Length == 0)
                sError += ", Cliente.Telefono anterior";
            if (anterior.Ubicacion.UbicacionID == null)
                sError += ", Ubicacion.UbicacionID anterior";
            if (anterior.UsuarioRegistro.UsuarioID == null)
                sError += ", UsuarioRegistro.UsuarioID anterior";
            if (sError.Length > 0)
                throw new Exception("ContratoUpdHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", "ContratoUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ContratoUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", "ContratoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Contrato ");
            // contrato.Clave
            sCmd.Append(" SET Clave =@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (contrato.Clave == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.FechaContrato
            sCmd.Append(" ,FechaContrato =@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (contrato.FechaContrato == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.FechaContrato;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.InicioContrato
            sCmd.Append(" ,InicioContrato =@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (contrato.InicioContrato == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.InicioContrato;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.FinContrato
            sCmd.Append(" ,FinContrato =@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (contrato.FinContrato == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.FinContrato;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.LicenciasLimintadas
            sCmd.Append(" ,LicenciasLimitadas =@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (contrato.LicenciasLimitadas == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.LicenciasLimitadas;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.Estatus
            sCmd.Append(" ,Estatus =@dbp4ram51 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram51";
            if (contrato.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.NumeroLicencias
            sCmd.Append(" ,NumeroLicencias =@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (contrato.NumeroLicencias == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.NumeroLicencias;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.Cliente.Nombre
            sCmd.Append(" ,ClienteNombre =@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (contrato.Cliente.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Cliente.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.Cliente.Domicilio
            sCmd.Append(" ,ClienteDomicilio =@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (contrato.Cliente.Domicilio == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Cliente.Domicilio;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.Cliente.Representante
            sCmd.Append(" ,ClienteRepresentante =@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if (contrato.Cliente.Representante == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Cliente.Representante;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.Cliente.Telefono
            sCmd.Append(" ,ClienteTelefono =@dbp4ram10 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram10";
            if (contrato.Cliente.Telefono == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Cliente.Telefono;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contrato.Ubicacion.UbicacionID
            sCmd.Append(" ,UbicacionID =@dbp4ram11 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram11";
            if (contrato.Ubicacion.UbicacionID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contrato.Ubicacion.UbicacionID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            if (anterior.ContratoID == null)
                sCmd.Append(" WHERE ContratoID = NULL ");
            else
            {
                // anterior.ContratoID
                sCmd.Append(" WHERE ContratoID = @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = anterior.ContratoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Clave == null)
                sCmd.Append(" AND Clave = NULL ");
            else
            {
                // anterior.Clave
                sCmd.Append(" AND Clave = @dbp4ram13 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram13";
                sqlParam.Value = anterior.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.FechaContrato == null)
                sCmd.Append(" AND FechaContrato = NULL ");
            else
            {
                // anterior.FechaContrato
                sCmd.Append(" AND FechaContrato = @dbp4ram14 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram14";
                sqlParam.Value = anterior.FechaContrato;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.InicioContrato == null)
                sCmd.Append(" AND InicioContrato = NULL ");
            else
            {
                // anterior.InicioContrato
                sCmd.Append(" AND InicioContrato = @dbp4ram15 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram15";
                sqlParam.Value = anterior.InicioContrato;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.FinContrato == null)
                sCmd.Append(" AND FinContrato = NULL ");
            else
            {
                // anterior.FinContrato
                sCmd.Append(" AND FinContrato = @dbp4ram16 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram16";
                sqlParam.Value = anterior.FinContrato;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.LicenciasLimitadas == null)
                sCmd.Append(" AND LicenciasLimitadas = NULL ");
            else
            {
                // anterior.LicenciasLimintadas
                sCmd.Append(" AND LicenciasLimitadas = @dbp4ram17 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram17";
                sqlParam.Value = anterior.LicenciasLimitadas;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Estatus == null)
                sCmd.Append(" AND Estatus = NULL ");
            else
            {
                // anterior.LicenciasLimintadas
                sCmd.Append(" AND Estatus = @dbp4ram171 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram171";
                sqlParam.Value = anterior.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.NumeroLicencias == null)
                sCmd.Append(" AND NumeroLicencias = NULL ");
            else
            {
                // anterior.NumeroLicencias
                sCmd.Append(" AND NumeroLicencias = @dbp4ram18 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram18";
                sqlParam.Value = anterior.NumeroLicencias;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Cliente.Nombre == null)
                sCmd.Append(" AND ClienteNombre = NULL ");
            else
            {
                // anterior.Cliente.Nombre
                sCmd.Append(" AND ClienteNombre = @dbp4ram19 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram19";
                sqlParam.Value = anterior.Cliente.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Cliente.Domicilio == null)
                sCmd.Append(" AND ClienteDomicilio = NULL ");
            else
            {
                // anterior.Cliente.Domicilio
                sCmd.Append(" AND ClienteDomicilio = @dbp4ram20 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram20";
                sqlParam.Value = anterior.Cliente.Domicilio;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Cliente.Representante == null)
                sCmd.Append(" AND ClienteRepresentante = NULL ");
            else
            {
                // anterior.Cliente.Representante
                sCmd.Append(" AND ClienteRepresentante = @dbp4ram21 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram21";
                sqlParam.Value = anterior.Cliente.Representante;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Cliente.Telefono == null)
                sCmd.Append(" AND ClienteTelefono = NULL ");
            else
            {
                // anterior.Cliente.Telefono
                sCmd.Append(" AND ClienteTelefono = @dbp4ram22 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram22";
                sqlParam.Value = anterior.Cliente.Telefono;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Ubicacion.UbicacionID == null)
                sCmd.Append(" AND UbicacionID = NULL ");
            else
            {
                // anterior.Ubicacion.UbicacionID
                sCmd.Append(" AND UbicacionID = @dbp4ram23 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram23";
                sqlParam.Value = anterior.Ubicacion.UbicacionID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.UsuarioRegistro.UsuarioID == null)
                sCmd.Append(" AND UsuarioID = NULL ");
            else
            {
                // anterior.Usuario.UsuarioID
                sCmd.Append(" AND UsuarioID = @dbp4ram24 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram24";
                sqlParam.Value = anterior.UsuarioRegistro.UsuarioID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.FechaRegistro == null)
                sCmd.Append(" AND FechaRegistro = NULL ");
            else
            {
                // anterior.FechaRegistro.FechaRegistroID
                sCmd.Append(" AND FechaRegistro = @dbp4ram25 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram25";
                sqlParam.Value = anterior.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
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
                throw new Exception("ContratoUpdHlp: Hubo un error al actualizar el registro." + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ContratoUpdHlp: Hubo un error al actualizar el registro.");
        }
    }
}
