using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Elimina un registro de InformacionSocial en la BD
   /// </summary>
   public class InformacionSocialDelHlp { 
      /// <summary>
      /// Elimina un registro de InformacionSocialDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="informacionSocialDelHlp">InformacionSocialDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, InformacionSocial informacionSocial){
         object myFirm = new object();
         string sError = String.Empty;
         if (informacionSocial == null)
            sError += ", InformacionSocial";
         if (sError.Length > 0)
            throw new Exception("InformacionSocialDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (informacionSocial.InformacionSocialID == null)
            sError += ", InformacionSocialID";
         if (sError.Length > 0)
            throw new Exception("InformacionSocialDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "InformacionSocialDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "InformacionSocialDelHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "InformacionSocialDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM InformacionSocial ");
         sCmd.Append(" WHERE InformacionSocialID=@informacionSocial_InformacionSocialID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "informacionSocial_InformacionSocialID";
         if (informacionSocial.InformacionSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = informacionSocial.InformacionSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("InformacionSocialDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("InformacionSocialDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
