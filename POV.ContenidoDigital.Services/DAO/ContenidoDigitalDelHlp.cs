using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;

namespace POV.ContenidosDigital.DAO
{
    /// <summary>
    /// Elimina un registro de contenido digital en la base de datos.
    /// </summary>
    internal class ContenidoDigitalDelHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de ContenidoDigitalUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigital">ContenidoDigital que tiene los datos nuevos</param>
        /// <param name="anterior">ContenidoDigital que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, ContenidoDigital contenidoDigital, ContenidoDigital anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (contenidoDigital == null)
                sError += ", ContenidoDigital";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.ContenidoDigitalID == null)
                sError += ", Anterior ContenidoDigitalID";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.DAO",
                   "ContenidoDigitalDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ContenidoDigitalDelHlp: Hubo un error al conectarse a la base de datos", "POV.ContenidosDigital.DAO",
                   "ContenidoDigitalDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE ContenidoDigital ");
            StringBuilder s_VarSET = new StringBuilder();
            if (contenidoDigital.TipoDocumento.TipoDocumentoID != null)
            {
                s_VarSET.Append(" TipoDocumentoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = contenidoDigital.TipoDocumento.TipoDocumentoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.Clave != null)
            {
                s_VarSET.Append(" ,Clave = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = contenidoDigital.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.Nombre != null)
            {
                s_VarSET.Append(" ,Nombre = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = contenidoDigital.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.EsInterno != null)
            {
                s_VarSET.Append(" ,EsInterno = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = contenidoDigital.EsInterno;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.EsDescargable != null)
            {
                s_VarSET.Append(" ,EsDescargable = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = contenidoDigital.EsDescargable;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.InstitucionOrigen.Nombre != null)
            {
                s_VarSET.Append(" ,InstitucionOrigen = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = contenidoDigital.InstitucionOrigen.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.EstatusContenido != null)
            {
                s_VarSET.Append(" ,EstatusContenido = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = contenidoDigital.EstatusContenido;
                sqlParam.DbType = DbType.Int16;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.Tags != null)
            {
                s_VarSET.Append(" ,Tags = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = contenidoDigital.Tags;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            string s_VarSETres = s_VarSET.ToString().Trim();
            if (s_VarSETres.Length > 0)
            {
                if (s_VarSETres.StartsWith("AND "))
                    s_VarSETres = s_VarSETres.Substring(4);
                else if (s_VarSETres.StartsWith("OR "))
                    s_VarSETres = s_VarSETres.Substring(3);
                else if (s_VarSETres.StartsWith(","))
                    s_VarSETres = s_VarSETres.Substring(1);
                sCmd.Append(" SET " + s_VarSETres);
            }
            StringBuilder s_VarWHERE = new StringBuilder();
            if (anterior.ContenidoDigitalID != null)
            {
                s_VarWHERE.Append(" ContenidoDigitalID = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = anterior.ContenidoDigitalID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.TipoDocumento.TipoDocumentoID != null)
            {
                s_VarWHERE.Append(" AND TipoDocumentoID = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = anterior.TipoDocumento.TipoDocumentoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Clave != null)
            {
                s_VarWHERE.Append(" AND Clave = @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = anterior.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Nombre != null)
            {
                s_VarWHERE.Append(" AND Nombre = @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = anterior.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.EsInterno != null)
            {
                s_VarWHERE.Append(" AND EsInterno = @dbp4ram13 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram13";
                sqlParam.Value = anterior.EsInterno;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.EsDescargable != null)
            {
                s_VarWHERE.Append(" AND EsDescargable = @dbp4ram14 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram14";
                sqlParam.Value = anterior.EsDescargable;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.InstitucionOrigen.Nombre != null)
            {
                s_VarWHERE.Append(" AND InstitucionOrigen = @dbp4ram15 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram15";
                sqlParam.Value = anterior.InstitucionOrigen.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.Tags != null)
            {
                s_VarWHERE.Append(" AND Tags = @dbp4ram16 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram16";
                sqlParam.Value = anterior.Tags;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.EstatusContenido != null)
            {
                s_VarWHERE.Append(" AND EstatusContenido = @dbp4ram17 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram17";
                sqlParam.Value = anterior.EstatusContenido;
                sqlParam.DbType = DbType.Int16;
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
                throw new Exception("ContenidoDigitalDelHlp: Hubo  un Error al Eliminar el Registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ContenidoDigitalDelHlp: Hubo  un Error al Eliminar el Registro. ");
        }
    }
}
