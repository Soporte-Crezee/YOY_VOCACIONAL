using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace POV.CentroEducativo.DAO
{

    public class AsignacionEspecialistaEscuelaUpdHlp
    {
        public void Action(IDataContext dctx, AsignacionEspecialistaEscuela asignacion, AsignacionEspecialistaEscuela anterior, Escuela escuela)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (asignacion == null)
                sError += ", AsignacionEspecialistaEscuela";
            if (anterior == null)
                sError += ", Anterior";
            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("AsignacionEspecialistaEscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (asignacion.AsignacionEspecialistaEscuelaID == null)
                sError += ", AsignacionEspecialistaEscuelaID";
            if (asignacion.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (asignacion.Activo == null)
                sError += ", Activo";
            if (asignacion.Especialista == null)
                sError += ", Docente";
            if (anterior.AsignacionEspecialistaEscuelaID == null)
                sError += ", AsignacionEspecialistaEscuelaID anterior";
            if (anterior.FechaRegistro == null)
                sError += ", FechaRegistro anterior";
            if (anterior.Activo == null)
                sError += ", Activo anterior";
            if (anterior.Especialista == null)
                sError += ", Docente anterior";
            if (sError.Length > 0)
                throw new Exception("AsignacionEspecialistaEscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (asignacion.Especialista.EspecialistaPruebaID == null)
                sError += ", Especialista.EspecialistaPruebaID";
            if (anterior.Especialista.EspecialistaPruebaID == null)
                sError += ", Especialista.EspecialistaPruebaID anterior";
            if (sError.Length > 0)
                throw new Exception("AsignacionEspecialistaEscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (asignacion.AsignacionEspecialistaEscuelaID != anterior.AsignacionEspecialistaEscuelaID)
            {
                sError = "Los parametros no coinciden";
            }
            if (sError.Length > 0)
                throw new Exception("AsignacionEspecialistaEscuelaUpdHlp: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AsignacionEspecialistaEscuelaUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionDocenteEscuelaUpdHlp: Ocurrió un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "AsignacionEspecialistaEscuelaUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE ESPECIALISTAESCUELA ");
            // asignacion.Activo
            sCmd.Append(" SET Estatus =@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (asignacion.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = asignacion.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // asignacion.FechaBaja
            sCmd.Append(" ,FechaBaja =@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (asignacion.FechaBaja == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = asignacion.FechaBaja;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            // anterior.AsignacionEspecialistaEscuelaID
            if (anterior.AsignacionEspecialistaEscuelaID == null)
                sCmd.Append(" WHERE EspecialistaEscuelaID IS NULL ");
            else
            {
                sCmd.Append(" WHERE EspecialistaEscuelaID =@dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = anterior.AsignacionEspecialistaEscuelaID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // escuela.EscuelaID
            if (escuela.EscuelaID == null)
                sCmd.Append(" AND EscuelaID IS NULL ");
            else
            {
                sCmd.Append(" AND EscuelaID =@dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // anterior.Docente.DocenteID
            if (anterior.Especialista.EspecialistaPruebaID == null)
                sCmd.Append(" AND EspecialistaID IS NULL ");
            else
            {
                sCmd.Append(" AND EspecialistaID =@dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = anterior.Especialista.EspecialistaPruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // anterior.Estatus
            if (anterior.Activo == null)
                sCmd.Append(" AND Estatus IS NULL ");
            else
            {
                sCmd.Append(" AND Estatus =@dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = anterior.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // anterior.FechaRegistro
            if (anterior.FechaRegistro == null)
                sCmd.Append(" AND FechaRegistro IS NULL ");
            else
            {
                sCmd.Append(" AND FechaRegistro =@dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = anterior.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // anterior.FechaBaja
            if (anterior.FechaBaja == null)
                sCmd.Append(" AND FechaBaja IS NULL ");
            else
            {
                sCmd.Append(" AND FechaBaja =@dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = anterior.FechaBaja;
                sqlParam.DbType = DbType.DateTime;
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
                throw new Exception("AsignacionEspecialistaEscuelaUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("AsignacionEspecialistaEscuelaUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado.");
        }
    }
}
