using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    /// <summary>
    /// Consulta un registro de BancoReactivosDinamico en la BD
    /// </summary>
    internal class BancoReactivosDinamicoRetHlp
    {
        /// <summary>
        /// Consulta registros de BancoReactivosDinamico en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="bancoReactivosDinamico">BancoReactivosDinamico que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de BancoReactivosDinamico generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, BancoReactivosDinamico bancoReactivosDinamico)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (bancoReactivosDinamico == null)
                sError += ", BancoReactivosDinamico";
            if (sError.Length > 0)
                throw new Exception("BancoReactivosDinamicoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "BancoReactivosDinamicoRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "BancoReactivosDinamicoRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "BancoReactivosDinamicoRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT BancoReactivoID,PruebaID,NumeroReactivos,FechaRegistro,Activo,EsSeleccionOrdenada,TipoSeleccionBanco, ReactivosPorPagina,EsPorGrupo ");
            sCmd.Append(" FROM BancoReactivosDinamico ");
            sCmd.Append(" WHERE ");
            StringBuilder s_Var = new StringBuilder();
            if (bancoReactivosDinamico.BancoReactivoID != null)
            {
                s_Var.Append(" BancoReactivoID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = bancoReactivosDinamico.BancoReactivoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (bancoReactivosDinamico.Prueba != null)
                if (bancoReactivosDinamico.Prueba.PruebaID != null)
                {
                    s_Var.Append(" AND PruebaID = @dbp4ram2 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram2";
                    sqlParam.Value = bancoReactivosDinamico.Prueba.PruebaID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            if (bancoReactivosDinamico.NumeroReactivos != null)
            {
                s_Var.Append(" AND NumeroReactivos = @dbp4ram3 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram3";
                sqlParam.Value = bancoReactivosDinamico.NumeroReactivos;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (bancoReactivosDinamico.FechaRegistro != null)
            {
                s_Var.Append(" AND FechaRegistro = @dbp4ram4 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram4";
                sqlParam.Value = bancoReactivosDinamico.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (bancoReactivosDinamico.Activo != null)
            {
                s_Var.Append(" AND Activo = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = bancoReactivosDinamico.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (bancoReactivosDinamico.EsSeleccionOrdenada != null)
            {
                s_Var.Append(" AND EsSeleccionOrdenada = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = bancoReactivosDinamico.EsSeleccionOrdenada;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (bancoReactivosDinamico.TipoSeleccionBanco != null)
            {
                s_Var.Append(" AND TipoSeleccionBanco = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = bancoReactivosDinamico.TipoSeleccionBanco;
                sqlParam.DbType = DbType.Byte;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (bancoReactivosDinamico.ReactivosPorPagina != null)
            {
                s_Var.Append(" AND ReactivosPorPagina = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = bancoReactivosDinamico.ReactivosPorPagina;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (bancoReactivosDinamico.EsPorGrupo != null)
            {
                s_Var.Append(" AND EsPorGrupo = @dbp4ram9 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram9";
                sqlParam.Value = bancoReactivosDinamico.EsPorGrupo;
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
                sqlAdapter.Fill(ds, "BancoReactivosDinamico");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("BancoReactivosDinamicoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
