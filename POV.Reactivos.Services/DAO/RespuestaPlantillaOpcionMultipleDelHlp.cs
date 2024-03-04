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
   /// Elimina un registro de RespuestaPlantillaOpcionMultiple en la BD
   /// </summary>
   public class RespuestaPlantillaOpcionMultipleDelHlp { 
      /// <summary>
      /// Elimina un registro de RespuestaPlantillaOpcionMultipleDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaPlantillaOpcionMultipleDelHlp">RespuestaPlantillaOpcionMultipleDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, RespuestaPlantillaOpcionMultiple respuestaPlantillaOpcionMultiple){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaPlantillaOpcionMultiple == null)
            sError += ", RespuestaPlantillaOpcionMultiple";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaOpcionMultipleDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaPlantillaOpcionMultiple.RespuestaPlantillaID == null)
            sError += ", RespuestaPlantillaOpcionMultipleID";
         if (sError.Length > 0)
            throw new Exception("RespuestaPlantillaOpcionMultipleDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "RespuestaPlantillaOpcionMultipleDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaPlantillaOpcionMultipleDelHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO", 
         "RespuestaPlantillaOpcionMultipleDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM RespuestaPlantillaOpcionMultiple ");
         sCmd.Append(" WHERE RespuestaPlantillaID=@respuestaPlantillaOpcionMultiple_RespuestaPlantillaID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "respuestaPlantillaOpcionMultiple_RespuestaPlantillaID";
         if (respuestaPlantillaOpcionMultiple.RespuestaPlantillaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaPlantillaOpcionMultiple.RespuestaPlantillaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RespuestaPlantillaOpcionMultipleDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaPlantillaOpcionMultipleDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
