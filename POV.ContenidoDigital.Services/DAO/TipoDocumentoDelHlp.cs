using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;

namespace POV.ContenidosDigital.DAO { 
   /// <summary>
   /// Elimina un registro de TipoDocumento en la BD
   /// </summary>
   internal class TipoDocumentoDelHlp { 
      /// <summary>
      /// Elimina un registro de TipoDocumentoDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="tipoDocumentoDelHlp">TipoDocumentoDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, TipoDocumento tipoDocumento){
         object myFirm = new object();
         string sError = String.Empty;
         if (tipoDocumento == null)
            sError += ", TipoDocumento";
         if (sError.Length > 0)
            throw new Exception("TipoDocumentoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (tipoDocumento.TipoDocumentoID == null)
            sError += ", TipoDocumentoID";
         if (sError.Length > 0)
            throw new Exception("TipoDocumentoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "TipoDocumentoDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TipoDocumentoDelHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "TipoDocumentoDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE TipoDocumento ");
         sCmd.Append(" SET Activo = 0 ");
         if (tipoDocumento.TipoDocumentoID == null)
            sCmd.Append(" WHERE TipoDocumentoID IS NULL ");
         else{ 
            // tipoDocumento.TipoDocumentoID
            sCmd.Append(" WHERE TipoDocumentoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = tipoDocumento.TipoDocumentoID;
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
            throw new Exception("TipoDocumentoDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("TipoDocumentoDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
