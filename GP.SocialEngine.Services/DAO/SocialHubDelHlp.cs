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
   /// Elimina un registro de SocialHub en la BD
   /// </summary>
   public class SocialHubDelHlp { 
      /// <summary>
      /// Elimina un registro de SocialHubDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="socialHubDelHlp">SocialHubDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, SocialHub socialHub){
         object myFirm = new object();
         string sError = String.Empty;
         if (socialHub == null)
            sError += ", SocialHub";
         if (sError.Length > 0)
            throw new Exception("SocialHubDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (socialHub.SocialHubID == null)
            sError += ", SocialHubID";
         if (sError.Length > 0)
            throw new Exception("SocialHubDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "SocialHubDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "SocialHubDelHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "SocialHubDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM SocialHub ");
         sCmd.Append(" WHERE SocialHubID=@socialHub_SocialHubID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "socialHub_SocialHubID";
         if (socialHub.SocialHubID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = socialHub.SocialHubID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("SocialHubDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("SocialHubDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
