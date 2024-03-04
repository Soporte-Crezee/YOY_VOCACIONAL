using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;

namespace POV.Comun.DAO { 
   /// <summary>
   /// Borra una referencia a una imagen en la base de datos
   /// </summary>
   public class AdjuntoImagenDelHlp { 
      /// <summary>
      /// Elimina un registro de adjuntoImagen en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="adjuntoImagen">adjuntoImagen que desea eliminar</param>
      public void Action(IDataContext dctx, AdjuntoImagen adjuntoImagen){
         object myFirm = new object();
         string sError = String.Empty;
         if (adjuntoImagen.AdjuntoImagenID == null)
            sError += ", AdjuntoImagenID";
         if (sError.Length > 0)
            throw new Exception("AdjuntoImagenUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Comun.DAO", 
         "AdjuntoImagenDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AdjuntoImagenDelHllp:Ocurrió un error al conectarse a la base de datos", "POV.Comun.DAO", 
         "AdjuntoImagenDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM AdjuntoImagen WHERE AdjuntoImagenID = @adjuntoImagen_AdjuntoImagenID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "adjuntoImagen_AdjuntoImagenID";
         if (adjuntoImagen.AdjuntoImagenID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = adjuntoImagen.AdjuntoImagenID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         if (adjuntoImagen.Extension != null){
            sCmd.Append(" AND Extension = @adjuntoImagen_Extension ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "adjuntoImagen_Extension";
            sqlParam.Value = adjuntoImagen.Extension;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (adjuntoImagen.NombreImagen != null){
            sCmd.Append(" AND NombreImagen =@adjuntoImagen_NombreImagen ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "adjuntoImagen_NombreImagen";
            sqlParam.Value = adjuntoImagen.NombreImagen;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AdjuntoImagenDelHlp: Ocurró un error al eliminar el registro o fue modificado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AdjuntoImagenDelHlp: Ocurró un error al eliminar el registro o fue modificado.");
      }
   } 
}
