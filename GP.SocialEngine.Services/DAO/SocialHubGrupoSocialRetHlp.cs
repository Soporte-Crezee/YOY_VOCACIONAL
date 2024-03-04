using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.Interfaces;
using POV.Reactivos.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Consulta registros de GrupoSocial relacionados a un socialHub en la BD
   /// </summary>
   public class SocialHubGrupoSocialRetHlp { 
      /// <summary>
      /// Consulta registros de GrupoSocial en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="grupoSocial">GrupoSocial que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de GrupoSocial generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, SocialHub socialHub){
         object myFirm = new object();
         string sError = String.Empty;
         if (socialHub == null)
            sError += ", SocialHub";
         if (sError.Length > 0)
            throw new Exception("SocialHubGrupoSocialRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (socialHub.SocialHubID == null)
            sError += ", SocialHubID";
         if (sError.Length > 0)
            throw new Exception("SocialHubGrupoSocialRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "SocialHubGrupoSocialRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "SocialHubGrupoSocialRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "SocialHubGrupoSocialRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT SocialHubID, GrupoSocialID ");
         sCmd.Append(" FROM SocialHubGrupoSocial ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (socialHub.SocialHubID != null){
            s_VarWHERE.Append(" SocialHubID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = socialHub.SocialHubID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            else if (s_VarWHEREres.StartsWith(","))
               s_VarWHEREres = s_VarWHEREres.Substring(1);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }

         sCmd.Append(" ORDER BY GrupoSocialID ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "GrupoSocial");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("SocialHubGrupoSocialRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
