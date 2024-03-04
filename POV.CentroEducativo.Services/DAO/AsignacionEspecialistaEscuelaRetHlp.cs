using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace POV.CentroEducativo.DAO
{
    public class AsignacionEspecialistaEscuelaRetHlp
    {
        public DataSet Action(IDataContext dctx, AsignacionEspecialistaEscuela asignacion, Escuela escuela)
        {
            object myFirm = new object();
            string sError = "";
            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("AsignacionEspecialistaEscuelaRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (sError.Length > 0)
                throw new Exception("AsignacionEspecialistaEscuelaRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (asignacion == null)
                asignacion = new AsignacionEspecialistaEscuela();
            if (asignacion.Especialista == null)
                asignacion.Especialista = new EspecialistaPruebas();
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AsignacionEspecialistaEscuelaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionDocenteEscuelaRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "AsignacionEspecialistaEscuelaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ESPECIALISTAESCUELAID,ESPECIALISTAID,ESTATUS,FECHAREGISTRO, FECHABAJA, ESCUELAID ");
            sCmd.Append(" FROM ESPECIALISTAESCUELA ");
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
            if (asignacion.AsignacionEspecialistaEscuelaID != null)
            {
                sCmd.Append(" AND ESPECIALISTAESCUELAID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = asignacion.AsignacionEspecialistaEscuelaID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.Especialista.EspecialistaPruebaID != null)
            {
                sCmd.Append(" AND ESPECIALISTAID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = asignacion.Especialista.EspecialistaPruebaID;
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
                sqlAdapter.Fill(ds, "AsignacionEspecialistaEscuela");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AsignacionEspecialistaEscuelaRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
