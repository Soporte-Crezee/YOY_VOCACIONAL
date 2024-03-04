using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;

namespace POV.Comun.DAO { 
   /// <summary>
   /// Consulta AdjutoImagen
   /// </summary>
   public class AdjuntoImagenRetHlp { 
      /// <summary>
      /// Consulta registros de adjuntoImagen en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="adjuntoImagen">adjuntoImagen que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de adjuntoImagen generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, AdjuntoImagen adjuntoImagen){
         object myFirm = new object();
         string sError = String.Empty;
         if (adjuntoImagen == null)
            sError += ", adjuntoImagen";
         if (sError.Length > 0)
            throw new Exception("ImagenPerfilRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Comun.DAO", 
         "AdjuntoImagenRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AdjuntoImagenRetHlp:No se pudo", "POV.Comun.DAO", 
         "AdjuntoImagenRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT ");
         sCmd.Append(" img.AdjuntoImagenID, img.Extension, img.MIME, ");
         sCmd.Append(" img.NombreImagen,img.NombreThumb, img.CarpetaID, carpeta.Ruta Carpeta ");
         sCmd.Append(" FROM AdjuntoImagen img ");
         sCmd.Append(" INNER JOIN CarpetaSistema carpeta ON carpeta.CarpetaID =img.CarpetaID ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (adjuntoImagen.AdjuntoImagenID != null){
            s_VarWHERE.Append(" img.AdjuntoImagenID=@adjuntoImagen_AdjuntoImagenID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "adjuntoImagen_AdjuntoImagenID";
            sqlParam.Value = adjuntoImagen.AdjuntoImagenID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (adjuntoImagen.Extension != null){
             s_VarWHERE.Append( " AND img.Extension=@adjuntoImagen_Extension " );
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "adjuntoImagen_Extension";
            sqlParam.Value = adjuntoImagen.Extension;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (adjuntoImagen.MIME != null){
             s_VarWHERE.Append( "  AND img.MIME =@adjuntoImagen_MIME " );
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "adjuntoImagen_MIME";
            sqlParam.Value = adjuntoImagen.MIME;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (adjuntoImagen.NombreImagen != null){
             s_VarWHERE.Append( " AND img.NombreImagen=@adjuntoImagen_NombreImagen " );
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "adjuntoImagen_NombreImagen";
            sqlParam.Value = adjuntoImagen.NombreImagen;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (adjuntoImagen.NombreThumb != null){
             s_VarWHERE.Append( " AND img.NombreThumb=@adjuntoImagen_NombreThumb " );
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "adjuntoImagen_NombreThumb";
            sqlParam.Value = adjuntoImagen.NombreThumb;
            sqlParam.DbType = DbType.String;
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
            sqlAdapter.Fill(ds, "AdjuntoImagen");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AdjuntoImagenRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
