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
   /// Actualiza un registro de RespuestaPlantilla(Diagnostico) en la BD
   /// </summary>
   public class RespuestaPlantillaDiagnosticoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de RespuestaPlantillaDiagnosticoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaPlantillaDiagnosticoUpdHlp">RespuestaPlantillaDiagnosticoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">RespuestaPlantillaDiagnosticoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, RespuestaPlantillaOpcionMultiple respuestaPlantilla, RespuestaPlantillaOpcionMultiple anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (respuestaPlantilla == null)
            sError += ", RespuestaPlantilla";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaDiagnosticoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.RespuestaPlantillaID == null)
            sError += ", Anterior RespuestaPlantillaID";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaDiagnosticoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "RespuestaPlantillaDiagnosticoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaPlantillaDiagnosticoUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Reactivos.DAO", 
         "RespuestaPlantillaDiagnosticoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE RespuestaPlantillaDiagnostico ");
         sCmd.Append(" SET ");
         // respuestaPlantilla.NumeroSeleccionablesMaximo
         sCmd.Append(" NumeroSeleccionableMaximo = @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (respuestaPlantilla.NumeroSeleccionablesMaximo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantilla.NumeroSeleccionablesMaximo;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPlantilla.NumeroSeleccionablesMinimo
         sCmd.Append(" ,NumeroSeleccionableMinimo = @dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (respuestaPlantilla.NumeroSeleccionablesMinimo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantilla.NumeroSeleccionablesMinimo;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPlantilla.ModoSeleccion
         sCmd.Append(" ,ModoSeleccion = @dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (respuestaPlantilla.ModoSeleccion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantilla.ModoSeleccion;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaPlantilla.Estatus
         sCmd.Append(" ,Estatus = @dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (respuestaPlantilla.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantilla.Estatus;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         if (anterior.RespuestaPlantillaID == null)
            sCmd.Append(" WHERE RespuestaPlantillaID IS NULL ");
         else{ 
            // anterior.RespuestaPlantillaID
            sCmd.Append(" WHERE RespuestaPlantillaID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.RespuestaPlantillaID;
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
            throw new Exception("RespuestaPlantillaDiagnosticoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaPlantillaDiagnosticoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
