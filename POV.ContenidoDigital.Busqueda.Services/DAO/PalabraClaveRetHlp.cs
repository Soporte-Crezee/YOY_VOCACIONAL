using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.Busqueda.BO;

namespace POV.ContenidosDigital.Busqueda.DAO 
{ 
   /// <summary>
   /// Consulta un registro de PalabraClave en la BD
   /// </summary>
   internal class PalabraClaveRetHlp 
   { 
      /// <summary>
      /// Consulta registros de PalabraClave en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="palabraClave">PalabraClave que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de PalabraClave generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, PalabraClave palabraClave)
      {
         object myFirm = new object();
         string sError = String.Empty;
         if (palabraClave == null)
            sError += ", palabraClave";
         if (sError.Length > 0)
            throw new Exception("PalabraClaveRetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.Busqueda.DAO", 
         "PalabraClaveRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try
         {
             dctx.OpenConnection(myFirm);
             sqlCmd = dctx.CreateCommand();
         }
         catch (Exception ex)
         {
             throw new StandardException(MessageType.Error, "",
                                         "PalabraClaveRetHlp: No se pudo conectar a la base de datos",
                                         "POV.ContenidosDigital.Busqueda.DAO",
                                         "PalabraClaveRetHlp", "Action", null, null);
         }
          DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT PalabraClaveID, Tag, TipoPalabraClave ");
         sCmd.Append(" FROM PalabraClave ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (palabraClave.PalabraClaveID != null){
            s_VarWHERE.Append(" PalabraClaveID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = palabraClave.PalabraClaveID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (palabraClave.Tag != null){
            s_VarWHERE.Append(" AND Tag = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = palabraClave.Tag;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (palabraClave.TipoPalabraClave != null){
            s_VarWHERE.Append(" AND TipoPalabraClave = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = palabraClave.TipoPalabraClave;
            sqlParam.DbType = DbType.Byte;
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
         try
         {
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "PalabraClave");
         } 
         catch(Exception ex)
         {
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PalabraClaveRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } 
         finally
         {
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
