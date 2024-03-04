using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Consulta un registro de Termino en la BD
   /// </summary>
   public class TerminoRetHlp { 
      /// <summary>
      /// Consulta registros de Termino en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="termino">Termino que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Termino generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Termino termino){
         object myFirm = new object();
         string sError = String.Empty;
         if (termino == null)
            sError += ", Termino";
         if (sError.Length > 0)
            throw new Exception("TerminoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "TerminoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TerminoRetHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "TerminoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT TerminoID, Cuerpo, FechaCreacion, Estatus ");
         sCmd.Append(" FROM Termino ");
         sCmd.Append(" WHERE ");
         StringBuilder s_Var = new StringBuilder();
         if (termino.TerminoID != null){
            s_Var.Append(" TerminoID = @termino_TerminoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "termino_TerminoID";
            sqlParam.Value = termino.TerminoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (termino.Cuerpo != null){
            s_Var.Append(" AND Cuerpo= @termino_Cuerpo ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "termino_Cuerpo";
            sqlParam.Value = termino.Cuerpo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (termino.FechaCreacion != null){
            s_Var.Append(" AND FechaCreacion= @termino_FechaCreacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "termino_FechaCreacion";
            sqlParam.Value = termino.FechaCreacion;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (termino.Estatus != null){
            s_Var.Append(" AND Estatus= @termino_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "termino_Estatus";
            sqlParam.Value = termino.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         s_Var.Append("  ");
         string s_Varres = s_Var.ToString().Trim();
         if (s_Varres.Length > 0){
            if (s_Varres.StartsWith("AND "))
               s_Varres = s_Varres.Substring(4);
            else if (s_Varres.StartsWith("OR "))
               s_Varres = s_Varres.Substring(3);
            else if (s_Varres.StartsWith(","))
               s_Varres = s_Varres.Substring(1);
            sCmd.Append("  " + s_Varres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "Termino");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("TerminoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
