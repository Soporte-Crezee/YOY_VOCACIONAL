using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Licencias.BO;
using POV.Comun.BO;
using POV.Localizacion.BO;

namespace POV.Licencias.DAO
{
    /// <summary>
    /// LicenciaEscuelaRetHlp
    /// </summary>
    internal class LicenciaEscuelaRetHlp
    {
        /// <summary>
        /// Consulta registros de LicenciaEscuelaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaEscuela">LicenciaEscuelaRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de LicenciaEscuelaRetHlp generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, LicenciaEscuela licenciaEscuela)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaRetHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));

            if (licenciaEscuela.Contrato == null)
                licenciaEscuela.Contrato = new Contrato();
            if (licenciaEscuela.Escuela == null)
                licenciaEscuela.Escuela = new Escuela();
            if (licenciaEscuela.CicloEscolar == null)
                licenciaEscuela.CicloEscolar = new CicloEscolar();

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", "LicenciaEscuelaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaEscuelaRetHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", "LicenciaEscuelaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT LicenciaEscuelaID, EscuelaID, NumeroLicencias, CicloEscolarID, FechaInicio, FechaFin,Tipo, Activo, ContratoID");
            sCmd.Append(" FROM LicenciaEscuela ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (licenciaEscuela.LicenciaEscuelaID != null)
            {
                s_VarWHERE.Append(" LicenciaEscuelaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = licenciaEscuela.LicenciaEscuelaID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela.Escuela.EscuelaID != null)
            {
                s_VarWHERE.Append(" AND EscuelaID =@dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = licenciaEscuela.Escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela.CicloEscolar.CicloEscolarID != null)
            {
                s_VarWHERE.Append(" AND CicloEscolarID =@dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = licenciaEscuela.CicloEscolar.CicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela.Activo != null)
            {
                s_VarWHERE.Append(" AND Activo =@dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = licenciaEscuela.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela.Contrato.ContratoID != null)
            {
                s_VarWHERE.Append(" AND ContratoID =@dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = licenciaEscuela.Contrato.ContratoID;
                sqlParam.DbType = DbType.Int64;
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
            sCmd.Append(" ORDER BY LicenciaEscuelaID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "LicenciaEscuela");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("LicenciaEscuelaRetHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }


        public DataSet ActionFilterByEscuela(IDataContext dctx, LicenciaEscuela licenciaEscuela)
        {
            DataSet dsEscuelas = new DataSet();
            string sError = string.Empty;
            if (licenciaEscuela.Contrato == null)
                licenciaEscuela.Contrato = new Contrato();
            if (licenciaEscuela.CicloEscolar == null)
                licenciaEscuela.CicloEscolar = new CicloEscolar();

            Escuela escuela = licenciaEscuela.Escuela;
            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("EscuelaDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (escuela.Ubicacion == null)
                escuela.Ubicacion = new Ubicacion();
            if (escuela.Ubicacion.Pais == null)
                escuela.Ubicacion.Pais = new Pais();

            if (escuela.Ubicacion.Estado == null)
                escuela.Ubicacion.Estado = new Estado();
            if (escuela.Ubicacion.Ciudad == null)
                escuela.Ubicacion.Ciudad = new Ciudad();
            if (escuela.Ubicacion.Localidad == null)
                escuela.Ubicacion.Localidad = new Localidad();

            if (escuela.DirectorID == null)
                escuela.DirectorID = new Director();
            if (escuela.TipoServicio == null)
                escuela.TipoServicio = new TipoServicio();
            if (escuela.TipoServicio.NivelEducativoID == null)
                escuela.TipoServicio.NivelEducativoID = new NivelEducativo();
            if (escuela.TipoServicio.NivelEducativoID.TipoNivelEducativoID == null)
                escuela.TipoServicio.NivelEducativoID.TipoNivelEducativoID = new TipoNivelEducativo();
            if (escuela.ZonaID == null)
                escuela.ZonaID = new Zona();

            //variables
            string swhere = string.Empty;
            StringBuilder query = new StringBuilder();
            DbParameter dbParameter = null;
            DbCommand dbCommand = null;
            DbCommand dbCommandControl = null;
            object myFirm = new object();
            try
            {
                dctx.OpenConnection(myFirm);
                dbCommand = dctx.CreateCommand();
                dbCommandControl = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("EscuelaDARetHlp: No se pudo conectar con la base de datos {0}", ex.Message));
            }
            #region ControlData

            query.Append(" SELECT ");
            query.Append(" LICESC.LicenciaEscuelaID, LICESC.ContratoID, LICESC.CicloEscolarID, LICESC.Tipo, LICESC.Activo, ");
            query.Append(" ESC.EscuelaID, ESC.Clave, ESC.NombreEscuela, ESC.FechaRegistro, ESC.Estatus, ESC.Turno, ESC.ZonaID,ESC.DirectorID, ESC.Control, ESC.Ambito,");
            query.Append(" ESC.UbicacionID, UBC.PaisID, UBC.EstadoID, UBC.CiudadID, UBC.LocalidadID, ESC.TipoServicioID, TIPS.NivelEducativoID, TIPONIV.TipoNivelEducativoID ");
            query.Append(" FROM LicenciaEscuela AS LICESC INNER JOIN ");
            query.Append(" Escuela AS ESC ON Esc.EscuelaID = LICESC.EscuelaID INNER JOIN");
            query.Append(" Ubicacion AS UBC ON ESC.UbicacionID = UBC.UbicacionID INNER JOIN ");
            query.Append(" TipoServicio AS TIPS ON ESC.TipoServicioID = TIPS.TipoServicioID INNER JOIN ");
            query.Append(" NivelEducativo AS NIV ON TIPS.NivelEducativoID = NIV.NivelEducativoID INNER JOIN ");
            query.Append(" TipoNivelEducativo AS TIPONIV ON NIV.TipoNivelEducativoID = TIPONIV.TipoNivelEducativoID ");

            if (escuela.EscuelaID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.EscuelaID =  @EscuelaID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@EscuelaID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.EscuelaID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Clave != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.Clave LIKE  @Clave";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Clave";
                dbParameter.DbType = DbType.String;
                dbParameter.Value = escuela.Clave;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.NombreEscuela != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.NombreEscuela LIKE  @NombreEscuela";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@NombreEscuela";
                dbParameter.DbType = DbType.String;
                dbParameter.Value = escuela.NombreEscuela;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.FechaRegistro != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.FechaRegistro =  @CFechaRegistro";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@FechaRegistro";
                dbParameter.DbType = DbType.DateTime;
                dbParameter.Value = escuela.FechaRegistro;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Estatus != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.Estatus =  @Estatus";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Estatus";
                dbParameter.DbType = DbType.Boolean;
                dbParameter.Value = escuela.Estatus;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Turno != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.Turno =  @Turno";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Turno";
                dbParameter.DbType = DbType.Byte;
                dbParameter.Value = (byte)escuela.Turno.Value;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.ZonaID.ZonaID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.ZonaID =  @ZonaID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@ZonaID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = escuela.ZonaID.ZonaID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.DirectorID.DirectorID != null)
            {

                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.DirectorID =  @DirectorID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@DirectorID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.DirectorID.DirectorID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Control != null)
            {

                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.Control =  @Control";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Control";
                dbParameter.DbType = DbType.Byte;
                dbParameter.Value = (byte)escuela.Control.Value;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Ambito != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.Ambito =  @Ambito";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@Ambito";
                dbParameter.DbType = DbType.Byte;
                dbParameter.Value = (byte)escuela.Ambito.Value;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Ubicacion.UbicacionID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.UbicacionID =  @UbicacionID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@UbicacionID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = escuela.Ubicacion.UbicacionID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Ubicacion.Pais.PaisID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "UBC.PaisID =  @PaisID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@PaisID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.Ubicacion.Pais.PaisID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Ubicacion.Estado.EstadoID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "UBC.EstadoID =  @EstadoID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@EstadoID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.Ubicacion.Estado.EstadoID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Ubicacion.Ciudad.CiudadID != null)
            {

                if (swhere.Length > 0) swhere += " AND ";
                swhere += "UBC.CiudadID =  @CiudadID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@CiudadID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.Ubicacion.Ciudad.CiudadID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.Ubicacion.Localidad.LocalidadID != null)
            {

                if (swhere.Length > 0) swhere += " AND ";
                swhere += "UBC.LocalidadID =  @LocalidadID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@LocalidadID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.Ubicacion.Localidad.LocalidadID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.TipoServicio.TipoServicioID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "ESC.TipoServicioID =  @TipoServicioID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@TipoServicioID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.TipoServicio.TipoServicioID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.TipoServicio.NivelEducativoID.NivelEducativoID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "TIPS.NivelEducativoID =  @NivelEducativoID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@NivelEducativoID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.TipoServicio.NivelEducativoID.NivelEducativoID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (escuela.TipoServicio.NivelEducativoID.TipoNivelEducativoID.TipoNivelEducativoID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += "TIPONIV.TipoNivelEducativoID =  @TipoNivelEducativoID";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "@TipoNivelEducativoID";
                dbParameter.DbType = DbType.Int32;
                dbParameter.Value = escuela.TipoServicio.NivelEducativoID.TipoNivelEducativoID.TipoNivelEducativoID;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (licenciaEscuela.LicenciaEscuelaID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " LICESC.LicenciaEscuelaID = @dbp4ram1 ";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "dbp4ram1";
                dbParameter.Value = licenciaEscuela.LicenciaEscuelaID;
                dbParameter.DbType = DbType.Int64;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (licenciaEscuela.CicloEscolar.CicloEscolarID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " LICESC.CicloEscolarID =@dbp4ram3 ";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "dbp4ram3";
                dbParameter.Value = licenciaEscuela.CicloEscolar.CicloEscolarID;
                dbParameter.DbType = DbType.Int32;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (licenciaEscuela.Activo != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " LICESC.Activo =@dbp4ram4 ";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "dbp4ram4";
                dbParameter.Value = licenciaEscuela.Activo;
                dbParameter.DbType = DbType.Boolean;
                dbCommandControl.Parameters.Add(dbParameter);
            }
            if (licenciaEscuela.Contrato.ContratoID != null)
            {
                if (swhere.Length > 0) swhere += " AND ";
                swhere += " LICESC.ContratoID =@dbp4ram5 ";
                dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = "dbp4ram5";
                dbParameter.Value = licenciaEscuela.Contrato.ContratoID;
                dbParameter.DbType = DbType.Int64;
                dbCommandControl.Parameters.Add(dbParameter);
            }

            if (swhere.Length > 0) query.Append(" WHERE " + swhere);
            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter();
                dbCommandControl.CommandText = query.ToString();
                adapter.SelectCommand = dbCommandControl;
                adapter.Fill(dsEscuelas, "Escuelas");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("EscuelaDARetHlp: Ocurrió un problema al recuperar los datos de control: {0}", ex.Message));
            }


            #endregion

            return dsEscuelas;
        }

        /// <summary>
        /// Consulta registros de ModuloFuncional en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaEscuela">LicenciaEscuela que provee el criterio de selección para realizar la consulta</param>
        /// <param name="moduloFuncional">ModuloFuncional que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ModuloFuncional generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, LicenciaEscuela licenciaEscuela, ModuloFuncional moduloFuncional)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaRetHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));

            if (moduloFuncional == null)
                sError += ", ModuloFuncional";
            if (sError.Length > 0)
                throw new Exception("LicenciaEscuelaRetHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));


            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", "ModeloFuncionalRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaEscuelaRetHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", "ModeloFuncionalRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT lemf.LicenciaEscuelaID, lemf.ModuloFuncionalID, mf.Clave, mf.Nombre, mf.Descripcion ");
            sCmd.Append(" FROM LicenciaEscuelaModuloFuncional lemf");
            sCmd.Append(" INNER JOIN ModuloFuncional mf on lemf.ModuloFuncionalId = mf.ModuloFuncionalId ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (licenciaEscuela.LicenciaEscuelaID != null)
            {
                s_VarWHERE.Append(" lemf.LicenciaEscuelaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = licenciaEscuela.LicenciaEscuelaID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (moduloFuncional.ModuloFuncionalId != null)
            {
                s_VarWHERE.Append(" AND lemf.ModuloFuncionalID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = moduloFuncional.ModuloFuncionalId;
                sqlParam.DbType = DbType.Int32;
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
            sCmd.Append(" ORDER BY ModuloFuncionalID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "LicenciaEscuelaModuloFuncional");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("LicenciaEscuelaRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
