using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Expediente.BO;

namespace POV.Expediente.DAO { 
   /// <summary>
   /// Actualiza un registro de DetalleCicloEscolar en la BD
   /// </summary>
   public class DetalleCicloEscolarUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de DetalleCicloEscolarUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="detalleCicloEscolarUpdHlp">DetalleCicloEscolarUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">DetalleCicloEscolarUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, DetalleCicloEscolar anterior, ExpedienteEscolar expedienteEscolar){
         object myFirm = new object();
         String sError = string.Empty;
         if (detalleCicloEscolar == null)
            sError += ", DetalleCicloEscolar";
         if (anterior == null)
            sError += ", Anterior";
         if (expedienteEscolar == null)
            sError += ", ExpedienteEscolar";
         if (sError.Length > 0)
            throw new Exception("DetalleCicloEscolarUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.DetalleCicloEscolarID == null)
            sError += ", Anterior DetalleCicloEscolarID";
         if (sError.Length > 0)
            throw new Exception("DetalleCicloEscolarUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (expedienteEscolar.ExpedienteEscolarID == null)
            sError += ", ExpedienteEscolarID";
         if (sError.Length > 0)
            throw new Exception("DetalleCicloEscolarUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "DetalleCicloEscolarUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "DetalleCicloEscolarUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", 
         "DetalleCicloEscolarUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE DetalleCicloEscolar ");
         if (detalleCicloEscolar.Activo != null){
            sCmd.Append(" SET Activo = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = detalleCicloEscolar.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (expedienteEscolar.ExpedienteEscolarID != null){
            sCmd.Append(" WHERE ExpedienteEscolarID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = expedienteEscolar.ExpedienteEscolarID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.DetalleCicloEscolarID != null){
            sCmd.Append(" AND DetalleCicloEscolarID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = anterior.DetalleCicloEscolarID;
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
            throw new Exception("DetalleCicloEscolarUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("DetalleCicloEscolarUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
