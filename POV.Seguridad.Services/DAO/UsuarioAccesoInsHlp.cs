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
   /// Crea un UsuarioAcceso en la base de datos
   /// </summary>
   public class UsuarioAccesoInsHlp { 
      /// <summary>
      /// Crea un registro de UsuarioAcceso en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioAcceso">UsuarioAcceso que desea crear</param>
      public void Action(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios, UsuarioAcceso usuarioAcceso){
         object myFirm = new object();
         string sError = "";
         if (usuarioAcceso == null)
            sError += ", UsuarioAcceso";
         if (usuarioPrivilegios == null)
            sError += ", UsuarioPrivilegios";
         if (sError.Length > 0)
            throw new Exception("Error DA1938.- UsuarioAccesoInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (usuarioAcceso.UsuarioAsigno == null)
             usuarioAcceso.UsuarioAsigno = new Usuario();
         if (usuarioAcceso.Privilegio == null)
            sError += ", Privilegio";
         if (usuarioPrivilegios.UsuarioPrivilegiosID == null)
            sError += ", UsuarioPrivilegios.UsuarioPrivilegiosID";
         if (sError.Length > 0)
            throw new Exception("Error DA1939.- UsuarioAccesoInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (usuarioAcceso.FechaAsignacion == null)
            sError += ", FechaAsignacion";
         if (usuarioAcceso.Privilegio.PrivilegioID == null)
            sError += ", Privilegio.PrivilegioID";
         if (sError.Length > 0)
            throw new Exception("Error DA1940.- UsuarioAccesoInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (dctx == null)
            throw new Exception("Error DA1941.- UsuarioAccesoInsertar: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1942.- UsuarioAccesoInsertar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO UsuarioAcceso(UsuarioPrivilegiosID, FechaAsignacion, UsuarioAsignoID) ");
         sCmd.Append(" VALUES(@usuarioPrivilegios_UsuarioPrivilegiosID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioPrivilegios_UsuarioPrivilegiosID";
         if (usuarioPrivilegios.UsuarioPrivilegiosID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioPrivilegios.UsuarioPrivilegiosID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@usuarioAcceso_FechaAsignacion ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioAcceso_FechaAsignacion";
         if (usuarioAcceso.FechaAsignacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioAcceso.FechaAsignacion;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@usuarioAcceso_UsuarioAsigno_UsuarioID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioAcceso_UsuarioAsigno_UsuarioID";
         if (usuarioAcceso.UsuarioAsigno.UsuarioID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioAcceso.UsuarioAsigno.UsuarioID;
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
            throw new Exception("Error DA1943.- UsuarioAccesoInsertar: Hubo un error al crear el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("Error DA1944.- UsuarioAccesoInsertar: Hubo un error al crear el registro.");
      }
   } 
}
