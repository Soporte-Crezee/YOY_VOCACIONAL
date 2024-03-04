using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;

namespace POV.ContenidosDigital.DAO
{
    /// <summary>
    /// Consulta un registro de ContenidoDigital en la BD
    /// </summary>
    internal class ContenidoDigitalRetHlp
    {
        /// <summary>
        /// Consulta registros de ContenidoDigital en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigital">ContenidoDigital que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ContenidoDigital generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, ContenidoDigital contenidoDigital)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (contenidoDigital == null)
                sError += ", ContenidoDigital";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (contenidoDigital.TipoDocumento == null)
            {
                contenidoDigital.TipoDocumento = new TipoDocumento();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidoDigital.DAO",
                   "ContenidoDigitalRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ContenidoDigitalRetHlp: No se pudo conectar a la base de datos", "POV.ContenidoDigital.DAO",
                   "ContenidoDigitalRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ContenidoDigitalID, TipoDocumentoID, Clave, Nombre, EsInterno, EsDescargable, InstitucionOrigen, Tags, FechaRegistro, EstatusContenido ");
            sCmd.Append(" FROM ContenidoDigital ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (contenidoDigital.ContenidoDigitalID != null)
            {
                s_VarWHERE.Append(" ContenidoDigitalID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = contenidoDigital.ContenidoDigitalID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.TipoDocumento.TipoDocumentoID != null)
            {
                s_VarWHERE.Append(" AND TipoDocumentoID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = contenidoDigital.TipoDocumento.TipoDocumentoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.Clave != null)
            {
                s_VarWHERE.Append(" AND Clave = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = contenidoDigital.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.Nombre != null)
            {
                s_VarWHERE.Append(" AND Nombre like @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = contenidoDigital.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.EsInterno != null)
            {
                s_VarWHERE.Append(" AND EsInterno = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = contenidoDigital.EsInterno;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.EsDescargable != null)
            {
                s_VarWHERE.Append(" AND EsDescargable = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = contenidoDigital.EsDescargable;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = contenidoDigital.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.EstatusContenido != null)
            {
                s_VarWHERE.Append(" AND EstatusContenido = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = contenidoDigital.EstatusContenido;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            else
            {

                s_VarWHERE.Append(" AND (EstatusContenido = @dbp4ram9 OR  EstatusContenido = @dbp4ram10) ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = EEstatusContenido.ACTIVO;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);

                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = EEstatusContenido.MANTENIMIENTO;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (contenidoDigital.InstitucionOrigen != null && !string.IsNullOrEmpty(contenidoDigital.InstitucionOrigen.Nombre))
            {
                s_VarWHERE.Append(" AND InstitucionOrigen like @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = contenidoDigital.InstitucionOrigen.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (!string.IsNullOrEmpty(contenidoDigital.Tags))
            {
                s_VarWHERE.Append(" AND Tags like @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = contenidoDigital.Tags;
                sqlParam.DbType = DbType.String;
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
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "ContenidoDigital");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ContenidoDigitalRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
