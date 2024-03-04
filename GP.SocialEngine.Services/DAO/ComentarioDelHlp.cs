using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Elimina un registro de Comentario en la BD
   /// </summary>
   public class ComentarioDelHlp { 
      /// <summary>
      /// Elimina un registro de ComentarioDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="comentarioDelHlp">ComentarioDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, Comentario comentario,Publicacion publicacion){
         object myFirm = new object();
         string sError = String.Empty;
         if (comentario == null)
            sError += ", Comentario";
         if (sError.Length > 0)
            throw new Exception("ComentarioDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (comentario.ComentarioID == null)
            sError += ", ComentarioID";
         if (sError.Length > 0)
            throw new Exception("ComentarioDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (publicacion == null)
            sError += ", Publicacion";
         if (sError.Length > 0)
            throw new Exception("ComentarioDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (publicacion.PublicacionID == null)
            sError += ", PublicacionID";
         if (sError.Length > 0)
            throw new Exception("ComentarioDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "ComentarioDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ComentarioDelHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "ComentarioDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM Comentario ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (comentario.ComentarioID == null)
            s_VarWHERE.Append(" ComentarioID IS NULL ");
         else{ 
            s_VarWHERE.Append(" ComentarioID = @comentario_ComentarioID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_ComentarioID";
            sqlParam.Value = comentario.ComentarioID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (publicacion.PublicacionID == null)
            s_VarWHERE.Append(" AND PublicacionID IS NULL ");
         else{ 
            s_VarWHERE.Append(" AND PublicacionID = @publicacion_PublicacionID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_PublicacionID";
            sqlParam.Value = publicacion.PublicacionID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            else if (s_VarWHEREres.StartsWith(","))
               s_VarWHEREres = s_VarWHEREres.Substring(1);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ComentarioDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ComentarioDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
