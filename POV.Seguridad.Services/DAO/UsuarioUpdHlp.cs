using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;

namespace POV.Seguridad.DAO { 
   /// <summary>
   /// Actualiza un usuario en la base de datos
   /// </summary>
   public class UsuarioUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de usuario en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuario">usuario que tiene los datos nuevos</param>
      /// <param name="anterior">usuario que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Usuario usuario, Usuario anterior){
         object myFirm = new object();
         string sError = String.Empty;
         if (usuario == null)
            sError += ", Usuario";
         if (anterior == null)
            sError += ", anterior";
         if (sError.Length > 0)
            throw new Exception("UsuarioUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (usuario.UsuarioID != anterior.UsuarioID) {
         sError = "Los parametros no coinciden";
      }
         if (sError.Length > 0)
            throw new Exception("UsuarioUpdHlp: " + sError.Substring(2));
         if (usuario.UsuarioID == null)
            sError += ", UsuarioID";
         if (usuario.NombreUsuario == null)
            sError += ", NombreUsuario";
         if (usuario.Password == null)
            sError += ", Password";
         if (usuario.EsActivo == null)
            sError += ", EsActivo";
         if (anterior.UsuarioID == null)
            sError += ", Anterior UsuarioID";
         if (anterior.NombreUsuario == null)
            sError += ", Anterior NombreUsuario";
         if (anterior.Password == null)
            sError += ", Anterior Password";
         if (anterior.EsActivo == null)
            sError += ", Anterior EsActivo";
         if (sError.Length > 0)
            throw new Exception("UsuarioUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Seguridad.DAO", 
         "UsuarioUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UsuarioUpdHlp: Ocurrió un error al conectarse a la base de datos", "POV.Seguridad.DAO", 
         "UsuarioUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Usuario ");
         sCmd.Append(" SET ");
         if (usuario.NombreUsuario != null){
            sCmd.Append(" NombreUsuario = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = usuario.NombreUsuario;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
            sCmd.Append(" ,Email = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (usuario.Email == null)
            {
                sqlParam.Value = DBNull.Value;
            }
            else
            {
                sqlParam.Value = usuario.Email;
                sqlParam.DbType = DbType.String;
            }
            sqlCmd.Parameters.Add(sqlParam);
         // usuario.Password
         sCmd.Append(" ,Password =@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (usuario.Password == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuario.Password;
         sqlParam.DbType = DbType.Binary;
         sqlCmd.Parameters.Add(sqlParam);
         // usuario.EsActivo
         sCmd.Append(" ,EsActivo =@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (usuario.EsActivo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuario.EsActivo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // usuario.FechaUltimoAcceso
         sCmd.Append(" ,FechaUltimoAcceso =@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (usuario.FechaUltimoAcceso == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuario.FechaUltimoAcceso;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // usuario.FechaUltimoCambioPassword
         sCmd.Append(" ,FechaUltimoCambioPassword = @dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (usuario.FechaUltimoCambioPassword == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuario.FechaUltimoCambioPassword;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // usuario.Comentario
         sCmd.Append(" ,Comentario = @dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (usuario.Comentario == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuario.Comentario;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // usuario.PasswordTemp
         sCmd.Append(" ,PasswordTemp =@dbp4ram8 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram8";
         if (usuario.PasswordTemp == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuario.PasswordTemp;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // usuario.AceptoTerminos
         sCmd.Append(" ,AceptoTerminos=@dbp4ram9 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram9";
         if (usuario.AceptoTerminos == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuario.AceptoTerminos;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // usuario.EmailVerificado
         sCmd.Append(" ,EmailVerificado=@dbp4ram10 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram10";
         if (usuario.EmailVerificado == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuario.EmailVerificado;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
        
         // usuario.TelefonoReferencia
         sCmd.Append(" ,TelefonoReferencia=@dbp4ram11 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram11";
         if (usuario.TelefonoReferencia == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuario.TelefonoReferencia;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // usuario.EmailAlternativo
         sCmd.Append(" ,EmailAlternativo=@dbp4ram16 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram16";
         if (usuario.EmailAlternativo == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = usuario.EmailAlternativo;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         
          // usuario.TelefonoCasa
         sCmd.Append(" ,TelefonoCasa=@dbp4ram17 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram17";
         if (usuario.TelefonoCasa == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = usuario.TelefonoCasa;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);

         sCmd.Append(" WHERE ");
         if (anterior.UsuarioID != null){
            sCmd.Append(" UsuarioID = @dbp4ram12 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram12";
            sqlParam.Value = anterior.UsuarioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         sCmd.Append(" AND ");
         if (anterior.NombreUsuario != null){
            sCmd.Append(" NombreUsuario = @dbp4ram13 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram13";
            sqlParam.Value = anterior.NombreUsuario;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         sCmd.Append(" AND ");
         if (anterior.EsActivo != null){
            sCmd.Append(" EsActivo = @dbp4ram14 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram14";
            sqlParam.Value = anterior.EsActivo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         sCmd.Append(" AND ");
         if (anterior.PasswordTemp != null){
            sCmd.Append(" PasswordTemp=@dbp4ram15 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram15";
            sqlParam.Value = anterior.PasswordTemp;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("UsuarioUpdHlp: Ocurrio un error al actualizar el usuario o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("UsuarioUpdHlp: Ocurrio un error al actualizar el usuario o fue modificado mientras era editado.");
      }
   } 
}
