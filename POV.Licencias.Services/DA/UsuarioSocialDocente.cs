// Licencias de Director
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;

using POV.CentroEducativo.BO;
using POV.Licencias.BO;

namespace POV.Licencias.DA
{
    /// <summary>
    /// UsuarioSocialDocente
    /// </summary>
    internal class UsuarioSocialDocente
    {
        /// <summary>
        /// Consulta registros de UsuarioSocialDocente en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="docente">Director que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de LicenciaDirector generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, Docente docente)
        {
            object myFirm = new object();
            string sError = "";
            if (docente == null)
                sError += ", docente";
            if (sError.Length > 0)
                throw new Exception("UsuarioSocialDocente: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (docente.DocenteID == null && docente.Curp == null && string.IsNullOrEmpty(docente.Curp.Trim()) && docente.Correo == null && string.IsNullOrEmpty(docente.Correo.Trim()))
                sError += ", DirectorID, CURP, Correo";
            if (sError.Length > 0)
                throw new Exception("UsuarioSocialDocente: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA", "UsuarioSocialDocente", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UsuarioSocialDocente: Hubo un error al conectarse a la base de datos", "POV.Licencias.DA", "UsuarioSocialDocente", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT DISTINCT UsuarioSocialID");
            sCmd.Append(" FROM Licencia lic ");
            sCmd.Append(" INNER JOIN LicenciaReferencia licRef ON (licRef.LicenciaID = lic.LicenciaID) ");
            sCmd.Append(" INNER JOIN Docente doc ON (doc.DocenteID = licRef.DocenteID) ");
            StringBuilder s_VarWHERE = new StringBuilder();

            if (docente.DocenteID != null)
            {
                s_VarWHERE.Append(" AND doc.DocenteID =@dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = docente.DocenteID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }

            if (docente.Curp != null)
            {
                s_VarWHERE.Append(" AND doc.Curp =@dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = docente.Curp;
                sqlParam.DbType = DbType.String;
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
            sCmd.Append(" ORDER BY UsuarioSocialID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "UsuarioSocialDocente");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UsuarioSocialDocente: Hubo un error al consultar los registros. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            return ds;
        }


        /// <summary>
        /// Consulta registros de UsuarioSocialDocente en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="docente">Director que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de LicenciaDirector generada por la consulta</returns>
        public DataSet Action(IDataContext dctx,LicenciaEscuela licenciaEscuela ,Docente docente)
        {
            object myFirm = new object();
            string sError = "";
            if (licenciaEscuela == null)
                sError += ", docente";
            if (docente == null)
                sError += ", docente";
            if (sError.Length > 0)
                throw new Exception("UsuarioSocialDocente: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (docente.DocenteID == null && docente.Curp == null && string.IsNullOrEmpty(docente.Curp.Trim()) && docente.Correo == null && string.IsNullOrEmpty(docente.Correo.Trim()))
                sError += ", DirectorID, CURP, Correo";
            if (licenciaEscuela.Escuela == null)
                sError += ", escuela";
            if (sError.Length > 0)
                throw new Exception("UsuarioSocialDocente: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (licenciaEscuela.Escuela.EscuelaID == null || licenciaEscuela.Escuela.EscuelaID <= 0)
                sError += ", escuelaID";
            if (sError.Length > 0)
                throw new Exception("UsuarioSocialDocente: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA", "UsuarioSocialDocente", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UsuarioSocialDocente: Hubo un error al conectarse a la base de datos", "POV.Licencias.DA", "UsuarioSocialDocente", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT UsuarioSocialID");
            sCmd.Append(" FROM LicenciaEscuela licEsc ");
            sCmd.Append(" INNER JOIN Licencia AS lic ON (lic.LicenciaEscuelaID = licEsc.LicenciaEscuelaID) ");
            sCmd.Append(" INNER JOIN LicenciaReferencia licRef ON (licRef.LicenciaID = lic.LicenciaID) ");
            sCmd.Append(" INNER JOIN Docente doc ON (doc.DocenteID = licRef.DocenteID) ");
            StringBuilder s_VarWHERE = new StringBuilder();

            if (docente.Curp != null)
            {
                s_VarWHERE.Append(" AND doc.Curp =@dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = docente.Curp;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (docente.DocenteID != null)
            {
                s_VarWHERE.Append(" AND doc.DocenteID =@dbp4ram2");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = docente.DocenteID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela.Escuela.EscuelaID != null)
            {
                s_VarWHERE.Append(" AND licEsc.EscuelaID =@dbp4ram3");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = licenciaEscuela.Escuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela.CicloEscolar.CicloEscolarID != null)
            {
                s_VarWHERE.Append(" AND licEsc.CicloEscolarID =@dbp4ram4");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = licenciaEscuela.CicloEscolar.CicloEscolarID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (licenciaEscuela.Activo != null)
            {
                s_VarWHERE.Append(" AND licEsc.Activo =@dbp4ram5");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = licenciaEscuela.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            s_VarWHERE.Append(" AND lic.Activo =1");

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
            sCmd.Append(" ORDER BY UsuarioSocialID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "UsuarioSocialDocente");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UsuarioSocialDocente: Hubo un error al consultar los registros. " + exmsg);
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
