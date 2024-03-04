using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Elimina un registro de TemaAsistencia en la BD
   /// </summary>
    internal class TemaAsistenciaDelHlp
    { 
      /// <summary>
      /// Elimina un registro de TemaAsistenciaDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaAsistenciaDelHlp">TemaAsistenciaDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, TemaAsistencia temaAsistencia){
         object myFirm = new object();
         string sError = String.Empty;
         if (temaAsistencia == null)
            sError += ", TemaAsistencia";
         if (sError.Length > 0)
            throw new Exception("TemaAsistenciaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (temaAsistencia.TemaAsistenciaID == null)
            sError += ", TemaAsistenciaID";
         if (sError.Length > 0)
            throw new Exception("TemaAsistenciaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "TemaAsistenciaDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TemaAsistenciaDelHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "TemaAsistenciaDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE TemaAsistencia ");
         sCmd.Append(" SET Activo = 0 ");
         if (temaAsistencia.TemaAsistenciaID == null)
            sCmd.Append(" WHERE TemaAsistenciaID IS NULL ");
         else{ 
            // temaAsistencia.TemaAsistenciaID
            sCmd.Append(" WHERE TemaAsistenciaID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = temaAsistencia.TemaAsistenciaID;
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
            throw new Exception("TemaAsistenciaDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("TemaAsistenciaDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
