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
   /// Elimina un UsuarioAcceso en la base de datos
   /// </summary>
   public class UsuarioAccesoDelHlp { 
      /// <summary>
      /// Elimina un registro de UsuarioAcceso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioAcceso">UsuarioAcceso que desea eliminar</param>
      public void Action(IDataContext dctx, UsuarioAcceso usuarioAcceso){
         object myFirm = new object();
         string sError = "";
         if (usuarioAcceso == null)
            sError += ", UsuarioAcceso";
         if (sError.Length > 0)
            throw new Exception("Error DA1926.- UsuarioAccesoEliminar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (usuarioAcceso.UsuarioAccesoID == null)
            sError += ", UsuarioAccesoID";
         if (sError.Length > 0)
            throw new Exception("Error DA1927.- UsuarioAccesoEliminar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (dctx == null)
            throw new Exception("Error DA1928.- UsuarioAccesoEliminar: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1929.- UsuarioAccesoEliminar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM UsuarioAcceso ");
         if (usuarioAcceso.UsuarioAccesoID == null)
            sCmd.Append(" WHERE UsuarioAccesoID IS NULL ");
         else{ 
            sCmd.Append(" WHERE UsuarioAccesoID = @usuarioAcceso_UsuarioAccesoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioAcceso_UsuarioAccesoID";
            sqlParam.Value = usuarioAcceso.UsuarioAccesoID;
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
            throw new Exception("Error DA1930.- UsuarioAccesoEliminar: Hubo un error al eliminar el registro o no existe. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("Error DA1931.- UsuarioAccesoEliminar: Hubo un error al eliminar el registro o no existe.");
      }
   } 
}
