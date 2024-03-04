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
   /// Actualiza un registro de RespuestaPlantilla en la BD
   /// </summary>
   public class RespuestaPlantillaUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de RespuestaPlantillaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaPlantillaUpdHlp">RespuestaPlantillaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">RespuestaPlantillaUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, RespuestaPlantilla respuestaPlantilla, RespuestaPlantilla anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (respuestaPlantilla == null)
            sError += ", RespuestaPlantilla";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.RespuestaPlantillaID == null)
            sError += ", Anterior RespuestaPlantillaID";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "RespuestaPlantillaUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaPlantillaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Reactivos.DAO", 
         "RespuestaPlantillaUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE RespuestaPlantillaDinamico ");
         if (respuestaPlantilla.Estatus == null)
            sCmd.Append(" SET Estatus = NULL ");
         else{ 
            sCmd.Append(" SET Estatus = @respuestaPlantilla_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "respuestaPlantilla_Estatus";
            sqlParam.Value = respuestaPlantilla.Estatus;
            sqlParam.DbType = DbType.Boolean;
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
            throw new Exception("RespuestaPlantillaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaPlantillaUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
