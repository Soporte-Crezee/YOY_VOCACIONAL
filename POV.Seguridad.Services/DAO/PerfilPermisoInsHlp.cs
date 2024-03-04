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
   /// Crea un PerfilPermiso en la base de datos
   /// </summary>
   public class PerfilPermisoInsHlp { 
      /// <summary>
      /// Crea un registro de PerfilPermiso en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="perfilPermiso">PerfilPermiso que desea crear</param>
      public void Action(IDataContext dctx, int? perfilID, int? permisoID){
         object myFirm = new object();
         string sError = "";
         if (perfilID == null)
            sError += ", perfilID";
         if (permisoID == null)
            sError += ", permisoID";
         if (sError.Length > 0)
            throw new Exception("Error DA1881.- PerfilPermisoInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (dctx == null)
            throw new Exception("Error DA1882.- PerfilPermisoInsertar: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1883.- PerfilPermisoInsertar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO PerfilPermiso(PerfilID, PermisoID) ");
         sCmd.Append(" VALUES(@perfilID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "perfilID";
         if (perfilID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = perfilID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@permisoID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "permisoID";
         if (permisoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = permisoID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.ToString();
            iRes = sqlCmd.ExecuteNonQuery();
            dctx.CommitTransaction(myFirm);
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.RollbackTransaction(myFirm); } catch(Exception){ }
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("Error DA1884.- PerfilPermisoInsertar: Hubo un error al crear el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("Error DA1885.- PerfilPermisoInsertar: Hubo un error al crear el registro.");
      }
   } 
}
