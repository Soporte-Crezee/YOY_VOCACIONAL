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
   /// Guarda un registro de ExpedienteEscolar en la BD
   /// </summary>
   public class ExpedienteEscolarInsHlp { 
      /// <summary>
      /// Crea un registro de ExpedienteEscolar en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="expedienteEscolar">ExpedienteEscolar que desea crear</param>
      public void Action(IDataContext dctx, ExpedienteEscolar expedienteEscolar){
         object myFirm = new object();
         string sError = String.Empty;
         if (expedienteEscolar == null)
            sError += ", ExpedienteEscolar";
         if (sError.Length > 0)
            throw new Exception("ExpedienteEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (expedienteEscolar.Alumno == null)
            sError += ", Alumno";
         if (sError.Length > 0)
            throw new Exception("ExpedienteEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (expedienteEscolar.Alumno.AlumnoID == null)
            sError += ", AlumnoID";
         if (sError.Length > 0)
            throw new Exception("ExpedienteEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (expedienteEscolar.Activo == null)
            sError += ", Activo";
         if (expedienteEscolar.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (sError.Length > 0)
            throw new Exception("ExpedienteEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "ExpedienteEscolarInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ExpedienteEscolarInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "ExpedienteEscolarInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO ExpedienteEscolar (AlumnoID, Activo, FechaRegistro) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // expedienteEscolar.Alumno.AlumnoID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (expedienteEscolar.Alumno.AlumnoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = expedienteEscolar.Alumno.AlumnoID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // expedienteEscolar.Activo
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (expedienteEscolar.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = expedienteEscolar.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // expedienteEscolar.FechaRegistro
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (expedienteEscolar.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = expedienteEscolar.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ExpedienteEscolarInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ExpedienteEscolarInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
