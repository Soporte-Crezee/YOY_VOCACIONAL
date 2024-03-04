using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace POV.Licencias.DA
{
    /// <summary>
    /// UsuarioOrientador
    /// </summary>
    internal class UsuarioOrientador
    {
        /// <summary>
        /// Consulta registros de LicenciaDocente en la base de datos
        /// </summary>
        /// <param name="dctx"> El DataContext que proveerá acceso a la base de datos </param>
        /// <param name="docente"> Docente ue provee el criterio de selección para realizar la consulta</param>
        /// <returns></returns>
        public DataSet Action(IDataContext dctx, Usuario usuario) 
        {
            object myFirm = new object();
            string sError = "";

            if (usuario == null)
                sError += ", usuario";
            if (sError.Length > 0)
                throw new Exception("UsuarioOrientador: Al menos uno de los siguientes campos es necesario: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA", "UsuarioOrientador", "Action", null, null);

            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception)
            {
                throw new StandardException(MessageType.Error, "", "UsuarioOrientador: Hubo un error al conectarse a la base de datos", "POV.Licencia.DA", "UsuarioOrientador", "Action", null, null);
            }

            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT DISTINCT us.UsuarioID, us.NombreUsuario, doc.DocenteID, doc.nombre, doc.primerapellido, doc.segundoapellido, us.universidadID, un.nombreuniversidad");
            sCmd.Append(" FROM Licencia lic ");
            sCmd.Append(" INNER JOIN LicenciaReferencia licRef ON (licRef.LicenciaID = lic.LicenciaID) ");
            sCmd.Append(" INNER JOIN Docente doc ON (doc.DocenteID = licRef.DocenteID) ");
            sCmd.Append(" INNER JOIN Usuario us ON (us.UsuarioID = lic.UsuarioID) ");
            sCmd.Append(" LEFT OUTER JOIN Universidad un ON (un.universidadID = us.universidadID) ");
            StringBuilder s_VarWHERE = new StringBuilder();

            if (usuario != null)
            {
                if (usuario.UsuarioID != null)
                {
                    s_VarWHERE.Append(" AND us.UsuarioID =@dbp4ram1 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram1";
                    sqlParam.Value = usuario.UsuarioID;
                    sqlParam.DbType = DbType.String;
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
            sCmd.Append(" ORDER BY UsuarioID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "UsuarioOrientador");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UsuarioOrientador: Hubo un error al consultar los registros. " + exmsg);
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
