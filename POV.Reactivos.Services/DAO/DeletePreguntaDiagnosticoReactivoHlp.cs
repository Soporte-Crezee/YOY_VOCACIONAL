using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;

namespace POV.Reactivos.DAO { 
   /// <summary>
   /// Elimina un registro de Pregunta (Diagnostico) en la BD
   /// </summary>
   public class DeletePreguntaDiagnosticoReactivoHlp { 
      /// <summary>
      /// Elimina un registro de DeletePreguntaDiagnosticoReactivoHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="deletePreguntaDiagnosticoReactivoHlp">DeletePreguntaDiagnosticoReactivoHlp que desea eliminar</param>
      public void Action(IDataContext dctx, Reactivo reactivo){
         object myFirm = new object();
         string sError = String.Empty;
         if (reactivo == null)
            sError += ", Reactivo";
         if (sError.Length > 0)
            throw new Exception("DeletePreguntaDiagnosticoReactivoHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (reactivo.ReactivoID == null)
            sError += ", ReactivoID";
         if (sError.Length > 0)
            throw new Exception("DeletePreguntaDiagnosticoReactivoHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "DeletePreguntaDiagnosticoReactivoHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "DeletePreguntaDiagnosticoReactivoHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO", 
         "DeletePreguntaDiagnosticoReactivoHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE FROM PreguntaDiagnostico SET Activo = 0 ");
         sCmd.Append(" WHERE ReactivoID=@dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (reactivo.ReactivoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = reactivo.ReactivoID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("DeletePreguntaDiagnosticoReactivoHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("DeletePreguntaDiagnosticoReactivoHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
