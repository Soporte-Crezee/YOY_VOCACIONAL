﻿// Clase motor de red social
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.Localizacion.Service;
using POV.Localizacion.BO;

namespace POV.CentroEducativo.DAO
{
    /// <summary>
    /// Elimina un registro de Escuela en la BD
    /// </summary>
    public class EscuelaDelHlp
    {
        /// <summary>
        /// Elimina un registro de EscuelaDelHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="escuelaDelHlp">EscuelaDelHlp que desea eliminar</param>
        public void Action(IDataContext dctx, Escuela escuela)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (escuela == null)
                sError += ", Escuela";
            if (sError.Length > 0)
                throw new Exception("EscuelaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (escuela.EscuelaID == null)
                sError += ", EscuelaID";
            if (sError.Length > 0)
                throw new Exception("EscuelaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO",
                   "EscuelaDelHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "EscuelaDelHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO",
                   "EscuelaDelHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Escuela ");
            sCmd.Append(" SET Clave = 0 ");
            // escuela.EscuelaID
            sCmd.Append(" WHERE @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (escuela.EscuelaID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = escuela.EscuelaID;
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
                throw new Exception("EscuelaDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("EscuelaDelHlp: Ocurrió un error al ingresar el registro.");
        }
    }
}
