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
    internal class SumarioGeneralFrasesVocacionalesRetHlp
    {
        /// <summary>
        /// Consulta registros de sumarioGeneralSacks en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="sumarioGeneralSacks">sumarioGeneralSacks que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de sumarioGeneralSacks generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, SumarioGeneralFrasesVocacionales sumarioGeneralFrasesVocacionales)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (sumarioGeneralFrasesVocacionales == null)
                sError += ", SumarioGeneralFrasesVocacionales";
            if (sError.Length > 0)
                throw new Exception("SumarioGeneralFrasesVocacionalesRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "SumarioGeneralFrasesVocacionalesRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "SumarioGeneralFrasesVocacionalesRetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO",
                   "SumarioGeneralFrasesVocacionalesRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT SumarioGeneralFrasesID,PruebaID,AlumnoID,SumarioOrganizacionPersonalidad,SumarioPerspectivaOpciones,SumarioFuentesConflicto,FechaRegistro ");
            sCmd.Append(" FROM SumarioGeneralFrasesVocacionales ");
            sCmd.Append(" WHERE ");
            StringBuilder s_Var = new StringBuilder();
            if (sumarioGeneralFrasesVocacionales.SumarioGeneralFrasesID != null)
            {
                s_Var.Append(" SumarioGeneralFrasesID = @dbp4ram1 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram1";
                sqlParam.Value = sumarioGeneralFrasesVocacionales.SumarioGeneralFrasesID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (sumarioGeneralFrasesVocacionales.Prueba != null)
                if (sumarioGeneralFrasesVocacionales.Prueba.PruebaID != null)
                {
                    s_Var.Append(" AND PruebaID = @dbp4ram2 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram2";
                    sqlParam.Value = sumarioGeneralFrasesVocacionales.Prueba.PruebaID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }

            if (sumarioGeneralFrasesVocacionales.Alumno != null)
                if (sumarioGeneralFrasesVocacionales.Alumno.AlumnoID != null)
                {
                    s_Var.Append(" AND AlumnoID = @dbp4ram3 ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "dbp4ram3";
                    sqlParam.Value = sumarioGeneralFrasesVocacionales.Alumno.AlumnoID;
                    sqlParam.DbType = DbType.Int32;
                    sqlCmd.Parameters.Add(sqlParam);
                }
            if (sumarioGeneralFrasesVocacionales.SumarioOrganizacionPersonalidad != null)
            {
                s_Var.Append(" AND SumarioOrganizacionPersonalidad = @dbp4ram5 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram5";
                sqlParam.Value = sumarioGeneralFrasesVocacionales.SumarioOrganizacionPersonalidad;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (sumarioGeneralFrasesVocacionales.SumarioPerspectivaOpciones != null)
            {
                s_Var.Append(" AND SumarioPerspectivaOpciones = @dbp4ram6 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram6";
                sqlParam.Value = sumarioGeneralFrasesVocacionales.SumarioPerspectivaOpciones;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (sumarioGeneralFrasesVocacionales.SumarioFuentesConflicto != null)
            {
                s_Var.Append(" AND SumarioFuentesConflicto = @dbp4ram7 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram7";
                sqlParam.Value = sumarioGeneralFrasesVocacionales.SumarioFuentesConflicto;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (sumarioGeneralFrasesVocacionales.FechaRegistro != null)
            {
                s_Var.Append(" AND FechaRegistro = @dbp4ram8 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram8";
                sqlParam.Value = sumarioGeneralFrasesVocacionales.FechaRegistro;
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
                sqlAdapter.Fill(ds, "SumarioGeneralFrasesVocacionales");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("SumarioGeneralFrasesVocacionalesRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
