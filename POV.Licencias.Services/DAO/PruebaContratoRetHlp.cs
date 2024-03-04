using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;

namespace POV.Licencias.DAO { 
   /// <summary>
   /// Consulta un registro de PruebaContrato en la BD
   /// </summary>
   internal class PruebaContratoRetHlp { 
      /// <summary>
      /// Consulta registros de PruebaContrato en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="pruebaContrato">PruebaContrato que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de PruebaContrato generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, RecursoContrato recursoContrato, PruebaContrato pruebaContrato){
         object myFirm = new object();
         string sError = String.Empty;
         if (recursoContrato == null)
             sError += ", RecursoContrato";
         if (sError.Length > 0)
            throw new Exception("PruebaContratoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (pruebaContrato == null)
            sError += ", PruebaContrato";
         if (sError.Length > 0)
            throw new Exception("PruebaContratoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", 
         "PruebaContratoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PruebaContratoRetHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO", 
         "PruebaContratoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT pc.PruebaContratoID, pc.RecursoContratoID, pc.PruebaID, pc.TipoPruebaContrato, pc.FechaRegistro, pc.Activo, p.Tipo, p.TipoPruebaPresentacion, p.Espremium ");
         sCmd.Append(" FROM PruebaContrato pc ");
         sCmd.Append(" JOIN Prueba p ON p.PruebaID = pc.PruebaID ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (pruebaContrato.PruebaContratoID != null)
         {
             s_VarWHERE.Append(" pc.PruebaContratoID = @dbp4ram01 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram01";
             sqlParam.Value = pruebaContrato.PruebaContratoID;
             sqlParam.DbType = DbType.Int64;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (recursoContrato.RecursoContratoID != null)
         {
             s_VarWHERE.Append(" AND pc.RecursoContratoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = recursoContrato.RecursoContratoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pruebaContrato.Prueba != null)
             if (pruebaContrato.Prueba.PruebaID != null){
                 s_VarWHERE.Append(" AND pc.PruebaID = @dbp4ram2 ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "dbp4ram2";
                sqlParam.Value = pruebaContrato.Prueba.PruebaID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add(sqlParam);
             }
         if (pruebaContrato.TipoPruebaContrato != null){
             s_VarWHERE.Append(" AND pc.TipoPruebaContrato = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = pruebaContrato.TipoPruebaContrato;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pruebaContrato.FechaRegistro != null){
             s_VarWHERE.Append(" AND pc.FechaRegistro = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = pruebaContrato.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pruebaContrato.Activo != null){
             s_VarWHERE.Append(" AND pc.Activo = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = pruebaContrato.Activo;
            sqlParam.DbType = DbType.Boolean;
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
            sqlAdapter.Fill(ds, "PruebaContrato");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PruebaContratoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
