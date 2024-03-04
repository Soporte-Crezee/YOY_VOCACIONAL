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
   /// Guarda un registro de CalendarizacionPruebaGrupo en la BD
   /// </summary>
   internal class CalendarizacionPruebaGrupoInsHlp { 
      /// <summary>
      /// Crea un registro de CalendarizacionPruebaGrupo en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="calendarizacionPruebaGrupo">CalendarizacionPruebaGrupo que desea crear</param>
      public void Action(IDataContext dctx, CalendarizacionPruebaGrupo calendarizacionPruebaGrupo){
         object myFirm = new object();
         string sError = String.Empty;
         if (calendarizacionPruebaGrupo == null)
            sError += ", GrupoCicloEscolar";
         if (sError.Length > 0)
            throw new Exception("CalendarizacionPruebaGrupoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (calendarizacionPruebaGrupo.GrupoCicloEscolar == null)
            sError += ", GrupoCicloEscolar";
         if (sError.Length > 0)
            throw new Exception("CalendarizacionPruebaGrupoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (calendarizacionPruebaGrupo.GrupoCicloEscolar.GrupoCicloEscolarID == null)
            sError += ", GrupoCicloEscolarID";
         if (sError.Length > 0)
            throw new Exception("CalendarizacionPruebaGrupoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (calendarizacionPruebaGrupo.PruebaContrato == null)
            sError += ", PruebaContrato";
         if (sError.Length > 0)
            throw new Exception("CalendarizacionPruebaGrupoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (calendarizacionPruebaGrupo.PruebaContrato.PruebaContratoID == null)
            sError += ", PruebaContratoID";
         if (sError.Length > 0)
            throw new Exception("CalendarizacionPruebaGrupoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (calendarizacionPruebaGrupo.ConVigencia == null)
            sError += ", ConVigencia";
      if (calendarizacionPruebaGrupo.ConVigencia == true) {
         if (calendarizacionPruebaGrupo.FechaInicioVigencia == null)
            sError += ", FechaInicioVigencia";
         if (calendarizacionPruebaGrupo.FechaFinVigencia == null)
            sError += ", FechaFinVigencia";
      }
         if (calendarizacionPruebaGrupo.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (calendarizacionPruebaGrupo.Activo == null)
            sError += ", Activo";
         if (sError.Length > 0)
            throw new Exception("calendarizacionPruebaGrupoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", 
         "CalendarizacionPruebaGrupoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "CalendarizacionPruebaGrupoInsHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO", 
         "CalendarizacionPruebaGrupoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO CalendarizacionPruebaGrupo (GrupoCicloEscolarID, PruebaContratoID, ConVigencia, FechaInicioVigencia, FechaFinVigencia, FechaRegistro, Activo) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // calendarizacionPruebaGrupo.GrupoCicloEscolar.GrupoCicloEscolarID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (calendarizacionPruebaGrupo.GrupoCicloEscolar.GrupoCicloEscolarID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = calendarizacionPruebaGrupo.GrupoCicloEscolar.GrupoCicloEscolarID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         // calendarizacionPruebaGrupo.PruebaContrato.PruebaContratoID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (calendarizacionPruebaGrupo.PruebaContrato.PruebaContratoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = calendarizacionPruebaGrupo.PruebaContrato.PruebaContratoID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // calendarizacionPruebaGrupo.ConVigencia
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (calendarizacionPruebaGrupo.ConVigencia == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = calendarizacionPruebaGrupo.ConVigencia;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // calendarizacionPruebaGrupo.FechaInicioVigencia
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (calendarizacionPruebaGrupo.FechaInicioVigencia == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = calendarizacionPruebaGrupo.FechaInicioVigencia;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // calendarizacionPruebaGrupo.FechaFinVigencia
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (calendarizacionPruebaGrupo.FechaFinVigencia == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = calendarizacionPruebaGrupo.FechaFinVigencia;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // calendarizacionPruebaGrupo.FechaRegistro
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (calendarizacionPruebaGrupo.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = calendarizacionPruebaGrupo.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // calendarizacionPruebaGrupo.Activo
         sCmd.Append(" ,@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (calendarizacionPruebaGrupo.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = calendarizacionPruebaGrupo.Activo;
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
            throw new Exception("CalendarizacionPruebaGrupoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("CalendarizacionPruebaGrupoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
