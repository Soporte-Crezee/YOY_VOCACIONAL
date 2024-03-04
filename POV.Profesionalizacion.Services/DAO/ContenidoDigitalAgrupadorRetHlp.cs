using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;
using POV.ContenidosDigital.BO;

namespace POV.Profesionalizacion.DAO
{
    /// <summary>
    /// Consulta un registro de ContenidoDigitalAgrupador en la BD
    /// </summary>
    internal class ContenidoDigitalAgrupadorRetHlp
    {
        /// <summary>
        /// Consulta registros de ContenidoDigitalAgrupador en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="contenidoDigitalAgrupador">ContenidoDigitalAgrupador que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ContenidoDigitalAgrupador generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (contenidoDigitalAgrupador == null)
                sError += ", ContenidoDigitalAgrupador";
            if (sError.Length > 0)
                throw new Exception("Nombre: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (contenidoDigitalAgrupador.EjeTematico == null)
            {
                contenidoDigitalAgrupador.EjeTematico = new EjeTematico();
            }
            if (contenidoDigitalAgrupador.SituacionAprendizaje == null)
            {
                contenidoDigitalAgrupador.SituacionAprendizaje = new SituacionAprendizaje();
            }
            if (contenidoDigitalAgrupador.ContenidoDigital == null)
            {
                contenidoDigitalAgrupador.ContenidoDigital = new ContenidoDigital();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                   "ContenidoDigitalAgrupadorRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "Nombre: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO",
                   "ContenidoDigitalAgrupadorRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT cda.ContenidoDigitalAgrupadorID, cda.EjeTematicoID, cda.SituacionAprendizajeID, cda.AgrupadorContenidoDigitalID, cda.ContenidoDigitalID, cda.Activo, cda.FechaRegistro, acd.TipoAgrupador ");
            sCmd.Append(" FROM ContenidoDigitalAgrupador cda ");
            sCmd.Append(" INNER JOIN AgrupadorContenidoDigital acd ON cda.AgrupadorContenidoDigitalID = acd.AgrupadorContenidoDigitalID ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID != null)
            {
                s_VarWHERE.Append(" cda.ContenidoDigitalAgrupadorID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigitalAgrupador.EjeTematico.EjeTematicoID != null)
            {
                s_VarWHERE.Append(" AND cda.EjeTematicoID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = contenidoDigitalAgrupador.EjeTematico.EjeTematicoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigitalAgrupador.SituacionAprendizaje.SituacionAprendizajeID != null)
            {
                s_VarWHERE.Append(" AND cda.SituacionAprendizajeID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = contenidoDigitalAgrupador.SituacionAprendizaje.SituacionAprendizajeID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigitalAgrupador.AgrupadorContenidoDigital != null && contenidoDigitalAgrupador.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID != null)
            {
                s_VarWHERE.Append(" AND cda.AgrupadorContenidoDigitalID = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = contenidoDigitalAgrupador.AgrupadorContenidoDigital.AgrupadorContenidoDigitalID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigitalAgrupador.ContenidoDigital.ContenidoDigitalID != null)
            {
                s_VarWHERE.Append(" AND cda.ContenidoDigitalID = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = contenidoDigitalAgrupador.ContenidoDigital.ContenidoDigitalID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigitalAgrupador.Activo != null)
            {
                s_VarWHERE.Append(" AND cda.Activo = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = contenidoDigitalAgrupador.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (contenidoDigitalAgrupador.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND cda.FechaRegistro = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = contenidoDigitalAgrupador.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
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
                sqlAdapter.Fill(ds, "ContenidoDigitalAgrupador");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("Nombre: Se encontraron problemas al recuperar los datos. " + exmsg);
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
