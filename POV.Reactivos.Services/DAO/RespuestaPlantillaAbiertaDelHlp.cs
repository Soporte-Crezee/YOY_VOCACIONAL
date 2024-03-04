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
   /// Elimina un registro de RespuestaPlantillaAbierta en la BD
   /// </summary>
   internal class RespuestaPlantillaAbiertaDelHlp { 
      /// <summary>
      /// Elimina un registro de RespuestaPlantillaAbiertaDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaPlantillaAbiertaDelHlp">RespuestaPlantillaAbiertaDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, RespuestaPlantillaAbierta respuestaPlantillaAbierta){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaPlantillaAbierta == null)
            sError += ", RespuestaPlantillaAbierta";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaAbiertaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPlantillaAbierta.RespuestaPlantillaID == null)
            sError += ", RespuestaPlantillaAbiertaID";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaAbiertaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "RespuestaPlantillaAbiertaDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaPlantillaAbiertaDelHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO", 
         "RespuestaPlantillaAbiertaDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM RespuestaPlantillaAbierta ");
         sCmd.Append(" WHERE RespuestaPlantillaID=@respuestaPlantillaAbierta_RespuestaPlantillaID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "respuestaPlantillaAbierta_RespuestaPlantillaID";
         if (respuestaPlantillaAbierta.RespuestaPlantillaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantillaAbierta.RespuestaPlantillaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RespuestaPlantillaAbiertaDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaPlantillaAbiertaDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
