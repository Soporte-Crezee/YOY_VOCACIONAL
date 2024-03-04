using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.CentroEducativo.BO;

namespace POV.Licencias.DAO { 
   /// <summary>
   /// Elimina un registro de CalendarizacionPruebaGrupo en la BD
   /// </summary>
   internal class CalendarizacionPruebaGrupoDelHlp { 
      /// <summary>
      /// Elimina un registro de CalendarizacionPruebaGrupo en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="calendarizacionPruebaGrupo">CalendarizacionPruebaGrupo que desea eliminar</param>
      public void Action(IDataContext dctx, CalendarizacionPruebaGrupo calendarizacionPruebaGrupo){
         object myFirm = new object();
         string sError = String.Empty;
         if (calendarizacionPruebaGrupo == null)
            sError += ", GrupoCicloEscolar";
         if (sError.Length > 0)
            throw new Exception("CalendarizacionPruebaGrupoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (calendarizacionPruebaGrupo.CalendarizacionPruebaGrupoID == null)
            sError += ", Anterior CalendarizacionPruebaGrupoID";
         if (sError.Length > 0)
            throw new Exception("calendarizacionPruebaGrupoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", 
         "CalendarizacionPruebaGrupoDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "CalendarizacionPruebaGrupoDelHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO", 
         "CalendarizacionPruebaGrupoDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         //sCmd.Append(" UPDATE CalendarizacionPruebaGrupo ");
         //sCmd.Append(" SET Activo = 0 ");
         sCmd.Append(" DELETE CalendarizacionPruebaGrupo ");
         if (calendarizacionPruebaGrupo.CalendarizacionPruebaGrupoID == null)
            sCmd.Append(" WHERE CalendarizacionPruebaGrupoID IS NULL ");
         else{ 
            // calendarizacionPruebaGrupo.CalendarizacionPruebaGrupoID
            sCmd.Append(" WHERE CalendarizacionPruebaGrupoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = calendarizacionPruebaGrupo.CalendarizacionPruebaGrupoID;
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
            throw new Exception("CalendarizacionPruebaGrupoUpdHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("CalendarizacionPruebaGrupoUpdHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
