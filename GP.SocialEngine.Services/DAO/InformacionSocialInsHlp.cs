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
    /// Guarda un registro de InformacionSocial en la BD
    /// </summary>
    public class InformacionSocialInsHlp
    {
        /// <summary>
        /// Crea un registro de InformacionSocial en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="informacionSocial">InformacionSocial que desea crear</param>
        public void Action(IDataContext dctx, InformacionSocial informacionSocial)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (informacionSocial == null)
                sError += ", InformacionSocial";
            if (sError.Length > 0)
                throw new Exception("InformacionSocialInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (informacionSocial.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (sError.Length > 0)
                throw new Exception("InformacionSocialInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "InformacionSocialInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "InformacionSocialInsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO",
                   "InformacionSocialInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO InformacionSocial (IMMSN, IMSkype, IMAOL,IMICQ,IMYahoo,Firma,FechaRegistro) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            sCmd.Append(" @informacionSocial_IMMSN ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "informacionSocial_IMMSN";
            if (informacionSocial.IMMSN == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = informacionSocial.IMMSN;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@informacionSocial_IMSkype ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "informacionSocial_IMSkype";
            if (informacionSocial.IMSkype == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = informacionSocial.IMSkype;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@informacionSocial_IMAOL ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "informacionSocial_IMAOL";
            if (informacionSocial.IMAOL == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = informacionSocial.IMAOL;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@informacionSocial_IMICQ ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "informacionSocial_IMICQ";
            if (informacionSocial.IMICQ == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = informacionSocial.IMICQ;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@informacionSocial_IMYahoo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "informacionSocial_IMYahoo";
            if (informacionSocial.IMYahoo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = informacionSocial.IMYahoo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@informacionSocial_Firma ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "informacionSocial_Firma";
            if (informacionSocial.Firma == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = informacionSocial.Firma;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@informacionSocial_FechaRegistro ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "informacionSocial_FechaRegistro";
            if (informacionSocial.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = informacionSocial.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
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
                throw new Exception("InformacionSocialInsHlp: OcurriÃ³ un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("InformacionSocialInsHlp: OcurriÃ³ un error al ingresar el registro.");
        }
    }
}
