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
   /// Guarda un registro de DocenteEscuela en la BD
   /// </summary>
   public class DocenteEscuelaInsHlp { 
      /// <summary>
      /// Crea un registro de DocenteEscuela en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docenteEscuela">DocenteEscuela que desea crear</param>
      public void Action(IDataContext dctx, DocenteEscuela docenteEscuela){
         object myFirm = new object();
         string sError = String.Empty;
         if (docenteEscuela == null)
            sError += ", DocenteEscuela";
         if (sError.Length > 0)
            throw new Exception("DocenteEscuelaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "DocenteEscuelaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "DocenteEscuelaInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "DocenteEscuelaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO DocenteEscuela (DocenteID, EscuelaID, Estatus) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         sCmd.Append(" @docenteEscuela_DocenteID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "docenteEscuela_DocenteID";
         if (docenteEscuela.DocenteID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = docenteEscuela.DocenteID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@docenteEscuela_EscuelaID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "docenteEscuela_EscuelaID";
         if (docenteEscuela.EscuelaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = docenteEscuela.EscuelaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@docenteEscuela_Estatus ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "docenteEscuela_Estatus";
         if (docenteEscuela.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = docenteEscuela.Estatus;
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
            throw new Exception("DocenteEscuelaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("DocenteEscuelaInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
