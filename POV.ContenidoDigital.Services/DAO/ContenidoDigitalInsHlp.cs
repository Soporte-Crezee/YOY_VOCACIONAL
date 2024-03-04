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
    /// Guarda un registro de ContenidoDigital en la BD
    /// </summary>
    internal class ContenidoDigitalInsHlp
    {
        /// <summary>
        /// Crea un registro de ContenidoDigital en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigital">ContenidoDigital que desea crear</param>
        public void Action(IDataContext dctx, ContenidoDigital contenidoDigital)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (contenidoDigital == null)
                sError += ", ContenidoDigital";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (contenidoDigital.TipoDocumento == null)
                sError += ", TipoDocumento";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (contenidoDigital.TipoDocumento.TipoDocumentoID == null)
                sError += ", TipoDocumentoID";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (contenidoDigital.InstitucionOrigen == null)
                sError += ", InstitucionOrigen";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (contenidoDigital.Clave == null || contenidoDigital.Clave.Trim().Length == 0)
                sError += ", Clave";
            if (contenidoDigital.Nombre == null || contenidoDigital.Nombre.Trim().Length == 0)
                sError += ", Nombre";
            if (contenidoDigital.EsInterno == null)
                sError += ", EsInterno";
            if (contenidoDigital.EsDescargable == null)
                sError += ", EsDescargable";
            if (contenidoDigital.Tags == null || contenidoDigital.Tags.Trim().Length == 0)
                sError += ", Tags";
            if (contenidoDigital.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (contenidoDigital.EstatusContenido == null)
                sError += ", EstatusContenido";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.DAO",
                   "ContenidoDigitalInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ContenidoDigitalInsHlp: No se pudo conectar a la base de datos", "POV.ContenidosDigital.DAO",
                   "ContenidoDigitalInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO ContenidoDigital (TipoDocumentoID, Clave, Nombre, EsInterno, EsDescargable, InstitucionOrigen, Tags, FechaRegistro, EstatusContenido) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // contenidoDigital.TipoDocumento.TipoDocumentoID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (contenidoDigital.TipoDocumento.TipoDocumentoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contenidoDigital.TipoDocumento.TipoDocumentoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // contenidoDigital.Clave
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (contenidoDigital.Clave == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contenidoDigital.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contenidoDigital.Nombre
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (contenidoDigital.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contenidoDigital.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contenidoDigital.EsInterno
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (contenidoDigital.EsInterno == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contenidoDigital.EsInterno;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // contenidoDigital.EsDescargable
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (contenidoDigital.EsDescargable == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contenidoDigital.EsDescargable;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // contenidoDigital.InstitucionOrigen.Nombre
            sCmd.Append(" ,@dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            if (contenidoDigital.InstitucionOrigen.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contenidoDigital.InstitucionOrigen.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contenidoDigital.Tags
            sCmd.Append(" ,@dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            if (contenidoDigital.Tags == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contenidoDigital.Tags;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            // contenidoDigital.FechaRegistro
            sCmd.Append(" ,@dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            if (contenidoDigital.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contenidoDigital.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // contenidoDigital.EstatusContenido
            sCmd.Append(" ,@dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            if (contenidoDigital.EstatusContenido == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = contenidoDigital.EstatusContenido;
            sqlParam.DbType = DbType.Int16;
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
                throw new Exception("ContenidoDigitalInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ContenidoDigitalInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
