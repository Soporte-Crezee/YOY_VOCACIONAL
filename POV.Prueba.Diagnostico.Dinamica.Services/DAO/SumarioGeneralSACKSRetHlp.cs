using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.DAO
{
    internal class SumarioGeneralSACKSRetHlp
    {
        /// <summary>
        /// Consulta registros de sumarioGeneralSacks en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="sumarioGeneralSacks">sumarioGeneralSacks que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de sumarioGeneralSacks generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, SumarioGeneralSacks sumarioGeneralSacks)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (sumarioGeneralSacks == null)
                sError += ", sumarioGeneralSacks";
            if (sError.Length > 0)
                throw new Exception("sumarioGeneralSacksRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "sumarioGeneralSacksRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "sumarioGeneralSacksRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "sumarioGeneralSacksRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT SumarioGeneralSACKSID,PruebaID,AlumnoID,SumarioMadurez,SumarioNivelRealidad,SumarioConflictosExpresados,FechaRegistro ");
            sCmd.Append(" FROM SumarioGeneralSacks ");
            sCmd.Append(" WHERE ");
            StringBuilder s_Var = new StringBuilder();
            if (sumarioGeneralSacks.SumarioGeneralSACKSID != null)
            {
                s_Var.Append(" SumarioGeneralSACKSID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = sumarioGeneralSacks.SumarioGeneralSACKSID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (sumarioGeneralSacks.Prueba != null)
                if (sumarioGeneralSacks.Prueba.PruebaID != null)
                {
                    s_Var.Append(" AND PruebaID = @dbp4ram2 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram2";
                    sqlParam.Value = sumarioGeneralSacks.Prueba.PruebaID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            
            if (sumarioGeneralSacks.Alumno != null)
                if (sumarioGeneralSacks.Alumno.AlumnoID != null)
                {
                    s_Var.Append(" AND AlumnoID = @dbp4ram3 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram3";
                    sqlParam.Value = sumarioGeneralSacks.Alumno.AlumnoID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            if (sumarioGeneralSacks.SumarioMadurez != null)
            {
                s_Var.Append(" AND sumariomadurez = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = sumarioGeneralSacks.SumarioMadurez;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (sumarioGeneralSacks.SumarioNivelRealida != null)
            {
                s_Var.Append(" AND SumarioNivelRealidad = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = sumarioGeneralSacks.SumarioNivelRealida;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (sumarioGeneralSacks.SumarioConflictoExpresados != null)
            {
                s_Var.Append(" AND SumarioConflictosExpresados = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = sumarioGeneralSacks.SumarioConflictoExpresados;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (sumarioGeneralSacks.FechaRegistro != null)
            {
                s_Var.Append(" AND FechaRegistro = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = sumarioGeneralSacks.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
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
                sqlAdapter.Fill(ds, "SumarioGeneralSACKS");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("sumarioGeneralSacksRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
