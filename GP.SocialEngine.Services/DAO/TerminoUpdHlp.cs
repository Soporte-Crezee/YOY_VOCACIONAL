using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Actualiza un registro de Termino en la BD
   /// </summary>
   public class TerminoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de TerminoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="terminoUpdHlp">TerminoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">TerminoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Termino termino, Termino anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (termino == null)
            sError += ", Termino";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("TerminoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.TerminoID == null)
            sError += ", Anterior TerminoID";
         if (sError.Length > 0)
            throw new Exception("TerminoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "TerminoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TerminoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "TerminoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Termino ");
         if (termino.Estatus == null)
            sCmd.Append(" SET Estatus = NULL ");
         else{ 
            sCmd.Append(" SET Estatus = @termino_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "termino_Estatus";
            sqlParam.Value = termino.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.TerminoID == null)
            sCmd.Append(" WHERE terminoID IS NULL ");
         else{ 
            sCmd.Append(" WHERE terminoID = @anterior_TerminoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_TerminoID";
            sqlParam.Value = anterior.TerminoID;
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
            throw new Exception("TerminoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("TerminoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
