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
    /// Actualiza una asignación de alumno en la base de datos
    /// </summary>
    public class AsignacionAlumnoGrupoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de AsignacionAlumnoGrupo en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="asignacion">AsignacionAlumnoGrupo que tiene los datos nuevos</param>
        /// <param name="anterior">AsignacionAlumnoGrupo que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, AsignacionAlumnoGrupo asignacion, AsignacionAlumnoGrupo anterior, GrupoCicloEscolar grupoCicloEscolar)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (asignacion == null)
                sError += ", AsignacionAlumnoGrupo";
            if (anterior == null)
                sError += ", Anterior";
            if (grupoCicloEscolar == null)
                sError += ", GrupoCicloEscolar";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                sError += ", GrupoCicloEscolarID";
            if (asignacion.AsignacionAlumnoGrupoID == null)
                sError += ", AsignacionAlumnoGrupoID";
            if (asignacion.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (asignacion.Activo == null)
                sError += ", Activo";
            if (asignacion.Alumno == null)
                sError += ", Alumno";
            if (anterior.AsignacionAlumnoGrupoID == null)
                sError += ", AsignacionAlumnoGrupoID anterior";
            if (anterior.FechaRegistro == null)
                sError += ", FechaRegistro anterior";
            if (anterior.Activo == null)
                sError += ", Activo anterior";
            if (anterior.Alumno == null)
                sError += ", Alumno anterior";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (asignacion.Alumno.AlumnoID == null)
                sError += ", Alumno.AlumnoID";
            if (anterior.Alumno.AlumnoID == null)
                sError += ", Alumno.AlumnoID anterior";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (asignacion.AsignacionAlumnoGrupoID != anterior.AsignacionAlumnoGrupoID)
            {
                sError = "Los parametros no coinciden";
            }
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoUpdHlp: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AsignacionAlumnoGrupoUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionAlumnoGrupoUpdHlp: Ocurrió un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "AsignacionAlumnoGrupoUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE ASIGNACIONALUMNOGRUPO ");
            // asignacion.Activo
            sCmd.Append(" SET Activo =@dbp4ram1 ");
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
            // anterior.AsignacionAlumnoGrupoID
            if (anterior.AsignacionAlumnoGrupoID == null)
                sCmd.Append(" WHERE AsignacionAlumnoGrupoID IS NULL ");
            else
            {
                sCmd.Append(" WHERE AsignacionAlumnoGrupoID =@dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = anterior.AsignacionAlumnoGrupoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // grupoCicloEscolar.GrupoCicloEscolarID
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                sCmd.Append(" AND GrupoCicloEscolarID IS NULL ");
            else
            {
                sCmd.Append(" AND GrupoCicloEscolarID =@dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = grupoCicloEscolar.GrupoCicloEscolarID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // anterior.Alumno.AlumnoID
            if (anterior.Alumno.AlumnoID == null)
                sCmd.Append(" AND AlumnoID IS NULL ");
            else
            {
                sCmd.Append(" AND AlumnoID =@dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = anterior.Alumno.AlumnoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            // anterior.Activo
            if (anterior.Activo == null)
                sCmd.Append(" AND Activo IS NULL ");
            else
            {
                sCmd.Append(" AND Activo =@dbp4ram6 ");
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
                throw new Exception("AsignacionAlumnoGrupoUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("AsignacionAlumnoGrupoUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado.");
        }

        /// <summary>
        /// Actualiza de manera optimista un registro de AsignacionAlumnoGrupo en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="asignacion">AsignacionAlumnoGrupo que tiene los datos nuevos</param>
        /// <param name="anterior">AsignacionAlumnoGrupo que tiene los datos anteriores</param>
        public void ActionDesactivarAlumno(IDataContext dctx, AsignacionAlumnoGrupo asignacion, GrupoCicloEscolar grupoCicloEscolar)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (asignacion == null)
                sError += ", AsignacionAlumnoGrupo";
            if (grupoCicloEscolar == null)
                sError += ", GrupoCicloEscolar";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                sError += ", GrupoCicloEscolarID";
            if (grupoCicloEscolar.CicloEscolar == null)
                sError += ", GrupoCicloEscolar.CicloEscolar";
            if (asignacion.AsignacionAlumnoGrupoID == null)
                sError += ", AsignacionAlumnoGrupoID";
            if (asignacion.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (asignacion.Activo == null)
                sError += ", Activo";
            if (asignacion.Alumno == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (asignacion.Alumno.AlumnoID == null)
                sError += ", Alumno.AlumnoID";
            if (grupoCicloEscolar.CicloEscolar.CicloEscolarID == null)
                sError += ", CicloEscolar.CicloEscolarID";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AsignacionAlumnoGrupoUpdHlp", "ActionDesactivarAlumno", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionAlumnoGrupoUpdHlp: Ocurrió un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "AsignacionAlumnoGrupoUpdHlp", "ActionDesactivarAlumno", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE ASIGNACIONALUMNOGRUPO ");
            // asignacion.Activo
            sCmd.Append(" SET Activo =@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = false;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // asignacion.FechaBaja
            sCmd.Append(" ,FechaBaja =@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = DateTime.Now;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" WHERE GrupoCicloEscolarID IN (SELECT GrupoCicloEscolarID FROM GrupoCicloEscolar WHERE CicloEscolarID = @dbp4ram3) ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = grupoCicloEscolar.CicloEscolar.CicloEscolarID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" AND AlumnoID =@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = asignacion.Alumno.AlumnoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);


            sCmd.Append(" AND GrupoCicloEscolarID <> @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = grupoCicloEscolar.GrupoCicloEscolarID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);

            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AsignacionAlumnoGrupoUpdHlp: Ocurrio un error al actualizar el objeto o fue modificado mientras era editado. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
        }
    }
}
