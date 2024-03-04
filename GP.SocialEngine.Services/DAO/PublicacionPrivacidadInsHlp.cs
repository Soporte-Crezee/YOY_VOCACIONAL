using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO
{
    /// <summary>
    /// Inserta la privacidad de una publicacion
    /// </summary>
    public class PublicacionPrivacidadInsHlp
    {
        /// <summary>
        /// Crea un registro de privacidad de una publicacion en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="publicacion">Publicacion que desea crear</param>
        /// <param name="Privacidad">Privacidad de la publicacion</param>
        public void Action(IDataContext dctx, Publicacion publicacion,IPrivacidad privacidad)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (privacidad == null)
                sError += ", IPrivacidad";
            if (sError.Length > 0)
                throw new Exception("PublicacionPrivacidadInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (publicacion == null)
                sError += ", Publicacion";
            if (sError.Length > 0)
                throw new Exception("PublicacionPrivacidadInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (publicacion.PublicacionID == null)
                sError += ", PublicacionID";
            if (sError.Length > 0)
                throw new Exception("PublicacionPrivacidadInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            GrupoSocial grupoSocial = privacidad is GrupoSocial ? (GrupoSocial)privacidad : null;
            UsuarioSocial usuarioSocial = privacidad is UsuarioSocial ? (UsuarioSocial)privacidad : null;
            if (grupoSocial != null)
                if (grupoSocial.GrupoSocialID == null)
                    sError += ", GrupoSocialID";
            if (sError.Length > 0)
                throw new Exception("PublicacionPrivacidadInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (usuarioSocial != null)
                if (usuarioSocial.UsuarioSocialID == null)
                    sError += ", UsuarioSocialID";
            if (sError.Length > 0)
                throw new Exception("PublicacionPrivacidadInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "PublicacionPrivacidadInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PublicacionPrivacidadInsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO",
                   "PublicacionPrivacidadInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Privacidad (PublicacionID, UsuarioSocialID, GrupoSocialID) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            // publicacion.PublicacionID
            sCmd.Append(" @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            if (publicacion.PublicacionID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = publicacion.PublicacionID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);

            // privacidad para usuarios
            sCmd.Append(" ,@dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            if (usuarioSocial == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = usuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);

            // privacidad para grupos
            sCmd.Append(" ,@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            if (grupoSocial == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = grupoSocial.GrupoSocialID;
            sqlParam.DbType = DbType.Int32;
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
                throw new Exception("PublicacionPrivacidadInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("PublicacionPrivacidadInsHlp: Ocurrio un error al ingresar el registro.");
        }

    }
}
