using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Elimina un objeto la base de datos
   /// </summary>
   public class AsistenciaDelHlp { 
      /// <summary>
      /// Elimina un registro de Asistencia en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="asistencia">Asistencia que desea eliminar</param>
      public void Action(IDataContext dctx, Asistencia asistencia){
         object myFirm = new object();
         string sError = "";
         if (asistencia == null)
            sError += ", Asistencia";
         if (sError.Length > 0)
            throw new Exception("AsistenciaDelHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (asistencia.AgrupadorContenidoDigitalID == null)
            sError += ", AgrupadorContenidoDigitalID";
         if (sError.Length > 0)
            throw new Exception("AsistenciaDelHlp:Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "AsistenciaDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AsistenciaDelHlp: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO", 
         "AsistenciaDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Asistencia ");
         // asistencia.Estatus
         sCmd.Append(" SET EstatusProfesionalizacion =@dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
          if (asistencia.Estatus == null)
              sqlParam.Value = DBNull.Value;
          else
              sqlParam.Value = EEstatusProfesionalizacion.INACTIVO;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         if (asistencia.AgrupadorContenidoDigitalID == null)
            sCmd.Append(" WHERE AgrupadorContenidoDigitalID IS NULL ");
         else{ 
            // asistencia.AgrupadorContenidoDigitalID
            sCmd.Append(" WHERE AgrupadorContenidoDigitalID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = asistencia.AgrupadorContenidoDigitalID;
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
            throw new Exception("AsistenciaDelHlp: Hubo un error al eliminar el registro o no existe. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AsistenciaDelHlp: Hubo un error al eliminar el registro o no existe.");
      }



      /// <summary>
      /// Elimina un registro de AAgrupadorContenidoDigital de Asistencia en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="aAgrupadorContenido">Asistencia que desea eliminar</param>
      public void Action(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenido,long? agrupadorPadreID)
      {
          object myFirm = new object();
          string sError = "";
          if (aAgrupadorContenido == null)
              sError += ", AAgrupadorContenidoDigital";
          if (sError.Length > 0)
              throw new Exception("AsistenciaDelHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
          if (aAgrupadorContenido.AgrupadorContenidoDigitalID == null)
              sError += ", AgrupadorContenidoDigitalID";
          if (sError.Length > 0)
              throw new Exception("AsistenciaDelHlp:Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");

          if (agrupadorPadreID == null)
              sError += ", AgrupadorPadreID";
          if (sError.Length > 0)
              throw new Exception("AsistenciaDelHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2) + ".");
          if (dctx == null)
              throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO",
                 "AsistenciaDelHlp", "Action", null, null);
          DbCommand sqlCmd = null;
          try
          {
              dctx.OpenConnection(myFirm);
              sqlCmd = dctx.CreateCommand();
          }
          catch (Exception ex)
          {
              throw new StandardException(MessageType.Error, "", "AsistenciaDelHlp: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO",
                 "AsistenciaDelHlp", "Action", null, null);
          }
          DbParameter sqlParam;
          StringBuilder sCmd = new StringBuilder();
          sCmd.Append(" UPDATE Asistencia ");
          // asistencia.Estatus
          sCmd.Append(" SET EstatusProfesionalizacion =@dbp4ram1 ");
          sqlParam = sqlCmd.CreateParameter();
          sqlParam.ParameterName = "dbp4ram1";
          if (aAgrupadorContenido.Estatus == null)
              sqlParam.Value = DBNull.Value;
          else
              sqlParam.Value = EEstatusProfesionalizacion.INACTIVO;
          sqlParam.DbType = DbType.Byte;
          sqlCmd.Parameters.Add(sqlParam);

            // agrupador.AgrupadorContenidoDigitalID
              sCmd.Append(" WHERE AgrupadorContenidoDigitalID = @dbp4ram2 ");
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "dbp4ram2";
              sqlParam.Value = aAgrupadorContenido.AgrupadorContenidoDigitalID;
              sqlParam.DbType = DbType.Int64;
              sqlCmd.Parameters.Add(sqlParam);

          //AgrupadorPadreID
          sCmd.Append(" AND AgrupadorPadreID = @dbp4ram3");
          sqlParam = sqlCmd.CreateParameter();
          sqlParam.ParameterName = "@dbp4ram3";
          sqlParam.Value = agrupadorPadreID;
          sqlParam.DbType = DbType.Int64;
          sqlCmd.Parameters.Add(sqlParam);
          
          int iRes = 0;
          try
          {
              sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
              iRes = sqlCmd.ExecuteNonQuery();
          }
          catch (Exception ex)
          {
              string exmsg = ex.Message;
              try { dctx.CloseConnection(myFirm); }
              catch (Exception) { }
              throw new Exception("AsistenciaDelHlp: Hubo un error al eliminar el registro o no existe. " + exmsg);
          }
          finally
          {
              try { dctx.CloseConnection(myFirm); }
              catch (Exception) { }
          }
          if (iRes < 1)
              throw new Exception("AsistenciaDelHlp: Hubo un error al eliminar el registro o no existe.");
      }
   } 
}
