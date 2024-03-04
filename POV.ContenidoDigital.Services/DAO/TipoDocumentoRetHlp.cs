using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;

namespace POV.ContenidosDigital.DAO { 
   /// <summary>
   /// Consulta un registro de TipoDocumento en la BD
   /// </summary>
   internal class TipoDocumentoRetHlp { 
      /// <summary>
      /// Consulta registros de TipoDocumento en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="tipoDocumento">TipoDocumento que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de TipoDocumento generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, TipoDocumento tipoDocumento){
         object myFirm = new object();
         string sError = String.Empty;
         if (tipoDocumento == null)
            sError += ", TipoDocumento";
         if (sError.Length > 0)
            throw new Exception("TipoDocumentoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "TipoDocumentoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TipoDocumentoRetHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "TipoDocumentoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT TipoDocumentoID,Nombre,Extension,MIME,EsEditable,Fuente,Activo,FechaRegistro,ImagenDocumento ");
         sCmd.Append(" FROM TipoDocumento ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (tipoDocumento.TipoDocumentoID != null){
            s_VarWHERE.Append(" TipoDocumentoID =@dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = tipoDocumento.TipoDocumentoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (tipoDocumento.Nombre != null){
             s_VarWHERE.Append(" AND Nombre LIKE @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = tipoDocumento.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (tipoDocumento.Extension != null){
            s_VarWHERE.Append(" AND Extension =@dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = tipoDocumento.Extension;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (tipoDocumento.Activo != null){
            s_VarWHERE.Append(" AND Activo =@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = tipoDocumento.Activo;
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
            sqlAdapter.Fill(ds, "TipoDocumento");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("TipoDocumentoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
