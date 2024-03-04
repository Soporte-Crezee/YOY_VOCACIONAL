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
   /// Consulta un registro de SocialHub en la BD
   /// </summary>
   public class RetriveFriendsHlp { 
      /// <summary>
      /// Consulta registros de SocialHub en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="socialHub">SocialHub que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de SocialHub generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, SocialHub socialHub){
         object myFirm = new object();
         string sError = String.Empty;
         if (socialHub == null)
            sError += ", SocialHub";
         if (sError.Length > 0)
            throw new Exception("RetriveFriendsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "RetriveFriendsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RetriveFriendsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "RetriveFriendsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT UsuarioGrupo.UsuarioSocialID ");
         sCmd.Append(" FROM SocialHub INNER JOIN ");
         sCmd.Append(" GrupoSocial ON SocialHub.SocialHubID = GrupoSocial.SocialHubID INNER JOIN ");
         sCmd.Append(" UsuarioGrupo ON GrupoSocial.GrupoSocialID = UsuarioGrupo.GrupoSocialID ");
         sCmd.Append(" WHERE ");
         StringBuilder s_Var = new StringBuilder();
         if (socialHub.SocialHubID != null){
            s_Var.Append(" SocialHub.SocialHubID = @socialHub_SocialHubID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "socialHub_SocialHubID";
            sqlParam.Value = socialHub.SocialHubID;
            sqlParam.DbType = DbType.Int32;
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
            sCmd.Append("  " + s_Varres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "SocialHub");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RetriveFriendsHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
