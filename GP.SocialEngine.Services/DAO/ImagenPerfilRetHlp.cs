using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;
using POV.Comun.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Consulta un registro de ImagenPerfil en la BD
   /// </summary>
   public class ImagenPerfilRetHlp { 
      /// <summary>
      /// Consulta registros de ImagenPerfil en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="imagenPerfil">ImagenPerfil que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de ImagenPerfil generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, UsuarioSocial usuarioSocial, ImagenPerfil imagenPerfil){
         object myFirm = new object();
         string sError = string.Empty;
         if (imagenPerfil == null)
            sError += ", ImagenPerfil";
         if (sError.Length > 0)
            throw new Exception("ImagenPerfilInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (usuarioSocial == null)
            sError += ", UsuarioSocial";
         if (sError.Length > 0)
            throw new Exception("ImagenPerfilInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (usuarioSocial.UsuarioSocialID == null)
            sError += ", UsuarioSocial";
         if (sError.Length > 0)
            throw new Exception("ImagenPerfilInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (imagenPerfil.AdjuntoImagen == null) {
         imagenPerfil.AdjuntoImagen = new AdjuntoImagen();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "ImagenPerfilRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ImagenPerfilRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "ImagenPerfilRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT UsuarioSocialID, AdjuntoImagenID, Estatus ");
         sCmd.Append(" FROM ImagenPerfilUsuario ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (usuarioSocial.UsuarioSocialID != null){
            s_VarWHERE.Append(" UsuarioSocialID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = usuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (imagenPerfil.AdjuntoImagen.AdjuntoImagenID != null){
            s_VarWHERE.Append(" AND AdjuntoImagenID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = imagenPerfil.AdjuntoImagen.AdjuntoImagenID;
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
            sqlAdapter.Fill(ds, "ImagenPerfil");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ImagenPerfilRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
