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
   /// Guarda un registro de RespuestaPlantillaOpcionMultiple en la BD
   /// </summary>
   public class RespuestaPlantillaOpcionMultipleInsHlp { 
      /// <summary>
      /// Crea un registro de RespuestaPlantillaOpcionMultipe en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaPlantillaOpcionMultipe">RespuestaPlantillaOpcionMultipe que desea crear</param>
      public void Action(IDataContext dctx, RespuestaPlantillaOpcionMultiple respuestaPlantillaOpcionMultiple){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaPlantillaOpcionMultiple == null)
            sError += ", RespuestaPlantillaOpcionMultiple";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaOpcionMultipleInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPlantillaOpcionMultiple.RespuestaPlantillaID == null)
            sError += ", RespuestaPlantillaOpcionMultiple";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaOpcionMultipleInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMaximo == null)
             sError += ", NumeroSeleccionablesMaximo";
         if (respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMinimo == null)
             sError += ", NumeroSeleccionablesMinimo";
         if (respuestaPlantillaOpcionMultiple.ModoSeleccion == null)
             sError += ", ModoSeleccion";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaOpcionMultipleInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "RespuestaPlantillaOpcionMultipleInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaPlantillaOpcionMultipleInsHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO", 
         "RespuestaPlantillaOpcionMultipleInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO RespuestaPlantillaOpcionMultiple (RespuestaPlantillaID,NumeroSeleccionablesMaximo,NumeroSeleccionablesMinimo,ModoSeleccion) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         sCmd.Append(" @respuestaPlantillaOpcionMultiple_RespuestaPlantillaID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "respuestaPlantillaOpcionMultiple_RespuestaPlantillaID";
         if (respuestaPlantillaOpcionMultiple.RespuestaPlantillaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantillaOpcionMultiple.RespuestaPlantillaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@respuestaPlantillaOpcionMultiple_NumeroSeleccionablesMaximo ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "respuestaPlantillaOpcionMultiple_NumeroSeleccionablesMaximo";
         if (respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMaximo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMaximo;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@respuestaPlantillaOpcionMultiple_NumeroSeleccionablesMinimo ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "respuestaPlantillaOpcionMultiple_NumeroSeleccionablesMinimo";
         if (respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMinimo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantillaOpcionMultiple.NumeroSeleccionablesMinimo;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@respuestaPlantillaOpcionMultiple_ModoSeleccion ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "respuestaPlantillaOpcionMultiple_ModoSeleccion";
         if (respuestaPlantillaOpcionMultiple.ModoSeleccion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantillaOpcionMultiple.ModoSeleccion;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RespuestaPlantillaOpcionMultipleInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaPlantillaOpcionMultipleInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
