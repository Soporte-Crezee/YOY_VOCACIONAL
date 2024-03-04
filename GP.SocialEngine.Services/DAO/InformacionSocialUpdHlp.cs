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
    /// Actualiza un registro de InformacionSocial en la BD
    /// </summary>
    public class InformacionSocialUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de InformacionSocialUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="informacionSocialUpdHlp">InformacionSocialUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">InformacionSocialUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, InformacionSocial informacionSocial, InformacionSocial anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (informacionSocial == null)
                sError += ", InformacionSocial";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("InformacionSocialUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.InformacionSocialID == null)
                sError += ", Anterior InformacionSocialID";
            if (sError.Length > 0)
                throw new Exception("InformacionSocialUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "InformacionSocialUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "InformacionSocialUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO",
                   "InformacionSocialUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE InformacionSocial ");
            if (informacionSocial.IMMSN == null)
                sCmd.Append(" SET IMMSN = NULL ");
            else
            {
                sCmd.Append(" SET IMMSN = @informacionSocial_IMMSN ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_IMMSN";
                sqlParam.Value = informacionSocial.IMMSN;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (informacionSocial.IMSkype == null)
                sCmd.Append(" ,IMSkype = NULL ");
            else
            {
                sCmd.Append(" ,IMSkype = @informacionSocial_IMSkype ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_IMSkype";
                sqlParam.Value = informacionSocial.IMSkype;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (informacionSocial.IMAOL == null)
                sCmd.Append(" ,IMAOL = NULL ");
            else
            {
                sCmd.Append(" ,IMAOL = @informacionSocial_IMAOL ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_IMAOL";
                sqlParam.Value = informacionSocial.IMAOL;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (informacionSocial.IMICQ == null)
                sCmd.Append(" ,IMICQ = NULL ");
            else
            {
                sCmd.Append(" ,IMICQ = @informacionSocial_IMICQ ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_IMICQ";
                sqlParam.Value = informacionSocial.IMICQ;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (informacionSocial.IMYahoo == null)
                sCmd.Append(" ,IMYahoo = NULL ");
            else
            {
                sCmd.Append(" ,IMYahoo = @informacionSocial_IMYahoo ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_IMYahoo";
                sqlParam.Value = informacionSocial.IMYahoo;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (informacionSocial.Firma == null)
                sCmd.Append(" ,Firma = NULL ");
            else
            {
                sCmd.Append(" ,Firma = @informacionSocial_Firma ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "informacionSocial_Firma";
                sqlParam.Value = informacionSocial.Firma;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.InformacionSocialID == null)
                sCmd.Append(" WHERE InformacionSocialID IS NULL ");
            else
            {
                sCmd.Append(" WHERE InformacionSocialID = @anterior_InformacionSocialID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "anterior_InformacionSocialID";
                sqlParam.Value = anterior.InformacionSocialID;
                sqlParam.DbType = DbType.Int64;
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
                throw new Exception("InformacionSocialUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("InformacionSocialUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
