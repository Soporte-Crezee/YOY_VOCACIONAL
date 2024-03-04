using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;

using POV.Licencias.BO;
using POV.Seguridad.BO;

namespace POV.Licencias.DAO
{
    /// <summary>
    /// LicenciaRetHlp
    /// </summary>
    internal class LicenciaRetHlp
    {
        /// <summary>
        /// Consulta registros de LicenciaRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="licenciaRetHlp">LicenciaRetHlp que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de LicenciaRetHlp generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Usuario usuario)
        {
            object myFirm = new object();
            string sError = "";
            if (usuario == null)
                sError += ", Usuario";
            if (sError.Length > 0)
                throw new Exception("LicenciaRetHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (usuario.UsuarioID == null)
                sError += ", Usuario.UsuarioID";
            if (sError.Length > 0)
                throw new Exception("LicenciaRetHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "PISA1021.Licencias.DAO", "LicenciaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaRetHlp: Hubo un error al conectarse a la base de datos", "PISA1021.Licencias.DAO", "LicenciaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT lic.LicenciaID, lic.LicenciaEscuelaID, lic.UsuarioID, lic.UsuarioSocialID, lic.Activo, licRef.AlumnoID, licRef.DocenteID, licRef.DirectorID, licRef.EspecialistaID, licRef.TutorID, licRef.UniversidadID, lic.Tipo, contrato.ContratoID ");
            sCmd.Append(" FROM Licencia lic ");
            sCmd.Append(" INNER JOIN LicenciaReferencia licRef ON (licRef.LicenciaID = lic.LicenciaID) ");
            sCmd.Append(" INNER JOIN LicenciaEscuela licEsc ON (licEsc.LicenciaEscuelaID = lic.LicenciaEscuelaID) ");
            sCmd.Append(" INNER JOIN CicloEscolar ciclo ON (ciclo.CicloEscolarID = licEsc.CicloEscolarID) ");
            sCmd.Append(" INNER JOIN Contrato contrato ON (contrato.ContratoID = licEsc.ContratoID) ");

            sCmd.Append(" WHERE lic.UsuarioID =@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = usuario.UsuarioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" AND contrato.InicioContrato <= @dbp4ram3 AND @dbp4ram3 <= contrato.FinContrato ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = DateTime.Now;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" AND ciclo.InicioCiclo <= @dbp4ram2 AND @dbp4ram2 <= ciclo.FinCiclo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = DateTime.Now;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" AND lic.Activo = 1 ");
            sCmd.Append(" AND licEsc.Activo = 1 ");
            sCmd.Append(" AND contrato.Estatus = 1 ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "Licencia");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("LicenciaRetHlp: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }

        public DataSet Action(IDataContext dctx, LicenciaEscuela licenciaEscuela)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaEscuela == null)
                sError += ", LicenciaEscuela";
            if (sError.Length > 0)
                throw new Exception("LicenciaRetHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEscuela.LicenciaEscuelaID == null)
                sError += ", LicenciaEscuela.LicenciaEscuelaID";
            if (sError.Length > 0)
                throw new Exception("LicenciaRetHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "PISA1021.Licencias.DAO", "LicenciaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "LicenciaRetHlp: Hubo un error al conectarse a la base de datos", "PISA1021.Licencias.DAO", "LicenciaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT lic.LicenciaID, lic.LicenciaEscuelaID, lic.UsuarioID, lic.UsuarioSocialID, lic.Activo, licRef.AlumnoID, licRef.DocenteID, licRef.DirectorID, licRef.EspecialistaID, licRef.TutorID, licRef.UniversidadID, lic.Tipo ");
            sCmd.Append(" FROM Licencia lic ");
            sCmd.Append(" INNER JOIN LicenciaReferencia licRef ON (licRef.LicenciaID = lic.LicenciaID) ");
            sCmd.Append(" INNER JOIN LicenciaEscuela licEsc ON (licEsc.LicenciaEscuelaID = lic.LicenciaEscuelaID) ");

            sCmd.Append(" WHERE lic.LicenciaEscuelaID =@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = licenciaEscuela.LicenciaEscuelaID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" AND lic.Activo = 1 ");
            sCmd.Append(" AND licEsc.Activo = 1 ");

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
                throw new Exception("LicenciaRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
