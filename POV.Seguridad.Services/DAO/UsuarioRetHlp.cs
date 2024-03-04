using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;
using GP.SocialEngine.BO;

namespace POV.Seguridad.DAO { 
   /// <summary>
   /// Consultar usuarios de la base de datos
   /// </summary>
   public class UsuarioRetHlp { 
      /// <summary>
      /// Consulta registros de usuario en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuario">usuario que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de usuario generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Usuario usuario, bool isService = false){
         object myFirm = new object();
         string sError = String.Empty;
         if (usuario == null)
            sError += ", Usuario";
         if (sError.Length > 0)
            throw new Exception("UsuarioRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (usuario.Termino == null) {
         usuario.Termino = new Termino();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Seguridad.DAO", 
         "UsuarioRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UsuarioRetHlp: No se pudo conectar a la base de datos", "POV.Seguridad.DAO", 
         "UsuarioRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT ");
         sCmd.Append(" UsuarioID, NombreUsuario, Password, EsActivo, FechaCreacion, FechaUltimoAcceso, FechaUltimoCambioPassword, Comentario, PasswordTemp, Email, ");
         sCmd.Append(" EmailAlternativo, EmailVerificado, AceptoTerminos, TerminoID, Tipo, TelefonoReferencia, GuidPeticionJuego, FechaPeticionJuego, TelefonoCasa, UniversidadID ");
         sCmd.Append(" FROM Usuario ");
         StringBuilder s_Var = new StringBuilder();
         if (usuario.UsuarioID != null){
            s_Var.Append(" UsuarioID= @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = usuario.UsuarioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuario.NombreUsuario != null){
            s_Var.Append(" AND NombreUsuario = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = usuario.NombreUsuario;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuario.Password != null){
            s_Var.Append(" AND Password= @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = usuario.Password;
            sqlParam.DbType = DbType.Binary;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuario.EsActivo != null){
            s_Var.Append(" AND EsActivo= @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = usuario.EsActivo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuario.FechaCreacion != null){
            s_Var.Append(" AND FechaCreacion= @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = usuario.FechaCreacion;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuario.FechaUltimoAcceso != null){
             if(isService)
                 s_Var.Append(" AND FechaUltimoAcceso < @dbp4ram6 ");
             else
                s_Var.Append(" AND FechaUltimoAcceso = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = usuario.FechaUltimoAcceso;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuario.FechaUltimoCambioPassword != null){
            s_Var.Append(" AND FechaUltimoCambioPassword= @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = usuario.FechaUltimoCambioPassword;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuario.EmailAlternativo != null){
            s_Var.Append(" AND EmailAlternativo = @dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            sqlParam.Value = usuario.EmailAlternativo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuario.PasswordTemp != null){
            s_Var.Append(" AND PasswordTemp= @dbp4ram9 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram9";
            sqlParam.Value = usuario.PasswordTemp;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuario.Email != null){
            s_Var.Append(" AND Email= @dbp4ram10 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram10";
            sqlParam.Value = usuario.Email;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuario.EmailVerificado != null){
            s_Var.Append(" AND EmailVerificado= @dbp4ram11 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram11";
            sqlParam.Value = usuario.EmailVerificado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuario.AceptoTerminos != null){
            s_Var.Append(" AND AceptoTerminos= @dbp4ram12 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram12";
            sqlParam.Value = usuario.AceptoTerminos;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuario.Termino.TerminoID != null){
            s_Var.Append(" AND TerminoID= @dbp4ram13 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram13";
            sqlParam.Value = usuario.Termino.TerminoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuario.TelefonoReferencia != null){
            s_Var.Append(" AND TelefonoReferencia= @dbp4ram15 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram15";
            sqlParam.Value = usuario.TelefonoReferencia;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }

         if (usuario.TelefonoCasa != null)
         {
             s_Var.Append(" AND TelefonoCasa= @dbp4ram16 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram16";
             sqlParam.Value = usuario.TelefonoCasa;
             sqlParam.DbType = DbType.String;
             sqlCmd.Parameters.Add(sqlParam);
         }

         if (usuario.UniversidadId != null)
         {
             s_Var.Append(" AND UniversidadId= @dbp4ram17 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram17";
             sqlParam.Value = usuario.UniversidadId;
             sqlParam.DbType = DbType.Int64;
             sqlCmd.Parameters.Add(sqlParam);
         }

          s_Var.Append("  ");
         string s_Varres = s_Var.ToString().Trim();
         if (s_Varres.Length > 0){
            if (s_Varres.StartsWith("AND "))
               s_Varres = s_Varres.Substring(4);
            else if (s_Varres.StartsWith("OR "))
               s_Varres = s_Varres.Substring(3);
            else if (s_Varres.StartsWith(","))
               s_Varres = s_Varres.Substring(1);
            sCmd.Append(" WHERE " + s_Varres);
         }

         sCmd.Append(" ORDER BY NombreUsuario ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "Usuario");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("UsuarioRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
