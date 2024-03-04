using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;

namespace POV.CentroEducativo.DAO { 
   /// <summary>
   /// Consultar los Tipos de Niveles Educativos de la base de datos
   /// </summary>
   public class TipoNivelEducativoRetHlp { 
      /// <summary>
      /// Consulta registros de TipoNivelEducativo en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="tipoNivelEducativo">TipoNivelEducativo que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de TipoNivelEducativo generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, TipoNivelEducativo tipoNivelEducativo){
         object myFirm = new object();
         string sError = "";
         if (tipoNivelEducativo == null)
            sError += ", TipoNivelEducativo";
         if (sError.Length > 0)
            throw new Exception("TipoNivelEducativoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "TipoNivelEducativoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TipoNivelEducativoRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", 
         "TipoNivelEducativoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT TIPONIVELEDUCATIVOID,CLAVE,NOMBRE ");
         sCmd.Append(" FROM TIPONIVELEDUCATIVO ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (tipoNivelEducativo.TipoNivelEducativoID != null){
            s_VarWHERE.Append(" TIPONIVELEDUCATIVOID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = tipoNivelEducativo.TipoNivelEducativoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (tipoNivelEducativo.Clave != null){
            s_VarWHERE.Append(" AND Clave LIKE @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = tipoNivelEducativo.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (tipoNivelEducativo.Nombre != null){
            s_VarWHERE.Append(" AND Nombre LIKE @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = tipoNivelEducativo.Nombre;
            sqlParam.DbType = DbType.String;
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
         sCmd.Append(" ORDER BY TipoNivelEducativoID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "TipoNivelEducativo");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("TipoNivelEducativoRetHlp: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
