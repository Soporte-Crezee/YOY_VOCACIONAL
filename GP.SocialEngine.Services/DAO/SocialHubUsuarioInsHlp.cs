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
   /// Crea un socialHub de un usuario en la base de datos
   /// </summary>
   public class SocialHubUsuarioInsHlp { 
      /// <summary>
      /// Crea un registro de SocialHub en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="socialHub">SocialHub que desea crear</param>
      public void Action(IDataContext dctx, SocialHub socialHub){
         object myFirm = new object();
         String sError = String.Empty;
         if (socialHub == null)
            sError += ", SocialHub";
         if (socialHub.SocialProfileType == null)
            sError += ", SocialProfileType";
         if (sError.Length > 0)
            throw new Exception("SocialHubInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
            
         if (sError.Length > 0)
            throw new Exception("SocialHubInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
         if (socialHub.SocialProfile == null)
            sError += ", SocialProfile";
         if (sError.Length > 0)
            throw new Exception("SocialHubInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
         if (socialHub.SocialProfile is UsuarioSocial)
         {
             if ((socialHub.SocialProfile as UsuarioSocial).UsuarioSocialID == null)
                 sError += ", UsuarioSocialID";
         }
         if (sError.Length > 0)
            throw new Exception("SocialHubInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
         if (socialHub.InformacionSocial == null)
            sError += ", InformacionSocial";
         if (sError.Length > 0)
            throw new Exception("SocialHubInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
         if (socialHub.InformacionSocial.InformacionSocialID == null)
            sError += ", InformacionSocialID";
         if (sError.Length > 0)
            throw new Exception("SocialHubInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
         if (socialHub.Alias == null || socialHub.Alias.Trim().Length == 0)
            sError += ", Alias";
         if (socialHub.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (sError.Length > 0)
            throw new Exception("SocialHubInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "SocialHubUsuarioInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "SocialHubInsHlp: No pudo conectarse a la base de datos.", "GP.SocialEngine.DAO", 
         "SocialHubUsuarioInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO SocialHub (FechaRegistro,Alias,InformacionSocialID,SocialProfileType,UsuarioSocialID,ReactivoID, JuegoID) ");
         sCmd.Append(" VALUES ( ");
         sCmd.Append(" @socialHub_FechaRegistro ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "socialHub_FechaRegistro";
         if (socialHub.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = socialHub.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@socialHub_Alias ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "socialHub_Alias";
         if (socialHub.Alias == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = socialHub.Alias;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         
         sCmd.Append(" ,@socialHub_InformacionSocial_InformacionSocialID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "socialHub_InformacionSocial_InformacionSocialID";
         if (socialHub.InformacionSocial.InformacionSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = socialHub.InformacionSocial.InformacionSocialID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@socialHub_SocialProfileType ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "socialHub_SocialProfileType";
         if (socialHub.SocialProfileType == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = socialHub.SocialProfileType;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);

         if (socialHub.SocialProfile is UsuarioSocial)
         {
             sCmd.Append(" ,@dbp4ram1 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram1";
             if ((socialHub.SocialProfile as UsuarioSocial).UsuarioSocialID == null)
                 sqlParam.Value = DBNull.Value;
             else
                 sqlParam.Value = (socialHub.SocialProfile as UsuarioSocial).UsuarioSocialID;
             sqlParam.DbType = DbType.Int64;
             sqlCmd.Parameters.Add(sqlParam);
             sCmd.Append(" ,NULL, NULL ");
         }
         
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("SocialHubInsHlp: Se encontraron problemas al crear el registro.. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("SocialHubInsHlp: Se encontraron problemas al crear el registro..");
      }
   } 
}
