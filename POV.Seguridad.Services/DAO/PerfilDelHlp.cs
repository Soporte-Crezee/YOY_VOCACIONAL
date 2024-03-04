using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;

namespace POV.Seguridad.DAO
{ 
   /// <summary>
   /// Elimina un Perfil en la base de datos
   /// </summary>
   public class PerfilDelHlp { 
      /// <summary>
      /// Elimina un registro de Perfil en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="perfil">Perfil que desea eliminar</param>
      public void Action(IDataContext dctx, Perfil perfil){
         object myFirm = new object();
         string sError = "";
         if (perfil == null)
            sError += ", Perfil";
         if (sError.Length > 0)
            throw new Exception("Error DA1854.- UsuarioEliminar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (perfil.PerfilID == null)
            sError += ", PerfilID";
         if (sError.Length > 0)
            throw new Exception("Error DA1855.- UsuarioEliminar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (dctx == null)
            throw new Exception("Error DA1856.- PerfilEliminar: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1857.- UsuarioEliminar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM Perfil ");
         if (perfil.PerfilID == null)
            sCmd.Append(" WHERE PerfilID IS NULL ");
         else{ 
            sCmd.Append(" WHERE PerfilID = @perfil_PerfilID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "perfil_PerfilID";
            sqlParam.Value = perfil.PerfilID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.ToString();
            iRes = sqlCmd.ExecuteNonQuery();
            dctx.CommitTransaction(myFirm);
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.RollbackTransaction(myFirm); } catch(Exception){ }
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("Error DA1857.- UsuarioEliminar: Hubo un error al eliminar el registro o no existe. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("Error DA1859.- UsuarioEliminar: Hubo un error al eliminar el registro o no existe.");
      }
   } 
}