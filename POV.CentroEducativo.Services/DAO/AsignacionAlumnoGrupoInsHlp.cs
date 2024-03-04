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
    /// Inserta una AsignacionAlumnoGrupo en la base de datos
    /// </summary>
    public class AsignacionAlumnoGrupoInsHlp
    {
        /// <summary>
        /// Crea un registro de AsignacionAlumnoGrupo en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="asignacion">AsignacionAlumnoGrupo que desea crear</param>
        public void Action(IDataContext dctx, AsignacionAlumnoGrupo asignacion, GrupoCicloEscolar grupoCicloEscolar)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (asignacion == null)
                sError += ", AsignacionAlumnoGrupo";
            if (grupoCicloEscolar == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
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
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (asignacion.Alumno.AlumnoID == null)
                sError += ", Docente.DocenteID";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AsignacionAlumnoGrupoInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionAlumnoGrupoInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", "AsignacionAlumnoGrupoInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO ASIGNACIONALUMNOGRUPO (ASIGNACIONALUMNOGRUPOID,GRUPOCICLOESCOLARID,ALUMNOID,ACTIVO,FECHAREGISTRO) ");
            // asignacion.AsignacionAlumnoGrupoID
            sCmd.Append(" VALUES (@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (asignacion.AsignacionAlumnoGrupoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = asignacion.AsignacionAlumnoGrupoID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // grupoCicloEscolar.GrupoCicloEscolarID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupoCicloEscolar.GrupoCicloEscolarID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            // asignacion.Alumno.AlumnoID
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (asignacion.Alumno.AlumnoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = asignacion.Alumno.AlumnoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            // asignacion.Activo
            sCmd.Append(" ,@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            if (asignacion.Activo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = asignacion.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            // asignacion.FechaRegistro
            sCmd.Append(" ,@dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            if (asignacion.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = asignacion.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
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
                throw new Exception("AsignacionAlumnoGrupoInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("AsignacionAlumnoGrupoInsHlp: Ocurrio un error al ingresar el registro.");
        }
    }
}
