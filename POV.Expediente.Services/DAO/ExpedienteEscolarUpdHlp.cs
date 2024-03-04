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
   /// Actualiza un registro de ExpedienteEscolar en la BD
   /// </summary>
   public class ExpedienteEscolarUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de ExpedienteEscolarUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="expedienteEscolarUpdHlp">ExpedienteEscolarUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">ExpedienteEscolarUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, ExpedienteEscolar expedienteEscolar, ExpedienteEscolar anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (expedienteEscolar == null)
            sError += ", ExpedienteEscolar";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("ExpedienteEscolarUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.Alumno == null)
            sError += ", Anterior.Alumno";
         if (sError.Length > 0)
            throw new Exception("ExpedienteEscolarUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.ExpedienteEscolarID == null)
            sError += ", Anterior ExpedienteEscolarID";
         if (sError.Length > 0)
            throw new Exception("ExpedienteEscolarUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.Alumno.AlumnoID == null)
            sError += ", Anterior AlumnoID";
         if (sError.Length > 0)
            throw new Exception("ExpedienteEscolarUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "ExpedienteEscolarUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ExpedienteEscolarUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", 
         "ExpedienteEscolarUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE ExpedienteEscolar ");
         if (expedienteEscolar.Activo != null){
            sCmd.Append(" SET Activo = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = expedienteEscolar.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (expedienteEscolar.FechaRegistro != null){
            sCmd.Append(" ,FechaRegistro = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = expedienteEscolar.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (expedienteEscolar.Apuntes != null)
         {
             sCmd.Append(" ,Apuntes = @dbp4ram21 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram21";
             sqlParam.Value = expedienteEscolar.Apuntes;
             sqlParam.DbType = DbType.String;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.ExpedienteEscolarID != null)
         {
            sCmd.Append(" WHERE ExpedienteEscolarID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = anterior.ExpedienteEscolarID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Alumno.AlumnoID != null){
            sCmd.Append(" AND AlumnoID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = anterior.Alumno.AlumnoID;
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
            throw new Exception("ExpedienteEscolarUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ExpedienteEscolarUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
