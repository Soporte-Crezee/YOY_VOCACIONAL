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
   /// Actualiza un registro de DocenteEscuela en la BD
   /// </summary>
   public class DocenteEscuelaUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de DocenteEscuelaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="docenteEscuelaUpdHlp">DocenteEscuelaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">DocenteEscuelaUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, DocenteEscuela docenteEscuela, DocenteEscuela anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (docenteEscuela == null)
            sError += ", DocenteEscuela";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("DocenteEscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.DocenteEscuelaID == null)
            sError += ", Anterior DocenteEscuelaID";
         if (sError.Length > 0)
            throw new Exception("DocenteEscuelaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "DocenteEscuelaUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "DocenteEscuelaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", 
         "DocenteEscuelaUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE DocenteEscuela ");
         if (docenteEscuela.EscuelaID == null)
            sCmd.Append(" SET EscuelaID = NULL ");
         else{ 
            sCmd.Append(" SET EscuelaID = @docenteEscuela_EscuelaID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "docenteEscuela_EscuelaID";
            sqlParam.Value = docenteEscuela.EscuelaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         sCmd.Append(" ,DocenteID=@docenteEscuela_DocenteID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "docenteEscuela_DocenteID";
         if (docenteEscuela.DocenteID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = docenteEscuela.DocenteID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,Estatus=@docenteEscuela_Estatus ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "docenteEscuela_Estatus";
         if (docenteEscuela.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = docenteEscuela.Estatus;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         if (anterior.DocenteEscuelaID == null)
            sCmd.Append(" WHERE DocenteEscuelaID IS NULL ");
         else{ 
            sCmd.Append(" WHERE DocenteEscuelaID = @anterior_DocenteEscuelaID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_DocenteEscuelaID";
            sqlParam.Value = anterior.DocenteEscuelaID;
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
            throw new Exception("DocenteEscuelaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("DocenteEscuelaUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
