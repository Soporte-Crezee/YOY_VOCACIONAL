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
    internal class ContratoRetHlp
    {
        /// <summary>
        /// Consulta registros de Contrato en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contrato">Contrato que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Contrato generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Contrato contrato)
        {
            object myFirm = new object();
            string sError = "";
            if (contrato == null)
                sError += ", Contrato";
            if (sError.Length > 0)
                throw new Exception("ContratoRetHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (contrato.Cliente == null)
            {
                contrato.Cliente = new Cliente();
            }
            if (contrato.Ubicacion == null)
            {
                contrato.Ubicacion = new Ubicacion();
            }
            if (contrato.UsuarioRegistro == null)
            {
                contrato.UsuarioRegistro = new Usuario();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", "ContratoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ContratoRetHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", "ContratoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ContratoID, Clave, FechaContrato, InicioContrato, FinContrato, LicenciasLimitadas, NumeroLicencias, ClienteNombre, ClienteDomicilio, ClienteRepresentante, ClienteTelefono, UbicacionID, UsuarioID, FechaRegistro, Estatus ");
            sCmd.Append(" FROM Contrato ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (contrato.ContratoID != null)
            {
                s_VarWHERE.Append(" ContratoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = contrato.ContratoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contrato.Clave != null && contrato.Clave.Trim().Length != 0)
            {
                s_VarWHERE.Append(" AND Clave = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = contrato.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contrato.Cliente.Nombre != null && contrato.Cliente.Nombre.Trim().Length != 0)
            {
                s_VarWHERE.Append(" AND ClienteNombre LIKE @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = contrato.Cliente.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contrato.Estatus != null )
            {
                s_VarWHERE.Append(" AND Estatus = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = contrato.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            string s_VarWHEREres = s_VarWHERE.ToString().Trim();
            if (s_VarWHEREres.Length > 0)
            {
                if (s_VarWHEREres.StartsWith("AND "))
                    s_VarWHEREres = s_VarWHEREres.Substring(4);
                else if (s_VarWHEREres.StartsWith("OR "))
                    s_VarWHEREres = s_VarWHEREres.Substring(3);
                else if (s_VarWHEREres.StartsWith(","))
                    s_VarWHEREres = s_VarWHEREres.Substring(1);
                sCmd.Append(" WHERE " + s_VarWHEREres);
            }
            sCmd.Append(" ORDER BY ContratoID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Contrato");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ContratoRetHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }
    }
}
