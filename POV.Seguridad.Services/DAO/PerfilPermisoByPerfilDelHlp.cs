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
   /// Elimina todos los PerfilPermiso de un Perfil en la base de datos
   /// </summary>
   public class PerfilPermisoByPerfilDelHlp { 
      /// <summary>
      /// Elimina un registro de PerfilPermiso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="perfilPermiso">PerfilPermiso que desea eliminar</param>
      public void Action(IDataContext dctx, int? perfilID){
         object myFirm = new object();
         if (dctx == null)
            throw new Exception("Error DA1877.- PerfilPermisoEliminarPorPerfil: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1878.- PerfilPermisoEliminarPorPerfil: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM PerfilPermiso ");
         if (perfilID == null)
            sCmd.Append(" WHERE PerfilID IS NULL ");
         else{ 
            sCmd.Append(" WHERE PerfilID = @perfilID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "perfilID";
            sqlParam.Value = perfilID;
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
            throw new Exception("Error DA1879.- PerfilPermisoEliminarPorPerfil: Hubo un error al eliminar el registro o no existe. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
      }
   } 
}
