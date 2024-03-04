using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
namespace POV.CentroEducativo.DAO
{
    public class EspecialistaEscuelaDelHlp
    {
        public void Action(IDataContext dctx, EspecialistaEscuela especialistaEscuela)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (especialistaEscuela == null)
                sError += ", EspecialistaEscuela";
            if (sError.Length > 0)
                throw new Exception("EspecialistaEscuelaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (especialistaEscuela.EspecialistaEscuelaID == null)
                sError += ", EspecialistaEscuelaID";
            if (sError.Length > 0)
                throw new Exception("EspecialistaEscuelaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO",
                   "EspecialistaEscuelaDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EspecialistaEscuelaDelHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO",
                   "EspecialistaEscuelaDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" DELETE FROM EspecialistaEscuela ");
            sCmd.Append(" WHERE @especialistaEscuela_EspecialistaEscuelaID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "especialistaEscuela_EspecialistaEscuelaID";
            if (especialistaEscuela.EspecialistaEscuelaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialistaEscuela.EspecialistaEscuelaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            int iRes = 0;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                iRes = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("EspecialistaEscuelaDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("EspecialistaEscuelaDelHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
