using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Expediente.BO;
using POV.Expediente.DAO;

namespace POV.Expediente.DAO { 
   /// <summary>
   /// Actualiza un registro de AAsignacionRecurso en la BD
   /// </summary>
    internal class AsignacionRecursoUpdHlp
    { 
      /// <summary>
      /// Actualiza de manera optimista un registro de AsignacionRecursoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="asignacionRecursoUpdHlp">AsignacionRecursoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">AsignacionRecursoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, AAsignacionRecurso asignacionRecurso, AAsignacionRecurso anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (asignacionRecurso == null)
            sError += ", AAsignacionRecurso";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("AsignacionRecursoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.AsignacionRecursoID == null)
            sError += ", Anterior AsignacionRecursoID";
         if (sError.Length > 0)
            throw new Exception("AsignacionRecursoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Expediente.DAO", 
         "AsignacionRecursoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AsignacionRecursoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Expediente.DAO", 
         "AsignacionRecursoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE AsignacionRecurso ");
         if (asignacionRecurso.EstaConfirmado == null)
            sCmd.Append(" SET EstaConfirmado = NULL ");
         else{ 
            // asignacionRecurso.EstaConfirmado
            sCmd.Append(" SET EstaConfirmado = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = asignacionRecurso.EstaConfirmado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.AsignacionRecursoID == null)
            sCmd.Append(" WHERE AsignacionRecursoID IS NULL ");
         else{ 
            // anterior.AsignacionRecursoID
            sCmd.Append(" WHERE AsignacionRecursoID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = anterior.AsignacionRecursoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AsignacionRecursoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AsignacionRecursoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
