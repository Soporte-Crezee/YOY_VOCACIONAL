using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Reactivos.DAO;

namespace POV.Reactivos.DAO { 
   /// <summary>
   /// Actualiza un registro de RespuestaPlantillaOpcionMultiple en la BD
   /// </summary>
   public class RespuestaPlantillaOpcionMultipleUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de RespuestaPlantillaOpcionMultipleUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaPlantillaOpcionMultipleUpdHlp">RespuestaPlantillaOpcionMultipleUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">RespuestaPlantillaOpcionMultipleUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, RespuestaPlantillaOpcionMultiple respuestaPlantilla, RespuestaPlantillaOpcionMultiple anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (respuestaPlantilla == null)
            sError += ", RespuestaPlantillaOpcionMultiple";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaOpcionMultipleUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.RespuestaPlantillaID == null)
            sError += ", Anterior RespuestaPlantillaID";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaOpcionMultipleUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "RespuestaPlantillaOpcionMultipleUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaPlantillaOpcionMultipleUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Reactivos.DAO", 
         "RespuestaPlantillaOpcionMultipleUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE RespuestaPlantillaOpcionMultiple ");
         if (respuestaPlantilla.NumeroSeleccionablesMaximo == null)
            sCmd.Append(" SET NumeroSeleccionablesMaximo = NULL ");
         else{ 
            sCmd.Append(" SET NumeroSeleccionablesMaximo = @respuestaPlantilla_NumeroSeleccionablesMaximo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "respuestaPlantilla_NumeroSeleccionablesMaximo";
            sqlParam.Value = respuestaPlantilla.NumeroSeleccionablesMaximo;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaPlantilla.NumeroSeleccionablesMinimo == null)
            sCmd.Append(" ,NumeroSeleccionablesMinimo = NULL ");
         else{ 
            sCmd.Append(" ,NumeroSeleccionablesMinimo = @respuestaPlantilla_NumeroSeleccionablesMinimo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "respuestaPlantilla_NumeroSeleccionablesMinimo";
            sqlParam.Value = respuestaPlantilla.NumeroSeleccionablesMinimo;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaPlantilla.ModoSeleccion == null)
            sCmd.Append(" ,ModoSeleccion = NULL ");
         else{ 
            sCmd.Append(" ,ModoSeleccion = @respuestaPlantilla_ModoSeleccion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "respuestaPlantilla_ModoSeleccion";
            sqlParam.Value = respuestaPlantilla.ModoSeleccion;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.RespuestaPlantillaID == null)
            sCmd.Append(" WHERE RespuestaPlantillaID IS NULL ");
         else{ 
            sCmd.Append(" WHERE RespuestaPlantillaID = @anterior_RespuestaPlantillaID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_RespuestaPlantillaID";
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
            throw new Exception("RespuestaPlantillaOpcionMultipleUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaPlantillaOpcionMultipleUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
