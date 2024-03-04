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
   /// Elimina un Registro de UsuarioGrupo en la BD
   /// </summary>
   public class UsuarioGrupoDelHlp { 
      /// <summary>
      /// Elimina un registro de UsuarioGrupoDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioGrupoDelHlp">UsuarioGrupoDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, UsuarioGrupo usuarioGrupo){
         object myFirm = new object();
         string sError = String.Empty;
         if (usuarioGrupo == null)
            sError += ", UsuarioGrupo";
         if (sError.Length > 0)
            throw new Exception("UsuarioGrupoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (usuarioGrupo.UsuarioGrupoID == null)
            sError += ", UsuarioGrupo";
         if (sError.Length > 0)
            throw new Exception("UsuarioGrupoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "UsuarioGrupoDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UsuarioGrupoDelHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "UsuarioGrupoDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM UsuarioGrupo ");
         // usuarioGrupo.UsuarioGrupoID
         sCmd.Append(" WHERE UsuarioGrupoID= @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (usuarioGrupo.UsuarioGrupoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioGrupo.UsuarioGrupoID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("UsuarioGrupoDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("UsuarioGrupoDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
