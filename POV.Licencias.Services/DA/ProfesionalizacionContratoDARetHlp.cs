using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.Profesionalizacion.BO;

namespace POV.Licencias.DA { 
   /// <summary>
   /// Consulta un registro de ProfesionalizacionContrato en la BD
   /// </summary>
   internal class ProfesionalizacionContratoDARetHlp { 
      /// <summary>
      /// Consulta registros de ProfesionalizacionContrato en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="profesionalizacionContrato">ProfesionalizacionContrato que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de ProfesionalizacionContrato generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Contrato contrato, EjeTematico ejeTematico){
         object myFirm = new object();
         string sError = String.Empty;
         if (contrato == null)
            sError += ", Contrato";
         if (sError.Length > 0)
            throw new Exception("ProfesionalizacionContratoDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA", 
         "ProfesionalizacionContratoDARetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ProfesionalizacionContratoRetHlp: No se pudo conectar a la base de datos", "POV.Licencias.DA", 
         "ProfesionalizacionContratoDARetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT EjeTematicoID,ContratoID ");
         sCmd.Append(" FROM EjeContrato ");
         sCmd.Append(" WHERE ");
         StringBuilder s_Var = new StringBuilder();
         if (contrato.ContratoID != null){
            s_Var.Append(" ContratoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = contrato.ContratoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ejeTematico !=null && ejeTematico.EjeTematicoID != null)
         {
             s_Var.Append(" AND EjeTematicoID = @dbp4ram2 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram2";
             sqlParam.Value = ejeTematico.EjeTematicoID;
             sqlParam.DbType = DbType.Int64;
             sqlCmd.Parameters.Add(sqlParam);
         }
         s_Var.Append("  ");
         string s_Varres = s_Var.ToString().Trim();
         if (s_Varres.Length > 0){
            if (s_Varres.StartsWith("AND "))
               s_Varres = s_Varres.Substring(4);
            else if (s_Varres.StartsWith("OR "))
               s_Varres = s_Varres.Substring(3);
            else if (s_Varres.StartsWith(","))
               s_Varres = s_Varres.Substring(1);
            sCmd.Append("  " + s_Varres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "ProfesionalizacionContrato");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ProfesionalizacionContratoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
