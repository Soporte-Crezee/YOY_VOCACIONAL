using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Consultar Registros de Rating en la base de datos
   /// </summary>
   public class RatingRetHlp { 
      /// <summary>
      /// Consulta registros de RatingSocial en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ratingSocial">RatingSocial que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de RatingSocial generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Rating rating){
         object myFirm = new object();
         string sError = String.Empty;
         if (rating == null)
            sError += ", publicacion";
         if (sError.Length > 0)
            throw new Exception("RatingRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (rating.UsuarioSocial == null) {
         rating.UsuarioSocial = new UsuarioSocial();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "RatingRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RatingRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "RatingRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT RatingID, Puntuacion, FechaRegistro,UsuarioSocialID ");
         sCmd.Append(" FROM Rating ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (rating.RatingID != null){
            s_VarWHERE.Append(" Rating.RatingID= @rating_RatingID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "rating_RatingID";
            sqlParam.Value = rating.RatingID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (rating.Puntuacion != null){
            s_VarWHERE.Append(" AND Puntuacion= @rating_Puntuacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "rating_Puntuacion";
            sqlParam.Value = rating.Puntuacion;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (rating.FechaRegistro != null){
            s_VarWHERE.Append(" AND FechaRegistro = @rating_FechaRegistro ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "rating_FechaRegistro";
            sqlParam.Value = rating.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (rating.UsuarioSocial.UsuarioSocialID != null){
            s_VarWHERE.Append(" AND UsuarioSocialID = @rating_UsuarioSocial_UsuarioSocialID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "rating_UsuarioSocial_UsuarioSocialID";
            sqlParam.Value = rating.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            else if (s_VarWHEREres.StartsWith(","))
               s_VarWHEREres = s_VarWHEREres.Substring(1);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "Rating");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RatingRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
