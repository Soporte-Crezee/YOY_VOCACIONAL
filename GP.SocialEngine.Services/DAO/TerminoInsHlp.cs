using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Guarda un registro de Termino en la BD
   /// </summary>
   public class TerminoInsHlp { 
      /// <summary>
      /// Crea un registro de Termino en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="termino">Termino que desea crear</param>
      public void Action(IDataContext dctx, Termino termino){
         object myFirm = new object();
         string sError = String.Empty;
         if (termino == null)
            sError += ", Termino";
         if (termino.Cuerpo == null)
            sError += ", Termino.Cuerpo";
         if (termino.FechaCreacion == null)
            sError += ", termino.FechaCreacion";
         if (termino.Estatus == null)
            sError += ", Termino.Estatus";
         if (sError.Length > 0)
            throw new Exception("TerminoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "TerminoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TerminoInsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "TerminoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO Termino (Cuerpo, FechaCreacion, Estatus) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         sCmd.Append(" @termino_Cuerpo ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "termino_Cuerpo";
         if (termino.Cuerpo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = termino.Cuerpo;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@termino_FechaCreacion ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "termino_FechaCreacion";
         if (termino.FechaCreacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = termino.FechaCreacion;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@termino_Estatus ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "termino_Estatus";
         if (termino.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = termino.Estatus;
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
            throw new Exception("TerminoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("TerminoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
