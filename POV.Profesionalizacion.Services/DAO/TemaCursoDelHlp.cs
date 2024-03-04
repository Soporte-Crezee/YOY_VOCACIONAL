using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Elimina un registro de TemaCurso en la BD
   /// </summary>
   internal class TemaCursoDelHlp { 
      /// <summary>
      /// Elimina un registro de TemaCursoDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="TemaCurso">TemaCurso que desea eliminar</param>
      public void Action(IDataContext dctx, TemaCurso temacurso){
         object myFirm = new object();
         string sError = String.Empty;
         if (temacurso == null)
            sError += ", TemaCurso";
         if (sError.Length > 0)
             throw new Exception("TemaCursoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (temacurso.TemaCursoID == null)
            sError += ", TemaCursoID";
         if (sError.Length > 0)
             throw new Exception("TemaCursoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
         "TemaCursoDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
             throw new StandardException(MessageType.Error, "", "TemaCursoDelHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO",
         "TemaCursoDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE TemaCurso SET Activo = 0 ");
         sCmd.Append(" WHERE TemaCursoID = @temacurso_TemaCursoID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "temacurso_TemaCursoID";
         if (temacurso.TemaCursoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = temacurso.TemaCursoID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("TemaCursoDelHlp: Ocurrió un error al eliminar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
             throw new Exception("TemaCursoDelHlp: Ocurrió un error al eliminar el registro.");
      }
   } 
}
