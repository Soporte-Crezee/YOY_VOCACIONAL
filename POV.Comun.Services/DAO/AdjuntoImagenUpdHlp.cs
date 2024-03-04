using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;

namespace POV.Comun.DAO { 
   /// <summary>
   /// Actualiza un registro de un AdjuntoImagen
   /// </summary>
   public class AdjuntoImagenUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de adjuntoImagen en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="adjuntoImagen">adjuntoImagen que tiene los datos nuevos</param>
      /// <param name="anterior">adjuntoImagen que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, AdjuntoImagen adjuntoImagen, AdjuntoImagen anterior){
         object myFirm = new object();
         string sError = String.Empty;
         if (adjuntoImagen == null)
            sError += ", adjuntoImagen";
         if (anterior == null)
            sError += ", anterior";
         if (sError.Length > 0)
            throw new Exception("AdjuntoImagenUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (adjuntoImagen.AdjuntoImagenID == null)
            sError += ", AdjuntoImagenID";
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
         if (anterior.AdjuntoImagenID == null)
            sError += ", anterior.AdjuntoImagenID";
         if (anterior.Extension == null || anterior.Extension.Trim().Length == 0)
            sError += ", anterior.Extension";
         if (anterior.MIME == null || anterior.MIME.Trim().Length == 0)
            sError += ", anterior.MIME";
         if (anterior.NombreImagen == null || anterior.NombreImagen.Trim().Length == 0)
            sError += ", anterior.NombreImagen";
         if (anterior.NombreThumb == null || anterior.NombreThumb.Trim().Length == 0)
            sError += ", anterior.NombreThumb";
         if (anterior.CarpetaID == null)
            sError += ", anterior.CarpetaID";
         if (sError.Length > 0)
            throw new Exception("AdjuntoImagenUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (adjuntoImagen.AdjuntoImagenID != anterior.AdjuntoImagenID) {
         sError = "Los objetos no coinciden";
      }
         if (sError.Length > 0)
            throw new Exception("AdjuntoImagenUpdHlp: " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Comun.DAO", 
         "AdjuntoImagenUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AdjuntoImagenUpdHlp: Ocurrió un error al conectarse a la base de datos", "POV.Comun.DAO", 
         "AdjuntoImagenUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         if (adjuntoImagen.Extension != null){
            sCmd.Append(" UPDATE AdjuntoImagen SET Extension =@adjuntoImagen_Extension ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "adjuntoImagen_Extension";
            sqlParam.Value = adjuntoImagen.Extension;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         sCmd.Append(" ,MIME =@adjuntoImagen_MIME ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "adjuntoImagen_MIME";
         if (adjuntoImagen.MIME == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = adjuntoImagen.MIME;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,NombreImagen = @adjuntoImagen_NombreImagen ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "adjuntoImagen_NombreImagen";
         if (adjuntoImagen.NombreImagen == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = adjuntoImagen.NombreImagen;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,NombreThumb = @adjuntoImagen_NombreThumb ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "adjuntoImagen_NombreThumb";
         if (adjuntoImagen.NombreThumb == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = adjuntoImagen.NombreThumb;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,CarpetaID = @adjuntoImagen_CarpetaID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "adjuntoImagen_CarpetaID";
         if (adjuntoImagen.CarpetaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = adjuntoImagen.CarpetaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" WHERE ");
         if (anterior.AdjuntoImagenID != null){
            sCmd.Append(" AdjuntoImagenID =@anterior_AdjuntoImagenID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_AdjuntoImagenID";
            sqlParam.Value = anterior.AdjuntoImagenID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Extension != null){
            sCmd.Append(" AND Extension =@anterior_Extension ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_Extension";
            sqlParam.Value = anterior.Extension;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.MIME != null){
            sCmd.Append(" AND MIME =@anterior_MIME ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_MIME";
            sqlParam.Value = anterior.MIME;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.NombreImagen != null){
            sCmd.Append(" AND NombreImagen =@anterior_NombreImagen ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_NombreImagen";
            sqlParam.Value = anterior.NombreImagen;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.NombreThumb != null){
            sCmd.Append(" AND NombreThumb =@anterior_NombreThumb ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_NombreThumb";
            sqlParam.Value = anterior.NombreThumb;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.CarpetaID != null){
            sCmd.Append(" AND CarpetaID =@anterior_CarpetaID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_CarpetaID";
            sqlParam.Value = anterior.CarpetaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AdjuntoImagenUpdHlp: Ocurrio un error al actualizar la imagen o fue modificada mientras era editada. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AdjuntoImagenUpdHlp: Ocurrio un error al actualizar la imagen o fue modificada mientras era editada.");
      }
   } 
}
