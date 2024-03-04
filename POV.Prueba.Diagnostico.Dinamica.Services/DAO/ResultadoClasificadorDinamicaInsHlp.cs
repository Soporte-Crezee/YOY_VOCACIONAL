using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    /// <summary>
    /// Guarda un registro de ResultadoClasificadorDinamica en la BD
    /// </summary>
    internal class ResultadoClasificadorDinamicaInsHlp
    {
        /// <summary>
        /// Crea un registro de ResultadoClasificadorDinamica en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="resultadoClasificadorDinamica">ResultadoClasificadorDinamica que desea crear</param>
        public void Action(IDataContext dctx, ResultadoClasificadorDinamica resultadoClasificadorDinamica)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (resultadoClasificadorDinamica == null)
                sError += ", ResultadoClasificadorDinamica";
            if (sError.Length > 0)
                throw new Exception("ResultadoClasificadorDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (resultadoClasificadorDinamica.ResultadoClasificadorID == null)
                sError += ", ResultadoClasificadorID";
            if(sError.Length > 0)
                throw new Exception("ResultadoClasificadorDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (resultadoClasificadorDinamica.Clasificador.ClasificadorID == null)
                sError += ", ClasificadorID";
            if (sError.Length > 0)
                throw new Exception("ResultadoClasificadorDinamicaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "ResultadoClasificadorDinamicaInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ResultadoClasificadorDinamicaInsHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "ResultadoClasificadorDinamicaInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO ResultadoClasificadorDinamica (ResultadoClasificadorID, ClasificadorID) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // resultadoClasificadorDinamica.ResultadoClasificadorDinamicaID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (resultadoClasificadorDinamica.ResultadoClasificadorID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = resultadoClasificadorDinamica.ResultadoClasificadorID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            // resultadoClasificadorDinamica.Clasificador.ClasificadorID
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (resultadoClasificadorDinamica.Clasificador.ClasificadorID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = resultadoClasificadorDinamica.Clasificador.ClasificadorID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ) ");
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
                throw new Exception("ResultadoClasificadorDinamicaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ResultadoClasificadorDinamicaInsHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
