using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Expediente.BO;
using POV.CentroEducativo.BO;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.DA { 
   /// <summary>
   /// Consulta un registro de AResultadoPrueba en la BD
   /// </summary>
   internal class ResultadoPruebaDARetHlp { 
      /// <summary>
      /// Consulta registros de AResultadoPrueba en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="aResultadoPrueba">AResultadoPrueba que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de AResultadoPrueba generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Alumno alumno, Escuela escuela, GrupoCicloEscolar grupoCicloEscolar, APrueba prueba, ETipoResultadoPrueba? tipoResultado){
         object myFirm = new object();
         string sError = String.Empty;
         if (prueba == null)
            sError += ", APrueba";
         if (alumno == null)
            sError += ", Alumno";
         if (escuela == null)
            sError += ", Escuela";
         if (grupoCicloEscolar == null)
            sError += ", GrupoCicloEscolar";
         if (sError.Length > 0)
            throw new Exception("ResultadoPruebaDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (prueba.PruebaID == null)
            sError += ", PruebaID";
         if (alumno.AlumnoID == null)
            sError += ", AlumnoID";
         if (escuela.EscuelaID == null)
            sError += ", EscuelaID";
         if (grupoCicloEscolar.GrupoCicloEscolarID == null)
            sError += ", GrupoCicloEscolarID";
         if (sError.Length > 0)
            throw new Exception("ResultadoPruebaDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.PruebaDiagnostico.DA", 
         "ResultadoPruebaDARetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ResultadoPruebaDARetHlp: No se pudo conectar a la base de datos", "POV.PruebaDiagnostico.DA", 
         "ResultadoPruebaDARetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT resp.ResultadoPruebaID, resp.DetalleCicloEscolarID, resp.FechaRegistro, resp.Tipo, resp.PruebaID, pru.Tipo as TipoPrueba ");
         sCmd.Append(" FROM ResultadoPrueba resp ");
         sCmd.Append(" INNER JOIN DetalleCicloEscolar dce ON dce.DetalleCicloEscolarID = resp.DetalleCicloEscolarID ");
         sCmd.Append(" INNER JOIN ExpedienteEscolar exp ON exp.ExpedienteEscolarID = dce.ExpedienteEscolarID ");
         sCmd.Append(" INNER JOIN Prueba pru ON pru.PruebaID = resp.PruebaID ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (prueba.PruebaID != null){
            s_VarWHERE.Append(" resp.PruebaID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = prueba.PruebaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (tipoResultado != null)
         {
             s_VarWHERE.Append(" AND resp.Tipo = @dbp4ram221 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram221";
             sqlParam.Value = tipoResultado;
             sqlParam.DbType = DbType.Byte;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (escuela.EscuelaID != null){
            s_VarWHERE.Append(" AND dce.EscuelaID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = escuela.EscuelaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (grupoCicloEscolar.GrupoCicloEscolarID != null){
            s_VarWHERE.Append(" AND dce.GrupoCicloEscolarID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = grupoCicloEscolar.GrupoCicloEscolarID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (alumno.AlumnoID != null){
            s_VarWHERE.Append(" AND exp.AlumnoID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = alumno.AlumnoID;
            sqlParam.DbType = DbType.Int64;
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
            sqlAdapter.Fill(ds, "ResultadoPrueba");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ResultadoPruebaDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
