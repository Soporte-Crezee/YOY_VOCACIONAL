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
    /// Elimina un registro de ContenidoDigitalAgrupador en la BD
    /// </summary>    
    internal class ContenidoDigitalAgrupadorDelHlp
    {
        /// <summary>
        /// Elimina un registro de ContenidoDigitalAgrupador en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>        
        /// <param name="contenidoDigitalAgrupador">contenidoDigitalAgrupador</param>
        public void Action(IDataContext dctx, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            object myFirm = new object();
            string sError = String.Empty;

            if (contenidoDigitalAgrupador == null)
                sError += ", contenidoDigitalAgrupador";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalAgrupadorDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (contenidoDigitalAgrupador.EjeTematico == null)
                sError += ", EjeTematico";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalAgrupadorDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (contenidoDigitalAgrupador.EjeTematico.EjeTematicoID == null)
                sError += ", EjeTematico.EjeTematicoID";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalAgrupadorDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (contenidoDigitalAgrupador.SituacionAprendizaje == null)
                sError += ", SituacionAprendizaje";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalAgrupadorDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (contenidoDigitalAgrupador.SituacionAprendizaje.SituacionAprendizajeID == null)
                sError += ", SituacionAprendizaje.SituacionAprendizajeID";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalAgrupadorDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (contenidoDigitalAgrupador.AgrupadorContenidoDigital == null)
                sError += ", AgrupadorContenidoDigital";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalAgrupadorDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (contenidoDigitalAgrupador.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
                sError += ", AgrupadorContenidoDigital.AgrupadorContenidoDigitalID";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalAgrupadorDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (contenidoDigitalAgrupador.ContenidoDigital == null)
                sError += ", ContenidoDigital";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalAgrupadorDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (contenidoDigitalAgrupador.ContenidoDigital.ContenidoDigitalID == null)
                sError += ", ContenidoDigital.ContenidoDigitalID";
            if (sError.Length > 0)
                throw new Exception("ContenidoDigitalAgrupadorDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "ContenidoDigitalAgrupadorDelHlp", "Action", null, null);

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ContenidoDigitalAgrupadorDelHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO",
                   "ContenidoDigitalAgrupadorDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" DELETE FROM ContenidoDigitalAgrupador ");

            if (contenidoDigitalAgrupador.EjeTematico.EjeTematicoID == null)
                sCmd.Append(" WHERE EjeTematicoID IS NULL ");
            else
            {
                sCmd.Append(" WHERE EjeTematicoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = contenidoDigitalAgrupador.EjeTematico.EjeTematicoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigitalAgrupador.SituacionAprendizaje.SituacionAprendizajeID == null)
                sCmd.Append(" AND SituacionAprendizajeID IS NULL ");
            else
            {
                sCmd.Append(" AND SituacionAprendizajeID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = contenidoDigitalAgrupador.SituacionAprendizaje.SituacionAprendizajeID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigitalAgrupador.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
                sCmd.Append(" AND AgrupadorContenidoDigitalID IS NULL ");
            else
            {
                sCmd.Append(" AND AgrupadorContenidoDigitalID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = contenidoDigitalAgrupador.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigitalAgrupador.ContenidoDigital.ContenidoDigitalID == null)
                sCmd.Append(" AND ContenidoDigitalID IS NULL ");
            else
            {
                sCmd.Append(" AND ContenidoDigitalID = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = contenidoDigitalAgrupador.ContenidoDigital.ContenidoDigitalID;
                sqlParam.DbType = DbType.Int64;
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
                throw new Exception("ContenidoDigitalAgrupadorDelHlp: Ocurrió un error al eliminar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ContenidoDigitalAgrupadorDelHlp: Ocurrió un error al eliminar el registro.");
        }
    }
}
