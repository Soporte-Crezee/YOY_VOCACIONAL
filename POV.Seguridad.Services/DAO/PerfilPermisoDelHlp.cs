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
   /// Elimina un PerfilPermiso en la base de datos
   /// </summary>
   public class PerfilPermisoDelHlp { 
      /// <summary>
      /// Elimina un registro de PerfilPermiso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="perfilPermiso">PerfilPermiso que desea eliminar</param>
      public void Action(IDataContext dctx, int? perfilPermisoID){
         object myFirm = new object();
         string sError = "";
         if (perfilPermisoID == null)
            sError += ", perfilPermisoID";
         if (sError.Length > 0)
            throw new Exception("Error DA1872.- PerfilPermisoEliminar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (dctx == null)
            throw new Exception("Error DA1873.- PerfilPermisoEliminar: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1874.- PerfilPermisoEliminar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM PerfilPermiso ");
         if (perfilPermisoID == null)
            sCmd.Append(" WHERE PerfilPermisoID IS NULL ");
         else{ 
            sCmd.Append(" WHERE PerfilPermisoID = @perfilPermisoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "perfilPermisoID";
            sqlParam.Value = perfilPermisoID;
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
            throw new Exception("Error DA1875.- PerfilPermisoEliminar: Hubo un error al eliminar el registro o no existe. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("Error DA1876.- PerfilPermisoEliminar: Hubo un error al eliminar el registro o no existe.");
      }
   } 
}
