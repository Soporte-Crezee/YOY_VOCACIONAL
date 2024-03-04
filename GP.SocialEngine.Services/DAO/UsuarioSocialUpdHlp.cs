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
   /// Actualiza los datos de un UsuarioSocial
   /// </summary>
   public class UsuarioSocialUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de Actualiza los datos de un UsuarioSocial en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="actualizalosdatosdeunUsuarioSocial">Actualiza los datos de un UsuarioSocial que tiene los datos nuevos</param>
      /// <param name="anterior">Actualiza los datos de un UsuarioSocial que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, UsuarioSocial usuarioSocial, UsuarioSocial anterior){
         object myFirm = new object();
         string sError = String.Empty;
         if (usuarioSocial == null)
            sError += ", UsuarioSocial";
         if (anterior == null)
            sError += ", anterior";
         if (sError.Length > 0)
            throw new Exception("usuarioSocialUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (usuarioSocial.Email == null)
            sError += ", Email";
         if (usuarioSocial.LoginName == null)
            sError += ", LoginName";
         if (usuarioSocial.ScreenName == null)
            sError += ", ScreenName";
         if (usuarioSocial.Estatus == null)
            sError += ", Estatus";
         if (anterior.Email == null)
            sError += ", anterior.Email";
         if (anterior.LoginName == null)
            sError += ", anterior.LoginName";
         if (anterior.ScreenName == null)
            sError += ", anterior.ScreenName";
         if (anterior.Estatus == null)
            sError += ", anterior.Estatus";
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "UsuarioSocialUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "usuarioSocialUpdHlp: Ocurrió un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "UsuarioSocialUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE usuarioSocial SET ");
         sCmd.Append(" Email =@usuarioSocial_Email ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioSocial_Email";
         if (usuarioSocial.Email == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioSocial.Email;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,LoginName = @usuarioSocial_LoginName ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioSocial_LoginName";
         if (usuarioSocial.LoginName == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioSocial.LoginName;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,ScreenName = @usuarioSocial_ScreenName ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioSocial_ScreenName";
         if (usuarioSocial.ScreenName == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioSocial.ScreenName;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,Estatus=@usuarioSocial_Estatus ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioSocial_Estatus";
         if (usuarioSocial.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioSocial.Estatus;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         
         sCmd.Append(" WHERE ");
         if (anterior.UsuarioSocialID != null){
            sCmd.Append(" UsuarioSocialID= @anterior_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_UsuarioSocialID";
            sqlParam.Value = anterior.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("usuarioSocialUpdHlp: Ocurrio un error al actualizar el usuarioSocial o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("usuarioSocialUpdHlp: Ocurrio un error al actualizar el usuarioSocial o fue modificado mientras era editado.");
      }
   } 
}
