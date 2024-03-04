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
    internal class PalabraClaveContenidoDelHlp
    {
        /// <summary>
        /// Elimina un registro de PalabraClaveContenidoDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="PalabraClaveDelHlp">PalabraClaveContenidoDelHlp que desea eliminar</param>
        public void Action(IDataContext dctx, APalabraClaveContenido palabraClaveContenido)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (palabraClaveContenido == null)
                sError += ", PalabraClaveContenido";
            if (sError.Length > 0)
                throw new Exception("PalabraClaveContenidoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (palabraClaveContenido.PalabraClaveContenidoID == null)
                sError += ", PalabraClaveContenidoID";
            if (sError.Length > 0)
                throw new Exception("PalabraClaveContenidoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.Busqueda.DAO",
                   "PalabraClaveContenidoDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PalabraClaveContenidoDelHlp: No se pudo conectar a la base de datos", "POV.ContenidosDigital.Busqueda.DAO",
                   "PalabraClaveContenidoDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" DELETE FROM PalabraClaveContenido ");

            if (palabraClaveContenido.PalabraClaveContenidoID == null)
                sCmd.Append(" WHERE PalabraClaveContenidoID IS NULL ");
            else
            {
                sCmd.Append(" WHERE PalabraClaveContenidoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = palabraClaveContenido.PalabraClaveContenidoID;
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
                throw new Exception("PalabraClaveContenidoDelHlp: Ocurrió un error al eliminar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("PalabraClaveContenidoDelHlp: Ocurrió un error al eliminar el registro.");
        }
    }
}
