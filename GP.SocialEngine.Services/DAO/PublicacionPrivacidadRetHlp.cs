using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Consulta un registro de privacidades de una Publicacion en la BD
   /// </summary>
   public class PublicacionPrivacidadRetHlp { 
      /// <summary>
      /// Consulta registros de Publicacion en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="publicacion">Publicacion que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Publicacion generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Publicacion publicacion){
         object myFirm = new object();
         string sError = String.Empty;
         if (publicacion == null)
            sError += ", Publicacion";
         if (sError.Length > 0)
            throw new Exception("PublicacionPrivacidadRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (publicacion.PublicacionID == null)
            sError += ", PublicacionID";
         if (sError.Length > 0)
            throw new Exception("PublicacionPrivacidadRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "PublicacionPrivacidadRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PublicacionPrivacidadRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "PublicacionPrivacidadRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT PublicacionID, UsuarioSocialID, GrupoSocialID ");
         sCmd.Append(" FROM Privacidad ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (publicacion.PublicacionID != null){
            s_VarWHERE.Append(" PublicacionID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = publicacion.PublicacionID;
            sqlParam.DbType = DbType.Guid;
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
            sqlAdapter.Fill(ds, "Privacidad");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PublicacionPrivacidadRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
