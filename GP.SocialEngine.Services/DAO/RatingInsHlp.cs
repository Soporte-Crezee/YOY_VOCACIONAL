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
    /// Insertar un registro de Rating en la base de datos
    /// </summary>
    public class RatingInsHlp
    {
        /// <summary>
        /// Crea un registro de Rating en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="rating">Rating que desea crear</param>
        public void Action(IDataContext dctx, Rating rating)
        {
            object myFirm = new object();
            string sError = String.Empty;
            if (rating == null)
                sError += ", rating";
            if (sError.Length > 0)
                throw new Exception("RatingInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (rating.UsuarioSocial == null)
                sError += ", UsuarioSocial";
            if (sError.Length > 0)
                throw new Exception("RatingInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (rating.UsuarioSocial.UsuarioSocialID == null)
                sError += ", UsuarioSocialID";
            if (sError.Length > 0)
                throw new Exception("RatingInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (rating.Puntuacion == null)
                sError += ", Puntuacion";
            if (rating.FechaRegistro == null)
                sError += ", FechaRegistro";
            if (sError.Length > 0)
                throw new Exception("RatingInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
            if (dctx == null)
                throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO",
                   "RatingInsHlp", "Action", null, null);
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection(myFirm);
                sqlCmd = dctx.CreateCommand();
            }
            catch (Exception ex)
            {
                throw new StandardException(MessageType.Error, "", "RatingInsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO",
                   "RatingInsHlp", "Action", null, null);
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder();
            sCmd.Append(" INSERT INTO Rating (Puntuacion, FechaRegistro, UsuarioSocialID) ");
            sCmd.Append(" VALUES ");
            sCmd.Append(" ( ");
            sCmd.Append(" @rating_Puntuacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "rating_Puntuacion";
            if (rating.Puntuacion == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = rating.Puntuacion;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@rating_FechaRegistro ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "rating_FechaRegistro";
            if (rating.FechaRegistro == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = rating.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
            sCmd.Append(" ,@rating_UsuarioSocial_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "rating_UsuarioSocial_UsuarioSocialID";
            if (rating.UsuarioSocial.UsuarioSocialID == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = rating.UsuarioSocial.UsuarioSocialID;
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
                throw new Exception("RatingInsHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
            }
            finally
            {
                try { dctx.CloseConnection(myFirm); }
                catch (Exception) { }
            }
            if (iRes < 1)
                throw new Exception("RatingInsHlp: Hubo  un Error al Actualizar el Registro .");
        }
    }
}
