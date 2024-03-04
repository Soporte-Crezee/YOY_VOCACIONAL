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
   /// Elimina un registro de DocenteEscuela en la BD
   /// </summary>
   public class DocenteEscuelaDelHlp { 
      /// <summary>
      /// Elimina un registro de DocenteEscuelaDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docenteEscuelaDelHlp">DocenteEscuelaDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, DocenteEscuela docenteEscuela){
         object myFirm = new object();
         string sError = String.Empty;
         if (docenteEscuela == null)
            sError += ", DocenteEscuela";
         if (sError.Length > 0)
            throw new Exception("DocenteEscuelaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (docenteEscuela.DocenteEscuelaID == null)
            sError += ", DocenteEscuelaID";
         if (sError.Length > 0)
            throw new Exception("DocenteEscuelaDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "DocenteEscuelaDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "DocenteEscuelaDelHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "DocenteEscuelaDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM DocenteEscuela ");
         sCmd.Append(" WHERE @docenteEscuela_DocenteEscuelaID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "docenteEscuela_DocenteEscuelaID";
         if (docenteEscuela.DocenteEscuelaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = docenteEscuela.DocenteEscuelaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("DocenteEscuelaDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("DocenteEscuelaDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
