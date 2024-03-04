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
   /// Consultar los Niveles Educativos de la base de datos
   /// </summary>
   public class NivelEducativoRetHlp { 
      /// <summary>
      /// Consulta registros de NivelEducativo en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="nivelEducativo">NivelEducativo que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de NivelEducativo generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, NivelEducativo nivelEducativo){
         object myFirm = new object();
         string sError = "";
         if (nivelEducativo == null)
            sError += ", NivelEducativo";
         if (sError.Length > 0)
            throw new Exception("NivelEducativoRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
      if (nivelEducativo.TipoNivelEducativoID == null) {
         nivelEducativo.TipoNivelEducativoID = new TipoNivelEducativo();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "NivelEducativoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "NivelEducativoRetHlp: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", 
         "NivelEducativoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT NIVELEDUCATIVOID,TITULO,DESCRIPCION,NUMEROGRADOS,TIPONIVELEDUCATIVOID ");
         sCmd.Append(" FROM NIVELEDUCATIVO ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (nivelEducativo.NivelEducativoID != null){
            s_VarWHERE.Append(" NIVELEDUCATIVOID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = nivelEducativo.NivelEducativoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (nivelEducativo.Titulo != null){
            s_VarWHERE.Append(" AND Titulo LIKE @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = nivelEducativo.Titulo;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (nivelEducativo.Descripcion != null){
            s_VarWHERE.Append(" AND Descripcion LIKE @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = nivelEducativo.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (nivelEducativo.NumeroGrados != null){
            s_VarWHERE.Append(" AND NumeroGrados = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = nivelEducativo.NumeroGrados;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (nivelEducativo.TipoNivelEducativoID.TipoNivelEducativoID != null){
            s_VarWHERE.Append(" AND TipoNivelEducativoID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = nivelEducativo.TipoNivelEducativoID.TipoNivelEducativoID;
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
         sCmd.Append(" ORDER BY NivelEducativoID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "NivelEducativo");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("NivelEducativoRetHlp: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
