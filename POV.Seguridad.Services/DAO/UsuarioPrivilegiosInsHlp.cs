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
   /// Crea un UsuarioPrivilegios en la base de datos
   /// </summary>
   public class UsuarioPrivilegiosInsHlp { 
      /// <summary>
      /// Crea un registro de UsuarioPrivilegios en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioPrivilegios">UsuarioPrivilegios que desea crear</param>
      public void Action(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios){
         object myFirm = new object();
         string sError = "";
         if (usuarioPrivilegios == null)
            sError += ", UsuarioPrivilegios";
         if (sError.Length > 0)
            throw new Exception("Error DA1983.- UsuarioPrivilegiosInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (usuarioPrivilegios.Usuario == null)
            sError += ", Usuario";
         if (sError.Length > 0)
            throw new Exception("Error DA1984.- UsuarioPrivilegiosInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (usuarioPrivilegios.Usuario.UsuarioID == null)
            sError += ", Usuario.UsuarioID";
         if (usuarioPrivilegios.FechaCreacion == null)
            sError += ", FechaCreacion";
         if (usuarioPrivilegios.Estado == null)
            sError += ", Estado";
         if (sError.Length > 0)
            throw new Exception("Error DA1985.- UsuarioPrivilegiosInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));

         if (usuarioPrivilegios is UsuarioEscolarPrivilegios)
         {
             if ((usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela == null)
             {
                 sError += ", Escuela";
                 throw new Exception("Error DA1985.- UsuarioPrivilegiosInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
             }
             else if ((usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela.EscuelaID == null)
             {
                 sError += ", EscuelaID";
                 throw new Exception("Error DA1985.- UsuarioPrivilegiosInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
             }

             if ((usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar == null)
             {
                 sError += ", CicloEscolar";
                 throw new Exception("Error DA1985.- UsuarioPrivilegiosInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
             }
             else if ((usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar.CicloEscolarID == null)
             {
                 sError += ", CicloEscolarID";
                 throw new Exception("Error DA1985.- UsuarioPrivilegiosInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
             }
         }
         if (dctx == null)
            throw new Exception("Error DA1986.- UsuarioPrivilegiosInsertar: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1987.- UsuarioPrivilegiosInsertar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO UsuarioPrivilegios(UsuarioID, FechaCreacion, Estado, EscuelaID, CicloEscolarID) ");
         sCmd.Append(" VALUES(@usuarioPrivilegios_Usuario_UsuarioID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioPrivilegios_Usuario_UsuarioID";
         if (usuarioPrivilegios.Usuario.UsuarioID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioPrivilegios.Usuario.UsuarioID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);

         sCmd.Append(" ,@usuarioPrivilegios_FechaCreacion ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioPrivilegios_FechaCreacion";
         if (usuarioPrivilegios.FechaCreacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioPrivilegios.FechaCreacion;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);

         sCmd.Append(" ,@usuarioPrivilegios_Estado ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "usuarioPrivilegios_Estado";
         if (usuarioPrivilegios.Estado == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioPrivilegios.Estado;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);

         if (usuarioPrivilegios is UsuarioEscolarPrivilegios)
         {
             //escuela
             sCmd.Append(" ,@dbp4ram4 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram4";
             if ((usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela.EscuelaID == null)
                 sqlParam.Value = DBNull.Value;
             else
                 sqlParam.Value = (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela.EscuelaID;
             sqlParam.DbType = DbType.Int32;
             sqlCmd.Parameters.Add(sqlParam);
             //ciclo escolar
             sCmd.Append(" ,@dbp4ram5 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram5";
             if ((usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar.CicloEscolarID == null)
                 sqlParam.Value = DBNull.Value;
             else
                 sqlParam.Value = (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar.CicloEscolarID;
             sqlParam.DbType = DbType.Int32;
             sqlCmd.Parameters.Add(sqlParam);
         }
         else
         {
             sCmd.Append(" , NULL, NULL ");
         }
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
            throw new Exception("Error DA1988.- UsuarioPrivilegiosInsertar: Hubo un error al crear el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("Error DA1989.- UsuarioPrivilegiosInsertar: Hubo un error al crear el registro.");
      }
   } 
}
