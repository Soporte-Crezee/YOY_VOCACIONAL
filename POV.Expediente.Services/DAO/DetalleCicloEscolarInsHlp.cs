using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Expediente.BO;

namespace POV.Expediente.DAO
{ 
   /// <summary>
   /// Guarda un registro de DetalleCicloEscolar en la BD
   /// </summary>
   public class DetalleCicloEscolarInsHlp { 
      /// <summary>
      /// Crea un registro de DetalleCicloEscolar en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="detalleCicloEscolar">DetalleCicloEscolar que desea crear</param>
      public void Action(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, ExpedienteEscolar expedienteEscolar){
         object myFirm = new object();
         string sError = String.Empty;
         if (detalleCicloEscolar == null)
            sError += ", DetalleCicloEscolar";
         if (sError.Length > 0)
            throw new Exception("DetalleCicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (expedienteEscolar == null)
            sError += ", ExpedienteEscolar";
         if (sError.Length > 0)
            throw new Exception("DetalleCicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (expedienteEscolar.ExpedienteEscolarID == null)
            sError += ", ExpedienteEscolarID";
         if (sError.Length > 0)
            throw new Exception("DetalleCicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (detalleCicloEscolar.GrupoCicloEscolar == null)
            sError += ", GrupoCicloEscolar";
         if (sError.Length > 0)
            throw new Exception("DetalleCicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (detalleCicloEscolar.GrupoCicloEscolar.GrupoCicloEscolarID == null)
            sError += ", GrupoCicloEscolarID";
         if (sError.Length > 0)
            throw new Exception("DetalleCicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (detalleCicloEscolar.Escuela == null)
            sError += ", Escuela";
         if (sError.Length > 0)
            throw new Exception("DetalleCicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (detalleCicloEscolar.Escuela.EscuelaID == null)
            sError += ", EscuelaID";
         if (sError.Length > 0)
            throw new Exception("DetalleCicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (detalleCicloEscolar.Activo == null)
            sError += ", Activo";
         if (detalleCicloEscolar.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (sError.Length > 0)
            throw new Exception("DetalleCicloEscolarInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "DetalleCicloEscolarInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "DetalleCicloEscolarInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "DetalleCicloEscolarInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO DetalleCicloEscolar (ExpedienteEscolarID, GrupoCicloEscolarID, EscuelaID, Activo, FechaRegistro) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // expedienteEscolar.ExpedienteEscolarID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (expedienteEscolar.ExpedienteEscolarID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = expedienteEscolar.ExpedienteEscolarID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // detalleCicloEscolar.GrupoCicloEscolar.GrupoCicloEscolarID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (detalleCicloEscolar.GrupoCicloEscolar.GrupoCicloEscolarID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = detalleCicloEscolar.GrupoCicloEscolar.GrupoCicloEscolarID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         // detalleCicloEscolar.Escuela.EscuelaID
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (detalleCicloEscolar.Escuela.EscuelaID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = detalleCicloEscolar.Escuela.EscuelaID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // detalleCicloEscolar.Activo
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (detalleCicloEscolar.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = detalleCicloEscolar.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // detalleCicloEscolar.FechaRegistro
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (detalleCicloEscolar.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = detalleCicloEscolar.FechaRegistro;
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
            throw new Exception("DetalleCicloEscolarInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("DetalleCicloEscolarInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
