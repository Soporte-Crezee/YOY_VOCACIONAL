using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// Elimina un Grupo la base de datos
    /// </summary>
    public class GrupoDelHlp
    {
        /// <summary>
        /// Elimina un registro de Grupo en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="grupo">Grupo que desea eliminar</param>
        public void Action(IDataContext dctx, Grupo grupo, Escuela escuela)
        {
            object myFirm = new object();
            string sError = "";
            if (grupo == null)
                sError += ", Grupo";
            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("GrupoDelHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
            if (grupo.GrupoID == null)
                sError += ", GrupoID";
            if (escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (sError.Length > 0)
                throw new Exception("GrupoDelHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", "GrupoDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "GrupoDelHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", "GrupoDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" DELETE FROM GRUPO ");
            if (grupo.GrupoID == null)
                sCmd.Append(" WHERE GRUPOID IS NULL ");
            else
            {
                // grupo.GrupoID
                sCmd.Append(" WHERE GRUPOID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = grupo.GrupoID;
                sqlParam.DbType = DbType.Guid;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (escuela.EscuelaID != null)
            {
                sCmd.Append(" AND ESCUELAID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = escuela.EscuelaID;
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
                throw new Exception("GrupoDelHlp: Hubo un error al eliminar el registro o no existe. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("GrupoDelHlp: Hubo un error al eliminar el registro o no existe.");
        }
    }
}
