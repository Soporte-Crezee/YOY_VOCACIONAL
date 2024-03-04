using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// Consultar una AsignacionDocenteEscuela de la base de datos
    /// </summary>
    public class AsignacionDocenteEscuelaRetHlp
    {
        /// <summary>
        /// Consulta registros de AsignacionDocenteEscuela en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="asignacion">AsignacionDocenteEscuela que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AsignacionDocenteEscuela generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, AsignacionDocenteEscuela asignacion, Escuela escuela)
        {
            object myFirm = new object();
            string sError = "";
            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("AsignacionDocenteEscuelaRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (sError.Length > 0)
                throw new Exception("AsignacionDocenteEscuelaRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (asignacion == null)
                asignacion = new AsignacionDocenteEscuela();
            if (asignacion.Docente == null)
                asignacion.Docente = new Docente();
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AsignacionDocenteEscuelaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionDocenteEscuelaRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "AsignacionDocenteEscuelaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT DOCENTEESCUELAID,DOCENTEID,ESTATUS,FECHAREGISTRO, FECHABAJA, ESCUELAID ");
            sCmd.Append(" FROM DOCENTEESCUELA ");
            if (escuela.EscuelaID == null)
                sCmd.Append(" WHERE ESCUELAID IS NULL ");
            else
            {
                // escuela.EscuelaID
                sCmd.Append(" WHERE ESCUELAID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.AsignacionDocenteEscuelaID != null)
            {
                sCmd.Append(" AND DOCENTEESCUELAID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = asignacion.AsignacionDocenteEscuelaID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.Docente.DocenteID != null)
            {
                sCmd.Append(" AND DOCENTEID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = asignacion.Docente.DocenteID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.Activo != null)
            {
                sCmd.Append(" AND ESTATUS = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = asignacion.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.FechaRegistro != null)
            {
                sCmd.Append(" AND FECHAREGISTRO = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = asignacion.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.FechaBaja != null)
            {
                sCmd.Append(" AND FECHABAJA = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = asignacion.FechaBaja;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "AsignacionDocenteEscuela");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AsignacionDocenteEscuelaRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
