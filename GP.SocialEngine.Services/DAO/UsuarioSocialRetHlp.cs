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
   /// Consultar Usuario Social en la base de datos
   /// </summary>
   public class UsuarioSocialRetHlp { 
      /// <summary>
      /// Consulta registros de usuarioSocial en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocial">usuarioSocial que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de usuarioSocial generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, UsuarioSocial usuarioSocial){
         object myFirm = new object();
         string sError = String.Empty;
         if (usuarioSocial == null)
            sError += ", usuarioSocial";
         if (sError.Length > 0)
            throw new Exception("UsuarioSocialRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "UsuarioSocialRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UsuarioSocialRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "UsuarioSocialRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT UsuarioSocialID, Email, LoginName, ScreenName, Estatus,FechaNacimiento ");
         sCmd.Append(" FROM UsuarioSocial ");
        
         StringBuilder s_Var = new StringBuilder();
         if (usuarioSocial.UsuarioSocialID != null){
            s_Var.Append(" UsuarioSocialID= @usuarioSocial_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioSocial_UsuarioSocialID";
            sqlParam.Value = usuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioSocial.Email != null){
            s_Var.Append(" AND Email =@usuarioSocial_Email ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioSocial_Email";
            sqlParam.Value = usuarioSocial.Email;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioSocial.LoginName != null){
            s_Var.Append(" AND LoginName= @usuarioSocial_LoginName ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioSocial_LoginName";
            sqlParam.Value = usuarioSocial.LoginName;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioSocial.ScreenName != null){
            s_Var.Append(" AND ScreenName= @usuarioSocial_ScreenName ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioSocial_ScreenName";
            sqlParam.Value = usuarioSocial.ScreenName;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioSocial.Estatus != null){
            s_Var.Append(" AND Estatus= @usuarioSocial_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioSocial_Estatus";
            sqlParam.Value = usuarioSocial.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioSocial.FechaNacimiento != null)
         {
             s_Var.Append(" AND FechaNacimiento=@usuarioSocial_FechaNacimiento");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "@usuarioSocial_FechaNacimiento";
             sqlParam.Value = usuarioSocial.FechaNacimiento;
             sqlParam.DbType = DbType.DateTime;
             sqlCmd.Parameters.Add(sqlParam);
         }
          s_Var.Append("  ");
         string s_Varres = s_Var.ToString().Trim();
         if (s_Varres.Length > 0){
            if (s_Varres.StartsWith("AND "))
               s_Varres = s_Varres.Substring(4);
            else if (s_Varres.StartsWith("OR "))
               s_Varres = s_Varres.Substring(3);
            else if (s_Varres.StartsWith(","))
               s_Varres = s_Varres.Substring(1);
            sCmd.Append(" WHERE " + s_Varres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "usuarioSocial");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("UsuarioSocialRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
