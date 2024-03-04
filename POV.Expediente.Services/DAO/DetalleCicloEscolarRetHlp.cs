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
   /// Consulta un registro de DetalleCicloEscolar en la BD
   /// </summary>
   public class DetalleCicloEscolarRetHlp { 
      /// <summary>
      /// Consulta registros de DetalleCicloEscolar en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="detalleCicloEscolar">DetalleCicloEscolar que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de DetalleCicloEscolar generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, DetalleCicloEscolar detalleCicloEscolar, ExpedienteEscolar expedienteEscolar){
         object myFirm = new object();
         string sError = String.Empty;
         if (detalleCicloEscolar == null)
            sError += ", DetalleCicloEscolar";
         if (sError.Length > 0)
            throw new Exception("DetalleCicloEscolarRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (expedienteEscolar == null)
            sError += ", ExpedienteEscolar";
         if (sError.Length > 0)
            throw new Exception("DetalleCicloEscolarRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (detalleCicloEscolar.GrupoCicloEscolar == null) {
         detalleCicloEscolar.GrupoCicloEscolar = new GrupoCicloEscolar();
      }
      if (detalleCicloEscolar.Escuela == null) {
         detalleCicloEscolar.Escuela = new Escuela();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "DetalleCicloEscolarRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "DetalleCicloEscolarRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "DetalleCicloEscolarRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT DetalleCicloEscolarID, ExpedienteEscolarID, GrupoCicloEscolarID, EscuelaID, Activo, FechaRegistro ");
         sCmd.Append(" FROM DetalleCicloEscolar ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (detalleCicloEscolar.DetalleCicloEscolarID != null){
            s_VarWHERE.Append(" DetalleCicloEscolarID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = detalleCicloEscolar.DetalleCicloEscolarID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (expedienteEscolar.ExpedienteEscolarID != null){
            s_VarWHERE.Append(" AND ExpedienteEscolarID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = expedienteEscolar.ExpedienteEscolarID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (detalleCicloEscolar.GrupoCicloEscolar.GrupoCicloEscolarID != null){
            s_VarWHERE.Append(" AND GrupoCicloEscolarID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = detalleCicloEscolar.GrupoCicloEscolar.GrupoCicloEscolarID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (detalleCicloEscolar.Escuela.EscuelaID != null){
            s_VarWHERE.Append(" AND EscuelaID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = detalleCicloEscolar.Escuela.EscuelaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (detalleCicloEscolar.Activo != null){
            s_VarWHERE.Append(" AND Activo = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = detalleCicloEscolar.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (detalleCicloEscolar.FechaRegistro != null){
            s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = detalleCicloEscolar.FechaRegistro;
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
            sqlAdapter.Fill(ds, "DetalleCicloEscolar");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("DetalleCicloEscolarRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
