using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;

namespace POV.Seguridad.DAO
{
    /// <summary>
    /// Consulta registros de UsuarioPrivilegios en la base de datos
    /// </summary>
    public class UsuarioPrivilegiosUsuarioRetHlp
    {
        /// <summary>
        /// Consulta registros de UsuarioPrivilegios en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioPrivilegios">UsuarioPrivilegios que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de UsuarioPrivilegios generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios)
        {
            object myFirm = new object();
            string sError = "";
            if (usuarioPrivilegios == null)
                sError += ", UsuarioPrivilegios";
            if (sError.Length > 0)
                throw new Exception("UsuarioPrivilegiosConsultar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (usuarioPrivilegios.Usuario == null)
                sError += ", Usuario";
            if (sError.Length > 0)
                throw new Exception("UsuarioPrivilegiosConsultar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GEOPESCA.Seguridad.AccesoDatos", "UsuarioPrivilegiosConsultarPorUsuario", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UsuarioPrivilegiosConsultar: Hubo un error al conectarse a la base de datos", "Seguridad.AccesoDatos", "UsuarioPrivilegiosConsultarPorUsuario", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT usrprv.UsuarioPrivilegiosID AS UsuarioPrivilegiosID, usrprv.UsuarioID AS UsuarioID, usr.NombreUsuario AS NombreUsuario, usr.FechaCreacion as FechaCreacion ");
            sCmd.Append(" FROM UsuarioPrivilegios usrprv ");
            sCmd.Append(" INNER JOIN Usuario usr ON usrprv.UsuarioID = usr.UsuarioID ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (usuarioPrivilegios.UsuarioPrivilegiosID != null)
            {
                s_VarWHERE.Append(" usrprv.UsuarioPrivilegiosID = @usuarioPrivilegios_UsuarioPrivilegiosID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "usuarioPrivilegios_UsuarioPrivilegiosID";
                sqlParam.Value = usuarioPrivilegios.UsuarioPrivilegiosID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (usuarioPrivilegios.Usuario.NombreUsuario != null)
            {
                s_VarWHERE.Append(" AND usr.NombreUsuario LIKE @usuarioPrivilegios_Usuario_NombreUsuario ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "usuarioPrivilegios_Usuario_NombreUsuario";
                sqlParam.Value = usuarioPrivilegios.Usuario.NombreUsuario;
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
            sCmd.Append(" ORDER BY UsuarioPrivilegiosID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "UsuarioPrivilegios");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UsuarioPrivilegiosConsultar: Hubo un error al consultar los registros. " + exmsg);
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
