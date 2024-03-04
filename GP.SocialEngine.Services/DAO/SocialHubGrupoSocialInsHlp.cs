using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.Interfaces;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Guarda un registro de SocialHubGrupoSocial BD
   /// </summary>
   public class SocialHubGrupoSocialInsHlp { 
      /// <summary>
      /// Crea un registro de SocialHubGrupoSocial en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="socialHubGrupoSocial">SocialHubGrupoSocial que desea crear</param>
      public void Action(IDataContext dctx, GrupoSocial grupoSocial,SocialHub socialHub){
         object myFirm = new object();
         string sError = String.Empty;
         if (socialHub == null)
            sError += ", SocialHub";
         if (grupoSocial == null)
            sError += ", GrupoSocial";
         if (sError.Length > 0)
            throw new Exception("SocialHubGrupoSocialRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (socialHub.SocialHubID == null)
            sError += ", SocialHubID";
         if (grupoSocial.GrupoSocialID == null)
            sError += ", SocialHubID";
         if (sError.Length > 0)
            throw new Exception("SocialHubGrupoSocialRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (sError.Length > 0)
            throw new Exception("SocialHubGrupoSocialInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "SocialHubGrupoSocialInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "SocialHubGrupoSocialInsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "SocialHubGrupoSocialInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO SocialHubGrupoSocial(SocialHubID, GrupoSocialID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // socialHub.SocialHubID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (socialHub.SocialHubID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = socialHub.SocialHubID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // grupoSocial.GrupoSocialID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (grupoSocial.GrupoSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = grupoSocial.GrupoSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("SocialHubGrupoSocialInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("SocialHubGrupoSocialInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
