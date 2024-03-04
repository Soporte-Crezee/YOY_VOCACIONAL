using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO
{
    /// <summary>
    /// Consulta un registro de InformacionSocial en la BD
    /// </summary>
    public class InformacionSocialRetHlp
    {
        /// <summary>
        /// Consulta registros de InformacionSocial en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="informacionSocial">InformacionSocial que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de InformacionSocial generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, InformacionSocial informacionSocial)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (informacionSocial == null)
                sError += ", InformacionSocial";
            if (sError.Length > 0)
                throw new Exception("InformacionSocialRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "InformacionSocialRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "InformacionSocialRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO",
                   "InformacionSocialRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT ");
            sCmd.Append(" InformacionSocialID, IMMSN, IMSkype, IMAOL, IMICQ, IMYahoo, Firma, FechaRegistro ");
            sCmd.Append(" FROM InformacionSocial ");
            sCmd.Append(" WHERE ");
            StringBuilder s_Var = new StringBuilder();
            if (informacionSocial.InformacionSocialID != null)
            {
                s_Var.Append(" InformacionSocialID = @informacionSocial_InformacionSocialID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_InformacionSocialID";
                sqlParam.Value = informacionSocial.InformacionSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (informacionSocial.IMMSN != null)
            {
                s_Var.Append(" AND IMMSN = @informacionSocial_IMMSN ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_IMMSN";
                sqlParam.Value = informacionSocial.IMMSN;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (informacionSocial.IMSkype != null)
            {
                s_Var.Append(" AND IMSkype = @informacionSocial_IMSkype ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_IMSkype";
                sqlParam.Value = informacionSocial.IMSkype;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (informacionSocial.IMAOL != null)
            {
                s_Var.Append(" AND IMAOL = @informacionSocial_IMAOL ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_IMAOL";
                sqlParam.Value = informacionSocial.IMAOL;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (informacionSocial.IMICQ != null)
            {
                s_Var.Append(" AND IMICQ = @informacionSocial_IMICQ ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_IMICQ";
                sqlParam.Value = informacionSocial.IMICQ;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (informacionSocial.IMYahoo != null)
            {
                s_Var.Append(" AND IMYahoo = @informacionSocial_IMYahoo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_IMYahoo";
                sqlParam.Value = informacionSocial.IMYahoo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (informacionSocial.Firma != null)
            {
                s_Var.Append(" AND Firma = @informacionSocial_Firma ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_Firma";
                sqlParam.Value = informacionSocial.Firma;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (informacionSocial.FechaRegistro != null)
            {
                s_Var.Append(" AND FechaRegistro = @informacionSocial_FechaRegistro ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_FechaRegistro";
                sqlParam.Value = informacionSocial.FechaRegistro;
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
                sqlAdapter.Fill(ds, "InformacionSocial");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("InformacionSocialRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
