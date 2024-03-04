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
   /// Guarda un registro de un Plan Educativo en la BD
   /// </summary>
   public class PlanEducativoInsHlp { 
      /// <summary>
      /// Crea un registro de PlanEducativo en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="planEducativo">PlanEducativo que desea crear</param>
      public void Action(IDataContext dctx, PlanEducativo planEducativo){
         object myFirm = new object();
         string sError = String.Empty;
         if (planEducativo == null)
            sError += ", PlanEducativo";
         if (sError.Length > 0)
            throw new Exception("PlanEducativoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (planEducativo.Titulo == null)
            sError += ", Titulo";
         if (planEducativo.Descripcion == null)
            sError += ", Descripcion";
         if (planEducativo.ValidoDesde == null)
            sError += ", ValidoDesde";
         if (planEducativo.ValidoHasta == null)
            sError += ", ValidoHasta";
         if (planEducativo.NivelEducativo == null)
            sError += ", NivelEducativo";
         if (sError.Length > 0)
            throw new Exception("PlanEducativoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (planEducativo.NivelEducativo.NivelEducativoID == null)
            sError += ", NivelEducativoID";
         if (sError.Length > 0)
            throw new Exception("PlanEducativoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "PlanEducativoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PlanEducativoInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "PlanEducativoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO PLANEDUCATIVO (Titulo,Descripcion,ValidoDesde,ValidoHasta,NivelEducativoID, Estatus) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // planEducativo.Titulo
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (planEducativo.Titulo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = planEducativo.Titulo;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // planEducativo.Descripcion
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (planEducativo.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = planEducativo.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // planEducativo.ValidoDesde
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (planEducativo.ValidoDesde == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = planEducativo.ValidoDesde;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // planEducativo.ValidoHasta
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (planEducativo.ValidoHasta == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = planEducativo.ValidoHasta;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // planEducativo.NivelEducativo.NivelEducativoID
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (planEducativo.NivelEducativo.NivelEducativoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = planEducativo.NivelEducativo.NivelEducativoID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // Estatus
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
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
            throw new Exception("PlanEducativoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PlanEducativoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
