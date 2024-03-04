using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;

namespace POV.Licencias.DAO { 
   /// <summary>
   /// Consulta un registro de RecursoContrato en la BD
   /// </summary>
   internal class RecursoContratoRetHlp { 
      /// <summary>
      /// Consulta registros de RecursoContrato en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="recursoContrato">RecursoContrato que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de RecursoContrato generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, CicloContrato cicloContrato, RecursoContrato recursoContrato){
         object myFirm = new object();
         string sError = String.Empty;
         if (recursoContrato == null)
            sError += ", RecursoContrato";
         if (sError.Length > 0)
            throw new Exception("RecursoContratoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (cicloContrato == null)
            sError += ", CicloContrato";
         if (sError.Length > 0)
            throw new Exception("RecursoContratoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", 
         "RecursoContratoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RecursoContratoRetHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO", 
         "RecursoContratoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT RecursoContratoID, CicloContratoID, EsAsignacionManual, EsPaquetePorPruebaPivote, Activo, FechaRegistro ");
         sCmd.Append(" FROM RecursoContrato ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (recursoContrato.RecursoContratoID != null){
            s_VarWHERE.Append(" RecursoContratoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = recursoContrato.RecursoContratoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (cicloContrato.CicloContratoID != null){
            s_VarWHERE.Append(" AND CicloContratoID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = cicloContrato.CicloContratoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (recursoContrato.EsAsignacionManual != null){
            s_VarWHERE.Append(" AND EsAsignacionManual = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = recursoContrato.EsAsignacionManual;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (recursoContrato.EsPaquetePorPruebaPivote != null){
            s_VarWHERE.Append(" AND EsPaquetePorPruebaPivote = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = recursoContrato.EsPaquetePorPruebaPivote;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (recursoContrato.Activo != null){
            s_VarWHERE.Append(" AND Activo = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = recursoContrato.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (recursoContrato.FechaRegistro != null){
            s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = recursoContrato.FechaRegistro;
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
            sqlAdapter.Fill(ds, "RecursoContrato");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RecursoContratoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}