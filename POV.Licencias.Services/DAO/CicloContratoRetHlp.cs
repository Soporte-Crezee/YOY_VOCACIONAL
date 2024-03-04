using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.CentroEducativo.BO;

namespace POV.Licencias.DAO
{ 
   /// <summary>
   /// Consulta un registro de CicloContrato en la BD
   /// </summary>
   internal class CicloContratoRetHlp { 
      /// <summary>
      /// Consulta registros de CicloContrato en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="cicloContrato">CicloContrato que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de CicloContrato generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato){
         object myFirm = new object();
         string sError = String.Empty;
         if (cicloContrato == null)
            sError += ", CicloContrato";
         if (sError.Length > 0)
            throw new Exception("CicloContratoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (contrato == null)
            sError += ", Contrato";
         if (sError.Length > 0)
            throw new Exception("CicloContratoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (cicloContrato.CicloEscolar == null) {
         cicloContrato.CicloEscolar = new CicloEscolar();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.DAO", 
         "CicloContratoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "CicloContratoRetHlp: No se pudo conectar a la base de datos", "POV.DAO", 
         "CicloContratoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT CicloContratoID, ContratoID, CicloEscolarID, EstaLiberado, Activo, FechaRegistro ");
         sCmd.Append(" FROM CicloContrato ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (cicloContrato.CicloContratoID != null){
            s_VarWHERE.Append(" CicloContratoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = cicloContrato.CicloContratoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (contrato.ContratoID != null){
            s_VarWHERE.Append(" AND ContratoID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = contrato.ContratoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (cicloContrato.CicloEscolar.CicloEscolarID != null){
            s_VarWHERE.Append(" AND CicloEscolarID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = cicloContrato.CicloEscolar.CicloEscolarID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (cicloContrato.EstaLiberado != null){
            s_VarWHERE.Append(" AND EstaLiberado = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = cicloContrato.EstaLiberado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (cicloContrato.Activo != null){
            s_VarWHERE.Append(" AND Activo = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = cicloContrato.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (cicloContrato.FechaRegistro != null){
            s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = cicloContrato.FechaRegistro;
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
            sqlAdapter.Fill(ds, "CicloContrato");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("CicloContratoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
