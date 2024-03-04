// Clase motor de red social
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.Localizacion.Service;
using POV.Localizacion.BO;
namespace POV.CentroEducativo.DAO { 
   /// <summary>
   /// Consulta un registro de DocenteEscuela en la BD
   /// </summary>
   public class DocenteEscuelaRetHlp { 
      /// <summary>
      /// Consulta registros de DocenteEscuela en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docenteEscuela">DocenteEscuela que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de DocenteEscuela generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, DocenteEscuela docenteEscuela){
         object myFirm = new object();
         string sError = String.Empty;
         if (docenteEscuela == null)
            sError += ", DocenteEscuela";
         if (sError.Length > 0)
            throw new Exception("DocenteEscuelaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "DocenteEscuelaRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "DocenteEscuelaRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "DocenteEscuelaRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT DocenteID, EscuelaID, DocenteEscuelaID, Estatus ");
         sCmd.Append(" FROM DocenteEscuela ");
         sCmd.Append(" WHERE ");
         StringBuilder s_Var = new StringBuilder();
         if (docenteEscuela.DocenteEscuelaID != null){
            s_Var.Append(" DocenteEscuelaID = @docenteEscuela_DocenteEscuelaID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "docenteEscuela_DocenteEscuelaID";
            sqlParam.Value = docenteEscuela.DocenteEscuelaID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (docenteEscuela.EscuelaID != null){
            s_Var.Append(" AND EscuelaID =@docenteEscuela_EscuelaID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "docenteEscuela_EscuelaID";
            sqlParam.Value = docenteEscuela.EscuelaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (docenteEscuela.DocenteID != null){
            s_Var.Append(" AND DocenteID =@docenteEscuela_DocenteID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "docenteEscuela_DocenteID";
            sqlParam.Value = docenteEscuela.DocenteID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (docenteEscuela.Estatus != null){
            s_Var.Append(" AND Estatus =@docenteEscuela_Estatus ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "docenteEscuela_Estatus";
            sqlParam.Value = docenteEscuela.Estatus;
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
            sqlAdapter.Fill(ds, "DocenteEscuela");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("DocenteEscuelaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
