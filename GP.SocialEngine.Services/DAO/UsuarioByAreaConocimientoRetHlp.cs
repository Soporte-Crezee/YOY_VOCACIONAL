using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using POV.CentroEducativo.BO;


namespace GP.SocialEngine.DAO
{
    /// <summary>
    /// Consulta un registro de UsuarioGrupo en la BD
    /// </summary>
    public class UsuarioByAreaConocimientoRetHlp
    {
        /// <summary>
        /// Consulta registros de UsuarioGrupo en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioGrupo">UsuarioGrupo que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de UsuarioGrupo generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, UsuarioGrupo usuarioGrupo, GrupoSocial grupoSocial, string areasConocimiento, long? UniverisdadID, bool tienepublicacion = false)
        {
            object myFirm = new object();
            if (usuarioGrupo == null)
            {
                usuarioGrupo = new UsuarioGrupo();
            }
            if (usuarioGrupo.UsuarioSocial == null)
            {
                usuarioGrupo.UsuarioSocial = new UsuarioSocial();
            }
            if (grupoSocial == null)
            {
                grupoSocial = new GrupoSocial();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "UsuarioByAreaConocimientoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UsuarioByAreaConocimientoRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO",
                   "UsuarioByAreaConocimientoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT distinct * from (");
            sCmd.Append(" SELECT distinct UsuarioGrupoID, FechaAsignacion, Estatus, GrupoSocialID, UsuarioSocialID, EsModerador, DocenteID, TienePublicacion, 'Hola mundo' as Texto ");
            sCmd.Append(" FROM ViewUsuarioByAreaConocimientodmf ");
            sCmd.Append(" WHERE ");
            string s_Varres = string.Empty;
            StringBuilder s_Var = new StringBuilder();
            if (usuarioGrupo.UsuarioGrupoID != null)
            {
                s_Var.Append(" UsuarioGrupoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = usuarioGrupo.UsuarioGrupoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.UsuarioSocial.UsuarioSocialID != null)
            {
                s_Var.Append(" AND UsuarioSocialID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = usuarioGrupo.UsuarioSocial.UsuarioSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoSocial.GrupoSocialID != null)
            {
                s_Var.Append(" AND GrupoSocialID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = grupoSocial.GrupoSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.FechaAsignacion != null)
            {
                s_Var.Append(" AND FechaAsignacion = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = usuarioGrupo.FechaAsignacion;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.Estatus != null)
            {
                s_Var.Append(" AND Estatus = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = usuarioGrupo.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.EsModerador != null)
            {
                s_Var.Append(" AND EsModerador = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = usuarioGrupo.EsModerador;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.DocenteID != null)
            {
                s_Var.Append(" AND DocenteID = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = usuarioGrupo.DocenteID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (areasConocimiento != "")
            {
                s_Var.Append(" AND AreaConocimientoID in (" + areasConocimiento + ") ");
            }
            else
                s_Var.Append(" AND AreaConocimientoID is not null");
            if (UniverisdadID == null)
                s_Var.Append(" AND UniversidadId is null ");
            else
            {
                s_Var.Append(" AND UniversidadId = @dbp4ram9");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = UniverisdadID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (tienepublicacion)
            {
                s_Var.Append(" AND TienePublicacion = @dbp4ram20 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram20";
                sqlParam.Value = tienepublicacion;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            s_Var.Append("  ");
            s_Varres = s_Var.ToString().Trim();
            if (s_Varres.Length > 0)
            {
                if (s_Varres.StartsWith("AND "))
                    s_Varres = s_Varres.Substring(4);
                else if (s_Varres.StartsWith("OR "))
                    s_Varres = s_Varres.Substring(3);
                else if (s_Varres.StartsWith(","))
                    s_Varres = s_Varres.Substring(1);
                sCmd.Append("  " + s_Varres);
            }

            sCmd.Append(" UNION ALL ");

            sCmd.Append(" SELECT distinct UsuarioGrupoID, FechaAsignacion, Estatus, GrupoSocialID, UsuarioSocialID, EsModerador, DocenteID, TienePublicacion, 'Hola mundo' as Texto ");
            sCmd.Append(" FROM ViewUsuarioByAreaConocimientodmf ");
            sCmd.Append(" WHERE AlumnoID is null AND ");
            s_Var = new StringBuilder();
            if (usuarioGrupo.UsuarioGrupoID != null)
            {
                s_Var.Append(" UsuarioGrupoID = @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = usuarioGrupo.UsuarioGrupoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.UsuarioSocial.UsuarioSocialID != null)
            {
                s_Var.Append(" AND UsuarioSocialID = @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = usuarioGrupo.UsuarioSocial.UsuarioSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoSocial.GrupoSocialID != null)
            {
                s_Var.Append(" AND GrupoSocialID = @dbp4ram13 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram13";
                sqlParam.Value = grupoSocial.GrupoSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.FechaAsignacion != null)
            {
                s_Var.Append(" AND FechaAsignacion = @dbp4ram14 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram14";
                sqlParam.Value = usuarioGrupo.FechaAsignacion;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.Estatus != null)
            {
                s_Var.Append(" AND Estatus = @dbp4ram15 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram15";
                sqlParam.Value = usuarioGrupo.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.EsModerador != null)
            {
                s_Var.Append(" AND EsModerador = @dbp4ram16 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram16";
                sqlParam.Value = usuarioGrupo.EsModerador;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.DocenteID != null)
            {
                s_Var.Append(" AND DocenteID = @dbp4ram17 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram17";
                sqlParam.Value = usuarioGrupo.DocenteID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (UniverisdadID == null)
                s_Var.Append(" AND UniversidadId is null ");
            else
            {
                s_Var.Append(" AND UniversidadId = @dbp4ram19");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram19";
                sqlParam.Value = UniverisdadID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (tienepublicacion)
            {
                s_Var.Append(" AND TienePublicacion = @dbp4ram21 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram21";
                sqlParam.Value = tienepublicacion;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }

            s_Var.Append("  ");
            s_Varres = s_Var.ToString().Trim();
            if (s_Varres.Length > 0)
            {
                if (s_Varres.StartsWith("AND "))
                    s_Varres = s_Varres.Substring(4);
                else if (s_Varres.StartsWith("OR "))
                    s_Varres = s_Varres.Substring(3);
                else if (s_Varres.StartsWith(","))
                    s_Varres = s_Varres.Substring(1);
                sCmd.Append("  " + s_Varres);
                sCmd.Append(" ) as tabla");
            }
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "UsuarioGrupo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UsuarioByAreaConocimientoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }

        public DataSet Action(IDataContext dctx, UsuarioGrupo usuarioGrupo, GrupoSocial grupoSocial, string areasConocimiento, long? UniverisdadID, int pagesize, int currentpage, bool tienepublicacion = false)
        {
            object myFirm = new object();
            if (usuarioGrupo == null)
            {
                usuarioGrupo = new UsuarioGrupo();
            }
            if (usuarioGrupo.UsuarioSocial == null)
            {
                usuarioGrupo.UsuarioSocial = new UsuarioSocial();
            }
            if (grupoSocial == null)
            {
                grupoSocial = new GrupoSocial();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "UsuarioByAreaConocimientoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;

            int pageindex = 0;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UsuarioByAreaConocimientoRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO",
                   "UsuarioByAreaConocimientoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            pageindex = currentpage - 1;
            StringBuilder sCmd = new StringBuilder();
            // Para paginado
            sCmd.Append(" SELECT * from (");
            sCmd.Append(" SELECT distinct *, rownumber = ROW_NUMBER() OVER (order by UsuarioSocialID) from (");
            sCmd.Append(" SELECT distinct  *, ROW_NUMBER() OVER (partition by usuariosocialid order by UsuarioSocialID) as yoy from (");
            sCmd.Append(" SELECT distinct UsuarioGrupoID, FechaAsignacion, Estatus, GrupoSocialID, UsuarioSocialID, EsModerador, DocenteID, TienePublicacion, 'Hola mundo' as Texto ");
            sCmd.Append(" FROM ViewUsuarioByAreaConocimientodmf ");
            sCmd.Append(" WHERE ");
            string s_Varres = string.Empty;
            StringBuilder s_Var = new StringBuilder();
            if (usuarioGrupo.UsuarioGrupoID != null)
            {
                s_Var.Append(" UsuarioGrupoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = usuarioGrupo.UsuarioGrupoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.UsuarioSocial.UsuarioSocialID != null)
            {
                s_Var.Append(" AND UsuarioSocialID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = usuarioGrupo.UsuarioSocial.UsuarioSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoSocial.GrupoSocialID != null)
            {
                s_Var.Append(" AND GrupoSocialID = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = grupoSocial.GrupoSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.FechaAsignacion != null)
            {
                s_Var.Append(" AND FechaAsignacion = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = usuarioGrupo.FechaAsignacion;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.Estatus != null)
            {
                s_Var.Append(" AND Estatus = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = usuarioGrupo.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.EsModerador != null)
            {
                s_Var.Append(" AND EsModerador = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = usuarioGrupo.EsModerador;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.DocenteID != null)
            {
                s_Var.Append(" AND DocenteID = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = usuarioGrupo.DocenteID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (areasConocimiento != "")
            {
                s_Var.Append(" AND AreaConocimientoID in (" + areasConocimiento + ") ");
            }
            else
                s_Var.Append(" AND AreaConocimientoID is not null");
            if (UniverisdadID == null)
                s_Var.Append(" AND UniversidadId is null ");
            else
            {
                s_Var.Append(" AND UniversidadId = @dbp4ram9");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = UniverisdadID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (tienepublicacion)
            {
                s_Var.Append(" AND TienePublicacion = @dbp4ram20 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram20";
                sqlParam.Value = tienepublicacion;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            s_Var.Append("  ");
            s_Varres = s_Var.ToString().Trim();
            if (s_Varres.Length > 0)
            {
                if (s_Varres.StartsWith("AND "))
                    s_Varres = s_Varres.Substring(4);
                else if (s_Varres.StartsWith("OR "))
                    s_Varres = s_Varres.Substring(3);
                else if (s_Varres.StartsWith(","))
                    s_Varres = s_Varres.Substring(1);
                sCmd.Append("  " + s_Varres);
            }

            sCmd.Append(" UNION ALL ");

            sCmd.Append(" SELECT distinct UsuarioGrupoID, FechaAsignacion, Estatus, GrupoSocialID, UsuarioSocialID, EsModerador, DocenteID, TienePublicacion, 'Hola mundo' as Texto ");
            sCmd.Append(" FROM ViewUsuarioByAreaConocimientodmf ");
            sCmd.Append(" WHERE AlumnoID is null AND ");
            s_Var = new StringBuilder();
            if (usuarioGrupo.UsuarioGrupoID != null)
            {
                s_Var.Append(" UsuarioGrupoID = @dbp4ram11 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram11";
                sqlParam.Value = usuarioGrupo.UsuarioGrupoID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.UsuarioSocial.UsuarioSocialID != null)
            {
                s_Var.Append(" AND UsuarioSocialID = @dbp4ram12 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram12";
                sqlParam.Value = usuarioGrupo.UsuarioSocial.UsuarioSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (grupoSocial.GrupoSocialID != null)
            {
                s_Var.Append(" AND GrupoSocialID = @dbp4ram13 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram13";
                sqlParam.Value = grupoSocial.GrupoSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.FechaAsignacion != null)
            {
                s_Var.Append(" AND FechaAsignacion = @dbp4ram14 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram14";
                sqlParam.Value = usuarioGrupo.FechaAsignacion;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.Estatus != null)
            {
                s_Var.Append(" AND Estatus = @dbp4ram15 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram15";
                sqlParam.Value = usuarioGrupo.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.EsModerador != null)
            {
                s_Var.Append(" AND EsModerador = @dbp4ram16 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram16";
                sqlParam.Value = usuarioGrupo.EsModerador;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioGrupo.DocenteID != null)
            {
                s_Var.Append(" AND DocenteID = @dbp4ram17 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram17";
                sqlParam.Value = usuarioGrupo.DocenteID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (UniverisdadID == null)
                s_Var.Append(" AND UniversidadId is null ");
            else
            {
                s_Var.Append(" AND UniversidadId = @dbp4ram19");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram19";
                sqlParam.Value = UniverisdadID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (tienepublicacion)
            {
                s_Var.Append(" AND TienePublicacion = @dbp4ram21 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram21";
                sqlParam.Value = tienepublicacion;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }

            s_Var.Append("  ");
            s_Varres = s_Var.ToString().Trim();
            if (s_Varres.Length > 0)
            {
                if (s_Varres.StartsWith("AND "))
                    s_Varres = s_Varres.Substring(4);
                else if (s_Varres.StartsWith("OR "))
                    s_Varres = s_Varres.Substring(3);
                else if (s_Varres.StartsWith(","))
                    s_Varres = s_Varres.Substring(1);
                sCmd.Append("  " + s_Varres);
                sCmd.Append(" ) as tabla");
                // Paginado
                sCmd.Append(" ) as onlyusuariosocial where onlyusuariosocial.yoy = 1");
                sCmd.Append(" ) as result");
                if (pagesize > 0)
                {
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "@PageSize";
                    sqlParam.DbType = DbType.Int32;
                    sqlParam.Value = pagesize;
                    sqlCmd.Parameters.Add(sqlParam);
                }

                if (pageindex > -2)
                {
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "@PageIndex";
                    sqlParam.DbType = DbType.Int32;
                    sqlParam.Value = pageindex;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                sCmd.Append(" WHERE result.rownumber BETWEEN cast(@PageSize * @PageIndex + 1 as varchar(10))");
                sCmd.Append(" AND cast(@PageSize * (@PageIndex + 1) as varchar(10))");

            }
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "UsuarioGrupo");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UsuarioByAreaConocimientoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
