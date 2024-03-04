using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;
using POV.ContenidosDigital.DAO;

namespace POV.ContenidosDigital.DA { 
   /// <summary>
   /// Consulta un registro de Visor en la BD
   /// </summary>
   internal class VisorTipoDocumentoRetHlp { 
      /// <summary>
      /// Consulta registros de Visor en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="visor">Visor que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Visor generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, int? visorID, int? tipoDocumentoID){
         object myFirm = new object();
         string sError = String.Empty;
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.DA", 
         "VisorTipoDocumentoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "VisorTipoDocumentoRetHlp: No se pudo conectar a la base de datos", "POV.ContenidosDigital.DA", 
         "VisorTipoDocumentoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT VisorID, TipoDocumentoID ");
         sCmd.Append(" FROM VisorTipoDocumento ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (visorID != null){
            s_VarWHERE.Append(" VisorID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = visorID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (tipoDocumentoID != null){
            s_VarWHERE.Append(" AND TipoDocumentoID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = tipoDocumentoID;
            sqlParam.DbType = DbType.Int32;
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
            sqlAdapter.Fill(ds, "VisorTipoDocumento");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("VisorTipoDocumentoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
