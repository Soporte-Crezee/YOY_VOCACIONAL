using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Prueba.Diagnostico.Dinamica.BO;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using System.Data.Common;
using System.Data;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    internal class PruebaDinamicaDelHlp
    {
        /// <summary>
        /// Elimina un registro de PruebaDinamica en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="prueba">PruebaDinamica que se eliminará</param>
        public void Action(IDataContext dctx, PruebaDinamica prueba)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (prueba == null)
                sError += ", PruebaDinamica";
            if (sError.Length > 0)
                throw new Exception("PruebaDinamicaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (prueba.PruebaID == null)
                sError += ", PruebaID";
            if (sError.Length > 0)
                throw new Exception("PruebaDinamicaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "PruebaDinamicaDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PruebaDinamicaDelHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "PruebaDinamicaDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" DELETE FROM PruebaDinamica ");
            if (prueba.PruebaID == null)
                sCmd.Append(" WHERE PruebaID IS NULL ");
            else
            {
                // prueba.PruebaID
                sCmd.Append(" WHERE PruebaID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = prueba.PruebaID;
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
                throw new Exception("PruebaDinamicaDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("PruebaDinamicaDelHlp: Ocurrió un error al ingresar el registro.");
        }

    }
}
