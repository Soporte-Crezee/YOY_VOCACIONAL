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
   /// Elimina todos los UsuarioAcceso de un UsuarioPrivilegios en la base de datos
   /// </summary>
   public class UsuarioAccesoUsuarioPrivilegiosDelHlp { 
      /// <summary>
      /// Elimina un registro de UsuarioAcceso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioAcceso">UsuarioAcceso que desea eliminar</param>
      public void Action(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios){
         object myFirm = new object();
         string sError = "";
         if (usuarioPrivilegios == null)
            sError += ", UsuarioPrivilegios";
         if (sError.Length > 0)
            throw new Exception("Error DA1932.- UsuarioAccesoEliminarPorUsuarioPrivilegios: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (usuarioPrivilegios.UsuarioPrivilegiosID == null)
            sError += ", UsuarioPrivilegiosID";
         if (sError.Length > 0)
            throw new Exception("Error DA1933.- UsuarioAccesoEliminarPorUsuarioPrivilegios: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (dctx == null)
            throw new Exception("Error DA1934.- UsuarioAccesoEliminarPorUsuarioPrivilegios: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1935.- UsuarioAccesoEliminarPorUsuarioPrivilegios: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM UsuarioAcceso ");
         if (usuarioPrivilegios.UsuarioPrivilegiosID == null)
            sCmd.Append(" WHERE UsuarioPrivilegiosID IS NULL ");
         else{ 
            sCmd.Append(" WHERE UsuarioPrivilegiosID = @usuarioPrivilegios_UsuarioPrivilegiosID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioPrivilegios_UsuarioPrivilegiosID";
            sqlParam.Value = usuarioPrivilegios.UsuarioPrivilegiosID;
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
            throw new Exception("Error DA1936.- UsuarioAccesoEliminarPorUsuarioPrivilegios: Hubo un error al eliminar el registro o no existe. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("Error DA1937.- UsuarioAccesoEliminarPorUsuarioPrivilegios: Hubo un error al eliminar el registro o no existe.");
      }
   } 
}
