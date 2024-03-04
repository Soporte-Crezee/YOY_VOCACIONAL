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
    /// Actualiza un registro de TipoDocumento en la BD
    /// </summary>
    internal class TipoDocumentoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de TipoDocumentoUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="tipoDocumentoUpdHlp">TipoDocumentoUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">TipoDocumentoUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, TipoDocumento tipoDocumento, TipoDocumento anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (tipoDocumento == null)
                sError += ", TipoDocumento";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("TipoDocumentoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.TipoDocumentoID == null)
                sError += ", Anterior TipoDocumentoID";
            if (sError.Length > 0)
                throw new Exception("TipoDocumentoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (tipoDocumento.Nombre == null || tipoDocumento.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (tipoDocumento.EsEditable == null)
                sError += ", EsEditable";
            if (sError.Length > 0)
                throw new Exception("TipoDocumentoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "TipoDocumentoUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "TipoDocumentoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO",
                   "TipoDocumentoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE TipoDocumento ");
            if (tipoDocumento.Nombre == null)
                sCmd.Append(" SET Nombre = NULL ");
            else
            {
                // tipoDocumento.Nombre
                sCmd.Append(" SET Nombre = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = tipoDocumento.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (tipoDocumento.Extension == null)
                sCmd.Append(" ,Extension = NULL ");
            else
            {
                // tipoDocumento.Extension
                sCmd.Append(" ,Extension = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = tipoDocumento.Extension;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (tipoDocumento.MIME == null)
                sCmd.Append(" ,MIME = NULL ");
            else
            {
                // tipoDocumento.MIME
                sCmd.Append(" ,MIME = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = tipoDocumento.MIME;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (tipoDocumento.EsEditable == null)
                sCmd.Append(" ,EsEditable = NULL ");
            else
            {
                // tipoDocumento.EsEditable
                sCmd.Append(" ,EsEditable = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = tipoDocumento.EsEditable;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (tipoDocumento.Fuente == null)
                sCmd.Append(" ,Fuente = NULL ");
            else
            {
                // tipoDocumento.Fuente
                sCmd.Append(" ,Fuente = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = tipoDocumento.Fuente;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (tipoDocumento.ImagenDocumento == null)
                sCmd.Append(" ,ImagenDocumento = NULL ");
            else
            {
                // tipoDocumento.Fuente
                sCmd.Append(" ,ImagenDocumento = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = tipoDocumento.ImagenDocumento;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.TipoDocumentoID == null)
                sCmd.Append(" WHERE tipoDocumentoID IS NULL ");
            else
            {
                // anterior.TipoDocumentoID
                sCmd.Append(" WHERE tipoDocumentoID = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = anterior.TipoDocumentoID;
                sqlParam.DbType = DbType.Int32;
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
                throw new Exception("TipoDocumentoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("TipoDocumentoUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
