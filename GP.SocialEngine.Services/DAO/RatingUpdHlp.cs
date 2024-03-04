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
    /// Actualiza un registro de Rating en la BD
    /// </summary>
    public class RatingUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de RatingUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="ratingUpdHlp">RatingUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RatingUpdHlp que tiene los datos anteriores</param>
        public void Action(IDataContext dctx, Rating rating, Rating anterior)
        {
            object myFirm = new object();
            String sError = string.Empty;
            if (rating == null)
                sError += ", Rating";
            if (anterior == null)
                sError += ", Anterior";
            if (sError.Length > 0)
                throw new Exception("RatingUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (anterior.RatingID == null)
                sError += ", Anterior RatingID";
            if (sError.Length > 0)
                throw new Exception("RatingUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (rating.Puntuacion == null)
                sError += ", Puntuacion";
            if (rating.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (sError.Length > 0)
                throw new Exception("RatingInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "RatingUpdHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RatingUpdHlp: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO",
                   "RatingUpdHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" UPDATE Rating ");
            if (rating.Puntuacion == null)
                sCmd.Append(" SET Puntuacion = NULL ");
            else
            {
                sCmd.Append(" SET Puntuacion = @rating_Puntuacion ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "rating_Puntuacion";
                sqlParam.Value = rating.Puntuacion;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (rating.FechaRegistro != null)
            {
                sCmd.Append(" ,FechaRegistro = @rating_FechaRegistro ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "rating_FechaRegistro";
                sqlParam.Value = rating.FechaRegistro;
                sqlParam.DbType = DbType.DateTime;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (rating.UsuarioSocial.UsuarioSocialID != null)
            {
                sCmd.Append(" ,UsuarioSocialID = @rating_UsuarioSocial_UsuarioSocialID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "rating_UsuarioSocial_UsuarioSocialID";
                sqlParam.Value = rating.UsuarioSocial.UsuarioSocialID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if (anterior.RatingID == null)
                sCmd.Append(" WHERE RatingID IS NULL ");
            else
            {
                sCmd.Append(" WHERE RatingID = @anterior_RatingID ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "anterior_RatingID";
                sqlParam.Value = anterior.RatingID;
                sqlParam.DbType = DbType.Int64;
                sqlCmd.Parameters.Add(sqlParam);
            }
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
                throw new Exception("RatingUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RatingUpdHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
