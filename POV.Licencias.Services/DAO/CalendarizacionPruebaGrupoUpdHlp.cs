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
   /// Actualiza un registro de CalendarizacionPruebaGrupo en la BD
   /// </summary>
   internal class CalendarizacionPruebaGrupoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de CalendarizacionPruebaGrupo en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="calendarizacionPruebaGrupo">CalendarizacionPruebaGrupo que tiene los datos nuevos</param>
      /// <param name="anterior">CalendarizacionPruebaGrupo que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, CalendarizacionPruebaGrupo calendarizacionPruebaGrupo, CalendarizacionPruebaGrupo anterior){
         object myFirm = new object();
         string sError = String.Empty;
         if (calendarizacionPruebaGrupo == null)
            sError += ", GrupoCicloEscolar";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("CalendarizacionPruebaGrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.CalendarizacionPruebaGrupoID == null)
            sError += ", Anterior CalendarizacionPruebaGrupoID";
         if (sError.Length > 0)
            throw new Exception("calendarizacionPruebaGrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", 
         "CalendarizacionPruebaGrupoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "CalendarizacionPruebaGrupoUpdHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO", 
         "CalendarizacionPruebaGrupoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE CalendarizacionPruebaGrupo ");
         if (calendarizacionPruebaGrupo.ConVigencia == null)
            sCmd.Append(" SET ConVigencia = NULL ");
         else{ 
            // calendarizacionPruebaGrupo.ConVigencia
            sCmd.Append(" SET ConVigencia = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = calendarizacionPruebaGrupo.ConVigencia;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (calendarizacionPruebaGrupo.FechaInicioVigencia == null)
            sCmd.Append(" ,FechaInicioVigencia = NULL ");
         else{ 
            // calendarizacionPruebaGrupo.FechaInicioVigencia
            sCmd.Append(" ,FechaInicioVigencia = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = calendarizacionPruebaGrupo.FechaInicioVigencia;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (calendarizacionPruebaGrupo.FechaFinVigencia == null)
            sCmd.Append(" ,FechaFinVigencia = NULL ");
         else{ 
            // calendarizacionPruebaGrupo.FechaFinVigencia
            sCmd.Append(" ,FechaFinVigencia = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = calendarizacionPruebaGrupo.FechaFinVigencia;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (calendarizacionPruebaGrupo.Activo == null)
             sCmd.Append(" ,Activo = NULL ");
         else
         {
             // calendarizacionPruebaGrupo.FechaFinVigencia
             sCmd.Append(" ,Activo = @dbp4ram4 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram4";
             sqlParam.Value = calendarizacionPruebaGrupo.Activo;
             sqlParam.DbType = DbType.Boolean;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.CalendarizacionPruebaGrupoID == null)
            sCmd.Append(" WHERE CalendarizacionPruebaGrupoID IS NULL ");
         else{ 
            // anterior.CalendarizacionPruebaGrupoID
            sCmd.Append(" WHERE CalendarizacionPruebaGrupoID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.CalendarizacionPruebaGrupoID;
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
