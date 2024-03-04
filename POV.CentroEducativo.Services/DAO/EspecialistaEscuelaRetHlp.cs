using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace POV.CentroEducativo.DAO
{
    public class EspecialistaEscuelaRetHlp
    {
        public DataSet Action(IDataContext dctx, EspecialistaEscuela especialistaEscuela)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (especialistaEscuela == null)
                sError += ", EspecialistaEscuela";
            if (sError.Length > 0)
                throw new Exception("EspecialistaEscuelaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO",
                   "DocenteEscuelaRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EspecialistaEscuelaRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO",
                   "EspecialistaEscuelaRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT EspecialistaID, EscuelaID, EspecialistaEscuelaID, Estatus ");
            sCmd.Append(" FROM EspecialistaEscuela ");
            sCmd.Append(" WHERE ");
            StringBuilder s_Var = new StringBuilder();
            if (especialistaEscuela.EspecialistaEscuelaID != null)
            {
                s_Var.Append(" EspecialistaEscuelaID = @especialistaEscuela_DocenteEscuelaID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialistaEscuela_DocenteEscuelaID";
                sqlParam.Value = especialistaEscuela.EspecialistaEscuelaID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialistaEscuela.EscuelaID != null)
            {
                s_Var.Append(" AND EscuelaID =@especialistaEscuela_EscuelaID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialistaEscuela_EscuelaID";
                sqlParam.Value = especialistaEscuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialistaEscuela.EspecialistaEscuelaID != null)
            {
                s_Var.Append(" AND EspecialistaID =@especialistaEscuela_DocenteID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialistaEscuela_DocenteID";
                sqlParam.Value = especialistaEscuela.EspecialistaEscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (especialistaEscuela.Estatus != null)
            {
                s_Var.Append(" AND Estatus =@especialistaEscuela_Estatus ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialistaEscuela_Estatus";
                sqlParam.Value = especialistaEscuela.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            s_Var.Append("  ");
            string s_Varres = s_Var.ToString().Trim();
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
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "EspecialistaEscuela");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("EspecialistaEscuelaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
