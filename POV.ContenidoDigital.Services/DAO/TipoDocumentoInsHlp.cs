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
    /// Guarda un registro de TipoDocumento en la BD
    /// </summary>
    internal class TipoDocumentoInsHlp
    {
        /// <summary>
        /// Crea un registro de TipoDocumento en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="tipoDocumento">TipoDocumento que desea crear</param>
        public void Action(IDataContext dctx, TipoDocumento tipoDocumento)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (tipoDocumento == null)
                sError += ", TipoDocumento";
            if (sError.Length > 0)
                throw new Exception("TipoDocumentoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (tipoDocumento.Nombre == null || tipoDocumento.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (tipoDocumento.EsEditable == null)
                sError += ", EsEditable";
            if (sError.Length > 0)
                throw new Exception("TipoDocumentoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "TipoDocumentoInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "TipoDocumentoInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO",
                   "TipoDocumentoInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO TipoDocumento (Nombre,Extension,MIME,EsEditable,Fuente,Activo,FechaRegistro, ImagenDocumento) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // tipoDocumento.Nombre
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (tipoDocumento.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = tipoDocumento.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // tipoDocumento.Extension
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (tipoDocumento.Extension == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = tipoDocumento.Extension;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // tipoDocumento.MIME
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (tipoDocumento.MIME == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = tipoDocumento.MIME;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // tipoDocumento.EsEditable
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (tipoDocumento.EsEditable == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = tipoDocumento.EsEditable;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // tipoDocumento.Fuente
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (tipoDocumento.Fuente == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = tipoDocumento.Fuente;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // tipoDocumento.Activo
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (tipoDocumento.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = tipoDocumento.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // tipoDocumento.FechaRegistro
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (tipoDocumento.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = tipoDocumento.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // tipoDocumento.imagen
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (tipoDocumento.ImagenDocumento == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = tipoDocumento.ImagenDocumento;
            sqlParam.DbType = DbType.String;
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
                throw new Exception("TipoDocumentoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("TipoDocumentoInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
