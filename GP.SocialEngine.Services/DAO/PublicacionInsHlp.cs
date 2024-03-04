using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using POV.Reactivos.BO;
using POV.ContenidosDigital.BO;

namespace GP.SocialEngine.DAO
{
    /// <summary>
    /// Insertar un registro de Publicacion en la base de datos
    /// </summary>
    public class PublicacionInsHlp
    {
        /// <summary>
        /// Crea un registro de Publicacion en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="publicacion">Publicacion que desea crear</param>
        public void Action(IDataContext dctx, Publicacion publicacion)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (publicacion == null)
                sError += ", publicacion";
            if (sError.Length > 0)
                throw new Exception("PublicacionInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
            if (publicacion.Ranking == null)
                sError += ", Ranking";
            if (publicacion.Ranking.RankingID == null)
                sError += ", RankingID";
            if (publicacion.UsuarioSocial == null)
                sError += ", UsuarioSocial";
            if (sError.Length > 0)
                throw new Exception("PublicacionInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
            if (publicacion.UsuarioSocial.UsuarioSocialID == null)
                sError += ", UsuarioSocialID";
            if (publicacion.SocialHub == null)
                sError += ", SocialHub";
            if (sError.Length > 0)
                throw new Exception("PublicacionInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
            if (publicacion.SocialHub.SocialHubID == null)
                sError += ", SocialHubID";
            if (sError.Length > 0)
                throw new Exception("PublicacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (publicacion.PublicacionID == null)
                sError += ", PublicacionID";
            if (publicacion.Contenido == null)
                sError += ", Contenido";
            if (publicacion.FechaPublicacion == null)
                sError += ", FechaPublicacion";
            if (publicacion.Estatus == null)
                sError += ", Estatus";
            if (publicacion.TipoPublicacion == null)
                sError += ", TipoPublicacion";
            if (sError.Length > 0)
                throw new Exception("PublicacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (publicacion.TipoPublicacion != ETipoPublicacion.TEXTO)
            {
                if (publicacion.AppSocial == null)
                    sError += ", AppSocial";
                if (sError.Length > 0)
                    throw new Exception("PublicacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

                if (publicacion.AppSocial is Reactivo) 
                { 
                    if ((publicacion.AppSocial as Reactivo).ReactivoID == null)
                        sError += ", Reactivo.ReactivoID";
                }
                else if (publicacion.AppSocial is ContenidoDigital)
                {
                    if ((publicacion.AppSocial as ContenidoDigital).ContenidoDigitalID == null)
                        sError += ", ContenidoDigital.ContenidoDigitalID";
                }

                if (sError.Length > 0)
                    throw new Exception("PublicacionInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "PublicacionInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "PublicacionInsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO",
                   "PublicacionInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO ");
            sCmd.Append(" Publicacion ");
            sCmd.Append(" (PublicacionID, Contenido, FechaPublicacion, Estatus, SocialHubID, UsuarioSocialID, RankingID,TipoPublicacion,AppSocialID,JuegoID,ContenidoDigitalID) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            sCmd.Append(" @publicacion_PublicacionID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_PublicacionID";
            if (publicacion.PublicacionID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = publicacion.PublicacionID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@publicacion_Contenido ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_Contenido";
            if (publicacion.Contenido == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = publicacion.Contenido;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@publicacion_FechaPublicacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_FechaPublicacion";
            if (publicacion.FechaPublicacion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = publicacion.FechaPublicacion;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@publicacion_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_Estatus";
            if (publicacion.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = publicacion.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@publicacion_SocialHub_SocialHubID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_SocialHub_SocialHubID";
            if (publicacion.SocialHub.SocialHubID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = publicacion.SocialHub.SocialHubID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@publicacion_UsuarioSocial_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_UsuarioSocial_UsuarioSocialID";
            if (publicacion.UsuarioSocial.UsuarioSocialID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = publicacion.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@publicacion_Ranking_RankingID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_Ranking_RankingID";
            if (publicacion.Ranking.RankingID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = publicacion.Ranking.RankingID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@publicacion_TipoPublicacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_TipoPublicacion";
            if (publicacion.TipoPublicacion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = publicacion.TipoPublicacion;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);


            if (publicacion.TipoPublicacion == ETipoPublicacion.TEXTO)
            {
                sCmd.Append(" ,NULL,NULL,NULL ");
            }
            else
            {
                if (publicacion.AppSocial is Reactivo)
                {
                    sCmd.Append(" ,@publicacion_AppKey,NULL,NULL ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "publicacion_AppKey";
                    sqlParam.Value = (publicacion.AppSocial as Reactivo).ReactivoID;
                    sqlParam.DbType = DbType.Guid;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                else if (publicacion.AppSocial is ContenidoDigital)
                {
                    sCmd.Append(" ,NULL,NULL,@publicacion_AppKey ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "publicacion_AppKey";
                    sqlParam.Value = (publicacion.AppSocial as ContenidoDigital).ContenidoDigitalID;
                    sqlParam.DbType = DbType.Int64;
                    sqlCmd.Parameters.Add(sqlParam);
                }
                else
                    throw new ArgumentException("PublicacionInsHlp: el tipo de appsocial no es soportado");
            }
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
                throw new Exception("PublicacionInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("PublicacionInsHlp: Ocurrio un error al ingresar el registro.");
        }
    }
}
