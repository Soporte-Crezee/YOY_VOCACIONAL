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
   /// Consulta un registro de TipoServicio en la BD
   /// </summary>
   public class TipoServicioRetHlp { 
      /// <summary>
      /// Consulta registros de TipoServicio en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="tipoServicio">TipoServicio que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de TipoServicio generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, TipoServicio tipoServicio){
         object myFirm = new object();
         string sError = String.Empty;
         if (tipoServicio.NivelEducativoID == null)
         {
             tipoServicio.NivelEducativoID = new NivelEducativo();
         }
         if (tipoServicio == null)
            sError += ", TipoServicio";
         if (sError.Length > 0)
            throw new Exception("TipoServicioRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "TipoServicioRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TipoServicioRetHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "TipoServicioRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT TipoServicioID,Clave,Nombre,NivelEducativoID ");
         sCmd.Append(" FROM TipoServicio ");
         StringBuilder s_Var = new StringBuilder();
         if (tipoServicio.TipoServicioID != null){
            s_Var.Append(" tipoServicioID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = tipoServicio.TipoServicioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (tipoServicio.Clave != null){
            s_Var.Append(" AND Clave = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = tipoServicio.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (tipoServicio.Nombre != null){
            s_Var.Append(" AND Nombre = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = tipoServicio.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (tipoServicio.NivelEducativoID.NivelEducativoID != null){
            s_Var.Append(" AND NivelEducativoID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = tipoServicio.NivelEducativoID.NivelEducativoID;
            sqlParam.DbType = DbType.Int32;
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
            sCmd.Append(" WHERE " + s_Varres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "TipoServicio");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("TipoServicioRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
