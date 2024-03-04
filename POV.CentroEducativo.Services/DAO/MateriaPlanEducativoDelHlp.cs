using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;

namespace POV.CentroEducativo.DAO { 
   /// <summary>
   /// Elimina un registro de la asignacion de una materia a un PlanEducativo en la BD
   /// </summary>
   public class MateriaPlanEducativoDelHlp { 
      /// <summary>
      /// Elimina un registro de MateriaPlanEducativoDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="materiaPlanEducativoDelHlp">MateriaPlanEducativoDelHlp que desea eliminar</param>
      public void Action(IDataContext dctx, Materia materia,PlanEducativo planEducativo){
         object myFirm = new object();
         string sError = String.Empty;
         if (planEducativo == null)
            sError += ", PlanEducativo";
         if (materia == null)
            sError += ", Materia";
         if (sError.Length > 0)
            throw new Exception("MateriaPlanEducativoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (planEducativo.PlanEducativoID == null)
            sError += ", PlanEducativoID";
         if (materia.MateriaID == null)
            sError += ", MateriaID";
         if (sError.Length > 0)
            throw new Exception("MateriaPlanEducativoDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "MateriaPlanEducativoDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "MateriaPlanEducativoDelHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "MateriaPlanEducativoDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" DELETE FROM MateriaPlanEducativo ");
         // planEducativo.PlanEducativoID
         sCmd.Append(" WHERE PlanEducativoID=@dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (planEducativo.PlanEducativoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = planEducativo.PlanEducativoID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // materia.MateriaID
         sCmd.Append(" ,MateriaID=@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (materia.MateriaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = materia.MateriaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("MateriaPlanEducativoDelHlp: Ocurrió un error al eliminar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("MateriaPlanEducativoDelHlp: Ocurrió un error al eliminar el registro.");
      }
   } 
}
