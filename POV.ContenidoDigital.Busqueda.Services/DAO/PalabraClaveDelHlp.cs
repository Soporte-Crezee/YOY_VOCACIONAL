using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.Busqueda.BO;

namespace POV.ContenidosDigital.Busqueda.DAO
{
    /// <summary>
    /// Elimina un registro de PalabraClave en la BD
    /// </summary>
    internal class PalabraClaveDelHlp
    {
        /// <summary>
        /// Elimina un registro de PalabraClaveDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="PalabraClaveDelHlp">PalabraClaveDelHlp que desea eliminar</param>
        public void Action(IDataContext dctx, PalabraClave palabraClave)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (palabraClave == null)
                sError += ", PalabraClave";
            if (sError.Length > 0)
                throw new Exception("PalabraClaveDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (palabraClave.PalabraClaveID == null)
                sError += ", PalabraClaveID";
            if (sError.Length > 0)
                throw new Exception("PalabraClaveDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.Busqueda.DAO",
                   "PalabraClaveDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PalabraClaveDelHlp: No se pudo conectar a la base de datos", "POV.ContenidosDigital.Busqueda.DAO",
                   "PalabraClaveDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" DELETE FROM PalabraClave ");

            if (palabraClave.PalabraClaveID == null)
                sCmd.Append(" WHERE PalabraClaveID IS NULL ");
            else
            {
                sCmd.Append(" WHERE PalabraClaveID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = palabraClave.PalabraClaveID;
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
                throw new Exception("PalabraClaveDelHlp: Ocurrió un error al eliminar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("PalabraClaveDelHlp: Ocurrió un error al eliminar el registro.");
        }
    }
}
