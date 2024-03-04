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
   /// Guarda un registro de AAsignacionRecurso en la BD
   /// </summary>
    internal class AsignacionRecursoInsHlp
    { 
      /// <summary>
      /// Crea un registro de AAsignacionRecurso en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="aAsignacionRecurso">AAsignacionRecurso que desea crear</param>
      public void Action(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, AAsignacionRecurso asignacionRecurso){
         object myFirm = new object();
         string sError = String.Empty;
         if (asignacionRecurso == null)
            sError += ", AAsignacionRecurso";
         if (sError.Length > 0)
            throw new Exception("AsignacionRecursoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (detalleCicloEscolar == null)
            sError += ", DetalleCicloEscolar";
         if (sError.Length > 0)
            throw new Exception("AsignacionRecursoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (detalleCicloEscolar.DetalleCicloEscolarID == null)
            sError += ", DetalleCicloEscolarID";
         if (sError.Length > 0)
            throw new Exception("AsignacionRecursoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (asignacionRecurso.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (asignacionRecurso.EstaConfirmado == null)
            sError += ", EstaConfirmado";
         if (asignacionRecurso.TipoAsignacionRecurso == null)
            sError += ", TipoAsignacionRecurso";
         if (sError.Length > 0)
            throw new Exception("AsignacionRecursoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Expediente.DAO", 
         "AsignacionRecursoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AsignacionRecursoInsHlp: No se pudo conectar a la base de datos", "POV.Expediente.DAO", 
         "AsignacionRecursoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO AsignacionRecurso (DetalleCicloEscolarID, FechaRegistro, Tipo, EstaConfirmado) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // detalleCicloEscolar.DetalleCicloEscolarID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (detalleCicloEscolar.DetalleCicloEscolarID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = detalleCicloEscolar.DetalleCicloEscolarID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // asignacionRecurso.FechaRegistro
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (asignacionRecurso.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = asignacionRecurso.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // asignacionRecurso.TipoAsignacionRecurso
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (asignacionRecurso.TipoAsignacionRecurso == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = asignacionRecurso.TipoAsignacionRecurso;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         // asignacionRecurso.EstaConfirmado
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (asignacionRecurso.EstaConfirmado == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = asignacionRecurso.EstaConfirmado;
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
            throw new Exception("AsignacionRecursoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AsignacionRecursoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
