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
    /// Inserta un usuario en la base de datos
    /// </summary>
    public class UsuarioInsHlp
    {
        /// <summary>
        /// Crea un registro de usuario en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuario">usuario que desea crear</param>
        public void Action(IDataContext dctx, Usuario usuario)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (usuario == null)
                sError += ", Usuario";
            if (sError.Length > 0)
                throw new Exception("UsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (usuario.NombreUsuario == null || usuario.NombreUsuario.Trim().Length == 0)
                sError += ", NombreUsuario";
            if (usuario.Password == null)
                sError += ", Password";
            if (usuario.EsActivo == null)
                sError += ", EsActivo";
            if (usuario.FechaCreacion == null)
                sError += ", FechaCreacion";
            if (usuario.PasswordTemp == null)
                sError += ", PasswordTemp";
            if (sError.Length > 0)
                throw new Exception("UsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Seguridad.DAO", "UsuarioInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "UsuarioInsHlp: No se pudo conectar a la base de datos", "POV.Seguridad.DAO", "UsuarioInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Usuario (NombreUsuario, Password, EsActivo, FechaCreacion, FechaUltimoAcceso, FechaUltimoCambioPassword, Comentario, PasswordTemp, Email, EmailVerificado, EmailAlternativo, AceptoTerminos, TerminoID, TelefonoReferencia, TelefonoCasa, UniversidadID) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            sCmd.Append(" @usuario_NombreUsuario ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_NombreUsuario";
            if (usuario.NombreUsuario == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.NombreUsuario;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@usuario_Password ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_Password";
            if (usuario.Password == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.Password;
            sqlParam.DbType = DbType.Binary;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@usuario_EsActivo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_EsActivo";
            if (usuario.EsActivo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.EsActivo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@usuario_FechaCreacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_FechaCreacion";
            if (usuario.FechaCreacion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.FechaCreacion;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@usuario_FechaUltimoAcceso ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_FechaUltimoAcceso";
            if (usuario.FechaUltimoAcceso == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.FechaUltimoAcceso;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@usuario_FechaUltimoCambioPassword ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_FechaUltimoCambioPassword";
            if (usuario.FechaUltimoCambioPassword == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.FechaUltimoCambioPassword;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@usuario_Comentario ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_Comentario";
            if (usuario.Comentario == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.Comentario;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@usuario_PasswordTemp ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_PasswordTemp";
            if (usuario.PasswordTemp == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.PasswordTemp;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@usuario_Email ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_Email";
            if (usuario.Email == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.Email;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@usuario_EmailVerificado ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_EmailVerificado";
            if (usuario.EmailVerificado == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.EmailVerificado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@usuario_EmailAlternativo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_EmailAlternativo";
            if (usuario.EmailAlternativo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.EmailAlternativo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@usuario_AceptoTerminos ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_AceptoTerminos";
            if (usuario.AceptoTerminos == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.AceptoTerminos;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@usuario_Termino_TerminoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_Termino_TerminoID";
            if (usuario.Termino.TerminoID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.Termino.TerminoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            //Telefono Referencia
            sCmd.Append(" ,@usuario_TelefonoReferencia ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_TelefonoReferencia";
            if (usuario.TelefonoReferencia == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.TelefonoReferencia;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            //Telefono Casa
            sCmd.Append(" ,@usuario_TelefonoCasa ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_TelefonoCasa";
            if (usuario.TelefonoCasa == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.TelefonoCasa;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            //UniversidadId
            sCmd.Append(" ,@usuario_UniversidadID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuario_UniversidadID";
            if (usuario.UniversidadId == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuario.UniversidadId;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ) ");
            int iRes = 0;
            try
            {
                sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
                iRes = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
                throw new Exception("UsuarioInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("UsuarioInsHlp: Ocurrio un error al ingresar el registro.");
        }
    }
}
