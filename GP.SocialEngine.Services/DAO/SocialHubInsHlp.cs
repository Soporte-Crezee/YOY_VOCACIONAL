using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Crea un socialHub en la base de datos
   /// </summary>
   public class SocialHubInsHlp { 
      /// <summary>
      /// Crea un registro de SocialHub en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="socialHub">SocialHub que desea crear</param>
      public void Action(IDataContext dctx, SocialHub socialHub){
         object myFirm = new object();
         String sError = String.Empty;

         if ( socialHub.InformacionSocial == null )
         {
             socialHub.InformacionSocial = new InformacionSocial( );
         }

         if (socialHub == null)
            sError += ", SocialHub";
         if (socialHub.InformacionSocial == null)
            sError += ", InformacionSocial";
         if (socialHub.InformacionSocial.InformacionSocialID == null)
            sError += ", InformacionSocial";
         if (socialHub.Alias == null || socialHub.Alias.Trim().Length == 0)
            sError += ", Alias";        

         if (socialHub.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (sError.Length > 0)
            throw new Exception("SocialHubInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "SocialHubInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "SocialHubInsHlp: No pudo conectarse a la base de datos.", "GP.SocialEngine.DAO", 
         "SocialHubInsHlp", "Action", null, null);
         }

         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO SocialHub (FechaRegistro,Alias,UsuarioSocialID,InformacionSocialID) ");
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
