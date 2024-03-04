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
   /// Crea un Privilegio en la base de datos
   /// </summary>
   public class PrivilegioInsHlp { 
      /// <summary>
      /// Crea un registro de Privilegio en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="privilegio">Privilegio que desea crear</param>
      public void Action(IDataContext dctx, UsuarioAcceso usuarioAcceso, int? perfilID, int? permisoID){
         object myFirm = new object();
         string sError = "";
         if (usuarioAcceso == null)
            sError += ", UsuarioAcceso";
         if (sError.Length > 0)
            throw new Exception("Error DA1915.- PrivilegioInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (perfilID == null && permisoID == null)
            sError += ", perfilID y permisoID";
         if (usuarioAcceso.UsuarioAccesoID == null)
            sError += ", UsuarioAcceso.UsuarioAccesoID";
         if (sError.Length > 0)
            throw new Exception("Error DA1916.- UsuarioInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (dctx == null)
            throw new Exception("Error DA1917.- PrivilegioInsertar: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1918.- UsuarioInsertar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO Privilegio(UsuarioAccesoID, PerfilID, PermisoID) ");
         sCmd.Append(" VALUES(@usuarioAcceso_UsuarioAccesoID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioAcceso_UsuarioAccesoID";
         if (usuarioAcceso.UsuarioAccesoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioAcceso.UsuarioAccesoID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@perfilID ");
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
            throw new Exception("Error DA1919.- UsuarioInsertar: Hubo un error al crear el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("Error DA1920.- UsuarioInsertar: Hubo un error al crear el registro.");
      }
   } 
}
