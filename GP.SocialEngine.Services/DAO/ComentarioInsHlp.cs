using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using POV.ContenidosDigital.BO;
using POV.Reactivos.BO;

namespace GP.SocialEngine.DAO
{
    /// <summary>
    /// Crea un comentario de una Publicación en la base de datos
    /// </summary>
    public class ComentarioInsHlp
    {
        /// <summary>
        /// Crea un registro de Comentario en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="comentario">Comentario que desea crear</param>
        public void Action(IDataContext dctx, Comentario comentario, Publicacion publicacion)
        {
            object myFirm = new object();
            String sError = String.Empty;
            if (publicacion == null)
                sError += ", Publicacion";
            if (sError.Length > 0)
                throw new Exception("ComentarioInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
            if (comentario == null)
                sError += ", Comentario";
            if (sError.Length > 0)
                throw new Exception("ComentarioInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
            if (comentario.Ranking == null)
                sError += ", Ranking";
            if (sError.Length > 0)
                throw new Exception("ComentarioInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
            if (comentario.Ranking.RankingID == null)
                sError += ", RankingID";
            if (comentario.UsuarioSocial == null)
                sError += ", UsuarioSocial";
            if (sError.Length > 0)
                throw new Exception("ComentarioInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
            if (comentario.UsuarioSocial.UsuarioSocialID == null)
                sError += ", UsuarioSocialID";
            if (comentario.Cuerpo == null || comentario.Cuerpo.Trim().Length == 0)
                sError += ", Cuerpo";
            if (comentario.FechaComentario == null)
                sError += ", Fecha de Comentario";
            if (comentario.Estatus == null)
                sError += ", Estatus";
            if (sError.Length > 0)
                throw new Exception("ComentarioInsHlp: Los siguientes campos no puede ser vacios: " + sError.Substring(2) + " ");
            if (comentario.TipoPublicacion == null)
                sError += ", TipoPublicacion";
            if (sError.Length > 0)
                throw new Exception("ComentarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

            if (comentario.TipoPublicacion != ETipoPublicacion.TEXTO)
            {
                if (comentario.AppSocial == null)
                    sError += ", AppSocial";
                if (sError.Length > 0)
                    throw new Exception("ComentarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));

                if (comentario.AppSocial is Reactivo)
                {
                    if ((comentario.AppSocial as Reactivo).ReactivoID == null)
                        sError += ", Reactivo.ReactivoID";
                }
                else if (comentario.AppSocial is ContenidoDigital)
                {
                    if ((comentario.AppSocial as ContenidoDigital).ContenidoDigitalID == null)
                        sError += ", ContenidoDigital.ContenidoDigitalID";
                }

                if (sError.Length > 0)
                    throw new Exception("ComentarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            }
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "ComentarioInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "ComentarioInsHlp: No pudo conectarse a la base de datos.", "GP.SocialEngine.DAO",
                   "ComentarioInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Comentario (ComentarioID, Cuerpo, FechaComentario,Estatus,UsuarioSocialID,RankingID,PublicacionID,TipoPublicacion,ReactivoID,JuegoID,ContenidoDigitalID) ");
            sCmd.Append(" VALUES ( ");
            sCmd.Append(" @comentario_ComentarioID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_ComentarioID";
            if (comentario.ComentarioID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = comentario.ComentarioID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@comentario_Cuerpo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_Cuerpo";
            if (comentario.Cuerpo == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = comentario.Cuerpo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@comentario_FechaComentario ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_FechaComentario";
            if (comentario.FechaComentario == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = comentario.FechaComentario;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@comentario_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_Estatus";
            if (comentario.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = comentario.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@comentario_UsuarioSocial_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_UsuarioSocial_UsuarioSocialID";
            if (comentario.UsuarioSocial.UsuarioSocialID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = comentario.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@comentario_Ranking_RankingID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_Ranking_RankingID";
            if (comentario.Ranking.RankingID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = comentario.Ranking.RankingID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@publicacion_PublicacionID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "publicacion_PublicacionID";
            if (publicacion.PublicacionID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = publicacion.PublicacionID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);

            sCmd.Append(" ,@comentario_TipoPublicacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "comentario_TipoPublicacion";
            if (comentario.TipoPublicacion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = comentario.TipoPublicacion;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);

            if (comentario.TipoPublicacion == ETipoPublicacion.TEXTO)
            {
                sCmd.Append(" ,NULL,NULL,NULL ");
            }
            else
            {
                if (comentario.AppSocial is Reactivo)
                {
                    sCmd.Append(" ,@comentario_AppKey,NULL,NULL ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "comentario_AppKey";
                    sqlParam.Value = (comentario.AppSocial as Reactivo).ReactivoID;
                    sqlParam.DbType = DbType.Guid;
                    sqlCmd.Parameters.Add(sqlParam);
                }
               else if (comentario.AppSocial is ContenidoDigital)
                {
                    sCmd.Append(" ,NULL,NULL,@comentario_AppKey ");
                    sqlParam = sqlCmd.CreateParameter();
                    sqlParam.ParameterName = "comentario_AppKey";
                    sqlParam.Value = (comentario.AppSocial as ContenidoDigital).ContenidoDigitalID;
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
                throw new Exception("ComentarioInsHlp: Se encontraron problemas al crear el registro.. " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("ComentarioInsHlp: Se encontraron problemas al crear el registro..");
        }
    }
}
