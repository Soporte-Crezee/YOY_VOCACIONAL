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
   /// Actualiza un registro de una materia de un PlanEducativo en la BD
   /// </summary>
   public class MateriaPlanEducativoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de PlanEducativoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="planEducativoUpdHlp">PlanEducativoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">PlanEducativoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Materia materia,Materia matanterior,PlanEducativo planEducativo,PlanEducativo plananterior, Boolean status){
         object myFirm = new object();
         String sError = string.Empty;
         if (planEducativo == null)
            sError += ", PlanEducativo";
         if (plananterior == null)
            sError += ", PlanEducativoAnterior";
         if (materia == null)
            sError += ", Materia";
         if (matanterior == null)
            sError += ", MateriaAnterior";
         if (sError.Length > 0)
            throw new Exception("PlanEducativoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (plananterior.PlanEducativoID == null)
            sError += ", Anterior PlanEducativoID";
         if (planEducativo.PlanEducativoID == null)
            sError += ", PlanEducativoID";
         if (matanterior == null)
            sError += ", Anterior MateriaID";
         if (materia.MateriaID == null)
            sError += ", MateriaID";
         if (sError.Length > 0)
            throw new Exception("MateriaPlanEducativoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "MateriaPlanEducativoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "MateriaPlanEducativoUpdHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", 
         "MateriaPlanEducativoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE MateriaPlanEducativo ");
         if (materia.MateriaID == null)
            sCmd.Append(" SET MateriaID = NULL ");
         else{ 
            // materia.MateriaID
            sCmd.Append(" SET MateriaID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = materia.MateriaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (planEducativo.PlanEducativoID == null)
            sCmd.Append(" ,PlanEducativoID = NULL ");
         else{ 
            // planEducativo.PlanEducativoID
            sCmd.Append(" ,PlanEducativoID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = planEducativo.PlanEducativoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         // status
         sCmd.Append(" ,Estatus = @dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         sqlParam.Value = status;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);

         if (plananterior.PlanEducativoID == null)
            sCmd.Append(" WHERE PlanEducativoID IS NULL ");
         else{ 
            // plananterior.PlanEducativoID
            sCmd.Append(" WHERE PlanEducativoID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = plananterior.PlanEducativoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         // matanterior.MateriaID
         sCmd.Append(" AND MateriaID =@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (matanterior.MateriaID == null)
             sqlParam.Value = DBNull.Value;
         else
         {
             sqlParam.Value = matanterior.MateriaID;
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
            throw new Exception("MateriaPlanEducativoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("MateriaPlanEducativoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
