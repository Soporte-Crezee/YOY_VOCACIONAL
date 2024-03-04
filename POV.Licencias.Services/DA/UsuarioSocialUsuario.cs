using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Licencias.BO;
using POV.Seguridad.BO;
using GP.SocialEngine.BO;

namespace POV.Licencias.DA
{
    /// <summary>
    /// UsuarioSocialUsuario
    /// </summary>
    internal class UsuarioSocialUsuario
    {
        /// <summary>
        /// Consulta registros de UsuarioSocialUsuario ó Usuario en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioSocial">Usuario social que provee el criterio de selección para realizar la consulta del usuario</param>
        /// <param name="usuario">Usuario que provee el criterio de selección para realizar la consulta del usuario social</param>
        /// <returns>El DataSet que contiene la información de LicenciaDirector generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, UsuarioSocial usuarioSocial = null, Usuario usuario = null, bool esDocente = true)
        {
            object myFirm = new object();
            string sError = "";
            if (usuarioSocial == null && usuario == null)
                sError += ", usuarioSocial, usuario";
            if (sError.Length > 0)
                throw new Exception("UsuarioSocialUsuario: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA", "UsuarioSocialUsuario", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UsuarioSocialUsuario: Hubo un error al conectarse a la base de datos", "POV.Licencias.DA", "UsuarioSocialUsuario", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT DISTINCT us.ScreenName, us.UsuarioSocialID, u.UsuarioID");
            sCmd.Append(" FROM Licencia lic ");
            sCmd.Append(" INNER JOIN LicenciaReferencia licRef ON (licRef.LicenciaID = lic.LicenciaID) ");
            if (esDocente)
                sCmd.Append(" INNER JOIN Docente doc ON (doc.DocenteID = licRef.DocenteID) ");
            sCmd.Append(" INNER JOIN Usuario u ON (u.UsuarioID = lic.UsuarioID) ");
            sCmd.Append(" INNER JOIN UsuarioSocial us ON (us.UsuarioSocialID = lic.UsuarioSocialID) ");

            StringBuilder s_VarWHERE = new StringBuilder();

            if (usuarioSocial != null)
            {
                if (usuarioSocial.UsuarioSocialID != null)
                {
                    s_VarWHERE.Append(" AND us.UsuarioSocialID =@dbp4ram1 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram1";
                    sqlParam.Value = usuarioSocial.UsuarioSocialID;
                    sqlParam.DbType = DbType.Int64;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            }

            if (usuario != null)
            {
                if (usuario.UsuarioID != null)
                {
                    s_VarWHERE.Append(" AND u.UsuarioID =@dbp4ram3 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram3";
                    sqlParam.Value = usuario.UsuarioID;
                    sqlParam.DbType = DbType.Int64;
                    sqlCmd.Parameters.Add(sqlParam);
                }
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
            sCmd.Append(" ORDER BY us.UsuarioSocialID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "UsuarioSocialUsuario");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UsuarioSocialUsuario: Hubo un error al consultar los registros. " + exmsg);
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
