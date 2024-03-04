using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// Consultar una AsignacionAlumnoGrupo de la base de datos
    /// </summary>
    public class AsignacionAlumnoGrupoRetHlp
    {
        /// <summary>
        /// Consulta registros de AsignacionAlumnoGrupo en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="asignacion">AsignacionAlumnoGrupo que provee el criterio de selección para realizar la consulta</param>
        /// <param name="grupoCicloEscolar">GrupoCicloEscolar  que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de AsignacionAlumnoGrupo generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, AsignacionAlumnoGrupo asignacion, GrupoCicloEscolar grupoCicloEscolar)
        {
            object myFirm = new object();
            string sError = "";
            if (grupoCicloEscolar == null)
                sError += ", GrupoCicloEscolar";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                sError += ", GrupoCicloEscolarID";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (asignacion == null)
                asignacion = new AsignacionAlumnoGrupo();
            if (asignacion.Alumno == null)
                asignacion.Alumno = new Alumno();
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AsignacionAlumnoGrupoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionAlumnoGrupoRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "AsignacionAlumnoGrupoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ASIGNACIONALUMNOGRUPOID, ALUMNOID, ACTIVO, FECHAREGISTRO, FECHABAJA, GRUPOCICLOESCOLARID ");
            sCmd.Append(" FROM ASIGNACIONALUMNOGRUPO ");
            if (grupoCicloEscolar.GrupoCicloEscolarID == null)
                sCmd.Append(" WHERE GRUPOCICLOESCOLARID IS NULL ");
            else
            {
                // grupoCicloEscolar.GrupoCicloEscolarID
                sCmd.Append(" WHERE GRUPOCICLOESCOLARID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = grupoCicloEscolar.GrupoCicloEscolarID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.AsignacionAlumnoGrupoID != null)
            {
                sCmd.Append(" AND ASIGNACIONALUMNOGRUPOID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = asignacion.AsignacionAlumnoGrupoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.Alumno.AlumnoID != null)
            {
                sCmd.Append(" AND ALUMNOID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = asignacion.Alumno.AlumnoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.Activo != null)
            {
                sCmd.Append(" AND ACTIVO = @dbp4ram4 ");
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
                sqlAdapter.Fill(ds, "AsignacionAlumnoGrupo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AsignacionAlumnoGrupoRetHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }

        public DataSet ActionAsignacionAlumno(IDataContext dctx, AsignacionAlumnoGrupo asignacion, CicloEscolar cicloEscolar)
        {
            object myFirm = new object();
            string sError = "";
            if (asignacion == null)
                sError += ", AsignacionAlumnoGrupo";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (asignacion.Alumno == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (asignacion.Alumno.AlumnoID == null)
                sError += ", Alumno.AlumnoID";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AsignacionAlumnoGrupoRetHlp", "ActionAsignacionAlumno", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionAlumnoGrupoRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "AsignacionAlumnoGrupoRetHlp", "ActionAsignacionAlumno", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ag.ASIGNACIONALUMNOGRUPOID, ag.ALUMNOID, ag.ACTIVO, ag.FECHAREGISTRO, ag.FECHABAJA, ag.GRUPOCICLOESCOLARID ");
            sCmd.Append(" FROM ASIGNACIONALUMNOGRUPO ag ");
            sCmd.Append(" INNER JOIN GRUPOCICLOESCOLAR gce ON (gce.GRUPOCICLOESCOLARID=ag.GRUPOCICLOESCOLARID) ");
            if (asignacion.Alumno.AlumnoID == null)
                sCmd.Append(" WHERE ALUMNOID IS NULL ");
            else
            {
                // grupoCicloEscolar.GrupoCicloEscolarID
                sCmd.Append(" WHERE ag.ALUMNOID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = asignacion.Alumno.AlumnoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (asignacion.Activo != null)
            {
                sCmd.Append(" AND ag.ACTIVO = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = asignacion.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (cicloEscolar.CicloEscolarID != null)
            {
                sCmd.Append(" AND gce.CicloEscolarID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = cicloEscolar.CicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "AsignacionAlumnoGrupo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AsignacionAlumnoGrupoRetHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="grupoCicloEscolar"></param>
        /// <param name="alumno"></param>
        /// <returns></returns>
        public DataSet ActionAsignacionAlumnoByGrupoAlumno(IDataContext dctx, GrupoCicloEscolar grupoCicloEscolar, Alumno alumno, AreaConocimiento areaConocimiento, Docente docente = null, Usuario usuario = null)
        {
            object myFirm = new object();
            string sError = "";
            if (grupoCicloEscolar == null)
                sError += ", GrupoCicloEscolar";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (alumno == null)
                sError += ", Alumno";
            if (sError.Length > 0)
                throw new Exception("AsignacionAlumnoGrupoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
            if (grupoCicloEscolar.Grupo == null)
                grupoCicloEscolar.Grupo = new Grupo();

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "AsignacionAlumnoGrupoRetHlp", "ActionAsignacionAlumno", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AsignacionAlumnoGrupoRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "AsignacionAlumnoGrupoRetHlp", "ActionAsignacionAlumno", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT DISTINCT aag.AsignacionAlumnoGrupoID,aag.Activo, gce.GrupoCicloEscolarID, gce.Clave, gce.EscuelaID, gce.CicloEscolarID,  ");
            sCmd.Append(" g.GrupoID, g.Grado, g.Nombre AS NombreGrupo, ");
            sCmd.Append(" RTRIM(al.Nombre + ' ' + al.PrimerApellido + ' ' + al.SegundoApellido) as NombreCompletoAlumno, ");
            sCmd.Append(" al.AlumnoID, al.DocenteID, al.Nombre, al.PrimerApellido, al.SegundoApellido, al.Curp, ");
            sCmd.Append(" al.Direccion, al.Estatus, al.EstatusIdentificacion, al.FechaNacimiento, ");
            sCmd.Append(" al.FechaRegistro, al.Matricula, al.NombreCompletoTutor, al.NombreCompletoTutorDos, ubac.AreaConocimientoID ");
            sCmd.Append(" FROM AsignacionAlumnoGrupo aag ");
            sCmd.Append(" INNER JOIN GrupoCicloEscolar gce ON gce.GrupoCicloEscolarID = aag.GrupoCicloEscolarID ");
            sCmd.Append(" INNER JOIN Grupo g ON g.GrupoID = gce.GrupoID ");
            sCmd.Append(" INNER JOIN Alumno al ON al.AlumnoID = aag.AlumnoID ");
            sCmd.Append(" INNER JOIN ViewUsuarioByAreaConocimiento ubac ON ubac.AlumnoID = al.AlumnoID ");
            StringBuilder s_VarWHERE = new StringBuilder();

            #region filtros grupocicloescolar
            if (grupoCicloEscolar.GrupoCicloEscolarID != null)
            {
                s_VarWHERE.Append(" gce.GrupoCicloEscolarID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = grupoCicloEscolar.GrupoCicloEscolarID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.Escuela != null && grupoCicloEscolar.Escuela.EscuelaID != null)
            {
                s_VarWHERE.Append(" AND gce.EscuelaID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = grupoCicloEscolar.Escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.CicloEscolar != null && grupoCicloEscolar.CicloEscolar.CicloEscolarID != null)
            {
                s_VarWHERE.Append(" AND gce.CicloEscolarID = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = grupoCicloEscolar.CicloEscolar.CicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.Grupo != null && grupoCicloEscolar.Grupo.GrupoID != null)
            {
                s_VarWHERE.Append(" AND g.GrupoID = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = grupoCicloEscolar.Grupo.GrupoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.Grupo != null && grupoCicloEscolar.Grupo.Grado != null)
            {
                s_VarWHERE.Append(" AND g.Grado = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = grupoCicloEscolar.Grupo.Grado;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoCicloEscolar.Grupo != null && grupoCicloEscolar.Grupo.Nombre != null)
            {
                s_VarWHERE.Append(" AND g.Nombre = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = grupoCicloEscolar.Grupo.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            #endregion

            #region filtros alumnos
            if (alumno.AlumnoID != null)
            {
                s_VarWHERE.Append(" AND al.AlumnoID = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = alumno.AlumnoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (!string.IsNullOrEmpty(alumno.Nombre))
            {
                s_VarWHERE.Append(" AND (RTRIM(al.Nombre + ' ' + al.PrimerApellido + ' ' + al.SegundoApellido) LIKE @dbp4ram9) ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = alumno.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (!string.IsNullOrEmpty(alumno.Curp))
            {
                s_VarWHERE.Append(" AND al.Curp = @dbp4ram10 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram10";
                sqlParam.Value = alumno.Curp;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.Estatus != null)
            {
                s_VarWHERE.Append(" AND al.Estatus = @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = alumno.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (alumno.FechaNacimiento != null)
            {
                s_VarWHERE.Append(" AND al.FechaNacimiento = @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = alumno.FechaNacimiento;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (areaConocimiento.AreaConocimentoID != null)
            {
                s_VarWHERE.Append(" AND ubac.AreaConocimientoID = @dbp4ram13 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram13";
                sqlParam.Value = areaConocimiento.AreaConocimentoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }

            #endregion

            #region filtros Docente
            if (docente != null)
            {
                if (docente.DocenteID != null)
                {
                    s_VarWHERE.Append(" and ubac.docenteid = @dbp4ram14 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram14";
                    sqlParam.Value = docente.DocenteID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            #endregion
            #region filtros Usuario
            if (usuario != null)
            {
                if (usuario.UniversidadId == null)
                    s_VarWHERE.Append(" AND ubac.UniversidadId is null ");
                else
                {
                    s_VarWHERE.Append(" AND ubac.UniversidadId = @dbp4ram15 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram15";
                    sqlParam.Value = usuario.UniversidadId;
                    sqlParam.DbType = DbType.Int64;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            #endregion

            s_VarWHERE.Append(" AND aag.Activo = 1 ");

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

            sCmd.Append(" ORDER BY g.Grado, g.Nombre , al.Nombre ASC");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "AsignacionAlumnoGrupo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AsignacionAlumnoGrupoRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
