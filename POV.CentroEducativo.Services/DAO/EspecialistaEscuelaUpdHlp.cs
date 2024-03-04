using Framework.Base.DataAccess;
using System;
using System.Text;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using System.Data;
using System.Data.Common;

namespace POV.CentroEducativo.DAO
{
    public class EspecialistaEscuelaUpdHlp
    {
        public void Action(IDataContext dctx, EspecialistaEscuela especialistaEscuela, EspecialistaEscuela anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (especialistaEscuela == null)
                sError += ", EspecialistaEscuela";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("EspecialistaEscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.EspecialistaEscuelaID == null)
                sError += ", Anterior EspecialistaEscuelaID";
            if (sError.Length > 0)
                throw new Exception("EspecialistaEscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO",
                   "EspecialistaEscuelaUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EspecialistaEscuelaUpdHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO",
                   "EspecialistaEscuelaUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE EspecialistaEscuela ");
            if (especialistaEscuela.EscuelaID == null)
                sCmd.Append(" SET EscuelaID = NULL ");
            else
            {
                sCmd.Append(" SET EscuelaID = @especialistaEscuela_EscuelaID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "especialistaEscuela_EscuelaID";
                sqlParam.Value = especialistaEscuela.EscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            sCmd.Append(" ,EspecialistaID=@especialistaEscuela_EspecialistaID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "especialistaEscuela_EspecialistaID";
            if (especialistaEscuela.EspecialistaEscuelaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialistaEscuela.EspecialistaEscuelaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,Estatus=@especialistaEscuela_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "especialistaEscuela_Estatus";
            if (especialistaEscuela.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = especialistaEscuela.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            if (anterior.EspecialistaEscuelaID == null)
                sCmd.Append(" WHERE EspecialistaEscuelaID IS NULL ");
            else
            {
                sCmd.Append(" WHERE EspecialistaEscuelaID = @anterior_EspecialistaEscuelaID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "anterior_EspecialistaEscuelaID";
                sqlParam.Value = anterior.EspecialistaEscuelaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
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
                throw new Exception("EspecialistaEscuelaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("EspecialistaEscuelaUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
