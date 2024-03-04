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
   /// Consulta un registro de ExpedienteEscolar en la BD
   /// </summary>
   public class ExpedienteEscolarRetHlp { 
      /// <summary>
      /// Consulta registros de ExpedienteEscolar en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="expedienteEscolar">ExpedienteEscolar que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de ExpedienteEscolar generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, ExpedienteEscolar expedienteEscolar){
         object myFirm = new object();
         string sError = String.Empty;
         if (expedienteEscolar == null)
            sError += ", ExpedienteEscolar";
         if (sError.Length > 0)
            throw new Exception("ExpedienteEscolarRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (expedienteEscolar.Alumno == null) {
         expedienteEscolar.Alumno = new Alumno();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "ExpedienteEscolarRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ExpedienteEscolarRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "ExpedienteEscolarRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT ExpedienteEscolarID, AlumnoID, Activo, FechaRegistro, Apuntes ");
         sCmd.Append(" FROM ExpedienteEscolar ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (expedienteEscolar.ExpedienteEscolarID != null){
            s_VarWHERE.Append(" ExpedienteEscolarID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = expedienteEscolar.ExpedienteEscolarID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (expedienteEscolar.Alumno.AlumnoID != null){
            s_VarWHERE.Append(" AND AlumnoID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = expedienteEscolar.Alumno.AlumnoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (expedienteEscolar.Activo != null){
            s_VarWHERE.Append(" AND Activo = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = expedienteEscolar.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (expedienteEscolar.FechaRegistro != null){
            s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = expedienteEscolar.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            else if (s_VarWHEREres.StartsWith(","))
               s_VarWHEREres = s_VarWHEREres.Substring(1);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "ExpedienteEscolar");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ExpedienteEscolarRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
