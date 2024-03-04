using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;

namespace POV.Seguridad.DAO { 
   /// <summary>
   /// Elimina un registro de Usuario en la BD
   /// </summary>
   public class UsuarioDelHlp { 
      /// <summary>
      /// Elimina un registro de UsuarioDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioDelHlp">UsuarioDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, Usuario usuario){
         object myFirm = new object();
         string sError = String.Empty;
         if (usuario == null)
            sError += ", Usuario";
         if (sError.Length > 0)
            throw new Exception("UsuarioDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (usuario.UsuarioID == null)
            sError += ", UsuarioID";
         if (sError.Length > 0)
            throw new Exception("UsuarioDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Seguridad.DAO", 
         "UsuarioDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UsuarioDelHlp: No se pudo conectar a la base de datos", "POV.Seguridad.DAO", 
         "UsuarioDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM Usuario ");
         if (usuario.UsuarioID == null)
            sCmd.Append(" WHERE UsuarioID IS NULL ");
         else{ 
            sCmd.Append(" WHERE UsuarioID = @usuario_UsuarioID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_UsuarioID";
            sqlParam.Value = usuario.UsuarioID;
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
            throw new Exception("UsuarioDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("UsuarioDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
