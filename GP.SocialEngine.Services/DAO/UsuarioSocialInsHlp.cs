// Clase motor de red social
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Guarda
   /// </summary>
   public class UsuarioSocialInsHlp { 
      /// <summary>
      /// Crea un registro de usuarioSocial en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocial">usuarioSocial que desea crear</param>
      public void Action(IDataContext dctx, UsuarioSocial usuarioSocial){
         object myFirm = new object();
         string sError = String.Empty;
         if (usuarioSocial == null)
            sError += ", usuarioSocial";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (usuarioSocial.LoginName == null)
            sError += ", LoginName";
         if (usuarioSocial.ScreenName == null)
            sError += ", ScreenName";
         if (usuarioSocial.Estatus == null)
            sError += ", Estatus";
         if (usuarioSocial.FechaNacimiento == null)
             sError += ", FechaNacimiento";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "UsuarioSocialInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UsuarioSocialInsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "UsuarioSocialInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO UsuarioSocial (Email,LoginName,ScreenName,Estatus,FechaNacimiento) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         sCmd.Append(" @usuarioSocial_Email ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioSocial_Email";
         if (usuarioSocial.Email == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioSocial.Email;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@usuarioSocial_LoginName ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioSocial_LoginName";
         if (usuarioSocial.LoginName == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioSocial.LoginName;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@usuarioSocial_ScreenName ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioSocial_ScreenName";
         if (usuarioSocial.ScreenName == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioSocial.ScreenName;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@usuarioSocial_Estatus ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioSocial_Estatus";
         if (usuarioSocial.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioSocial.Estatus;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@usuarioSocial_FechaNacimiento ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioSocial_FechaNacimiento";
         if (usuarioSocial.FechaNacimiento == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = usuarioSocial.FechaNacimiento;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("UsuarioSocialInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("UsuarioSocialInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
