﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;

namespace POV.Licencias.DAO
{
    /// <summary>
    /// Dao encargado de la consulta de los modulos funcionales del sistema
    /// </summary>
    internal class ModeloFuncionalRetHlp
    {
        /// <summary>
        /// Consulta registros de ModuloFuncional en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="moduloFuncional">ModuloFuncional que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de ModuloFuncional generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, ModuloFuncional moduloFuncional)
        {
            object myFirm = new object();
            string sError = "";
            if (moduloFuncional == null)
                sError += ", ModuloFuncional";
            if (sError.Length > 0)
                throw new Exception("ModeloFuncionalRetHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
           

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", "ModeloFuncionalRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ModeloFuncionalRetHlp: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", "ModeloFuncionalRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ModuloFuncionalID, Clave, Nombre, Descripcion ");
            sCmd.Append(" FROM ModuloFuncional ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (moduloFuncional.ModuloFuncionalId != null)
            {
                s_VarWHERE.Append(" ModuloFuncionalID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = moduloFuncional.ModuloFuncionalId;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (moduloFuncional.Clave != null)
            {
                s_VarWHERE.Append(" AND Clave = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = moduloFuncional.Clave;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (moduloFuncional.Nombre != null)
            {
                s_VarWHERE.Append(" AND Nombre LIKE @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = moduloFuncional.Nombre;
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
            sCmd.Append(" ORDER BY ModuloFuncionalID ASC ");
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "ModuloFuncional");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("ModeloFuncionalRetHlp: Hubo un error al consultar los registros. " + exmsg);
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
