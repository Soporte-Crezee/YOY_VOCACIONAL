using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace POV.Licencias.DA
{
    public class UsuarioEspecialista
    {
        public DataSet Action(IDataContext dctx, EspecialistaPruebas especialista)
        {
            object myFirm = new object();
            string sError = "";
            if (especialista == null)
                sError += ", especialista";
            if (sError.Length > 0)
                throw new Exception("UsuarioEspecialista: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (especialista.EspecialistaPruebaID == null && especialista.Curp == null && string.IsNullOrEmpty(especialista.Curp.Trim()) && especialista.Correo == null && string.IsNullOrEmpty(especialista.Correo.Trim()))
                sError += ", EspecialistaID, CURP, Correo";
            if (sError.Length > 0)
                throw new Exception("UsuarioEspecialista: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA", "UsuarioEspecialista", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UsuarioEspecialista: Hubo un error al conectarse a la base de datos", "POV.Licencias.DA", "UsuarioEspecialista", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT DISTINCT UsuarioID");
            sCmd.Append(" FROM Licencia lic ");
            sCmd.Append(" INNER JOIN LicenciaReferencia licRef ON (licRef.LicenciaID = lic.LicenciaID) ");
            sCmd.Append(" INNER JOIN EspecialistaPruebas esp ON (esp.EspecialistaID = licRef.EspecialistaID) ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (especialista.Curp != null)
            {
                s_VarWHERE.Append(" AND esp.Curp =@dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = especialista.Curp;
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
            sCmd.Append(" ORDER BY UsuarioID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "UsuarioEspecialista");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UsuarioEspecialista: Hubo un error al consultar los registros. " + exmsg);
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
