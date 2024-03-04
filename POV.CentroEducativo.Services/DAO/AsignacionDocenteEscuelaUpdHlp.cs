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
    /// Actualiza una asignación de docente en la base de datos
    /// </summary>
    public class AsignacionDocenteEscuelaUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de AsignacionDocenteEscuela en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="asignacion">AsignacionDocenteEscuela que tiene los datos nuevos</param>
        /// <param name="anterior">AsignacionDocenteEscuela que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, AsignacionDocenteEscuela asignacion, AsignacionDocenteEscuela anterior, Escuela escuela)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (asignacion == null)
                sError += ", AsignacionDocenteEscuela";
            if (anterior == null)
                sError += ", Anterior";
            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("AsignacionDocenteEscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (asignacion.AsignacionDocenteEscuelaID == null)
                sError += ", AsignacionDocenteEscuelaID";
            if (asignacion.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (asignacion.Activo == null)
                sError += ", Activo";
            if (asignacion.Docente == null)
                sError += ", Docente";
            if (anterior.AsignacionDocenteEscuelaID == null)
                sError += ", AsignacionDocenteEscuelaID anterior";
            if (anterior.FechaRegistro == null)
                sError += ", FechaRegistro anterior";
            if (anterior.Activo == null)
                sError += ", Activo anterior";
            if (anterior.Docente == null)
                sError += ", Docente anterior";
            if (sError.Length > 0)
                throw new Exception("AsignacionDocenteEscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (asignacion.Docente.DocenteID == null)
                sError += ", Docente.DocenteID";
            if (anterior.Docente.DocenteID == null)
                sError += ", Docente.DocenteID anterior";
            if (sError.Length > 0)
                throw new Exception("AsignacionDocenteEscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (asignacion.AsignacionDocenteEscuelaID != anterior.AsignacionDocenteEscuelaID)
            {
                sError = "Los parametros no coinciden";
            }
            if (sError.Length > 0)
                throw new Exception("AsignacionDocenteEscuelaUpdHlp: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AsignacionDocenteEscuelaUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionDocenteEscuelaUpdHlp: Ocurrió un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "AsignacionDocenteEscuelaUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE DOCENTEESCUELA ");
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
            // anterior.AsignacionDocenteEscuelaID
            if (anterior.AsignacionDocenteEscuelaID == null)
                sCmd.Append(" WHERE DocenteEscuelaID IS NULL ");
            else
            {
                sCmd.Append(" WHERE DocenteEscuelaID =@dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = anterior.AsignacionDocenteEscuelaID;
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
            if (anterior.Docente.DocenteID == null)
                sCmd.Append(" AND DocenteID IS NULL ");
            else
            {
                sCmd.Append(" AND DocenteID =@dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = anterior.Docente.DocenteID;
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
                throw new Exception("AsignacionDocenteEscuelaUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("AsignacionDocenteEscuelaUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado.");
        }
    }
}
