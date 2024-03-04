using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.Interfaces;
using POV.Reactivos.BO;

namespace GP.SocialEngine.DAO
{
    /// <summary>
    /// Consultar Registros de SocialHub en la base de datos
    /// </summary>
    public class SocialHubUsuarioRetHlp
    {
        /// <summary>
        /// Consulta registros de SocialHub en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="socialHub">SocialHub que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de SocialHub generada por la consulta</returns>
        public DataSet Action(IDataContext dctx, SocialHub socialHub)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (socialHub == null)
                sError += ", socialHub";
            if (sError.Length > 0)
                throw new Exception("SocialHubRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (socialHub.SocialProfileType == null)
                sError += ", socialHub";
            if (sError.Length > 0)
                throw new Exception("SocialHubRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (socialHub.SocialProfileType != ESocialProfileType.USUARIOSOCIAL)
            {
                sError = "El Social Profile debe ser de tipo Usuario Social";
            }
            if (sError.Length > 0)
                throw new Exception("SocialHubInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
            if (socialHub.SocialProfile == null)
            {
                socialHub.SocialProfile = new UsuarioSocial();
            }
            if (socialHub.InformacionSocial == null)
            {
                socialHub.InformacionSocial = new InformacionSocial();
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO","SocialHubUsuarioRetHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "SocialHubRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO","SocialHubUsuarioRetHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" SELECT SocialHubID, FechaRegistro, Alias, UsuarioSocialID,ReactivoID, InformacionSocialID,SocialProfileType ");
            sCmd.Append(" FROM SocialHub ");
            StringBuilder s_VarWHERE = new StringBuilder();
            if (socialHub.SocialHubID != null)
            {
                s_VarWHERE.Append(" SocialHubID =@socialHub_SocialHubID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "socialHub_SocialHubID";
                sqlParam.Value = socialHub.SocialHubID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (socialHub.FechaRegistro != null)
            {
                s_VarWHERE.Append(" AND FechaRegistro= @socialHub_FechaRegistro ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "socialHub_FechaRegistro";
                sqlParam.Value = socialHub.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (socialHub.Alias != null)
            {
                s_VarWHERE.Append(" AND Alias= @socialHub_Alias ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "socialHub_Alias";
                sqlParam.Value = socialHub.Alias;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (socialHub.SocialProfileID != null)
            {
                s_VarWHERE.Append(" AND UsuarioSocialID= @socialHub_SocialProfileID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "socialHub_SocialProfileID";
                sqlParam.Value = socialHub.SocialProfileID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (socialHub.InformacionSocial.InformacionSocialID != null)
            {
                s_VarWHERE.Append(" AND InformacionSocialID= @socialHub_InformacionSocial_InformacionSocialID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "socialHub_InformacionSocial_InformacionSocialID";
                sqlParam.Value = socialHub.InformacionSocial.InformacionSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (socialHub.SocialProfileType != null)
            {
                s_VarWHERE.Append(" AND SocialProfileType = @socialHub_SocialProfileType ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "socialHub_SocialProfileType";
                sqlParam.Value = socialHub.SocialProfileType;
                sqlParam.DbType = DbType.Int16;
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
            DataSet ds = new DataSet();
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                sqlAdapter.Fill(ds, "SocialHub");
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("SocialHubRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
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
