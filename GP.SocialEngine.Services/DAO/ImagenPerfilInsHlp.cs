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
   /// Guarda un registro de ImagenPerfil en la BD
   /// </summary>
   public class ImagenPerfilInsHlp { 
      /// <summary>
      /// Crea un registro de ImagenPerfil en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="imagenPerfil">ImagenPerfil que desea crear</param>
      public void Action(IDataContext dctx, UsuarioSocial usuarioSocial, ImagenPerfil imagenPerfil){
         object myFirm = new object();
         string sError = String.Empty;
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
         if (imagenPerfil.AdjuntoImagen == null)
            sError += ", AdjuntoImagen";
         if (sError.Length > 0)
            throw new Exception("ImagenPerfilInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (imagenPerfil.AdjuntoImagen.AdjuntoImagenID == null)
            sError += ", AdjuntoImagenID";
         if (sError.Length > 0)
            throw new Exception("ImagenPerfilInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "ImagenPerfilInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ImagenPerfilInsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "ImagenPerfilInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO ImagenPerfilUsuario (UsuarioSocialID, AdjuntoImagenID, Estatus) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // usuarioSocial.UsuarioSocialID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (usuarioSocial.UsuarioSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioSocial.UsuarioSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // imagenPerfil.AdjuntoImagen.AdjuntoImagenID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (imagenPerfil.AdjuntoImagen.AdjuntoImagenID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = imagenPerfil.AdjuntoImagen.AdjuntoImagenID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // imagenPerfil.Estatus
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (imagenPerfil.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = imagenPerfil.Estatus;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ImagenPerfilInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ImagenPerfilInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
