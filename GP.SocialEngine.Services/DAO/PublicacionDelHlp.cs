using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Elimina un registro de Publicacion en la BD
   /// </summary>
   public class PublicacionDelHlp { 
      /// <summary>
      /// Elimina un registro de PublicacionDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="publicacionDelHlp">PublicacionDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, Publicacion publicacion){
         object myFirm = new object();
         string sError = String.Empty;
         if (publicacion == null)
            sError += ", Publicacion";
         if (sError.Length > 0)
            throw new Exception("PublicacionDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (publicacion.PublicacionID == null)
            sError += ", PublicacionID";
         if (sError.Length > 0)
            throw new Exception("PublicacionDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "PublicacionDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PublicacionDelHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "PublicacionDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM Publicacion ");
         sCmd.Append(" WHERE PublicacionID=@publicacion_PublicacionID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "publicacion_PublicacionID";
         if (publicacion.PublicacionID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = publicacion.PublicacionID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PublicacionDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PublicacionDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
