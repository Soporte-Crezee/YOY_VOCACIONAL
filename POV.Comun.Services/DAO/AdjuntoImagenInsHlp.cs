using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;

namespace POV.Comun.DAO { 
   /// <summary>
   /// Inserta una referencia a una imagen
   /// </summary>
   public class AdjuntoImagenInsHlp { 
      /// <summary>
      /// Crea un registro de adjuntoImagen en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="adjuntoImagen">adjuntoImagen que desea crear</param>
      public void Action(IDataContext dctx, AdjuntoImagen adjuntoImagen){
         object myFirm = new object();
         string sError = String.Empty;
         if (adjuntoImagen == null)
            sError += ", AdjuntoImagen";
         if (sError.Length > 0)
            throw new Exception("AdjuntoImagenInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (adjuntoImagen.Extension == null || adjuntoImagen.Extension.Trim().Length == 0)
            sError += ", Extension";
         if (adjuntoImagen.MIME == null || adjuntoImagen.MIME.Trim().Length == 0)
            sError += ", MIME";
         if (adjuntoImagen.NombreImagen == null || adjuntoImagen.NombreImagen.Trim().Length == 0)
            sError += ", NombreImagen";
         if (adjuntoImagen.NombreThumb == null || adjuntoImagen.NombreThumb.Trim().Length == 0)
            sError += ", NombreThumb";
         if (adjuntoImagen.CarpetaID == null)
            sError += ", CarpetaID";
         if (sError.Length > 0)
            throw new Exception("AdjuntoImagenInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Comun.DAO", 
         "AdjuntoImagenInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AdjuntoImagenInsHlp: No se pudo conectar a la base de datos", "POV.Comun.DAO", 
         "AdjuntoImagenInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO AdjuntoImagen (Extension, MIME, NombreImagen, NombreThumb, CarpetaID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         sCmd.Append(" @adjuntoImagen_Extension ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "adjuntoImagen_Extension";
         if (adjuntoImagen.Extension == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = adjuntoImagen.Extension;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@adjuntoImagen_MIME ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "adjuntoImagen_MIME";
         if (adjuntoImagen.MIME == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = adjuntoImagen.MIME;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@adjuntoImagen_NombreImagen ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "adjuntoImagen_NombreImagen";
         if (adjuntoImagen.NombreImagen == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = adjuntoImagen.NombreImagen;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@adjuntoImagen_NombreThumb ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "adjuntoImagen_NombreThumb";
         if (adjuntoImagen.NombreThumb == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = adjuntoImagen.NombreThumb;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@adjuntoImagen_CarpetaID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "adjuntoImagen_CarpetaID";
         if (adjuntoImagen.CarpetaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = adjuntoImagen.CarpetaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AdjuntoImagenInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AdjuntoImagenInsHlp: Ocurrio un error al ingresar el registro.");
      }
   } 
}
