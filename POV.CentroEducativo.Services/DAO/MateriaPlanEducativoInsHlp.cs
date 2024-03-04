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
   /// Guarda un registro de la asignacion de una materia al Plan Educativo en la BD
   /// </summary>
   public class MateriaPlanEducativoInsHlp { 
      /// <summary>
      /// Crea un registro de MateriaPlanEducativo en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="materiaPlanEducativo">MateriaPlanEducativo que desea crear</param>
      public void Action(IDataContext dctx, Materia materia,PlanEducativo planEducativo){
         object myFirm = new object();
         string sError = String.Empty;
         if (planEducativo == null)
            sError += ", PlanEducativo";
         if (materia == null)
            sError += ", Materia";
         if (sError.Length > 0)
            throw new Exception("MateriaPlanEducativoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (planEducativo.PlanEducativoID == null)
            sError += ", PlanEducativoID";
         if (materia.MateriaID == null)
            sError += ", MateriaID";
         if (sError.Length > 0)
            throw new Exception("MateriaPlanEducativoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (sError.Length > 0)
            throw new Exception("MateriaPlanEducativoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "MateriaPlanEducativoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PlanEducativoInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "MateriaPlanEducativoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO MATERIAPLANEDUCATIVO (MATERIAID,PLANEDUCATIVOID, ESTATUS) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // materia.MateriaID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (materia.MateriaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = materia.MateriaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // planEducativo.PlanEducativoID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (planEducativo.PlanEducativoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = planEducativo.PlanEducativoID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
          // Estatus
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         sqlParam.Value = true;
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
            throw new Exception("MateriaPlanEducativoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("MateriaPlanEducativoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
