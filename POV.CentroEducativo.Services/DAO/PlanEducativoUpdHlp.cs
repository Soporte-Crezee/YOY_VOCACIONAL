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
   /// Actualiza un registro de un PlanEducativo en la BD
   /// </summary>
   public class PlanEducativoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de PlanEducativoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="planEducativoUpdHlp">PlanEducativoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">PlanEducativoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, PlanEducativo planEducativo,PlanEducativo anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (planEducativo == null)
            sError += ", PlanEducativo";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("PlanEducativoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.PlanEducativoID == null)
            sError += ", Anterior PlanEducativoID";
         if (anterior.Titulo == null)
            sError += ", Titulo";
         if (anterior.Descripcion == null)
            sError += ", Descripcion";
         if (anterior.ValidoDesde == null)
            sError += ", ValidoDesde";
         if (anterior.ValidoHasta == null)
            sError += ", ValidoHasta";
         if (anterior.NivelEducativo == null)
            sError += ", NivelEducativo";
         if (planEducativo.Titulo == null)
            sError += ", Titulo";
         if (planEducativo.Descripcion == null)
            sError += ", Descripcion";
         if (planEducativo.ValidoDesde == null)
             sError += ", ValidoDesde";
         if (planEducativo.ValidoHasta == null)
             sError += ", validoHasta";
         if (planEducativo.NivelEducativo == null)
             sError += ", NivelEducativo";
         if (planEducativo.Estatus == null)
             sError += ", Estatus";
         if (sError.Length > 0)
            throw new Exception("PlanEducativoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "PlanEducativoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
             throw new StandardException(MessageType.Error, "", "PlanEducativoUpdHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", 
         "PlanEducativoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE PlanEducativo ");
         if (planEducativo.Titulo == null)
            sCmd.Append(" SET Titulo = NULL ");
         else{ 
            // planEducativo.Titulo
            sCmd.Append(" SET Titulo = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = planEducativo.Titulo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (planEducativo.Descripcion == null)
            sCmd.Append(" ,Descripcion = NULL ");
         else{ 
            // planEducativo.Descripcion
            sCmd.Append(" ,Descripcion = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = planEducativo.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (planEducativo.ValidoDesde == null)
             sCmd.Append(" ,ValidoDesde = NULL ");
         else
         {
             // planEducativo.ValidoDesde
             sCmd.Append(" ,ValidoDesde = @dbp4ram3 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram3";
             sqlParam.Value = planEducativo.ValidoDesde;
             sqlParam.DbType = DbType.DateTime;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (planEducativo.ValidoHasta == null)
             sCmd.Append(" ,ValidoHasta = NULL ");
         else
         {
             // planEducativo.ValidoHasta
             sCmd.Append(" ,ValidoHasta = @dbp4ram4 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram4";
             sqlParam.Value = planEducativo.ValidoHasta;
             sqlParam.DbType = DbType.DateTime;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (planEducativo.NivelEducativo == null)
             sCmd.Append(" ,NivelEducativoID = NULL ");
         else
         {
             // planEducativo.NivelEducativoID
             sCmd.Append(" ,NivelEducativoID = @dbp4ram5 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram5";
             sqlParam.Value = planEducativo.NivelEducativo.NivelEducativoID;
             sqlParam.DbType = DbType.Int32;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (planEducativo.Estatus == null)
             sCmd.Append(" ,Estatus = NULL ");
         else
         {
             // planEducativo.Titulo
             sCmd.Append(" ,Estatus = @dbp4ram6 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram6";
             sqlParam.Value = planEducativo.Estatus;
             sqlParam.DbType = DbType.Boolean;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.PlanEducativoID == null)
            sCmd.Append(" WHERE PlanEducativoID IS NULL ");
         else{ 
            // anterior.PlanEducativoID
            sCmd.Append(" WHERE PlanEducativoID = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = anterior.PlanEducativoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         // anterior.NivelEducativo.NivelEducativoID
         sCmd.Append(" AND NivelEducativoID =@dbp4ram8 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram8";
         if (anterior.NivelEducativo.NivelEducativoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.NivelEducativo.NivelEducativoID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.Titulo
         sCmd.Append(" AND Titulo =@dbp4ram9 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram9";
         if (anterior.Titulo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.Titulo;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.Descripcion
         sCmd.Append(" AND Descripcion =@dbp4ram10 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram10";
         if (anterior.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.ValidoDesde
         sCmd.Append(" AND ValidoDesde =@dbp4ram11 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram11";
         if (anterior.ValidoDesde == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.ValidoDesde;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // anterior.ValidoHasta
         sCmd.Append(" AND ValidoHasta =@dbp4ram12 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram12";
         if (anterior.ValidoHasta == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = anterior.ValidoHasta;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PlanEducativoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PlanEducativoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
