// Licencias de Docente
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;

using POV.CentroEducativo.BO;
using POV.Seguridad.BO;

namespace POV.CentroEducativo.DA
{
    /// <summary>
    /// AlumnosAsignadosDocentePortal
    /// </summary>
    public class AlumnosAsignadosDocentePortal
    {
        public DataSet Action(IDataContext dctx, Docente docente = null, Usuario usuario = null)
        {
            object myFirm = new object();
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DA", "AlumnosAsignadosDocentePortal", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "AlumnosAsignadosDocentePortal: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DA", "UsuarioDocente", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" select d2.DocenteID, d2.Nombre, d2.PrimerApellido, d2.SegundoApellido, ");
            sCmd.Append(" d2.FechaNacimiento, d2.Estatus, d2.FechaRegistro, d2.Curp, ");
            sCmd.Append(" d2.Sexo, d2.Correo, d2.Clave, d2.EstatusIdentificacion, ");
            sCmd.Append(" d2.Cedula, d2.NivelEstudio, d2.Titulo, d2.Especialidades, ");
            sCmd.Append(" d2.Experiencia, d2.Cursos, d2.UsuarioSkyPe, d2.EsPremium, ");
            sCmd.Append(" d2.PermiteAsignaciones, u2.UniversidadID, u2.UsuarioID, count(ue2.UsuarioID) as Asignados ");
            sCmd.Append(" from Docente d2 ");
            sCmd.Append(" inner join licenciareferencia lr2 on d2.DocenteID = lr2.DocenteID ");
            sCmd.Append(" inner join licencia l2 on l2.LicenciaID = lr2.LicenciaID ");
            sCmd.Append(" inner join usuario u2 on u2.UsuarioID = l2.UsuarioID ");
            sCmd.Append(" left outer join UsuarioExpediente ue2 on l2.UsuarioID = ue2.UsuarioID ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (docente != null)
            {
                if (docente.DocenteID != null)
                {
                    s_VarWHERE.Append(" AND d2.DocenteID =@dbp4ram1 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram1";
                    sqlParam.Value = docente.DocenteID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (docente.Estatus != null)
                {
                    s_VarWHERE.Append(" AND d2.Estatus =@dbp4ram11 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram11";
                    sqlParam.Value = docente.Estatus;
                    sqlParam.DbType = DbType.Boolean;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (docente.EsPremium != null)
                {
                    s_VarWHERE.Append(" AND d2.Espremium =@dbp4ram12 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram12";
                    sqlParam.Value = docente.EsPremium;
                    sqlParam.DbType = DbType.Boolean;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }
            if (usuario != null)
            {
                if (usuario.UsuarioID != null)
                {
                    s_VarWHERE.Append(" AND u2.UsuarioID =@dbp4ram2 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram2";
                    sqlParam.Value = usuario.UsuarioID;
                    sqlParam.DbType = DbType.String;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                if (usuario.EsActivo != null)
                {
                    s_VarWHERE.Append(" AND u2.EsActivo =@dbp4ram21 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram21";
                    sqlParam.Value = usuario.UsuarioID;
                    sqlParam.DbType = DbType.Boolean;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }

            string s_VarWHEREres = "AND u2.UniversidadID IS NULL " + s_VarWHERE.ToString().Trim();
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
            sCmd.Append(" group by d2.DocenteID, d2.Nombre, d2.PrimerApellido, d2.SegundoApellido, ");
            sCmd.Append(" d2.FechaNacimiento, d2.Estatus, d2.FechaRegistro, d2.Curp, ");
            sCmd.Append(" d2.Sexo, d2.Correo, d2.Clave, d2.EstatusIdentificacion, ");
            sCmd.Append(" d2.Cedula, d2.NivelEstudio, d2.Titulo, d2.Especialidades, ");
            sCmd.Append(" d2.Experiencia, d2.Cursos, d2.UsuarioSkyPe, d2.EsPremium, ");
            sCmd.Append(" d2.PermiteAsignaciones, u2.UniversidadID, u2.UsuarioID ");
            sCmd.Append(" ORDER BY count(ue2.usuarioid) DESC");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Docente");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("AlumnosAsignadosDocentePortal: Hubo un error al consultar los registros. " + exmsg);
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
