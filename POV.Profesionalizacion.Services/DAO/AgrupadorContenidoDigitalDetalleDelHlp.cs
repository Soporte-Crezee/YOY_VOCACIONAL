using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO
{
    /// <summary>
    /// Elimina un registro de AgrupadorContenidoDigitalDetalle en la BD
    /// </summary>    
    internal class AgrupadorContenidoDigitalDetalleDelHlp
    {    
        /// <summary>
        /// Elimina un registro de AgrupadorContenidoDigitalDetalle en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="agrupadorContenidoDigital">agrupadorContenidoDigital</param>
        /// <param name="contenidoDigital">contenidoDigital</param>
        public void Action(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenidoDigital, ContenidoDigital contenidoDigital)
        {
            object myFirm = new object();
            string sError = String.Empty;

            if (agrupadorContenidoDigital == null)
                sError += ", agrupadorContenidoDigital";
            if (sError.Length > 0)
                throw new Exception("AgrupadorContenidoDigitalDetalleDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (agrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
                sError += ", AgrupadorContenidoDigitalID";
            if (sError.Length > 0)
                throw new Exception("AgrupadorContenidoDigitalDetalleDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (contenidoDigital == null)
                sError += ", contenidoDigital";
            if (sError.Length > 0)
                throw new Exception("AgrupadorContenidoDigitalDetalleDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (contenidoDigital.ContenidoDigitalID == null)
                sError += ", ContenidoDigitalID";
            if (sError.Length > 0)
                throw new Exception("AgrupadorContenidoDigitalDetalleDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "AgrupadorContenidoDigitalDetalleDelHlp", "Action", null, null);
            
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AgrupadorContenidoDigitalDetalleDelHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO",
                   "AgrupadorContenidoDigitalDetalleDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" DELETE FROM AgrupadorContenidoDigitalDetalle ");

            if (agrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
                sCmd.Append(" WHERE AgrupadorContenidoDigitalID IS NULL ");
            else
            {
                sCmd.Append(" WHERE AgrupadorContenidoDigitalID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = agrupadorContenidoDigital.AgrupadorContenidoDigitalID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigital.ContenidoDigitalID == null)
                sCmd.Append(" AND ContenidoDigitalID IS NULL ");
            else
            {
                sCmd.Append(" AND ContenidoDigitalID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = contenidoDigital.ContenidoDigitalID;
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
                throw new Exception("AgrupadorContenidoDigitalDetalleDelHlp: Ocurrió un error al eliminar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("AgrupadorContenidoDigitalDetalleDelHlp: Ocurrió un error al eliminar el registro.");
        }
    }
}
